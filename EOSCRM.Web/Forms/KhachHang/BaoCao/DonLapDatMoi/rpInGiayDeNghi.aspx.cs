using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.KhachHang.BaoCao.DonLapDatMoi
{
    public partial class rpInGiayDeNghi : Authentication
    {
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly ReportClass _rpDao = new ReportClass();
        private readonly PhuongDao _pDao = new PhuongDao();
        private readonly DateTimeUtil _dateDao = new DateTimeUtil();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    //TODO: Load references
                    LoadReferences();
                }
                else
                {      
                    if (reloadm.Text == "1")
                    {
                        ReLoadBaoCao();
                    }
                    else if (reloadm.Text == "2")
                    {
                        ReLoadBaoCao_mau();
                    }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void LoadReferences()
        {
            UpdateMode = Mode.Create;
            txtTENNV2.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
            txtNGAYIN.Text = DateTime.Now.ToString("dd/MM/yyyy");
            timkv();            
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

                if (a.MAKV == "99" && b == "nguyen")
                {
                    var kvList = _kvDao.GetList();
                    ddlMaKV.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else if (a.MAKV == "99")
                {
                    var kvList = _kvDao.GetList();
                    ddlMaKV.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    ddlMaKV.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }

        #region Startup script registeration
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

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }

        private void SetReadonly(string id, bool isReadonly)
        {
            ((EOS)Page.Master).SetReadonly(id, isReadonly);
        }

        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
        }

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
        }

        private void HideDialog(string divId)
        {
            ((EOS)Page.Master).HideDialog(divId);
        }

        #endregion

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_DLM_INGIAYDENGHI;

            var header = (Header)Master.FindControl("header");
            if (header == null) return;

            header.ModuleName = Resources.Message.MODULE_KHACHHANG;
            header.TitlePage = Resources.Message.TITLE_TC_INPHIEULAPNUOC;

            CommonFunc.SetPropertiesForGrid(gvDDK);
            //CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }

        private void ReLoadBaoCao()
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
                catch
                {

                }
            }

            //DataTable dt = new ReportClass().IN_GIAYDENGHI(txtMADDK.Text.Trim()).Tables[0];
            //DataTable dt = _rpDao.IN_GIAYDENGHI(txtMADDK.Text.Trim()).Tables[0];
            DataTable dt = new ReportClass().INGIAYDENGHIN(txtMADDK.Text.Trim()).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/DonLapDatMoi/IN_GIAYDENGHI.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            //divCR.Visible = true;
            //upnlCrystalReport.Update();

            //reloadm.Text = "6";
            //String maphuong = ddkDao.Get(txtMADDK.Text.Trim()).MAPHUONG;
            //String tenphuong = _pDao.Get(maphuong).TENPHUONG.ToString();
            String makv = ddkDao.Get(txtMADDK.Text.Trim()).MAKV;
            String tenkv = _kvDao.Get(makv).TENKV.ToString();
            var ngaydk2 = ddkDao.Get(txtMADDK.Text.Trim());

            //DateTime ngay8 = DateTimeUtil.GetVietNamDate(Convert.ToString(ngaydk2.NGAYDK));
            string ngay3 = ngaydk2.NGAYDK.Value.ToString("dd/MM/yyyy");
            

            String ngay = ngay3.Substring(0, 2);
            String thang = ngay3.Substring(3 ,2);
            String nam = ngay3.Substring(6, 4);
            //DateTime ngaydk = DateTimeUtil.GetVietNamDate(ddkDao.Get(txtMADDK.Text.Trim()).NGAYDK);
            TextObject txtKINHGUI = rp.ReportDefinition.ReportObjects["txtKINHGUI"] as TextObject;
            txtKINHGUI.Text = "Kính gửi: Xí nghiệp Điện Nước huyện, thị: " + tenkv;
            TextObject txtDENGHI = rp.ReportDefinition.ReportObjects["txtDENGHI"] as TextObject;
            txtDENGHI.Text = "Đề nghị Xí nghiệp Điện Nước huyện, thị " + tenkv + " lắp đặt đồng hồ nước cho tôi để sử dụng vào mục đích: (đánh dấu x vào mục đích cần sử dụng).";
            TextObject txtNGAYDK = rp.ReportDefinition.ReportObjects["txtNGAYDK"] as TextObject;
            txtNGAYDK.Text = "An Giang, ngày " + ngay + " tháng " + thang + " năm " + nam;

            //txtNOILAPDHHN;txtNOILAPDHHN
            if (ddkDao.Get(txtMADDK.Text.Trim()).NOILAPDHHN != null)
            {
                TextObject txtNOILAPDHHN = rp.ReportDefinition.ReportObjects["txtNOILAPDHHN"] as TextObject;
                txtNOILAPDHHN.Text = ddkDao.Get(txtMADDK.Text.Trim()).NOILAPDHHN;

                TextObject txtNOILAPDHNM = rp.ReportDefinition.ReportObjects["txtNOILAPDHNM"] as TextObject;
                txtNOILAPDHNM.Text = ddkDao.Get(txtMADDK.Text.Trim()).NOILAPDHHN;
            }
            else
            {
                TextObject txtNOILAPDHHN = rp.ReportDefinition.ReportObjects["txtNOILAPDHHN"] as TextObject;
                txtNOILAPDHHN.Text = "";

                TextObject txtNOILAPDHNM = rp.ReportDefinition.ReportObjects["txtNOILAPDHNM"] as TextObject;
                txtNOILAPDHNM.Text = "";
            }

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                //txtNGAYIN.Text = txtNGAYIN.Text.Trim().ToString();
                txtNGAYIN.Text = DateTime.Now.ToString("dd/MM/yyyy"); 

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtTENNV2.Text.Trim().ToString();
            var txtNHANVIENIN2 = rp.ReportDefinition.ReportObjects["txtNHANVIENIN2"] as TextObject;
            if (txtNHANVIENIN2 != null)
                txtNHANVIENIN2.Text = txtTENNV2.Text.Trim().ToString();

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
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
                catch {  }
            }

            if (string.IsNullOrEmpty(txtMADDK.Text.Trim()))
            {
                CloseWaitingDialog();
                ShowError("Chọn đơn đăng ký khách hàng cho đúng.");
                return;
            }

            DataTable dt = new ReportClass().INGIAYDENGHIN(txtMADDK.Text.Trim()).Tables[0];
            //DataTable dt = new ReportClass().INHOPDONG(txtMADDK.Text.Trim()).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/DonLapDatMoi/IN_GIAYDENGHI.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            //divCR.Visible = true;
            //upnlCrystalReport.Update();

            //reloadm.Text = "6";
            //String maphuong = ddkDao.Get(txtMADDK.Text.Trim()).MAPHUONG;
            //String tenphuong = _pDao.Get(maphuong).TENPHUONG.ToString();
            String makv = ddkDao.Get(txtMADDK.Text.Trim()).MAKV;
            String tenkv = _kvDao.Get(makv).TENKV.ToString();
            var ngaydk2 =  ddkDao.Get(txtMADDK.Text.Trim());

            //DateTime ngay8 = DateTimeUtil.GetVietNamDate(Convert.ToString(ngaydk2.NGAYDK));
            //String ngay3 = Convert.ToString(ngaydk2.NGAYDK);

            string ngay3 = ngaydk2.NGAYDK.Value.ToString("dd/MM/yyyy");
            
            String ngay = ngay3.Substring(0, 2);
            String thang = ngay3.Substring(3, 2);
            String nam = ngay3.Substring(6, 4);
            //DateTime ngaydk = DateTimeUtil.GetVietNamDate(ddkDao.Get(txtMADDK.Text.Trim()).NGAYDK);
            TextObject txtKINHGUI = rp.ReportDefinition.ReportObjects["txtKINHGUI"] as TextObject;
            txtKINHGUI.Text = "Kính gửi: Xí nghiệp Điện Nước huyện, thị: " + tenkv;
            TextObject txtDENGHI = rp.ReportDefinition.ReportObjects["txtDENGHI"] as TextObject;
            txtDENGHI.Text = "Đề nghị Xí nghiệp Điện Nước huyện, thị " + tenkv + " lắp đặt đồng hồ nước cho tôi để sử dụng vào mục đích: (đánh dấu x vào mục đích cần sử dụng).";
            TextObject txtNGAYDK = rp.ReportDefinition.ReportObjects["txtNGAYDK"] as TextObject;
            txtNGAYDK.Text = "An Giang, ngày " + ngay + " tháng " + thang + " năm " + nam;

            //txtNOILAPDHHN;txtNOILAPDHHN
            if (ddkDao.Get(txtMADDK.Text.Trim()).NOILAPDHHN != null)
            {
                TextObject txtNOILAPDHHN = rp.ReportDefinition.ReportObjects["txtNOILAPDHHN"] as TextObject;
                txtNOILAPDHHN.Text = ddkDao.Get(txtMADDK.Text.Trim()).NOILAPDHHN;

                TextObject txtNOILAPDHNM = rp.ReportDefinition.ReportObjects["txtNOILAPDHNM"] as TextObject;
                txtNOILAPDHNM.Text = ddkDao.Get(txtMADDK.Text.Trim()).NOILAPDHHN;
            }
            else
            {
                TextObject txtNOILAPDHHN = rp.ReportDefinition.ReportObjects["txtNOILAPDHHN"] as TextObject;
                txtNOILAPDHHN.Text = "";

                TextObject txtNOILAPDHNM = rp.ReportDefinition.ReportObjects["txtNOILAPDHNM"] as TextObject;
                txtNOILAPDHNM.Text = "";
            }

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)                
                //txtNGAYIN.Text = txtNGAYIN.Text.Trim().ToString();
                txtNGAYIN.Text = DateTime.Now.ToString("dd/MM/yyyy"); 

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtTENNV2.Text.Trim().ToString();
            var txtNHANVIENIN2 = rp.ReportDefinition.ReportObjects["txtNHANVIENIN2"] as TextObject;
            if (txtNHANVIENIN2 != null)
                txtNHANVIENIN2.Text = txtTENNV2.Text.Trim().ToString();

            reloadm.Text = "1";

            upnlCrystalReport.Update();

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {

        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvList.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindDataForGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var madon = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        if (!string.Empty.Equals(madon))
                        {
                            var don = ddkDao.Get(madon);
                            if (don == null) return;

                            var ds = _rpDao.INHOPDONG(madon.ToString());

                            if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                            //Report(ds.Tables[0]);
                            txtMADDK.Text = madon;

                            /*
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
                                // ReSh+arper disable EmptyGeneralCatchClause
                                catch
                                // ReSharper restore EmptyGeneralCatchClause
                                {

                                }
                            }
                            #endregion FreeMemory

                            var dt = _rpDao.INHOPDONG(madon);                                
                            rp = new ReportDocument();
                            var path = Server.MapPath("../../../Reports/DonLapDatMoi/INHOPDONG.rpt");
                            rp.Load(path);
                            rp.SetDataSource(dt);
                            rpViewer.ReportSource = rp;
                            rpViewer.DataBind();
                            divCR.Visible = true;
                            upnlCrystalReport.Update();
                            Session["DSBAOCAO"] = dt;
                            Session[Constants.REPORT_FREE_MEM] = rp;
                            */

                            upnlInfor.Update();
                            HideDialog("divHopDong");


                            CloseWaitingDialog();
                            //SetDDKToForm(don);
                        }

                        CloseWaitingDialog();

                        break;

                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindDataForGrid()
        {
            try
            {

                // ReSharper restore EmptyGeneralCatchClause

                //hien theo phong ban, khu vuc
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.Get(b);//nhan vien khu vuc ??


                var objList = ddkDao.GetListHD(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), query.MAKV.ToString());

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();

                //}
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void linkGDN_Click(object sender, EventArgs e)
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
                catch      {     }
            }

            if (string.IsNullOrEmpty(txtMADDK.Text.Trim()))
            {
                CloseWaitingDialog();
                ShowError("Chọn đơn đăng ký khách hàng cho đúng.");
                return;
            }

            DataTable dt = new ReportClass().INGIAYDENGHIN(txtMADDK.Text.Trim()).Tables[0];
            //DataTable dt = new ReportClass().INHOPDONG(txtMADDK.Text.Trim()).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/DonLapDatMoi/IN_GIAYDENGHIMAU.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            //divCR.Visible = true;
            //upnlCrystalReport.Update();

            /*
            //reloadm.Text = "6";
            String maphuong = ddkDao.Get(txtMADDK.Text.Trim()).MAPHUONG;
            String tenphuong = _pDao.Get(maphuong).TENPHUONG.ToString();
            var ngaydk2 = ddkDao.Get(txtMADDK.Text.Trim());

            //DateTime ngay8 = DateTimeUtil.GetVietNamDate(Convert.ToString(ngaydk2.NGAYDK));
            //String ngay3 = Convert.ToString(ngaydk2.NGAYDK);

            string ngay3 = ngaydk2.NGAYDK.Value.ToString("dd/mm/yyyy");

            String ngay = ngay3.Substring(0, 2);
            String thang = ngay3.Substring(3, 2);
            String nam = ngay3.Substring(6, 4);
            //DateTime ngaydk = DateTimeUtil.GetVietNamDate(ddkDao.Get(txtMADDK.Text.Trim()).NGAYDK);
            TextObject txtKINHGUI = rp.ReportDefinition.ReportObjects["txtKINHGUI"] as TextObject;
            txtKINHGUI.Text = "Kính gửi: Xí nghiệp Điện nước huyện, thị: " + tenphuong;
            TextObject txtDENGHI = rp.ReportDefinition.ReportObjects["txtDENGHI"] as TextObject;
            txtDENGHI.Text = "Đề nghị Xí nghiệp Điện nước huyện, thị " + tenphuong + " lắp đặt đồng hồ nước cho tôi để sử dụng vào mục đích: (đánh dấu x vào mục đích cần sử dụng).";
            TextObject txtNGAYDK = rp.ReportDefinition.ReportObjects["txtNGAYDK"] as TextObject;
            txtNGAYDK.Text = "An Giang, ngày " + ngay + " tháng " + thang + " năm " + nam;
            */
            
            reloadm.Text = "2";
            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        private void ReLoadBaoCao_mau()
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
                catch  {  }
            }

            DataTable dt = new ReportClass().INGIAYDENGHIN(txtMADDK.Text.Trim()).Tables[0];
            //DataTable dt = new ReportClass().INHOPDONG(txtMADDK.Text.Trim()).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/DonLapDatMoi/IN_GIAYDENGHIMAU.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            //divCR.Visible = true;
            //upnlCrystalReport.Update();
            
            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        protected void btnBrowseDDK_Click(object sender, EventArgs e)
        {
            BindDDK();
            upnlDonDangKy.Update();
            UnblockDialog("divDonDangKy");
        }

        protected void btnFilterDDK_Click(object sender, EventArgs e)
        {
            BindDDK();
            CloseWaitingDialog();
        }

        private void BindDDK()
        {
            DateTime? tungay = null;
            DateTime? denngay = null;
            try { tungay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim()); }
            catch { txtTuNgay.Text = ""; }
            try { denngay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim()); }
            catch { txtDenNgay.Text = ""; }

            //phong ban
            //var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            //if (loginInfo == null) return;
            string b = LoginInfo.MANV;
            var pb = _nvDao.GetKV(b);

            var list = ddkDao.GetList(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue, LoginInfo.MANV);
            //var list = ddkDao.GetListPB(txtFilter.Text.Trim(), pb.MAPB.ToString());

            gvDDK.DataSource = list;
            gvDDK.PagerInforText = list.Count.ToString();
            gvDDK.DataBind();
        }

        protected void gvDDK_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvDDK_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvDDK.PageIndex = e.NewPageIndex;               
                BindDDK();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDDK_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var obj = ddkDao.Get(id);
                        if (obj == null) return;

                        BindToInfor(obj);
                        CloseWaitingDialog();
                        HideDialog("divDonDangKy");

                        UpdateMode = Mode.Create;

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindToInfor(DONDANGKY obj)
        {
            //SetControlValue(txtMADDK.ClientID, obj.MADDK);
            txtMADDK.Text = obj.MADDK;
            //SetControlValue(txtTENKH.ClientID, ddkDao.Get(obj.MADDK).TENKH); 
            upnlInfor.Update();
        }

        protected void txtTENKH_TextChanged(object sender, EventArgs e)
        {

        }

    }
}