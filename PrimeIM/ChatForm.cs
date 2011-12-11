using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using agsXMPP.protocol.client;
using agsXMPP.protocol.extensions.chatstates;
using Imgur.Service.Upload.ResponseStructs;
using PrimeIM.CustomControls;
using PrimeIM.Data;
using Message = agsXMPP.protocol.client.Message;

namespace PrimeIM
{
    public partial class ChatForm : Form
    {
        public static readonly ChatForm Singleton = new ChatForm();
        private const string MESSAGE_FORMAT = "{0}: {1}";

        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }

        private ChatForm()
        {
            PimMessageHandler.OnMessage += MessageReceived;
            InitializeComponent();
        }

        //public ChatForm(Buddy buddy, Message msg = null)
        //{
        //    PimMessageHandler.OnMessage += MessageReceived;
        //    InitializeComponent();
        //    AddChat(buddy, msg);
        //}

        void SendEnteredText(object sender, EventArgs e)
        {
            if (messageTextBox.TextLength > 0)
            {
                var currentTab = GetCurrentTab();
                PimMessageHandler.SendChatState(Chatstate.paused, currentTab.Buddy, currentTab.Resource, currentTab.CurrentThread);
            }
        }

        public IEnumerable<Buddy> OpenBuddies
        {
            get { return tabControl.TabPages.Cast<BuddyTab>().Select(t => t.Buddy); }
        }

        //void BuddyListChanged(object sender, BuddyListEventArgs args)
        //{
        //    var tab = GetTabPage(args.Buddy);
        //    if (tab != null)
        //        if (InvokeRequired)
        //            Invoke(new Action(() => UpdateBuddyStatus(args.Buddy, tab))); 
        //}

        //private void UpdateBuddyStatus(Buddy buddy, BuddyTab tab = null)
        //{
        //    if (tab == null)
        //        tab = GetTabPage(buddy);

        //    string status = String.IsNullOrEmpty(buddy.Status) ? buddy.MainPresenceTypeString: buddy.Status;
        //    tab.Text = String.Format("{0} ({1})", tab.Buddy.Name, status);
        //}

        public void AddChat(Buddy buddy, Message msg = null)
        {
            var buddyTab = GetTabPage(buddy);
            if (buddyTab == null)
            {
                buddyTab = msg == null ? new BuddyTab(buddy.Jid) : new BuddyTab(msg.From);
                tabControl.TabPages.Add(buddyTab);
                buddyTab = GetTabPage(buddy);
            }
            
            tabControl.SelectTab(buddyTab);

            if (msg != null)
            {
                buddyTab.Resource = msg.From.Resource;
                buddyTab.AddMessage(msg);
            }

            UpdateTitleText();
        }

        private BuddyTab GetCurrentTab()
        {
            return (tabControl.SelectedTab as BuddyTab);
        }

        private BuddyTab GetTabPage(string buddyEmail)
        {
            return (tabControl.TabPages.Cast<BuddyTab>().
                Where(t => t.Buddy.Email.ToUpper() == buddyEmail.ToUpper())).SingleOrDefault();
        }

        private BuddyTab GetTabPage(Buddy buddy)
        {
            return (tabControl.TabPages.Cast<BuddyTab>().
                Where(t => t.Buddy.Equals(buddy))).SingleOrDefault();
        }

        private void MessageReceived(object sender, Message msg)
        {
            if (InvokeRequired)
                Invoke(new Action(() => AddChat(BuddyList.Instance.Get(msg.From), msg)));
            else
                AddChat(BuddyList.Instance.Get(msg.From)); //.From.Bare.ToLower()), msg);

            if (msg.Type != MessageType.chat) 
                return;

            if (InvokeRequired && !Visible)
            {
                Invoke(new Action(Show));
            }
            else if (!Visible)
                Show();
        
        }


        private string FormatInboundMessage(Message msg, Buddy buddy)
        {
            //var time = msg.XDelay == null ? DateTime.Now : msg.XDelay.Stamp;
            return String.Format(MESSAGE_FORMAT, buddy.Name, msg.Body);
        }

        private string FormatOutboundMessage(string message)
        {
            return String.Format(MESSAGE_FORMAT, "Me", message);
        }

        private void MessageTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            string msg = messageTextBox.Text.Trim();
            e.Handled = true;
            e.SuppressKeyPress = true;

            //BuddyTab tab;
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Dispose();
                    break;
                case Keys.Tab:
                    MessageTextBoxKeyDown_HandleTabKey();
                    break;
                case Keys.Enter:
                    if (!String.IsNullOrEmpty(msg) && !e.Control)
                    {
                        MessageTextBoxKeyDown_HandleEnterKey(msg);
                    }
                    break;
                case Keys.V:
                    if (!e.Control)
                        goto default;
                    e.Handled = Clipboard.ContainsImage() || Clipboard.ContainsText();
                        
                    if (!e.Handled)
                        return;
                    MessageTextBoxKeyDown_HandleCtrlV();
                    break;
                default:
                    e.Handled = false;
                    e.SuppressKeyPress = false;
                    break;
            }
        }

        private void MessageTextBoxKeyDown_HandleCtrlV()
        {
            if (Clipboard.ContainsImage())
            {
                BuddyTab tab = GetCurrentTab();
                PimMessageHandler.SendImage(tab.Buddy, Clipboard.GetImage(), AddImage);
            }
            else if (Clipboard.ContainsText())
            {
                messageTextBox.Text += Clipboard.GetText();
            }
        }

        private void MessageTextBoxKeyDown_HandleEnterKey(string msg)
        {
            BuddyTab tab = GetCurrentTab();
            messageTextBox.Clear();
            tab.SendMessage(FormatOutboundMessage(msg));
        }

        private void MessageTextBoxKeyDown_HandleTabKey()
        {
            if (tabControl.TabCount <= 1) 
                return;
            
            int sel = tabControl.SelectedIndex == tabControl.TabPages.Count - 1
                          ? 0
                          : tabControl.SelectedIndex + 1;
            tabControl.SelectTab(sel);
        }

        private void AddImage(Buddy buddy, ImgurResponse imgurResponse)
        {
            if (imgurResponse == null)
                return;

            BuddyTab tab = GetTabPage(buddy);

            if (tab == null)
                return;

            tab.AddImage(imgurResponse.UploadLinks.DirectLink);
        }

        private void TabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTitleText();
        }

        private void UpdateTitleText()
        {
            var tab = GetCurrentTab();
            if (tab != null)
                Text = tab.Buddy.Name;
            
        }


        private void MessageTextBoxTextChanged(object sender, EventArgs e)
        {
            Chatstate state = messageTextBox.TextLength == 0 ? Chatstate.inactive : Chatstate.composing;
            var currentTab = GetCurrentTab();
            currentTab.Buddy.SendChatState(state, currentTab.Resource, currentTab.CurrentThread);
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            tabControl.TabPages.Clear();
            Hide();
            e.Cancel = true;
        }
    }
}
