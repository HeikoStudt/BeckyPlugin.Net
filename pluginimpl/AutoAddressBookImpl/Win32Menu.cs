using System;
using System.Runtime.InteropServices;

namespace AutoAddressBookImpl {
    public static class Win32Menu {
        [DllImport("User32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMenuItemInfo(IntPtr hMenu, uint uItem, [MarshalAs(UnmanagedType.Bool)] bool fByPosition, ref MENUITEMINFO lpmii);

        /// <summary>
        ///   Determines the number of items in the specified menu. 
        /// </summary>
        /// <param name="hMenu">
        ///   Type: HMENU
        ///   A handle to the menu to be examined.
        /// </param>
        /// <returns>
        ///   Type: int
        ///   If the function succeeds, the return value specifies the number of items in the menu.
        ///   If the function fails, the return value is -1. To get extended error information, call GetLastError. 
        /// </returns>
        [DllImport("User32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetMenuItemCount(IntPtr hMenu);
    }
}