// ReSharper disable InconsistentNaming
namespace BeckyPlugin.DllExported
{
    public enum BeckyOutgoingMode {
        /// <summary>
        /// Save to outbox.
        /// </summary>
        OUTGOINGMODE_OUTBOX = 0,
        /// <summary>
        ///  Save to draft box.
        /// </summary>
        OUTGOINGMODE_DRAFTBOX = 1,
        /// <summary>
        /// Save as a reminder.
        /// </summary>
        OUTGOINGMODE_REMINDER = 2,
    }
}