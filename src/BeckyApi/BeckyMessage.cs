using System;
// ReSharper disable InconsistentNaming

namespace BeckyApi
{
    [Flags]
    public enum BeckyMessage {
        NONE = 0,
        /// <summary>
        /// Message is read.
        /// </summary>
        MESSAGE_READ = 0x00000001,
        /// <summary>
        /// Message is forwarded.
        /// </summary>
        MESSAGE_FORWARDED = 0x00000002,
        /// <summary>
        /// Message is replied.
        /// </summary>
        MESSAGE_REPLIED = 0x00000004,
        /// <summary>
        ///  Message has attachments
        /// </summary>
        MESSAGE_ATTACHMENT = 0x00000008,
        /// <summary>
        /// Message is a part of message/partial
        /// </summary>
        MESSAGE_PARTIAL = 0x00000100,
        /// <summary>
        /// Message is sent as a redirected message.
        /// (Resent- headers are found.)
        /// </summary>
        MESSAGE_REDIRECT = 0x00000200,

        UNDOCUMENTED_1 = 0x02000,
        UNDOCUMENTED_2 = 0x10000,
    }
}