using ElectricVehicleChargingPredictor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ElectricVehicleChargingPredictor
{
    public partial class State_of_Charge : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var vsm = new VehicleServiceManager();
            var result = new SimpleResult();

            var locations = new List<LocationModel>();
            var socs = new List<SOCModel>();
            result = vsm.Predict_SOC(ref socs, "3");




        }
    }
}