using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoorLockDemo
{
    public partial class aboutForm : Form
    {
        public aboutForm()
        {
            InitializeComponent();

            aboutLnkLbl.Text = "Icons by Daniel Bruce and Freepik. Licensed by CC 3.0 BY";
            aboutLnkLbl.Links.Add(9, 12, "http://www.flaticon.com/authors/daniel-bruce");
            aboutLnkLbl.Links.Add(26, 7, "http://www.freepik.com");
            aboutLnkLbl.Links.Add(47, 9, "http://creativecommons.org/licenses/by/3.0/");
            aboutLnkLbl.LinkClicked += AboutLnkLbl_LinkClicked;

            
        }

        private void AboutLnkLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(e.Link.LinkData.ToString());
            Process.Start(sInfo);
        }

        private void aboutForm_Load(object sender, EventArgs e)
        {
            Width = aboutLnkLbl.Width + 6;
            Height = aboutLnkLbl.Height + 6;

            aboutLnkLbl.Parent = aboutPnl;
            int x = (aboutPnl.Width / 2) - (aboutLnkLbl.Width / 2);
            int y = (aboutPnl.Height / 2) - (aboutLnkLbl.Height / 2);
            aboutLnkLbl.Location = new Point(x, y);
        }
    }
}
