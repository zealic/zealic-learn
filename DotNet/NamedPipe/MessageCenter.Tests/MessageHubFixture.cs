using System;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Threading.Tasks;
using Ninject;
using Xunit;

namespace MessageCenter
{
    public class MessageHubFixture : TestBase
    {
        public class TestMessage : MessageBase
        {
            public int Id { get; set; }
            public string Content { get; set; }
        }

        private const string PipeName = "MessageHubFixture";

        internal MessageHub TestObject { get; set; }

        public NamedPipeClientStream Client { get; set; }

        public BinaryReader Reader { get; set; }

        public BinaryWriter Writer { get; set; }

        protected override void OnRegisterServices()
        {
            base.OnRegisterServices();
            Container.Bind<MessageHub>().ToMethod(c => new MessageHub(PipeName));
            Container.Bind<NamedPipeClientStream>()
                .ToConstant(new NamedPipeClientStream(".", PipeName, PipeDirection.InOut));
            Container.Bind<BinaryReader>()
                .ToMethod(c => new BinaryReader(c.Kernel.Get<NamedPipeClientStream>()));
            Container.Bind<BinaryWriter>()
                .ToMethod(c => new BinaryWriter(c.Kernel.Get<NamedPipeClientStream>()));
        }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            TestObject.RegisterAssemblyMessages(Assembly.GetExecutingAssembly());
        }

        [Fact]
        public void TestSend()
        {
            const string msgBody = @"{""Content"":""Hello World!"",""Id"":4321}";

            TestObject.Send(new TestMessage { Id = 4321, Content = "Hello World!" });

            Client.Connect();
            var name = Reader.ReadString();
            var actualMsgBody = Reader.ReadString();
            Assert.Equal("TestMessage", name);
            Assert.Equal(msgBody, actualMsgBody);
        }

        [Fact]
        public void TestReceive()
        {
            const string msgBody = @"{""Content"":""Hello World!"",""Id"":4321}";

            Task.Run(() =>
            {
                Client.Connect();
                Writer.Write("TestMessage");
                Writer.Write(msgBody);
            });
            var message = TestObject.Receive();
            Assert.Equal(message.GetType(), typeof(TestMessage));
            var actualMessage = (TestMessage)message;

            Assert.Equal(4321, actualMessage.Id);
            Assert.Equal("Hello World!", actualMessage.Content);
        }

    }
}
