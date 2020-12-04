using System.Web.Configuration;
using System;

namespace ElectricVehicleChargingPredictor
{
    public class ConfigSettings
    {
        public static string APIBaseUrl
        {
            get
            {
                return WebConfigurationManager.AppSettings["API.BaseUrl"];
            }
        }
        public static string NWGUsername
        {
            get
            {
                return WebConfigurationManager.AppSettings["NWG.Username"];
            }
        }
        public static string NWGPassword
        {
            get
            {
                return WebConfigurationManager.AppSettings["NWG.Password"];
            }
        }
    }
}