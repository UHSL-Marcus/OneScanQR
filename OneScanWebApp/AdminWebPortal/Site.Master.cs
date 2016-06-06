using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdminWebPortal
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool loggedIn = (HttpContext.Current.User != null) && HttpContext.Current.User.Identity.IsAuthenticated;

            if(!loggedIn)
            {
                sidebar.Visible = false;
            }
        }
    }
}