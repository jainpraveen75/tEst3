using Core.INotification;
using Core.Notifications;
using PushSharp.Core;
using PushSharp.Google;
using System;
using Core.Entities.Rights;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Core.Enums;
using logger = Core.Logging;
using System.Web.Script.Serialization;
using Core.IRepository.Rights;
using System.Web.Script.Serialization;

namespace Infrastructure.Notification
{
    public class PushNotification : IPushNotifications, IDisposable
    {
        private readonly logger.ILogger _logger;
        private readonly INotificationsRepository _notificationsRepository;

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
                //_assetsRepository.Dispose();
                _disposed = true;
        }

        private readonly string GCM_SENDER_ID = System.Configuration.ConfigurationSettings.AppSettings["GCM_SENDER_ID"];// "61247653235"; // From Krishan's EMAIL 
        private readonly string GCM_SENDER_AUTHTOKEN = System.Configuration.ConfigurationSettings.AppSettings["GCM_SENDER_AUTHTOKEN"];//"AIzaSyCg6q8doBaaN331upbEyHJ4Br-BgRArhtU";
        private readonly string GCM_SEND_URL1 = System.Configuration.ConfigurationSettings.AppSettings["GCM_SEND_URL1"];//"https://fcm.googleapis.com/fcm/send";
        private readonly string GCM_SENDER_PACKAGNAME = System.Configuration.ConfigurationSettings.AppSettings["GCM_SENDER_PACKAGNAME"];//"com.bsf.eventtracker";



