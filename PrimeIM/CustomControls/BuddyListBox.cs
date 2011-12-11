using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using agsXMPP.protocol.client;
using PrimeIM.Data;

namespace PrimeIM.CustomControls
{
    public partial class BuddyListBox : UserControl
    {
        private static readonly object thisLock = new object();

        #region Delegates

        public delegate void BuddyDoubleClickedHandler(object sender, BuddyListEventArgs args);

        #endregion

        public BuddyListBox()
        {
            InitializeComponent();
            BuddyList.Instance.BuddyListChanged += BuddyListChanged;
        }

        private void BuddyListChanged(object sender, BuddyListChangedEventHandlerArgs args)
        {
            lock (thisLock)
            {
                if (InvokeRequired)
                    Invoke(new Action(RemoveEmptyItems));
                else
                    RemoveEmptyItems();

                Action<IList<Buddy>> action = null;
                switch (args.Action)
                {
                    case BuddyListChangedAction.Add:
                        action = Add;
                        break;
                    case BuddyListChangedAction.Update:
                        action = Update;
                        break;
                    case BuddyListChangedAction.Remove:
                        action = Remove;
                        break;
                }

                if (action != null)
                {
                    Invoke(action, args.ChangedBuddies);
                }

            }
        }

        private void Remove(IList<Buddy> buddies)
        {
            foreach (BuddyListboxItem existingItem in buddies.Select(GetItem).Where(item => item != null))
            {
                Controls.Remove(existingItem);
            }
        }

        private void UpdateAllControls()
        {
            foreach (var control in flowLayoutPanel.Controls.OfType<BuddyListboxItem>())
            {    
                if (InvokeRequired)
                    Invoke(new Action(() => control.UpdateBuddyInformation()));
                else
                    control.UpdateBuddyInformation();
            
            }
            Sort();
        }

        private void RemoveEmptyItems()
        {
            var cleanupControls = flowLayoutPanel.Controls.OfType<BuddyListboxItem>().
                Where(b => b.Buddy == null);

            foreach (var buddyListboxItem in cleanupControls)
            {
                flowLayoutPanel.Controls.Remove(buddyListboxItem);
            }
            Sort();
        }

        private void Update(IList<Buddy> buddies)
        {
            SuspendLayout();
            foreach (var buddy in buddies)
            {
                var existingBuddyItem = GetItem(buddy);

                if (existingBuddyItem == null)
                    return;

                existingBuddyItem.UpdateBuddyInformation(); 
            }
            ResumeLayout(true);
            Sort();
        }

        [Browsable(true)]
        public event BuddyDoubleClickedHandler BuddyDoubleClicked;

        public void Add(IList<Buddy> items)
        {
            SuspendLayout();
            foreach (var buddy in items)
            {
                if (GetItem(buddy) != null)
                    return;

                var item = new BuddyListboxItem(buddy);

                item.MouseDoubleClick += ItemMouseDoubleClick;
                flowLayoutPanel.Controls.Add(item);
            }
            ResumeLayout(true);
            Sort();
        }

        private BuddyListboxItem GetItem(Buddy buddy)
        {
            return flowLayoutPanel.Controls.OfType<BuddyListboxItem>().
                Where(b => b.Equals(buddy)).
                SingleOrDefault();
        }

        private void AddItem(BuddyListboxItem item)
        {
            
        }

        private void ItemMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            var item = (sender as BuddyListboxItem);
            if (item == null)
                return;

            if (BuddyDoubleClicked != null)
                BuddyDoubleClicked.Invoke(this, new BuddyListEventArgs(item.Buddy, EventType.DoubleClicked));
        }
        
        private void Sort()
        {
            var items =
                flowLayoutPanel.Controls.OfType<BuddyListboxItem>().OrderBy(x => x.Buddy).
                    ToArray();
            flowLayoutPanel.Controls.Clear();
            flowLayoutPanel.Controls.AddRange(items);
        }
    }
}