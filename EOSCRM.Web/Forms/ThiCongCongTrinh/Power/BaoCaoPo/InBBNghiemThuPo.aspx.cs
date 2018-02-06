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

namespace EOSCRM.Web.Forms.ThiCongCongTrinh.Power.BaoCaoPo
{
    public partial class InBBNghiemThuPo : Authentication
    {
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();        
        private readonly PhuongPoDao _ppoDao = new PhuongPoDao();        
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();        
        private readonly ThietKePoDao _tkpoDao = new ThietKePoDao();
        private readonly HopDongPoDao _hdpoDao = new HopDongPoDao();        
        private readonly BBNghiemThuPoDao _bbntpoDao = new BBNghiemThuPoDao();        
        private readonly DongHoPoDao _dhpoDao = new DongHoPoDao();
        private readonly LoaiDongHoPoDao _ldhpoDao = new LoaiDongHoPoDao();

        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly ReportClass _rpDao = new ReportClass();
        private readonly ThiCongDao _tcDao = new ThiCongDao();
        private readonly CongViecDao _cvDao = new CongViecDao();
        private readonly DateTimeUtil _dateDao = new DateTimeUtil();
        private readonly PhongBanDao _pbDao = new PhongBanDao();

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
                        ReLoadBaoCaoMau();
                    }
                    else if (reloadm.Text == "tk")
                    {
                        ReBaoCaoBBNTTK();
                    }
                    //ReLoadBaoCao();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void LoadReferences()
        {
            txtHOTENNV1.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
            txtNGAYIN.Text = DateTime.Now.ToString("dd/MM/yyyy");

            if (LoginInfo.NHANVIEN.MAPB != "KTDN")
            {
                btnBaoCao.Visible = false;
            }
            else
            {
                //btnBaoCao.Visible = true;
                btnBaoCaoTKN.Visible = false;
            }
        }

        #region Startup script registeration
        private void ShowError(string message, string controlId)
        {
            ((PO)Page.Master).ShowError(message, controlId);
        }

        private void ShowError(string message)
        {
            ((PO)Page.Master).ShowError(message);
        }

