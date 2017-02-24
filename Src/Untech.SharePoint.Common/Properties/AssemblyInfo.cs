using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Untech.SharePoint.Common")]
[assembly: AssemblyDescription("Library that defines common logic for data access in SharePoint with SSOM, CSOM")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Sergey Svirsky")]
[assembly: AssemblyProduct("Untech.SharePoint.Common")]
[assembly: AssemblyCopyright("Copyright \u00A9 Sergey Svirsky 2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("3049587e-e427-478a-9c18-6bd4f56635f3")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.2.0")]
[assembly: AssemblyVersion("1.0.2.0")]
[assembly: AssemblyFileVersion("1.0.2.0")]
[assembly: AssemblyInformationalVersion("1.0.2")]

#if !SIGNED
[assembly: InternalsVisibleTo("Untech.SharePoint.Common.Test")]
#endif
