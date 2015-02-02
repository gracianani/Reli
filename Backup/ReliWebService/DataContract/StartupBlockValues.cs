using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [CollectionDataContract]
    public class StartupBlockValues : List<string>
    {
        public StartupBlockValues() { }
        public StartupBlockValues(List<string> startupBlockValues) : base(startupBlockValues) {}
    }
}