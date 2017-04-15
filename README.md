# BeckyPlugin.Net
.NET Plugin SDK for Becky! 2.

# What does it do?
The SDK provides all callbacks (but those for the icons as discussed later) into the generated DLL, abstracting most of the low-level stuff.
It is calling the plugin methods in your own BeckyPlugin.cs through the interface IBeckyPlugin.

Additionally, it provides you with all the DllImport/PInvoke of the Becky! 2 API in B2.EXE. You have got a nice .NET API using C# delegates, the C# Garbage Collector while reducing the handling with pointers.

It gives you the opportunity of referencing arbitrary libraries as it provides you with an assembly loader able to gather the referenced .NET assemblies out of plugins/(assemblyname).

# Existing plugins
 * [AutoAddressBook] (https://github.com/HeikoStudt/BeckyPlugin.Net/plugins/AutoAddressBook/)

# How to Use (old)
First, you will have to provide your own plugin information in BeckyPlugin/Properties/AssemblyInfo.cs.
Here, the AssemblyTitle is mapped onto the plugin name and the AssemblyCompany is the "vendor" of the plugin.
AssemblyDescription and AssemblyVersion are mapped as well.

Second, you have to rename the assembly library name (dll name) in Project Properties.

Third, you will have to add nlog.dll and nlog.config into the folder of b2.exe (not the plugin one).

Fourth, you will have to add your code in BeckyPlugin.cs and call into an object of type CallsIntoBecky.

IF you want to seperate your plugin from changes to the SDK, you should

For easier developement, add an Visual Studio "Tools:External Tool" to some copy.bat with "Use Output Window" and some shortcut.
=> copy-plugins-and-start.bat

# How to create a new Plugin (new)
0. Create and configure a nlog.config in directory of B2.exe (if not yet existing).

1. Add a new plugin assembly project into /plugins folder having the same output assembly name as its folder name. (Sample: plugins/AutoAddressBook with AutoAddressBook.dll)

Note: The plugin assembly name should not be a prefix of some other plugin if you like the automatisms of copy-and-start.ps1.

2. Provide your own plugin information in BeckyPlugin/Properties/AssemblyInfo.cs.

Here, the AssemblyTitle is mapped onto the plugin name and the AssemblyCompany is the "vendor" of the plugin.
AssemblyDescription and AssemblyVersion are mapped as well.

3. Install Nuget Packages (DllExports, NLog, ModuleInit.Fody) into project. 

DllExports need to be set to x86 and "System.Runtime.Interop". If you have got problems, uninstall and restart Visual Studio.

Note: You could be able to use UnmanagedExports instead as well, but it messes a bit with ModuleInit.Fody.

4. Add BeckyTypes, BeckyApi as project references. Do **not** add "BeckyPlugin" or any other plugin.

5. Add System.Windows.Forms as assembly reference.

6. Change processor architecture to x86 (aka 32 bit).

7. Copy over ModuleInitializer, BeckyApiEventListener and BeckyPlugin.cs

Change the namespace inside BeckyApiEventListener and BeckyPlugin accordingly.

8. Alter BeckyPlugin.cs as your business logic impedes.

Use _callsIntoBecky as an object for API calls into Becky!.

9. If neccessary, add the icon exports into BeckyApiEventListener and find out, how to use it. If so, please create a pull request for your bugfixes. :-)

10. modify and start copy-and-start.ps1; it will automatically deploy all plugins into becky and start b2.exe.

11. Create a Readme.md, put it on GitHub and send me a nice postcard. :-)


# Open ToDos
 * Creating a Nuget-Package of this SDK, so that each plugin may get the templates via t1?
 * Test all the methods and their mappings/marshalling. (around 40% done)
 * Review possibilities to embed WPF into the Win32 API of Becky! 2.
   (https://msdn.microsoft.com/en-us/library/ms742522.aspx)
 * Ask Carty to
   * implement some meaning to say "I do NOT implement BKC_OnRequestResource" while exporting, as well as BKC_OnRequestResource2
   * tell me my error on "X-Becky-Attachment" in ComposeMail
     => got an answer, write here...
   * Nlog.config still needs to be placed in b2.exe folder.
     All dlls may be in plugins/name/ folder but the main entry point dll.

# Build Tools
 * Visual Studio 2015 (having Resharper).
 * Nuget Package Manager
   * NLog
   * UnmanagedExports (RGiesecke.DllExport (could be a mess with Fody)) or DllExport (GitHub)
   * ModuleInit.Fody
   * PInvoke.User32

# The structure of the solution?

  * BeckyTypes consists of types used for BeckyAPI and the plugin exporters
  * BeckyPluginTemplate is a template to copy files from and for the time being to test the API manually.
  * BeckyApi is the callable API of B2.EXE
  * AutoAddressBook is my first 'real' plugin, still in raw shape :)


# License
This SDK is postcardware. Please send a real postcard to me (HeikoStudt). :-)

It would be very nice to open source your plugins as they might be swallowed by time otherwise.


# Acknowledges
Becky! 2 is a software by Rimarts, Inc. http://rimarts.co.jp

This C# .NET SDK is a transcription of the official SDK (originally using version 2.64.0).

The exported methods are using [RGiesecke.DllExport] (https://sites.google.com/site/robertgiesecke/Home/uploads/unmanagedexports) via its [Nuget package] (https://www.nuget.org/packages/UnmanagedExports/1.2.2.23707).

Another possibility is a fork of above [DllExport] (https://github.com/3F/DllExport) which is quite harder (but more advanced).

The module initializing is done via [ModuleInit.Fody] (https://www.nuget.org/packages/ModuleInit.Fody/).

For putting up some window and menu stuff, you can use the [Nuget Package of PInvoke] (https://github.com/AArnott/pinvoke). While most of Win32 APIs are still missing in the package, you can add those [Win32 APIs] (http://www.pinvoke.net) yourself.
