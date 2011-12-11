using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using PrimeIM.Data;
using Utility;

namespace PrimeIM.CustomControls
{
    public partial class ConversationItem : UserControl
    {
        private static readonly object thisLock = new object();
        public readonly Buddy Buddy;
        private const int X_LOCATION = 2;
        private const int Y_MARGIN = 5;
        public ConversationItem(Buddy from, string message = null)
        {
            Buddy = from;
            InitializeComponent();

            if (message != null)
                AddMessage(message);
        }

        public void AddMessage(string message)
        {
            lock (thisLock)
            {
                Label newMessageObject = new Label {Text = message, Location = NextLocation()};
                messagePanel.Controls.Add(newMessageObject);
            }
        }

        private Point NextLocation()
        {
            int controlCount = messagePanel.Controls.Count;
            switch (controlCount)
            {
                case 0:
                    return new Point(X_LOCATION, Y_MARGIN);
                default:
                    return new Point(X_LOCATION, (Height + Y_MARGIN) * controlCount);
            }
        }

        public void AddImage(string imageUrl)
        {
            lock (thisLock)
            {
                if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                    return;

                PictureBox newMessageObject = new PictureBox();
                newMessageObject.Image = MyWebUtility.DownloadImage(imageUrl);
                newMessageObject.Size = new Size(100, 100);
                newMessageObject.SizeMode = PictureBoxSizeMode.StretchImage;
                newMessageObject.Location = NextLocation();
                messagePanel.Controls.Add(newMessageObject);
            }
        }

        
    }
}