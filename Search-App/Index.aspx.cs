using Search_App.Common;
using Search_App.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Search_App
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["AppName"] = null;
            Session["AppCode"] = null;
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            string uname, pwd;
            uname = txt_username.Value.Trim();
            pwd = txt_password.Value.Trim();
            bool userValid = IsValidUser(uname,pwd);
           // Response.Redirect("SearchPage.aspx");
            if (userValid)
            {
                Response.Redirect("SearchPage.aspx");
            }
            else
            {
                lbl_error.Text = "Invalid UserName or Password"; 
            }

           
        }
        private bool IsValidUser(string userName, string password)
        {
            bool isValid = false;
            SearchAppRepository _repo = new SearchAppRepository();
            ApplicationDetails details = _repo.GetApplicationDetails(userName);

            if(password == details.AppPassword)
            {
                isValid = true;
                Session["AppName"] = details.AppName;
                Session["AppCode"] = details.AppCode;
            }
            else
            {
                Session["AppName"] = null;
                Session["AppCode"] =null;

            }



            return isValid;

        }

        protected void Unnamed2_Click(object sender, EventArgs e)
        {
            txt_password.Value = "";
            txt_username.Value = "";
            lbl_error.Text = "";
        }
    }
}