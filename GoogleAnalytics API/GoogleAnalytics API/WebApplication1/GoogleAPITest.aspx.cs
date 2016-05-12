using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace GoogleAnalyticsAPIDemo
{
    public partial class form : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string profileID = "121434585";
            string serviceAccountEmail = "gaintegrate-1309@appspot.gserviceaccount.com";
            getAnalyticsData(profileID, serviceAccountEmail);
        }
        public void getAnalyticsData(string ProfileID, string serviceAccountEmail)
        {

            var certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(Server.MapPath("GAIntegrate-8ca6d26683fa.p12"), "notasecret", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
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


            var request = GoogleAnalyticsService.Data.Ga.Get("ga:" +121434585, "2016-04-09", "2016-05-09",  "ga:users");
            request.Dimensions = "ga:year,   ga:month,  ga:day,   ga:country, ga:city";

            Google.Apis.Analytics.v3.Data.GaData d = request.Execute();
            if (d != null)
            {
                litData.Text = ("<table>");
                foreach (IList<string> row in d.Rows)
                {
                    foreach (string col in row) { litData.Text += "<td>" + col + "</td>"; }
                    litData.Text += ("</tr><tr>");
                }
                litData.Text += ("</tr><table>");
            }

        }
    }

}
