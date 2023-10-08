using Search_App.BL;
using Search_App.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Search_App
{
    public partial class SearchPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            #region static data -  Commented
            //DataTable dt = new DataTable();

            ////adpt.Fill(dt);
            //DropDownList1.DataSource = dt;
            //DropDownList1.DataBind();
            //DropDownList1.DataTextField = "Name";
            //DropDownList1.DataValueField = "ID";
            //DropDownList1.DataBind();

            /*DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("ItemId", typeof(int)),
                    new DataColumn("ItemName", typeof(string)),
                    new DataColumn("ItemQuantity",typeof(string)) });
            dt.Rows.Add(1, "Flour", "500 Kg");
            dt.Rows.Add(2, "Tea", "20 Kg");
            dt.Rows.Add(3, "Rice", "1000 Kg");
            gv_result.DataSource = dt;
            gv_result.DataBind();

            gv_result.UseAccessibleHeader = true;
            gv_result.HeaderRow.TableSection = TableRowSection.TableHeader;*/
            #endregion

            //get data from db for states and cities for binging dropdowns

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            
            SearchBL _search = new SearchBL();
            SRequest sreq
                = new SRequest
                {
                    Name = txtName.Value, 
                    Address= txtAddress.Value,
                    StateCode= drpState.SelectedValue,
                    City= drpCity.SelectedValue,
                    PostalCode= txtPostalCode.Value
                };
            string appCode = "CRE";

            List<SResponse> result = new List<SResponse>();
            result = _search.SearchResult(sreq, appCode);

            gv_result.DataSource = result;
            gv_result.DataBind();

            gv_result.UseAccessibleHeader = true;
            if (result.Count > 0)
            {
                gv_result.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }
}