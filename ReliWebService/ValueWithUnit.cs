using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ReliWebService
{
    // TODO: Edit the SampleItem class
    public class ValueWithUnit
    {
        public string Unit { get; set; }
        public decimal Value { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
