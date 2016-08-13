# BeckyPlugin.Net
.NET Plugin SDK for Becky! 2.

# What does it do?
The SDK provides all callbacks (but those for icons) into the generated DLL, abstracting most of the low-level stuff.
It is calling the plugin methods in BeckyPlugin.cs through the interface IBeckyPlugin.

Secondly, it adds all the DllImport/PInvoke of the Becky! 2 API in B2.EXE and higher up the layer using C# delegates, C# Garbage Collector while reducing the pointers.


# How to Use
First, you will have to provide your own plugin information in OnPlugInInfo.

Second, you will have to add your code in BeckyPlugin.cs and Call into an object of type CallsIntoBecky.

Using NLog for the logging, you have to place the nlog.config in the same folder as B2.EXE currently, not into the plugin folder.

For developement, add an Visual Studio "Tools:External Tool" to some copy.bat with "Use Output Window" and some shortcut.

# Open ToDos
 * Adding some kind of possibility for seperating this SDK from the real plugin.
 * reduce/move the current test code into nicer test classes.
 * Test all the methods and their mappings/marshalling. (around 40% done)
 * Review possibilities to embed WPF into the Win32 API of Becky! 2.
 * If listener is disabled (because of exceptions), rethink what we should answer on OnPlugInInfo
 * Ask Carty to
   * implement some meaning to say "I do NOT implement BKC_OnRequestResource" while exporting, as well as BKC_OnRequestResource2
   * describe why SetStatus get some negative value on some occasions (IMAP)
   * describe all enum values of BeckyMessage, even if they are internal
   * tell me my error on "X-Becky-Attachment" in ComposeMail

# Build Tools
 * Visual Studio 2015 (having Resharper).
 * Nuget Package Manager
   * NLog
   * UnmanagedExports (RGiesecke.DllExport)
   * PInvoke.User32


# Acknowledges
Becky! 2 is a software by Rimarts, Inc. http://rimarts.co.jp

This C# .NET SDK is a transcription of the official SDK (originally using version 2.64.0).

The exported methods are using [RGiesecke.DllExport] (https://sites.google.com/site/robertgiesecke/Home/uploads/unmanagedexports) via its [Nuget package] (https://www.nuget.org/packages/UnmanagedExports/1.2.2.23707).

For putting up some window and menu stuff, you can use the [Nuget Package of PInvoke] (https://github.com/AArnott/pinvoke). While most of Win32 APIs are still missing in the package, you can add those [Win32 APIs] (http://www.pinvoke.net) yourself.
