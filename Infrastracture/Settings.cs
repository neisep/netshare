using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture
{
    public class SettingFileHandler
    {
        public void Save<T>(T jsonObject)
        {
            var dummy = JsonConvert.SerializeObject(jsonObject);
            //File.WriteAllBytes();
        }

        public void Load<T>()
        {

        }
    }
}
