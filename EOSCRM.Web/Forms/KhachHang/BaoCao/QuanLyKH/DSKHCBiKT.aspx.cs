using System;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using CrystalDecisions.CrystalReports.Engine;


namespace EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH
{
    public partial class DSKHCBiKT : Authentication
    {
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly XaPhuongDao _xpDao = new XaPhuongDao();
        private readonly PhongBanDao _pbDao = new PhongBanDao();
        private readonly PhuongDao _pDao = new PhuongDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

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

        private void UnblockWaitingDialog()
        {
            ((EOS)Page.Master).UnblockWaitingDialog();
        }

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindPhuong();
                }
                else
                {
                    BaoCao();
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
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var kvnv = _nvDao.Get(b);

                ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
                txtNAM.Text = DateTime.Now.Year.ToString();

                var xaList = _xpDao.GetListKV(kvnv.MAKV);
                ddlXAPHUONG.Items.Clear();
                ddlXAPHUONG.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var xa in xaList)
                {
                    ddlXAPHUONG.Items.Add(new ListItem(xa.MAXA + ": " + xa.TENXA, xa.MAXA));
                } 

                var NhaMayList = _pbDao.GetListKV(kvnv.MAKV);
                ddlNHAMAYTO.Items.Clear();
                ddlNHAMAYTO.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in NhaMayList)
                {
                    ddlNHAMAYTO.Items.Add(new ListItem(kv.MAPB + " " + kv.TENPB, kv.MAPB));
                }               

                var kvList = _pDao.GetListKV(kvnv.MAKV);
                ddlMAPHUONG.Items.Clear();
                ddlMAPHUONG.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in kvList)
                {
                    ddlMAPHUONG.Items.Add(new ListItem(kv.MAPHUONG + " " + kv.TENPHUONG, kv.MAPHUONG));
                }

                timkv();

                var dotin = _diDao.GetListKVNN(_nvDao.Get(b).MAKV);
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
                    var kvList = _kvDao.GetList();
                    ddlKHUVUC1.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC1.Items.Add(new System.Web.UI.WebControls.ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    ddlKHUVUC1.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC1.Items.Add(new System.Web.UI.WebControls.ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }

        private void PrepareUI()
        {           
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_DSKHCBiKH;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAO_DSKHCBiKH;
            }

            CommonFunc.SetPropertiesForGrid(gvPhuong);
            //CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }        

        protected void gvPhuong_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;               

                switch (e.CommandName)
                {
                    case "SelectMAPHUONG":
                        var maphuong = _pDao.GetMAKV(id.ToString(), _nvDao.Get(b).MAKV).MAPHUONG;

                        txtMAPHUONG.Text = maphuong.ToString();

                        upKHCHUANBIKT.Update();
                        HideDialog("divPhuong");
                        CloseWaitingDialog();

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvPhuong_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvPhuong.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindPhuong();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindPhuong()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var list = _pDao.GetListKVTS(_nvDao.Get(b).MAKV, txtKeywordDP.Text.Trim());

                gvPhuong.DataSource = list;
                gvPhuong.PagerInforText = list.Count.ToString();
                gvPhuong.DataBind();

                upnlPhuong.Update();
                CloseWaitingDialog();
            }
            catch { }
        }

        protected void btnBrowseSOHD_Click(object sender, EventArgs e)
        {
            BindPhuong();
            upnlPhuong.Update();
            UnblockDialog("divPhuong");
        }        

        protected void btnFilterPhuong_Click(object sender, EventArgs e)
        {
            BindPhuong();
        }

        protected void btnDSKHCBiKT_Click(object sender, EventArgs e)
        {
            BaoCao();
        }

        private void BaoCao()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.Get(b);

                var TuNgay = DateTimeUtil.GetVietNamDate("01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim());     //thang
                var DenNgay = DateTimeUtil.GetVietNamDate("01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim());    //nam

                //var ds = new ReportClass().dsKHCBiKT(query.MAKV.ToString(), ddlNHAMAYTO.SelectedValue.ToString(), "dsCBIKT", TuNgay, DenNgay);
                var ds = new ReportClass().DSQuiTrinhNuocBien(TuNgay, DenNgay, query.MAKV.ToString(), ddlNHAMAYTO.SelectedValue.ToString(),
                            ddlDOTGCS.SelectedValue, "", "dsCBIKTDOTIN");
                
                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReportNgayN(ds.Tables[0]);

                CloseWaitingDialog();
            }
            catch { }
        }

        private void ReportNgayN(DataTable dt)
        {
            if (dt == null)
                return;

            #region FreeMemory
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

            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSCBIKT.rpt");
            //var path = Server.MapPath("~/Reports/QuanLyKhachHang/DSCBIKT.rpt");
            rp.Load(path);

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            string tenkv = _kvDao.Get(_nvDao.Get(b).MAKV).TENKV;

            //txtXN
            var txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            if (txtXN != null)
                txtXN.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + tenkv.ToUpper();
            //txtTIEUDE
            var txtTIEUDE = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            if (txtTIEUDE != null)
                txtTIEUDE.Text = "DANH SÁCH KHÁCH HÀNG CHUẨN BỊ KHAI THÁC";
            //txtTENPHUONG
            var txtTENPHUONG = rp.ReportDefinition.ReportObjects["txtTENPHUONG"] as TextObject;
            string kyF = ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim();
            if (txtTENPHUONG != null)
                //txtTENPHUONG.Text = "Danh bộ: " + _nvDao.Get(b).MAKV + (ddlMAPHUONG.SelectedValue != "%" ? ddlMAPHUONG.SelectedValue : "");
                txtTENPHUONG.Text = "Kỳ: " + kyF + ". " + ddlDOTGCS.SelectedItem.ToString();

            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = "An Giang, ngày " + d + " tháng " + m + " nãm " + y;
          
            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            //divReport.Visible = true;
            upnlCrystalReport.Update();

            Session[SessionKey.TK_BAOCAO_DONDANGKYNGAYN] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }


    }
}