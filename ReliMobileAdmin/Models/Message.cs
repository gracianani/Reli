using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReliMobileAdmin.Helper;

namespace ReliMobileAdmin.Models
{
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
                    }
                    else
                    {
                        _url = _dbMessage.imageUrl;
                    }
                }
            }
        }
        public string SendToUser_Department 
        {
            get
            {
                return CompanyHelper.GetDepartmentId(DBMessage.sendToUser.所属公司, DBMessage.sendToUser.是否为集团员工);
            }
        }
        public string SendFromUserName
        {
            get
            {
                return DBMessage.sendFromUser.email;
            }
        }

        public string SendToUserName
        {
            get
            {
                return DBMessage.sendToUser.email;
            }
        }


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

        private string _url;
        public string imageUrl
        {
            get
            {
                return "http://192.168.57.238:14875/" + _url.ToLower().Replace(@"c:/tests/", "");
                //.Replace(HttpContext.Current.Request.PhysicalApplicationPath, string.Empty);
            }
        }
        public bool hasImage
        {
            get
            {
                return !string.IsNullOrEmpty(DBMessage.imageUrl);
            }
        }

        public IEnumerable<Message> RepliedByMessages
        {
            get
            {
                return DBMessage.repliedByMessages.Select(i => new Message() { DBMessage = i } );
            }
        }

        public Message()
        {
        }

        public Message(message dbMessage)
        {
            DBMessage = dbMessage;
        }
    }
}