using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectricVehicleChargingPredictor.Models
{
    public class LocationModel
    {
        public int loc_id { get; set; }
        public string address { get; set; }
        public string cadaster { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public string location_type { get; set; }

    }

    public class SOCModel
    {
        public int ev_id { get; set; }
        public int time { get; set; }
        public float soc { get; set; }
    }

    public class DrivingBehaviourModel
    {
        public int ev_id { get; set; }
        public int time { get; set; }
        public float soc { get; set; }
    }

    public class VehicleModelModel
    {
        public int model_id { get; set; }
        public string model { get; set; }
        public float battery_size { get; set; }
        public float charge_power { get; set; }
        public float efficiency { get; set; }
    }
}