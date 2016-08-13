namespace BeckyPlugin.Helpers
{
    public static class PInvokeExtensions
    {
        /// <summary>
        ///   Ensures the chararray size arrSize on str (adding \0 or cutting).
        ///   Currently it is probably not the fastest implementation.
        /// </summary>
        public static char[] ToCharArray(this string str, int arrSize) {
            if (str == null) {
                return new string('\0', arrSize).ToCharArray();
            }
            if (str.Length > arrSize) {
                return str.Substring(0, arrSize).ToCharArray();
            }
            return (str + new string('\0', arrSize - str.Length)).ToCharArray();
        }
    }
}