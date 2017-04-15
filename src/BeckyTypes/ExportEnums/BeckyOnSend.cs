// ReSharper disable InconsistentNaming
namespace BeckyTypes.ExportEnums
{
    public enum BeckyOnSend {
        /// <summary>
        ///   Did nothing.
        /// </summary>
        NOTHING = 0,
        /// <summary>
        ///    Return BKC_ONSEND_ERROR, if you want to cancel the sending operation.
        ///   You are responsible for displaying an error message.
        /// </summary>
        BKC_ONSEND_ERROR = -1,
        /// <summary>
        ///   if you have processed this message
        ///   and don't need Becky! to send it.
        ///   Becky! will move this message to Sent box when the sending
        ///   operation is done.
        /// </summary>
        /// <remarks>
        ///   CAUTION: You are responsible for the destination of this
        ///       message if you return BKC_ONSEND_PROCESSED.
        /// </remarks>
        BKC_ONSEND_PROCESSED = -2,
    }
}