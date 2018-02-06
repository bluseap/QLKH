
using System;
using System.Globalization;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.TruyThu
{
    public partial class GhiSaiChiSo : Authentication
    {
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly GhiChiSoDao _gcsDao = new GhiChiSoDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TRUYTHU_GhiSaiChiSo, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_TRUYTHU_GHISAICHISO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_TRUYTHU;
                header.TitlePage = Resources.Message.PAGE_TRUYTHU_GHISAICHISO;
            }

            CommonFunc.SetPropertiesForGrid(gvDanhSach);
            CommonFunc.SetPropertiesForGrid(gvList);
        }

        private void LoadStaticReferences()
        {
            var kvList = _kvDao.GetList();

            ddlKHUVUC.Items.Clear();
            ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var kv in kvList)
            {
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }

            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);

          
        }
      

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
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
        #endregion

        private void BindData()
        {
            if (txtSODB.Text.Trim() == "")
            {
                CloseWaitingDialog();
                ShowError("Chọn khách hàng để cập nhật chỉ số.", txtSODB.ClientID);
                return;
            }

            var kynay = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);
            var kh = _khDao.GetKhachHangFromMadb(txtSODB.Text.Trim());

            if (kh == null)
            {
                CloseWaitingDialog();
                ShowError("Khách hàng không tồn tại. Vui lòng chọn lại danh bộ khách hàng.", txtSODB.ClientID);
                return;
            }

            if (!_gcsDao.IsDaKhoiTao(kynay, kh.IDKH))
            {
                var msg = _gcsDao.KhoiTaoGhiChiSo(kynay, kh);

                if (msg != null && msg.MsgType.Equals(MessageType.Error))
                {
                    CloseWaitingDialog();
                    ShowError("Khởi tạo chỉ số cho khách hàng mới không thành công.", txtSODB.ClientID);
                    return;
                }
            }

            var list = _gcsDao.GetListForUpdate(kynay, txtSODB.Text.Trim(), kh.MADP);

            gvList.DataSource = list;
            gvList.PagerInforText = list.Count.ToString(CultureInfo.InvariantCulture);
            gvList.DataBind();
            divList.Visible = list.Count > 0;

            upnlGrid.Update();
            CloseWaitingDialog();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var hfGCS = e.Row.FindControl("hfGCS") as HiddenField;
            var txtCHISODAU = e.Row.FindControl("txtCHISODAU") as TextBox;
            var txtCHISOCUOI = e.Row.FindControl("txtCHISOCUOI") as TextBox;
            var txtKLTIEUTHU = e.Row.FindControl("txtKLTIEUTHU") as TextBox;

            if (hfGCS == null || txtCHISODAU == null || txtCHISOCUOI == null ||
                txtKLTIEUTHU == null) return;

            var onKeyDownEventHandler = "javascript:onKeyDownEventHandler(\"" + txtCHISODAU.ClientID +
                                                                "\", \"" + txtCHISOCUOI.ClientID +
                                                                "\", \"" + txtKLTIEUTHU.ClientID +
                                                                "\", \"" + hfGCS.ClientID +
                                                                "\"";
            txtCHISODAU.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 1, event);");
            txtCHISOCUOI.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 2, event);");
            txtKLTIEUTHU.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 3, event);");

            txtCHISODAU.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtCHISODAU.ClientID + "\");");
            txtCHISOCUOI.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtCHISOCUOI.ClientID + "\");");
            txtKLTIEUTHU.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtKLTIEUTHU.ClientID + "\");");
        }

        #region Khách hàng
        protected void btnFilterKH_Click(object sender, EventArgs e)
        {
            BindKhachHang();
            upnlKhachHang.Update();
            CloseWaitingDialog();
        }

        protected void btnBrowseKH_Click(object sender, EventArgs e)
        {
            //TODO: do not bind khach hang first, wait for filter
            //BindKhachHang();
            //upnlKhachHang.Update();
            UnblockDialog("divKhachHang");
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
                            SetControlValue(txtSODB.ClientID, id);
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
    }
}
