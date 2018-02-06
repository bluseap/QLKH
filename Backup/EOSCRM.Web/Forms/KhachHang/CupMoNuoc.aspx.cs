using System;
using System.Globalization;
using System.Web.UI.WebControls;
using EOSCRM.Domain;
using EOSCRM.Util ;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;


namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class CupMoNuoc : Authentication
    {
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
      
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.CN_NhapCongNo, Permission.Read);

                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_CUPMONUOC;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_CUPMONUOC;
            }

            CommonFunc.SetPropertiesForGrid(gvDanhSach);
            CommonFunc.SetPropertiesForGrid(gvKhachHang);
        }

        private void LoadStaticReferences()
        {
            //TODO: Load các đối tượng có liên quan lên UI
            try
            {

                var kvList = _kvDao.GetList();

                ddlKHUVUC.Items.Clear();
                ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in kvList)
                {
                    ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                }

                txtNAM.Text = DateTime.Now.Year.ToString();
                ddlTHANG.SelectedIndex = DateTime.Now.Month - 1; 
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
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
       
        #region Khách hàng
        protected void btnFilterKH_Click(object sender, EventArgs e)
        {
            BindKhachHang();
            upnlKhachHang.Update();
            CloseWaitingDialog();
        }

        private void BindKhachHang()
        {
            var danhsach = _khDao.SearchKhachHang(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(),
                                                           txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
                                                           txtSONHA.Text.Trim(), txtTENDP.Text.Trim(),
                                                           ddlKHUVUC.SelectedValue.Trim());
            gvDanhSach.DataSource = danhsach;
            gvDanhSach.PagerInforText = danhsach.Count.ToString();
            cpeFilter.Collapsed = true;
            gvDanhSach.DataBind();
            tdDanhSach.Visible = true;

            upnlKhachHang.Update();
        }

        protected void btnBrowseKH_Click(object sender, EventArgs e)
        {
            UnblockDialog("divKhachHang");
        }

        private void BindStatus(KHACHHANG kh)
        {
            SetControlValue(txtSODB.ClientID, kh.MADP + kh.DUONGPHU + kh.MADB);
            SetLabel(lblTENKH.ClientID, kh.TENKH);
            SetLabel(lblIDKH.ClientID, kh.IDKH);
            SetLabel(lblTENDP.ClientID, kh.DUONGPHO != null ? kh.DUONGPHO.TENDP : "");
            SetLabel(lblTENKV.ClientID, kh.KHUVUC != null ? kh.KHUVUC.TENKV : "");

            var kv = ddlTTSD.Items.FindByValue(kh.TTSD);
            if (kv != null)
                ddlTTSD.SelectedIndex = ddlTTSD.Items.IndexOf(kv);

            upnlThongTin.Update();
        }

        protected void gvDanhSach_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "SelectSODB":
                        var khachhang = _khDao.GetKhachHangFromMadb(id);
                        if (khachhang != null)
                        {
                            BindStatus(khachhang);
                            HideDialog("divKhachHang");
                            CloseWaitingDialog();
                            txtSODB.Focus();
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvDanhSach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvDanhSach.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindKhachHang();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #endregion

        private void ClearForm()
        {
            txtIDKH.Text = "";
            lblTENKH.Text = "";
            lblIDKH.Text = "";
            lblTENDP.Text = "";
            lblTENKV.Text = "";
            txtGhiChu.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasPermission(Functions.KH_CupMoNuoc, Permission.Insert))
            {
                CloseWaitingDialog();
                ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                return;
            }

            try
            {
                //var kh = _khDao.GetKhachHangFromMadb(txtSODB.Text.Trim());
                var kh = _khDao.Get(lblIDKH.Text.Trim());
                if(kh == null)
                {
                    CloseWaitingDialog();
                    ShowError("Khách hàng không tồn tại", txtSODB.ClientID);
                    return;
                }

                kh.TTSD = ddlTTSD.SelectedValue;
                var msg = _khDao.UpdateTTSD(kh, DateTime.Now.Month, DateTime.Now.Year, txtGhiChu.Text.Trim(), 
                                                CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    CloseWaitingDialog();
                    ShowInfor(ResourceLabel.Get(msg));
                    ClearForm();
                    BindGrid();
                }
                else
                {
                    CloseWaitingDialog();
                    ShowError("<strong>Lỗi xảy ra</strong>. <br/><br/>Dò lỗi: <br />" +
                        ResourceLabel.Get(msg), txtSODB.ClientID);
                }
            }
            catch (Exception ex)
            {
                CloseWaitingDialog();
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void txtSODB_TextChanged(object sender, EventArgs e)
        {
            var khachhang = _khDao.GetKhachHangFromMadb(txtSODB.Text.Trim());
            if (khachhang != null)
            {
                lblTENKH.Text = khachhang.TENKH;
                lblIDKH.Text = khachhang.IDKH;
                lblTENDP.Text = khachhang.DUONGPHO != null ? khachhang.DUONGPHO.TENDP : "";
                lblTENKV.Text = khachhang.KHUVUC != null ? khachhang.KHUVUC.TENKV : "";

                ddlTTSD.SelectedIndex = (khachhang.TTSD == "CUP") ? 0 : 1;

                CloseWaitingDialog();
                txtSODB.Focus();
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Khách hàng không tồn tại", txtSODB.ClientID);
            }
        }
        
        private void BindGrid()
        {
            int thang, nam;
            try
            {
                DateTime.ParseExact(txtNAM.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
                thang = int.Parse(ddlTHANG.SelectedValue);
                nam = int.Parse(txtNAM.Text.Trim());
            }
            catch
            {
                return;
            }

            var list = _khDao.GetTTSDList(thang, nam, ddlTTSD.SelectedValue);
            gvKhachHang.DataSource = list;
            gvKhachHang.PagerInforText = list.Count.ToString();
            gvKhachHang.DataBind();

            upnlGrid.Update();
        }

        protected void gvKhachHang_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvKhachHang.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvKhachHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "SelectKH":
                        var res = id.Split('-');
                        var khachhang = _khDao.Get(res[0]);
                        try
                        {
                            var ttsd = _khDao.GetTTSD(int.Parse(res[1]));
                            if(ttsd != null)
                            {
                                SetControlValue(txtGhiChu.ClientID, ttsd.GHICHU);
                            }
                        }
                        catch
                        {
                            
                        }

                        if (khachhang != null)
                        {
                            BindStatus(khachhang);
                            HideDialog("divKhachHang");
                            CloseWaitingDialog();
                            txtSODB.Focus();
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvKhachHang_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int thang, nam;
            try
            {
                DateTime.ParseExact(txtNAM.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
                thang = int.Parse(ddlTHANG.SelectedValue);
                nam = int.Parse(txtNAM.Text.Trim());
            }
            catch
            {
                CloseWaitingDialog();
                ShowError("Vui lòng nhập năm hợp lệ.", txtNAM.ClientID);
                return;
            }

            var list = _khDao.GetTTSDList(thang, nam, ddlTTSD.SelectedValue);
            gvKhachHang.DataSource = list;
            gvKhachHang.PagerInforText = list.Count.ToString();
            gvKhachHang.DataBind();

            ClearForm();

            upnlGrid.Update();
            CloseWaitingDialog();
        }
    }
}