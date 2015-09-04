namespace SexyFishHorse.WauwBot.View
{
    partial class WauwBot
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
            this.chatList = new System.Windows.Forms.ListView();
            this.providerHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.usernameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timestampHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.messageHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // chatList
            // 
            this.chatList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chatList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.providerHeader,
            this.usernameHeader,
            this.timestampHeader,
            this.messageHeader});
            this.chatList.Location = new System.Drawing.Point(12, 12);
            this.chatList.Name = "chatList";
            this.chatList.Size = new System.Drawing.Size(690, 425);
            this.chatList.TabIndex = 0;
            this.chatList.UseCompatibleStateImageBehavior = false;
            this.chatList.View = System.Windows.Forms.View.Details;
            // 
            // providerHeader
            // 
            this.providerHeader.Tag = "";
            this.providerHeader.Text = "Provider";
            // 
            // usernameHeader
            // 
            this.usernameHeader.Text = "Username";
            // 
            // timestampHeader
            // 
            this.timestampHeader.Text = "Timestamp";
            // 
            // messageHeader
            // 
            this.messageHeader.Text = "Message";
            // 
            // WauwBot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 449);
            this.Controls.Add(this.chatList);
            this.Name = "WauwBot";
            this.Text = "WauwBot";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView chatList;
        private System.Windows.Forms.ColumnHeader providerHeader;
        private System.Windows.Forms.ColumnHeader usernameHeader;
        private System.Windows.Forms.ColumnHeader timestampHeader;
        private System.Windows.Forms.ColumnHeader messageHeader;
    }
}

