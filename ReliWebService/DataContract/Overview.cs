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
        public float instanceHeat0
        {
            get
            {
                return _瞬时热量[0];
            }
            set
            {
            }
        }
        [DataMember]
        public float instanceHeat1
        {
            get
            {
                return _瞬时热量[1];
            }
            set
            {
            }
        }
        [DataMember]
        public float instanceHeat2
        {
            get
            {
                return _瞬时热量[2];
            }
            set
            {
            }
        }
        [DataMember]
        public float area0
        {
            get
            {
                return _面积[0];
            }
            set
            {
            }
        }
        [DataMember]
        public float area1
        {
            get
            {
                return _面积[1];
            }
            set
            {
            }
        }
        [DataMember]
        public float area2
        {
            get
            {
                return _面积[2];
            }
            set
            {
            }
        }
        [DataMember]
        public float unitHeatLoad0
        {
            get
            {
                return _单耗[0];
            }
            set { }
        }
        [DataMember]
        public float unitHeatLoad1
        {
            get
            {
                return _单耗[1];
            }
            set { }
        }
        [DataMember]
        public float unitHeatLoad2
        {
            get
            {
                return _单耗[2];
            }
            set { }
        }

        [DataMember]
        public float water0
        {
            get
            {
                return _总流量[0];
            }
            set { }
        }
        [DataMember]
        public float water1
        {
            get
            {
                return _总流量[1];
            }
            set { }
        }
        [DataMember]
        public float water2
        {
            get
            {
                return _总流量[2];
            }
            set { }
        }

        [DataMember]
        public float tenk0
        {
            get
            {
                return _万平米流量[0];
            }
            set { }
        }
        [DataMember]
        public float tenk1
        {
            get
            {
                return _万平米流量[1];
            }
            set { }
        }
        [DataMember]
        public float tenk2
        {
            get
            {
                return _万平米流量[2];
            }
            set { }
        }

        [DataMember]
        public float todaysGJ0
        {
            get
            {
                return _今日累计[0];
            }
            set { }
        }
        [DataMember]
        public float todaysGJ1
        {
            get
            {
                return _今日累计[1];
            }
            set { }
        }
        [DataMember]
        public float todaysGJ2
        {
            get
            {
                return _今日累计[2];
            }
            set { }
        }

        [DataMember]
        public float yesterdaysGJ0
        {
            get
            {
                return _昨日累计[0];
            }
            set { }
        }
        [DataMember]
        public float yesterdaysGJ1
        {
            get
            {
                return _昨日累计[1];
            }
            set { }
        }
        [DataMember]
        public float yesterdaysGJ2
        {
            get
            {
                return _昨日累计[2];
            }
            set { }
        }

        [DataMember]
        public float todaysPerdict0
        {
            get
            {
                return _今日预计[0];
            }
            set { }
        }
        [DataMember]
        public float todaysPerdict1
        {
            get
            {
                return _今日预计[1];
            }
            set { }
        }
        [DataMember]
        public float todaysPerdict2
        {
            get
            {
                return _今日预计[2];
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