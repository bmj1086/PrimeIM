using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using agsXMPP.protocol.iq.roster;
using PrimeIM.Data;

namespace PrimeIM
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginWindow());
            if (PimMessageHandler.Authenticated)
                Application.Run(new BuddyListForm());
            if (PimMessageHandler.Authenticated)
                PimMessageHandler.Logout();
            Application.Exit();
        }
        
    }
}
