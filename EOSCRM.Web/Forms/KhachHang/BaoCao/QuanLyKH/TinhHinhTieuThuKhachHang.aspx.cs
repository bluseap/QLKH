using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH
{
    public partial class TinhHinhTieuThuKhachHang : Authentication
    {
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly KhachHangDao khDao = new KhachHangDao();

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

        private void ShowWarning(string message)
        {
            ((EOS)Page.Master).ShowWarning(message);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_BaoCao_QLKH_TinhHinhTieuThuKhachHang, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {                    
                    LoadReferences();                    
                }

                if (reloadm.Text == "1")
                {
                    LayBaoCao();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_QLKH_TINHHINHTIEUTHU;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAO_QLKH_TINHHINHTIEUTHU;
            }
            CommonFunc.SetPropertiesForGrid(gvDanhSach);
        }

        private void LoadReferences()
        {
            timkv();
            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
        }        

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            LayBaoCao();
        }

        private void LayBaoCao()
        {
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

            //var khachhang = khDao.GetKhachHangFromMadb(txtSODB.Text.Trim());
            var khachhang = khDao.Get(lbIDKH.Text.Trim());

            if (khachhang == null)
            {
                CloseWaitingDialog();
                ShowError("Không tìm thấy dữ liệu để làm báo cáo");
                return;
            }

            var dtDsinhoadon = new ReportClass().TinhHinhTieuThu(lbIDKH.Text.Trim()).Tables[0];

            if (dtDsinhoadon == null || dtDsinhoadon.Rows.Count == 0)
            {
                CloseWaitingDialog();
                ShowError("Không tìm thấy dữ liệu để làm báo cáo");
                return;
            }

            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/QuanLyKhachHang/TTTTKH.rpt");
            rp.Load(path);

            rp.SetDataSource(dtDsinhoadon);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null) 
                txtNguoiLap1.Text = txtNguoiLap.Text;

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            string kvnv = _nvDao.Get(b).MAKV;
            var txtMAKV_r = rp.ReportDefinition.ReportObjects["txtMAKV_r"] as TextObject;
            if (txtMAKV_r != null)
                txtMAKV_r.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + _kvDao.Get(kvnv).TENKV.ToUpper();

            //rp.SetDataSource(dtDsinhoadon);
            //rpViewer.ReportSource = rp;
            //rpViewer.DataBind();
            //Session["DSBAOCAO"] = dtDsinhoadon;

            divCR.Visible = true;

            reloadm.Text = "1";
            Session["DS_DonDangKy"] = dtDsinhoadon;
            Session[Constants.REPORT_FREE_MEM] = rp;

            CloseWaitingDialog();
            upnlCrystalReport.Update();

            //Session[Constants.REPORT_FREE_MEM] = rp;
            //CloseWaitingDialog();

            upnlInfor.Update();
        }

        private void ReLoadBaoCao()
        {
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

            //var khachhang = khDao.GetKhachHangFromMadb(txtSODB.Text.Trim());
            var khachhang = khDao.Get(lbIDKH.Text.Trim());

            //if (khachhang == null)
            //{
            //    CloseWaitingDialog();
            //    ShowError("Không tìm thấy dữ liệu để làm báo cáo");
            //    return;
            //}

            var dtDsinhoadon = new ReportClass().TinhHinhTieuThu(lbIDKH.Text.Trim()).Tables[0];

            //if (dtDsinhoadon == null || dtDsinhoadon.Rows.Count == 0)
            //{
            //    CloseWaitingDialog();
            //    ShowError("Không tìm thấy dữ liệu để làm báo cáo");
            //    return;
            //}

            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/QuanLyKhachHang/TTTTKH.rpt");
            rp.Load(path);

            rp.SetDataSource(dtDsinhoadon);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            string kvnv = _nvDao.Get(b).MAKV;
            var txtMAKV_r = rp.ReportDefinition.ReportObjects["txtMAKV_r"] as TextObject;
            if (txtMAKV_r != null)
                txtMAKV_r.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + _kvDao.Get(kvnv).TENKV.ToUpper();

            //rp.SetDataSource(dtDsinhoadon);
            //rpViewer.ReportSource = rp;
            //rpViewer.DataBind();
            //Session["DSBAOCAO"] = dtDsinhoadon;

            divCR.Visible = true;

            //reloadm.Text = "1";
            Session["DS_DonDangKy"] = dtDsinhoadon;
            Session[Constants.REPORT_FREE_MEM] = rp;

            CloseWaitingDialog();
            upnlCrystalReport.Update();

            //Session[Constants.REPORT_FREE_MEM] = rp;
            //CloseWaitingDialog();

            upnlInfor.Update();
        }

        protected void btnFilterKH_Click(object sender, EventArgs e)
        {
            BindKhachHang();
            upnlKhachHang.Update();
            CloseWaitingDialog();
        }

        private void BindKhachHang()
        {
            var danhsach = khDao.SearchKhachHang(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(),
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
                        //var khachhang = _khDao.GetKhachHangFromMadb(id);
                        var khachhang = khDao.Get(id);                        

                        if (khachhang != null)
                        {
                            HideDialog("divKhachHang");
                            CloseWaitingDialog();

                            lbIDKH.Text = khachhang.IDKH;
                            txtSODB.Text = khachhang.MADP + khachhang.MADB;
                            
                            upnlInfor.Update();
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
                gvDanhSach.PageIndex = e.NewPageIndex;                
                BindKhachHang();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnKHACHHANG_Click(object sender, EventArgs e)
        {
            UnblockDialog("divKhachHang");
            upnlKhachHang.Update();
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

    }
}