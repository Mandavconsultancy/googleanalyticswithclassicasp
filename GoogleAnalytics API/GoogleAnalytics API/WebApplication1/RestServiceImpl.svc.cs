using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace GoogleAnalyticsAPIDemo
{
    public class RestServiceImpl : IRestServiceImpl
    {
        #region IRestServiceImpl Members

        public string XMLData(string id)
        {
            return "You requested product " + id;
        }

        public List<string> JSONData()
        {
            string profileID = ConfigurationManager.AppSettings["profileID"].ToString();
            string serviceAccountEmail =  ConfigurationManager.AppSettings["serviceAccountEmail"].ToString();
            var certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(AppDomain.CurrentDomain.BaseDirectory+ "//"+ ConfigurationManager.AppSettings["skey"].ToString(), "notasecret", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            var credential = new ServiceAccountCredential(
            new ServiceAccountCredential.Initializer(serviceAccountEmail)
            {
                Scopes = new[] { AnalyticsService.Scope.Analytics }
            }.FromCertificate(certificate));

            // Create the service.
            var GoogleAnalyticsService = new AnalyticsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "MyApp",
            });


            var request = GoogleAnalyticsService.Data.Ga.Get("ga:" + profileID, "2016-04-09", "2016-05-09", "ga:users");
            request.Dimensions = "ga:year,   ga:month,  ga:day,   ga:country, ga:city";

            Google.Apis.Analytics.v3.Data.GaData d = request.Execute();
            if (d != null)
            {

                List<string> lst = new List<string>();

                string temp = string.Empty;

                foreach (IList<string> row in d.Rows)
                {  
                    foreach (string col in row) { temp += " " + col + ""; }
                    lst.Add(temp);
                }
                return lst;
            }

            return null;
        }

        #endregion
    }
}