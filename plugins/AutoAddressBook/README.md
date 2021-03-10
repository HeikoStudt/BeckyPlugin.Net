# AutoAddressBook (.NET)
This is a .NET Plugin for Becky! 2 to automatically add addresses into some Becky! address book after having sent an email to.

Tested with Becky! 2.73.0

# What does it do?
This plugin takes all addresses in To, Cc, Bcc and Rcc of a outgoing message and adds them into your default address book.
(Currently the first folder, first address book).

Upon opening this address book once, you can use those addresses for autocompletion.

# How/Where to intall the DLLs to?
Currently, you will find the compiled DLLs in [AutoAddressBook_1.0.0.zip](https://github.com/HeikoStudt/BeckyPlugin.Net/blob/addressbook-build/plugins/AutoAddressBook/AutoAddressBook_1.0.0.zip).
Please note, that I do not give any warranty on it, my recommendation is to build the DLLs yourself.

I recommend to install the plugin to the data folder with the following structure.
Afterwards, at startup Becky! will ask you whether to activate the plugin.

Then, you need to configure it to use the correct address book (Tools -> Plug-Ins Setup -> AutoAddressBook).
Then, try to write a test email to any new address and it should be added.

    B2.ini (Base Data Dir)
    PlugIns\AutoAddressBook\AutoAddressBook.ini
    PlugIns\AutoAddressBook\AutoAddressBookImpl.dll
    PlugIns\AutoAddressBook\AutoAddressBookImpl.pdb
    PlugIns\AutoAddressBook\BeckyApi.dll"
    PlugIns\AutoAddressBook\BeckyApi.pdb"
    PlugIns\AutoAddressBook\BeckyTypes.dll"
    PlugIns\AutoAddressBook\BeckyTypes.pdb"
    PlugIns\AutoAddressBook\INIFileParser.dll"
    PlugIns\AutoAddressBook\INIFileParser.xml"
    PlugIns\AutoAddressBook\MimeKitLite.dll"
    PlugIns\AutoAddressBook\MimeKitLite.xml"
    PlugIns\AutoAddressBook\nlog.config"
    PlugIns\AutoAddressBook\NLog.dll"
    PlugIns\AutoAddressBook\NLog.xml"
    PlugIns\AutoAddressBook\PInvoke.Kernel32.dll"
    PlugIns\AutoAddressBook\PInvoke.Kernel32.xml"
    PlugIns\AutoAddressBook\PInvoke.User32.dll"
    PlugIns\AutoAddressBook\PInvoke.User32.xml"
    PlugIns\AutoAddressBook\PInvoke.Windows.Core.dll"
    PlugIns\AutoAddressBook\PInvoke.Windows.Core.xml"
    PlugIns\AutoAddressBook\Utilities.dll"
    PlugIns\AutoAddressBook\Utilities.pdb"
    PlugIns\AutoAddressBook\Validation.dll"
    PlugIns\AutoAddressBook\Validation.xml"

    PlugIns\AutoAddressBook.dll
    PlugIns\AutoAddressBook.dll.config
    PlugIns\AutoAddressBook.pdb


# Known caveats
I need to manually update the address book files (append a vcard). This may interact with open address book windows.
Apparently, this seems to be quite unproblematic (append data to the file while running Becky Addressbook and adding addresses there as well), 
though this might not be for sure.

The addresses are only known to your Becky! autocompletion list after opening the default group once in the address book manager.


# Open ToDos
 * Review possibilities to embed WPF into the Win32 API of Becky! 2.
   (https://msdn.microsoft.com/en-us/library/ms742522.aspx)
   Currently the dialog box is a win32 form. Probably, I will stay with it.
 * Ask Carty for an address book API.
   => Said, I should write into address book and remove group.idx

# Build Tools
 * Visual Studio 2015 (having Resharper) and Visual Studio 2017.
 * Nuget Package Manager
   * NLog
   * DllExport (GitHub)
   * ModuleInit.Fody
   * MimeKit (Github, parsing mail messages)
   * ini-parser

 * I got it working in Visual Studio 2019 Community as well. 
   However, the compiling seems not 100% stable, probably because of some timing issue with DllExport at initial build. The resulting DLLs are stable, though.

# The structure of the plugin project
  * There are two projects: 
    1. AutoAddressBook for exporting the DLLs and loading the assemblies.
    2. AutoAddressBookImpl for the implementation, configuration form and debug symbols

# License
This plugin is postcardware. Please send a real postcard to me (HeikoStudt). :-)


# Acknowledges
Becky! 2 is a software by [Rimarts, Inc.](http://rimarts.co.jp)

The C# .NET SDK [BeckyPlugin.Net](https://github.com/HeikoStudt/BeckyPlugin.Net)

The exported methods are using [DllExport](https://github.com/3F/DllExport) which is quite harder (but more advanced).

The module initializing is done via [ModuleInit.Fody](https://www.nuget.org/packages/ModuleInit.Fody/).

The mail message parsing is done via [MimeKit](https://github.com/jstedfast/MimeKit).

The ini-file handling is done via [ini-parser](https://github.com/rickyah/ini-parser).
