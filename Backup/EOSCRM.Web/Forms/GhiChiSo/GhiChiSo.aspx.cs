using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Web.UI;
using System.Data;
using CrystalDecisions.Shared;

namespace EOSCRM.Web.Forms.GhiChiSo
{
    public partial class GhiChiSo : Authentication
    {
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly GhiChiSoDao gcsDao = new GhiChiSoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                   
                   
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_GCS_GHICHISO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_GHICHISO;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
        }

        private void SetLabel(string id, string value)
        {
            ((EOS)Page.Master).SetLabel(id, value);
        }

        private void ShowError(string message, string controlId)
        {
            ((EOS)Page.Master).ShowError(message, controlId);
        }

        private void ShowError(string message)
        {
            ((EOS)Page.Master).ShowError(message);
        }

        private void ShowInfor(string message)
        {
            ((EOS)Page.Master).ShowInfor(message);
        }

        private void ShowWarning(string message)
        {
            ((EOS)Page.Master).ShowWarning(message);
        }

        private void HideDialog(string divId)
        {
            ((EOS)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
        }

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        private void LoadStaticReferences()
        {
            var listKhuVuc = new KhuVucDao().GetList();
            ddlKHUVUC.DataSource = listKhuVuc;
            ddlKHUVUC.DataTextField = "TENKV";
            ddlKHUVUC.DataValueField = "MAKV";
            ddlKHUVUC.DataBind();
            timkv();
            ClearForm();
        }

        public void timkv()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvDao.GetListKV(b);
            foreach (var a in query)
            {
                string d = a.MAKV;

                if (a.MAKV == "99" && b=="nguyen" )
                {
                    var kvList = _kvDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                    btnSave.Visible = true;
                }
                else if (a.MAKV == "99" )
                {
                    var kvList = _kvDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                    
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }
        
        private void ClearForm()
        {
            /*
             * clear phần thông tin hồ sơ
             */
            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString();
            txtMADP.Text = "";
            txtDUONGPHU.Text = "";
            lblTENDUONG.Text = "";
        }

        private bool BindData()
        {            
            if (txtMADP.Text.Trim() == "")
                return false;

            var dp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
            if (dp == null) return false;

            var kynay = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);
            var list = gcsDao.GetList(kynay, dp);

            gvList.DataSource = list;
            gvList.PagerInforText = list.Count.ToString(CultureInfo.InvariantCulture);
            gvList.DataBind();

            //TODO: kiểm tra kỳ khai thác có bị lock tính cước trên đường được chọn hay không
            gvList.Enabled = !gcsDao.IsLockTinhCuoc(kynay, dp);
            divList.Visible = true;
            //divWarning.Visible = dp.GIAKHAC.HasValue && dp.GIAKHAC.Value;

            upnlGrid.Update();

            return true;
        }

       /* private bool BindData()
        {
            if (ddlLOAIKH.SelectedValue == "TG" && txtMADP.Text.Trim() == "")
                return false;

            var dp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
            if (dp == null) return false;

            var kynay = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);
            var list = gcsDao.GetList(kynay, ddlLOAIKH.SelectedValue, dp);

            gvList.DataSource = list;
            gvList.PagerInforText = list.Count.ToString(CultureInfo.InvariantCulture);
            gvList.DataBind();

            divList.Visible = true;
            
            upnlGrid.Update();

            return true;
        }*/
        
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Update page index
            gvList.PageIndex = 0;

            if (!BindData())
            {
                CloseWaitingDialog();
                ShowError("Chọn đường phố để ghi chỉ số.", txtMADP.ClientID);
                return;
            }

            CloseWaitingDialog();
        }

        
        #region Đường phố
        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
        }

        private void BindDuongPho()
        {
            var list = dpDao.GetList("%", txtKeywordDP.Text.Trim());
            gvDuongPho.DataSource = list;
            gvDuongPho.PagerInforText = list.Count.ToString();
            gvDuongPho.DataBind();

            upnlDuongPho.Update();

            CloseWaitingDialog();
        }

        protected void btnBrowseDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            upnlDuongPho.Update();
            UnblockDialog("divDuongPho");
        }

        protected void gvDuongPho_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMADP":
                        var res = id.Split('-');
                        var dp = dpDao.Get(res[0], res[1]);
                        if (dp != null)
                        {
                            SetControlValue(txtMADP.ClientID, dp.MADP);
                            SetControlValue(txtDUONGPHU.ClientID, dp.DUONGPHU);

                            upnlGhiChiSo.Update();

                            HideDialog("divDuongPho");
                            CloseWaitingDialog();

                            txtMADP.Focus();
                        }

                        break;

                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #endregion



        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var hfGCS = e.Row.FindControl("hfGCS") as HiddenField;
            var ddlTTHAIGHI = e.Row.FindControl("ddlTTHAIGHI") as DropDownList;
            var txtCHISODAU = e.Row.FindControl("txtCHISODAU") as TextBox;
            var txtCHISOCUOI = e.Row.FindControl("txtCHISOCUOI") as TextBox;
            var txtKLTIEUTHU = e.Row.FindControl("txtKLTIEUTHU") as TextBox;

            if (hfGCS == null || txtCHISODAU == null || txtCHISOCUOI == null ||
                txtKLTIEUTHU == null || ddlTTHAIGHI == null) return;

            var onKeyDownEventHandler = "javascript:onKeyDownEventHandler(\"" + txtCHISODAU.ClientID +
                                                                "\", \"" + txtCHISOCUOI.ClientID +
                                                                "\", \"" + txtKLTIEUTHU.ClientID +
                                                                "\", \"" + ddlTTHAIGHI.ClientID +
                                                                "\", \"" + hfGCS.ClientID +
                                                                "\"";
            txtCHISODAU.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 1, event);");
            txtCHISOCUOI.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 2, event);");
            txtKLTIEUTHU.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 3, event);");
            ddlTTHAIGHI.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 4, event);");

            //txtCHISODAU.Attributes.Add("onkeypress", onKeyDownEventHandler + ", 1, event);");
            //txtCHISOCUOI.Attributes.Add("onkeypress", onKeyDownEventHandler + ", 2, event);");
            //txtKLTIEUTHU.Attributes.Add("onkeypress", onKeyDownEventHandler + ", 3, event);");
            //ddlTTHAIGHI.Attributes.Add("onkeypress", onKeyDownEventHandler + ", 4, event);");

            txtCHISODAU.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtCHISODAU.ClientID + "\");");
            txtCHISOCUOI.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtCHISOCUOI.ClientID + "\");");
            txtKLTIEUTHU.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtKLTIEUTHU.ClientID + "\");");
            ddlTTHAIGHI.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + ddlTTHAIGHI.ClientID + "\");");

            var onSelectedIndexChangedEventHandler = "javascript:onSelectedIndexChangedEventHandler(\"" + txtCHISODAU.ClientID +
                                                                "\", \"" + txtCHISOCUOI.ClientID +
                                                                "\", \"" + txtKLTIEUTHU.ClientID +
                                                                "\", \"" + ddlTTHAIGHI.ClientID +
                                                                "\", \"" + hfGCS.ClientID +
                                                                "\"";
            //txtCHISODAU.Attributes.Add("onchange", onSelectedIndexChangedEventHandler + ");");
            ddlTTHAIGHI.Attributes.Add("onchange", onSelectedIndexChangedEventHandler + ");");
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvList.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindData();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var nam = int.Parse(txtNAM.Text.Trim());
            var thang = int.Parse(ddlTHANG.SelectedValue);
            var kv = ddlKHUVUC.SelectedValue;

            var msg = gcsDao.TinhTien(thang, nam, kv);

            CloseWaitingDialog();
            ShowInfor(ResourceLabel.Get(msg));
        }
        private void ExportToExcel(string strFileName, DataGrid dg)
        {
            this.EnableViewState = false;
            Response.Clear();
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "filename="+strFileName);
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            dg.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            // get data from madp,thang,nam
            var kynay = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);
            var data = gcsDao.GetLitForExport(kynay, ddlLOAIKH.SelectedValue, txtMADP.Text.Trim());
            if (data.Count == 0)
                return;
            // format 
            

            string[][] arrCols = new string[][]{
                    new string[]{"Số danh bộ","SODB","90"},
                    new string[]{"Tên khách hàng","KHACHHANG.TENKH","300"},
                    new string[]{"Đối tượng","KHACHHANG.MDSD.TENMDSD","500"},
                    new string[]{"Phường","KHACHHANG.TENKH","100", ""},
                    new string[]{"Khu vực","KHACHHANG.TENKH","1000"},
                    new string[]{"Số nhà","KHACHHANG.TENKH","100"},
                    new string[]{"Đường","KHACHHANG.TENKH","100"},
                    new string[]{"Chỉ số cũ","CHISOCU","60"}
            };
            DataGrid dg = new DataGrid();
            DataTable dt = new DataTable();
            dt.Columns.Add("SODB");
            dt.Columns.Add("TENKH");
            dt.Columns.Add("TENMDSD");
            dt.Columns.Add("PHUONG");
            dt.Columns.Add("KHUVUC");
            dt.Columns.Add("SONHA");
            dt.Columns.Add("DUONG");
            dt.Columns.Add("CHISOCU");
            dt.Columns.Add("CHISOMOI");
            dt.Columns.Add("NGAYGHICHISO");
            DataRow r;
            foreach (var row in data)
            {
                r = dt.NewRow();
                r[0] = row.SODB;
                r[1] = row.KHACHHANG.TENKH;
                r[2] = row.KHACHHANG.TENKH;
                r[3] = row.KHACHHANG.TENKH;
                r[4] = row.KHACHHANG.TENKH;
                r[5] = row.KHACHHANG.TENKH;
                r[6] = row.KHACHHANG.TENKH;
                r[7] = row.KHACHHANG.TENKH;
                r[8] = row.KHACHHANG.TENKH;
                r[9] = row.KHACHHANG.TENKH;
                dt.Rows.Add(r);
            }
            DiskFileDestinationOptions destOptions = new DiskFileDestinationOptions();
            CrystalDecisions.CrystalReports.Engine.ReportDocument repDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            repDoc.SetDataSource(dt);
            repDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            repDoc.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;            
            repDoc.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;


            repDoc.ExportOptions.DestinationOptions = destOptions;
            repDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "");                                     
            repDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, false, "");  



            dg.DataSource = dt;
            dg.DataBind();
            this.EnableViewState = false;
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "filename=GhiChiSo.xls");
            Response.Charset = "UTF-8";
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            dg.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());
            Response.End();
        }
    }
}
