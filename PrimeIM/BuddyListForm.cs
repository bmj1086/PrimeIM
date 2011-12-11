using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PrimeIM.CustomControls;
using PrimeIM.Data;
using Message = agsXMPP.protocol.client.Message;

namespace PrimeIM
{
    public partial class BuddyListForm : Form
    {
        public BuddyListForm()
        {
            InitializeComponent();
            ChatForm.Singleton.Hide();
            //BuddyList.BuddyListChanged += BuddyListChanged;
            //AddBuddies(BuddyList.Buddies);
        }

        //void BuddyListChanged(object sender, BuddyListEventArgs args)
        //{
        //    switch (args.EventType)
        //    {
        //        case EventType.Add:
        //            if (InvokeRequired)
        //                Invoke(new Action(() => AddBuddy(args.Buddy)));
        //            break;
        //        case EventType.Update:
        //            if (InvokeRequired)
        //                Invoke(new Action(() => UpdateBuddy(args.Buddy)));
        //            break;
        //        case EventType.Remove:
        //            if (InvokeRequired)
        //                Invoke(new Action(() => RemoveBuddy(args.Buddy)));
        //            break;
        //    }
        //}

        //private void RemoveBuddy(Buddy buddy)
        //{
        //    if (BuddyList.Buddies.Contains(buddy))
        //        BuddyList.Buddies.Remove(buddy);
        //}

        //private void UpdateBuddy(Buddy buddy)
        //{
        //    var buddyItem = buddyListBox.Controls.OfType<BuddyListboxItem>().
        //        Where(i => i.Buddy.Equals(buddy)).
        //        SingleOrDefault();

        //    if (buddyItem != null)
        //    {
        //        buddyItem.UpdateBuddyInformation();
        //        if (InvokeRequired)
        //            Invoke(new Action(buddyListBox.Invalidate));
        //        else
        //            buddyListBox.Invalidate();
        //    }

        //}
    

        private void OpenChatForm(Buddy buddy, Message msg = null)
        {
            if (!ChatForm.Singleton.OpenBuddies.Contains(buddy))
                ChatForm.Singleton.AddChat(buddy, msg);

            ChatForm.Singleton.Show();
        }

        private void SettingsLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SettingsForm form = new SettingsForm();
            form.Show();
        }

        private void BuddyDoubleClicked(object sender, BuddyListEventArgs args)
        {
            if (InvokeRequired)
                Invoke(new Action(() => OpenChatForm(args.Buddy)));
            else
                OpenChatForm(args.Buddy);
        }

        [DebuggerStepThrough]
        private void BuddyListForm_Activated(object sender, EventArgs e)
        {
            this.buddyListBox.Invalidate();
        }

        
    }
}
