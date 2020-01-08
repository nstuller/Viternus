using System;
using System.Linq;
using System.Web.Mvc;
using Viternus.Data;
using Viternus.Data.Repository;
using Viternus.DeliveryAutomation;
using Viternus.Web.Filters;
using Viternus.Web.ViewModels;

namespace Viternus.Web.Controllers
{

    [LogError]
    public class MessageController : ApplicationController
    {
        private MessageRepository _msgRepository = new MessageRepository();
        private MessageRecipientRepository _msgRecipientRepository = new MessageRecipientRepository();
        private ProfileRepository _profileRepository = new ProfileRepository();


        [Authorize(Roles = "Admin")]
        public ActionResult DeliveryAdmin()
        {
            return View();
        }

        public ActionResult MarkEventMessagesForDelivery(bool? scheduled)
        {
            //This is an admin function but we need to be able to automatically schedule it
            bool schedParam = scheduled ?? false;
            if (User.IsInRole("Admin") || schedParam)
            {
                DeliveryProcess dp = new DeliveryProcess();
                dp.MarkEventMessagesForDelivery();

                dp.DeliverOverdueMessages();
                return Content("Success");
            }

            return Content("Not Authorized");
        }

        public ActionResult DeliverOverdueMessages(bool? scheduled)
        {
            //This is an admin function but we need to be able to automatically schedule it
            bool schedParam = scheduled ?? false;
            if (User.IsInRole("Admin") || schedParam)
            {
                DeliveryProcess dp = new DeliveryProcess();
                dp.DeliverOverdueMessages();
                return Content("Success");
            }
            
            return Content("Not Authorized");
        }

        //
        // GET: /Message/
        [Authorize()]
        public ActionResult Index()
        {
            return View(_msgRepository.FindAllMessagesByFromUser(User.Identity.Name).ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult IndexAll()
        {
            return View(_msgRepository.GetAll().ToList());
        }

        //
        // GET: /Message/Details/5
        [Authorize()]
        public ActionResult Details(Guid id)
        {
            Message msg = _msgRepository.GetById(id);
            TempData["VideoSecurity"] = "MessagePreview";

            if (null == msg)
                return View("NotFound");
            else
                return View(new MessageDisplayViewModel(msg, User.Identity.Name));
        }

        //
        // POST: /Message/Create
        [Authorize()]
        public ActionResult Create()
        {
            Message msg = _msgRepository.New();
            msg.ScheduleDate = DateTime.Now.AddDays(7);
            return View(new MessageEditViewModel(msg, User.Identity.Name));
        }

        [Authorize()]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string messageType, string video, string title, string comments, string deliverBy, string scheduleDate, string deliveryOffset, string eventType)
        {
            Message msg = null;
            try
            {
                msg = SetFormValuesForSaving(null, messageType, video, title, comments, deliverBy, scheduleDate, deliveryOffset, eventType);

                return RedirectToAction("AddRecipients", new { id = msg.Id });
            }
            catch
            {
                //ModelState.AddRuleViolations(msg.GetRuleViolations());
            }
            return View(new MessageEditViewModel(msg, User.Identity.Name));
        }

        [Authorize()]
        public ActionResult Edit(Guid id)
        {
            Message msg = _msgRepository.GetById(id);

            if (null == msg)
                return View("NotFound");
            else
                return View(new MessageEditViewModel(msg, User.Identity.Name));
        }

        [Authorize()]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Guid id, string messageType, string video, string title, string comments, string deliverBy, string scheduleDate, string deliveryOffset, string eventType)
        {
            Message msg = null;
            try
            {
                msg = SetFormValuesForSaving(id, messageType, video, title, comments, deliverBy, scheduleDate, deliveryOffset, eventType);

                return RedirectToAction("AddRecipients", new { id = id });
            }
            catch (Exception)
            {
                //ModelState.AddRuleViolations(msg.GetRuleViolations());

                return View(new MessageEditViewModel(msg, User.Identity.Name));
            }
        }

        [Authorize()]
        public ActionResult AddRecipients(Guid id)
        {
            Message msg = _msgRepository.GetById(id);

            return View(msg);
        }

