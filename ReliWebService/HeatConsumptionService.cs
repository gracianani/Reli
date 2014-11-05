using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using ReliDemo.ViewModels;
using ReliDemo.Models;
using ReliDemo.Infrastructure.Services;
using ReliDemo.Infrastructure.Repositories;

namespace ReliWebService
{
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.

    [ServiceContract]
    public interface IHeatConsumptionService
    {
        [OperationContract]
        [WebGet(UriTemplate = "GetTodaysGJ/{companyId}/{managershipId}", ResponseFormat = WebMessageFormat.Json)]
        ValueWithUnit GetTodaysGJ(string companyId, string managershipId);

        [OperationContract]
        [WebGet(UriTemplate = "GetRealTime/{companyId}/{managershipId}", ResponseFormat = WebMessageFormat.Json)]
        IList<HeatConsumptionTotalItem> GetRealTime(string companyId, string managershipId);

        [OperationContract]
        [WebGet(UriTemplate = "GetHeatConsumptionGraphData/{heatConsumptionGraphType}/{fromDate}/{toDate}/{companyId}/{managershipId}/{stationId}", ResponseFormat = WebMessageFormat.Json)]
        GJHistoriesViewModel GetHeatConsumptionGraphData(string heatConsumptionGraphType, string fromDate, string toDate, string companyId, string managershipId, string stationId);
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file
    public class HeatConsumptionService : IHeatConsumptionService
    {
        public IList<HeatConsumptionTotalItem> GetRealTime(string companyId, string managershipId)
        {
            var web=new ReliDemo.Infrastructure.Services.HeatConsumptionSummaryService();
            if (Convert.ToInt32(companyId) != -1)
            {
                var todays = web.GetTodaysGJRealTimeByCompanyId(Convert.ToInt32(companyId));
                return todays;
            }
            else if (Convert.ToInt32(managershipId) != -1)
            {
                var todays = web.GetTodaysGJRealTimeByManagershipId(Convert.ToInt32(managershipId));
                return todays;
            }
            else
            {
                var todays = web.GetTodaysGJRealTime();
                return todays;
            }
        }
        public ValueWithUnit GetTodaysGJ(string companyId, string managershipId)
        {
            var web = new ReliDemo.Infrastructure.Services.HeatConsumptionSummaryService();
            if (Convert.ToInt32(companyId) != -1)
            {
                var todays = web.GetHeatConsumptionAccuByCompany(Convert.ToInt32(companyId), DateTime.Today);
                if (todays > 10000)
                {
                    return new ValueWithUnit() { Unit = "万GJ", UpdatedAt = DateTime.Now, Value = Convert.ToDecimal(todays / 10000.0m) };
                }
                return new ValueWithUnit() { Unit = "GJ", UpdatedAt = DateTime.Now, Value = todays };
            }
            else if (Convert.ToInt32(managershipId) != -1)
            {
                var todays = web.GetHeatConsumptionAccuByManagership(Convert.ToInt32(managershipId), DateTime.Today);
                if (todays > 10000)
                {
                    return new ValueWithUnit() { Unit = "万GJ", UpdatedAt = DateTime.Now, Value = Convert.ToDecimal(todays / 10000.0m) };
                }
                return new ValueWithUnit() { Unit = "GJ", UpdatedAt = DateTime.Now, Value = todays };
            }
            else
            {
                var todays = web.GetHeatConsumptionAccuByDate(Region.全网, DateTime.Today);
                if (todays > 10000)
                {
                    return new ValueWithUnit() { Unit = "万GJ", UpdatedAt = DateTime.Now, Value = Convert.ToDecimal(todays / 10000.0m) };
                }
                return new ValueWithUnit() { Unit = "GJ", UpdatedAt = DateTime.Now, Value = todays };
            }
        }

