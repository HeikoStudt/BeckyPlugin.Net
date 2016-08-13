using System;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace BeckyPluginTest.WinApi
{
    public class Menus
    {

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        public static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, MenuFlags wFlags, Int32 wIdNewItem, string lpNewItem);

        [Flags]
        public enum MenuFlags : uint {
            MF_STRING = 0,

            /// <summary>
            ///  Indicates that the uPosition parameter gives the zero-based relative position of the menu item. 
            /// </summary>
            MF_BYPOSITION = 0x400,
            MF_SEPARATOR = 0x800,
            MF_REMOVE = 0x1000,
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool AppendMenu(IntPtr hMenu, MenuFlags uFlags, uint uIDNewItem, string lpNewItem);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSubMenu(IntPtr hMenu, int nPos);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool InsertMenuItem(IntPtr hMenu, uint uItem, bool fByPosition, [In] ref tagMENUITEMINFO lpmii);

        [StructLayout(LayoutKind.Sequential)]
        public struct tagMENUITEMINFO {
            public uint cbSize;
            public uint fMask;
            public uint fType;
            public uint fState;
            public uint wID;
            public IntPtr hSubMenu;
            public IntPtr hbmpChecked;
            public IntPtr hbmpUnchecked;
            public UIntPtr dwItemData;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string dwTypeData;
            public uint cch;
            public IntPtr hbmpItem;

            // return the size of the structure
            public static uint sizeOf => (uint)Marshal.SizeOf(typeof(tagMENUITEMINFO));
        }
    }
}