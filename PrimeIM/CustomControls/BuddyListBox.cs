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
                SuspendLayout();
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

                if (action == null)
                    throw new NotImplementedException(args.Action + " is not implemented for BuddyListBox.");
 
                Invoke(action, args.ChangedBuddies);
                ResumeLayout(false);
                Invoke(new Action(Sort));
            }
        }

        private void Remove(IList<Buddy> buddies)
        {
            foreach (BuddyListboxItem existingItem in buddies.Select(GetItem))
            {
                flowLayoutPanel.Controls.Remove(existingItem);
            }
        }

        private void Update(IList<Buddy> buddies)
        {
            List<Buddy> adds = new List<Buddy>();
            foreach (var buddy in buddies)
            {
                var existingBuddyItem = GetItem(buddy);

                if (existingBuddyItem == null)
                {
                    adds.Add(buddy);
                    continue;
                }

                existingBuddyItem.UpdateBuddyInformation(); 
            }

            if (adds.Count > 0)
                Add(adds);
            
        }

        [Browsable(true)]
        public event BuddyDoubleClickedHandler BuddyDoubleClicked;

        public void Add(IList<Buddy> items)
        {
            foreach (var buddy in items)
            {
                if (GetItem(buddy) != null || !buddy.IsOnline)
                    return;

                var item = new BuddyListboxItem(buddy);

                item.MouseDoubleClick += ItemMouseDoubleClick;
                flowLayoutPanel.Controls.Add(item);
            }
        }

        private BuddyListboxItem GetItem(Buddy buddy)
        {
            var rtn = flowLayoutPanel.Controls.OfType<BuddyListboxItem>().
                Where(b => b.Buddy == buddy).
                SingleOrDefault();
            return rtn;
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
            var items = flowLayoutPanel.Controls.OfType<BuddyListboxItem>().
                OrderBy(x => x.Buddy).ToArray();

            flowLayoutPanel.Controls.Clear();
            flowLayoutPanel.Controls.AddRange(items);
            Invalidate();
        }
    }
}