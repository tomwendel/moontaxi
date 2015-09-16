using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MoonTaxi
{
    public partial class MainWindow : Form
    {
        public bool IsServer { get;private set; }
        public string Username { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void serverButton_Click(object sender, EventArgs e)
        {
            IsServer = true;
            Close();
        }
        
        private void clientButton_Click(object sender, EventArgs e)
        {
            Username = usernameTextBox.Text;
            IsServer = false;
            Close();
        }
    }
}
