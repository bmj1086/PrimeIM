using System;
using System.Drawing;
using System.Windows.Forms;
using agsXMPP;
using PrimeIM.Data;
using Message = agsXMPP.protocol.client.Message;
using Uri = System.Uri;

namespace PrimeIM.CustomControls
{
    class BuddyTab : TabPage
    {
        private readonly RichTextBox convoTextBox = new RichTextBox();
        private readonly Jid from;
        private readonly Buddy originalBuddy;
        public string Resource = string.Empty;
        private DateTime threadExpires;

        private string currentThread;

        public string CurrentThread
        {
            get
            {
                if (DateTime.Now > threadExpires || String.IsNullOrEmpty(currentThread))
                    SetThread(new Message().CreateNewThread());
                return currentThread;
            }
            set { currentThread = value; }
        }

        public Buddy Buddy
        {
            get
            {
                return BuddyList.Instance.Get(from) ?? originalBuddy;
            }
        }
        private const string IMG_CONVO_FMT_IN = "<a href=\"{0}\"><img src=\"{0}\" width=\"100\"></a>";

        public BuddyTab(Jid jid)
        {
            from = jid;
            originalBuddy = BuddyList.Instance.Get(from);
            InitializeComponents();
        }

        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }

        public void AddMessage(Message message)
        {
            this.Resource = message.From.Resource;
            SetThread(message.Thread ?? message.CreateNewThread());
            AddConvoMessage(message.Body);
        }

        public void SetThread(string threadId)
        {
            UpdateThreadExpiration();
            currentThread = threadId;
        }

        public void UpdateThreadExpiration()
        {
            threadExpires = DateTime.Now.AddMinutes(10);
        }

        private void InitializeComponents()
        {
            convoTextBox.BorderStyle = BorderStyle.None;
            convoTextBox.Dock = DockStyle.Fill;
            convoTextBox.Font = new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular);
            convoTextBox.Location = new Point(3, 3);
            convoTextBox.Name = "convoListBox";
            convoTextBox.Size = new Size(330, 154);
            convoTextBox.TabStop = false;
            convoTextBox.TextChanged += ConvoTextBoxTextChanged;
            // 
            // tabPage1
            // 
            Controls.Add(convoTextBox);
            Dock = DockStyle.Fill;
            Location = new Point(0, 0);
            Name = "tabPage";
            Padding = new Padding(3);
            Size = new Size(336, 160);
            TabIndex = 0;
            Text = Buddy.Name;
            TabStop = false;
            UseVisualStyleBackColor = true;
            
        }

        void ConvoTextBoxTextChanged(object sender, EventArgs e)
        {
            convoTextBox.SelectionStart = convoTextBox.Text.Length;
            convoTextBox.ScrollToCaret();
        }

        private void AddConvoMessage(string message)
        {
            convoTextBox.Text += message + Environment.NewLine;
        }

        public void AddImage(string imageUrl)
        {
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                return;

            string msgToAdd = string.Format(IMG_CONVO_FMT_IN, imageUrl);
            AddConvoMessage(msgToAdd);
        }

        public void SendMessage(string message)
        {
            PimMessageHandler.SendMessage(Buddy, message, Resource, currentThread);
        }
    }
}
