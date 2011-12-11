using System;
using System.Collections;
using System.Collections.Specialized;
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
            BuddyList.Instance.CollectionChanged += BuddyListChanged;
        }

        private void BuddyListChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            lock (thisLock)
            {
                if (InvokeRequired)
                    Invoke(new Action(RemoveEmptyItems));
                else
                    RemoveEmptyItems();

                Action<Presence> action = null;
                IList items = args.NewItems;
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        action = Add;
                        break;
                    case NotifyCollectionChangedAction.Move:
                        throw new NotImplementedException();
                    case NotifyCollectionChangedAction.Reset:
                        UpdateAllControls();
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        action = Update;
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        break;
                }

                if (action != null)
                {
                    foreach (var item in items)
                        Invoke(action, (item as Buddy).MainPresence);
                }

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

        private void Update(Presence presence)
        {
            var existingBuddyItem = flowLayoutPanel.Controls.OfType<BuddyListboxItem>().
                Where(b => b.Buddy.Equals(presence)).
                SingleOrDefault();

            if (existingBuddyItem == null)
                return;

            existingBuddyItem.UpdateBuddyInformation();
            Sort();
        }

        [Browsable(true)]
        public event BuddyDoubleClickedHandler BuddyDoubleClicked;

        public void Add(Presence presence)
        {
            if (GetItem(presence) != null)
                    return;

            var item = new BuddyListboxItem(presence);

            item.MouseDoubleClick += ItemMouseDoubleClick;
            AddItem(item);
        }

        private BuddyListboxItem GetItem(Presence presence)
        {
            return flowLayoutPanel.Controls.OfType<BuddyListboxItem>().
                Where(b => b.Buddy.MainPresence.Equals(presence)).
                SingleOrDefault();
        }

        private void AddItem(BuddyListboxItem item)
        {
            flowLayoutPanel.Controls.Add(item);
            Sort();
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