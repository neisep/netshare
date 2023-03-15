using Infrastracture.Encryption;
using System;
using System.IO;

namespace Infrastracture
{
    public class KeyFile
    {
        private string fileName => $@"{Helper.AppConfigPath}\keyfile.dat";
        public byte[] CreateNew()
        {
            byte[] key = null;
            try
            {
                if (File.Exists(fileName))
                    return LoadFromFile();

                using (var fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
                {
                    key = AESGCM.NewKey();
                    fs.Write(key, 0, key.Length);
                    fs.Close();
                }
            }
            catch (Exception) { }
            return key;
        }

        private byte[] LoadFromFile()
        {
            byte[] key = null;
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    key = new byte[fs.Length];
                    fs.Read(key, 0, (int)fs.Length);
                }
            }
            catch (Exception){ }
            return key;
        }
    }
}
