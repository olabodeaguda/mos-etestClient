using MOSEtestClient.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOSEtestClient.Dao
{
    public class AppConfigDao
    {
        public void updateConfig(Profile lm)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["username"].Value = lm.username;
                config.AppSettings.Settings["firstname"].Value = lm.firstname;
                config.AppSettings.Settings["status"].Value = lm.status;
                config.AppSettings.Settings["surname"].Value = lm.surname;
                config.AppSettings.Settings["lastname"].Value = lm.lastname;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception x)
            {
                throw new Exception("An error occure while trying to update user section");
            }
        }

        public Profile read()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Profile loginM = new Profile();
            loginM.username = config.AppSettings.Settings["username"]?.Value;
            loginM.firstname = config.AppSettings.Settings["firstname"]?.Value;
            loginM.surname = config.AppSettings.Settings["surname"]?.Value;
            loginM.lastname = config.AppSettings.Settings["lastname"]?.Value;
            loginM.status = config.AppSettings.Settings["status"]?.Value;
            loginM.remoteUrl = config.AppSettings.Settings["remoteUrl"]?.Value;

            int qCount = 100;
            if (int.TryParse(config.AppSettings.Settings["questionCount"]?.Value, out qCount))
            {
                loginM.questionCount = qCount;
            }
            else
            {
                loginM.questionCount = 100;
            }

            return loginM;
        }
    }
}