        private void ShowInfor(string message)
        {
            ((PO)Page.Master).ShowInfor(message);
        }

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }

        private void SetReadonly(string id, bool isReadonly)
        {
            ((PO)Page.Master).SetReadonly(id, isReadonly);
        }

        private void SetControlValue(string id, string value)
        {
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
        }

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        #endregion

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_TC_INBBNGHIEMTHUPO;

            var header = (Header)Master.FindControl("header");
            if (header == null) return;

            header.ModuleName = Resources.Message.MODULE_THICONG;
            header.TitlePage = Resources.Message.PAGE_TC_INBBNGHIEMTHUPO;

            //CommonFunc.SetPropertiesForGrid(gvDuongPho);
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
                catch { }
            }


            DataTable dt = new ReportClass().BaoCaoVatTuThietKePo(txtMADDK.Text.Trim()).Tables[0];
            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/KiemDinhDongHo/BBNghiemThuPo.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            var tenkv = _kvpoDao.GetPo(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var tke = _tkpoDao.Get(txtMADDK.Text.Trim());
            String ngay3, ngaytk = "", tencv11 = "", tencv12 = "", tencv13 = "";
            String tennv11 = "", tennv12 = "", tennv13 = "";
            if (tke != null)
            {
                ngay3 = Convert.ToString(tke.NGAYLTK.Value.ToString("dd/MM/yyyy"));
                ngaytk = ngay3.Substring(0, 10);
            }

            var dkk = _ddkpoDao.Get(txtMADDK.Text.Trim());
            var hd = _hdpoDao.Get(txtMADDK.Text.Trim());
            var pb = _pbDao.Get(LoginInfo.NHANVIEN.MAPB);
            var bb = _bbntpoDao.GetMADDK(txtMADDK.Text.Trim());
            if (bb == null)
            {
                ShowError("Chưa có biên bản nghiệm thu.");
                CloseWaitingDialog();
                return;
            }

            var nv1 = _nvDao.Get(bb.MANV1);
            if (nv1 != null)
            {
                var cv1 = _cvDao.Get(nv1.MACV);
                tennv11 = nv1.HOTEN;
                tencv11 = cv1.TENCV;
            }

            var nv2 = _nvDao.Get(bb.MANV2);
            if (nv2 != null)
            {
                var cv2 = _cvDao.Get(nv2.MACV);
                tennv12 = nv2.HOTEN;
                tencv12 = cv2.TENCV;
            }

            var nv3 = _nvDao.Get(bb.MANV3);
            if (nv3 != null)
            {
                var cv3 = _cvDao.Get(nv3.MACV);
                tennv13 = nv3.HOTEN;
                tencv13 = cv3.TENCV;
            }

            String ngay4 = "", ngaybb = "", thangbb = "", nambb = "";
            String sn1 = "", madp1 = "", madb1 = "", htcn = "";
            if (bb.NGAYLAPBB != null)
            {
                ngay4 = Convert.ToString(bb.NGAYLAPBB.Value.ToString("dd/MM/yyyy"));
                ngaybb = ngay4.Substring(0, 2);
                thangbb = ngay4.Substring(3, 2);
                nambb = ngay4.Substring(6, 4);
            }

            if (dkk.SONHA != null)
            { sn1 = dkk.SONHA; }
            else { sn1 = ""; }

            if (hd.MADPPO != null)
            { madp1 = hd.MADPPO; }
            else { madp1 = ""; }

            if (hd.MADB != null)
            { madb1 = hd.MADB; }
            else { madb1 = ""; }

            if (bb.HETHONGCN != null)
            { htcn = bb.HETHONGCN.ToString(); }
            else { htcn = ""; }

            string sotrukh = "", tentramkh = "", tuyendayhathe = "";
            if (tke.SOTRUKH != null)
                sotrukh = tke.SOTRUKH.ToString();
            if (tke.TENTRAMKH != null)
                tentramkh = tke.TENTRAMKH.ToString();
            if (tke.TUYENDAYHATHE != null)
                tuyendayhathe = tke.TUYENDAYHATHE.ToString();
            TextObject txtCANCU = rp.ReportDefinition.ReportObjects["txtCANCU"] as TextObject;
            if (txtCANCU != null)
                txtCANCU.Text = "   Căn cứ vào bảng thiết kế chiết tính vật tư ngày " + ngaytk + " của khách hàng là Ông (Bà): "
                    + dkk.TENKH + ", địa chỉ " + sn1 + ". Danh số: " + hd.MADPPO + hd.MADB + "; Trạm biến áp: " + tentramkh +
                    "; ĐZtrung thế:........ Sử dụng điện tại trụ số: " + sotrukh + "; đường dây hạ thế: " + tuyendayhathe + ".";
            TextObject txtLYDO = rp.ReportDefinition.ReportObjects["txtLYDO"] as TextObject;
            if (txtLYDO != null)
                txtLYDO.Text = "    Lý do nghiệm thu: " + tke.TENTK + ".";
            TextObject txtHOMNAY = rp.ReportDefinition.ReportObjects["txtHOMNAY"] as TextObject;
            if (txtHOMNAY != null)
                txtHOMNAY.Text = "    Hôm nay ngày " + ngaybb + " tháng " + thangbb + " năm " + nambb + ". Tại nhà.";
            TextObject txtTENNV1 = rp.ReportDefinition.ReportObjects["txtTENNV1"] as TextObject;
            if (txtTENNV1 != null)
                txtTENNV1.Text = tennv11 + ".";
            TextObject txtCV1 = rp.ReportDefinition.ReportObjects["txtCV1"] as TextObject;
            if (txtCV1 != null)
                txtCV1.Text = "Chức vụ: " + tencv11 + ".";
            TextObject txtTENNV2 = rp.ReportDefinition.ReportObjects["txtTENNV2"] as TextObject;
            if (txtTENNV2 != null)
                txtTENNV2.Text = tennv12 + ".";
            TextObject txtCV2 = rp.ReportDefinition.ReportObjects["txtCV2"] as TextObject;
            if (txtCV2 != null)
                txtCV2.Text = "Chức vụ: " + tencv12 + ".";
            TextObject txtTENNV3 = rp.ReportDefinition.ReportObjects["txtTENNV3"] as TextObject;
            if (txtTENNV3 != null)
                txtTENNV3.Text = tennv13 + ".";
            TextObject txtCV3 = rp.ReportDefinition.ReportObjects["txtCV3"] as TextObject;
            if (txtCV3 != null)
                txtCV3.Text = "Chức vụ: " + tencv13 + ".";


            String madh1 = "", sono1 = "", kdm1 = "", kdm2 = "", motakc = "", tenloaidhpo = "", nuocchetao = "", dongdien = "", dienthe = "";
            var tc = _tcDao.Get(txtMADDK.Text.Trim());
            if (tc.MADH != null)
            {
                var dh = _dhpoDao.Get(tc.MADH.ToString());
                var ldh = _ldhpoDao.Get(dh.MALDHPO.ToString());
                madh1 = dh.MADHPO;
                sono1 = dh.SONO;
                kdm1 = tc.CHIKDM1;
                kdm2 = tc.CHIKDM2;

                motakc = ldh.MOTAKC.ToString();
                tenloaidhpo = ldh.TENLOAIDHPO.ToString();
                nuocchetao = ldh.MOTANSX.ToString();
                dongdien = ldh.DONGDIEN.ToString();
                dienthe = ldh.DIENTHE.ToString();
            }
            TextObject txtLOAIDH = rp.ReportDefinition.ReportObjects["txtLOAIDH"] as TextObject;
            if (txtLOAIDH != null)
                txtLOAIDH.Text = "- Đồng hồ điện: Loại: " + motakc;
            TextObject txtHIEUDHPO = rp.ReportDefinition.ReportObjects["txtHIEUDHPO"] as TextObject;
            if (txtHIEUDHPO != null)
                txtHIEUDHPO.Text = "  + Hiệu: " + tenloaidhpo + "; Nước chế tạo: " + nuocchetao + "; Dòng điện: " + dongdien +
                        "; Điện thế: " + dienthe + ".";

            String madh2 = "", chieucao = "", khoangcach = "", chiniem1 = "", chiniem2 = "";
            if (bb.CHIEUCAO != null)
            { chieucao = Convert.ToString(bb.CHIEUCAO); }
            if (bb.MADHPO != null)
            { madh2 = bb.MADHPO; }
            TextObject txtSONO = rp.ReportDefinition.ReportObjects["txtSONO"] as TextObject;
            if (txtSONO != null)
                txtSONO.Text = "  + Số No: " + sono1 + "; Chỉ số tích luỹ: " + madh2 + "; Hệ số nhân trên hộp số:........";


            TextObject txtCHIKDM1 = rp.ReportDefinition.ReportObjects["txtCHIKDM1"] as TextObject;
            if (txtCHIKDM1 != null)
                txtCHIKDM1.Text = kdm1;
            TextObject txtCHIKDM2 = rp.ReportDefinition.ReportObjects["txtCHIKDM2"] as TextObject;
            if (txtCHIKDM2 != null)
                txtCHIKDM2.Text = kdm2;

            if (bb.CHINIEMM1 != null)
            { chiniem1 = bb.CHINIEMM1; }
            TextObject txtCHINIEMM1 = rp.ReportDefinition.ReportObjects["txtCHINIEMM1"] as TextObject;
            if (txtCHINIEMM1 != null)
                txtCHINIEMM1.Text = chiniem1;

            if (bb.CHINIEMM2 != null)
            { chiniem2 = bb.CHINIEMM2; }
            TextObject txtCHINIEMM2 = rp.ReportDefinition.ReportObjects["txtCHINIEMM2"] as TextObject;
            if (txtCHINIEMM2 != null)
                txtCHINIEMM2.Text = chiniem2;

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtHOTENNV1.Text.ToString();


            divCR.Visible = true;
            upnlCrystalReport.Update();


            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            BaoCaoBBNTCu();
        }

        protected void btnBaoCaoTKN_Click(object sender, EventArgs e)
        {
            BaoCaoBBNTTK();
        }

        protected void BaoCaoBBNTTK()
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

            DataTable dt = new ReportClass().BaoCaoVatTuThietKePo(txtMADDK.Text.Trim()).Tables[0];
            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/KiemDinhDongHo/BBNghiemThuNTKPo.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            var tenkv = _kvpoDao.GetPo(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var tke = _tkpoDao.Get(txtMADDK.Text.Trim());
            String ngay3, ngaytk = "", tencv11 = "", tencv12 = "", tencv13 = "";
            String tennv11 = "", tennv12 = "", tennv13 = "";
            if (tke != null)
            {
                ngay3 = Convert.ToString(tke.NGAYLTK.Value.ToString("dd/MM/yyyy"));
                ngaytk = ngay3.Substring(0, 10);
            }

            var dkk = _ddkpoDao.Get(txtMADDK.Text.Trim());
            var hd = _hdpoDao.Get(txtMADDK.Text.Trim());
            var pb = _pbDao.Get(LoginInfo.NHANVIEN.MAPB); 

            String ngay4 = "", ngaybb = "", thangbb = "", nambb = "";
            String sn1 = "", htcn = "";           

            if (dkk.SONHA != null)
            { sn1 = dkk.SONHA; }
            else { sn1 = ""; }                   

            string sotrukh = "", tentramkh = "", tuyendayhathe = "";
            if (tke.SOTRUKH != null)
            {
                if (tke.SOTRUKH != null)
                    sotrukh = tke.SOTRUKH.ToString();
                else sotrukh = ".................";
            }
            if (tke.TENTRAMKH != null)
            {
                if (tke.TENTRAMKH != null)
                    tentramkh = tke.TENTRAMKH.ToString();
                else tentramkh = ".....................";
            }
            if (tke.TUYENDAYHATHE != null)
            {
                if (tke.TUYENDAYHATHE != null)
                    tuyendayhathe = tke.TUYENDAYHATHE.ToString();
                else tuyendayhathe = "...........................";
            }
            TextObject txtCANCU = rp.ReportDefinition.ReportObjects["txtCANCU"] as TextObject;
            if (txtCANCU != null)
                txtCANCU.Text = "   Căn cứ vào bảng thiết kế chiết tính vật tư ngày " + ngaytk + " của khách hàng là Ông (Bà): "
                    + dkk.TENKH + ", địa chỉ " + sn1 + ". Danh số:.......................; Trạm biến áp: " + tentramkh +
                    "; ĐZtrung thế:........ Sử dụng điện tại trụ số: " + sotrukh + "; đường dây hạ thế: " + tuyendayhathe + ".";
            TextObject txtLYDO = rp.ReportDefinition.ReportObjects["txtLYDO"] as TextObject;
            if (txtLYDO != null)
                txtLYDO.Text = "    Lý do nghiệm thu: " + tke.TENTK + ".";
            TextObject txtHOMNAY = rp.ReportDefinition.ReportObjects["txtHOMNAY"] as TextObject;
            if (txtHOMNAY != null)
                txtHOMNAY.Text = "    Hôm nay ngày " + ngaybb + " tháng " + thangbb + " năm " + nambb + ". Tại nhà."; 
   

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtHOTENNV1.Text.ToString();


            divCR.Visible = true;
            upnlCrystalReport.Update();


            reloadm.Text = "tk";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
            
        }

        protected void ReBaoCaoBBNTTK()
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

            DataTable dt = new ReportClass().BaoCaoVatTuThietKePo(txtMADDK.Text.Trim()).Tables[0];
            rp = new ReportDocument();            
            var path = Server.MapPath("~/Reports/KiemDinhDongHo/BBNghiemThuNTKPo.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            var tenkv = _kvpoDao.GetPo(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var tke = _tkpoDao.Get(txtMADDK.Text.Trim());
            String ngay3, ngaytk = "";

            if (tke != null)
            {
                ngay3 = Convert.ToString(tke.NGAYLTK.Value.ToString("dd/MM/yyyy"));
                ngaytk = ngay3.Substring(0, 10);
            }
            else
            {
                ngay3 = "";
                ngaytk = "";
            }

            var dkk = _ddkpoDao.Get(txtMADDK.Text.Trim());
            var hd = _hdpoDao.Get(txtMADDK.Text.Trim());
            var pb = _pbDao.Get(LoginInfo.NHANVIEN.MAPB);
            var bb = _bbntpoDao.GetMADDK(txtMADDK.Text.Trim());
            /*if (bb == null)
            {
                ShowError("Chưa có biên bản nghiệm thu.");
                CloseWaitingDialog();
                return;
            }*/



            String sn1 = "", madp1 = "", madb1 = "", htcn = "", tenkhm = "";

            if (dkk.TENKH != null)
            { tenkhm = dkk.TENKH; }
            else { tenkhm = ""; }

            if (dkk.SONHA != null)
            { sn1 = dkk.SONHA; }
            else { sn1 = ""; }

            if (hd != null)
            {
                if (hd.MADPPO != null)
                { madp1 = hd.MADPPO; }
                else { madp1 = ""; }

                if (hd.MADB != null)
                { madb1 = hd.MADB; }
                else { madb1 = ""; }
            }

            if (bb != null)
            {
                if (bb.HETHONGCN != null)
                { htcn = bb.HETHONGCN.ToString(); }
                else { htcn = ""; }
            }
            TextObject txtCANCU = rp.ReportDefinition.ReportObjects["txtCANCU"] as TextObject;
            if (txtCANCU != null)
                txtCANCU.Text = "   Căn cứ vào bảng thiết kế chiết tính vật tư ngày " + ngaytk + " của khách hàng là "
                    + tenkhm + ", địa chỉ " + sn1 + ". Danh số:.........................; Thuộc HTCN: ....................................";

            TextObject txtLYDO = rp.ReportDefinition.ReportObjects["txtLYDO"] as TextObject;
            if (txtLYDO != null)
                txtLYDO.Text = "    Lý do nghiệm thu: ................";

            TextObject txtHOMNAY = rp.ReportDefinition.ReportObjects["txtHOMNAY"] as TextObject;
            if (txtHOMNAY != null)
                txtHOMNAY.Text = "    Hôm nay ngày ......, tháng ......., năm 201...... Chúng tôi tiến hành nghiệm thu đồng hồ nước của khách hàng. Nội dung như sau:";



            String madh1 = "", sono1 = "", kdm1 = "", kdm2 = "";
            var tc = _tcDao.Get(txtMADDK.Text.Trim());
            if (tc != null)
            {
                if (tc.MADH != null)
                {
                    var dh = _dhpoDao.Get(tc.MADH.ToString());
                    //var ldh = _ldhDao.Get(dh.MALDH.ToString());
                    madh1 = dh.MADHPO;
                    sono1 = dh.SONO;
                    kdm1 = tc.CHIKDM1;
                    kdm2 = tc.CHIKDM2;
                }
                else
                {
                    madh1 = "";
                    sono1 = "";
                    kdm1 = "";
                    kdm2 = "";
                }
            }


            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtHOTENNV1.Text.ToString();

            divCR.Visible = true;
            upnlCrystalReport.Update();

            reloadm.Text = "tk";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        protected void BaoCaoBBNTCu()
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


            DataTable dt = new ReportClass().BaoCaoVatTuThietKePo(txtMADDK.Text.Trim()).Tables[0];
            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/KiemDinhDongHo/BBNghiemThuPo.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            var tenkv = _kvpoDao.GetPo(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var tke = _tkpoDao.Get(txtMADDK.Text.Trim());
            String ngay3, ngaytk = "", tencv11 = "", tencv12 = "", tencv13 = "";
            String tennv11 = "", tennv12 = "", tennv13 = "";
            if (tke != null)
            {
                ngay3 = Convert.ToString(tke.NGAYLTK.Value.ToString("dd/MM/yyyy"));
                ngaytk = ngay3.Substring(0, 10);
            }

            var dkk = _ddkpoDao.Get(txtMADDK.Text.Trim());
            var hd = _hdpoDao.Get(txtMADDK.Text.Trim());
            var pb = _pbDao.Get(LoginInfo.NHANVIEN.MAPB);
            var bb = _bbntpoDao.GetMADDK(txtMADDK.Text.Trim());
            if (bb == null)
            {
                ShowError("Chưa có biên bản nghiệm thu.");
                CloseWaitingDialog();
                return;
            }

            var nv1 = _nvDao.Get(bb.MANV1);
            if (nv1 != null)
            {
                var cv1 = _cvDao.Get(nv1.MACV);
                tennv11 = nv1.HOTEN;
                tencv11 = cv1.TENCV;
            }

            var nv2 = _nvDao.Get(bb.MANV2);
            if (nv2 != null)
            {
                var cv2 = _cvDao.Get(nv2.MACV);
                tennv12 = nv2.HOTEN;
                tencv12 = cv2.TENCV;
            }

            var nv3 = _nvDao.Get(bb.MANV3);
            if (nv3 != null)
            {
                var cv3 = _cvDao.Get(nv3.MACV);
                tennv13 = nv3.HOTEN;
                tencv13 = cv3.TENCV;
            }

            String ngay4 = "", ngaybb = "", thangbb = "", nambb = "";
            String sn1 = "", madp1 = "", madb1 = "", htcn = "";
            if (bb.NGAYLAPBB != null)
            {
                ngay4 = Convert.ToString(bb.NGAYLAPBB.Value.ToString("dd/MM/yyyy"));
                ngaybb = ngay4.Substring(0, 2);
                thangbb = ngay4.Substring(3, 2);
                nambb = ngay4.Substring(6, 4);
            }

            if (dkk.SONHA != null)
            { sn1 = dkk.SONHA; }
            else { sn1 = ""; }

            if (hd.MADPPO != null)
            { madp1 = hd.MADPPO; }
            else { madp1 = ""; }

            if (hd.MADB != null)
            { madb1 = hd.MADB; }
            else { madb1 = ""; }

            if (bb.HETHONGCN != null)
            { htcn = bb.HETHONGCN.ToString(); }
            else { htcn = ""; }

            string sotrukh = "", tentramkh = "", tuyendayhathe = "";
            if (tke.SOTRUKH != null)
                sotrukh = tke.SOTRUKH.ToString();
            if (tke.TENTRAMKH != null)
                tentramkh = tke.TENTRAMKH.ToString();
            if (tke.TUYENDAYHATHE != null)
                tuyendayhathe = tke.TUYENDAYHATHE.ToString();
            TextObject txtCANCU = rp.ReportDefinition.ReportObjects["txtCANCU"] as TextObject;
            if (txtCANCU != null)
                txtCANCU.Text = "   Căn cứ vào bảng thiết kế chiết tính vật tư ngày " + ngaytk + " của khách hàng là Ông (Bà): "
                    + dkk.TENKH + ", địa chỉ " + sn1 + ". Danh số: " + hd.MADPPO + hd.MADB + "; Trạm biến áp: " + tentramkh +
                    "; ĐZtrung thế:........ Sử dụng điện tại trụ số: " + sotrukh + "; đường dây hạ thế: " + tuyendayhathe + ".";
            TextObject txtLYDO = rp.ReportDefinition.ReportObjects["txtLYDO"] as TextObject;
            if (txtLYDO != null)
                txtLYDO.Text = "    Lý do nghiệm thu: " + tke.TENTK + ".";
            TextObject txtHOMNAY = rp.ReportDefinition.ReportObjects["txtHOMNAY"] as TextObject;
            if (txtHOMNAY != null)
                txtHOMNAY.Text = "    Hôm nay ngày " + ngaybb + " tháng " + thangbb + " năm " + nambb + ". Tại nhà.";
            TextObject txtTENNV1 = rp.ReportDefinition.ReportObjects["txtTENNV1"] as TextObject;
            if (txtTENNV1 != null)
                txtTENNV1.Text = tennv11 + ".";
            TextObject txtCV1 = rp.ReportDefinition.ReportObjects["txtCV1"] as TextObject;
            if (txtCV1 != null)
                txtCV1.Text = "Chức vụ: " + tencv11 + ".";
            TextObject txtTENNV2 = rp.ReportDefinition.ReportObjects["txtTENNV2"] as TextObject;
            if (txtTENNV2 != null)
                txtTENNV2.Text = tennv12 + ".";
            TextObject txtCV2 = rp.ReportDefinition.ReportObjects["txtCV2"] as TextObject;
            if (txtCV2 != null)
                txtCV2.Text = "Chức vụ: " + tencv12 + ".";
            TextObject txtTENNV3 = rp.ReportDefinition.ReportObjects["txtTENNV3"] as TextObject;
            if (txtTENNV3 != null)
                txtTENNV3.Text = tennv13 + ".";
            TextObject txtCV3 = rp.ReportDefinition.ReportObjects["txtCV3"] as TextObject;
            if (txtCV3 != null)
                txtCV3.Text = "Chức vụ: " + tencv13 + ".";
            

            String madh1 = "", sono1 = "", kdm1 = "", kdm2 = "", motakc = "", tenloaidhpo = "", nuocchetao = "", dongdien = "", dienthe = "";
            var tc = _tcDao.Get(txtMADDK.Text.Trim());
            if (tc.MADH != null)
            {
                var dh = _dhpoDao.Get(tc.MADH.ToString());
                var ldh = _ldhpoDao.Get(dh.MALDHPO.ToString());
                madh1 = dh.MADHPO;
                sono1 = dh.SONO;
                kdm1 = tc.CHIKDM1;
                kdm2 = tc.CHIKDM2;

                motakc = ldh.MOTAKC.ToString();
                tenloaidhpo = ldh.TENLOAIDHPO.ToString();
                nuocchetao = ldh.MOTANSX.ToString();
                dongdien = ldh.DONGDIEN.ToString();
                dienthe = ldh.DIENTHE.ToString();
            }
            TextObject txtLOAIDH = rp.ReportDefinition.ReportObjects["txtLOAIDH"] as TextObject;
            if (txtLOAIDH != null)
                txtLOAIDH.Text = "- Đồng hồ điện: Loại: " + motakc ;
            TextObject txtHIEUDHPO = rp.ReportDefinition.ReportObjects["txtHIEUDHPO"] as TextObject;
            if (txtHIEUDHPO != null)
                txtHIEUDHPO.Text = "  + Hiệu: " + tenloaidhpo + "; Nước chế tạo: " + nuocchetao + "; Dòng điện: " + dongdien +
                        "; Điện thế: " + dienthe + ".";

            String madh2 = "", chieucao = "", khoangcach = "", chiniem1 = "", chiniem2 = "";
            if (bb.CHIEUCAO != null)
            { chieucao = Convert.ToString(bb.CHIEUCAO); }
            if (bb.MADHPO != null)
            { madh2 = bb.MADHPO; }
            TextObject txtSONO = rp.ReportDefinition.ReportObjects["txtSONO"] as TextObject;
            if (txtSONO != null)
                txtSONO.Text = "  + Số No: " + sono1 + "; Chỉ số tích luỹ: " + madh2 + "; Hệ số nhân trên hộp số:........";

           
            TextObject txtCHIKDM1 = rp.ReportDefinition.ReportObjects["txtCHIKDM1"] as TextObject;
            if (txtCHIKDM1 != null)
                txtCHIKDM1.Text = kdm1;
            TextObject txtCHIKDM2 = rp.ReportDefinition.ReportObjects["txtCHIKDM2"] as TextObject;
            if (txtCHIKDM2 != null)
                txtCHIKDM2.Text = kdm2;

            if (bb.CHINIEMM1 != null)
            { chiniem1 = bb.CHINIEMM1; }
            TextObject txtCHINIEMM1 = rp.ReportDefinition.ReportObjects["txtCHINIEMM1"] as TextObject;
            if (txtCHINIEMM1 != null)
                txtCHINIEMM1.Text = chiniem1;

            if (bb.CHINIEMM2 != null)
            { chiniem2 = bb.CHINIEMM2; }
            TextObject txtCHINIEMM2 = rp.ReportDefinition.ReportObjects["txtCHINIEMM2"] as TextObject;
            if (txtCHINIEMM2 != null)
                txtCHINIEMM2.Text = chiniem2;

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtHOTENNV1.Text.ToString();


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
                            var don = _ddkpoDao.Get(madon);
                            if (don == null) return;

                            var ds = _rpDao.INHOPDONG(madon.ToString());

                            if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                            //Report(ds.Tables[0]);
                            txtMADDK.Text = madon;

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

                var objList = _ddkpoDao.GetListHD(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), query.MAKV.ToString());
                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void linkBBNTHUMAU_Click(object sender, EventArgs e)
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

            //var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);           
            DataTable dt = new ReportClass().BaoCaoVatTuThietKe(txtMADDK.Text.Trim()).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/KiemDinhDongHo/BBNghiemThuMau.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            var tenkv = _kvpoDao.GetPo(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                //txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();
                txtTENKV.Text = tenkv.ToString().ToUpper();

            var tke = _tkpoDao.Get(txtMADDK.Text.Trim());
            String ngay3, ngaytk = "", tencv11 = "", tencv12 = "", tencv13 = "";
            String tennv11 = "", tennv12 = "", tennv13 = "";
            if (tke != null)
            {
                ngay3 = Convert.ToString(tke.NGAYLTK.Value.ToString("dd/MM/yyyy"));
                ngaytk = ngay3.Substring(0, 10);
            }

            var dkk = _ddkpoDao.Get(txtMADDK.Text.Trim());
            var hd = _hdpoDao.Get(txtMADDK.Text.Trim());
            var pb = _pbDao.Get(LoginInfo.NHANVIEN.MAPB);
            var bb = _bbntpoDao.GetMADDK(txtMADDK.Text.Trim());
            if (bb == null)
            {
                ShowError("Chưa có biên bản nghiệm thu.");
                CloseWaitingDialog();
                return;
            }

            var nv1 = _nvDao.Get(bb.MANV1);
            if (nv1 != null)
            {
                var cv1 = _cvDao.Get(nv1.MACV);
                tennv11 = nv1.HOTEN;
                tencv11 = cv1.TENCV;
            }

            var nv2 = _nvDao.Get(bb.MANV2);
            if (nv2 != null)
            {
                var cv2 = _cvDao.Get(nv2.MACV);
                tennv12 = nv2.HOTEN;
                tencv12 = cv2.TENCV;
            }

            var nv3 = _nvDao.Get(bb.MANV3);
            if (nv3 != null)
            {
                var cv3 = _cvDao.Get(nv3.MACV);
                tennv13 = nv3.HOTEN;
                tencv13 = cv3.TENCV;
            }

            String ngay4 = "", ngaybb = "", thangbb = "", nambb = "";
            String sn1 = "", madp1 = "", madb1 = "", htcn = "";
            if (bb.NGAYLAPBB != null)
            {
                ngay4 = Convert.ToString(bb.NGAYLAPBB.Value.ToString("dd/MM/yyyy"));
                ngaybb = ngay4.Substring(0, 2);
                thangbb = ngay4.Substring(3, 2);
                nambb = ngay4.Substring(6, 4);
            }

            if (dkk.SONHA != null)
            { sn1 = dkk.SONHA; }
            if (hd.MADPPO != null)
            { madp1 = hd.MADPPO; }
            if (hd.MADB != null)
            { madb1 = hd.MADB; }
            if (bb.HETHONGCN != null)
            { htcn = bb.HETHONGCN.ToString(); }
            else { htcn = ""; }


            TextObject txtHETHONGCN = rp.ReportDefinition.ReportObjects["txtHETHONGCN"] as TextObject;
            if (txtHETHONGCN != null)
                txtHETHONGCN.Text = htcn;
            TextObject txtLYDONT = rp.ReportDefinition.ReportObjects["txtLYDONT"] as TextObject;
            if (txtLYDONT != null)
                txtLYDONT.Text = "Lắp mới.";
            /*TextObject txtKETLUANNT = rp.ReportDefinition.ReportObjects["txtKETLUANNT"] as TextObject;
            if (txtKETLUANNT != null)
                txtKETLUANNT.Text = "Thuỷ lượng kế hoạt động bình thường‎, ‎lắp đặt đúng qui định‎, ‎hoạ‎t đ‎ộ‎ng ‎bì‎nh th‎ườ‎ng‎.‎";*/
            /*Thuỷ lượng kế hoạt động bình thường‎, ‎lắp đặt đúng qui định‎, ‎hoạ‎t đ‎ộ‎ng ‎bì‎nh th‎ườ‎ng‎.‎
            TextObject txtCANCU = rp.ReportDefinition.ReportObjects["txtCANCU"] as TextObject;
            if (txtCANCU != null)
                txtCANCU.Text = "   Căn cứ vào bảng thiết kế chiết tính vật tư ngày " + ngaytk + " của khách hàng là " + dkk.TENKH +
                    ", địa chỉ " + sn1 + ". Danh số: " + hd.MADP + hd.MADB + " thuộc HTCN: " + pb.TENPB + ".";
            
            TextObject txtLYDO = rp.ReportDefinition.ReportObjects["txtLYDO"] as TextObject;
            if (txtLYDO != null)
                txtLYDO.Text = "    Lý do nghiệm thu: " + tke.TENTK + ".";
            */
            TextObject txtNGAYTK = rp.ReportDefinition.ReportObjects["txtNGAYTK"] as TextObject;
            if (txtNGAYTK != null)
                txtNGAYTK.Text = ngaytk;
            TextObject txtTENKHM = rp.ReportDefinition.ReportObjects["txtTENKHM"] as TextObject;
            if (txtTENKHM != null)
                txtTENKHM.Text = dkk.TENKH;

            TextObject txtDANHSOM = rp.ReportDefinition.ReportObjects["txtDANHSOM"] as TextObject;
            if (txtDANHSOM != null)
                txtDANHSOM.Text = hd.MADPPO + hd.MADB;



            TextObject txtHOMNAY = rp.ReportDefinition.ReportObjects["txtHOMNAY"] as TextObject;
            if (txtHOMNAY != null)
                //txtHOMNAY.Text = "    Hôm nay ngày " + ngaybb + " tháng " + thangbb + " năm " + nambb + ". Chúng tôi tiến hành nghiệm thu đồng hồ nước của khách hàng. Nội dung như sau:";
                txtHOMNAY.Text = ngaybb + " / " + thangbb + " / " + nambb;


            TextObject txtTENNV1 = rp.ReportDefinition.ReportObjects["txtTENNV1"] as TextObject;
            if (txtTENNV1 != null)
                txtTENNV1.Text = tennv11 + ".";
            TextObject txtCV1 = rp.ReportDefinition.ReportObjects["txtCV1"] as TextObject;
            if (txtCV1 != null)
                txtCV1.Text = "Chức vụ: " + tencv11 + ".";
            TextObject txtTENNV2 = rp.ReportDefinition.ReportObjects["txtTENNV2"] as TextObject;
            if (txtTENNV2 != null)
                txtTENNV2.Text = tennv12 + ".";
            TextObject txtCV2 = rp.ReportDefinition.ReportObjects["txtCV2"] as TextObject;
            if (txtCV2 != null)
                txtCV2.Text = "Chức vụ: " + tencv12 + ".";
            TextObject txtTENNV3 = rp.ReportDefinition.ReportObjects["txtTENNV3"] as TextObject;
            if (txtTENNV3 != null)
                txtTENNV3.Text = tennv13 + ".";
            TextObject txtCV3 = rp.ReportDefinition.ReportObjects["txtCV3"] as TextObject;
            if (txtCV3 != null)
                txtCV3.Text = "Chức vụ: " + tencv13 + ".";

            String madh1 = "", sono1 = "", kdm1 = "", kdm2 = "";
            var tc = _tcDao.Get(txtMADDK.Text.Trim());
            if (tc.MADH != null)
            {
                var dh = _dhpoDao.Get(tc.MADH.ToString());
                //var ldh = _ldhDao.Get(dh.MALDH.ToString());
                madh1 = dh.MADHPO;
                sono1 = dh.SONO;
                kdm1 = tc.CHIKDM1;
                kdm2 = tc.CHIKDM2;
            }


            TextObject txtLOAIDH = rp.ReportDefinition.ReportObjects["txtLOAIDH"] as TextObject;
            if (txtLOAIDH != null)
                txtLOAIDH.Text = madh1;


            String madh2 = "", chieucao = "", khoangcach = "", chiniem1 = "", chiniem2 = "";

            if (bb.CHIEUCAO != null)
            { chieucao = Convert.ToString(bb.CHIEUCAO); }
            if (bb.MADHPO != null)
            { madh2 = bb.MADHPO; }


            TextObject txtSONO = rp.ReportDefinition.ReportObjects["txtSONO"] as TextObject;
            if (txtSONO != null)
                //txtSONO.Text = "Số No: " + sono1 + "; Chỉ số đồng hồ: " + madh2 + "; Chiều cao lắp đặt: " + chieucao + ".";
                txtSONO.Text = sono1;
            TextObject txtCHISODH = rp.ReportDefinition.ReportObjects["txtCHISODH"] as TextObject;
            if (txtCHISODH != null)
                txtCHISODH.Text = madh2;
            TextObject txtCHIEUCAOL = rp.ReportDefinition.ReportObjects["txtCHIEUCAOL"] as TextObject;
            if (txtCHIEUCAOL != null)
                txtCHIEUCAOL.Text = chieucao;

            if (bb.KHOANGCACH != null)
            { khoangcach = Convert.ToString(bb.KHOANGCACH); }
            TextObject txtKHOANGCACH = rp.ReportDefinition.ReportObjects["txtKHOANGCACH"] as TextObject;
            if (txtKHOANGCACH != null)
                //txtKHOANGCACH.Text = "Khoảng cách từ ống phân phối hoặc vị trí chẻ tê đến đồng hồ nước:  " + khoangcach + " m .";
                txtKHOANGCACH.Text = khoangcach + " m;";

            TextObject txtCHIKDM1 = rp.ReportDefinition.ReportObjects["txtCHIKDM1"] as TextObject;
            if (txtCHIKDM1 != null)
                txtCHIKDM1.Text = kdm1;
            TextObject txtCHIKDM2 = rp.ReportDefinition.ReportObjects["txtCHIKDM2"] as TextObject;
            if (txtCHIKDM2 != null)
                txtCHIKDM2.Text = kdm2;

            if (bb.CHINIEMM1 != null)
            { chiniem1 = bb.CHINIEMM1; }
            TextObject txtCHINIEMM1 = rp.ReportDefinition.ReportObjects["txtCHINIEMM1"] as TextObject;
            if (txtCHINIEMM1 != null)
                txtCHINIEMM1.Text = chiniem1;

            if (bb.CHINIEMM2 != null)
            { chiniem2 = bb.CHINIEMM2; }
            TextObject txtCHINIEMM2 = rp.ReportDefinition.ReportObjects["txtCHINIEMM2"] as TextObject;
            if (txtCHINIEMM2 != null)
                txtCHINIEMM2.Text = chiniem2;


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
                catch { }
            }

            //var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);           
            DataTable dt = new ReportClass().BaoCaoVatTuThietKe(txtMADDK.Text.Trim()).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/KiemDinhDongHo/BBNghiemThuMau.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            var tenkv = _kvpoDao.GetPo(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                //txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();
                txtTENKV.Text = tenkv.ToString().ToUpper();

            var tke = _tkpoDao.Get(txtMADDK.Text.Trim());
            String ngay3, ngaytk = "", tencv11 = "", tencv12 = "", tencv13 = "";
            String tennv11 = "", tennv12 = "", tennv13 = "";
            if (tke != null)
            {
                ngay3 = Convert.ToString(tke.NGAYLTK.Value.ToString("dd/MM/yyyy"));
                ngaytk = ngay3.Substring(0, 10);
            }

            var dkk = _ddkpoDao.Get(txtMADDK.Text.Trim());
            var hd = _hdpoDao.Get(txtMADDK.Text.Trim());
            var pb = _pbDao.Get(LoginInfo.NHANVIEN.MAPB);
            var bb = _bbntpoDao.GetMADDK(txtMADDK.Text.Trim());
            if (bb == null)
            {
                ShowError("Chưa có biên bản nghiệm thu.");
                CloseWaitingDialog();
                return;
            }

            var nv1 = _nvDao.Get(bb.MANV1);
            if (nv1 != null)
            {
                var cv1 = _cvDao.Get(nv1.MACV);
                tennv11 = nv1.HOTEN;
                tencv11 = cv1.TENCV;
            }

            var nv2 = _nvDao.Get(bb.MANV2);
            if (nv2 != null)
            {
                var cv2 = _cvDao.Get(nv2.MACV);
                tennv12 = nv2.HOTEN;
                tencv12 = cv2.TENCV;
            }

            var nv3 = _nvDao.Get(bb.MANV3);
            if (nv3 != null)
            {
                var cv3 = _cvDao.Get(nv3.MACV);
                tennv13 = nv3.HOTEN;
                tencv13 = cv3.TENCV;
            }

            String ngay4 = "", ngaybb = "", thangbb = "", nambb = "";
            String sn1 = "", madp1 = "", madb1 = "", htcn = "";
            if (bb.NGAYLAPBB != null)
            {
                ngay4 = Convert.ToString(bb.NGAYLAPBB.Value.ToString("dd/MM/yyyy"));
                ngaybb = ngay4.Substring(0, 2);
                thangbb = ngay4.Substring(3, 2);
                nambb = ngay4.Substring(6, 4);
            }

            if (dkk.SONHA != null)
            { sn1 = dkk.SONHA; }
            if (hd.MADPPO != null)
            { madp1 = hd.MADPPO; }
            if (hd.MADB != null)
            { madb1 = hd.MADB; }
            if (bb.HETHONGCN != null)
            { htcn = bb.HETHONGCN.ToString(); }
            else { htcn = ""; }


            TextObject txtHETHONGCN = rp.ReportDefinition.ReportObjects["txtHETHONGCN"] as TextObject;
            if (txtHETHONGCN != null)
                txtHETHONGCN.Text = htcn;
            TextObject txtLYDONT = rp.ReportDefinition.ReportObjects["txtLYDONT"] as TextObject;
            if (txtLYDONT != null)
                txtLYDONT.Text = "Lắp mới.";
            /*TextObject txtKETLUANNT = rp.ReportDefinition.ReportObjects["txtKETLUANNT"] as TextObject;
            if (txtKETLUANNT != null)
                txtKETLUANNT.Text = "Thuỷ lượng kế hoạt động bình thường‎, ‎lắp đặt đúng qui định‎, ‎hoạ‎t đ‎ộ‎ng ‎bì‎nh th‎ườ‎ng‎.‎";*/
            /*Thuỷ lượng kế hoạt động bình thường‎, ‎lắp đặt đúng qui định‎, ‎hoạ‎t đ‎ộ‎ng ‎bì‎nh th‎ườ‎ng‎.‎
            TextObject txtCANCU = rp.ReportDefinition.ReportObjects["txtCANCU"] as TextObject;
            if (txtCANCU != null)
                txtCANCU.Text = "   Căn cứ vào bảng thiết kế chiết tính vật tư ngày " + ngaytk + " của khách hàng là " + dkk.TENKH +
                    ", địa chỉ " + sn1 + ". Danh số: " + hd.MADP + hd.MADB + " thuộc HTCN: " + pb.TENPB + ".";
            
            TextObject txtLYDO = rp.ReportDefinition.ReportObjects["txtLYDO"] as TextObject;
            if (txtLYDO != null)
                txtLYDO.Text = "    Lý do nghiệm thu: " + tke.TENTK + ".";
            */
            TextObject txtNGAYTK = rp.ReportDefinition.ReportObjects["txtNGAYTK"] as TextObject;
            if (txtNGAYTK != null)
                txtNGAYTK.Text = ngaytk;
            TextObject txtTENKHM = rp.ReportDefinition.ReportObjects["txtTENKHM"] as TextObject;
            if (txtTENKHM != null)
                txtTENKHM.Text = dkk.TENKH;

            TextObject txtDANHSOM = rp.ReportDefinition.ReportObjects["txtDANHSOM"] as TextObject;
            if (txtDANHSOM != null)
                txtDANHSOM.Text = hd.MADPPO + hd.MADB;



            TextObject txtHOMNAY = rp.ReportDefinition.ReportObjects["txtHOMNAY"] as TextObject;
            if (txtHOMNAY != null)
                //txtHOMNAY.Text = "    Hôm nay ngày " + ngaybb + " tháng " + thangbb + " năm " + nambb + ". Chúng tôi tiến hành nghiệm thu đồng hồ nước của khách hàng. Nội dung như sau:";
                txtHOMNAY.Text = ngaybb + " / " + thangbb + " / " + nambb;


            TextObject txtTENNV1 = rp.ReportDefinition.ReportObjects["txtTENNV1"] as TextObject;
            if (txtTENNV1 != null)
                txtTENNV1.Text = tennv11 + ".";
            TextObject txtCV1 = rp.ReportDefinition.ReportObjects["txtCV1"] as TextObject;
            if (txtCV1 != null)
                txtCV1.Text = "Chức vụ: " + tencv11 + ".";
            TextObject txtTENNV2 = rp.ReportDefinition.ReportObjects["txtTENNV2"] as TextObject;
            if (txtTENNV2 != null)
                txtTENNV2.Text = tennv12 + ".";
            TextObject txtCV2 = rp.ReportDefinition.ReportObjects["txtCV2"] as TextObject;
            if (txtCV2 != null)
                txtCV2.Text = "Chức vụ: " + tencv12 + ".";
            TextObject txtTENNV3 = rp.ReportDefinition.ReportObjects["txtTENNV3"] as TextObject;
            if (txtTENNV3 != null)
                txtTENNV3.Text = tennv13 + ".";
            TextObject txtCV3 = rp.ReportDefinition.ReportObjects["txtCV3"] as TextObject;
            if (txtCV3 != null)
                txtCV3.Text = "Chức vụ: " + tencv13 + ".";

            String madh1 = "", sono1 = "", kdm1 = "", kdm2 = "";
            var tc = _tcDao.Get(txtMADDK.Text.Trim());
            if (tc.MADH != null)
            {
                var dh = _dhpoDao.Get(tc.MADH.ToString());
                //var ldh = _ldhDao.Get(dh.MALDH.ToString());
                madh1 = dh.MADHPO;
                sono1 = dh.SONO;
                kdm1 = tc.CHIKDM1;
                kdm2 = tc.CHIKDM2;
            }


            TextObject txtLOAIDH = rp.ReportDefinition.ReportObjects["txtLOAIDH"] as TextObject;
            if (txtLOAIDH != null)
                txtLOAIDH.Text = madh1;


            String madh2 = "", chieucao = "", khoangcach = "", chiniem1 = "", chiniem2 = "";

            if (bb.CHIEUCAO != null)
            { chieucao = Convert.ToString(bb.CHIEUCAO); }
            if (bb.MADHPO != null)
            { madh2 = bb.MADHPO; }


            TextObject txtSONO = rp.ReportDefinition.ReportObjects["txtSONO"] as TextObject;
            if (txtSONO != null)
                //txtSONO.Text = "Số No: " + sono1 + "; Chỉ số đồng hồ: " + madh2 + "; Chiều cao lắp đặt: " + chieucao + ".";
                txtSONO.Text = sono1;
            TextObject txtCHISODH = rp.ReportDefinition.ReportObjects["txtCHISODH"] as TextObject;
            if (txtCHISODH != null)
                txtCHISODH.Text = madh2;
            TextObject txtCHIEUCAOL = rp.ReportDefinition.ReportObjects["txtCHIEUCAOL"] as TextObject;
            if (txtCHIEUCAOL != null)
                txtCHIEUCAOL.Text = chieucao;

            if (bb.KHOANGCACH != null)
            { khoangcach = Convert.ToString(bb.KHOANGCACH); }
            TextObject txtKHOANGCACH = rp.ReportDefinition.ReportObjects["txtKHOANGCACH"] as TextObject;
            if (txtKHOANGCACH != null)
                //txtKHOANGCACH.Text = "Khoảng cách từ ống phân phối hoặc vị trí chẻ tê đến đồng hồ nước:  " + khoangcach + " m .";
                txtKHOANGCACH.Text = khoangcach + " m;";

            TextObject txtCHIKDM1 = rp.ReportDefinition.ReportObjects["txtCHIKDM1"] as TextObject;
            if (txtCHIKDM1 != null)
                txtCHIKDM1.Text = kdm1;
            TextObject txtCHIKDM2 = rp.ReportDefinition.ReportObjects["txtCHIKDM2"] as TextObject;
            if (txtCHIKDM2 != null)
                txtCHIKDM2.Text = kdm2;

            if (bb.CHINIEMM1 != null)
            { chiniem1 = bb.CHINIEMM1; }
            TextObject txtCHINIEMM1 = rp.ReportDefinition.ReportObjects["txtCHINIEMM1"] as TextObject;
            if (txtCHINIEMM1 != null)
                txtCHINIEMM1.Text = chiniem1;

            if (bb.CHINIEMM2 != null)
            { chiniem2 = bb.CHINIEMM2; }
            TextObject txtCHINIEMM2 = rp.ReportDefinition.ReportObjects["txtCHINIEMM2"] as TextObject;
            if (txtCHINIEMM2 != null)
                txtCHINIEMM2.Text = chiniem2;


            divCR.Visible = true;
            upnlCrystalReport.Update();



            reloadm.Text = "2";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();

        }

    }
}