        [Authorize()]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddRecipients(Guid id, string email, string firstName, string lastName)
        {
            Message msg = _msgRepository.GetById(id);

            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("email", "Please enter an email address.  It is required.");
                return View(msg);
            }

            Profile profile = _profileRepository.GetByEmail(email);
            MessageRecipient msgReceiver = _msgRecipientRepository.GetByEmailAndMessageId(email, id);

            if (null == profile)
            {
                profile = _profileRepository.New(firstName, lastName, String.Empty, email);
            }

            if (null == msgReceiver)
            {
                msgReceiver = _msgRecipientRepository.New();
            }

            _msgRecipientRepository.AddMessageRecipientToProfile(msgReceiver, profile);
            _msgRecipientRepository.AddMessageRecipientToMessage(msgReceiver, msg);
            _msgRecipientRepository.Save();

            return View(msg);
        }

        [Authorize()]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteMessageRecipient(Guid id)
        {
            MessageRecipient msgRecip = null;
            try
            {
                msgRecip = _msgRecipientRepository.GetById(id);

                if (msgRecip == null)
                    return View("NotFound");

                //Get the message Id before we delete it (because this reference will go away
                Guid messageId = msgRecip.Message.Id;

                _msgRecipientRepository.Delete(msgRecip);

                return RedirectToAction("AddRecipients", new { id = messageId });
            }
            catch (Exception)
            {
                //ModelState.AddRuleViolations(msg.GetRuleViolations());

                return View();
            }
        }

        private Message SetFormValuesForSaving(Guid? id, string messageType, string video, string title, string comments, string deliverBy, string scheduleDate, string deliveryOffset, string eventType)
        {
            //TODO: Extended business logic like this should go in a service/business layer
            Message msg;
            if (null != id)
                msg = _msgRepository.GetById((Guid)id);
            else
                msg = _msgRepository.New();

            //Get MessageType from Repository and add this message to it
            Guid messageTypeId = new Guid(messageType);
            _msgRepository.AddMessageToMessageType(msg, messageTypeId);

            //If it's a Video Message and the Video was Selected
            if (null != video && "Video" == msg.MessageType.Description)
            {
                Guid videoId = new Guid(video);
                _msgRepository.AddMessageToVideo(msg, videoId);
            }
            else
            {
                msg.Video = null;
            }

            _msgRepository.AddMessageToUser(msg, User.Identity.Name);

            msg.Title = title;
            msg.Comments = comments;

            //Here's some biz logic - they can only deliver by either date or event
            if (deliverBy.Equals("byDate"))
            {
                msg.ScheduleDate = DateTime.Parse(scheduleDate);
                msg.EventType = null;
                msg.EventScheduleOffsetDays = null;
            }
            else
            {
                msg.ScheduleDate = null;
                msg.EventScheduleOffsetDays = short.Parse(deliveryOffset);
                Guid eventTypeId = new Guid(eventType);
                _msgRepository.AddMessageToEventType(msg, eventTypeId);
            }

            _msgRepository.Save();
            return msg;
        }

        [Authorize()]
        public ActionResult Delete(Guid id)
        {
            Message msg = _msgRepository.GetById(id);

            if (msg == null)
                return View("NotFound");
            else
                return View(msg);
        }

        [Authorize()]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Guid id, string confirmButton)
        {
            Message msg = _msgRepository.GetById(id);

            if (msg == null)
                return View("NotFound");

            _msgRepository.Delete(msg);

            return View("Deleted");
        }

        /// <summary>
        /// Displays a message meant for a specific recipient
        /// </summary>
        /// <remarks>Does not require any authentication by default</remarks>
        /// <param name="id">The MessageRecipient Id</param>
        /// <returns></returns>
        public ActionResult ViewMessageFromUrl(Guid? id)
        {
            if (null == id)
                return View("NotFound");

            MessageRecipient msgRecipient = _msgRecipientRepository.GetByIdIfNotSent(id.Value);
            TempData["VideoSecurity"] = "MessageFromUrl";

            if (null == msgRecipient)
                return View("NotFound");
            else
            {
                //Record that this message was visited by this recipient
                msgRecipient.VisitedDate = DateTime.Now;
                _msgRecipientRepository.Save();

                return View(new MessageRecipientDisplayViewModel(msgRecipient, User.Identity.Name));
            }
        }
    }
}
