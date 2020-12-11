using ElectricVehicleChargingPredictor.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ElectricVehicleChargingPredictor
{
    public class VehicleServiceManager : BaseServiceManager
    {
        public VehicleServiceManager()
        {
        }

        public SimpleResult GetVehicles(string SAPVendorID = "")
        {
            var result = new SimpleResult { Success = false };

            try
            {
                var param = string.Empty;
                if (SAPVendorID != "")
                {
                    param = "?model_id=" + SAPVendorID;
                }


                //send request
                var request = new RestRequest("Vehicles" + param, Method.GET)
                {
                    RequestFormat = DataFormat.Json
                };

                //execute request
                var response = NWGClient.Execute(request);

                //handle response
                result = CommonResponseHandler(response);
                if (!result.Success) return result;

                //process data
                if (JsonHelper.IsValidJson(response.Content))
                {
                    JObject jsonObject = JObject.Parse(response.Content);
                    if (jsonObject["d"] != null)
                    {

                    }
                }

                result.Success = true;
            }
            catch
            {
                throw;
            }

            return result;
        }

        public SimpleResult GetChargerLocations(ref List<LocationModel> locations)
        {
            var result = new SimpleResult { Success = false };

            try
            {
                var param = string.Empty;

                //send request
                var request = new RestRequest("ChargerLocation" + param, Method.GET)
                {
                    RequestFormat = DataFormat.Json
                };

                //execute request
                var response = NWGClient.Execute(request);

                //handle response
                result = CommonResponseHandler(response);
                if (!result.Success) return result;

                var jsonResponse = JsonHelper.prepareJsonFromAPI(response.Content);

                //process data
                if (JsonHelper.IsValidJson(jsonResponse))
                {
                    
                    JObject jsonObject = JObject.Parse(jsonResponse);
                    if (jsonObject["d"] != null)
                    {
                        var jos = jsonObject["d"];

                        
                        foreach (var jo in jos)
                        {
                            var loc = new LocationModel();
                            loc.loc_id =int.Parse(jo["loc_id"].ToString().Trim());
                            loc.address = jo["address"].ToString().Trim();
                            loc.cadaster = jo["cadaster"].ToString().Trim();
                            loc.latitude = float.Parse(jo["latitude"].ToString().Trim());
                            loc.longitude = float.Parse(jo["longitude"].ToString().Trim());
                            loc.x = float.Parse(jo["x"].ToString().Trim());
                            loc.y = float.Parse(jo["y"].ToString().Trim());
                            loc.location_type = jo["location_type"].ToString().Trim();
                            locations.Add(loc);
                        }

                        
                    }
                }

                result.Success = true;
            }
            catch
            {
                throw;
            }

            return result;
        }

        public SimpleResult Predict_SOC(ref List<SOCModel> socs, string ev_id)
        {
            var result = new SimpleResult { Success = false };

            try
            {
                var param = "?ev_id="+ ev_id;

                //send request
                var request = new RestRequest("SOC" + param, Method.GET)
                {
                    RequestFormat = DataFormat.Json
                };

                //execute request
                var response = NWGClient.Execute(request);

                //handle response
                result = CommonResponseHandler(response);
                if (!result.Success) return result;

                var jsonResponse = JsonHelper.prepareJsonFromAPI(response.Content);

                //process data
                if (JsonHelper.IsValidJson(jsonResponse))
                {

                    JObject jsonObject = JObject.Parse(jsonResponse);
                    if (jsonObject["d"] != null)
                    {
                        var jos = jsonObject["d"];


                        foreach (var jo in jos)
                        {
                            var soc = new SOCModel();
                            soc.ev_id = int.Parse(jo["ev_id"].ToString().Trim());
                            soc.time = int.Parse(jo["time"].ToString().Trim());
                            soc.soc = float.Parse(jo["soc"].ToString().Trim());

                            socs.Add(soc);
                        }


                    }
                }

                result.Success = true;
            }
            catch
            {
                throw;
            }

            return result;
        }

        public SimpleResult GetVehicleModels(ref List<VehicleModelModel> models)
        {
            var result = new SimpleResult { Success = false };

            try
            {
                var param = "";

                //send request
                var request = new RestRequest("Models" + param, Method.GET)
                {
                    RequestFormat = DataFormat.Json
                };

                //execute request
                var response = NWGClient.Execute(request);

                //handle response
                result = CommonResponseHandler(response);
                if (!result.Success) return result;

                var jsonResponse = JsonHelper.prepareJsonFromAPI(response.Content);

                //process data
                if (JsonHelper.IsValidJson(jsonResponse))
                {
                    JObject jsonObject = JObject.Parse(jsonResponse);
                    if (jsonObject["d"] != null)
                    {
                        var jos = jsonObject["d"];

                        foreach (var jo in jos)
                        {
                            var model = new VehicleModelModel();
                            model.model_id = int.Parse(jo["model_id"].ToString().Trim());
                            model.model = jo["model"].ToString().Trim();
                            model.battery_size = float.Parse(jo["battery_size"].ToString().Trim());
                            model.charge_power = float.Parse(jo["charge_power"].ToString().Trim());
                            model.efficiency = float.Parse(jo["efficiency"].ToString().Trim());

                            models.Add(model);
                        }
                    }
                }

                result.Success = true;
            }
            catch
            {
                throw;
            }

            return result;
        }

        public SimpleResult GetVehicles(ref List<string> vehicles, string model_id)
        {
            var result = new SimpleResult { Success = false };

            try
            {
                var param = "?model_id=" + model_id;

                //send request
                var request = new RestRequest("Vehicles" + param, Method.GET)
                {
                    RequestFormat = DataFormat.Json
                };

                //execute request
                var response = NWGClient.Execute(request);

                //handle response
                result = CommonResponseHandler(response);
                if (!result.Success) return result;

                var jsonResponse = JsonHelper.prepareJsonFromAPI(response.Content);
                vehicles = jsonResponse.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',').ToList();


                result.Success = true;
            }
            catch
            {
                throw;
            }

            return result;
        }

    }
}