using System;
using System.Globalization;
using System.Web.UI.WebControls;
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
    public partial class XoaBoKHN : Authentication
    {
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly GhiChiSoDao _gcsDao = new GhiChiSoDao();
        private readonly TieuThuDao _ttDao = new TieuThuDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly KhachHangXoaDao _khxDao = new KhachHangXoaDao();
        private readonly XaPhuongDao _xpDao = new XaPhuongDao();
        private readonly ApToDao _atDao = new ApToDao();

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

        private void ShowError(string message)
        {
            ((EOS)Page.Master).ShowError(message);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_XoaBoKHN, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindKhachHangXoa();                                       
                }

                if (reloadm.Text == "1")
                {
                    gvKhachHang.Visible = false;
                    upnlCustomers.Update();
                    BaoCaoXoaBo();
                } 
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_XOABOKHN;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_XOABOKHN;
            }
            
            CommonFunc.SetPropertiesForGrid(gvKhachHang);
            CommonFunc.SetPropertiesForGrid(gvDanhSach);
        }        

        private void BindKhachHangXoa()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                string makv = _nvDao.Get(b).MAKV;

                var list = _khxDao.GetListKV(makv);

                gvKhachHang.DataSource = list;
                gvKhachHang.PagerInforText = list.Count.ToString();
                gvKhachHang.DataBind();

                upnlCustomers.Update();
            }
            catch { }
        }

        protected void gvKhachHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectHD":
                        var kh = _khxDao.Get(id);

                        lblIDKH.Text = kh.IDKH;
                        txtGhiChu.Text = kh.LYDOXOA;
                        
                        CloseWaitingDialog();
                        break;   
                }
              
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
                BindKhachHangXoa();
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
                
                ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
                txtNAM.Text = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);

                timkv();

                //xa phuong
                var xaphuong = _xpDao.GetListKV(ddlKHUVUC1.SelectedValue);
                ddlXAPHUONG.Items.Clear();
                ddlXAPHUONG.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var xp in xaphuong)
                {
                    ddlXAPHUONG.Items.Add(new ListItem(xp.TENXA, xp.MAXA));              
                } 
                //Ap khóm
                var apkhom = _atDao.GetList(ddlKHUVUC1.SelectedValue, ddlXAPHUONG.SelectedValue);
                ddlAPKHOM.Items.Clear();
                ddlAPKHOM.Items.Add(new ListItem("Tất cả", "%"));
                /*foreach (var ak in apkhom)
                {
                    ddlAPKHOM.Items.Add(new ListItem(ak.TENAPTO, ak.MAAPTO));
                }*/

                divCR.Visible = false;                

            }
            catch { }
        }

        private void timkv()
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
                    ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC1.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }                   
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    ddlKHUVUC1.Items.Clear();
                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC1.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }

        protected void btnBrowseKH_Click(object sender, EventArgs e)
        {
            try
            {
                int thangIndex = 0;
                if (DateTime.Now.Month == 1)
                {
                    thangIndex = 11;
                }
                else
                {
                    thangIndex = DateTime.Now.Month - 2;
                }

                int namIndex = DateTime.Now.Year - 1;
                //lock cap nhap chi so
                //int thang1 = DateTime.Now.Month;
                //string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                int thang1 = int.Parse(ddlTHANG.SelectedValue);
                int nam = int.Parse(txtNAM.Text.Trim());
                var kynay = new DateTime(nam, thang1, 1);

                //var kynay = new DateTime(2013, 6, 1);
                bool dung = _gcsDao.IsLockTinhCuocKy(kynay, ddlKHUVUC1.SelectedValue);
               
                if (txtNAM.Text == Convert.ToString(nam) || txtNAM.Text == Convert.ToString(namIndex))
                {                                           
                    if (dung == true) //kiem tra khoa so ky ghi
                    {
                        CloseWaitingDialog();
                        HideDialog("divKhachHang");
                        ShowInfor("Đã khoá sổ. Không được xóa bộ.");                        
                    }
                    else
                    {
                        UnblockDialog("divKhachHang");
                        //BindKhachHang();
                        CloseWaitingDialog();
                        upnlKhachHang.Update();
                    }                   
                }
                else
                {
                    CloseWaitingDialog();
                    HideDialog("divKhachHang");
                    ShowInfor("Chọn năm xóa bộ cho đúng.");
                }

                divCR.Visible = false;
                upnlCrystalReport.Update();
            }
            catch { }
        }

        private void BindKhachHang()
        {
            try
            {
                var danhsach = _khDao.SearchKhachHang(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(),
                                                               txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
                                                               txtSONHA.Text.Trim(), txtTENDP.Text.Trim(),
                                                               ddlKHUVUC.SelectedValue.Trim());
                gvDanhSach.DataSource = danhsach;
                gvDanhSach.PagerInforText = danhsach.Count.ToString();
                //cpeFilter.Collapsed = true;
                gvDanhSach.DataBind();
                tdDanhSach.Visible = true;

                upnlKhachHang.Update();
            }
            catch { }
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
                        var khachhang = _khDao.Get(id);
                        if (khachhang != null)
                        {
                            BindStatus(khachhang);
                            HideDialog("divKhachHang");
                            CloseWaitingDialog();                            
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

        private void BindStatus(KHACHHANG kh)
        {
            try
            {
                txtSODB.Text = (kh.MADP + kh.DUONGPHU + kh.MADB).ToString();
                lblTENKH.Text = kh.TENKH.ToString();
                lblIDKH.Text = kh.IDKH.ToString();
                lblIDKH.Text = kh.IDKH.ToString();
                lblTENDP.Text = kh.DUONGPHO != null ? kh.DUONGPHO.TENDP.ToString() : "";
                lblTENKV.Text = kh.KHUVUC != null ? kh.KHUVUC.TENKV.ToString() : "";
                lblMAMDSD.Text = kh.MAMDSD.ToString();
                var tieuthu = _ttDao.GetTN(kh.IDKH, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                if (tieuthu != null)
                {
                    lblCSMOI.Text = Convert.ToString(tieuthu.CHISOCUOI);
                    lblCSCU.Text = Convert.ToString(tieuthu.CHISODAU);
                    lblTIEUTHU.Text = Convert.ToString(tieuthu.KLTIEUTHU);
                    lblTHANHTIEN.Text = Convert.ToString(String.Format("{0:#.##}", tieuthu.TIENNUOC));
                    lblTHUEGTGT.Text = Convert.ToString(String.Format("{0:#.##}", tieuthu.TIENTHUE));
                    lblTONGTIEN.Text = Convert.ToString(String.Format("{0:#.####}", tieuthu.TONGTIEN));

                    upnlThongTin.Update();
                    CloseWaitingDialog();                    
                }
                else
                {
                    CloseWaitingDialog();
                    ShowError("Không có tiêu thụ trong tháng này. Xin chọn khách hàng lại", txtSODB.ClientID);
                }
                upnlThongTin.Update();
            }
            catch { }
        }

        protected void btnFilterKH_Click(object sender, EventArgs e)
        {
            BindKhachHang();
            upnlKhachHang.Update();
            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int thangIndex = 0;
                if (DateTime.Now.Month == 1)
                {
                    thangIndex = 11;
                }
                else
                {
                    thangIndex = DateTime.Now.Month - 2;
                }

                int namIndex = DateTime.Now.Year - 1;
                //lock cap nhap chi so
                //int thang1 = DateTime.Now.Month;
                //string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                int thang1 = int.Parse(ddlTHANG.SelectedValue);
                int nam = int.Parse(txtNAM.Text.Trim());
                var kynay = new DateTime(nam, thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);

                bool dung = _gcsDao.IsLockTinhCuocKy(kynay, ddlKHUVUC1.SelectedValue);
                //bool dung = _gcsDao.IsLockTinhCuocKy1(kynay, ddlKHUVUC1.SelectedValue,);
                //kiem tra khoa so
                if (dung == true)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đã khoá sổ. Không được phục hồi xóa bộ khách hàng.");
                    return;
                }

                // Authenticate
                if (!HasPermission(Functions.KH_XoaBoKHN, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                RestoreKH();
                BindKhachHangXoa();
                upnlCustomers.Update();

                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int thangIndex = 0;
                if (DateTime.Now.Month == 1)
                {
                    thangIndex = 11;
                }
                else
                {
                    thangIndex = DateTime.Now.Month - 2;
                }

                int namIndex = DateTime.Now.Year - 1;
                //lock cap nhap chi so
                //int thang1 = DateTime.Now.Month;
                //string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                int thang1 = int.Parse(ddlTHANG.SelectedValue);
                int nam = int.Parse(txtNAM.Text.Trim());
                var kynay = new DateTime(nam, thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var kh = _khDao.Get(lblIDKH.Text.Trim());

                //bool dung = _gcsDao.IsLockTinhCuocKy(kynay, ddlKHUVUC1.SelectedValue);
                bool dung = _gcsDao.IsLockTinhCuocKy1(kynay, ddlKHUVUC1.SelectedValue, kh.MADP);                

                if (dung == false)
                {
                    //var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                    //if (loginInfo == null) return;
                    //string b = loginInfo.Username;

                    //var kh = _khDao.Get(lblIDKH.Text.Trim());

                    _rpClass.InsKhachHangXoa(kh.IDKH, kh.MAKV, txtGhiChu.Text.Trim(), b, ddlXAPHUONG.SelectedValue, 
                            ddlAPKHOM.SelectedValue, kynay);

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = kh.IDKH,
                        IPAddress = "192.168.1.11",
                        MANV = b,
                        UserAgent = "192.168.1.11",
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "XBN",
                        MOTA = "Xóa bộ khách hàng nước."
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    BindKhachHangXoa();
                    upnlCustomers.Update();
                    CloseWaitingDialog();

                    ShowInfor("Xóa bộ thành công. Xin kiểm tra lại.");
                }
                else
                {
                    CloseWaitingDialog();                   
                    ShowInfor("Đã khoá sổ. Không được xóa bộ khách hàng.");
                }
                
            }
            catch { }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int thang1 = int.Parse(ddlTHANG.SelectedValue);
                int nam = int.Parse(txtNAM.Text.Trim());
                var kynay = new DateTime(nam, thang1, 1);

                //var list = _khxDao.GetListKV(ddlKHUVUC.SelectedValue);
                var list = _khxDao.GetListKVKy(ddlKHUVUC.SelectedValue, kynay);

                gvKhachHang.DataSource = list;
                gvKhachHang.PagerInforText = list.Count.ToString();
                gvKhachHang.DataBind();
                gvKhachHang.Visible = true;

                divCR.Visible = false;
                upnlCrystalReport.Update();

                CloseWaitingDialog();
                upnlCustomers.Update();
            }
            catch { }
        }

        private void RestoreKH()
        {
            try
            {
                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<KHACHHANGXOA>();
                    var listIds = strIds.Split(',');                    

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _khxDao.Get(ma)));

                    if (objs.Count > 0)
                    {
                        var msg = _khxDao.DeleteList(objs);
                        if (msg != null)
                        {
                            switch (msg.MsgType)
                            {
                                case MessageType.Error:
                                    ShowError(ResourceLabel.Get(msg));
                                    break;

                                case MessageType.Info:
                                    ShowInfor(ResourceLabel.Get(msg));
                                    break;

                                case MessageType.Warning:
                                    ShowWarning(ResourceLabel.Get(msg));
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                BaoCaoXoaBo();
                //Clear();
                CloseWaitingDialog();
                gvKhachHang.Visible = false;
                upnlCustomers.Update();
            }
            catch { }
        }

        private void BaoCaoXoaBo()
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

                //DataTable dt = new ReportClass().DskhMoi(int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), txtMaDp.Text.Trim(), txtDuongPhu.Text.Trim(), cboMucDichSuDung.Text.Trim(),
                 //                      cboTrangThai.Text.Trim(), cboKhuVuc.Text.Trim()).Tables[0];
                DataTable dt = new ReportClass().DSKHXOABO(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                    ddlKHUVUC1.SelectedValue, 1).Tables[0];
                
                rp = new ReportDocument();

                var path = Server.MapPath("../../Reports/QuanLyKhachHang/DSKhachHangXoa.rpt");
              //var path = Server.MapPath("../../Reports/QuanLyKhachHang/DSHoNgheo.rpt");

                rp.Load(path);

                rp.SetDataSource(dt);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();
                
                TextObject txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
                txtXN.Text = "XN ĐIỆN NƯỚC " + ddlKHUVUC1.SelectedItem.ToString().ToUpper();
                TextObject txtTIEUDE = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
                txtTIEUDE.Text = "DANH SÁCH KHÁCH HÀNG XÓA BỘ";
                //txtTuNgay
                TextObject txtTuNgay = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
                txtTuNgay.Text = "Kỳ: " + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim();
                var d = DateTime.Now.Day;
                var m = DateTime.Now.Month;
                var y = DateTime.Now.Year;
                TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
                ngay.Text = ddlKHUVUC1.SelectedItem + ", ngày " + d + " tháng " + m + " năm " + y;
                

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

        protected void ddlXAPHUONG_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Ap khóm
                var apkhom = _atDao.GetList(ddlKHUVUC1.SelectedValue, ddlXAPHUONG.SelectedValue);
                ddlAPKHOM.Items.Clear();
                //ddlAPKHOM.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var ak in apkhom)
                {
                    ddlAPKHOM.Items.Add(new ListItem(ak.TENAPTO, ak.MAAPTO));
                }
            }
            catch { }
        }


    }
}