using System.Windows.Forms;

namespace PrimeIM.CustomControls
{
    partial class BuddyListboxItem
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
            this.buddyStatusPictureBox = new System.Windows.Forms.PictureBox();
            this.buddyStatusLabel = new System.Windows.Forms.Label();
            this.buddyNameLabel = new System.Windows.Forms.Label();
            this.buddyPictureBox = new System.Windows.Forms.PictureBox();
            this.bottomSplitter = new System.Windows.Forms.Splitter();
            ((System.ComponentModel.ISupportInitialize)(this.buddyStatusPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buddyPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // buddyStatusPictureBox
            // 
            this.buddyStatusPictureBox.Location = new System.Drawing.Point(3, 4);
            this.buddyStatusPictureBox.Name = "buddyStatusPictureBox";
            this.buddyStatusPictureBox.Size = new System.Drawing.Size(10, 10);
            this.buddyStatusPictureBox.TabIndex = 5;
            this.buddyStatusPictureBox.TabStop = false;
            this.buddyStatusPictureBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ForwardDoubleClick);
            // 
            // buddyStatusLabel
            // 
            this.buddyStatusLabel.AutoEllipsis = true;
            this.buddyStatusLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.buddyStatusLabel.Location = new System.Drawing.Point(21, 18);
            this.buddyStatusLabel.Name = "buddyStatusLabel";
            this.buddyStatusLabel.Size = new System.Drawing.Size(166, 14);
            this.buddyStatusLabel.TabIndex = 6;
            this.buddyStatusLabel.Text = "label1";
            this.buddyStatusLabel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ForwardDoubleClick);
            // 
            // buddyNameLabel
            // 
            this.buddyNameLabel.AutoEllipsis = true;
            this.buddyNameLabel.Location = new System.Drawing.Point(21, 2);
            this.buddyNameLabel.Name = "buddyNameLabel";
            this.buddyNameLabel.Size = new System.Drawing.Size(166, 15);
            this.buddyNameLabel.TabIndex = 4;
            this.buddyNameLabel.Text = "label1";
            this.buddyNameLabel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ForwardDoubleClick);
            // 
            // buddyPictureBox
            // 
            this.buddyPictureBox.Location = new System.Drawing.Point(193, 3);
            this.buddyPictureBox.Name = "buddyPictureBox";
            this.buddyPictureBox.Size = new System.Drawing.Size(31, 31);
            this.buddyPictureBox.TabIndex = 3;
            this.buddyPictureBox.TabStop = false;
            this.buddyPictureBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ForwardDoubleClick);
            // 
            // bottomSplitter
            // 
            this.bottomSplitter.BackColor = System.Drawing.Color.Silver;
            this.bottomSplitter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomSplitter.Location = new System.Drawing.Point(0, 36);
            this.bottomSplitter.Name = "bottomSplitter";
            this.bottomSplitter.Size = new System.Drawing.Size(1633, 1);
            this.bottomSplitter.TabIndex = 7;
            this.bottomSplitter.TabStop = false;
            // 
            // BuddyListboxItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.bottomSplitter);
            this.Controls.Add(this.buddyStatusPictureBox);
            this.Controls.Add(this.buddyStatusLabel);
            this.Controls.Add(this.buddyNameLabel);
            this.Controls.Add(this.buddyPictureBox);
            this.Name = "BuddyListboxItem";
            this.Size = new System.Drawing.Size(1633, 37);
            ((System.ComponentModel.ISupportInitialize)(this.buddyStatusPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buddyPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        void ForwardDoubleClick(object sender, System.EventArgs e)
        {
            OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Left, 2, 1, 1, 0));
        }
        
        #endregion

        private System.Windows.Forms.PictureBox buddyStatusPictureBox;
        private System.Windows.Forms.Label buddyStatusLabel;
        private System.Windows.Forms.Label buddyNameLabel;
        private System.Windows.Forms.PictureBox buddyPictureBox;
        private Splitter bottomSplitter;
    }
}
