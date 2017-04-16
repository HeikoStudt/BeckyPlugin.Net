namespace Utilities {
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    //http://stackoverflow.com/questions/217902/reading-writing-an-ini-file
    // small-scale changes
    public class IniFile // revision 11
    {
        private readonly string _path;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string section, string key, string @default, StringBuilder retVal,
            int size, string filePath);

        public IniFile(string iniPath) {
            _path = new FileInfo(iniPath).FullName;
        }

        public string Read(string key, string section) {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", retVal, 255, _path);
            return retVal.ToString();
        }

        public void Write(string key, string value, string section) {
            WritePrivateProfileString(section, key, value, _path);
        }

        public void DeleteKey(string key, string section) {
            Write(key, null, section);
        }

        public void DeleteSection(string section) {
            Write(null, null, section);
        }

        public bool KeyExists(string key, string section = null) {
            return Read(key, section).Length > 0;
        }
    }
}
