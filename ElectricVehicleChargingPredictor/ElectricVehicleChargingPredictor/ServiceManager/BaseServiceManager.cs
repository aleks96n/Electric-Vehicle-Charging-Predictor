//using CAMS.BusinessEntities.Providers;
//using CAMS.Helpers;
//using CAMS.Utilities;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Net;

namespace ElectricVehicleChargingPredictor
{
    public class BaseServiceManager
    {
        //protected IDbContextScopeFactory dbContextScopeFactory { get; private set; }
        //protected DocumentTypeProvider DocumentTypeProvider { get; private set; }
        //protected MasterProvider MasterProvider { get; private set; }
        //protected VendorProvider VendorProvider { get; private set; }
        //protected CryptoHelper encryptor { get; private set; }
        protected static RestClient NWGClient { get; private set; }
        public BaseServiceManager()
        {
        
            //create static NWG Client
            if (NWGClient == null)
            {
                NWGClient = new RestClient();
                NWGClient.BaseUrl = new Uri(ConfigSettings.APIBaseUrl);
                //NWGClient.Authenticator = new HttpBasicAuthenticator(ConfigSettings.NWGUsername, ConfigSettings.NWGPassword);
                NWGClient.CookieContainer = new System.Net.CookieContainer();
            }
        }

        protected SimpleResult CommonResponseHandler(IRestResponse response)
        {
            var result = new SimpleResult { Success = false };
            var errorMessage = "";

            try
            {
                if (response.ErrorException != null)
                {
                    //throw response.ErrorException;
                    result.Messages.Add(response.ErrorException.ToString());
                }
                else if (response.ResponseStatus != ResponseStatus.Completed)
                {
                    //throw new Exception("network transport error");
                    result.Messages.Add("network transport error");
                }
                else if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.NoContent)
                {
                    //get error message from property StatusDescription
                    errorMessage = response.StatusDescription.ToLower();

                    //get error message from property Content
                    if (response.Content != "")
                    {
                        //get error message from SAP
                        if (JsonHelper.IsValidJson(response.Content))
                        {
                            JObject jsonObject = JObject.Parse(response.Content);
                            if (jsonObject["error"] != null && jsonObject["error"]["message"] != null && jsonObject["error"]["message"]["value"] != null)
                            {
                                errorMessage = jsonObject["error"]["message"]["value"].ToString().Trim();
                            }
                        }
                        //get error message from OCR
                        else
                        {
                            errorMessage = response.Content.Trim('\"');
                        }
                    }

                    result.Messages.Add(errorMessage);
                }

                result.Success = result.Messages.Count <= 0 ? true : false;
            }
            catch
            {
                throw;
            }
            return result;
        }
    }
}