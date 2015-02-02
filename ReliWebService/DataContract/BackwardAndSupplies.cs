using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [CollectionDataContract]
    public class BackwardAndSupplies: List<BackwardAndSupply>
    {
        public BackwardAndSupplies() { }
        public BackwardAndSupplies(List<BackwardAndSupply> backwardAndSupplies)
            : base(backwardAndSupplies) { }
    }
}