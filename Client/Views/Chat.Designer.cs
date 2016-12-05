namespace Client.Views
{
    partial class Chat
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
            this.userlist = new System.Windows.Forms.ListBox();
            this.chatrooms = new System.Windows.Forms.ComboBox();
            this.createChatroomButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // userlist
            // 
            this.userlist.FormattingEnabled = true;
            this.userlist.Location = new System.Drawing.Point(12, 9);
            this.userlist.Name = "userlist";
            this.userlist.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.userlist.Size = new System.Drawing.Size(109, 342);
            this.userlist.TabIndex = 0;
            this.userlist.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // chatrooms
            // 
            this.chatrooms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chatrooms.FormattingEnabled = true;
            this.chatrooms.Location = new System.Drawing.Point(127, 9);
            this.chatrooms.Name = "chatrooms";
            this.chatrooms.Size = new System.Drawing.Size(389, 21);
            this.chatrooms.TabIndex = 1;
            this.chatrooms.SelectedIndexChanged += new System.EventHandler(this.chatrooms_SelectedIndexChanged);
            // 
            // createChatroomButton
            // 
            this.createChatroomButton.Location = new System.Drawing.Point(522, 9);
            this.createChatroomButton.Name = "createChatroomButton";
            this.createChatroomButton.Size = new System.Drawing.Size(120, 23);
            this.createChatroomButton.TabIndex = 2;
            this.createChatroomButton.Text = "Create a chatroom";
            this.createChatroomButton.UseVisualStyleBackColor = true;
            this.createChatroomButton.Click += new System.EventHandler(this.createChatroomButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(127, 36);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(515, 289);
            this.textBox1.TabIndex = 3;
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(127, 331);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(445, 20);
            this.messageTextBox.TabIndex = 4;
            this.messageTextBox.TextChanged += new System.EventHandler(this.messageTextBox_TextChanged);
            this.messageTextBox.Enter += new System.EventHandler(this.sendButton_Click);
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(578, 331);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(64, 20);
            this.sendButton.TabIndex = 5;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 359);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.createChatroomButton);
            this.Controls.Add(this.chatrooms);
            this.Controls.Add(this.userlist);
            this.Name = "Chat";
            this.Text = "Chat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox userlist;
        private System.Windows.Forms.ComboBox chatrooms;
        private System.Windows.Forms.Button createChatroomButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.Button sendButton;
    }
}