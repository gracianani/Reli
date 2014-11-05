using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ReliMobileAdmin.Models
{
    [DataContract]
    public class MessageInsertResponse
    {
        [DataMember]
        public bool IsInserted { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public DateTime UpdatedAt { get; set; }

        [DataMember]
        public string MessageContent { get; set; }

        [DataMember]
        public string ReplyTo { get; set; }

        [DataMember]
        public string PageContent { get; set; }
    }
}