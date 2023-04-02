using Domain;
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
        public void Save(OptionsItem jsonObject)
        {
            File.WriteAllText(Helper.OptionsWithPathAndFileName, JsonConvert.SerializeObject(jsonObject));
        }

        public OptionsItem Load()
        {
            var jsonObject = File.ReadAllText(Helper.OptionsWithPathAndFileName);
            return JsonConvert.DeserializeObject<OptionsItem>(jsonObject);
        }
    }
}
