using System;
using System.Web.UI.WebControls;
using System.Globalization;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Collections.Generic;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;

namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class CapNhatSoBoTS : Authentication
    {
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly KhachHangDao khDao = new KhachHangDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly GhiChiSoDao _gcsDao = new GhiChiSoDao();
        private readonly ReportClass _rpClass = new ReportClass();

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

        private void ShowError(string message)
        {
            ((EOS)Page.Master).ShowError(message);
        }

        private void ShowInfor(string message)
        {
            ((EOS)Page.Master).ShowInfor(message);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_CapNhatSoBoTS, Permission.Read);
                AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                }

                if (reloadm.Text == "1")
                {          
                    BCTachDuong();
                    CloseWaitingDialog();
                    gvList.Visible = false;
                    upnlGrid.Update();
                } 
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_CAPNHATSOBO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_CAPNHATSOBO;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }        

        private void LoadStaticReferences()
        {
            ClearForm();
        }

        private void ClearForm()
        {
            ddlTHANG1.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM1.Text = DateTime.Now.Year.ToString();

            txtMADP.Text = "";
            txtDUONGPHU.Text = "";
            lblTENDUONG.Text = "";

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var dotin = _diDao.GetListKVNN(_nvDao.Get(b).MAKV);
            ddlDOTGCS.Items.Clear();
            ddlDOTGCS.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var d in dotin)
            {
                ddlDOTGCS.Items.Add(new ListItem(_dmdiDao.Get(d.MADOTIN).TENDOTIN, d.IDMADOTIN));
            }
        }

        protected bool BindData()
        {
            if (txtMADP.Text.Trim() == "")
                return false;

            //var list = khDao.GetList(txtMADP.Text.Trim());
            var list = khDao.GetListTS(txtMADP.Text.Trim());

            gvList.DataSource = list;
            gvList.PagerInforText = list.Count.ToString();
            gvList.DataBind();

            hfMADP.Value = txtMADP.Text.Trim();
            divList.Visible = true;

            return true;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.GetKV(b);

            int thang1 = DateTime.Now.Month;
            string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            var kyht = new DateTime(int.Parse(nam), thang1, 1);

            if (string.IsNullOrEmpty(txtMADP.Text.Trim()))
            {
                CloseWaitingDialog();
                ShowError("Chọn đường phố. Kiểm tra lại", txtMADP.ClientID);
                return;
            }

            bool dung2 = _gcsDao.IsLockTinhCuocKy1(kyht, query.MAKV.ToString(), txtMADP.Text.Trim());
            if (dung2 == true)
            {
                CloseWaitingDialog();
                ShowInfor("Đã khoá sổ ghi chỉ số theo đợt ghi.");
                return;
            }

            //if (!BindData())
            if (!BindDataTD())
            {
                CloseWaitingDialog();
                ShowError("Chọn đường phố để lọc.", txtMADP.ClientID);
                return;
            }            

            upnlGrid.Update();

            CloseWaitingDialog();
        }

        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
        }

        private void BindDuongPho()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            string makv = _nvDao.Get(b).MAKV;

            var list = dpDao.GetList(makv, txtKeywordDP.Text.Trim());
            gvDuongPho.DataSource = list;
            gvDuongPho.PagerInforText = list.Count.ToString();
            gvDuongPho.DataBind();

            CloseWaitingDialog();
        }

        private void UpdateKhuVuc(DUONGPHO dp)
        {
            SetLabel(lblTENDUONG.ClientID, dp.TENDP);
        }

        protected void btnBrowseDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            upnlDuongPho.Update();
            UnblockDialog("divDuongPho");
        }

        #region Đường phố             
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
                            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                            if (loginInfo == null) return;
                            string b = loginInfo.Username;
                            var querykv = _nvDao.GetKV(b);

                            int thang1 = DateTime.Now.Month;
                            string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);

                            //var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
                            var kynay1 = new DateTime(int.Parse(txtNAM1.Text.Trim()), int.Parse(ddlTHANG1.SelectedValue), 1);
                            bool dung = _gcsDao.IsLockTinhCuocKy1(kynay1, querykv.MAKV, dp.MADP);
                            if (dung == true)
                            {
                                CloseWaitingDialog();
                                ShowInfor("Đã khoá sổ ghi chỉ số.");
                                return;
                            }

                            if (int.Parse(ddlTHANG1.SelectedValue) != thang1 + 1)
                            {
                                CloseWaitingDialog();
                                ShowInfor("Sau 1 kỳ. Chọn kỳ cho đúng.");
                                return;
                            }

                            txtMADP.Text = dp.MADP;
                            SetControlValue(txtDUONGPHU.ClientID, dp.DUONGPHU);

                            UpdateKhuVuc(dp);
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

        protected void gvDuongPho_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvDuongPho.PageIndex = e.NewPageIndex;               
                BindDuongPho();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDuongPho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }
        #endregion

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var txtSTT = e.Row.FindControl("txtSTT") as TextBox;
            var txtTENKH = e.Row.FindControl("txtTENKH") as TextBox;
            var txtSONHA = e.Row.FindControl("txtSONHA") as TextBox;
            var txtMADPG = e.Row.FindControl("txtMADPG") as TextBox;
            var txtMADBG = e.Row.FindControl("txtMADBG") as TextBox;
            var hfCNSB = e.Row.FindControl("hfCNSB") as HiddenField;

            if (txtSTT == null || txtTENKH == null || txtSONHA == null || hfCNSB == null
                || txtMADPG == null || txtMADBG == null) return;

            var onBlurEventHandler = "javascript:onBlurEventHandler(\"" + txtSTT.ClientID +
                                                                "\", \"" + txtTENKH.ClientID +
                                                                "\", \"" + txtSONHA.ClientID +
                                                                "\", \"" + txtMADPG.ClientID +
                                                                "\", \"" + txtMADBG.ClientID +
                                                                "\", \"" + hfCNSB.ClientID +
                                                                "\", \"" + LoginInfo.MANV +
                                                                "\"";
            var onKeyDownEventHandler = "javascript:onKeyDownEventHandler(\"" + txtSTT.ClientID +
                                                                "\", \"" + txtTENKH.ClientID +
                                                                "\", \"" + txtSONHA.ClientID +
                                                                "\", \"" + txtMADPG.ClientID +
                                                                "\", \"" + txtMADBG.ClientID +
                                                                "\", \"" + hfCNSB.ClientID +
                                                                "\"";

            txtSTT.Attributes.Add("onblur", onBlurEventHandler + ", 1);");
            txtTENKH.Attributes.Add("onblur", onBlurEventHandler + ", 2);");
            txtSONHA.Attributes.Add("onblur", onBlurEventHandler + ", 3);");
            txtMADPG.Attributes.Add("onblur", onBlurEventHandler + ", 4);");
            txtMADBG.Attributes.Add("onblur", onBlurEventHandler + ", 5);");

            txtSTT.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 1, event);");
            txtTENKH.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 2, event);");
            txtSONHA.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 3, event);");
            txtMADPG.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 4, event);");
            txtMADBG.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 5, event);");
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvList.PageIndex = e.NewPageIndex;             
                //BindData();
                BindDataTD();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void linkBtnHidden_Click(object sender, EventArgs e)
        {
            var madp = hfMADP.Value;

            var list = khDao.GetList(madp);
            gvList.DataSource = list;
            gvList.PagerInforText = list.Count.ToString();
            gvList.DataBind();

            CloseWaitingDialog();
        }

        protected bool BindDataTD()
        {
            //if (txtMADP.Text.Trim() == "")
            //    return false;

            //var list = khDao.GetList(txtMADP.Text.Trim());
            //var list = khDao.GetListTS(txtMADP.Text.Trim());//, Int16.Parse(ddlTHANG1.SelectedValue), Int16.Parse(txtNAM1.Text.Trim()));
            var list = _rpClass.InTachDuong(Int16.Parse(ddlTHANG1.SelectedValue), Int16.Parse(txtNAM1.Text.Trim()), txtMADP.Text.Trim(), "nguyen", "listtach");

            gvList.DataSource = list;
            //gvList.PagerInforText = list.Count.ToString();
            gvList.DataBind();

            hfMADP.Value = txtMADP.Text.Trim();
            divList.Visible = true;

            return true;
        }

        #region gv Tach duong  
        protected void gvTACHDUONG_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {              
                gvTACHDUONG.PageIndex = e.NewPageIndex;              
                BindDataTD();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvTACHDUONG_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var txtSTT = e.Row.FindControl("txtSTTTD") as TextBox;
            var txtTENKH = e.Row.FindControl("txtTENKHTD") as TextBox;
            var txtSONHA = e.Row.FindControl("txtSONHATD") as TextBox;
            var txtMADPG = e.Row.FindControl("txtMADPGTD") as TextBox;
            var txtMADBG = e.Row.FindControl("txtMADBGTD") as TextBox;
            var hfCNSB = e.Row.FindControl("hfCNSBTD") as HiddenField;

            if (txtSTT == null || txtTENKH == null || txtSONHA == null || hfCNSB == null
                || txtMADPG == null || txtMADBG == null) return;

            var onBlurEventHandler = "javascript:onBlurEventHandlerTD(\"" + txtSTT.ClientID +
                                                                "\", \"" + txtTENKH.ClientID +
                                                                "\", \"" + txtSONHA.ClientID +
                                                                "\", \"" + txtMADPG.ClientID +
                                                                "\", \"" + txtMADBG.ClientID +
                                                                "\", \"" + hfCNSB.ClientID +
                                                                "\", \"" + LoginInfo.MANV +
                                                                "\"";
            var onKeyDownEventHandler = "javascript:onKeyDownEventHandlerTD(\"" + txtSTT.ClientID +
                                                                "\", \"" + txtTENKH.ClientID +
                                                                "\", \"" + txtSONHA.ClientID +
                                                                "\", \"" + txtMADPG.ClientID +
                                                                "\", \"" + txtMADBG.ClientID +
                                                                "\", \"" + hfCNSB.ClientID +
                                                                "\"";

            txtSTT.Attributes.Add("onblur", onBlurEventHandler + ", 1);");
            txtTENKH.Attributes.Add("onblur", onBlurEventHandler + ", 2);");
            txtSONHA.Attributes.Add("onblur", onBlurEventHandler + ", 3);");
            txtMADPG.Attributes.Add("onblur", onBlurEventHandler + ", 4);");
            txtMADBG.Attributes.Add("onblur", onBlurEventHandler + ", 5);");

            txtSTT.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 1, event);");
            txtTENKH.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 2, event);");
            txtSONHA.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 3, event);");
            txtMADPG.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 4, event);");
            txtMADBG.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 5, event);");
        }        
        #endregion

        protected void btSAVE_Click(object sender, EventArgs e)
        {
            try
            {
                if (!HasPermission(Functions.KH_CapNhatSoBoTS, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                //lock cap nhap chi so
                int thang1 = DateTime.Now.Month;
                string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.GetKV(b);

                int tdn = _gcsDao.IsLockTachDuongNKy(int.Parse(ddlTHANG1.SelectedValue), int.Parse(txtNAM1.Text.Trim()),
                            query.MAKV.ToString(), txtMADP.Text.Trim());
                if (tdn > 1)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đường phố đã tách rồi. Chọn đường khác.");
                    return;
                }
                                
                //bool dung = _gcsDao.IsLockTinhCuocKy(kynay1, query.MAKV.ToString());                
                //if (dung == true)
                //{
                //    CloseWaitingDialog();
                //    ShowInfor("Đã khoá sổ ghi chỉ số.");
                //    return;
                //}

                var kynay2 = new DateTime(int.Parse(txtNAM1.Text.Trim()), int.Parse(ddlTHANG1.SelectedValue), 1);
                bool dung2 = _gcsDao.IsLockTinhCuocKy(kynay2, query.MAKV.ToString());
                if (dung2 == true)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đã khoá sổ ghi chỉ số theo kỳ.");
                    return;
                }

                int thangtd = Int16.Parse(ddlTHANG1.SelectedValue);
                int namtd = Int16.Parse(txtNAM1.Text.Trim());
                string madptd = txtMADP.Text.Trim();

                if (string.IsNullOrEmpty(txtMADP.Text.Trim()))
                {
                    CloseWaitingDialog();
                    ShowInfor("Chọn đường phố. Kiểm tra lại");
                    return;
                }

                //if (thangtd != thang1 + 1)
                //{
                //    CloseWaitingDialog();
                //    ShowInfor("Chọn sau 1 kỳ. Kiểm tra lại kỳ.");
                //    return;
                //}

                _rpClass.InTachDuong(thangtd, namtd, madptd, b, "them");

                lkLOCTACKDUONG.Visible = true;
                //btnFilter.Visible = true;
                btSAVE.Visible = false;
                txtMADP.Text = "";

                upnlGrid.Visible = false;
                upgvTACHDUONG.Visible = true;

                upnlGhiChiSo.Update();
                upnlGrid.Update();
                upgvTACHDUONG.Update();                

                if (!BindDataTD())
                {
                    CloseWaitingDialog();
                    ShowError("Chọn đường phố khi bắt đầu tách.", txtMADP.ClientID);
                    return;
                }
                
                CloseWaitingDialog();
            }
            catch { }
        }

        protected void lkLOCTACKDUONG_Click(object sender, EventArgs e)
        {
            try
            {
                if (!HasPermission(Functions.KH_CapNhatSoBoTS, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                //lock cap nhap chi so
                int thang1 = DateTime.Now.Month;
                string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.GetKV(b);

                bool dung = _gcsDao.IsLockTinhCuocKy(kynay1, query.MAKV.ToString());
                if (dung == true)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đã khoá sổ ghi chỉ số. Vào báo cáo chi tiết tách đường để xem.");
                    return;
                }

                var kynay2 = new DateTime(int.Parse(txtNAM1.Text.Trim()), int.Parse(ddlTHANG1.SelectedValue), 1);
                bool dung2 = _gcsDao.IsLockTinhCuocKy(kynay2, query.MAKV.ToString());
                if (dung2 == true)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đã khoá sổ ghi chỉ số.");
                    return;
                }

                //btnFilter.Visible = true;
                btSAVE.Visible = false;

                if (!BindDataTD())
                {
                    CloseWaitingDialog();
                    ShowError("Chọn đường phố để ghi chỉ số.", txtMADP.ClientID);
                    return;
                }
                CloseWaitingDialog();
                upnlGrid.Update();

                upnlGhiChiSo.Update();
                upgvTACHDUONG.Update();
            }
            catch { }
        }

        protected void lkBAOCAOQUAKH_Click(object sender, EventArgs e)
        {
            try
            {
                if (!HasPermission(Functions.KH_CapNhatSoBoTS, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                //lock cap nhap chi so
                int thang1 = DateTime.Now.Month;
                string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.GetKV(b);

                bool dung = _gcsDao.IsLockTinhCuocKy(kynay1, query.MAKV.ToString());
                
                int thangtd = Int16.Parse(ddlTHANG1.SelectedValue);
                int namtd = Int16.Parse(txtNAM1.Text.Trim());

                if (dung == true)//kiem tra khoa so
                {
                    BaoCaoTachDuong();
                    CloseWaitingDialog();                    
                }
                else
                {
                    //var kynay2 = new DateTime(int.Parse(txtNAM1.Text.Trim()), int.Parse(ddlTHANG1.SelectedValue), 1);
                    var kynay2 = new DateTime(namtd, thangtd, 1);

                    bool dung2 = _gcsDao.IsLockTinhCuocKy(kynay2, query.MAKV.ToString());
                    if (dung2 == true)
                    {
                        CloseWaitingDialog();
                        ShowInfor("Đã khoá sổ ghi chỉ số.");
                        return;
                    }

                    _rpClass.InTachDuong(thangtd, namtd, "nguyen", query.MAKV.ToString(), "upquakh");//update madp,madb,stt qua khách hàng

                    BaoCaoTachDuong();
                }
            }
            catch { }
        }

        private void BaoCaoTachDuong()
        {
            try
            {
                BCTachDuong();
                
                CloseWaitingDialog();
                gvList.Visible = false;
                upnlGrid.Update();
            }
            catch { }
        }

        private void BCTachDuong()
        {
            try
            {
                ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
                if (rp != null)
                {
                    try
                    {
                        rp.Close();
                        rp.Dispose();
                        GC.Collect();
                    }
                    catch { }
                }

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.GetKV(b);
                string tenkv = _kvDao.Get(query.MAKV).TENKV.ToString();

                int thangtd = Int16.Parse(ddlTHANG1.SelectedValue);
                int namtd = Int16.Parse(txtNAM1.Text.Trim());

                DataTable dt = new ReportClass().DSKHTACHDUONG(thangtd, namtd, query.MAKV, 1).Tables[0];
                
                rp = new ReportDocument();

                var path = Server.MapPath("~/Reports/QuanLyKhachHang/DSTachDuong.rpt");                 

                rp.Load(path);

                rp.SetDataSource(dt);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();
                
                TextObject txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
                txtXN.Text = "XN ĐIỆN NƯỚC " + tenkv.ToUpper();
                TextObject txtTIEUDE = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
                txtTIEUDE.Text = "DANH SÁCH KHÁCH HÀNG CẦN XẾP LẠI DANH SỐ";
                //txtTuNgay
                TextObject txtTuNgay = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
                txtTuNgay.Text = "Kỳ: " + ddlTHANG1.SelectedValue + "/" + txtNAM1.Text.Trim();
                var d = DateTime.Now.Day;
                var m = DateTime.Now.Month;
                var y = DateTime.Now.Year;
                TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
                ngay.Text = tenkv + ", ngày " + d + " tháng " + m + " năm " + y;                

                divCR.Visible = true;
                upnlCrystalReport.Update();

                reloadm.Text = "1";

                Session["DS_DonDangKy"] = dt;
                Session[Constants.REPORT_FREE_MEM] = rp;

                CloseWaitingDialog();
                upnlCrystalReport.Update();
            }
            catch { }
        }

    }
}