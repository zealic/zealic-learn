using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if !UNIT_TESTING
[assembly: AssemblyTitle(ThisAssembly.Title)]
[assembly: AssemblyVersion(ThisAssembly.Version)]
[assembly: AssemblyFileVersion(ThisAssembly.Version)]
#else
[assembly: AssemblyProduct(ThisAssembly.Title + ".Tests")]
#endif
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Zealic")]
[assembly: AssemblyCopyright("Copyright © 2013 Zealic. All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]

#if !UNIT_TESTING
[assembly: InternalsVisibleTo(ThisAssembly.Title + ".Tests")]

internal static partial class ThisAssembly
{
    public const string Version = "1.0.0.0";
}
#endif
