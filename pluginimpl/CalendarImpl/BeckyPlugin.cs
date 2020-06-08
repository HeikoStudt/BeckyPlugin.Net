using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BeckyApi;
using BeckyTypes.ExportEnums;
using NLog;
using Utilities;
using BeckyTypes.PluginListener;
using System.Windows.Interop;
using System.Windows;

namespace CalendarImpl
{
    public class BeckyPlugin : AbstractBeckyPlugin
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public BeckyPlugin(string pluginName) : base(pluginName) {
            //OnMainMenuInit += OnMainMenuInitImpl;
        }

        /*
        private string DefaultAddressBook
        {
            get {
                var result = PluginConfiguration.Read("DefaultAddressBook", "Settings");
                if (string.IsNullOrWhiteSpace(result)) {
                    result = "@Personal";
                }
                return result;
            }
            set {
                PluginConfiguration.Write("DefaultAddressBook", value, "Settings");
            }
        }*/
        
        public override bool OnPlugInSetup(IntPtr hWnd) {
            var control = new TestUserControl();
            Window window = new Window() {
                Title = "Test",
                Content = control,
                SizeToContent = SizeToContent.WidthAndHeight,
                //ResizeMode = ResizeMode.NoResize
            };
            WindowInteropHelper wih = new WindowInteropHelper(window);
            wih.Owner = hWnd;
            if (true == window.ShowDialog()) {
                //DefaultAddressBook = form.ChosenAddressBook;
                //DefaultGroupPath = form.ChosenGroupPath;
            }
            return true;
        }

        /*
        public void OnMainMenuInitImpl(IntPtr hWnd, IntPtr hMenu) {
                Logger.Info("OnMainMenuInit");
                //TODO create and use a standard menu win32 wrapper
                //var menu = MenuUtils.GetStandardMenu(hMenu, "&Tools");

                IntPtr hToolsMenu = Menus.GetSubMenu(hMenu, 4); //Tools

                var nId = CallsIntoBecky.RegisterCommand(
                    "Configure AutoAddressBook", 
                    BeckyApi.Enums.BeckyMenu.BKC_MENU_MAIN, 
                    CmdOpenConfiguration);

                Menus.AppendMenu(hToolsMenu, Menus.MenuFlags.MF_SEPARATOR, 0, null);
                Menus.AppendMenu(hToolsMenu, Menus.MenuFlags.MF_STRING, nId, "Configure AutoAddressBook");
        }

        public void CmdOpenConfiguration(IntPtr hWnd, short menuCommandId, short futureUse) {
            //OpenConfigurationDialog(hWnd);
        }*/
    }
}
