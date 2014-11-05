using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [CollectionDataContract]
    public class Warnings : List<Warning>
    {
        public Warnings() { }
        public Warnings(List<Warning> warnings) : base(warnings) { }
    }
}