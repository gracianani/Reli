
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [DataContract]
    public class StartupBlock
    {
        private StartupBlockType _startupBlockType;
        
        public StartupBlockType TheStartupBlockType
        {
            get { return _startupBlockType; }
            set { _startupBlockType = value; }
        }

        private string _startupBlockName;
        private List<string> _startupBlockValues;

        public List<string> StartupBlockValues
        {
            get
            {
                return _startupBlockValues;
            }
            set
            {
                _startupBlockValues = value;
            }
        }
        [DataMember]
        public int startupBlockType
        {
            get { return (int)_startupBlockType; }
            set { _startupBlockType = (StartupBlockType)value; }
        }
        
        [DataMember]
        public string startupBlockName
        {
            get
            {
                return _startupBlockName;
            }
            set
            {
                _startupBlockName = value;
            }
        }

        
        [DataMember]
        public StartupBlockValues startupBlockValues
        {
            get
            {
                return new StartupBlockValues(_startupBlockValues);
            }
            set
            {
                _startupBlockValues = value;
            }
        }

        public StartupBlock( StartupBlockType startupBlockType, string startupBlockName)
        {
            _startupBlockType = startupBlockType;
            _startupBlockName = startupBlockName;
            _startupBlockValues = new List<string>();
        }
    }

}