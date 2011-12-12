using System;
using System.Drawing;
using System.Windows.Forms;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.avatar;
using PrimeIM.Data;
using agsXMPP.protocol.iq.roster;

namespace PrimeIM.CustomControls
{
    public partial class BuddyListboxItem : UserControl
    {
        private readonly RosterItem RosterItem;

        public Buddy Buddy
        {
            get { return BuddyList.Instance.Get(this.RosterItem.Jid); }
        }

        public BuddyListboxItem(Buddy buddy)
        {
            this.RosterItem = buddy.RosterItem;
            InitializeComponent();
            UpdateBuddyInformation();
        }

        public void UpdateBuddyInformation()
        {
            buddyNameLabel.Text = RosterItem.Name;
            buddyStatusLabel.Text = String.IsNullOrEmpty(Buddy.Status) ? Buddy.ShowTypeString : Buddy.Status;
            buddyStatusPictureBox.BackColor = ConvertBuddyStatusToColor();
            //buddyPictureBox.Image = ImageFromAvatar(Buddy.Avatar);
            
        }

        private Image ImageFromAvatar(Avatar avatar)
        {
            return null; //TODO: implement this
        }

        private Color ConvertBuddyStatusToColor()
        {
            switch (Buddy.MainPresenceType)
            {
                case ShowType.NONE:
                    return Color.Green;
                case ShowType.away:
                    return Color.Orange;
                case ShowType.chat:
                    return Color.White;
                case ShowType.dnd:
                    return Color.Red;
                case ShowType.xa:
                    return Color.DarkOrange;
                default:
                    return Color.DimGray;
            }
        }

    }
}
