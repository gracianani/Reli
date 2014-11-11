using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliDemo.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace ReliDemo.Infrastructure.Services
{
    public sealed class ConfigurationService
    {

        private xz2013Entities db = new xz2013Entities();

        private decimal _比核算多耗Exceed;
        private decimal _比计划多耗超标线;
        private decimal _流量超标线;
        private decimal _回温超标线;
        private DateTime? _报表默认开始时间;
        private DateTime? _报表默认结束时间;
        public DateTime 报表默认开始时间
        {
            get
            {
                if (_报表默认开始时间.HasValue)
                {
                    return _报表默认开始时间.Value;
                }
                else {
                    return new DateTime(2014, 11, 7);
                }
            }
            set
            {
                _报表默认开始时间 = value;
            }
        }
        public DateTime 报表默认结束时间
        {
            get
            {
                if (_报表默认结束时间.HasValue)
                {
                    return _报表默认结束时间.Value;
                }
                else
                {
                    return new DateTime(2015, 4, 1);
                }
            }
            set
            {
                _报表默认开始时间 = value;
            }
        }
        private static  Lazy<ConfigurationService> configurationService =
            new Lazy<ConfigurationService>(() =>
                new ConfigurationService()
        );

        public static void ReloadConfigurationService()
        {
            configurationService = new Lazy<ConfigurationService>(() =>
                new ConfigurationService());
        }

        internal ConfigurationService()
        {
            var configuration = db.configurations.First();
            _比计划多耗超标线 = configuration.比计划多耗超标线;
            _比核算多耗Exceed = configuration.比核算多耗超标线;
            _流量超标线 = configuration.流量超标线;
            _回温超标线 = configuration.回温超标线;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select top (1) 报表默认开始时间, 报表默认结束时间 from configuration";
                    cmd.CommandType = System.Data.CommandType.Text;
                    using (var reader = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0)) { 
                                _报表默认开始时间 = reader.GetDateTime(0);
                            }
                            if (!reader.IsDBNull(1))
                            {
                                _报表默认结束时间 = reader.GetDateTime(1);
                            }
                        }
                    }
                }
            }

        }

        public void Save报表日期()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["membership"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "update configuration set 报表默认开始时间 = @start, 报表默认结束时间 = @end";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("start", _报表默认开始时间));
                    cmd.Parameters.Add(new SqlParameter("end", _报表默认结束时间));
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static ConfigurationService Instance { get { return configurationService.Value; } }

        public DateTime getStartHeatSupplyDate()
        {
            return db.Constants.First().起始供热时间.Value;
        }

        public int getTotalHeatSupplyDays()
        {
            return db.Constants.First().起始供热时间.Value < DateTime.Today ? (DateTime.Today - db.Constants.First().起始供热时间.Value).Days : 0;
        }
        public DateTime getEndHeatSupplyDate()
        {
            return db.Constants.First().结束供热时间.Value;
        }

        public decimal GasPrice()
        {
            return db.Constants.First().燃气热价.Value;
        }

        public decimal CoalPrice()
        {
            return db.Constants.First().燃煤热价.Value;
        }

        public  decimal TemperatureExceed{
            get {
                return _回温超标线;
            }
            set
            {
                _回温超标线 = value;
                db.configurations.First().回温超标线 = value;
                db.SaveChanges();
            }
        }

        public decimal WaterExceed
        {
            get
            {
                return _流量超标线;
            }
        }

        public decimal 比计划多耗Exceed
        {
            get
            {
                return _比计划多耗超标线;
            }
        }

        public decimal 比核算多耗Exceed
        {
            get
            {
                return _比核算多耗Exceed;
            }
        }
    }
}