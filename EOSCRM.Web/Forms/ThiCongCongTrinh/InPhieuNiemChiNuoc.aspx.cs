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

namespace EOSCRM.Web.Forms.ThiCongCongTrinh
{
    public partial class InPhieuNiemChiNuoc : Authentication
    {
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly ReportClass _rpDao = new ReportClass();
        private readonly PhuongDao _pDao = new PhuongDao();
        private readonly DateTimeUtil _dateDao = new DateTimeUtil();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly ThiCongDao _tcDao = new ThiCongDao();
        private readonly BBNghiemThuDao _bbntDao = new BBNghiemThuDao();
        private readonly HopDongDao _hdDao = new HopDongDao();


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    //TODO: Load references
                    //LoadReferences();
                }
                else
                {
                    if (reloadm.Text == "1")
                    {
                        ReLoadBaoCao();
                    }
                    else if (reloadm.Text == "2")
                    {
                        ReLoadBaoCaoMau();
                    }
                    //ReLoadBaoCao();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
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
            Page.Title = Resources.Message.TITLE_TC_INPHIEUNIEMCHINUOC;

            var header = (Header)Master.FindControl("header");
            if (header == null) return;

            header.ModuleName = Resources.Message.MODULE_THICONG;
            header.TitlePage = Resources.Message.PAGE_TC_INPHIEUNIEMCHINUOC;

            //CommonFunc.SetPropertiesForGrid(gvDuongPho);
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
                                // ReSharper disable EmptyGeneralCatchClause
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
                catch
                {

                }
            }

            DataTable dt = new ReportClass().INPHIEUNIEMCHIN(txtMADDK.Text.Trim()).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/KiemDinhDongHo/PHIEUNIEMCHINUOC.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            var bbnt = _bbntDao.GetMADDK(txtMADDK.Text.Trim());

            if (bbnt != null)
            {
                if (bbnt.NGAYLAPBB != null)
                {
                    String ngay3 = bbnt.NGAYLAPBB.Value.ToString("dd/MM/yyyy").Substring(0, 10);
                    String ngaygan = ngay3.Substring(0, 10);
                    TextObject txtNGAYGAN = rp.ReportDefinition.ReportObjects["txtNGAYGAN"] as TextObject;
                    if (txtNGAYGAN != null)
                        txtNGAYGAN.Text = "Ngày " + ngaygan.Substring(0, 2) + " tháng " + ngaygan.Substring(3, 2) + " năm " + ngaygan.Substring(6, 4);
                }
                else
                {
                    TextObject txtNGAYGAN = rp.ReportDefinition.ReportObjects["txtNGAYGAN"] as TextObject;
                    if (txtNGAYGAN != null)
                        txtNGAYGAN.Text = "";
                }
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Chưa có đơn đăng ký này!");
                return;
            }

            var hopdong = _hdDao.Get(txtMADDK.Text.Trim());
            if (hopdong.MADP != null || hopdong.MADB != null)
            {
                TextObject txtDANHSO = rp.ReportDefinition.ReportObjects["txtDANHSO"] as TextObject;
                if (txtDANHSO != null)
                    txtDANHSO.Text = hopdong.MADP.ToString() + hopdong.MADB.ToString();
            }
            else
            {
                TextObject txtDANHSO = rp.ReportDefinition.ReportObjects["txtDANHSO"] as TextObject;
                if (txtDANHSO != null)
                    txtDANHSO.Text = "";
            }

            if (hopdong.SOHD != null)
            {
                TextObject txtSOHD = rp.ReportDefinition.ReportObjects["txtSOHD"] as TextObject;
                if (txtSOHD != null)
                    txtSOHD.Text = hopdong.SOHD.ToString();
            }
            else
            {
                TextObject txtSOHD = rp.ReportDefinition.ReportObjects["txtSOHD"] as TextObject;
                if (txtSOHD != null)
                    txtSOHD.Text = "";
            }

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.Get(b);//nhan vien khu vuc ??
            TextObject txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = query.HOTEN.ToString();

            divCR.Visible = true;
            upnlCrystalReport.Update();

            /*
            String maphuong = ddkDao.Get(txtMADDK.Text.Trim()).MAPHUONG;
            String tenphuong = _pDao.Get(maphuong).TENPHUONG.ToString();
            var ngaydk2 = ddkDao.Get(txtMADDK.Text.Trim());

            
            String ngay3 = Convert.ToString(ngaydk2.NGAYDK);

            String ngay = ngay3.Substring(0, 2);
            String thang = ngay3.Substring(3, 2);
            String nam = ngay3.Substring(6, 4);
            TextObject txtNGAYDK = rp.ReportDefinition.ReportObjects["txtNGAYDK"] as TextObject;
            txtNGAYDK.Text = "An Giang, ngày " + ngay + " tháng " + thang + " năm " + nam;

            //DateTime ngaydk = DateTimeUtil.GetVietNamDate(ddkDao.Get(txtMADDK.Text.Trim()).NGAYDK);
            TextObject txtKINHGUI = rp.ReportDefinition.ReportObjects["txtKINHGUI"] as TextObject;
            txtKINHGUI.Text = "Kính gửi: Xí nghiệp Điện nước huyện, thị: " + tenphuong;
            TextObject txtDENGHI = rp.ReportDefinition.ReportObjects["txtDENGHI"] as TextObject;
            txtDENGHI.Text = "Đề nghị Xí nghiệp Điện nước huyện, thị " + tenphuong + " lắp đặt đồng hồ nước cho tôi để sử dụng vào mục đích: (đánh dấu x vào mục đích cần sử dụng).";
            */
            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        protected void ReLoadBaoCao()
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

