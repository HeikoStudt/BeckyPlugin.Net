# AutoAddressBook (.NET)
This is a .NET Plugin for Becky! 2 to automatically add addresses into the Becky! address book.

Tested with Becky! 2.73.0

# What does it do?
This plugin takes all addresses in To, Cc, Bcc and Rcc of a outgoing message and adds them into your default address book.
(Currently the first folder, first address book).

Upon opening this address book once, you can use those addresses for autocompletion.

# Known caveats
Firstly, your addresses are not stored into the address book yet. ;-)

Secondly, I need to manually update the address book files (append vcards). This may interact with open address book windows.
Apparently, this seems to be quite unproblematic (append data to the file while running Becky Addressbook and adding addresses), it might not be for sure.

Thirdly, the addresses are only known to your Becky! autocompletion list after opening the default group once in the address book manager.


# Open ToDos
 * Find the correct (default) address book
 * Add VCards into the address book
 * Review possibilities to embed WPF into the Win32 API of Becky! 2.
   (https://msdn.microsoft.com/en-us/library/ms742522.aspx)
 * Adding configuration for default address book / default group.
 * Ask Carty for an address book API.

# Build Tools
 * Visual Studio 2015 (having Resharper).
 * Nuget Package Manager
   * NLog
   * DllExport (GitHub)
   * ModuleInit.Fody
   * MimeKit (Github, parsing mail messages)

# The structure of the plugin project
  * For the moment, it all resides in BeckyPlugin.cs


# License
This plugin is postcardware. Please send a real postcard to me (HeikoStudt). :-)


# Acknowledges
Becky! 2 is a software by Rimarts, Inc. http://rimarts.co.jp

The C# .NET SDK [BeckyPlugin.Net] (https://github.com/HeikoStudt/BeckyPlugin.Net)

The exported methods are using [DllExport] (https://github.com/3F/DllExport) which is quite harder (but more advanced).

The module initializing is done via [ModuleInit.Fody] (https://www.nuget.org/packages/ModuleInit.Fody/).

The mail message parsing is done via [MimeKit] (https://github.com/jstedfast/MimeKit).
