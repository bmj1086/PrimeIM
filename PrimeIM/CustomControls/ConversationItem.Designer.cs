namespace PrimeIM.CustomControls
{
    partial class ConversationItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.personLabel = new System.Windows.Forms.Label();
            this.messagePanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // personLabel
            // 
            this.personLabel.AutoSize = true;
            this.personLabel.Location = new System.Drawing.Point(3, 4);
            this.personLabel.Name = "personLabel";
            this.personLabel.Size = new System.Drawing.Size(43, 13);
            this.personLabel.TabIndex = 0;
            this.personLabel.Text = string.Format("{0}:", Buddy.Name);
            // 
            // messagePanel
            // 
            this.messagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top 
                        | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.messagePanel.Location = new System.Drawing.Point(45, 4);
            this.messagePanel.Name = "messagePanel";
            this.messagePanel.Size = new System.Drawing.Size(190, 62);
            this.messagePanel.TabIndex = 1;
            // 
            // ConversationItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.messagePanel);
            this.Controls.Add(this.personLabel);
            this.Name = "ConversationItem";
            this.Size = new System.Drawing.Size(238, 69);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        
        #endregion

        private System.Windows.Forms.Label personLabel;
        private System.Windows.Forms.Panel messagePanel;


    }
}
