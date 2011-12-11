using System;
using System.Drawing;
using System.Threading;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.extensions.chatstates;
using agsXMPP.protocol.iq.roster;
using agsXMPP.Xml.Dom;
using PrimeIM.Data;

namespace PrimeIM
{
    public class PimMessageHandler
    {
        private static readonly XmppClientConnection XmppClient = new XmppClientConnection();
        
        public static bool Authenticated
        {
            get { return XmppClient == null ? false : XmppClient.Authenticated; }
        }

        public static Jid MyJid
        {
            get { return XmppClient.MyJID; }
        }

        /// <summary>
        /// Login action. Performed in a separate thread to avoid UI lock.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="successCallback">The method you want to trigger when the login is successful</param>
        /// <param name="authenticationErrorCallback">The method you want to trigger when authentication fails</param>
        /// <param name="rosterEndCallback">The method you want to trigger when the roster has completely downloaded.</param>
        public static void Login(string username, string password, 
            Action<object> successCallback = null, 
            Action<object, Element> authenticationErrorCallback = null, 
            Action<object> rosterEndCallback = null)
        {
            if (authenticationErrorCallback != null)
                XmppClient.OnAuthError += new XmppElementHandler(authenticationErrorCallback);
            if (successCallback != null)
                XmppClient.OnLogin += new ObjectHandler(successCallback);
            if (rosterEndCallback != null)
                XmppClient.OnRosterEnd += new ObjectHandler(rosterEndCallback);
            
            AddEvents();
            
            XmppClient.AutoPresence = true;
            XmppClient.AutoRoster = true;
            XmppClient.AutoAgents = true;
            XmppClient.Username = username;
            XmppClient.Server = "gmail.com";
            XmppClient.Password = password;
            XmppClient.AutoResolveConnectServer = true;
            XmppClient.UseStartTLS = true;
            XmppClient.UseSSL = false;
            ThreadPool.QueueUserWorkItem(delegate{XmppClient.Open();}); 
        }

        private static void AddEvents()
        {
            XmppClient.OnPresence += XmppClientOnPresence;
            XmppClient.OnRosterItem += XmppClientOnRosterItem;
            XmppClient.OnError += XmppClientOnError;
            XmppClient.OnLogin += PresenceChanged;
            XmppClient.OnClose += PresenceChanged;
        }

        static void PresenceChanged(object sender)
        {
            SendMyPresence();
        }
        
        static void XmppClientOnError(object sender, Exception ex)
        {
            throw new Exception(ex.ToString());
        }

        private static void XmppClientOnPresence(object sender, Presence presence)
        {
            BuddyList.SetBuddyPresence(presence);
        }

        private static void XmppClientOnRosterItem(object sender, RosterItem item)
        {
            BuddyList.AddBuddyIfNotExists(item);
        }

        public static void SendMessage(Buddy buddy, string messageBody)
        {
            Message msg = new Message(buddy.Email, MyJid, MessageType.chat, messageBody, String.Empty, buddy.GetThread());
            SendMessage(buddy, msg);
        }

        public static void SendMessage(Buddy buddy, Message message)
        {
            buddy.UpdateThreadExpiration();
            ThreadPool.QueueUserWorkItem(delegate { XmppClient.Send(message); });
        }

        public static void SendChatState(Chatstate state, Buddy buddy)
        {
            Message msg = new Message(buddy.Jid);
            msg.Chatstate = state;
            ThreadPool.QueueUserWorkItem(delegate { XmppClient.Send(msg); });
        }

        public static void SendMyPresence()
        {
            ThreadPool.QueueUserWorkItem(delegate { XmppClient.SendMyPresence(); });
        }

        public static void Logout()
        {
            XmppClient.Close();
        }

        public static void SendImage(Buddy buddy, Image img)
        {
            throw new NotImplementedException();
        }

        public enum MessageSendType
        {
            Image, Text
        }

        #region EventHandlers
        public static event ObjectHandler OnRosterStart
        {
            add { XmppClient.OnRosterStart += value; }
            remove { XmppClient.OnRosterStart -= value; }
        }

        public static event ObjectHandler OnRosterEnd
        {
            add { XmppClient.OnRosterEnd += value; }
            remove { XmppClient.OnRosterEnd -= value; }
        }

        public static event XmppClientConnection.RosterHandler OnRosterItem
        {
            add { XmppClient.OnRosterItem += value; }
            remove { XmppClient.OnRosterItem -= value; }
        }

        public static event PresenceHandler OnPresence
        {
            add { XmppClient.OnPresence += value; }
            remove { XmppClient.OnPresence -= value; }
        }

        public static event ErrorHandler OnError
        {
            add { XmppClient.OnError += value; }
            remove { XmppClient.OnError -= value; }
        }

        public static event ObjectHandler OnClose
        {
            add { XmppClient.OnClose += value; }
            remove { XmppClient.OnClose -= value; }
        }

        public static event MessageHandler OnMessage
        {
            add { XmppClient.OnMessage += value; }
            remove { XmppClient.OnMessage -= value; }
        }

        #endregion

    }
}
