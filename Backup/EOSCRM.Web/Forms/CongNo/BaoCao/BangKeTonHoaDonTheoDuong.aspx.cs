﻿using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Web.Common;
using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.CongNo.BaoCao
{
    public partial class BangKeTonHoaDonTheoDuong : Authentication
    {
        private readonly KhuVucDao _kvDao = new KhuVucDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.CN_BaoCao_BangKeTonHoaDonTheoDuong, Permission.Read);
                PrepareUI();
                if (!Page.IsPostBack)
                {
                    LoadReferences();
                }
                else 
                {
                    var dt = (DataTable)Session[SessionKey.CN_BAOCAO_BANGKETONHOADONTHEODUONG];
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
            Page.Title = Resources.Message.TITLE_CN_BAOCAO_BẠNGKETONHOADONTHEODUONG;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_CONGNO;
                header.TitlePage = Resources.Message.PAGE_CN_BAOCAO_BẠNGKETONHOADONTHEODUONG;
            }
        }

        private void LoadReferences()
        {
            txtNAM.Text = DateTime.Now.Year.ToString();
            cboTHANG.SelectedIndex = DateTime.Now.Month - 1;

            var kvList = _kvDao.GetList();

            cboKhuVuc.Items.Clear();
            cboKhuVuc.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var kv in kvList)
                cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));

            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
        }

        #region Startup script registeration
        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var ds =
                new ReportClass().BangKeTonHoaDonTheoDuong(cboTHANG.SelectedIndex + 1, int.Parse(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue);

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
            var path = Server.MapPath("../../../Reports/CongNo/KHTonHoaDonTheoDuong.rpt");
            rp.Load(path);

            /*
            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text.Trim();
            */

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.CN_BAOCAO_BANGKETONHOADONTHEODUONG] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}