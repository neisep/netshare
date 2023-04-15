using Domain;
using Newtonsoft.Json;
using System.IO;

namespace Infrastracture
{
    public class SettingFileHandler
    {
        public void Save(SettingsItem jsonObject)
        {
            File.WriteAllText(Helper.OptionsWithPathAndFileName, JsonConvert.SerializeObject(jsonObject));
        }

        public SettingsItem Load()
        {
            var jsonObject = File.ReadAllText(Helper.OptionsWithPathAndFileName);
            return JsonConvert.DeserializeObject<SettingsItem>(jsonObject);
        }
    }
}