        public PushNotification(logger.ILogger logger, INotificationsRepository notificationsRepository)
        {
            _logger = logger;
            _notificationsRepository = notificationsRepository;
        }
        private NotificationStatus Send(List<UserDetail> UserDetail, string title, string message, int? relatedId, NotificationTypes NType, int notificationId)
        {
            NotificationStatus notificationStatus = new NotificationStatus();
            // Configuration
            var config = new GcmConfiguration(GCM_SENDER_ID, GCM_SENDER_AUTHTOKEN, GCM_SENDER_PACKAGNAME);
            config.GcmUrl = GCM_SEND_URL1;
            // Create a new broker
            var gcmBroker = new GcmServiceBroker(config);

            // Wire up events
            gcmBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {

                aggregateEx.Handle(ex =>
                {

                    // See what kind of exception it was to further diagnose
                    var exception = ex as GcmNotificationException;
                    if (exception != null)
                    {
                        var notificationException = exception;

                        // Deal with the failed notification
                        var gcmNotification = notificationException.Notification;
                        string notificationError  = new JavaScriptSerializer().Serialize(notificationException.Notification);
                        notificationStatus.NotificationException = new Core.Notifications.NotificationException { MessageId = gcmNotification.MessageId, Description = notificationException.Description };
                        notificationStatus.ErrorTypeId = Core.Enums.PushNotificationsErrorType.GcmNotificationException;
                        _logger.Log("Error while Sending notification");
                        _logger.Log(notificationError);
                    }
                    else if (ex is GcmMulticastResultException)
                    {
                        var multicastException = (GcmMulticastResultException)ex;
                        notificationStatus.ErrorTypeId = Core.Enums.PushNotificationsErrorType.GcmMulticastResultException;
                        List<string> lstSucess = new List<string>();
                        List<string> lstFail = new List<string>();
                        List<BsfNotifications> lstFailed = new List<BsfNotifications>();
                        foreach (var succeededNotification in multicastException.Succeeded)
                        {
                            notificationStatus.NotificationSucceededList.Add(new NotificationSucceededList { MessageId = succeededNotification.MessageId, To = succeededNotification.To });
                            lstSucess.Add(succeededNotification.To);
                        }

                        foreach (var failedKvp in multicastException.Failed)
                        {
                            var n = failedKvp.Key;
                            var e = failedKvp.Value;
                            notificationStatus.Failed.Add(new NotificationSucceededList { To = n.To, MessageId = n.MessageId, RegistrationIds = n.RegistrationIds, RestrictedPackageName = n.RestrictedPackageName }, e);
                            _logger.Log("Error while Sending notification failedKvp");
                            _logger.Log(string.Format("to:{0}, MessageId :{1}, RegistrationIds:{2}", n.To, n.MessageId, n.RegistrationIds));
                            _logger.Log(e);
                            //int =int.TryParse(n.Data.GetValue("notificationId")
                            //lstFailed.Add( new BsfNotifications { Id = n.Data.GetValue("notificationId") })
                            //lstFail.Add(n.To);
                            //?n.Data.GetValue("notificationId")
                            //JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                            //data routes_list = (data)json_serializer.DeserializeObject(n.Data.ToString());

                        }

                    }
                    else if (ex is DeviceSubscriptionExpiredException)
                    {
                        var expiredException = (DeviceSubscriptionExpiredException)ex;

                        var oldId = expiredException.OldSubscriptionId;
                        var newId = expiredException.NewSubscriptionId;

                        notificationStatus.ErrorTypeId = Core.Enums.PushNotificationsErrorType.DeviceSubscriptionExpiredException;
                        notificationStatus.OldSubscriptionId = oldId;


                        if (!string.IsNullOrWhiteSpace(newId))
                        {
                            // If this value isn't null, our subscription changed and we should update our database
                            notificationStatus.NewSubscriptionId = newId;

                        }
                        _logger.Log("Error while Sending notification DeviceSubscriptionExpiredException");
                        _logger.Log(string.Format("OldSubscriptionId:{0},NewSubscriptionId{1}", oldId, newId));
                    }
                    else if (ex is RetryAfterException)
                    {
                        var retryException = (RetryAfterException)ex;
                        notificationStatus.ErrorTypeId = Core.Enums.PushNotificationsErrorType.RetryAfterException;
                        // If you get rate limited, you should stop sending messages until after the RetryAfterUtc date
                        notificationStatus.RetryAfterUtc = retryException.RetryAfterUtc;
                        _logger.Log("Error while Sending notification RetryAfterException");
                        _logger.Log("retryException.Message" + retryException.Message);
                    }
                    else
                    {
                        notificationStatus.ErrorTypeId = Core.Enums.PushNotificationsErrorType.Unknown;
                        notificationStatus.Error = "GCM Notification Failed for some unknown reason";
                        _logger.Log("Error while Sending notification ErrorTypeId");
                        _logger.Log(ex);
                        _logger.Log("Else error");

                    }

                    // Mark it as handled
                    return true;
                });
            };

            gcmBroker.OnNotificationSucceeded += (notification) =>
            { };

            // Start the broker
            gcmBroker.Start();

            List<string> lstDeviceTokes = new List<string>();
            //lstDeviceTokes.Add("c6tX91UfBvk:APA91bHhtIfw-ZQqck5K72TsxOXK7EUYEKrHgBB9mNdzz-6MSco8OfSKEPDZgkEroStQoXKhJstyUK7UMyIjOC8kC6ykAy3VZ36pA02CaCb8KflkkSiZ9UOJi-6yYh-rlcN4Ckq536-h");
            foreach (var regId in UserDetail)
            {
                lstDeviceTokes.Add(regId.Device.DeviceToken);
            }
            notification jnoti = new notification { title = title, body = message};
            data jData = new data { title = title, body = message, relatedId = relatedId ?? 0, type = (int)NType };

            

            if (lstDeviceTokes.Count > 0)
            {
                string json = new JavaScriptSerializer().Serialize(jnoti);
                string jsondata = new JavaScriptSerializer().Serialize(jData);
                // Queue a notification to send
                gcmBroker.QueueNotification(new GcmNotification
                {
                    RegistrationIds = lstDeviceTokes,
                    Priority = GcmNotificationPriority.Normal,
                   // Notification = JObject.Parse(json),
                    Data = JObject.Parse(jsondata)
                });
            }

            // Stop the broker, wait for it to finish   
            // This isn't done after every message, but after you're
            // done with the broker
            gcmBroker.Stop();

            return notificationStatus;
        }

        public void SendFCM(List<UserDetail> UserDetail, string title, string message, int? relatedId, NotificationTypes type, int notificationId)
        {
            try
            {
                _logger.Log("calling PushNotification");
                var sts = Send(UserDetail, title, message, relatedId, type, notificationId);
            }
            catch (Exception ex)
            {
                _logger.Log("Error while sending Notification - Start");
                _logger.Log(ex);
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(UserDetail);
                _logger.Log("related ID :"+ relatedId);
                _logger.Log(json);
                _logger.Log("Error while sending Notification - END ");
            }

        }

        private class notification
        {
            public string title { get; set; }
            public string body { get; set; }
            //public string type { get; set; }
            //public int relatedId { get; set; }
            
        }
        private class data
        {
            public string title { get; set; }
            public string body { get; set; }
            public int type { get; set; }
            public int relatedId { get; set; }
            public int notificationId { get; set; }
            

        }
    }
}
