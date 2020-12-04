using Newtonsoft.Json;
using System;

namespace ElectricVehicleChargingPredictor
{
    public static class SerializationHelper
    {
        public static string ObjectToJsonString(object obj)
        {
            try
            {
                //var json = new JavaScriptSerializer().Serialize(obj);
                var json = JsonConvert.SerializeObject(obj);
                return json;
            }
            catch
            {
                throw;
            }
        }
    }
}
