using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Domain;
using EOSCRM.Web.Common;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.CongNo.BaoCao
{
    public partial class BangChiTietChuanThu : Authentication
    {
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadReferences();
                }
                else 
                {
                    var dt = (DataTable)Session[SessionKey.CN_BAOCAO_BANGKECHITIETCHUANTHU];
                    Report(dt);
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_CN_BAOCAO_BANGCHITIETCHUANTHU;

            var header = (Header)Master.FindControl("header");
            if (header == null) return;

            header.ModuleName = Resources.Message.MODULE_GHICHISO;
            header.TitlePage = Resources.Message.PAGE_CN_BAOCAO_BANGCHITIETCHUANTHU;

            CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
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

        private void LoadReferences()
        {
            var kvList = _kvDao.GetList();

            cboKhuVuc.Items.Clear();
            cboKhuVuc.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var kv in kvList)
            {
                cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }

            txtNguoiLap.Text = LoginInfo.NHANVIEN.HOTEN;

            var dt = DateTime.Now;
            txtNAM.Text = dt.Year.ToString();
            cboTHANG.SelectedIndex = dt.Month - 1;
            var dim = DateTime.DaysInMonth(dt.Year, dt.Month);
            txtTuNgay.Text = new DateTime(dt.Year, dt.Month, 1).ToString("dd/MM/yyyy");
            txtDenNgay.Text = new DateTime(dt.Year, dt.Month, dim).ToString("dd/MM/yyyy");
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var ds =
                new ReportClass().BangChiTietChuanThu(int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()),
                                              txtMaDp.Text.Trim(), txtDuongPhu.Text.Trim(), cboKhuVuc.Text.Trim(),
                                              DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim()), DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim())
                                              );
            if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }

            Report(ds.Tables[0]);
            CloseWaitingDialog();
        }

        private void Report(DataTable dt)
        {
            if (dt == null)
                return;

            var rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/CongNo/BangChiTietChuanThu.rpt");
            rp.Load(path);

            var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            if (txtKy != null)
                txtKy.Text = cboTHANG.Text + "/" + txtNAM.Text.Trim();

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            var txtTu = rp.ReportDefinition.ReportObjects["txtTu"] as TextObject;
            if (txtTu != null)
                txtTu.Text = string.Format("Từ {0} đến {1}", txtTuNgay.Text.Trim(), txtDenNgay.Text.Trim());

            var txtNgay = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgay != null)
                txtNgay.Text = string.Format("Đồng Tháp, ngày {0}", DateTime.Now.ToString("dd/MM/yyyy"));

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divCR.Visible = true;
            upnlReport.Update();

            Session[SessionKey.CN_BAOCAO_BANGKECHITIETCHUANTHU] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        #region Đường phố
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

        private void UpdateKhuVuc(DUONGPHO dp)
        {
            var kv = cboKhuVuc.Items.FindByValue(dp.MAKV);
            if (kv != null)
                cboKhuVuc.SelectedIndex = cboKhuVuc.Items.IndexOf(kv);
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
                            SetControlValue(txtMaDp.ClientID, dp.MADP);
                            SetControlValue(txtDuongPhu.ClientID, dp.DUONGPHU);

                            UpdateKhuVuc(dp);
                            upnlGhiChiSo.Update();

                            HideDialog("divDuongPho");
                            CloseWaitingDialog();

                            txtMaDp.Focus();
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

        protected void gvDuongPho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }
        #endregion
    }
}