using System;
// ReSharper disable InconsistentNaming

namespace BeckyApi
{
    [Flags]
    public enum BeckyCmdUI : uint {
        INVALID = 0,
        /// <summary>
        ///   menu item will be grayed out and the command will never been called.
        /// </summary>
        BKMENU_CMDUI_DISABLED = 1,
        /// <summary>
        /// menu item will be checked.
        /// </summary>
        BKMENU_CMDUI_CHECKED = 2,
    }
}