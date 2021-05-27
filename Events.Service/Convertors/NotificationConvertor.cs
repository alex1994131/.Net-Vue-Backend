using Events.Core.Models;
using Events.Core.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Service.Convertors
{
    class NotificationConvertor : EConvertor<Notification, NotificationView>
    {
        public NotificationView Convert(Notification entity)
        {
            //entity.
            NotificationView view = new NotificationView();
            //view.Title = 
            return view;
        }

        public Notification Reverse(NotificationView entity)
        {
            throw new NotImplementedException();
        }
    }
}
