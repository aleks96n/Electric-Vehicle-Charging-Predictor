using Newtonsoft.Json.Linq;
using System;

namespace ElectricVehicleChargingPredictor
{
    public static class JsonHelper
    {
        public static bool IsValidJson(string strInput)
        {
            try
            {
                JObject jsonObject = JObject.Parse(strInput);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static string prepareJsonFromAPI(string strInput)
        {
            var result = "";
            try 
            {
                result = strInput.Replace("\\", "").Replace("\":", ":").Replace(",\"", ",").Replace("{\"", "{");
                result = result.Remove(result.Length - 2, 2);
                result = result.Substring(1);

                return result;
            }
            catch
            {
                return "";
            }
        }
    }
}
