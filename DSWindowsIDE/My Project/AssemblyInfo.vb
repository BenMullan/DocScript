﻿Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Resources
Imports System.Windows

' General Information about an assembly is controlled through the following 
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("DocScript (IDE)")> 
<Assembly: AssemblyDescription("DocScript Windows IDE Executable. Create, Edit, Analyse, and Debug Scripts with ease. Execute scripts localy, remotely, or compile them to standalone exe files. Built on .NET v4.0.")> 
<Assembly: AssemblyCompany("Ben Mullan")> 
<Assembly: AssemblyProduct("DSWindowsIDE")> 
<Assembly: AssemblyCopyright("DocScript (C) 2023")> 
<Assembly: AssemblyTrademark("BM-TM")> 
<Assembly: ComVisible(false)>

'In order to begin building localizable applications, set 
'<UICulture>CultureYouAreCodingWith</UICulture> in your .vbproj file
'inside a <PropertyGroup>.  For example, if you are using US english 
'in your source files, set the <UICulture> to "en-US".  Then uncomment the
'NeutralResourceLanguage attribute below.  Update the "en-US" in the line
'below to match the UICulture setting in the project file.

'<Assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)> 


'The ThemeInfo attribute describes where any theme specific and generic resource dictionaries can be found.
'1st parameter: where theme specific resource dictionaries are located
'(used if a resource is not found in the page, 
' or application resource dictionaries)

'2nd parameter: where the generic resource dictionary is located
'(used if a resource is not found in the page, 
'app, and any theme specific resource dictionaries)
<Assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)>



'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("9c168d69-4e83-4fe6-b2bd-bfc7e7c3bc5c")> 

' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version 
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers 
' by using the '*' as shown below:
' <Assembly: AssemblyVersion("1.0.*")> 

<Assembly: AssemblyVersion("6.1.0.0")> 
<Assembly: AssemblyFileVersion("6.1.0.0")> 

<Assembly: NeutralResourcesLanguageAttribute("en-GB")> 