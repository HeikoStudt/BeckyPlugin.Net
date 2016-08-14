// ReSharper disable InconsistentNaming
namespace BeckyPlugin.DllExported
{
    public enum BeckyComposeMode {
        /// <summary>
        /// Compose new message.
        /// </summary>
        COMPOSE_MODE_COMPOSE1 = 0,
        /// <summary>
        /// Compose to replying address.
        /// </summary>
        COMPOSE_MODE_COMPOSE2 = 1,
        /// <summary>
        ///  Compose to selected addresses.
        /// </summary>
        COMPOSE_MODE_COMPOSE3 = 2,
        /// <summary>
        /// Edit/Create a template.
        /// </summary>
        COMPOSE_MODE_TEMPLATE = 3,
        /// <summary>
        /// Reply
        /// </summary>
        COMPOSE_MODE_REPLY1 = 5,
        /// <summary>
        /// Reply to All
        /// </summary>
        COMPOSE_MODE_REPLY2 = 6,
        /// <summary>
        /// Reply to selected addresses.
        /// </summary>
        COMPOSE_MODE_REPLY3 = 7,
        /// <summary>
        /// Forward.
        /// </summary>
        COMPOSE_MODE_FORWARD1 = 10,
        /// <summary>
        ///  Redirect.
        /// </summary>
        COMPOSE_MODE_FORWARD2 = 11,
        /// <summary>
        /// Forward as attachments.
        /// </summary>
        COMPOSE_MODE_FORWARD3 = 12,
    }
}