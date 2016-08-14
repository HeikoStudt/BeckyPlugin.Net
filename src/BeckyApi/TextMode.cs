// ReSharper disable InconsistentNaming
namespace BeckyApi
{
    public enum TextMode {
        /// <summary>
        /// Insert at the top of the existing text.
        /// </summary>
        INSERT_TOP = -1,
        /// <summary>
        /// Insert at the top of the existing text temporarily.
        /// </summary>
        INSERT_TOP_TEMP = -2,
        /// <summary>
        /// Replace the existing text.
        /// </summary>
        REPLACE = 0,
        /// <summary>
        /// Append to the existing text.
        /// </summary>
        APPEND = 1,
        /// <summary>
        /// Append to the existing text temporarily.
        /// </summary>
        APPEND_TEMP = 2,
    }
}