namespace MoonTaxi
{
    partial class MainWindow
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
            this.serverButton = new System.Windows.Forms.Button();
            this.clientButton = new System.Windows.Forms.Button();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // serverButton
            // 
            this.serverButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.serverButton.Location = new System.Drawing.Point(135, 50);
            this.serverButton.Name = "serverButton";
            this.serverButton.Size = new System.Drawing.Size(75, 23);
            this.serverButton.TabIndex = 0;
            this.serverButton.Text = "Server";
            this.serverButton.UseVisualStyleBackColor = true;
            this.serverButton.Click += new System.EventHandler(this.serverButton_Click);
            // 
            // clientButton
            // 
            this.clientButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.clientButton.Location = new System.Drawing.Point(135, 79);
            this.clientButton.Name = "clientButton";
            this.clientButton.Size = new System.Drawing.Size(75, 23);
            this.clientButton.TabIndex = 1;
            this.clientButton.Text = "Client";
            this.clientButton.UseVisualStyleBackColor = true;
            this.clientButton.Click += new System.EventHandler(this.clientButton_Click);
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Location = new System.Drawing.Point(12, 81);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(117, 20);
            this.usernameTextBox.TabIndex = 2;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 167);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.clientButton);
            this.Controls.Add(this.serverButton);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button serverButton;
        private System.Windows.Forms.Button clientButton;
        private System.Windows.Forms.TextBox usernameTextBox;
    }
}