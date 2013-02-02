/*=========================================================
 * THIS IS ASSEMBLY FOUNDATION
 * LINK TO PROJECT AND UNIT-TESTING PROJECT
 * YOU MAY DEFINE MACRO TO CONTROL BEHAVIOR
//=======================================================*/
/**********************************************************
Version: 20130202
**********************************************************/
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable RedundantUsingDirective
// ReSharper disable UnusedParameter.Local
#if MAIN_ASSEMBLY || UNIT_TESTING
#define DECLARE_KERNEL
#endif

using System;
#if DECLARE_KERNEL
using System.Linq;
using System.Reflection;
using Ninject;
using Ninject.Components;
using Ninject.Injection;
using Ninject.Planning;
using Ninject.Planning.Directives;
using Ninject.Planning.Strategies;
using Ninject.Syntax;
using Ninject.Modules;
#endif

#if UNIT_TESTING
using Moq;
#endif


#if DECLARE_KERNEL
#if UNIT_TESTING
internal class TestKernel : StandardKernel
{
    public TestKernel(params INinjectModule[] modules)
        : base(modules)
    {
    }
#elif MAIN_ASSEMBLY
internal class AutowiringKernel : StandardKernel
{
    public AutowiringKernel(params INinjectModule[] modules)
        : base(modules)
    {
    }
#endif

    #region AutowiringStrategy
    private class AutowiringStrategy : NinjectComponent, IPlanningStrategy
    {
        public AutowiringStrategy(IInjectorFactory injectorFactory)
        {
            if (injectorFactory == null) throw new ArgumentNullException("injectorFactory");
            InjectorFactory = injectorFactory;
        }

        private IInjectorFactory InjectorFactory { get; set; }

        public void Execute(IPlan plan)
        {
            var type = plan.Type;
            foreach (var info in type.GetProperties(Flags))
            {
                if (info.GetIndexParameters().Length > 0 // Ignore indexer
                    || info.PropertyType.IsPrimitive
                    || info.PropertyType.IsEnum
                    || info.PropertyType == typeof(string)
                    || info.PropertyType.IsValueType) continue;

                if (!CanInject(type, info)) continue;
                if (info.GetSetMethod(Settings.InjectNonPublic) != null)
                {
                    plan.Add(new PropertyInjectionDirective(info, InjectorFactory.Create(info)));
                }
            }
        }

        private bool CanInject(Type type, PropertyInfo propertyInfo)
        {
            const string mefAttributeName = "System.ComponentModel.Composition.ImportAttribute";
#if UNIT_TESTING
            if(type.IsSubclassOf(typeof(TestBase))) return true;
#endif
            Func<MemberInfo, bool> attributeChecker = t => Attribute
                .GetCustomAttributes(t)
                .Any(att => att.GetType().FullName == mefAttributeName);

            return attributeChecker(propertyInfo);
        }

        private BindingFlags Flags
        {
            get
            {
                return Settings.InjectNonPublic
                    ? BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance
                    : BindingFlags.Public | BindingFlags.Instance;
            }
        }
    }

    #endregion

    protected override void AddComponents()
    {
        base.AddComponents();
        Components.Add<IPlanningStrategy, AutowiringStrategy>();
    }
}

#endif

#if UNIT_TESTING
public abstract class TestBase<T> : TestBase
    where T : class
{
    public T TestObject { get; set; }

    protected override void OnRegisterServices()
    {
        base.OnRegisterServices();
        OnBindingSelf(Container.Bind<T>().ToSelf());
    }

    protected virtual void OnBindingSelf(IBindingWhenInNamedWithOrOnSyntax<T> binding)
    {
    }
}

public abstract class TestBase : IDisposable
{
    public IKernel Container { get; protected set; }

    protected TestBase()
    {
        SetUp();
    }

    private void SetUp()
    {
        Container = new TestKernel();
        Container.Settings.InjectNonPublic = true;
        OnRegisterServices();
        Container.Inject(this);

        OnSetUp();
    }

    protected virtual void OnSetUp()
    {
    }

    void IDisposable.Dispose()
    {
        TearDown();
    }

    private void TearDown()
    {
        OnTearDown();
        Container.Dispose();
        Container = null;
    }

    protected virtual void OnTearDown()
    {
    }

    protected virtual void OnRegisterServices()
    {
    }

    protected Mock<T> RegisterMock<T>(MockBehavior behavior = MockBehavior.Default, params object[] arguments)
        where T : class
    {
        var mock = new Mock<T>(behavior, arguments);
        Container.Bind<T>().ToMethod(m => mock.Object).InSingletonScope();
        return mock;
    }
}
#endif
