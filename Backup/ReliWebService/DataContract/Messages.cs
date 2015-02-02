using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliWebService
{
    [CollectionDataContract]
    public class Messages : List<Message>
    {
        public Messages() { }
        public Messages(List<Message> messages)
            : base(messages) { }
    }
}