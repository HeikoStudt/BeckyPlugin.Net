// ReSharper disable InconsistentNaming
namespace BeckyPlugin.DllExported
{
    public enum BeckyFilter {
        /// <summary>
        /// Do nothing and apply default filtering rules.
        /// </summary>
        BKC_FILTER_DEFAULT = 0,
        /// <summary>
        /// Apply default filtering rules after applying the rule it returns.
        /// </summary>
        BKC_FILTER_PASS = 1,
        /// <summary>
        /// Do not apply default rules.
        /// </summary>
        BKC_FILTER_DONE = 2,
        /// <summary>
        /// Request Becky! to call this callback again so that another rules can be added.
        /// </summary>
        BKC_FILTER_NEXT = 3,
    }
}