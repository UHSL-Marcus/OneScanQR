using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

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
                logoutBtn.Visible = false;
            }
            
        }

        protected void logoutBtn_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}