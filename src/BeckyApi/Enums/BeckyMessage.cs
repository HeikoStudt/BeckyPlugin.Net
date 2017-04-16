using System;

// ReSharper disable InconsistentNaming

namespace BeckyApi.Enums
{
    [Flags]
    public enum BeckyMessage : uint {
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

        UNDOCUMENTED_1 = 0x00000010,
        UNDOCUMENTED_2 = 0x00000020,
        UNDOCUMENTED_3 = 0x00000040,
        UNDOCUMENTED_4 = 0x00000080,
        
        /// <summary>
        /// Message is a part of message/partial
        /// </summary>
        MESSAGE_PARTIAL = 0x00000100,

        /// <summary>
        /// Message is sent as a redirected message.
        /// (Resent- headers are found.)
        /// </summary>
        MESSAGE_REDIRECT = 0x00000200,

        UNDOCUMENTED_5 = 0x00000400,
        UNDOCUMENTED_6 = 0x00000800,
        UNDOCUMENTED_7 = 0x00001000,
        UNDOCUMENTED_8 = 0x00002000,
        UNDOCUMENTED_9 = 0x00004000,
        UNDOCUMENTED_10 = 0x00008000,
        UNDOCUMENTED_11 = 0x00010000,
        UNDOCUMENTED_12 = 0x00020000,
        UNDOCUMENTED_13 = 0x00040000,
        UNDOCUMENTED_14 = 0x00080000,
        UNDOCUMENTED_15 = 0x00100000,
        UNDOCUMENTED_16 = 0x00200000,
        UNDOCUMENTED_17 = 0x00400000,
        UNDOCUMENTED_18 = 0x00800000,
        UNDOCUMENTED_19 = 0x01000000,
        UNDOCUMENTED_20 = 0x02000000,
        UNDOCUMENTED_21 = 0x04000000,
        UNDOCUMENTED_22 = 0x08000000,
        UNDOCUMENTED_23 = 0x10000000,
        UNDOCUMENTED_24 = 0x20000000,
        UNDOCUMENTED_25 = 0x40000000,
        UNDOCUMENTED_26 = 0x80000000,
    }
}