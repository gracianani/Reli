using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [CollectionDataContract]
    public class StartupBlocks : List<StartupBlock>
    {
        public StartupBlocks() { }
        public StartupBlocks(List<StartupBlock> startupBlocks)
            : base(startupBlocks) { }
    }
}