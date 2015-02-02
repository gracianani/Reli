using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReliWebService.Repository
{
    public class MessageRepository
    {
        private ReliMobileEntities db = new ReliMobileEntities();
        private List<Message> _messages;
        public List<Message> Messages
        {
            get
            {
                return _messages;
            }
            set
            {
                _messages = value;
            }
        }

        public MessageRepository()
        {
            _messages = new List<Message>();
            _messages = db.messages1.Select(i => new Message { DBMessage = i } ).ToList();
        }

        public void Insert(Message message)
        {
            db.messages1.AddObject(message.DBMessage);
            db.SaveChanges();
        }

        public void Update(Message message )
        {
            db.messages1.ApplyCurrentValues(message.DBMessage);
            db.SaveChanges();
        }
    }
}