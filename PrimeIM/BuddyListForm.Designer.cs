using System.Windows.Forms;

namespace PrimeIM
{
    partial class BuddyListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.settingsLinkLabel = new System.Windows.Forms.LinkLabel();
            this.buddyListBoxToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buddyListBox = new PrimeIM.CustomControls.BuddyListBox();
            this.SuspendLayout();
            // 
            // settingsLinkLabel
            // 
            this.settingsLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsLinkLabel.AutoSize = true;
            this.settingsLinkLabel.LinkColor = System.Drawing.Color.RoyalBlue;
            this.settingsLinkLabel.Location = new System.Drawing.Point(194, 9);
            this.settingsLinkLabel.Name = "settingsLinkLabel";
            this.settingsLinkLabel.Size = new System.Drawing.Size(45, 13);
            this.settingsLinkLabel.TabIndex = 0;
            this.settingsLinkLabel.TabStop = true;
            this.settingsLinkLabel.Text = "Settings";
            this.settingsLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SettingsLinkLabelLinkClicked);
            // 
            // buddyListBox
            // 
            this.buddyListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buddyListBox.BackColor = System.Drawing.Color.White;
            this.buddyListBox.Location = new System.Drawing.Point(1, 25);
            this.buddyListBox.Name = "buddyListBox";
            this.buddyListBox.Size = new System.Drawing.Size(240, 385);
            this.buddyListBox.TabIndex = 1;
            this.buddyListBox.BuddyDoubleClicked += new PrimeIM.CustomControls.BuddyListBox.BuddyDoubleClickedHandler(this.BuddyDoubleClicked);
            // 
            // BuddyListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(242, 413);
            this.Controls.Add(this.buddyListBox);
            this.Controls.Add(this.settingsLinkLabel);
            this.Name = "BuddyListForm";
            this.Text = "BuddyList";
            this.Activated += new System.EventHandler(this.BuddyListForm_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel settingsLinkLabel;
        private System.Windows.Forms.ToolTip buddyListBoxToolTip;
        private CustomControls.BuddyListBox buddyListBox;
    }
}