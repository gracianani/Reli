using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System.Configuration;
using ReliDemo.Core.Interfaces;
using ReliWebService.Repository;
using System.Net;
using WebMatrix.WebData;
using System.IO;
using ReliWebService.Helper;
using ReliDemo.Infrastructure.Services;
using ReliDemo.Models;

namespace ReliWebService
{
    [ServiceContract]
    [ServiceKnownType(typeof(ReliMobileUser))]
    public interface IMobileService
    {
        [OperationContract]
        [WebGet(UriTemplate = "AuthenticateUser?userName={userName}&password={password}",ResponseFormat = WebMessageFormat.Json)]
        bool AuthenticateUser(string userName, string password);
        
        [OperationContract]
        [WebGet(UriTemplate = "Users/{userName}", ResponseFormat = WebMessageFormat.Json)]
        ReliMobileUser GetUserAccount(string userName);

        [OperationContract]
        [WebGet(UriTemplate = "StartupBlocks?types={startupBlockTypes}", ResponseFormat = WebMessageFormat.Json)]
        StartupBlocks GetStartupBlocks(string startupBlockTypes);

        [OperationContract]
        [WebGet(UriTemplate = "DailyReports", ResponseFormat = WebMessageFormat.Json)]
        DailyReports GetDailyReports();

        [OperationContract]
        [WebGet(UriTemplate = "DailyReports/{dailyReportId}", ResponseFormat = WebMessageFormat.Json)]
        DailyReport GetDailyReportById(string dailyReportId);

        [OperationContract]
        [WebGet(UriTemplate = "CustomerServiceReports", ResponseFormat = WebMessageFormat.Json)]
        DailyReports GetCustomerServiceReports();

        [OperationContract]
        [WebGet(UriTemplate = "CustomerServiceReports/{customerServiceReportId}", ResponseFormat = WebMessageFormat.Json)]
        DailyReport GetCustomerServiceReportById(string customerServiceReportId);

        [OperationContract]
        [WebGet(UriTemplate = "Warnings", ResponseFormat = WebMessageFormat.Json)]
        Warnings GetWarnings();

        [OperationContract]
        [WebGet(UriTemplate = "Warnings/{warningId}", ResponseFormat = WebMessageFormat.Json)]
        Warning GetWarningById(string warningId);

        [OperationContract]
        [WebGet(UriTemplate = "{userName}/Messages", ResponseFormat = WebMessageFormat.Json)]
        Messages GetMessages(string userName);

        [OperationContract]
        [WebGet(UriTemplate = "{userName}/Messages/{messageId}", ResponseFormat = WebMessageFormat.Json)]
        Message GetMessageById(string userName, string messageId);

