using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture
{
    public static class Helper
    {
        public static string ApplicationName { get; set; }
        public static string AppConfigPath => $@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\netshare";
        public static string SharesConfigFileName => "shares.json";
        public static string SharesConfigWithPathAndFileName => $@"{AppConfigPath}\{SharesConfigFileName}";

        public static List<string> GetUnusedDriveLetters => Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (Char) i + ":").Except(DriveInfo.GetDrives().Select(x => x.Name.Replace("\\", ""))).ToList();

        public static byte[] Key { get; set; }
    }
}
