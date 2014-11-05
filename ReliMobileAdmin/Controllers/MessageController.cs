using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReliMobileAdmin.Models;
using ReliMobileAdmin.Helper;
using Newtonsoft.Json;

namespace ReliMobileAdmin.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        public static int PageSize = 3;

        ReliMobileEntities context;

        public MessageController()
        {
            context = new ReliMobileEntities();
        }

        public ActionResult Index(int pageIndex = 0, DateTime? fromDate = null, DateTime? toDate = null )
        {
            var messages = context.messages.Where(i=>
                !i.replyToMessageId.HasValue &&
                (!fromDate.HasValue || fromDate.HasValue && i.createdAt > fromDate.Value) &&
                (!toDate.HasValue || toDate.HasValue && i.createdAt < toDate.Value)
                ).Select(i => new Message() { DBMessage = i }).ToList().OrderBy(i=>i.CreatedAt);
            var totalPages = Convert.ToInt32( Math.Ceiling(messages.Count() / Convert.ToDecimal(PageSize)) ) ;
            return View(new MessageViewModel { Messages = messages.Skip(pageIndex*PageSize).Take(PageSize).ToList(), TotalPages = totalPages, PageIndex = pageIndex });
        }

        public ActionResult Create()
        {
            var createMessageMV = new CreateMessageViewModel()
            {
                Users = context.users.Select( i => new User() { UserId = i.userId, UserName=i.姓名 } )
            };
            return View(createMessageMV);
        }

        [HttpPost]
        public ActionResult Create(CreateMessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUserId > 0)
                {
                    var message = new message()
                    {
                        sendFromUserId = UserHelper.AdminId,
                        sendToUserId = model.SelectedUserId,
                        messageContent = model.Message,
                        createdAt = DateTime.Now
                    };
                    context.AddObject("messages", message);
                    context.SaveChanges();
                    model.IsCreated = true;
                    model.Message = "";
                }
            }
            model.Users = context.users.Select(i => new User() { UserId = i.userId, UserName = i.姓名 });
            return View(model);
        }

        [HttpDelete]
        public ActionResult Delete( int messageId )
        {
            var message = context.messages.Single(i => i.messageId == messageId);
            context.DeleteObject(message);
            context.SaveChanges();
            return new JsonResult() { Data = "{ \" isDeleted \" : true } " };
        }

        [HttpPost]
        public PartialViewResult ReplyMessage(string messageContent, int replyToMessageId)
        {
            var replyToMessage = context.messages.Single(i => i.messageId == replyToMessageId);
            var message = new message() { sendFromUserId = UserHelper.AdminId, sendToUserId = replyToMessage.sendFromUserId, messageContent = messageContent, replyToMessageId = replyToMessageId, createdAt=DateTime.Now };
            context.AddObject("messages", message);
            context.SaveChanges();

            var messageInsertResponse = new MessageInsertResponse() {
                 IsInserted = true, MessageContent = messageContent, ReplyTo = replyToMessage.sendFromUser.email, UpdatedAt = DateTime.Now};
            var pv = PartialView();
            
            var messageVM = new Message(message);
            return PartialView("_RepliedMessageView", messageVM);
        }

    }
}
