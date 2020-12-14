using System;

namespace ElectricVehicleChargingPredictor
{
    public static class DateTimeHelper
    {
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            try
            {
                // Unix timestamp is seconds past epoch
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
                return dtDateTime;
            }
            catch
            {
                throw;
            }
        }

        public static Boolean IsDateValid(string cellValue, out DateTime? date)
        {
            date = null;
            Double tmpDouble;
            DateTime tmpDate;


            if (!string.IsNullOrWhiteSpace(cellValue))
            {
                if (DateTime.TryParse(cellValue, out tmpDate))
                {
                    date = tmpDate;
                }
                else if (Double.TryParse(cellValue, out tmpDouble))
                {
                    try
                    {
                        date = DateTime.FromOADate(tmpDouble);
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