        public GJHistoriesViewModel GetHeatConsumptionGraphData(string heatConsumptionGraphType, string fromDate, string toDate, string companyId, string managershipId, string stationId)
        {
            var service = new HeatConsumptionSummaryService();
            var weatherService = new WeatherService();
            var cFromDate = new DateTime(Convert.ToInt64(fromDate) * 10000 + new DateTime(1970,1,1).Ticks );
            var cToDate = new DateTime(Convert.ToInt64(toDate) * 10000 + new DateTime(1970, 1, 1).Ticks);
            var forecasts = weatherService.GetForecast(cFromDate, cToDate);
            var actual = weatherService.GetActual(cFromDate, cToDate);
            var intCompanyId = Convert.ToInt32(companyId);
            var intManagershipId = Convert.ToInt32(managershipId);
            int intStationId = Convert.ToInt32(stationId);
            var days = (cToDate - cFromDate).Days;
            if (heatConsumptionGraphType == HeatConsumptionGraphType.Company.ToString())
            {
                var histories = service.GetHistoriesByCompany(intCompanyId, cFromDate, cToDate);
                return  new GJHistoriesViewModel
                {
                    FromDate = cFromDate,
                    ToDate = cToDate,
                    实际Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.采暖GJ ?? 0.0m, DateTime.Today.AddDays(-(days - j)).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    核算Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.核算GJ ?? 0.0m, DateTime.Today.AddDays(-(days - j)).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    计划Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.计划GJ ?? 0.0m, DateTime.Today.AddDays(-(days - j)).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    预报温度 = "[" + string.Join(",", forecasts.Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    实际温度 = "[" + string.Join(",", actual.Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    类型 = (HeatConsumptionGraphType) Enum.Parse(typeof(HeatConsumptionGraphType), heatConsumptionGraphType)
                };
            }
            else if (heatConsumptionGraphType == HeatConsumptionGraphType.Managership.ToString())
            {
                var histories = service.GetHistoriesByManagership(intManagershipId, cFromDate, cToDate);
                return new GJHistoriesViewModel
                {
                    FromDate = cFromDate,
                    ToDate = cToDate,
                    实际Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.采暖GJ ?? 0.0m, DateTime.Today.AddDays(-(days - j)).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    核算Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.核算GJ ?? 0.0m, DateTime.Today.AddDays(-(days - j)).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    计划Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.计划GJ ?? 0.0m, DateTime.Today.AddDays(-(days - j)).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    预报温度 = "[" + string.Join(",", forecasts.Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    实际温度 = "[" + string.Join(",", actual.Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    类型 = (HeatConsumptionGraphType)Enum.Parse(typeof(HeatConsumptionGraphType), heatConsumptionGraphType)
                };
            }
            else if (heatConsumptionGraphType == HeatConsumptionGraphType.Total.ToString())
            {
                var histories = service.GetHistories(cFromDate, cToDate);
                return new GJHistoriesViewModel
                {
                    FromDate = cFromDate,
                    ToDate = cToDate,
                    实际Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.采暖GJ ?? 0.0m, DateTime.Today.AddDays(-(days - j)).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    核算Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.核算GJ ?? 0.0m, DateTime.Today.AddDays(-(days - j)).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    计划Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.计划GJ ?? 0.0m, DateTime.Today.AddDays(-(days - j)).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    预报温度 = "[" + string.Join(",", forecasts.Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    实际温度 = "[" + string.Join(",", actual.Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    类型 = (HeatConsumptionGraphType)Enum.Parse(typeof(HeatConsumptionGraphType), heatConsumptionGraphType)
                };
            }
            else if (heatConsumptionGraphType == HeatConsumptionGraphType.Station.ToString())
            {
                var histories = service.GetHistoriesByStation(intStationId,cFromDate, cToDate);
                return new GJHistoriesViewModel
                {
                    FromDate = cFromDate,
                    ToDate = cToDate,
                    实际Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.采暖GJ ?? 0.0m, DateTime.Today.AddDays(-(days - j)).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    核算Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.核算GJ ?? 0.0m, DateTime.Today.AddDays(-(days - j)).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    计划Data = "[" + string.Join(",", histories.Select((i, j) => string.Format("[{1},{0}]", i.计划GJ ?? 0.0m, DateTime.Today.AddDays(-(days - j)).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    预报温度 = "[" + string.Join(",", forecasts.Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    实际温度 = "[" + string.Join(",", actual.Select(i => string.Format("[{1},{0}]", i.Temperature, i.时间.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToArray()) + "]",
                    类型 = (HeatConsumptionGraphType)Enum.Parse(typeof(HeatConsumptionGraphType), heatConsumptionGraphType)
                };
            }
            else 
            {
                return new GJHistoriesViewModel();
            }
        }
        /*
        [WebInvoke(UriTemplate = "", Method = "POST")]
        public SampleItem Create(SampleItem instance)
        {
            // TODO: Add the new instance of SampleItem to the collection
            throw new NotImplementedException();
        }

        [WebGet(UriTemplate = "{id}")]
        public SampleItem Get(string id)
        {
            // TODO: Return the instance of SampleItem with the given id
            throw new NotImplementedException();
        }
        */
    }
}
