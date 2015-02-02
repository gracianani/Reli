using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace ReliWebService
{
    [DataContract]
    public class Overview
    {
        private float[] _瞬时热量;
        private float[] _面积;
        private float[] _单耗;
        private float[] _总流量;
        private float[] _万平米流量;
        private float[] _今日累计;
        private float[] _昨日累计;
        private float[] _今日预计;
        
        [DataMember]
        public float[] instanceHeat{
            get
            {
                return _瞬时热量;
            }
            set
            {
            }
        }
        [DataMember]
        public float[] area
        {
            get
            {
                return _面积;
            }
            set
            {
            }
        }
        [DataMember]
        public float[] unitHeatLoad{
            get {
                return _单耗;
            }
            set {}
        }

        [DataMember]
        public float[] water
        {
            get
            {
                return _总流量;
            }
            set { }
        }

        [DataMember]
        public float[] tenk
        {
            get
            {
                return _万平米流量;
            }
            set { }
        }

        [DataMember]
        public float[] todaysGJ
        {
            get
            {
                 return _今日累计;
            }
            set { }
        }

        [DataMember]
        public float[] yesterdaysGJ
        {
            get
            {
                return _昨日累计;
            }
            set { }
        }

        [DataMember]
        public float[] todaysPerdict
        {
            get
            {
                return  _今日预计;
            }
            set { }
        }


        public Overview(float[] 瞬时热量, float[] 面积, float[] 单耗,
            float[] 总流量, float[] 万平米流量, float[] 今日累计,
             float[] 昨日累计, float[] 今日预计)
        {
            _瞬时热量 = 瞬时热量;
            _面积 = 面积;
            _单耗 = 单耗;
            _总流量 = 总流量;
            _万平米流量 = 万平米流量;
            _今日累计 = 今日累计;
            _昨日累计 = 昨日累计;
            _今日预计 = 今日预计;
        }
    }
}