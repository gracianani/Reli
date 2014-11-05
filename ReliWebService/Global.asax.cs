using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using WebMatrix.WebData;

namespace ReliWebService
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes();
            WebSecurity.InitializeDatabaseConnection("membership", "Users", "UserId", "Email", autoCreateTables: true);
        }

        private void RegisterRoutes()
        {
            RouteTable.Routes.Add(new ServiceRoute("HeatConsumptionService", new WebServiceHostFactory(), typeof(HeatConsumptionService)));
            RouteTable.Routes.Add(new ServiceRoute("MobileService", new WebServiceHostFactory(), typeof(MobileService)));
        }
    }
}
