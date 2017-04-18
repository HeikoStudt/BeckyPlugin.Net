namespace Utilities {
    using IniParser;
    using IniParser.Model;
    using System;
    using System.IO;

    //http://stackoverflow.com/questions/217902/reading-writing-an-ini-file
    // rewritten to use IniParser and touching the file
    public class IniFile
    {
        private readonly string _path;

        public IniFile(string iniPath) {
            _path = new FileInfo(iniPath).FullName;
        }

        public string Read(string key, string section) {
            EnsureExists();
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(_path);
            return data[section][key] ?? ""; // default is empty string, not null
        }

        public void Write(string key, string value, string section) {
            EnsureExists();
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(_path);

            if (section == null) {
                throw new ArgumentNullException(nameof(section));
            } else if (key == null) {
                data.Sections.RemoveSection(section);
            } else if (value == null) {
                data[section].RemoveKey(key);
            } else {
                data[section][key] = value;
            }
            parser.WriteFile(_path, data);
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

        private void EnsureExists() {
            var directory = Path.GetDirectoryName(_path);
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
            // touch the file
            if (!File.Exists(_path)) {
                File.WriteAllText(_path, "");
            }
        }
    }
}
