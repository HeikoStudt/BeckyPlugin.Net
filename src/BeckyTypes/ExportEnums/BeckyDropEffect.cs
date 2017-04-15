// ReSharper disable InconsistentNaming
namespace BeckyTypes.ExportEnums
{
    public enum BeckyDropEffect {
        INVALID = 0,
        /// <summary>
        /// Copy
        /// </summary>
        DROPEFFECT_COPY = 1,
        /// <summary>
        /// Move
        /// </summary>
        DROPEFFECT_MOVE = 2,
        /// <summary>
        /// Link (Used for filtering setup in Becky!)
        /// </summary>
        DROPEFFECT_LINK = 4,
    }
}