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

namespace EOSCRM.Web.Forms.GhiChiSo.Power
{
    public partial class GhiChiSoPo : Authentication
    {
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();        
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();

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
            Page.Title = Resources.Message.TITLE_GCS_GHICHISOPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_GHICHISOPO;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void SetLabel(string id, string value)
        {
            ((PO)Page.Master).SetLabel(id, value);
        }

        private void ShowError(string message, string controlId)
        {
            ((PO)Page.Master).ShowError(message, controlId);
        }

        private void ShowError(string message)
        {
            ((PO)Page.Master).ShowError(message);
        }

        private void ShowInfor(string message)
        {
            ((PO)Page.Master).ShowInfor(message);
        }

        private void ShowWarning(string message)
        {
            ((PO)Page.Master).ShowWarning(message);
        }

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
        }

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }
        #endregion

        private void LoadStaticReferences()
        {
            /*var listKhuVuc = new KhuVucPoDao().GetList();
            ddlKHUVUC.DataSource = listKhuVuc;
            ddlKHUVUC.DataTextField = "TENKV";
            ddlKHUVUC.DataValueField = "MAKVPO";
            ddlKHUVUC.DataBind();*/
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

                if (a.MAKV == "99" && b == "nguyen")
                {
                    var kvList = _kvpoDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                    btnSave.Visible = true;
                }
                else if (a.MAKV == "99")
                {
                    var kvList = _kvpoDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    //var kvList = _kvpoDao.GetListKV(d);
                    var kvList = _kvpoDao.GetListKVPO(_nvDao.Get(b).MAKV);

                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
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
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return false;
            string b = loginInfo.Username;
            var nhanvien = _nvDao.Get(b);

            int thang1 = DateTime.Now.Month;
            string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            var kynay1 = new DateTime(int.Parse(nam), thang1, 1);

            if (nhanvien.MAPB != "KD")
            {
                if (txtMADP.Text.Trim() == "")
                    return false;

                var dp = _dppoDao.GetDP(txtMADP.Text.Trim());
                if (dp == null) return false;

                var kynay = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);
                var list = _gcspoDao.GetListPoSH(kynay, dp);

                gvList.DataSource = list;
                gvList.PagerInforText = list.Count.ToString(CultureInfo.InvariantCulture);
                gvList.DataBind();

                //TODO: kiểm tra kỳ khai thác có bị lock tính cước trên đường được chọn hay không
                gvList.Enabled = false;
                divList.Visible = true;
                //divWarning.Visible = dp.GIAKHAC.HasValue && dp.GIAKHAC.Value;

                upnlGrid.Update();

                return true;
            }
            else
            {
                if (txtMADP.Text.Trim() == "")
                    return false;

                var dp = _dppoDao.GetDP(txtMADP.Text.Trim());
                if (dp == null) return false;

                var kynay = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);
                var list = _gcspoDao.GetListPoSH(kynay, dp);

                gvList.DataSource = list;
                gvList.PagerInforText = list.Count.ToString(CultureInfo.InvariantCulture);
                gvList.DataBind();

                //TODO: kiểm tra kỳ khai thác có bị lock tính cước trên đường được chọn hay không
                gvList.Enabled = !_gcspoDao.IsLockTinhCuoc(kynay, dp);
                divList.Visible = true;
                //divWarning.Visible = dp.GIAKHAC.HasValue && dp.GIAKHAC.Value;

                upnlGrid.Update();

                return true;
            }

            //bool p7d1 = _gcspoDao.IsLockDotInHD(kynay1, ddlKHUVUC.SelectedValue, "DDP7D1");//phien 7 , kh muc dich khac, ngoai sinh hoat

            //if (p7d1 == true)
            //{
            //    if (txtMADP.Text.Trim() == "")
            //        return false;

            //    var dp = _dppoDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
            //    if (dp == null) return false;

            //    var kynay = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);
            //    var list = _gcspoDao.GetListPoP7D1(kynay, dp);

            //    gvList.DataSource = list;
            //    gvList.PagerInforText = list.Count.ToString(CultureInfo.InvariantCulture);
            //    gvList.DataBind();

            //    //TODO: kiểm tra kỳ khai thác có bị lock tính cước trên đường được chọn hay không
            //    gvList.Enabled = !_gcspoDao.IsLockTinhCuoc(kynay, dp);
            //    divList.Visible = true;
            //    //divWarning.Visible = dp.GIAKHAC.HasValue && dp.GIAKHAC.Value;

            //    upnlGrid.Update();
               
            //    return true; ;
            //}
            //else // khach hang binh thuong
            //{
            //    if (txtMADP.Text.Trim() == "")
            //        return false;

            //    var dp = _dppoDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
            //    if (dp == null) return false;

            //    var kynay = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);
            //    var list = _gcspoDao.GetListPoSH(kynay, dp);

            //    gvList.DataSource = list;
            //    gvList.PagerInforText = list.Count.ToString(CultureInfo.InvariantCulture);
            //    gvList.DataBind();

            //    //TODO: kiểm tra kỳ khai thác có bị lock tính cước trên đường được chọn hay không
            //    gvList.Enabled = !_gcspoDao.IsLockTinhCuoc(kynay, dp);
            //    divList.Visible = true;
            //    //divWarning.Visible = dp.GIAKHAC.HasValue && dp.GIAKHAC.Value;

            //    upnlGrid.Update();

            //    return true;
            //}
        }        

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
            //var list = _dppoDao.GetList("%", txtKeywordDP.Text.Trim());
            var list = _dppoDao.GetList(ddlKHUVUC.SelectedValue, txtKeywordDP.Text.Trim());

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
                        var dp = _dppoDao.Get(res[0], res[1]);
                        if (dp != null)
                        {
                            SetControlValue(txtMADP.ClientID, dp.MADPPO);
                            SetControlValue(txtDUONGPHU.ClientID, dp.DUONGPHUPO);

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
            try
            {
                var nam = int.Parse(txtNAM.Text.Trim());
                var thang = int.Parse(ddlTHANG.SelectedValue);
                var kv = ddlKHUVUC.SelectedValue;

                var msg = _gcspoDao.TinhTien(thang, nam, kv);

                CloseWaitingDialog();
                ShowInfor(ResourceLabel.Get(msg));
            }
            catch { }
        }

        private void ExportToExcel(string strFileName, DataGrid dg)
        {
            this.EnableViewState = false;
            Response.Clear();
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "filename=" + strFileName);
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
            var data = _gcspoDao.GetLitForExport(kynay, ddlLOAIKH.SelectedValue, txtMADP.Text.Trim());
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
                r[0] = row.SODBPO;
                r[1] = row.KHACHHANGPO.TENKH;
                r[2] = row.KHACHHANGPO.TENKH;
                r[3] = row.KHACHHANGPO.TENKH;
                r[4] = row.KHACHHANGPO.TENKH;
                r[5] = row.KHACHHANGPO.TENKH;
                r[6] = row.KHACHHANGPO.TENKH;
                r[7] = row.KHACHHANGPO.TENKH;
                r[8] = row.KHACHHANGPO.TENKH;
                r[9] = row.KHACHHANGPO.TENKH;
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