        [OperationContract]
        [WebInvoke(UriTemplate = "{userName}/Messages", Method = "PUT", RequestFormat=WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message CreateMessage(string userName, Message message);

        [WebInvoke(UriTemplate = "{userName}/Messages/UploadPhoto", Method = "POST", ResponseFormat=WebMessageFormat.Json)]
        int UploadPhoto(string userName, Stream fileContents);
        [OperationContract]
        [WebInvoke(UriTemplate = "{userName}/Messages/UploadPhoto/{messageId}", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void UpdatePhoto(string userName, string messageId, Message message);

        [OperationContract]
        [WebGet(UriTemplate = "WeatherStations", ResponseFormat = WebMessageFormat.Json)]
        WeatherStations GetWeatherStations();

        [OperationContract]
        [WebGet(UriTemplate = "Weathers/{weatherTypeId}", ResponseFormat = WebMessageFormat.Json)]
        TemperatureDetails GetOfficalWeather(string weatherTypeId);

        [OperationContract]
        [WebGet(UriTemplate = "Weathers?from={fromDate}&to={toDate}", ResponseFormat = WebMessageFormat.Json)]
        TemperatureDetails GetOfficalWeatherDetails(string fromDate, string toDate);

        [OperationContract]
        [WebGet(UriTemplate = "StationTitles", ResponseFormat = WebMessageFormat.Json)]
        ReliMobileStationTitles GetStationPagedStationTitle();

        [OperationContract]
        [WebGet(UriTemplate = "Stations", ResponseFormat = WebMessageFormat.Json)]
        ReliMobileStationSummaries GetStations ();

        [OperationContract]
        [WebInvoke(UriTemplate = "SavedStations", ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        ReliMobileStationSummaries GetStationsByIds( List<int> ids );

        [OperationContract]
        [WebGet(UriTemplate = "Stations/{stationId}", ResponseFormat = WebMessageFormat.Json)]
        ReliMobileStation GetStationById( string stationId);

        [OperationContract]
        [WebGet(UriTemplate = "Stations/{stationId}/Histories?from={fromDate}&to={toDate}", ResponseFormat = WebMessageFormat.Json)]
        BackwardAndSupplies GetStationHistories(string stationId, string fromDate, string toDate);

        [OperationContract]
        [WebGet(UriTemplate = "Stations/{stationId}/GJHistories?from={fromDate}&to={toDate}", ResponseFormat = WebMessageFormat.Json)]
        StationGJs GetStationGJHistories(string stationId, string fromDate, string toDate);

        [OperationContract]
        [WebGet(UriTemplate = "HeatSources", ResponseFormat = WebMessageFormat.Json)]
        ReliMobileHeatSources GetHeatSources();

        [OperationContract]
        [WebGet(UriTemplate = "HeatSources/{heatSourceId}", ResponseFormat = WebMessageFormat.Json)]
        ReliMobileHeatSourceRecents GetHeatSourceRecentsByHeatSourceId(string heatSourceId);

        [OperationContract]
        [WebGet(UriTemplate = "HeatSources/{heatSourceId}/Recents/{heatSourceRecentId}/Histories?from={fromDate}&to={toDate}", ResponseFormat = WebMessageFormat.Json)]
        BackwardAndSupplies GetHeatSourceRecentHistories(string heatSourceId, string heatSourceRecentId, string fromDate, string toDate);

        [OperationContract]
        [WebGet(UriTemplate = "HeatSources/{heatSourceId}/Recents/{heatSourceRecentId}/GJHistories?from={fromDate}&to={toDate}", ResponseFormat = WebMessageFormat.Json)]
        HeatSourceGJs GetHeatSourceRecentGJHistories(string heatSourceId, string heatSourceRecentId, string fromDate, string toDate);

        [OperationContract]
        [WebGet(UriTemplate = "HeatSourceSummary", ResponseFormat = WebMessageFormat.Json)]
        ReliMobileHeatSourceSummary GetHeatSourceSummary();

        [OperationContract]
        [WebGet(UriTemplate = "Summary/{userName}", ResponseFormat = WebMessageFormat.Json)]
        Summary GetSummary(string userName);

        [OperationContract]
        [WebGet(UriTemplate = "Overview/{userName}", ResponseFormat = WebMessageFormat.Json)]
        Overview Overview(string userName);

        [OperationContract]
        [WebGet(UriTemplate = "Version", ResponseFormat = WebMessageFormat.Json)]
        string GetLastestVersion();
    }

    [AspNetCompatibilityRequirements(RequirementsMode= AspNetCompatibilityRequirementsMode.Allowed)]
    public class MobileService : IMobileService
    {
        public bool AuthenticateUser(string userName, string password)
        {
            if (WebSecurity.Login(userName, password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ReliMobileUser GetUserAccount(string userName)
        {
            var userRepo = new UserRepository();

            var iuser = userRepo.FindByUserName(userName);
            WebOperationContext.Current.OutgoingResponse.StatusCode =
               HttpStatusCode.OK;
            return (ReliMobileUser)iuser; 
        }

        

        public DailyReports GetDailyReports()
        {
            var dailyReportRepo = new ReportRepository();
            return new DailyReports(dailyReportRepo.DailyReports.OrderByDescending(i=>i.dtCreatedAt).ToList());
        }

        public DailyReport GetDailyReportById(string dailyReportId)
        {
            var dailyReportRepo = new ReportRepository();
            return dailyReportRepo.DailyReports.Find(i=>i.intReportId == Convert.ToInt32(dailyReportId));
        }

        public DailyReports GetCustomerServiceReports()
        {
            var dailyReportRepo = new ReportRepository();
            return new DailyReports(dailyReportRepo.CustomerServiceReports.OrderByDescending(i => i.dtCreatedAt).ToList());
        }

        public DailyReport GetCustomerServiceReportById(string customerServiceReportId)
        {
            var dailyReportRepo = new ReportRepository();
            return dailyReportRepo.CustomerServiceReports.Find(i => i.intReportId == Convert.ToInt32(customerServiceReportId));
        }

        public StartupBlocks GetStartupBlocks(string startupBlockTypes)
        {
            var parsedStartupBlockTypes = startupBlockTypes.Split(',').Select(i => Convert.ToInt32(i)).ToList();
            var startupBlocks = new List<StartupBlock>();
            foreach (var startupBlockType in parsedStartupBlockTypes)
            {
                StartupBlock startupBlock = null;
                if ((StartupBlockType)startupBlockType == StartupBlockType.DailyReport)
                {
                    startupBlock = new StartupBlock(StartupBlockType.DailyReport, "热力日报");
                }
                else if ((StartupBlockType)startupBlockType == StartupBlockType.Warning)
                {
                    startupBlock = new StartupBlock(StartupBlockType.DailyReport, "预警信息");
                    startupBlock.StartupBlockValues.Add("3");
                }
                else if ((StartupBlockType)startupBlockType == StartupBlockType.Weather)
                {
                    startupBlock = new StartupBlock(StartupBlockType.Weather, "天气预报");
                    var weatherRepo = new TemperatureRepository();
                    var weatherDetails =new TemperatureDetails(weatherRepo.TodayAndYesterday);
                    startupBlock.StartupBlockValues.Add(weatherRepo.Today.actualLowest.ToString());
                    startupBlock.StartupBlockValues.Add(weatherRepo.Today.actualHighest.ToString());
                    startupBlock.StartupBlockValues.Add(weatherRepo.Today.day);
                    startupBlock.StartupBlockValues.Add(weatherRepo.Today.weatherType.ToString());
                    startupBlock.StartupBlockValues.Add(weatherRepo.Today.windSpeedAndDirection);
                }
                else if ((StartupBlockType)startupBlockType == StartupBlockType.Message)
                {
                    startupBlock = new StartupBlock(StartupBlockType.Weather, "信息上传");
                    startupBlock.StartupBlockValues.Add("5");
                    startupBlock.StartupBlockValues.Add("28");
                }
                else if ((StartupBlockType)startupBlockType == StartupBlockType.HeatSource)
                {
                    startupBlock = new StartupBlock(StartupBlockType.HeatSource, "热源厂");
                }
                else if ((StartupBlockType)startupBlockType == StartupBlockType.Station)
                {
                    startupBlock = new StartupBlock(StartupBlockType.Station, "热力站");
                }
                if (startupBlock != null)
                {
                    startupBlocks.Add(startupBlock);
                }
            }
            return new StartupBlocks(startupBlocks);
        }

        public Warnings GetWarnings()
        {
            var warningRepo = new WarningRepository();
            return new Warnings(warningRepo.Warnings);
        }

        public Warning GetWarningById(string warningId)
        {
            var intWarningId = Convert.ToInt32(warningId);
            var warningRepo = new WarningRepository();
            return warningRepo.Warnings.Find(i => i.warningId == intWarningId);
        }

        public Messages GetMessages(string userName)
        {
            var messageRepo = new MessageRepository();
            var results = messageRepo.Messages.Where(i => string.Compare(i.SendToUserName,userName,true) == 0 || string.Compare( i.SendFromUserName, userName, true) == 0).ToList();
            return new Messages(results);
        }

        public Message GetMessageById(string userName, string messageId)
        {
            var messageRepo = new MessageRepository();
            return messageRepo.Messages.Find(i => i.messageId == Convert.ToInt32(messageId));
        }

        public Message CreateMessage(string userName, Message message)
        {
            var messageRepo = new MessageRepository();
            var user = new UserRepository().FindByUserName(userName);
            message.sendFromUserId = user.UserId;
            message.sendToUserId = UserHelper.AdminId;
            message.CreatedAt = DateTime.Now;
            messageRepo.Insert(message);
            return message;
        }

        public int UploadPhoto(string userName, Stream fileContents)
        {
            var userRepo = new UserRepository();
            var messageRepo = new MessageRepository();
            var user = userRepo.FindByUserName(userName);

            byte[] buffer = new byte[32768];
            var createdAt = DateTime.Now;
            var fileName = user.UserName + "_" + createdAt.Ticks + ".jpg";
            var uploadFolder = ConfigurationManager.AppSettings["UploadFolder"];
            fileName = Path.Combine(uploadFolder, fileName);
            using (var fileStream = File.Create(fileName))
            {
                int bytesRead = 0;
                do
                {
                    bytesRead = fileContents.Read(buffer, 0, buffer.Length);
                    fileStream.Write(buffer, 0, bytesRead);
                } while (bytesRead > 0);

                var message = new Message() { CreatedAt = DateTime.Now, imageUrl = fileName, messageContent="", sendFromUserId = user.UserId, sendToUserId = UserHelper.AdminId };
                messageRepo.Insert(message);
                return message.messageId;
            }
        } 

        public void UpdatePhoto(string userName, string messageId, Message message)
        {
            var messageRepo = new MessageRepository();
            var dbMessage = messageRepo.Messages.Find(i => i.messageId == int.Parse(messageId));
            dbMessage.imageUrl +=  "##" + message.imageUri;
            messageRepo.Update(dbMessage);
        }
        public TemperatureDetails GetOfficalWeather(string weatherTypeId)
        {
            var enumWeatherType = (WeatherType) Convert.ToInt32(weatherTypeId);
            var weatherRepo = new TemperatureRepository();
            if ( enumWeatherType == WeatherType.TodayAndYesterday )
            {
                return new TemperatureDetails(weatherRepo.TodayAndYesterday);
            }
            else if (enumWeatherType == WeatherType.SevenDays)
            {
                return new TemperatureDetails(weatherRepo.Forecasts);
            }
            else
            {
                return new TemperatureDetails(weatherRepo.Histories.Take(7).ToList());
            }
        }

        public WeatherStations GetWeatherStations()
        {
            var weatherRepo = new TemperatureRepository();
            return new WeatherStations( weatherRepo.WeatherStations );
        }

        public TemperatureDetails GetOfficalWeatherDetails(string fromDate, string toDate)
        {
            var weatherRepo = new TemperatureRepository();
            var dtFromDate = DateTime.Parse(fromDate);
            var dtToDate = DateTime.Parse(toDate);
            return new TemperatureDetails(weatherRepo.Histories.Where(i=>i.日期>= DateTime.Parse(fromDate) && i.日期 <= DateTime.Parse(toDate)).ToList());
        }
        public ReliMobileHeatSources GetHeatSources()
        {
            var heatSourceRepo = new HeatSourceRepository();
            return new ReliMobileHeatSources( heatSourceRepo.HeatSources.OrderBy(i=>i.innerOrOuter).ThenBy(i=>i.eastOrWest).ToList() );
        }

        public ReliMobileHeatSourceRecents GetHeatSourceRecentsByHeatSourceId(string heatSourceId)
        {
            var heatSourceRepo = new HeatSourceRepository();
            return new ReliMobileHeatSourceRecents( heatSourceRepo.HeatSourceRecents.Where(i=>i.heatSourceId == Convert.ToInt32(heatSourceId)).ToList() );
        }

        public BackwardAndSupplies GetHeatSourceRecentHistories(string heatSourceId, string heatSourceRecentId, string fromDate, string toDate)
        {
            var heatSourceRepo = new HeatSourceRepository();
            var dtFromDate = DateTime.Parse(fromDate);
            var dtToDate = DateTime.Parse(toDate);
            var histories = heatSourceRepo.FindHistoryByDate(Convert.ToInt32(heatSourceId), Convert.ToInt32(heatSourceRecentId), dtFromDate, dtToDate).ToList();
            return new BackwardAndSupplies(histories.Select(i => 
                new BackwardAndSupply() { temperatureBackward = i.TemperatureIn, temperatureSupply = i.TemperatureOut, pressureBackward = i.PressureIn, pressureSupply = i.PressureOut, date = i.时间}).ToList());
        }

        public HeatSourceGJs GetHeatSourceRecentGJHistories(string heatSourceId, string heatSourceRecentId, string fromDate, string toDate)
        {
            var heatSourceRepo = new HeatSourceRepository();
            var dtFromDate = DateTime.Parse(fromDate);
            var dtToDate = DateTime.Parse(toDate);
            var histories = heatSourceRepo.FindAccuHistoryByDate(Convert.ToInt32(heatSourceId), Convert.ToInt32(heatSourceRecentId), dtFromDate, dtToDate).ToList();
            return new HeatSourceGJs(histories.ToList());
        }

        public ReliMobileStationSummaries GetStations()
        {
            var stationRepo = new StationRepository();
            var summaries = stationRepo.Stations.Where(i=> !string.IsNullOrEmpty(i.DBStation.数据来源) && string.IsNullOrEmpty(i.DBStation.报警) && 
                (i.DBStation.生产热源ID == 1 || i.DBStation.生产热源ID == 22))
                .Select(i => new ReliMobileStationSummary()
                {
                    isChaoBiao = i.isChaoBiao,
                    pressureIn = i.pressureIn,
                    pressureOut = i.pressureOut,
                    stationId = i.stationId,
                    stationName = i.stationName,
                    temperatureIn = i.temperatureIn,
                    temperatureOut = i.temperatureOut,
                    type = i.DBStation.数据来源,
                    eastOrWest = i.eastOrWest
                }).Take(20).ToList();
            return new ReliMobileStationSummaries(summaries);
        }
        public ReliMobileStationSummaries GetStationsByIds(List<int> ids)
        {
            var stationRepo = new StationRepository();
            var summaries = stationRepo.Stations.Where(i => 
                !string.IsNullOrEmpty(i.DBStation.数据来源) && 
                string.IsNullOrEmpty(i.DBStation.报警) &&
                (i.DBStation.生产热源ID == 1 || i.DBStation.生产热源ID == 22) && 
                ids.Contains(i.stationId) )
                .Select(i => new ReliMobileStationSummary()
                {
                    isChaoBiao = i.isChaoBiao,
                    pressureIn = i.pressureIn,
                    pressureOut = i.pressureOut,
                    stationId = i.stationId,
                    stationName = i.stationName,
                    temperatureIn = i.temperatureIn,
                    temperatureOut = i.temperatureOut,
                    type = i.DBStation.数据来源,
                    eastOrWest = i.eastOrWest
                }).ToList();
            return new ReliMobileStationSummaries(summaries);
        }
        public ReliMobileStation GetStationById(string stationId)
        {
            var stationRepo = new StationRepository();
            var station = stationRepo.Stations.Single(i => i.stationId == Convert.ToInt32(stationId));
            return station; 
        }

        public ReliMobileStationTitles GetStationPagedStationTitle()
        {
            var stationRepo = new StationRepository();
            var summaries = stationRepo.Stations.Where(i =>
                !string.IsNullOrEmpty(i.DBStation.数据来源) &&
                string.IsNullOrEmpty(i.DBStation.报警) &&
                (i.DBStation.生产热源ID == 1 || i.DBStation.生产热源ID == 22))
                .Select(i => new ReliMobileStationTitle()
                {
                    stationId = i.stationId,
                    stationName = i.stationName,
                    type = i.DBStation.数据来源,
                    eastOrWest = i.eastOrWest
                }).ToList();
            return new ReliMobileStationTitles(summaries);
        }
        public BackwardAndSupplies GetStationHistories(string stationId, string fromDate, string toDate)
        {
            var stationRepo = new StationRepository();
            var dtFromDate = DateTime.Parse(fromDate);
            var dtToDate = DateTime.Parse(toDate);
            return new BackwardAndSupplies(stationRepo.FindHistoryByDate(Convert.ToInt32(stationId), dtFromDate, dtToDate).ToList().Select(i =>
                new BackwardAndSupply() { pressureBackward = i.PressureIn1st, pressureSupply = i.PressureOut1st, 
                    temperatureSupply = i.TemperatureOut1st, temperatureBackward = i.TemperatureIn1st, date = i.时间 }).ToList());
        }

        public StationGJs GetStationGJHistories(string stationId, string fromDate, string toDate)
        {
            var stationRepo = new StationRepository();
            var dtFromDate = DateTime.Parse(fromDate);
            var dtToDate = DateTime.Parse(toDate);
            return new StationGJs(
                stationRepo.FindHistoryByDate(Convert.ToInt32(stationId), dtFromDate, dtToDate).ToList()
                .Select(i =>
                    new StationGJ() { 
                        planGJ = i.plannedGJ, 
                        calculateGJ = i.calculatedGJ, 
                        actualGJ= i.actualGJ, 
                        forecastTemperature= i.forecastTemperature,
                        actualTemperature = i.actualTemperature,
                        actualOverCalculateGJ = i.exceed,
                        日期 = i.时间
                    }).ToList());
        }

        public ReliMobileHeatSourceSummary GetHeatSourceSummary()
        {
            var service = new HeatConsumptionSummaryService();
            var histories = service.GetLatestHistory().ToList();
            var todaysGJ = service.GetTodaysGJ();
            var 今日累计供热量 = service.GetHeatConsumptionAccuByDate(Region.全网, DateTime.Today);
            var 昨日累计供热量 = service.GetHeatConsumptionAccuByDate(Region.全网, DateTime.Today.AddDays(-1));
            var companies = new ReliDemo.Infrastructure.Repositories.CompanyRepository().GetAllCompanies();
            var stations = new ReliDemo.Infrastructure.Repositories.StationRepository().GetAllStations();
            var area = service.GetTodaysHeatConsumptionSummary();
            int 有效站个数 = Convert.ToInt32(companies.Sum(i => i.有效监控站数));
            int 智能卡站个数 = stations.Count(i => string.Compare(i.数据来源, "智能卡") == 0);
            int 监控站个数 = stations.Count(i => string.Compare(i.数据来源, "监控") == 0);
            int 手抄表站个数 = stations.Count(i => string.IsNullOrEmpty(i.数据来源));
            var summary = new ReliMobileHeatSourceSummary(histories[0], histories[1], histories[2],
                智能卡站个数, 监控站个数, 监控站个数, 有效站个数, area, 今日累计供热量, 昨日累计供热量);
            //var summary = new ReliMobileHeatSourceSummary(
            //    histories[0],
            //   histories[1],
            //   histories[2],
            //   12,12,12,12, new HeatConsumptionArea(), 12.0m, 12.0m);
            return summary;
        }

        public Summary GetSummary(string userName)
        {
            var weatherRepo = new TemperatureRepository();
            var messageRepo = new MessageRepository();
            var warningRepo = new WarningRepository();
            var weatherToday = weatherRepo.Today;
            var messageCount = messageRepo.Messages.Count(i => i.sendFromUserId == UserHelper.AdminId && 
                                                                !string.IsNullOrEmpty(i.messageContent) &&
                                                               string.Compare(i.SendToUserName, userName, true) == 0 );
            var photoCount = messageRepo.Messages.Count(i => i.sendFromUserId == UserHelper.AdminId &&
                                                                !string.IsNullOrEmpty(i.imageUrl) &&
                                                               string.Compare(i.SendToUserName, userName, true) == 0);
            var warningCount = warningRepo.Warnings.Count;
            var summary = new Summary(weatherToday.forecastHighest, weatherToday.forecastLowest, weatherToday.forecastAverage,
                                      weatherToday.windSpeedAndDirection, weatherToday.weatherIcon, weatherToday.weatherDescription,
                                      messageCount, photoCount, warningCount);
            return summary;
        }


        public Overview Overview(string userName)
        {
            var overviewRepo = new TotalRepository();

            return overviewRepo.Overview;

        }
        public string GetLastestVersion()
        {
            return "V2.1";
        }
    }
}