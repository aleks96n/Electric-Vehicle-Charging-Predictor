using System.Data;

namespace ElectricVehicleChargingPredictor
{
    public class Constants
    {
        //setup for every transaction created
        public static IsolationLevel IsolationSetup = IsolationLevel.Snapshot;

        //
        public static string Checkbox = "Checkbox";
        public static string NonCompliant = "Non-Compliant";
        public static string undefined = "undefined";
        public static string category = "category";
        public static string header = "header";
        public static string subheader = "subheader";
        public static string conditional = "conditional";
        public static string checkbox_checked = "1";
        public static string checkbox_unchecked = "0";
        public static string NotAnsweredValue = "-";
        public static string NotApplicable = "Not Applicable";
        

        //pdf html
        public static string div_panel_content_inner = "<div class='panel-content-inner'>";
    }
}

