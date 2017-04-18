using System;
using System.Runtime.InteropServices;
using PInvoke;

namespace AutoAddressBookImpl {
    /// <summary>Contains information about a menu item.</summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct MENUITEMINFO {
        /// <summary>The size of the structure, in bytes.</summary>
        public int cbSize;
        /// <summary>Indicates the members to be retrieved or set.</summary>
        public User32.MenuMembersMask fMask;
        /// <summary>The menu item type.</summary>
        public User32.MenuItemType fType;
        /// <summary>The menu item state.</summary>
        public User32.MenuItemState fState;
        /// <summary>
        ///     An application-defined value that identifies the menu item. Set <see cref="F:PInvoke.User32.MENUITEMINFO.fMask" /> to
        ///     <see cref="F:PInvoke.User32.MenuMembersMask.MIIM_ID" /> to use <see cref="F:PInvoke.User32.MENUITEMINFO.wID" />.
        /// </summary>
        public int wID;
        /// <summary>
        ///     A handle to the drop-down menu or submenu associated with the menu item. If the menu item is not an item that
        ///     opens a drop-down menu or submenu, this member is <see cref="F:System.IntPtr.Zero" />. Set <see cref="F:PInvoke.User32.MENUITEMINFO.fMask" /> to
        ///     <see cref="F:PInvoke.User32.MenuMembersMask.MIIM_SUBMENU" /> to use hSubMenu.
        /// </summary>
        public IntPtr hSubMenu;
        /// <summary>
        ///     A handle to the bitmap to display next to the item if it is selected. If this member is
        ///     <see cref="F:System.IntPtr.Zero" />, a default bitmap is used. If the <see cref="F:PInvoke.User32.MenuItemType.MFT_RADIOCHECK" /> type value
        ///     is specified, the default bitmap is a bullet. Otherwise, it is a check mark. Set fMask to
        ///     <see cref="F:PInvoke.User32.MenuMembersMask.MIIM_CHECKMARKS" /> to use <see cref="F:PInvoke.User32.MENUITEMINFO.hbmpChecked" />.
        /// </summary>
        public IntPtr hbmpChecked;
        /// <summary>
        ///     A handle to the bitmap to display next to the item if it is not selected. If this member is
        ///     <see cref="F:System.IntPtr.Zero" />, no bitmap is used. Set <see cref="F:PInvoke.User32.MENUITEMINFO.fMask" /> to
        ///     <see cref="F:PInvoke.User32.MenuMembersMask.MIIM_CHECKMARKS" /> to use <see cref="F:PInvoke.User32.MENUITEMINFO.hbmpUnchecked" />.
        /// </summary>
        public IntPtr hbmpUnchecked;
        /// <summary>
        ///     An application-defined value associated with the menu item. Set <see cref="F:PInvoke.User32.MENUITEMINFO.fMask" /> to
        ///     <see cref="F:PInvoke.User32.MenuMembersMask.MIIM_DATA" /> to use <see cref="F:PInvoke.User32.MENUITEMINFO.dwItemData" />.
        /// </summary>
        public IntPtr dwItemData;
        /// <summary>
        ///     The contents of the menu item. The meaning of this member depends on the value of fType and is used only if the
        ///     MIIM_TYPE flag is set in the fMask member.
        ///     <para>
        ///         To retrieve a menu item of type <see cref="F:PInvoke.User32.MenuItemType.MFT_STRING" />, first find the size of the string by
        ///         setting the <see cref="F:PInvoke.User32.MENUITEMINFO.dwTypeData" />
        ///         member of <see cref="T:PInvoke.User32.MENUITEMINFO" /> to <see cref="F:System.IntPtr.Zero" /> and then calling
        ///         <see cref="M:PInvoke.User32.GetMenuItemInfo(System.IntPtr,System.UInt32,System.Boolean,PInvoke.User32.MENUITEMINFO@)" />. The value of <see cref="F:PInvoke.User32.MENUITEMINFO.cch" />+1 is the size needed. Then allocate a buffer of
        ///         this size, place the pointer to the buffer in dwTypeData, increment cch, and call
        ///         <see cref="M:PInvoke.User32.GetMenuItemInfo(System.IntPtr,System.UInt32,System.Boolean,PInvoke.User32.MENUITEMINFO@)" /> once again to fill the buffer with the string. If the retrieved menu item is of
        ///         some other type, then <see cref="M:PInvoke.User32.GetMenuItemInfo(System.IntPtr,System.UInt32,System.Boolean,PInvoke.User32.MENUITEMINFO@)" /> sets the <see cref="F:PInvoke.User32.MENUITEMINFO.dwTypeData" /> member to a value whose
        ///         type is specified by the <see cref="F:PInvoke.User32.MENUITEMINFO.fType" /> member.
        ///     </para>
        ///     <para>
        ///         When using with the <see cref="M:PInvoke.User32.SetMenuItemInfo(System.IntPtr,System.UInt32,System.Boolean,PInvoke.User32.MENUITEMINFO@)" /> function, this member should contain a value whose type is
        ///         specified by the <see cref="F:PInvoke.User32.MENUITEMINFO.fType" /> member.
        ///     </para>
        ///     <para>
        ///         dwTypeData is used only if the <see cref="F:PInvoke.User32.MenuMembersMask.MIIM_STRING" /> flag is set in the
        ///         <see cref="F:PInvoke.User32.MENUITEMINFO.fMask" /> member
        ///     </para>
        /// </summary>
        public IntPtr dwTypeData;
        /// <summary>
        ///     The length of the menu item text, in characters, when information is received about a menu item of the
        ///     <see cref="F:PInvoke.User32.MenuItemType.MFT_STRING" />
        ///     type. However, <see cref="F:PInvoke.User32.MENUITEMINFO.cch" /> is used only if the <see cref="F:PInvoke.User32.MenuMembersMask.MIIM_TYPE" /> flag is set in the
        ///     <see cref="F:PInvoke.User32.MENUITEMINFO.fMask" /> member and is zero otherwise. Also, <see cref="F:PInvoke.User32.MENUITEMINFO.cch" />
        ///     is ignored when the content of a menu item is set by calling <see cref="M:PInvoke.User32.SetMenuItemInfo(System.IntPtr,System.UInt32,System.Boolean,PInvoke.User32.MENUITEMINFO@)" />.
        ///     <para>
        ///         Note that, before calling <see cref="M:PInvoke.User32.GetMenuItemInfo(System.IntPtr,System.UInt32,System.Boolean,PInvoke.User32.MENUITEMINFO@)" />, the application must set <see cref="F:PInvoke.User32.MENUITEMINFO.cch" /> to the
        ///         length of the buffer pointed to by the <see cref="F:PInvoke.User32.MENUITEMINFO.dwTypeData" /> member. If the retrieved menu item is of type
        ///         <see cref="F:PInvoke.User32.MenuItemType.MFT_STRING" /> (as indicated by the <see cref="F:PInvoke.User32.MENUITEMINFO.fType" />
        ///         member), then <see cref="M:PInvoke.User32.GetMenuItemInfo(System.IntPtr,System.UInt32,System.Boolean,PInvoke.User32.MENUITEMINFO@)" /> changes <see cref="F:PInvoke.User32.MENUITEMINFO.cch" /> to the length of the menu item text. If
        ///         the retrieved menu item is of some other type, <see cref="M:PInvoke.User32.GetMenuItemInfo(System.IntPtr,System.UInt32,System.Boolean,PInvoke.User32.MENUITEMINFO@)" /> sets the <see cref="F:PInvoke.User32.MENUITEMINFO.cch" /> field
        ///         to zero.
        ///     </para>
        ///     <para>
        ///         The <see cref="F:PInvoke.User32.MENUITEMINFO.cch" /> member is used when the <see cref="F:PInvoke.User32.MenuMembersMask.MIIM_STRING" /> flag is set in the
        ///         <see cref="F:PInvoke.User32.MENUITEMINFO.fMask" /> member.
        ///     </para>
        /// </summary>
        public int cch;
        /// <summary>
        ///     A handle to the bitmap to be displayed, or it can be one of the following values :
        ///     <para>
        ///         <see cref="F:PInvoke.User32.HBMMENU_CALLBACK" />
        ///     </para>
        ///     <para>
        ///         <see cref="F:PInvoke.User32.HBMMENU_MBAR_CLOSE" />
        ///     </para>
        ///     <para>
        ///         <see cref="F:PInvoke.User32.HBMMENU_MBAR_CLOSE_D" />
        ///     </para>
        ///     <para>
        ///         <see cref="F:PInvoke.User32.HBMMENU_MBAR_MINIMIZE" />
        ///     </para>
        ///     <para>
        ///         <see cref="F:PInvoke.User32.HBMMENU_MBAR_MINIMIZE_D" />
        ///     </para>
        ///     <para>
        ///         <see cref="F:PInvoke.User32.HBMMENU_MBAR_RESTORE" />
        ///     </para>
        ///     <para>
        ///         <see cref="F:PInvoke.User32.HBMMENU_POPUP_CLOSE" />
        ///     </para>
        ///     <para>
        ///         <see cref="F:PInvoke.User32.HBMMENU_POPUP_MAXIMIZE" />
        ///     </para>
        ///     <para>
        ///         <see cref="F:PInvoke.User32.HBMMENU_POPUP_MINIMIZE" />
        ///     </para>
        ///     <para>
        ///         <see cref="F:PInvoke.User32.HBMMENU_POPUP_RESTORE" />
        ///     </para>
        ///     <para>
        ///         <see cref="F:PInvoke.User32.HBMMENU_SYSTEM" />
        ///     </para>
        /// </summary>
        public IntPtr hbmpItem;

        /// <summary>
        /// Create a new instance of <see cref="T:PInvoke.User32.MENUITEMINFO" /> with <see cref="F:PInvoke.User32.MENUITEMINFO.cbSize" /> set to the correct value.
        /// </summary>
        /// <returns>A new instance of <see cref="T:PInvoke.User32.MENUITEMINFO" /> with <see cref="F:PInvoke.User32.MENUITEMINFO.cbSize" /> set to the correct value.</returns>
        public static MENUITEMINFO Create() {
            return new MENUITEMINFO() {
                cbSize = Marshal.SizeOf(typeof(MENUITEMINFO))
            };
        }
    }
}