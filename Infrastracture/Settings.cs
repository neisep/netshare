using Domain;
using Newtonsoft.Json;
using System.IO;

namespace Infrastracture
{
    public class SettingFileHandler
    {
        public void Save(SettingsItem jsonObject)
        {
            try
            {
                File.WriteAllText(Helper.OptionsWithPathAndFileName, JsonConvert.SerializeObject(jsonObject));
            }
            catch (System.Exception)
            {
                //Ignore error for now!
            }
        }

        public SettingsItem Load()
        {
            CheckIfOptionsDirectoryExists();
            SettingsItem settingsItem = null;
            try
            {
                var jsonObject = File.ReadAllText(Helper.OptionsWithPathAndFileName);
                settingsItem = JsonConvert.DeserializeObject<SettingsItem>(jsonObject);

            }
            catch (IOException ex) when (ex is FileLoadException ||
                                         ex is FileNotFoundException)
            {
                //Create Object and return new object!
                settingsItem = CreateDefaultSettingsItem();
                Save(settingsItem);
            }

            return settingsItem;
        }

        private void CheckIfOptionsDirectoryExists()
        {
            if(!Directory.Exists(Helper.AppConfigPath))
            {
                try
                {
                    Directory.CreateDirectory(Helper.AppConfigPath);
                }
                catch (System.Exception) {}
            }
        }

        private SettingsItem CreateDefaultSettingsItem()
        {
            return new SettingsItem
            {
                AutoMountOnStartUp = false,
                OnStartupMinimizeToTray = false,
            };
        }
    }
}
