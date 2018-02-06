using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.KhachHang.Power
{
    public partial class NhapKHMoiPoTS : Authentication
    {
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();        
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();             
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly NhanVienDao _nvDao = new NhanVienDao();

        #region Bien Loc,Up
        private Mode UpdateMode
        {
            get
            {
                try
                {
                    if (Session[SessionKey.MODE] != null)
                    {
                        var mode = Convert.ToInt32(Session[SessionKey.MODE]);
                        return (mode == Mode.Update.GetHashCode()) ? Mode.Update : Mode.Create;
                    }
                    return Mode.Create;
                }
                catch (Exception)
                {
                    return Mode.Create;
                }
            }
            set
            {
                Session[SessionKey.MODE] = value.GetHashCode();
            }
        }
        #endregion

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

        private void UnblockWaitingDialog()
        {
            ((PO)Page.Master).UnblockWaitingDialog();
        }

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    //BindKhachHangGrid();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void LoadStaticReferences()
        {
            try
            {
                ClearForm();
                timkv();

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var dotin = _diDao.GetListKVDDNotP7(_kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO);
                ddlDOTGCS.Items.Clear();
                ddlDOTGCS.Items.Add(new System.Web.UI.WebControls.ListItem("Tất cả", "%"));
                foreach (var d in dotin)
                {
                    ddlDOTGCS.Items.Add(new System.Web.UI.WebControls.ListItem(_dmdiDao.Get(d.MADOTIN).TENDOTIN, d.IDMADOTIN));
                }
            }
            catch { }
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

                if (a.MAKV == "99")
                {
                    var kvList = _kvpoDao.GetList();
                    ddlKHUVUC1.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC1.Items.Add(new System.Web.UI.WebControls.ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    var kvList = _kvpoDao.GetListPo(a.MAKV);
                    ddlKHUVUC1.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC1.Items.Add(new System.Web.UI.WebControls.ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
            }
        }

        private void ClearForm()
        {           
            //hdfIDKH.Value = "";

            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString();
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_NHAPMOIKHACHHANG;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_NHAPMOIKHACHHANG;
            }

            //CommonFunc.SetPropertiesForGrid(gvHopDong);
            CommonFunc.SetPropertiesForGrid(gvKhachHang);
        }

        private void BindKhachHangGrid()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
                        
            //var list = _rpClass.BienKHPo("", _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO, "", "", Int16.Parse(ddlTHANG.SelectedValue.ToString()), Int32.Parse(txtNAM.Text.Trim()), "KHCBIKTTSPO");
            var list = _rpClass.BienKHPo(ddlDOTGCS.SelectedValue, _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO, "", "", Int16.Parse(ddlTHANG.SelectedValue.ToString()), Int32.Parse(txtNAM.Text.Trim()), "KHCBIKTTSPO");

            gvKhachHang.DataSource = list;
            //gvKhachHang.PagerInforText = list.Count.ToString();
            gvKhachHang.DataBind();

            upnlCustomers.Update();
        }

        protected void gvKhachHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                /* var id = e.CommandArgument.ToString();

                 switch (e.CommandName)
                 {
                     case "SelectHD":
                         UnblockWaitingDialog();
                         var kh = khDao.Get(id);
                         KhachHang = kh;
                         SetControlEnable();

                         UpdateMode = Mode.Update;
                         CloseWaitingDialog();
                         break;
                 } */
                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvKhachHang_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvKhachHang.PageIndex = e.NewPageIndex;
                BindKhachHangGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvKhachHang_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;


            var hfGCS = e.Row.FindControl("hfGCS") as HiddenField;

            var txtCHISODAUKHM = e.Row.FindControl("txtCHISODAUKHM") as TextBox;
            var txtCHISOCUOIKHM = e.Row.FindControl("txtCHISOCUOIKHM") as TextBox;

            if (hfGCS == null || txtCHISODAUKHM == null || txtCHISOCUOIKHM == null) return;

            var onKeyDownEventHandler = "javascript:onKeyDownEventHandler(\"" + txtCHISODAUKHM.ClientID +
                                                                "\", \"" + txtCHISOCUOIKHM.ClientID +
                                                                "\", \"" + hfGCS.ClientID +
                                                                "\"";
            txtCHISODAUKHM.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 1, event);");
            txtCHISOCUOIKHM.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 2, event);");

            txtCHISODAUKHM.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtCHISODAUKHM.ClientID + "\");");
            txtCHISOCUOIKHM.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtCHISOCUOIKHM.ClientID + "\");");

            var onSelectedIndexChangedEventHandler = "javascript:onSelectedIndexChangedEventHandler(\"" + txtCHISODAUKHM.ClientID +
                                                                "\", \"" + txtCHISOCUOIKHM.ClientID +
                                                                "\", \"" + hfGCS.ClientID +
                                                                "\"";            
        }

        protected void btLOCDSKHCBKT_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var querykv = _nvDao.GetKV(b);

                int thangIndex = 0;
                if (DateTime.Now.Month == 12)
                {
                    thangIndex = 1;
                }
                else
                {
                    thangIndex = DateTime.Now.Month;
                }

                int namIndex = DateTime.Now.Year - 1;
                //lock cap nhap chi so
                int thang1 = DateTime.Now.Month;
                string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                var kynay = new DateTime(int.Parse(nam), thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);
                bool dung = _gcspoDao.IsLockTinhCuocKy(kynay, _kvpoDao.GetPo(querykv.MAKV).MAKVPO);//khoa so trong ky

                if (txtNAM.Text == Convert.ToString(nam))// || txtNAM.Text == Convert.ToString(namIndex))
                {
                    if (int.Parse(ddlTHANG.SelectedValue) == thang1)
                    {
                        if (dung == false)
                        {
                            BindKhachHangGrid();
                            CloseWaitingDialog();
                            upnlCustomers.Update();
                        }
                        else
                        {
                            CloseWaitingDialog();
                            HideDialog("divKhachHang");
                            ShowInfor("Đã khoá sổ. Không được Nhập khách hàng mới.");
                        }
                    }
                    else
                    {
                        //BindKhachHangGrid();
                        //CloseWaitingDialog();
                        //upnlCustomers.Update();

                        CloseWaitingDialog();
                        HideDialog("divKhachHang");
                        ShowInfor("Chọn kỳ nhập khách hàng mới cho đúng.");
                    }
                }
                else
                {
                    CloseWaitingDialog();
                    HideDialog("divKhachHang");
                    ShowInfor("Chọn năm nhập khách hàng mới cho đúng.");
                }

            }
            catch { }
        }


    }
}