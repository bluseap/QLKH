using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.GhiChiSo.BaoCao
{
    public partial class DSKDO : Authentication
    {
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.GCS_BaoCao_DSKTKY, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    SearchDuong();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_GCS_BAOCAO_DANHSACHKIEMTRA;

            var header = (Header)Master.FindControl("header");
            if (header == null) return;
            header.ModuleName = Resources.Message.MODULE_GHICHISO;
            header.TitlePage = Resources.Message.PAGE_GCS_BAOCAO_DANHSACHKIEMTRA;

            CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }

        #region Startup script registeration
        private void SetLabel(string id, string value)
        {
            ((EOS)Page.Master).SetLabel(id, value);
        }

        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
        }

        private void ShowError(string message, string controlId)
        {
            ((EOS)Page.Master).ShowError(message, controlId);
        }

        private void ShowError(string message)
        {
            ((EOS)Page.Master).ShowError(message);
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
            // load khu vuc
            var listKhuVuc = new KhuVucDao().GetList();

            ddlKHUVUC.Items.Clear();
            ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));

            foreach (var kv in listKhuVuc)
            {
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }
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

                if (a.MAKV == "99")
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

        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            CloseWaitingDialog();
        }

        private void BindDuongPho()
        {
            var list = dpDao.GetList("%", txtKeywordDP.Text.Trim());
            gvDuongPho.DataSource = list;
            gvDuongPho.PagerInforText = list.Count.ToString();
            gvDuongPho.DataBind();
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

        private void SearchDuong()
        {
            var list = string.IsNullOrEmpty(txtMADP.Text.Trim()) ?
                dpDao.GetList(ddlKHUVUC.SelectedValue) :
                new List<DUONGPHO> { dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim()) };

            dpDataList.DataSource = list;
            dpDataList.DataBind();

            upnlGrid.Update();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // load lo trinh tinh cuoc by khu vuc and duong pho
            try
            {
                DateTime.ParseExact(txtNAM.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
            }
            catch
            {
                CloseWaitingDialog();
                ShowError("Vui lòng chọn năm hợp lệ", txtNAM.ClientID);
                return;
            }

            SearchDuong();
            CloseWaitingDialog();
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var str_madp = "";

            for (var i = 0; i < dpDataList.Items.Count; i++)
            {
                var cb = dpDataList.Items[i].FindControl("chkDuongPho") as HtmlInputCheckBox;
                if (cb == null || !cb.Checked) continue;

                var madp = cb.Attributes["title"].Trim();
                var duongphu = cb.Attributes["lang"].Trim();

                if (duongphu.Length > 0)
                {
                    str_madp += " (DP.MADP= '" + madp + "' and DP.DUONGPHU = '" + duongphu + "') OR";
                }
                else
                {
                    str_madp += " (DP.MADP= '" + madp + "') OR";
                }
            }

            str_madp = "(" + str_madp + ") and";
            str_madp = (str_madp == "() and") ?
                "" :
                str_madp.Replace("OR) and", ")  ");
           

            var dtDSKTKY =
                new ReportClass().KiemDo3T(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC.SelectedValue)
                    .Tables[0];
            Session["DSKTKY"] = dtDSKTKY;
            Session["DSKTKY_THANGNAM"] = string.Format("{0}/{1}",
                                                    ddlTHANG.SelectedValue, txtNAM.Text.Trim());
            CloseWaitingDialog();
            Page.Response.Redirect(ResolveUrl("~") + "Forms/GhiChiSo/BaoCao/rpDSKDO.aspx");
            
        }

        protected void gvDuongPho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
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

                            UpdateKhuVuc(dp);
                            upnlTinhCuoc.Update();

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

        protected void btnBrowseDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            upnlDuongPho.Update();
            UnblockDialog("divDuongPho");
        }

        protected void gvDuongPho_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvDuongPho.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindDuongPho();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void UpdateKhuVuc(DUONGPHO dp)
        {
            SetLabel(lblTENDUONG.ClientID, dp.TENDP);

            var kv = ddlKHUVUC.Items.FindByValue(dp.MAKV);
            if (kv != null)
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);
        }

    }
}
