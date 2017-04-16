using System.IO;
using System.Text;

namespace Utilities {
    public static class StringToStream {
        public static MemoryStream GenerateStream(this string value, Encoding encoding = null) {
            return new MemoryStream((encoding ?? Encoding.UTF8).GetBytes(value ?? ""));
        }
    }
}