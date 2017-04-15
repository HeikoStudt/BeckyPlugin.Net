using System;
using System.Runtime.InteropServices;
using BeckyTypes.ExportEnums;
using BeckyTypes.Helpers;
using BeckyTypes.PluginListener;
using NLog;
using RGiesecke.DllExport;

namespace BeckyPlugin {
    /// <summary>
    ///   Includes all the handlers for Becky API.
    ///   Currently it instantiates statically the BeckyPlugin class.
    ///   Though, this has the consequence, that you cannot inject yourself,
    ///   I do not see a different choice for the moment, as the static
    ///   methods are used (as an initializer).
    /// </summary>
    /// <remarks>
    ///   I did not implement BKC_OnRequestResource and BKC_OnRequestResource2
    ///   as only one plugin should implement at the same time. I would need to
    ///   have a non-null return value for ignoring me.
    /// 
    ///   Important: BeckyApiEventListener must not use any generics as the DllExport
    ///     will not work.
    /// 
    ///   Important: BeckyApiEventListener is the main entry point.
    /// </remarks>
    public class BeckyApiEventListener {

        private static readonly ILogger Logger;
        private static readonly IBeckyPlugin Listener;

        static BeckyApiEventListener() {
            Logger = LogManager.GetCurrentClassLogger();
            Listener = new BeckyPlugin();
        }

        [DllExport("BKC_OnStart", CallingConvention.Winapi)]
        public static int BKC_OnStart() {
            if (Listener.IsDisabled) return 0;

            try {
                Logger.Info("BKC_OnStart");
                Listener.OnStart();
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
            }
            return 0; // Always return 0.
        }

        [DllExport("BKC_OnExit", CallingConvention.Winapi)]
        public static int BKC_OnExit() {
            if (Listener.IsDisabled) return 0;
            try {
                Logger.Info("BKC_OnExit");
                bool allowed = Listener.OnExit();
                return allowed ? 0 : -1; // Return -1 if you don't want to quit.
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
                return 0;
            }
        }

        [DllExport("BKC_OnMenuInit", CallingConvention.Winapi)]
        public static int BKC_OnMenuInit(IntPtr hWnd, IntPtr hMenu, BeckyMenu nType) {
            if (Listener.IsDisabled) return 0;

            try {
                Logger.Info("BKC_OnMenuInit for {0}", nType);
                Listener.OnMenuInit(hWnd, hMenu, nType);
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
            }
            return 0; // Always return 0.
        }

        [DllExport("BKC_OnOpenFolder", CallingConvention.Winapi)]
        public static int BKC_OnOpenFolder([MarshalAs(UnmanagedType.LPStr)] string lpFolderId) {
            if (Listener.IsDisabled) return 0;

            try {
                Logger.Info("BKC_OnOpenFolder for {0}", lpFolderId);
                Listener.OnOpenFolder(lpFolderId);
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
            }
            return 0; // Always return 0.
        }

        [DllExport("BKC_OnOpenMail", CallingConvention.Winapi)]
        public static int BKC_OnOpenMail([MarshalAs(UnmanagedType.LPStr)] string lpMailId) {
            if (Listener.IsDisabled) return 0;

            try {
                Logger.Info("BKC_OnOpenMail for {0}", lpMailId);
                Listener.OnOpenMail(lpMailId);
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
            }
            return 0; // Always return 0.
        }

        [DllExport("BKC_OnEveryMinute", CallingConvention.Winapi)]
        public static int BKC_OnEveryMinute() {
            if (Listener.IsDisabled) return 0;

            try {
                Logger.Info("BKC_OnEveryMinute");
                Listener.OnEveryMinute();
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
            }
            return 0; // Always return 0.
        }

        [DllExport("BKC_OnOpenCompose", CallingConvention.Winapi)]
        public static int BKC_OnOpenCompose(IntPtr hWnd, BeckyComposeMode nMode) {
            if (Listener.IsDisabled) return 0;

            try {
                Logger.Info("BKC_OnOpenCompose for {0}", nMode);
                Listener.OnOpenCompose(hWnd, nMode);
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
            }
            return 0; // Always return 0.
        }

        [DllExport("BKC_OnOutgoing", CallingConvention.Winapi)]
        public static int BKC_OnOutgoing(IntPtr hWnd, BeckyOutgoingMode nMode) {
            if (Listener.IsDisabled) return 0;

            try {
                Logger.Info("BKC_OnOutgoing for {0}", nMode);
                bool allowed = Listener.OnOutgoing(hWnd, nMode);
                return allowed ? 0 : -1; // Return -1 if you want to cancel the operation.
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
                return 0;
            }
        }

        [DllExport("BKC_OnKeyDispatch", CallingConvention.Winapi)]
        public static int BKC_OnKeyDispatch(IntPtr hWnd, System.Windows.Forms.Keys nKey, BeckyShiftMode nShift) {
            if (Listener.IsDisabled) return 0;

            try {
                Logger.Info("BKC_OnKeyDispatch for ({2}: {0}/{1})", nKey, nShift, (int)nKey);
                bool suppress = Listener.OnKeyDispatch(hWnd, nKey, nShift);

                // Return TRUE if you want to suppress subsequent command associated to this key.
                return suppress ? 1 : 0; // TRUE is 1 for us and for C
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
                return 0;
            }
        }



        [DllExport("BKC_OnRetrieve", CallingConvention.Winapi)]
        public static int BKC_OnRetrieve([MarshalAs(UnmanagedType.LPStr)] string lpMessage, [MarshalAs(UnmanagedType.LPStr)] string lpMailId) {
            if (Listener.IsDisabled) return 0;

            try {
                Logger.Info("BKC_OnRetrieve for ({0}/{1})", lpMessage, lpMailId);
                Listener.OnRetrieve(lpMessage, lpMailId);
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
            }
            return 0; // Always return 0.
        }

