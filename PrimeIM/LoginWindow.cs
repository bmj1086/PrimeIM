using System;
using System.Configuration;
using System.Windows.Forms;
using agsXMPP.Xml.Dom;
using PrimeIM.Data;

namespace PrimeIM
{
    public partial class LoginWindow : Form
    {
        public LoginWindow()
        {
            InitializeComponent();
#if DEBUG
            UsernameTextBox.Text = ConfigurationManager.AppSettings["DebugUsername"];
            PasswordTextBox.Text = ConfigurationManager.AppSettings["DebugPassword"];
#endif
        }

        private void LoginButtonClick(object sender, EventArgs e)
        {
            LoginButton.Text = @"Wait";
            LoginButton.Enabled = false;
            PimMessageHandler.Login(UsernameTextBox.Text, PasswordTextBox.Text, SuccessfulLogin, AuthenticationFailed);
        }
        
        private void SuccessfulLogin(object sender)
        {
            if (InvokeRequired)
                Invoke(new Action(Dispose));
        }

        private void AuthenticationFailed(object sender, Element e)
        {
            if (InvokeRequired)
                Invoke(new Action<string>(ShowInvalidLogin));
        }

        private void ShowInvalidLogin(string message = null)
        {
            LoginButton.Enabled = true;
            LoginButton.Text = @"Login";
            InvalidLoginLabel.Visible = true;
            PasswordTextBox.Clear();
        }

        private void UsernameLeave(object sender, EventArgs e)
        {
            if (UsernameTextBox.Text.Contains("@"))
                UsernameTextBox.Text = UsernameTextBox.Text.Remove(UsernameTextBox.Text.IndexOf("@"));
            
        }

        private void PasswordTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoginButton.PerformClick();
        }
    }
}
