using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using ReliDemo.Models;

namespace ReliWebService
{

    [DataContract]
    public class ReliMobileHeatSourceSummary
    {
        private float _东部GJ;
        private float _西部GJ;
        private float _全网GJ;

        private float _东部计划GJ;
        private float _西部计划GJ;
        private float _全网计划GJ;

        private float _东部核算GJ;
        private float _西部核算GJ;
        private float _全网核算GJ;

        private float _东部计划热指标;
        private float _西部计划热指标;
        private float _全网计划热指标;

        private float _东部核算热指标;
        private float _西部核算热指标;
        private float _全网核算热指标;

        private float _东部实际运行热指标;
        private float _西部实际运行热指标;
        private float _全网实际运行热指标;

        private int _热源个数;
        private int _智能卡站个数;
        private int _监控站个数;
        private int _有效站个数;

        private float _总面积;
        private float _实际供热面积;
        private float _东部面积;
        private float _西部面积;

        private float _今日累计供热量;
        private float _昨日累计供热量;


        [DataMember]
        public int countHeatSources
        {
            get
            {
                return _热源个数;
            }
            set
            {
            }
        }

        [DataMember]
        public int countIC
        {
            get 
            {
                return _智能卡站个数;
            }
            set
            {
            }
        }

        [DataMember]
        public int countAuto
        {
            get
            {
                return _监控站个数;
            }
            set
            {
            }
        }

        [DataMember]
        public int countActive
        {
            get
            {
                return _有效站个数;
            }
            set
            {
            }
        }

        [DataMember]
        public float todaysGJ
        {
            get
            {
                return _今日累计供热量;
            }
            set
            {
            }
        }

        [DataMember]
        public float yesterdaysGJ
        {
            get
            {
                return _昨日累计供热量;
            }
            set
            {
            }
        }


        [DataMember]
        public float area
        {
            get
            {
                return _总面积;
            }
            set
            {
            }
        }

        [DataMember]
        public float eastArea
        {
            get
            {
                return _东部面积;
            }
            set
            {
            }
        }

        [DataMember]
        public float westArea
        {
            get
            {
                return _西部面积;
            }
            set
            {
            }
        }

        [DataMember]
        public float actualArea
        {
            get
            {
                return _实际供热面积;
            }
            set
            {
            }
        }

        [DataMember]
        public float heatLoad
        {
            get
            {
                return _全网GJ;
            }
            set
            {
            }
        }

        [DataMember]
        public float eastHeatLoad
        {
            get 
            {
                return _东部GJ;
            }
            set
            {
            }
        }

        [DataMember]
        public float westHeatLoad
        {
            get
            {
                return _西部GJ;
            }
            set
            {

            }
        }

        [DataMember]
        public float heatLoadPlanned
        {
            get
            {
                return _全网计划GJ;
            }
            set
            {

            }
        }

        [DataMember]
        public float heatLoadEastPlanned
        {
            get
            {
                return _东部计划GJ;
            }
            set
            {

            }
        }

        [DataMember]
        public float heatLoadWestPlanned
        {
            get
            {
                return _西部计划GJ;
            }
            set
            {

            }
        }

        [DataMember]
        public float heatLoadCalculated
        {
            get
            {
                return _全网核算GJ;
            }
            set
            {

            }
        }
        
        [DataMember]
        public float heatLoadEastCalculated
        {
            get
            {
                return _东部核算GJ;
            }
            set
            {

            }
        }

        [DataMember]
        public float heatLoadWestCalculated
        {
            get
            {
                return _西部核算GJ;
            }
            set
            {

            }
        }

        [DataMember]
        public float heatIndex
        {
            get
            {
                return _全网实际运行热指标;
            }
            set
            {

            }
        }

        [DataMember]
        public float heatIndexEast
        {
            get
            {
                return _东部实际运行热指标;
            }
            set
            {

            }
        }

        [DataMember]
        public float heatIndexWest
        {
            get
            {
                return _西部实际运行热指标;
            }
            set
            {

            }
        }

        [DataMember]
        public float heatIndexPlanned
        {
            get
            {
                return _全网计划热指标;
            }
            set
            {

            }
        }

        [DataMember]
        public float heatIndexEastPlanned
        {
            get
            {
                return _东部计划热指标;
            }
            set
            {

            }
        }

        [DataMember]
        public float heatIndexWestPlanned
        {
            get
            {
                return _西部计划热指标;
            }
            set
            {

            }
        }

        [DataMember]
        public float heatIndexCalculated
        {
            get
            {
                return _全网核算热指标;
            }
            set
            {

            }
        }

        [DataMember]
        public float heatIndexEastCalculated
        {
            get
            {
                return _东部核算热指标;
            }
            set
            {

            }
        }

        [DataMember]
        public float heatIndexWestCalculated
        {
            get
            {
                return _西部核算热指标;
            }
            set
            {

            }
        }

        public ReliMobileHeatSourceSummary()
        {
        }

        public ReliMobileHeatSourceSummary( GJHistoryItem 全网, GJHistoryItem 东部, GJHistoryItem 西部,
            int 热源个数, int 智能卡站个数, int 监控站个数, int 有效站个数, HeatConsumptionArea area,
            float 今日累计供热量, float 昨日累计供热量)
        {

            _东部GJ = (float)(东部.采暖GJ ?? 0.0m);
            _西部GJ = (float)(西部.采暖GJ ?? 0.0m);
            _全网GJ = (float)(全网.采暖GJ ?? 0.0m);

            _东部计划GJ = (float)(东部.计划GJ ?? 0.0m);
            _西部计划GJ = (float)(西部.计划GJ ?? 0.0m);
            _全网计划GJ = (float)(全网.计划GJ ?? 0.0m);

            _东部核算GJ = (float)(东部.核算GJ ?? 0.0m);
            _西部核算GJ = (float)(西部.核算GJ ?? 0.0m);
            _全网核算GJ = (float)(全网.核算GJ ?? 0.0m);

            _东部计划热指标 = (float)(东部.计划热指标 ?? 0.0m);
            _西部计划热指标 = (float)(西部.计划热指标 ?? 0.0m);
            _全网计划热指标 = (float)(全网.计划热指标 ?? 0.0m);

            _东部核算热指标 = (float)(东部.核算热指标 ?? 0.0m);
            _西部核算热指标 = (float)(西部.核算热指标 ?? 0.0m);
            _全网核算热指标 = (float)(全网.核算热指标 ?? 0.0m);

            _东部实际运行热指标 = (float)(东部.实际运行热指标 ?? 0.0m);
            _西部实际运行热指标 = (float)(西部.实际运行热指标 ?? 0.0m);
            _全网实际运行热指标 = (float)(全网.实际运行热指标 ?? 0.0m);

            _热源个数 = 热源个数;
            _智能卡站个数 = 智能卡站个数;
            _监控站个数 = 监控站个数;
            _有效站个数 = 有效站个数;

            _总面积 = (float)(area.总供热面积 ?? 0.0m);
            _实际供热面积 = (float)(area.计算用监控面积 ?? 0.0m);

            _今日累计供热量 = 今日累计供热量;
            _昨日累计供热量 = 昨日累计供热量;
        }
    }
}