        [DllExport("BKC_OnSend", CallingConvention.Winapi)]
        public static int BKC_OnSend([MarshalAs(UnmanagedType.LPStr)] string lpMessage) {
            if (Listener.IsDisabled) return 0;

            try {
                Logger.Info("BKC_OnSend for {0}", lpMessage);
                BeckyOnSend result = Listener.OnSend(lpMessage);
                return (int)result; // Return 0 to proceed the sending operation.
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
                return 0;
            }
        }

        [DllExport("BKC_OnFinishRetrieve", CallingConvention.Winapi)]
        public static int BKC_OnFinishRetrieve(int nNumber) {
            if (Listener.IsDisabled) return 0;

            try {
                Logger.Info("BKC_OnFinishRetrieve for {0}", nNumber);
                Listener.OnFinishRetrieve(nNumber);
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
            }
            return 0; // Always return 0.
        }


        [DllExport("BKC_OnPlugInSetup", CallingConvention.Winapi)]
        public static int BKC_OnPlugInSetup(IntPtr hWnd) {
            if (Listener.IsDisabled) return 0;

            try {
                Logger.Info("BKC_OnPlugInSetup");
                bool processed = Listener.OnPlugInSetup(hWnd);
                return processed ? 1 : 0; // Return nonzero if you have processed.
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
                return 0;
            }
        }

        [DllExport("BKC_OnPlugInInfo", CallingConvention.Winapi)]
        public static int BKC_OnPlugInInfo(ref Api_TagBkPlugininfo lpPlugInInfo) {
            try {
                Logger.Info("BKC_OnPlugInInfo");
                Listener.OnPlugInInfo()
                    .FillStruct(ref lpPlugInInfo);

            } catch (Exception e) {
                Listener.GotUnhandledException(e);
            }
            return 0; // Always return 0.
        }

        [DllExport("BKC_OnDragDrop", CallingConvention.Winapi)]
        public static int BKC_OnDragDrop(
            [MarshalAs(UnmanagedType.LPStr)] string lpTgt,
            [MarshalAs(UnmanagedType.LPStr)] string lpSrc,
            int nCount, BeckyDropEffect dropEffect) {

            if (Listener.IsDisabled) return 0;

            try {
                Logger.Info("BKC_OnDragDrop from {1} to {0} with ({2}/{3})", lpTgt, lpSrc, nCount, dropEffect);
                bool cancel = Listener.OnDragDrop(lpTgt, lpSrc, nCount, dropEffect);

                // If you want to cancel the default drag and drop action,
                // return -1;
                // Do not assume the default action (copy, move, etc.) is always
                // processed, because other plug-ins might cancel the operation.
                return cancel ? -1 : 0;
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
                return 0;
            }
        }

        /*
        ////////////////////////////////////////////////////////////////////////
        // Called when certain image resources are requested.
        // You don't have to export this callback for regular plug-ins.
        // If more than one plug-ins respond this callback, the first response will be used.
        // So, the author should inform the users that they might need to disable some other resource
        // plug-ins in "General Setup" > "Advanced" > "Plug-Ins".
        // It is strongly recommended that you add "_b2icon_" prefix to your plug-in's file name
        // so that users can distinguish those resource-oriented plug-ins in the plug-in list.
        // e.g. _b2icon_abc.dll
        // For detailed information, see "_b2icon_" sample plug-in.
        [DllExport("BKC_OnRequestResource", CallingConvention.Winapi)]
        public static int BKC_OnRequestResource(int nType, int nImages, ref char[] lppResourceName) {
            if (Listener.IsDisabled) return 0;

            try {
                Logger.Info("BKC_OnRequestResource for ({0}/{1})", nType, nImages);
                lppResourceName = Listener.OnRequestResource(nType, nImages, );
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
            }
            return 0; // Always return 0.
        }

        [DllExport("BKC_OnRequestResource2", CallingConvention.Winapi)]
        public static int BKC_OnRequestResource2(int nType, int nImages, char** lppResourceName, int* pnImage) {
            if (Listener.IsDisabled) return 0;

            try{
                Logger.Info("BKC_OnRequestResource2 for ({0}/{1})", nType, nImages);
                lppResourceName = Listener.OnRequestResource2(nType, nImages);
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
            }
            return 0; // Always return 0.
        }*/

        ////////////////////////////////////////////////////////////////////////
        // 
        [DllExport("BKC_OnBeforeFilter2", CallingConvention.Winapi)]
        public static int BKC_OnBeforeFilter2(
            [MarshalAs(UnmanagedType.LPStr)] string lpMessage,
            [MarshalAs(UnmanagedType.LPStr)] string lpMailBox,
            [Out] out BeckyAction lpnAction, [Out] out char[] lppParam) {

            if (Listener.IsDisabled) {
                lpnAction = BeckyAction.ACTION_NOTHING;
                lppParam = null;
                return 0;
            }

            try {
                Logger.Info("BKC_OnBeforeFilter2 for ({0} :: {1})", lpMessage, lpMailBox);
                string strParam;
                var result = Listener.OnBeforeFilter2(lpMessage, lpMailBox, out lpnAction, out strParam);
                lppParam = strParam.ToCharArray(); //TODO: let this the marshalling do
                return (int)result;
            } catch (Exception e) {
                Listener.GotUnhandledException(e);
                lpnAction = BeckyAction.ACTION_NOTHING;
                lppParam = null;
                return 0;
            }
        }
    }
}
