# AutoAddressBook (.NET)
This is a .NET Plugin for Becky! 2 to automatically add addresses into some Becky! address book after having sent an email to.

Tested with Becky! 2.73.0

# What does it do?
This plugin takes all addresses in To, Cc, Bcc and Rcc of a outgoing message and adds them into your default address book.
(Currently the first folder, first address book).

Upon opening this address book once, you can use those addresses for autocompletion.

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
