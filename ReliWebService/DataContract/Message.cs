using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using ReliWebService.Repository;

namespace ReliWebService
{
    [DataContract]
    public class Message
    {
        private message _dbMessage;
        public message DBMessage
        {
            get
            {
                if (_dbMessage == null)
                {
                    _dbMessage = new message();
                }
                return _dbMessage;
            }
            set
            {
                _dbMessage = value;
                if (!string.IsNullOrEmpty(_dbMessage.imageUrl))
                {
                    var url_uri = _dbMessage.imageUrl.Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
                    if (url_uri.Count() == 2)
                    {
                        _url = url_uri[0];
                        _uri = url_uri[1];
                    }
                    else
                    {
                        _url = _dbMessage.imageUrl;
                    }
                }
            }
        }
        public string SendFromUserName
        {
            get
            {
                return DBMessage.SendFromUser.email;
            }
        }

        public string SendToUserName {
            get {
                return DBMessage.SendToUser.email;
            }
        }

        

        

        [DataMember]
        public string messageContent
        {
            get
            {
                return DBMessage.messageContent;
            }
            set
            {
                DBMessage.messageContent = value;
            }
        }

        [DataMember]
        public int direction
        {
            get
            {
                if (DBMessage.sendFromUserId == 1)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            set { }
        }
        [DataMember]
        public int messageId
        {
            get
            {
                return DBMessage.messageId;
            }
            set
            {
            }
        }

        public DateTime CreatedAt
        {
            get
            {
                return DBMessage.createdAt;
            }
            set
            {
                DBMessage.createdAt = value;
            }
        }

        [DataMember]
        public string createdAt
        {
            get
            {
                return DBMessage.createdAt.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
            }
        }

        public int sendFromUserId
        {
            get
            {
                return DBMessage.sendFromUserId;
            }
            set
            {
                DBMessage.sendFromUserId = value;
            }
        }
        [DataMember]
        public int sendToUserId
        {
            get
            {
                return DBMessage.sendToUserId;
            }
            set
            {
                DBMessage.sendToUserId = value;
            }
        }

        [DataMember]
        public string imageUrl
        {
            get
            {
                return DBMessage.imageUrl;
            }
            set
            {
                DBMessage.imageUrl = value;
            }
        }
        private string _uri;
        [DataMember]
        public string imageUri
        {
            get
            {
                return _uri;
            }
            set
            {
                _uri = value;
            }
        }

        private string _url;
        public string fileImageUrl
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
            }
        }
        public Message()
        {
        }
        public Message( message dbMessage )
        {
            DBMessage = dbMessage;
        }
    }
}