            DataTable dt = new ReportClass().INPHIEUNIEMCHIN(txtMADDK.Text.Trim()).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/KiemDinhDongHo/PHIEUNIEMCHINUOC.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            var bbnt = _bbntDao.GetMADDK(txtMADDK.Text.Trim());

            if (bbnt != null)
            {
                if (bbnt.NGAYLAPBB != null)
                {
                    String ngay3 = bbnt.NGAYLAPBB.Value.ToString("dd/MM/yyyy").Substring(0, 10);
                    String ngaygan = ngay3.Substring(0, 10);
                    TextObject txtNGAYGAN = rp.ReportDefinition.ReportObjects["txtNGAYGAN"] as TextObject;
                    if (txtNGAYGAN != null)
                        txtNGAYGAN.Text = "Ngày " + ngaygan.Substring(0, 2) + " tháng " + ngaygan.Substring(3, 2) + " năm " + ngaygan.Substring(6, 4);
                }
                else
                {
                    TextObject txtNGAYGAN = rp.ReportDefinition.ReportObjects["txtNGAYGAN"] as TextObject;
                    if (txtNGAYGAN != null)
                        txtNGAYGAN.Text = "";
                }
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Chưa có đơn đăng ký này!");
                return;
            }

            var hopdong = _hdDao.Get(txtMADDK.Text.Trim());
            if (hopdong.MADP != null || hopdong.MADB != null)
            {
                TextObject txtDANHSO = rp.ReportDefinition.ReportObjects["txtDANHSO"] as TextObject;
                if (txtDANHSO != null)
                    txtDANHSO.Text = hopdong.MADP.ToString() + hopdong.MADB.ToString();
            }
            else
            {
                TextObject txtDANHSO = rp.ReportDefinition.ReportObjects["txtDANHSO"] as TextObject;
                if (txtDANHSO != null)
                    txtDANHSO.Text = "";
            }

            if (hopdong.SOHD != null)
            {
                TextObject txtSOHD = rp.ReportDefinition.ReportObjects["txtSOHD"] as TextObject;
                if (txtSOHD != null)
                    txtSOHD.Text = hopdong.SOHD.ToString();
            }
            else
            {
                TextObject txtSOHD = rp.ReportDefinition.ReportObjects["txtSOHD"] as TextObject;
                if (txtSOHD != null)
                    txtSOHD.Text = "";
            }

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.Get(b);//nhan vien khu vuc ??
            TextObject txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = query.HOTEN.ToString();

            divCR.Visible = true;
            upnlCrystalReport.Update();

            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {

        }

        protected void linkNIEMCHIMAU_Click(object sender, EventArgs e)
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

            DataTable dt = new ReportClass().INPHIEUNIEMCHIN(txtMADDK.Text.Trim()).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/KiemDinhDongHo/PHIEUNIEMCHINUOCMAU.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            var bbnt = _bbntDao.GetMADDK(txtMADDK.Text.Trim());
            if (bbnt != null)
            {
                if (bbnt.NGAYLAPBB != null)
                {
                    String ngay3 = bbnt.NGAYLAPBB.Value.ToString("dd/MM/yyyy").Substring(0, 10);
                    String ngaygan = ngay3.Substring(0, 10);
                    TextObject txtNGAY = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
                    if (txtNGAY != null)
                        txtNGAY.Text = ngaygan.Substring(0, 2);
                    TextObject txtTHANG = rp.ReportDefinition.ReportObjects["txtTHANG"] as TextObject;
                    if (txtTHANG != null)
                        txtTHANG.Text = ngaygan.Substring(3, 2);

                    //+" tháng " + ngaygan.Substring(3, 2) + " năm " + ngaygan.Substring(6, 4);
                }
                else
                {
                    TextObject txtNGAY = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
                    if (txtNGAY != null)
                        txtNGAY.Text = "";
                    TextObject txtTHANG = rp.ReportDefinition.ReportObjects["txtTHANG"] as TextObject;
                    if (txtTHANG != null)
                        txtTHANG.Text = "";
                }
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Chưa có đơn đăng ký này!");
                return;
            }

            divCR.Visible = true;
            upnlCrystalReport.Update();

            reloadm.Text = "2";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        private void ReLoadBaoCaoMau()
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

            DataTable dt = new ReportClass().INPHIEUNIEMCHIN(txtMADDK.Text.Trim()).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/KiemDinhDongHo/PHIEUNIEMCHINUOCMAU.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            var bbnt = _bbntDao.GetMADDK(txtMADDK.Text.Trim());
            if (bbnt != null)
            {
                if (bbnt.NGAYLAPBB != null)
                {
                    String ngay3 = bbnt.NGAYLAPBB.Value.ToString("dd/MM/yyyy").Substring(0, 10);
                    String ngaygan = ngay3.Substring(0, 10);
                    TextObject txtNGAY = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
                    if (txtNGAY != null)
                        txtNGAY.Text = ngaygan.Substring(0, 2);
                    TextObject txtTHANG = rp.ReportDefinition.ReportObjects["txtTHANG"] as TextObject;
                    if (txtTHANG != null)
                        txtTHANG.Text = ngaygan.Substring(3, 2);

                    //+" tháng " + ngaygan.Substring(3, 2) + " năm " + ngaygan.Substring(6, 4);
                }
                else
                {
                    TextObject txtNGAY = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
                    if (txtNGAY != null)
                        txtNGAY.Text = "";
                    TextObject txtTHANG = rp.ReportDefinition.ReportObjects["txtTHANG"] as TextObject;
                    if (txtTHANG != null)
                        txtTHANG.Text = "";
                }
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Chưa có đơn đăng ký này!");
                return;
            }

            divCR.Visible = true;
            upnlCrystalReport.Update();

            reloadm.Text = "2";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }



    }
}