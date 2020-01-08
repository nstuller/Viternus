using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Viternus.Data.Interface;

namespace Viternus.Data
{
    public partial class Message : IEntity
    {
        public bool DeliverByDate
        {
            get
            {
                return null != ScheduleDate;
            }
        }
        public bool DeliverByEvent
        {
            get
            {
                return null != EventType;
            }
        }
    }
}
