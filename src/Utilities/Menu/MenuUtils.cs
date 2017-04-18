using System;
using System.Runtime.InteropServices;
//using NLog;
//using PInvoke;

namespace AutoAddressBookImpl {
    //using static Win32Menu;

    public class MenuUtils {
        /*
        public static int GetStandardMenuIndex(IntPtr hParentMenu, string name) {
            MENUITEMINFO mii = MENUITEMINFO.Create();
            for (int i = 0; i < GetMenuItemCount(hParentMenu); i++) {
                mii.dwTypeData = IntPtr.Zero;
                mii.fMask = User32.MenuMembersMask.MIIM_STRING;
                mii.fType = User32.MenuItemType.MFT_STRING;
                if (GetMenuItemInfo(hParentMenu, (uint)i, true, ref mii)) {
                    var oldMiiCch = mii.cch;
                    if (mii.fType == User32.MenuItemType.MFT_STRING) {
                        mii.cch = mii.cch + 1;
                        IntPtr buffer = Marshal.AllocHGlobal(mii.cch * 2);
                        try {
                            mii.dwTypeData = buffer;
                            if (GetMenuItemInfo(hParentMenu, (uint)i, true, ref mii)) {

                                if (mii.cch <= oldMiiCch) {
                                    //throw
                                }
                                string caption = Marshal.PtrToStringUni(mii.dwTypeData);
                                //LogManager.GetCurrentClassLogger().Info(caption);

                                //TODO use the name of the control if there is any
                                if (caption == name) {
                                    return i;
                                }
                            }
                        } finally {
                            Marshal.FreeHGlobal(buffer);
                        }
                    }
                }
            }
            return -1;
        }*/
    }
}