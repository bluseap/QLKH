using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Diagnostics;
using EOSCRM.Dao;

using System.Collections;
using System.Collections.Generic;



namespace EOSCRM.Web.NhanSu
{
    public partial class AboutPo : System.Web.UI.Page
    {
        private readonly WPOClass _wpClass = new WPOClass();
        private readonly ReportClass _rpClass = new ReportClass();

        #region "Private Methods"

        private DataTable BindParent(int ItemID)
        {
            DataTable objBind = new DataTable();

            objBind = BindData(ItemID);

            return objBind;
        }

        #endregion

        #region "Create Menu"

        private void CreateMenu()
        {
            DataTable objBind = new DataTable();
            DataTable objSubBind = new DataTable();
            int i = 1;
            string sURL = "";
            string sMenuName = "";

            objBind = BindData(-1);

            if ((objBind != null))
            {
                if (objBind.Rows.Count > 0)
                {
                    lblMenu.Controls.Add(new LiteralControl("<div class=\"container\">"));
                    lblMenu.Controls.Add(new LiteralControl("<div class=\"navbar navbar-inverse\" role=\"navigation\">"));
                    lblMenu.Controls.Add(new LiteralControl("<div class=\"container-fluid\">"));
                    lblMenu.Controls.Add(new LiteralControl("<div class=\"navbar-header\">"));
                    lblMenu.Controls.Add(new LiteralControl("<button type=\"button\" class=\"navbar-toggle collapsed\" data-toggle=\"collapse\" data-target=\".navbar-collapse\">"));
                    lblMenu.Controls.Add(new LiteralControl("<span class=\"sr-only\">Toggle navigation</span>"));
                    lblMenu.Controls.Add(new LiteralControl("<span class=\"icon-bar\"></span>"));
                    lblMenu.Controls.Add(new LiteralControl("<span class=\"icon-bar\"></span>"));
                    lblMenu.Controls.Add(new LiteralControl("<span class=\"icon-bar\"></span>"));
                    lblMenu.Controls.Add(new LiteralControl("</button>"));
                    lblMenu.Controls.Add(new LiteralControl("</div>"));

                    //navbar
                    lblMenu.Controls.Add(new LiteralControl("<div class=\"navbar-collapse collapse\" style=\"height: 1px;\">"));
                    lblMenu.Controls.Add(new LiteralControl("<ul class=\"nav navbar-nav\">"));

                    foreach (DataRow row in objBind.Rows)
                    {
                        sMenuName = row["MenuName"].ToString();
                        sURL = row["URL"].ToString();
                        if (i == 1)
                        {
                            lblMenu.Controls.Add(new LiteralControl("<li class=\"active\"><a href=" + sURL + "><span class=\"glyphicon glyphicon-home\"></span> " + sMenuName + "</a></li>"));
                        }
                        else
                        {
                            //Check Parent
                            int MenuID;
                            MenuID = (int)row["MenuID"];
                            objSubBind = BindParent(MenuID);
                            if ((objSubBind != null))
                            {
                                if (objSubBind.Rows.Count > 0)
                                {
                                    lblMenu.Controls.Add(new LiteralControl("<li class=\"dropdown\">"));
                                    lblMenu.Controls.Add(new LiteralControl("<a href=" + sURL + " class=\"dropdown-toggle\" data-toggle=\"dropdown\">" + sMenuName + "<b class=\"caret\"></b></a>"));
                                    lblMenu.Controls.Add(new LiteralControl("<ul class=\"dropdown-menu\">"));
                                    foreach (DataRow subrow in objSubBind.Rows)
                                    {
                                        sMenuName = subrow["MenuName"].ToString();
                                        sURL = subrow["URL"].ToString();
                                        lblMenu.Controls.Add(new LiteralControl("<li><a href=" + sURL + ">" + sMenuName + "</a></li>"));
                                    }
                                    lblMenu.Controls.Add(new LiteralControl("</ul>"));
                                    lblMenu.Controls.Add(new LiteralControl("</li>"));
                                }
                                else
                                {
                                    lblMenu.Controls.Add(new LiteralControl("<li><a href=" + sURL + ">" + sMenuName + "</a></li>"));
                                }
                            }
                            else
                            {
                                lblMenu.Controls.Add(new LiteralControl("<li><a href=" + sURL + ">" + sMenuName + "</a></li>"));
                            }
                        }
                        i = i + 1;
                    }
                    lblMenu.Controls.Add(new LiteralControl("</ul>"));
                    lblMenu.Controls.Add(new LiteralControl("</div><!--/.nav-collapse -->"));
                    lblMenu.Controls.Add(new LiteralControl("</div><!--/.container-fluid -->"));
                    lblMenu.Controls.Add(new LiteralControl("</div><!--/.navbar -->"));
                    lblMenu.Controls.Add(new LiteralControl("</div> <!-- /container -->"));
                }
            }
        }

        #endregion

        #region "Bind Data"

        private DataTable BindData(int ItemID)
        {
            //SqlDataProvider objSQL = new SqlDataProvider();
            DataTable objBind = _wpClass.FillTable("Pro_Menu_List", new ObjectPara("@ItemID", ItemID), new ObjectPara("@IsVisible", 1));

            //DataTable objBind = _rpClass.Pro_Menu_List(-1, 1).Tables[0];
            return objBind;
        }

        #endregion

        #region "Event Handles"

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (Page.IsPostBack == false)
                {
                    CreateMenu();
                }

            }
            catch
            {
            }
        }

        #endregion
    }
}