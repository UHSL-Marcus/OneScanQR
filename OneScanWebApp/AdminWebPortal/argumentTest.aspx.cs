using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdminWebPortal
{
    public partial class argumentTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
                Button btn = new Button();
                btn.Text = "A button";
                btn.ID = "btnUD";
                btn.CommandArgument = "Argument";
                btn.Click += Btn_Click;

                container.Controls.Add(btn);
            
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            showArgument.Text = btn.CommandArgument;
            btn.CommandArgument = "new argument";
        }
    }
}