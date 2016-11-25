namespace Client.Views
{
    partial class AddChatroom
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
            this.createChatroomButton = new System.Windows.Forms.Button();
            this.chatroomTextBox = new System.Windows.Forms.TextBox();
            this.chatroomNameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // createChatroomButton
            // 
            this.createChatroomButton.Location = new System.Drawing.Point(85, 88);
            this.createChatroomButton.Name = "createChatroomButton";
            this.createChatroomButton.Size = new System.Drawing.Size(116, 23);
            this.createChatroomButton.TabIndex = 0;
            this.createChatroomButton.Text = "Create chatroom";
            this.createChatroomButton.UseVisualStyleBackColor = true;
            this.createChatroomButton.Click += new System.EventHandler(this.createChatroomButton_Click);
            // 
            // chatroomTextBox
            // 
            this.chatroomTextBox.Location = new System.Drawing.Point(12, 48);
            this.chatroomTextBox.Name = "chatroomTextBox";
            this.chatroomTextBox.Size = new System.Drawing.Size(260, 20);
            this.chatroomTextBox.TabIndex = 1;
            // 
            // chatroomNameLabel
            // 
            this.chatroomNameLabel.AutoSize = true;
            this.chatroomNameLabel.Location = new System.Drawing.Point(102, 23);
            this.chatroomNameLabel.Name = "chatroomNameLabel";
            this.chatroomNameLabel.Size = new System.Drawing.Size(81, 13);
            this.chatroomNameLabel.TabIndex = 2;
            this.chatroomNameLabel.Text = "Chatroom name";
            // 
            // AddChatroom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 118);
            this.Controls.Add(this.chatroomNameLabel);
            this.Controls.Add(this.chatroomTextBox);
            this.Controls.Add(this.createChatroomButton);
            this.Name = "AddChatroom";
            this.Text = "Create a chatroom";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button createChatroomButton;
        private System.Windows.Forms.TextBox chatroomTextBox;
        private System.Windows.Forms.Label chatroomNameLabel;
    }
}