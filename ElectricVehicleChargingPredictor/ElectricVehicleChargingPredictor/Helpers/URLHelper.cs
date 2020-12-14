using System;
using System.Web;

namespace ElectricVehicleChargingPredictor
{
    public static class URLHelper
    {
        //return absolute path of the application
        public static string GetBaseURL()
        {
            try
            {
                return 
                    HttpContext.Current.Request.Url.Scheme + "://" + 
                    HttpContext.Current.Request.Url.Authority +
                    HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
            }
            catch
            {
                throw;
            }
        }

        //return physical path of the application
        public static string GetAppURL()
        {
            try
            {
                return HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath);
            }
            catch
            {
                throw;
            }
            
        }
    }
}
