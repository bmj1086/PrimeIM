using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Timers;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.extensions.chatstates;
using agsXMPP.protocol.iq.roster;
using agsXMPP.Xml.Dom;
using Imgur.Service.Upload;
using Imgur.Service.Upload.ResponseStructs;
using PrimeIM.Data.Comparers;
using Utility;

namespace PrimeIM.Data
{
    public class PimMessageHandler
    {
        private static readonly XmppClientConnection XmppClientSingleton = new XmppClientConnection();
        private static HashSet<RosterItem> RosterReserve = new HashSet<RosterItem>(RosterItemComparer.Instance);
        private static HashSet<Presence> PresenceReserve = new HashSet<Presence>(PresenceComparer.Instance);

        public static bool Authenticated
        {
            get { return XmppClientSingleton != null && XmppClientSingleton.Authenticated; }
        }

        public static Jid MyJid
        {
            get { return XmppClientSingleton.MyJID; }
        }
        
        public static void Login(string username, string password, 
            Action<object> successCallback, Action<object, Element> authenticationErrorCallback)
        {
            AddEvents(successCallback, authenticationErrorCallback);

            XmppClientSingleton.Username = username;
            XmppClientSingleton.Server = "gmail.com";
            XmppClientSingleton.Password = password;
            XmppClientSingleton.AutoResolveConnectServer = true;
            XmppClientSingleton.UseStartTLS = true;
            XmppClientSingleton.UseSSL = false;
            XmppClientSingleton.Open();
        }

        private static void AddEvents(Action<object> successCallback, Action<object, Element> authenticationErrorCallback)
        {
            XmppClientSingleton.OnAuthError += new XmppElementHandler(authenticationErrorCallback);
            XmppClientSingleton.OnLogin += new ObjectHandler(successCallback);
            XmppClientSingleton.OnRosterItem += XmppClient_RosterItemReceived;
            XmppClientSingleton.OnPresence += XmppClientSingleton_OnPresence;
            XmppClientSingleton.OnError += XmppClientSingletonOnError;
            XmppClientSingleton.OnLogin += PresenceChanged;
            XmppClientSingleton.OnClose += PresenceChanged;
        }

        static void XmppClient_RosterItemReceived(object sender, RosterItem item)
        {
            if (item.Subscription != SubscriptionType.both)
                return;

            BuddyList.Instance.Add(item);
        }

        static void PresenceChanged(object sender)
        {
            SendMyPresence();
        }
        
        static void XmppClientSingletonOnError(object sender, Exception ex)
        {
            throw new Exception("An error occured in agsXMPP.", ex);
        }

        private static void XmppClientSingleton_OnPresence(object sender, Presence presence)
        {
            BuddyList.Instance.HandlePresence(presence);
        }

        public static void SendMessage(Buddy buddy, string messageBody, string resource, string thread)
        {
            Message msg = new Message(buddy.Jid, MyJid, MessageType.chat, messageBody, String.Empty, thread ?? string.Empty);
            msg.To.Resource = resource;
            SendMessage(buddy, msg);
        }

        public static void SendMessage(Buddy buddy, Message message)
        {
            message.From = MyJid;
            XmppClientSingleton.Send(message);
        }

        public static void SendChatState(Chatstate state, Buddy buddy, string resource, string thread)
        {
            Message msg = new Message(buddy.Jid);
            msg.Thread = thread;
            msg.From = MyJid;
            msg.Chatstate = state;
            XmppClientSingleton.Send(msg);
        }

        public static void SendMyPresence()
        {
            XmppClientSingleton.SendMyPresence();
        }

        public static void Logout()
        {
            XmppClientSingleton.Close();
        }

        public static void SendImage(Buddy buddy, Image img, Action<Buddy, ImgurResponse> callBack)
        {
            using (BackgroundWorker backgroundWorker = new BackgroundWorker())
            {
                backgroundWorker.RunWorkerCompleted += (sender, e) => callBack(buddy, e.Result as ImgurResponse);
                backgroundWorker.DoWork += (sender, e) => { e.Result = ImgurUploader.PostToImgur(img); };
                backgroundWorker.RunWorkerAsync();
            }
        }

        public enum MessageSendType
        {
            Image, Text
        }

        #region EventHandlers
        public static event ObjectHandler OnRosterStart
        {
            add { XmppClientSingleton.OnRosterStart += value; }
            remove { XmppClientSingleton.OnRosterStart -= value; }
        }

        public static event ObjectHandler OnRosterEnd
        {
            add { XmppClientSingleton.OnRosterEnd += value; }
            remove { XmppClientSingleton.OnRosterEnd -= value; }
        }

        //public static event XmppClientConnection.RosterHandler OnRosterItem
        //{
        //    add { XmppClientSingleton.OnRosterItem += value; }
        //    remove { XmppClientSingleton.OnRosterItem -= value; }
        //}

        public static event PresenceHandler OnPresence
        {
            add { XmppClientSingleton.OnPresence += value; }
            remove { XmppClientSingleton.OnPresence -= value; }
        }

        public static event ErrorHandler OnError
        {
            add { XmppClientSingleton.OnError += value; }
            remove { XmppClientSingleton.OnError -= value; }
        }

        public static event ObjectHandler OnClose
        {
            add { XmppClientSingleton.OnClose += value; }
            remove { XmppClientSingleton.OnClose -= value; }
        }

        public static event MessageHandler OnMessage
        {
            add { XmppClientSingleton.OnMessage += value; }
            remove { XmppClientSingleton.OnMessage -= value; }
        }

        #endregion

    }
}
