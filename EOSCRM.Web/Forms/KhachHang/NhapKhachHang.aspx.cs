using System;
using System.Web.UI.WebControls;
using System.Globalization;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Data;

namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class NhapKhachHang : Authentication
    {
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly DotInHDDao _dihdDao = new DotInHDDao();
        private readonly DMThuHoDao _dmthDao = new DMThuHoDao();
        private readonly ThietKeDao _tkDao = new ThietKeDao(); 
        private readonly KhachHangTamLXDao _khtlxDao = new KhachHangTamLXDao();
        private readonly KhachHangDao khDao = new KhachHangDao();
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly DongHoDao dhDao = new DongHoDao();
        private readonly LoaiDongHoDao ldhDao = new LoaiDongHoDao();
        private readonly HopDongDao hdDao = new HopDongDao();
        private readonly PhuongDao pDao = new PhuongDao();
        private readonly KhuVucDao kvDao = new KhuVucDao();
        private readonly GhiChiSoDao _gcsDao = new GhiChiSoDao();
        private readonly DanhSachCoQuanThanhToanDao cqDao = new DanhSachCoQuanThanhToanDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly ThiCongDao _tcDao = new ThiCongDao();
        private readonly HoNgheoNDao _hnnDao = new HoNgheoNDao();
        private readonly XaPhuongDao _xpDao = new XaPhuongDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DonDangKyDao _ddkDao = new DonDangKyDao();

        int thangF, namF;

        private KHACHHANG KhachHang
        {
            get
            {
                if (!IsDataValid())
                    return null;

                var kh = string.IsNullOrEmpty(hdfIDKH.Value) ? new KHACHHANG() : khDao.Get(hdfIDKH.Value);
                if (kh == null)
                    return null;

                kh.MADB = txtMADB.Text.Trim();
                kh.MADP = txtMADP.Text.ToUpper().Trim();
                kh.DUONGPHU = txtDUONGPHU.Text.Trim();
                //kh.MALKHDB = ddlLKHDB.SelectedValue;
                kh.MALKHDB = "D";
                kh.MAMDSD = ckISHONGHEO.Checked == true  ? "Z" : ddlMDSD.SelectedValue;//CO HO NGHEO
                kh.MADDK = lbMADDK.Text.Trim();
                kh.SOHD = txtSOHD.Text.Trim();
                kh.MABG = ddlMAGIA.SelectedValue.Equals("NULL") ? null : ddlMAGIA.SelectedValue;                
                kh.MAHOTRO = ddlHOTRO.SelectedValue;
                //kh.MAPHUONG = String.IsNullOrEmpty(ddlPHUONG.SelectedValue) ? null : ddlPHUONG.SelectedValue;
                kh.MAPHUONG = String.IsNullOrEmpty(ddlPHUONG.SelectedValue) ? null :
                    txtMADP.Text.ToUpper().Trim().Substring(1,1);
                kh.TENKH = txtTENKH.Text.Trim();
                kh.SONHA = txtDIACHILD.Text.Trim();
                kh.SONHA2 = txtSONHA2.Text.Trim();

                kh.MST = txtMSTHUE.Text.Trim();
                kh.MAHTTT = ddlHTTT.SelectedValue;
                kh.STK = txtSOTK.Text.Trim();
                kh.SDT = txtSDT.Text.Trim();
                kh.TTSD = "";
                kh.KOPHINT = false;
                kh.THUYLK = ddlKICHCODH.SelectedValue;
                kh.NGAYGNHAP = DateTime.Now;                
                kh.VAT = true;
                //kh.KHONGTINH117 = cbKHONGTINH117.Checked;
                kh.KHONGTINH117 = false;
                //kh.MATT = ddlTT.SelectedValue;
                kh.MATT = "GDH_BT";

                string kykt = "01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim();
                kh.KYKHAITHAC = DateTimeUtil.GetVietNamDate(kykt);
                kh.GHI2THANG1LAN = ddlGHI2THANG1LAN.SelectedValue;
                kh.THUHO = ddlTHUHO.SelectedValue;

                var dp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
                if (dp == null)
                    return null;

                kh.MAKV = dp.MAKV;
                //kh.MAKVDN = ddlKHUVUCDN.SelectedValue;
                kh.MAKVDN = "O";

                if (!string.IsNullOrEmpty(txtCQ.Text.Trim()))
                    kh.MACQ = txtCQ.Text.Trim();

                //if (!string.IsNullOrEmpty(txtSOHO.Text.Trim()))
                //    kh.SOHO = decimal.Parse(txtSOHO.Text.Trim());
                //dinhmuc
                kh.SOHO = int.Parse(txtSOHO.Text.Trim());
                kh.SONK = int.Parse(txtSONK.Text.Trim());
                kh.SODINHMUC = int.Parse(txtSODINHMUC.Text.Trim());
                kh.ISDINHMUC = cbISDINHMUC.Checked;

                //vi pham
                kh.ISVIPH = false;

                /*string dateht = txtNGAYHT.Text.Trim();
                string ngayht = dateht.Substring(0, 2);
                string thanght = dateht.Substring(2, 2);
                string namht = dateht.Substring(4, 2);
                string dateht1 = ngayht + "/" + thanght + "/20" + namht;*/
                
                if (!string.IsNullOrEmpty(txtNGAYHT.Text.Trim()))
                    kh.NGAYHT = DateTimeUtil.GetVietNamDate(txtNGAYHT.Text.Trim());
                if (!string.IsNullOrEmpty(txtKYHOTRO.Text.Trim()))
                    kh.KYHOTRO = DateTimeUtil.GetVietNamDate("01/" + txtKYHOTRO.Text.Trim());
                if (!string.IsNullOrEmpty(txtMALDH.Text.Trim()))
                    kh.MALDH = txtMALDH.Text.Trim();
                if (!string.IsNullOrEmpty(txtMADH.Text.Trim()))
                    kh.MADH = txtMADH.Text.Trim();

                //if (!string.IsNullOrEmpty(txtM3.Text.Trim()))
                //    kh.KLKHOAN = int.Parse(txtM3.Text.Trim());
                kh.KLKHOAN = 4;

                if (!string.IsNullOrEmpty(txtSTT.Text.Trim()))
                    kh.STT = int.Parse(txtSTT.Text.Trim());

                kh.SDInfo_INHOADON = cbSDInfo_INHOADON.Checked;
                if (cbSDInfo_INHOADON.Checked) {
                    kh.TENKH_INHOADON = txtTENKH_INHOADON.Text.Trim();
                    //kh.DIACHI_INHOADON = txtDIACHI_INHOADON.Text.Trim();
                    kh.DIACHI_INHOADON2 = txtDIACHI_INHOADON.Text.Trim();
                }
                
                //if (!string.Empty.Equals(txtNAM.Text.Trim()))
                //    kh.NAMBDKT = int.Parse(txtNAM.Text.Trim());
                kh.NAMBDKT = DateTime.Now.Year;

                //kh.THANGBDKT = int.Parse(ddlTHANG.SelectedValue);
                kh.THANGBDKT = DateTime.Now.Month;

                //if (!string.Empty.Equals(txtCHISODAU.Text.Trim()))
                //    kh.CHISODAU = decimal.Parse(txtCHISODAU.Text.Trim());
                //kh.CHISODAU = 0;

                //so ho ngheo
                kh.ISHONGHEO = ckISHONGHEO.Checked;

                if (!string.Empty.Equals(txtCHISODAU.Text.Trim()))
                    kh.CHISODAU = decimal.Parse(txtCHISODAU.Text.Trim());
                if (!string.Empty.Equals(txtCHISOCUOI.Text.Trim()))
                    kh.CHISOCUOI = decimal.Parse(txtCHISOCUOI.Text.Trim());
                if (!string.Empty.Equals(txtTRUYTHU.Text.Trim()))
                    kh.MTRUYTHU = decimal.Parse(txtTRUYTHU.Text.Trim());

                kh.XOABOKH = false;

                kh.IDKHLX = txtIDKHLX.Text.Trim();
                kh.TIENCOCLX = Convert.ToDecimal(txtTIENCOCLX.Text.Trim());

                return kh;
            }

            set
            {
                if (value == null)
                    return;

                ClearForm();

                hdfIDKH.Value = value.IDKH;

                txtMADB.Text = value.MADB;
                txtMADP.Text = value.MADP;
                txtDUONGPHU.Text = value.DUONGPHU;
                lblTENDUONG.Text = value.DUONGPHO.TENDP;                
                
                var item = ddlLKHDB.Items.FindByValue(value.MALKHDB);
                if(item != null)
                    ddlLKHDB.SelectedIndex = ddlLKHDB.Items.IndexOf(item);

                var cq = cqDao.Get(value.MACQ);
                if (cq != null)
                {
                    txtCQ.Text = cq.MACQ;
                    lblTENCQ.Text = cq.TENCQ;
                }

                var item1 = ddlMDSD.Items.FindByValue(value.MAMDSD);
                if(item1 != null)
                    ddlMDSD.SelectedIndex = ddlMDSD.Items.IndexOf(item1);

                txtSOHD.Text = value.SOHD;
               
                var kv = kvDao.Get(value.MAKV);

                var item5 = ddlKHUVUC.Items.FindByValue(value.MAKV);
                if (item5 != null)
                    ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(item5);

                var item9 = ddlKHUVUCDN.Items.FindByValue(value.MAKVDN);
                if (item9 != null)
                    ddlKHUVUCDN.SelectedIndex = ddlKHUVUCDN.Items.IndexOf(item9);

                /**
                 * re-bind phuong
                 * 
                 */
                LoadDynamicReferences(kv);

                var item2 = ddlPHUONG.Items.FindByValue(value.MAPHUONG);
                if(item2 != null)
                    ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(item2);

                txtTENKH.Text = value.TENKH;
                txtDIACHILD.Text = value.SONHA;

                txtSOHO.Text = value.SOHO.HasValue ? value.SOHO.Value.ToString() : "1";
                txtSONK.Text = value.SONK.HasValue ? value.SONK.Value.ToString() : "1";
                cbISDINHMUC.Checked = value.ISDINHMUC.HasValue && value.ISDINHMUC.Value;

                txtMSTHUE.Text = value.MST;

                var item3 = ddlPHUONG.Items.FindByValue(value.MAPHUONG);
                if(item3 != null)
                    ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(item3);

                var item4 = ddlHTTT.Items.FindByValue(value.MAHTTT);
                if(item4 != null)
                    ddlHTTT.SelectedIndex = ddlHTTT.Items.IndexOf(item4);

                var g2t1l = ddlGHI2THANG1LAN.Items.FindByValue(value.GHI2THANG1LAN);
                if(g2t1l != null)
                {
                    ddlGHI2THANG1LAN.SelectedIndex = ddlGHI2THANG1LAN.Items.IndexOf(g2t1l);
                }

                var thuho = ddlTHUHO.Items.FindByValue(value.THUHO);
                if (thuho != null)
                {
                    ddlTHUHO.SelectedIndex = ddlTHUHO.Items.IndexOf(thuho);
                }

                txtSOTK.Text = value.STK;
                txtSDT.Text = value.SDT;

                txtMALDH.Text = value.MALDH;
                //var dh = dhDao.Get(value.MADH);
                //lblMALDH.Text = dh != null ? dh.MALDH : "";
                txtMADH.Text = value.MADH;
                //cbLADHTONG.Checked = value.ISDHT.HasValue ? value.ISDHT.Value : false;
                txtNGAYHT.Text = value.NGAYHT.HasValue ? value.NGAYHT.Value.ToString("dd/MM/yyyy") : "";
                txtKYHOTRO.Text = value.KYHOTRO.HasValue ? 
                    string.Format("{0}/{1}", value.KYHOTRO.Value.ToString("MM"), value.KYHOTRO.Value.Year) : "";
               
                var item6 = ddlKICHCODH.Items.FindByValue(value.THUYLK);
                if (item6 != null)
                    ddlKICHCODH.SelectedIndex = ddlKICHCODH.Items.IndexOf(item6);

                cbKHONGTINH117.Checked = value.KHONGTINH117.HasValue && value.KHONGTINH117.Value;

                txtM3.Text = value.KLKHOAN.HasValue ? value.KLKHOAN.Value.ToString() : "4";

                cbSDInfo_INHOADON.Checked = value.SDInfo_INHOADON.HasValue && value.SDInfo_INHOADON.Value;
                if(cbSDInfo_INHOADON.Checked) {
                    txtTENKH_INHOADON.Text = value.TENKH_INHOADON;
                    //txtDIACHI_INHOADON.Text = value.DIACHI_INHOADON ;
                    txtDIACHI_INHOADON.Text = value.DIACHI_INHOADON2 != null ? value.DIACHI_INHOADON2  : "";
                }

                var item7 = ddlTT.Items.FindByValue(value.MATT);
                if (item7 != null)
                    ddlTT.SelectedIndex = ddlTT.Items.IndexOf(item7);

                txtCHISODAU.Text = value.CHISODAU.HasValue ? value.CHISODAU.Value.ToString() : "0";
                txtCHISOCUOI.Text = value.CHISOCUOI.HasValue ? value.CHISOCUOI.Value.ToString() : "0";
                txtTRUYTHU.Text = value.MTRUYTHU.HasValue ? value.MTRUYTHU.Value.ToString() : "0";

                //txtLOTRINH.Text = value.LOTRINH.HasValue ? value.LOTRINH.Value.ToString() : "";
                txtSTT.Text = value.STT.HasValue ? value.STT.Value.ToString() : "";

                var item8 = ddlMAGIA.Items.FindByValue(value.MABG);
                ddlMAGIA.SelectedIndex = (item8 != null) ? ddlHTTT.Items.IndexOf(item8) : 0;
                var item10 = ddlHOTRO.Items.FindByValue(value.MAHOTRO);
                if (item10 != null)
                    ddlHOTRO.SelectedIndex = ddlHOTRO.Items.IndexOf(item10);

            }
        }

        private KHACHHANGTAMLX KhachHangTamLX
        {
            get
            {
                if (!IsDataValid())
                    return null;

                var kh = string.IsNullOrEmpty(hdfIDKH.Value) ? new KHACHHANGTAMLX() : _khtlxDao.Get(hdfIDKH.Value);
                if (kh == null)
                    return null;

                kh.MADB = txtMADB.Text.Trim();
                kh.MADP = txtMADP.Text.ToUpper().Trim();
                kh.DUONGPHU = txtDUONGPHU.Text.Trim();
                //kh.MALKHDB = ddlLKHDB.SelectedValue;
                kh.MALKHDB = "D";
                kh.MAMDSD = ckISHONGHEO.Checked == true ? "Z" : ddlMDSD.SelectedValue;//CO HO NGHEO
                kh.MADDK = lbMADDK.Text.Trim();
                kh.SOHD = txtSOHD.Text.Trim();
                kh.MABG = ddlMAGIA.SelectedValue.Equals("NULL") ? null : ddlMAGIA.SelectedValue;
                kh.MAHOTRO = ddlHOTRO.SelectedValue;
                //kh.MAPHUONG = String.IsNullOrEmpty(ddlPHUONG.SelectedValue) ? null : ddlPHUONG.SelectedValue;
                kh.MAPHUONG = String.IsNullOrEmpty(ddlPHUONG.SelectedValue) ? null :
                    txtMADP.Text.ToUpper().Trim().Substring(1, 1);
                kh.TENKH = txtTENKH.Text.Trim();
                kh.SONHA = txtDIACHILD.Text.Trim();
                kh.SONHA2 = txtSONHA2.Text.Trim();

                kh.MST = txtMSTHUE.Text.Trim();
                kh.MAHTTT = ddlHTTT.SelectedValue;
                kh.STK = txtSOTK.Text.Trim();
                kh.SDT = txtSDT.Text.Trim();
                kh.TTSD = "";
                kh.KOPHINT = false;
                kh.THUYLK = ddlKICHCODH.SelectedValue;
                kh.NGAYGNHAP = DateTime.Now;
                kh.VAT = true;
                //kh.KHONGTINH117 = cbKHONGTINH117.Checked;
                kh.KHONGTINH117 = false;
                //kh.MATT = ddlTT.SelectedValue;
                kh.MATT = "GDH_BT";

                string kykt = "01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim();
                kh.KYKHAITHAC = DateTimeUtil.GetVietNamDate(kykt);
                kh.GHI2THANG1LAN = ddlGHI2THANG1LAN.SelectedValue;
                kh.THUHO = ddlTHUHO.SelectedValue;

                var dp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
                if (dp == null)
                    return null;

                kh.MAKV = dp.MAKV;
                //kh.MAKVDN = ddlKHUVUCDN.SelectedValue;
                kh.MAKVDN = "O";

                if (!string.IsNullOrEmpty(txtCQ.Text.Trim()))
                    kh.MACQ = txtCQ.Text.Trim();

                //if (!string.IsNullOrEmpty(txtSOHO.Text.Trim()))
                //    kh.SOHO = decimal.Parse(txtSOHO.Text.Trim());
                //dinhmuc
                kh.SOHO = int.Parse(txtSOHO.Text.Trim());
                kh.SONK = int.Parse(txtSONK.Text.Trim());
                kh.SODINHMUC = ckDINHMUCTAM.Checked == true ? int.Parse(txtSODINHMUCTAM.Text.Trim()) : int.Parse(txtSODINHMUC.Text.Trim());
                kh.ISDINHMUC = cbISDINHMUC.Checked;

                kh.ISDINHMUCTAM = ckDINHMUCTAM.Checked;
                //txtSODINHMUCTAM

                //vi pham
                kh.ISVIPH = false;

                /*string dateht = txtNGAYHT.Text.Trim();
                string ngayht = dateht.Substring(0, 2);
                string thanght = dateht.Substring(2, 2);
                string namht = dateht.Substring(4, 2);
                string dateht1 = ngayht + "/" + thanght + "/20" + namht;*/

                if (!string.IsNullOrEmpty(txtNGAYHT.Text.Trim()))
                    kh.NGAYHT = DateTimeUtil.GetVietNamDate(txtNGAYHT.Text.Trim());
                if (!string.IsNullOrEmpty(txtKYHOTRO.Text.Trim()))
                    kh.KYHOTRO = DateTimeUtil.GetVietNamDate("01/" + txtKYHOTRO.Text.Trim());
                if (!string.IsNullOrEmpty(txtMALDH.Text.Trim()))
                    kh.MALDH = txtMALDH.Text.Trim();
                if (!string.IsNullOrEmpty(txtMADH.Text.Trim()))
                    kh.MADH = txtMADH.Text.Trim();

                //if (!string.IsNullOrEmpty(txtM3.Text.Trim()))
                //    kh.KLKHOAN = int.Parse(txtM3.Text.Trim());
                kh.KLKHOAN = 4;

                if (!string.IsNullOrEmpty(txtSTT.Text.Trim()))
                    kh.STT = int.Parse(txtSTT.Text.Trim());

                kh.SDInfo_INHOADON = cbSDInfo_INHOADON.Checked;
                if (cbSDInfo_INHOADON.Checked)
                {
                    kh.TENKH_INHOADON = txtTENKH_INHOADON.Text.Trim();
                    //kh.DIACHI_INHOADON = txtDIACHI_INHOADON.Text.Trim();
                    kh.DIACHI_INHOADON2 = txtDIACHI_INHOADON.Text.Trim();
                }

                //if (!string.Empty.Equals(txtNAM.Text.Trim()))
                //    kh.NAMBDKT = int.Parse(txtNAM.Text.Trim());
                kh.NAMBDKT = DateTime.Now.Year;

                //kh.THANGBDKT = int.Parse(ddlTHANG.SelectedValue);
                kh.THANGBDKT = DateTime.Now.Month;

                //if (!string.Empty.Equals(txtCHISODAU.Text.Trim()))
                //    kh.CHISODAU = decimal.Parse(txtCHISODAU.Text.Trim());
                //kh.CHISODAU = 0;

                //so ho ngheo
                kh.ISHONGHEO = ckISHONGHEO.Checked;

                if (!string.Empty.Equals(txtCHISODAU.Text.Trim()))
                    kh.CHISODAU = decimal.Parse(txtCHISODAU.Text.Trim());
                if (!string.Empty.Equals(txtCHISOCUOI.Text.Trim()))
                    kh.CHISOCUOI = decimal.Parse(txtCHISOCUOI.Text.Trim());
                if (!string.Empty.Equals(txtTRUYTHU.Text.Trim()))
                    kh.MTRUYTHU = decimal.Parse(txtTRUYTHU.Text.Trim());

                kh.XOABOKH = false;

                kh.IDKHLX = txtIDKHLX.Text.Trim();
                kh.TIENCOCLX = Convert.ToDecimal(txtTIENCOCLX.Text.Trim());

                return kh;
            }

            set
            {
                if (value == null)
                    return;

                ClearForm();

                hdfIDKH.Value = value.IDKH;

                txtMADB.Text = value.MADB;
                txtMADP.Text = value.MADP;
                txtDUONGPHU.Text = value.DUONGPHU;
                //lblTENDUONG.Text = value.DUONGPHO.TENDP;

                var item = ddlLKHDB.Items.FindByValue(value.MALKHDB);
                if (item != null)
                    ddlLKHDB.SelectedIndex = ddlLKHDB.Items.IndexOf(item);

                var cq = cqDao.Get(value.MACQ);
                if (cq != null)
                {
                    txtCQ.Text = cq.MACQ;
                    lblTENCQ.Text = cq.TENCQ;
                }

                var item1 = ddlMDSD.Items.FindByValue(value.MAMDSD);
                if (item1 != null)
                    ddlMDSD.SelectedIndex = ddlMDSD.Items.IndexOf(item1);

                txtSOHD.Text = value.SOHD;

                var kv = kvDao.Get(value.MAKV);

                var item5 = ddlKHUVUC.Items.FindByValue(value.MAKV);
                if (item5 != null)
                    ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(item5);

                var item9 = ddlKHUVUCDN.Items.FindByValue(value.MAKVDN);
                if (item9 != null)
                    ddlKHUVUCDN.SelectedIndex = ddlKHUVUCDN.Items.IndexOf(item9);

                /**
                 * re-bind phuong
                 * 
                 */
                LoadDynamicReferences(kv);

                var item2 = ddlPHUONG.Items.FindByValue(value.MAPHUONG);
                if (item2 != null)
                    ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(item2);

                txtTENKH.Text = value.TENKH;
                txtDIACHILD.Text = value.SONHA;

                txtSOHO.Text = value.SOHO.HasValue ? value.SOHO.Value.ToString() : "1";
                txtSONK.Text = value.SONK.HasValue ? value.SONK.Value.ToString() : "1";
                cbISDINHMUC.Checked = value.ISDINHMUC.HasValue && value.ISDINHMUC.Value;

                txtMSTHUE.Text = value.MST;

                var item3 = ddlPHUONG.Items.FindByValue(value.MAPHUONG);
                if (item3 != null)
                    ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(item3);

                var item4 = ddlHTTT.Items.FindByValue(value.MAHTTT);
                if (item4 != null)
                    ddlHTTT.SelectedIndex = ddlHTTT.Items.IndexOf(item4);

                var g2t1l = ddlGHI2THANG1LAN.Items.FindByValue(value.GHI2THANG1LAN);
                if (g2t1l != null)
                {
                    ddlGHI2THANG1LAN.SelectedIndex = ddlGHI2THANG1LAN.Items.IndexOf(g2t1l);
                }

                var thuho = ddlTHUHO.Items.FindByValue(value.THUHO);
                if (thuho != null)
                {
                    ddlTHUHO.SelectedIndex = ddlTHUHO.Items.IndexOf(thuho);
                }

                txtSOTK.Text = value.STK;
                txtSDT.Text = value.SDT;

                txtMALDH.Text = value.MALDH;
                //var dh = dhDao.Get(value.MADH);
                //lblMALDH.Text = dh != null ? dh.MALDH : "";
                txtMADH.Text = value.MADH;
                //cbLADHTONG.Checked = value.ISDHT.HasValue ? value.ISDHT.Value : false;
                txtNGAYHT.Text = value.NGAYHT.HasValue ? value.NGAYHT.Value.ToString("dd/MM/yyyy") : "";
                txtKYHOTRO.Text = value.KYHOTRO.HasValue ?
                    string.Format("{0}/{1}", value.KYHOTRO.Value.ToString("MM"), value.KYHOTRO.Value.Year) : "";

                var item6 = ddlKICHCODH.Items.FindByValue(value.THUYLK);
                if (item6 != null)
                    ddlKICHCODH.SelectedIndex = ddlKICHCODH.Items.IndexOf(item6);

                cbKHONGTINH117.Checked = value.KHONGTINH117.HasValue && value.KHONGTINH117.Value;

                txtM3.Text = value.KLKHOAN.HasValue ? value.KLKHOAN.Value.ToString() : "4";

                cbSDInfo_INHOADON.Checked = value.SDInfo_INHOADON.HasValue && value.SDInfo_INHOADON.Value;
                if (cbSDInfo_INHOADON.Checked)
                {
                    txtTENKH_INHOADON.Text = value.TENKH_INHOADON;
                    //txtDIACHI_INHOADON.Text = value.DIACHI_INHOADON;
                    txtDIACHI_INHOADON.Text = value.DIACHI_INHOADON2 != null ? value.DIACHI_INHOADON2 : "";
                }

                var item7 = ddlTT.Items.FindByValue(value.MATT);
                if (item7 != null)
                    ddlTT.SelectedIndex = ddlTT.Items.IndexOf(item7);

                txtCHISODAU.Text = value.CHISODAU.HasValue ? value.CHISODAU.Value.ToString() : "0";
                txtCHISOCUOI.Text = value.CHISOCUOI.HasValue ? value.CHISOCUOI.Value.ToString() : "0";
                txtTRUYTHU.Text = value.MTRUYTHU.HasValue ? value.MTRUYTHU.Value.ToString() : "0";

                //txtLOTRINH.Text = value.LOTRINH.HasValue ? value.LOTRINH.Value.ToString() : "";
                txtSTT.Text = value.STT.HasValue ? value.STT.Value.ToString() : "";

                var item8 = ddlMAGIA.Items.FindByValue(value.MABG);
                ddlMAGIA.SelectedIndex = (item8 != null) ? ddlHTTT.Items.IndexOf(item8) : 0;
                var item10 = ddlHOTRO.Items.FindByValue(value.MAHOTRO);
                if (item10 != null)
                    ddlHOTRO.SelectedIndex = ddlHOTRO.Items.IndexOf(item10);

            }
        }

        private HOPDONG HopDong
        {
            get { return (HOPDONG) Session["KHACHHANG_HOPDONG"]; }
            set { Session["KHACHHANG_HOPDONG"] = value; }
        }

        #region loc, up
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
        #endregion

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
                Authenticate(Functions.KH_NhapMoi, Permission.Read);
                PrepareUI();
                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();  
                    BindDuongPho();
                    BindKhachHangGrid();
                }
                thangF = Convert.ToInt16(ddlTHANG.SelectedValue);
                namF = Convert.ToInt16(txtNAM.Text.Trim());
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private bool IsDataValid()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            //if (loginInfo == null) return;
            string b = loginInfo.Username;

            #region Thông tin khách hàng
            //if (!string.Empty.Equals(txtNAM.Text.Trim())) {
            //    try {
            //        DateTime.ParseExact(txtNAM.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
            //    }
            //    catch {
            //        ShowError("Năm bắt đầu kỳ khai thác không hợp lệ.", txtNAM.ClientID);
            //        return false;
            //    }
            //}

            // check độ dài mã đường phố = 3
            if (txtMADP.Text.Trim().Length != 4) {
                ShowError("Độ dài mã đường phố phải là 4.", txtMADP.ClientID);
                return false;
            }            

            // check mã đường phố có tồn tại không
            var dp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
            if (dp == null) {
                ShowError("Mã đường phố không tồn tại.", txtMADP.ClientID);
                return false;
            }

            if (_nvDao.Get(b).MAKV == "X")
            {               
                if (txtMADB.Text.Trim().Length != 0 && txtMADB.Text.Trim().Length != 6)
                {
                    ShowError("Độ dài mã danh bộ phải là 6.", txtMADB.ClientID);
                    return false;
                }
            }
            else
            {
                // check độ dài mã danh bộ = 4
                if (txtMADB.Text.Trim().Length != 0 && txtMADB.Text.Trim().Length != 4)
                {
                    ShowError("Độ dài mã danh bộ phải là 4.", txtMADB.ClientID);
                    return false;
                }
            }  

            // check độ dài mã danh bộ = 4
            if (!string.Empty.Equals(txtSTT.Text.Trim()))
            {
                try
                {
                    int.Parse(txtSTT.Text.Trim());
                }
                catch
                {
                    ShowError("Số thứ tự phải là số.", txtSTT.ClientID);
                    return false;
                }
            }

            // check tên khách hàng
            if (string.Empty.Equals(txtTENKH.Text)) {
                ShowError("Tên khách hàng không được rỗng.", txtTENKH.ClientID);
                return false;
            }

            // check tên khách hàng
            if (!string.Empty.Equals(txtCQ.Text))
            {
                var nh = cqDao.Get(txtCQ.Text.Trim());
                if (nh == null){
                    ShowError("Cơ quan thanh toán không hợp lệ.", txtCQ.ClientID);
                    return false;
                }
            }

            // check mã đồng hồ và mã đồng hồ tổng
            if (!string.IsNullOrEmpty(txtMALDH.Text.Trim()))
            {
                var ldh = ldhDao.Get(txtMALDH.Text.Trim());
                if (ldh == null)
                {
                    ShowError("Loại đồng hồ không tồn tại.", txtMALDH.ClientID);
                    return false;
                }
            }

            /*
            // check mã đồng hồ và mã đồng hồ tổng
            if (!string.IsNullOrEmpty(txtMADH.Text.Trim())) {
                var dh = dhDao.Get(txtMADH.Text.Trim());
                if (dh == null) {
                    ShowError("Mã đồng hồ không tồn tại.", txtMADH.ClientID);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(txtMADHTONG.Text.Trim())) {
                var dh = dhDao.Get(txtMADHTONG.Text.Trim());
                if (dh == null) {
                    ShowError("Mã đồng hồ tổng không tồn tại.", txtMADHTONG.ClientID);
                    return false;
                }
            }
            */

            // check số hộ là số và phải >= 1
            if (!string.IsNullOrEmpty(txtSOHO.Text.Trim())) {
                try {
                    var sh = decimal.Parse(txtSOHO.Text.Trim());
                    if (sh < 1) {
                        ShowError("Số hộ sử dụng tối thiểu là 1.", txtSOHO.ClientID);
                        return false;
                    }
                }
                catch {
                    ShowError("Số hộ sử dụng không hợp lệ.", txtSOHO.ClientID);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(txtSONK.Text.Trim()))
            {
                try
                {
                    var sh = int.Parse(txtSONK.Text.Trim());
                    if (sh < 1)
                    {
                        ShowError("Số nhân khẩu tối thiểu là 1.", txtSONK.ClientID);
                        return false;
                    }
                }
                catch
                {
                    ShowError("Số nhân khẩu không hợp lệ.", txtSONK.ClientID);
                    return false;
                }
            }

            // check khối lượng khoán tối thiểu là số
            if (!string.IsNullOrEmpty(txtM3.Text.Trim())) {
                try {
                    decimal.Parse(txtM3.Text.Trim());
                }
                catch {
                    ShowError("Khối lượng khoán tối thiểu không hợp lệ.", txtM3.ClientID);
                    return false;
                }
            }
            // check ngày hoàn thành có định dạng dd/MM/yyyy
            if (!string.IsNullOrEmpty(txtNGAYHT.Text.Trim())) {
                try {
                    DateTimeUtil.GetVietNamDate(txtNGAYHT.Text.Trim());
                }
                catch {
                    ShowError("Ngày hoàn thành không hợp lệ.", txtNGAYHT.ClientID);
                    return false;
                }
            }

            // check kỳ hỗ trợ có định dạng MM/yyyy
            if (!string.IsNullOrEmpty(txtKYHOTRO.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate("01/" + txtKYHOTRO.Text.Trim());
                }
                catch
                {
                    ShowError("Kỳ hỗ trợ không hợp lệ.", txtKYHOTRO.ClientID);
                    return false;
                }
            } 
            #endregion

            #region Xuất hóa đơn, thông số tiêu thụ, giá hợp đồng
            // check tên và địa chỉ xuất hóa đơn
            if(cbSDInfo_INHOADON.Checked) {
                if(string.IsNullOrEmpty(txtTENKH_INHOADON.Text.Trim())) {
                    ShowError("Tên xuất hóa đơn không được rỗng.", txtTENKH_INHOADON.ClientID);
                    return false;
                }
                if (string.IsNullOrEmpty(txtDIACHI_INHOADON.Text.Trim())) {
                    ShowError("Địa chỉ xuất hóa đơn không được rỗng.", txtDIACHI_INHOADON.ClientID);
                    return false;
                }
            }

            //int namm = int.Parse(txtNGAYHT.Text.Substring(6, 4));
            //if (namm <= 2010 || namm >= 2020) 
            //{
            //    ShowError("Ngày hoàn thành không dúng.", txtNGAYHT.ClientID);
            //    return false; 
            //}

            // check chỉ số đầu là số
            if (!string.IsNullOrEmpty(txtCHISODAU.Text.Trim()) ||
                !string.IsNullOrEmpty(txtCHISOCUOI.Text.Trim())) {
                decimal csd;
                decimal csc;
                try {
                    csd = decimal.Parse(txtCHISODAU.Text.Trim());
                }
                catch {
                    ShowError("Chỉ số đầu không hợp lệ.", txtCHISODAU.ClientID);
                    return false;
                }
                
                try {
                    csc = decimal.Parse(txtCHISOCUOI.Text.Trim());
                }
                catch {
                    ShowError("Chỉ số cuối không hợp lệ.", txtCHISOCUOI.ClientID);
                    return false;
                }

                if(csd > csc) {
                    ShowError("Chỉ số đầu không được lớn hơn chỉ số cuối.", txtCHISODAU.ClientID);
                    return false;
                }
            }
            #endregion
            
            return true;
        }    
        
        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_NHAPMOIKHACHHANG;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_NHAPMOIKHACHHANG;
            }

            CommonFunc.SetPropertiesForGrid(gvDuongPho);
            CommonFunc.SetPropertiesForGrid(gvDongHo);
            CommonFunc.SetPropertiesForGrid(gvDongHoSoNo);
            CommonFunc.SetPropertiesForGrid(gvHopDong);
            CommonFunc.SetPropertiesForGrid(gvCQTT);
            CommonFunc.SetPropertiesForGrid(gvKhachHang);
        }        

        private void ClearForm()
        {
            try
            {
                /*
                 * clear phần thông tin hồ sơ
                 */
                hdfIDKH.Value = "";

                ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
                txtNAM.Text = DateTime.Now.Year.ToString();
                txtSOHD.Text = "";
                txtSDT.Text = "";
                txtTENKH.Text = "";
                txtDIACHILD.Text = "";
                ddlKHUVUC.SelectedIndex = 0;
                var kv = kvDao.Get(ddlKHUVUC.SelectedValue);
                //lblKHUVUC.Text = "";
                LoadDynamicReferences(kv);
                txtMADP.Text = "";
                txtDUONGPHU.Text = "";
                txtMADB.Text = "";
                lblTENDUONG.Text = "";
                ddlHTTT.SelectedIndex = 0;
                txtSOTK.Text = "";
                txtMSTHUE.Text = "";
                txtCQ.Text = "";
                lblTENCQ.Text = "";
                txtMALDH.Text = "";
                //lblMALDH.Text = "";
                ddlKICHCODH.SelectedIndex = 0;
                txtMADH.Text = "";
                //cbLADHTONG.Checked = false;
                ddlLOAIKH.SelectedIndex = 0;
                ddlLKHDB.SelectedIndex = 0;
                ddlMDSD.SelectedIndex = 0;
                txtSOHO.Text = "1";
                txtSONK.Text = "1";
                txtSODINHMUC.Text = "1";
                cbISDINHMUC.Checked = false;
                txtM3.Text = "4";
                cbKHONGTINH117.Checked = false;
                txtNGAYHT.Text = DateTime.Now.ToString("dd/MM/yyyy");
                /*
                 * clear phần đổi thông tin hóa đơn
                 */
                cbSDInfo_INHOADON.Checked = false;
                txtTENKH_INHOADON.Text = "";
                txtDIACHI_INHOADON.Text = "";
                ddlMAGIA.SelectedIndex = 0;
                //txtLOTRINH.Text = "0";
                txtSTT.Text = "0";
                /*
                 * clear thông số tiêu thụ áp dụng cho kỳ đầu tiên 
                 */
                ddlTT.SelectedIndex = 0;
                txtCHISODAU.Text = "0";
                txtCHISOCUOI.Text = "0";

                //dinh muc
                txtSOHO.Enabled = false;
                txtSONK.Enabled = false;
                txtSODINHMUC.Enabled = false;
                ckDINHMUCTAM.Checked = false;
                txtSODINHMUCTAM.Enabled = false;
                txtSODINHMUCTAM.Text = "1";
                ckThuHo.Checked = false;
                lbVITRI.Text = "";
                txtCongXuatDHN.Text = "";

                ddlTHUHO.SelectedIndex = 0;
                ddlDOTINHD.SelectedIndex = 0;
            }
            catch { }
        }

        private void LoadStaticReferences()
        {
            UpdateMode = Mode.Create;

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            // load khu vuc
            var listKhuVuc = new KhuVucDao().GetList();
            ddlKHUVUC.Items.Clear();
            ddlKHUVUCDN.Items.Clear();

            foreach(var kv in listKhuVuc)
            {
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                ddlKHUVUCDN.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }

            var listLoaiKhachHang = new LoaiKhDacBietDao().GetList();
            ddlLKHDB.DataSource = listLoaiKhachHang;
            ddlLKHDB.DataTextField = "TENLKHDB";
            ddlLKHDB.DataValueField = "MALKHDB";
            ddlLKHDB.DataBind();

            var listMucDichSuDung = new MucDichSuDungDao().GetList();
            ddlMDSD.DataSource = listMucDichSuDung;
            ddlMDSD.DataTextField = "TENMDSD";
            ddlMDSD.DataValueField = "MAMDSD";
            ddlMDSD.DataBind();

            var listHttt = new HinhThucThanhToanDao().GetList();
            ddlHTTT.DataSource = listHttt;
            ddlHTTT.DataTextField = "MOTA";
            ddlHTTT.DataValueField = "MAHTTT";
            ddlHTTT.DataBind();

            timkv();
            ClearForm();

            var dmth = _dmthDao.GetList();
            ddlTHUHO.DataSource = dmth;
            ddlTHUHO.DataTextField = "THUHO";
            ddlTHUHO.DataValueField = "ID";
            ddlTHUHO.DataBind();

            ThuHoToDotInHD(ddlTHUHO.SelectedValue);

            //dinh muc
            txtSOHO.Enabled = false;
            txtSONK.Enabled = false;
            txtSODINHMUC.Enabled = false;

            //ho ngheo
            txtKYHOTROHN.Enabled = false;
            txtNAMHOTROHN.Enabled = false;
            txtDONVICAP.Enabled = false;
            txtMASOHN.Enabled = false;
            txtNGAPCAPHN.Enabled = false;
            txtNGAYKTHN.Enabled = false;
            txtNGAYKYSOHN.Enabled = false;
            ImageButton1.Visible = false;
            ImageButton2.Visible = false;
            ImageButton3.Visible = false;
            ddlTENXA.Enabled = false;
            txtDIACHIHN.Enabled = false;

            //thu ho
            ddlTHUHO.Enabled = false;

            txtCHISODAU.Text = "0";
            txtCHISOCUOI.Text = "0";

            if (_nvDao.Get(b).MAKV == "X" || _nvDao.Get(b).MAKV == "S")
            {
                btCTKTKHM.Visible = true;
            }

            txtTENKH_INHOADON.Enabled = false;
            txtDIACHI_INHOADON.Enabled = false;
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
                    var kvList = kvDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = kvDao.GetListKV(d);
                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }

                    //xa phuong
                    var listXAPHUONG = _xpDao.GetListKV(d);
                    ddlTENXA.DataSource = listXAPHUONG;
                    ddlTENXA.DataTextField = "TENXA";
                    ddlTENXA.DataValueField = "MAXA";
                    ddlTENXA.DataBind();
                    txtDONVICAP.Text = ddlTENXA.SelectedValue;
                    
                    var kvIn = _dihdDao.GetListKVNN(d);
                    ddlDOTINHD.Items.Clear();
                    foreach (var dotin in kvIn)
                    {
                        ddlDOTINHD.Items.Add(new ListItem(_dmdiDao.Get(dotin.MADOTIN).TENDOTIN, dotin.IDMADOTIN));
                    } 
                }
            }
        }

        private void LoadDynamicReferences(KHUVUC kv)
        {
            if (kv == null) return;
            // load phuong
            var listPhuong = pDao.GetList(kv.MAKV);
            ddlPHUONG.DataSource = listPhuong;
            ddlPHUONG.DataTextField = "TENPHUONG";
            ddlPHUONG.DataValueField = "MAPHUONG";
            ddlPHUONG.DataBind();

            var item9 = ddlKHUVUCDN.Items.FindByValue(kv.MAKV);
            if (item9 != null)
                ddlKHUVUCDN.SelectedIndex = ddlKHUVUCDN.Items.IndexOf(item9);
        }

        #region Control event handlers
        protected void ddlKHUVUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            var kv = kvDao.Get(ddlKHUVUC.SelectedValue);
            LoadDynamicReferences(kv);

            txtMADP.Text = "";
            txtDUONGPHU.Text = "";
            lblTENDUONG.Text = "";

            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var querykv = _nvDao.GetKV(b);
                //lock cap nhap chi so
                //int thang1 = DateTime.Now.Month;
                //string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                int thang1 = int.Parse(ddlTHANG.SelectedValue);
                string nam = txtNAM.Text.Trim();

                var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
                //var kynay = new DateTime(2013, 6, 1); txtMADP.Text.Trim()
                bool dung = _gcsDao.IsLockTinhCuocKy1(kynay1, ddlKHUVUC.SelectedValue, txtMADP.Text.Trim());
                
                if (dung == true) 
                {
                    CloseWaitingDialog();                    
                    ShowInfor("Đã khoá sổ ghi chỉ số.");
                    return; 
                }                

                Message msg;
                Message msgkt;

                var kh = KhachHang;
                var khtlx = KhachHangTamLX;

                if (kh == null)
                {
                   CloseWaitingDialog();
                    return;
                }

                if (UpdateMode == Mode.Create)
                {
                    if (!HasPermission(Functions.KH_NhapMoi, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    kh.IDKH = khDao.NewId();
                    hdfIDKH.Value = kh.IDKH;
                    kh.ISTACHDUONG = false;

                    kh.VITRI = txtVITRI.Text.Trim();
                    kh.VITRICU = txtVITRI.Text.Trim();

                    kh.STTTS = khDao.NewSTTTS(querykv.MAKV);//stt thoai son

                    if (querykv.MAKV == "O")
                    {
                        var tontai = khDao.ExistsMaKhachHang(kh.IDKH, txtMADP.Text.Trim(), txtMADB.Text.Trim());
                        if (tontai)
                        {
                            CloseWaitingDialog();
                            ShowError("Mã danh bộ đã tồn tại.", txtMADP.ClientID);
                            return;
                        }
                    }                    

                    if (querykv.MAKV == "O")
                    {     
                        msg = khDao.Insert(kh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                        //insert hongheo
                        string mangheo = _hnnDao.NewId();
                        if (ckISHONGHEO.Checked == true)
                        {
                            kh.MANGHEO = mangheo;

                            _rpClass.InKHHoNgheo(kh.MANGHEO.ToString(), kh.IDKH.ToString(), querykv.MAKV.ToString(), ddlTENXA.SelectedValue,
                                txtDONVICAP.Text.Trim(), txtMASOHN.Text.Trim(), DateTimeUtil.GetVietNamDate(txtNGAPCAPHN.Text.Trim()),
                                DateTimeUtil.GetVietNamDate(txtNGAYKTHN.Text.Trim()), DateTimeUtil.GetVietNamDate(txtNGAYKYSOHN.Text.Trim()),
                                DateTimeUtil.GetVietNamDate("11/" + txtKYHOTROHN.Text.Trim() + "/" + txtNAMHOTROHN.Text.Trim()), b.ToString(),
                                txtDIACHIHN.Text.Trim());
                        }

                        // dinh muC
                        if (cbISDINHMUC.Checked == true)
                        {
                            _rpClass.InDinhMuc(kh.IDKH.ToString(), Convert.ToDecimal(txtSOHO.Text.Trim()), int.Parse(txtSONK.Text.Trim()),
                                cbISDINHMUC.Checked, int.Parse(txtSODINHMUC.Text.Trim()), b.ToString(), ddlMDSD.SelectedValue,
                              txtNAM.Text.Trim() + "/" + ddlTHANG.SelectedValue + "/01" );
                        }

                        // dinh muC TAM
                        if (ckDINHMUCTAM.Checked == true)
                        {
                            _rpClass.InDinhMucTamKHTAMLX(kh.IDKH.ToString(), "", Convert.ToDecimal(txtSOHO.Text.Trim()),
                                int.Parse(txtSONK.Text.Trim()), cbISDINHMUC.Checked, int.Parse(txtSODINHMUCTAM.Text.Trim()),
                                b.ToString(), ddlMDSD.SelectedValue, 
                                txtNAM.Text.Trim() + "/" + ddlTHANG.SelectedValue + "/01",
                                "INDMTAMKH");
                        }

                        //Thu ho
                        if (ckThuHo.Checked)
                        {
                            if (ddlTHUHO.SelectedValue != "KO")
                            {
                                //querykv.MAKV    "NNTHD1"
                                var ketqua = _rpClass.UPKHTTCOBIEN(kh.IDKH, "", kh.MAKV, Convert.ToInt16(ddlTHANG.SelectedValue), Convert.ToInt16(txtNAM.Text.Trim()),
                                    "", "", "", "", "", 0, 0, 0, "UPTHUHODOTIN");

                                DataTable dt = ketqua.Tables[0];

                                if (dt.Rows[0]["KETQUA"].ToString() != "DUNG")
                                {
                                    CloseWaitingDialog();
                                    ShowError("Lỗi Save thu hộ. Kiểm tra lại.", "");
                                }
                            }                            
                        }

                    } // chau thanh
                    else
                    {
                        if (querykv.MAKV == "X")
                        {
                            khtlx.IDKH = _khtlxDao.NewId();
                            hdfIDKH.Value = khtlx.IDKH;
                            khtlx.ISTACHDUONG = false;

                            khtlx.VITRI = lbVITRI.Text.Trim();

                            msg = _khtlxDao.InsertLX(khtlx, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                            var hd = HopDong; // update hop dong dasd
                            if (hd != null && hd.SOHD == kh.SOHD)
                            {
                                bool dacap = true;
                                hdDao.UpdateM(hd, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, dacap);
                            }
                            //update dong ho su dung
                            var dasd = dhDao.Get(txtMADH.Text);
                            dhDao.UpdateDASD(dasd);

                            //update ma so thue
                            if (!string.IsNullOrEmpty(txtMSTHUE.Text.Trim()) || txtMSTHUE.Text != "")
                            {
                                _rpClass.UPKHMST(kh.IDKH, txtMSTHUE.Text.Trim(),
                                    int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), "");
                            }

                            //insert hongheo
                            string mangheo = _hnnDao.NewId();
                            if (ckISHONGHEO.Checked == true)
                            {
                                khtlx.MANGHEO = mangheo;

                                _rpClass.InKHHoNgheo(khtlx.MANGHEO.ToString(), khtlx.IDKH.ToString(), querykv.MAKV.ToString(), ddlTENXA.SelectedValue,
                                    txtDONVICAP.Text.Trim(), txtMASOHN.Text.Trim(), DateTimeUtil.GetVietNamDate(txtNGAPCAPHN.Text.Trim()),
                                    DateTimeUtil.GetVietNamDate(txtNGAYKTHN.Text.Trim()), DateTimeUtil.GetVietNamDate(txtNGAYKYSOHN.Text.Trim()),
                                    DateTimeUtil.GetVietNamDate("11/" + txtKYHOTROHN.Text.Trim() + "/" + txtNAMHOTROHN.Text.Trim()), b.ToString(),
                                    txtDIACHIHN.Text.Trim());
                            }

                            // dinh muC
                            if (cbISDINHMUC.Checked == true)
                            {
                                _rpClass.InDinhMucKHTAMLX(khtlx.IDKH.ToString(), Convert.ToDecimal(txtSOHO.Text.Trim()), int.Parse(txtSONK.Text.Trim()),
                                    cbISDINHMUC.Checked, int.Parse(txtSODINHMUC.Text.Trim()), b.ToString(), ddlMDSD.SelectedValue,
                                    txtNAM.Text.Trim() + "/" + ddlTHANG.SelectedValue + "/01");
                            }

                            // dinh muC TAM
                            if (ckDINHMUCTAM.Checked == true)
                            {
                                _rpClass.InDinhMucTamKHTAMLX(khtlx.IDKH.ToString(), "", Convert.ToDecimal(txtSOHO.Text.Trim()),
                                    int.Parse(txtSONK.Text.Trim()), cbISDINHMUC.Checked, int.Parse(txtSODINHMUCTAM.Text.Trim()), 
                                    b.ToString(), ddlMDSD.SelectedValue, 
                                    txtNAM.Text.Trim() + "/" + ddlTHANG.SelectedValue + "/01", 
                                    "INDMTAMKHTAMLX");
                            }

                            //Thu ho
                            if (ckThuHo.Checked)
                            {
                                if (ddlTHUHO.SelectedValue != "KO")
                                {
                                    //var ketqua = _rpClass.UPKHTTCOBIEN(kh.IDKH, "", kh.MAKV, Convert.ToInt16(ddlTHANG.SelectedValue), Convert.ToInt16(txtNAM.Text.Trim()),
                                    //    "", "", "", "", "", 0, 0, 0, "UPTHUHODOTIN");

                                    var ketqua = _rpClass.UPKHTTCOBIEN(khtlx.IDKH, "", khtlx.MAKV, Convert.ToInt16(ddlTHANG.SelectedValue), Convert.ToInt16(txtNAM.Text.Trim()),
                                        "", "", "", "", "", 0, 0, 0, "UPTHUHODOTINTAMLX");

                                    DataTable dt = ketqua.Tables[0];

                                    if (dt.Rows[0]["KETQUA"].ToString() != "DUNG")
                                    {
                                        CloseWaitingDialog();
                                        ShowError("Lỗi Save thu hộ. Kiểm tra lại.", "");
                                    }
                                }
                            }
    
                            ClearForm();
                            BindKhachHangGrid();

                            upnlCustomers.Update();
                            UpdateMode = Mode.Create;

                            txtSOHD.Focus();
                            CloseWaitingDialog();
                            return;                            
                        } // long xuyen
                        else
                        {
                            if (querykv.MAKV == "S")
                            {
                                khtlx.IDKH = _khtlxDao.NewId();
                                hdfIDKH.Value = khtlx.IDKH;
                                khtlx.ISTACHDUONG = false;

                                khtlx.VITRI = lbVITRI.Text.Trim();

                                msg = _khtlxDao.InsertLX(khtlx, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                                var hd = HopDong; // update hop dong dasd
                                if (hd != null && hd.SOHD == kh.SOHD)
                                {
                                    bool dacap = true;
                                    hdDao.UpdateM(hd, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, dacap);
                                }
                                //update dong ho su dung
                                var dasd = dhDao.Get(txtMADH.Text);
                                dhDao.UpdateDASD(dasd);

                                //update ma so thue
                                if (!string.IsNullOrEmpty(txtMSTHUE.Text.Trim()) || txtMSTHUE.Text != "")
                                {
                                    _rpClass.UPKHMST(kh.IDKH, txtMSTHUE.Text.Trim(),
                                        int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), "");
                                }

                                //insert hongheo
                                string mangheo = _hnnDao.NewId();
                                if (ckISHONGHEO.Checked == true)
                                {
                                    khtlx.MANGHEO = mangheo;

                                    _rpClass.InKHHoNgheo(khtlx.MANGHEO.ToString(), khtlx.IDKH.ToString(), querykv.MAKV.ToString(), ddlTENXA.SelectedValue,
                                        txtDONVICAP.Text.Trim(), txtMASOHN.Text.Trim(), DateTimeUtil.GetVietNamDate(txtNGAPCAPHN.Text.Trim()),
                                        DateTimeUtil.GetVietNamDate(txtNGAYKTHN.Text.Trim()), DateTimeUtil.GetVietNamDate(txtNGAYKYSOHN.Text.Trim()),
                                        DateTimeUtil.GetVietNamDate("11/" + txtKYHOTROHN.Text.Trim() + "/" + txtNAMHOTROHN.Text.Trim()), b.ToString(),
                                        txtDIACHIHN.Text.Trim());
                                }

                                // dinh muC
                                if (cbISDINHMUC.Checked == true)
                                {
                                    _rpClass.InDinhMucKHTAMLX(khtlx.IDKH.ToString(), Convert.ToDecimal(txtSOHO.Text.Trim()), int.Parse(txtSONK.Text.Trim()),
                                        cbISDINHMUC.Checked, int.Parse(txtSODINHMUC.Text.Trim()), b.ToString(), ddlMDSD.SelectedValue,
                                        txtNAM.Text.Trim() + "/" + ddlTHANG.SelectedValue + "/01");
                                }

                                // dinh muC TAM
                                if (ckDINHMUCTAM.Checked == true)
                                {
                                    _rpClass.InDinhMucTamKHTAMLX(khtlx.IDKH.ToString(), "", Convert.ToDecimal(txtSOHO.Text.Trim()),
                                        int.Parse(txtSONK.Text.Trim()), cbISDINHMUC.Checked, int.Parse(txtSODINHMUCTAM.Text.Trim()),
                                        b.ToString(), ddlMDSD.SelectedValue, 
                                        txtNAM.Text.Trim() + "/" + ddlTHANG.SelectedValue + "/01",
                                        "INDMTAMKHTAMLX");
                                }

                                //Thu ho
                                if (ckThuHo.Checked)
                                {
                                    if (ddlTHUHO.SelectedValue != "KO")
                                    {
                                        //var ketqua = _rpClass.UPKHTTCOBIEN(kh.IDKH, "", kh.MAKV, Convert.ToInt16(ddlTHANG.SelectedValue), Convert.ToInt16(txtNAM.Text.Trim()),
                                        //    "", "", "", "", "", 0, 0, 0, "UPTHUHODOTIN");

                                        var ketqua = _rpClass.UPKHTTCOBIEN(khtlx.IDKH, "", khtlx.MAKV, Convert.ToInt16(ddlTHANG.SelectedValue), Convert.ToInt16(txtNAM.Text.Trim()),
                                            "", "", "", "", "", 0, 0, 0, "UPTHUHODOTINTAMLX");

                                        DataTable dt = ketqua.Tables[0];

                                        if (dt.Rows[0]["KETQUA"].ToString() != "DUNG")
                                        {
                                            CloseWaitingDialog();
                                            ShowError("Lỗi Save thu hộ. Kiểm tra lại.", "");
                                        }
                                    }
                                }

                                ClearForm();
                                BindKhachHangGrid();

                                upnlCustomers.Update();
                                UpdateMode = Mode.Create;

                                txtSOHD.Focus();
                                CloseWaitingDialog();
                                return; 
                            }// chau doc
                            else
                            {
                                //if (querykv.MAKV == "N") // chau phu dua vao tam, ko co chuyen VAO KHACHHANG
                                //{
                                //    khtlx.IDKH = _khtlxDao.NewId();
                                //    hdfIDKH.Value = khtlx.IDKH;
                                //    khtlx.ISTACHDUONG = false;

                                //    msg = _khtlxDao.InsertLX(khtlx, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                                //    ClearForm();
                                //    // bind grid
                                //    BindKhachHangGrid();
                                //    upnlCustomers.Update();
                                //    UpdateMode = Mode.Create;
                                //    txtSOHD.Focus();
                                //    CloseWaitingDialog();
                                //    return;
                                //}
                                //else
                                //{                                

                                msg = khDao.InsertTS(kh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                                //insert hongheo
                                string mangheo = _hnnDao.NewId();
                                if (ckISHONGHEO.Checked == true)
                                {
                                    kh.MANGHEO = mangheo;

                                    _rpClass.InKHHoNgheo(kh.MANGHEO.ToString(), kh.IDKH.ToString(), querykv.MAKV.ToString(), ddlTENXA.SelectedValue,
                                        txtDONVICAP.Text.Trim(), txtMASOHN.Text.Trim(), DateTimeUtil.GetVietNamDate(txtNGAPCAPHN.Text.Trim()),
                                        DateTimeUtil.GetVietNamDate(txtNGAYKTHN.Text.Trim()), DateTimeUtil.GetVietNamDate(txtNGAYKYSOHN.Text.Trim()),
                                        DateTimeUtil.GetVietNamDate("11/" + txtKYHOTROHN.Text.Trim() + "/" + txtNAMHOTROHN.Text.Trim()), b.ToString(),
                                        txtDIACHIHN.Text.Trim());
                                }

                                // dinh muC
                                if (cbISDINHMUC.Checked == true)
                                {
                                    _rpClass.InDinhMuc(kh.IDKH.ToString(), Convert.ToDecimal(txtSOHO.Text.Trim()), int.Parse(txtSONK.Text.Trim()),
                                        cbISDINHMUC.Checked, int.Parse(txtSODINHMUC.Text.Trim()), b.ToString(), ddlMDSD.SelectedValue,
                                        txtNAM.Text.Trim() + "/" + ddlTHANG.SelectedValue + "/01");
                                }

                                // dinh muC TAM
                                if (ckDINHMUCTAM.Checked == true)
                                {
                                    _rpClass.InDinhMucTamKHTAMLX(kh.IDKH.ToString(), "", Convert.ToDecimal(txtSOHO.Text.Trim()),
                                        int.Parse(txtSONK.Text.Trim()), cbISDINHMUC.Checked, int.Parse(txtSODINHMUCTAM.Text.Trim()),
                                        b.ToString(), ddlMDSD.SelectedValue,
                                        txtNAM.Text.Trim() + "/" + ddlTHANG.SelectedValue  + "/01",
                                        "INDMTAMKH");
                                }

                                //Thu ho
                                if (ckThuHo.Checked)
                                {
                                    if (ddlTHUHO.SelectedValue != "KO")
                                    {
                                        //querykv.MAKV    "NNTHD1"
                                        var ketqua = _rpClass.UPKHTTCOBIEN(kh.IDKH, "", kh.MAKV, Convert.ToInt16(ddlTHANG.SelectedValue), Convert.ToInt16(txtNAM.Text.Trim()),
                                            "", "", "", "", "", 0, 0, 0, "UPTHUHODOTIN");

                                        DataTable dt = ketqua.Tables[0];

                                        if (dt.Rows[0]["KETQUA"].ToString() != "DUNG")
                                        {
                                            CloseWaitingDialog();
                                            ShowError("Lỗi Save thu hộ. Kiểm tra lại.", "");
                                        }
                                    }
                                }
                                //}//thoai son
                            }//chau phu
                        }
                    }
                    
                    var kynay = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);
                    //var kh = _khDao.GetKhachHangFromMadb(txtSODB.Text.Trim());
                    msgkt = _gcsDao.KhoiTaoGhiChiSo(kynay, kh);

                    if (querykv.MAKV == "O")
                    {
                        if (msgkt != null && msgkt.MsgType.Equals(MessageType.Error))
                        //if (msgkt != null )//&& msgkt.MsgType.Equals(MessageType.Error))
                        {
                            CloseWaitingDialog();
                            ShowError("Trùng danh số!!..", kh.IDKH);
                            return;
                        }
                        else
                        {
                            var hd = HopDong;
                            if (hd != null && hd.SOHD == kh.SOHD)
                            {
                                //hd.DACAPDB = true;
                                //hdDao.Update(hd, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);
                                bool dacap = true;
                                hdDao.UpdateM(hd, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, dacap);
                            }

                            //update dong ho su dung
                            var dasd = dhDao.Get(txtMADH.Text);
                            dhDao.UpdateDASD(dasd);
                        }
                    }
                    else
                    {
                        if (msgkt != null && msgkt.MsgType.Equals(MessageType.Error))
                        //if (msgkt != null )//&& msgkt.MsgType.Equals(MessageType.Error))
                        {
                            CloseWaitingDialog();
                            ShowError("Trùng danh số!!.........", kh.IDKH);
                            return;
                        }
                        else
                        {
                            var hd = HopDong;
                            if (hd != null && hd.SOHD == kh.SOHD)
                            {
                                //hd.DACAPDB = true;
                                //hdDao.Update(hd, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);
                                bool dacap = true;
                                hdDao.UpdateM(hd, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, dacap);
                            }

                            //update dong ho su dung
                            var dasd = dhDao.Get(txtMADH.Text);
                            dhDao.UpdateDASD(dasd);
                        }
                    }
                    
                    //update ma so thue
                    if (!string.IsNullOrEmpty(txtMSTHUE.Text.Trim()) || txtMSTHUE.Text != "")
                    {
                        _rpClass.UPKHMST(kh.IDKH, txtMSTHUE.Text.Trim(),
                            int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), "KHÁCH HÀNG MỚI GẮN MST");
                    }

                    // update hop dong
                    /*var hd = HopDong;
                    if (hd != null && hd.SOHD == kh.SOHD)
                    {
                        hd.DACAPDB = true;
                        hdDao.Update(hd, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);
                    }

                    //update dong ho su dung
                    var dasd = dhDao.Get(txtMADH.Text);
                    dhDao.UpdateDASD(dasd);
                    */

                }
                else //update save
                {
                    if (!HasPermission(Functions.KH_NhapMoi, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    kh.IDKH = hdfIDKH.Value;

                    if (querykv.MAKV == "O")
                    {
                        var tontai = khDao.ExistsAnotherMaKhachHang(kh.IDKH, txtMADP.Text.Trim(), txtMADB.Text.Trim());
                        if (tontai)
                        {
                            CloseWaitingDialog();
                            ShowError("Mã danh bộ đã tồn tại.", txtMADP.ClientID);
                            return;
                        }
                    }

                    msg = khDao.Update(kh, DateTime.Now.Month, DateTime.Now.Year, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    /*var kynay = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);
                    msgkt = _gcsDao.KhoiTaoGhiChiSo(kynay, kh);
                    if (msgkt != null && msgkt.MsgType.Equals(MessageType.Error))
                    {
                        CloseWaitingDialog();
                        ShowError("Khởi tạo chỉ số cho khách hàng mới không thành công.2", kh.IDKH);
                        return;
                    }*/
                    
                }

                CloseWaitingDialog();

                ClearForm();

                // bind grid
                BindKhachHangGrid();

                upnlCustomers.Update();
                UpdateMode = Mode.Create;

                txtSOHD.Focus();

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            UpdateMode = Mode.Create;
        }

        //protected void ddlMDSD_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //UnblockWaitingDialog();
        //    //SetControlEnable();
        //    CloseWaitingDialog();
        //}

        private void SetControlEnable()
        {
            var val = ddlMDSD.SelectedValue;

            if (val == MAMDSD.HD.GetHashCode().ToString())
            {
                ddlLKHDB.Enabled = false;
                ddlLOAIKH.Enabled = false;
                txtSOHO.Enabled = false;
                txtSONK.Enabled = false;
                cbISDINHMUC.Enabled = false;
                txtKYHOTRO.Enabled = false;
            }

            if (val == MAMDSD.KD.GetHashCode().ToString() || val == MAMDSD.CQ.GetHashCode().ToString())
            {
                ddlLKHDB.Enabled = true;
                ddlLOAIKH.Enabled = true;
                txtSOHO.Enabled = true;
                txtSONK.Enabled = true;
                cbISDINHMUC.Enabled = true;
                txtKYHOTRO.Enabled = false;
            }

            if (val == MAMDSD.HN.GetHashCode().ToString())
            {
                ddlLKHDB.Enabled = false;
                ddlLOAIKH.Enabled = false;
                txtSOHO.Enabled = true;
                txtSONK.Enabled = true;
                cbISDINHMUC.Enabled = true;
                txtKYHOTRO.Enabled = true;
            }
        }
        #endregion

        #region Khách hàng
        private void BindKhachHangGrid()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            //var list = khDao.GetListInKKT(DateTime.Now.Month, DateTime.Now.Year);
            //var list = khDao.GetListKH(DateTime.Now.Month, DateTime.Now.Year);

            //int thangF = Convert.ToInt32(ddlTHANG.SelectedValue);
            //int namF = Convert.ToInt32(txtNAM.Text.Trim());

            if (_nvDao.Get(b).MAKV == "X" || _nvDao.Get(b).MAKV == "S")
            {
                var list = _khtlxDao.GetListKHKV(thangF, namF, _nvDao.Get(b).MAKV);

                gvKhachHang.DataSource = list;
                gvKhachHang.PagerInforText = list.Count.ToString();
                gvKhachHang.DataBind();
            }
            else
            {
                if (_nvDao.Get(b).MAKV == "N") // chau phu
                {
                    //var list = _khtlxDao.GetListKHKV(DateTime.Now.Month, DateTime.Now.Year, _nvDao.Get(b).MAKV);
                    var list = khDao.GetListKHKV(thangF, namF, _nvDao.Get(b).MAKV);

                    gvKhachHang.DataSource = list;
                    gvKhachHang.PagerInforText = list.Count.ToString();
                    gvKhachHang.DataBind();
                }
                else
                {
                    var list = khDao.GetListKHKV(thangF, namF, _nvDao.Get(b).MAKV);

                    gvKhachHang.DataSource = list;
                    gvKhachHang.PagerInforText = list.Count.ToString();
                    gvKhachHang.DataBind();
                }
            }
            upnlCustomers.Update();
        }

        protected void gvKhachHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
               /* var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectHD":
                        UnblockWaitingDialog();
                        var kh = khDao.Get(id);
                        KhachHang = kh;
                        SetControlEnable();

                        UpdateMode = Mode.Update;
                        CloseWaitingDialog();
                        break;
                } */
                CloseWaitingDialog();
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
                BindKhachHangGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        } 
        #endregion

        #region Đường phố
        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            CloseWaitingDialog();
        }

        private void BindDuongPho()
        {
            var list = dpDao.GetList(ddlKHUVUC.SelectedValue, txtKeywordDP.Text.Trim());
            gvDuongPho.DataSource = list;
            gvDuongPho.PagerInforText = list.Count.ToString();
            gvDuongPho.DataBind();
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
                            SetControlValue(txtMADP.ClientID, dp.MADP);
                            SetControlValue(txtDUONGPHU.ClientID, dp.DUONGPHU);
                            
                            UpdateKhuVuc(dp);
                            upnlCustomers.Update();

                            HideDialog("divDuongPho");
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

        private void UpdateKhuVuc(DUONGPHO dp)
        {
            // update khu vuc, generate new madb, update label
            //SetControlValue(txtMADB.ClientID, khDao.NewMADB(dp.MADP));
            SetControlValue(txtSTT.ClientID, khDao.NewSTT(dp.MADP).ToString());

            lblTENDUONG.Text = dp.DOT != null ? dp.TENDP + ", phiên: " + dp.DOT : dp.TENDP;

            var kv = ddlKHUVUC.Items.FindByValue(dp.MAKV);
            if (kv != null)
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);

            //LoadDynamicReferences(dp.KHUVUC);
            txtMADB.Focus();
        }

        protected void linkBtnHidden_Click(object sender, EventArgs e)
        {
            if (txtMADP.Text.Trim() == "")
            {
                CloseWaitingDialog();
                return;
            }

            var dp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());

            if (dp != null)
            {
                UpdateKhuVuc(dp);
                CloseWaitingDialog();
            }
            else
            {
                CloseWaitingDialog();
                ShowWarning("Mã đường phố không hợp lệ");
            }
            txtMADB.Focus();
        }
        #endregion

        #region Đồng hồ
        protected void btnFilterDH_Click(object sender, EventArgs e)
        {
            BindDongHo();
            CloseWaitingDialog();
        }

        protected void btnFilterDHSONO_Click(object sender, EventArgs e)
        {
            BindDongHoSoNo();
            CloseWaitingDialog();
        }

        private void BindDongHo()
        {
            var list = ldhDao.GetList(txtKeywordDH.Text.Trim());
            gvDongHo.DataSource = list;
            gvDongHo.PagerInforText = list.Count.ToString();
            gvDongHo.DataBind();
        }

        private void BindDongHoSoNo()
        {
            var list = dhDao.GetListDASD(txtKeywordDHSONO.Text.Trim());
            gvDongHoSoNo.DataSource = list;
            gvDongHoSoNo.PagerInforText = list.Count.ToString();
            gvDongHoSoNo.DataBind();
        }

        protected void btnBrowseLOAIDH_Click(object sender, EventArgs e)
        {
            BindDongHo();
            upnlDongHo.Update();
            UnblockDialog("divDongHo");
        }

        protected void btnBrowseDHSONO_Click(object sender, EventArgs e)
        {
            BindDongHoSoNo();
            upnlDongHoSoNo.Update();
            UnblockDialog("divDongHoSoNo");
        }

        protected void gvDongHo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMALDH":
                        var ldh = ldhDao.Get(id);
                        if (ldh != null)
                        {
                            SetControlValue(txtMALDH.ClientID, ldh.MALDH);
                            
                            //SetLabel(lblMALDH.ClientID, dp.MALDH);

                            HideDialog("divDongHo");
                        }

                        break;

                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDongHo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvDongHo.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindDongHo();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDongHoSoNo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMADH":
                        var dh = dhDao.Get(id);
                        if (dh != null)
                        {
                            //SetControlValue(txtMADH.ClientID, dh.MADH);
                            txtMADH.Text = dh.MADH;
                            SetControlValue(txtMALDH.ClientID, dh.MALDH);
                            SetLabel(lblSONO.ClientID, dh.SONO);
                            //SetLabel(lblKICHCO.ClientID, ldhDao.Get(dh.MALDH).ToString());
                            var ldhkc = ldhDao.GetListldh(dh.MALDH);
                            foreach (var kc in ldhkc)
                            {
                                //string a = kc.KICHCO;
                                SetLabel(lblKICHCO.ClientID, kc.KICHCO);
                            }
                            
                            HideDialog("divDongHoSoNo");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDongHoSoNo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvDongHoSoNo.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindDongHoSoNo();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        } 
        #endregion

        #region Cơ quan thanh toán
        protected void btnBrowseCQ_Click(object sender, EventArgs e)
        {
            BindCQTT();
            upnlCQTT.Update();
            UnblockDialog("divCQTT");
        }

        protected void btnFilterCQTT_Click(object sender, EventArgs e)
        {
            BindCQTT();
            CloseWaitingDialog();
        }

        private void BindCQTT()
        {
            var list = cqDao.GetList(txtKeywordCQTT.Text.Trim());
            gvCQTT.DataSource = list;
            gvCQTT.PagerInforText = list.Count.ToString();
            gvCQTT.DataBind();
        }

        protected void gvCQTT_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvCQTT.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindCQTT();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvCQTT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMACQ":
                        var dp = cqDao.Get(id);
                        if (dp != null)
                        {
                            SetControlValue(txtCQ.ClientID, dp.MACQ);
                            SetLabel(lblTENCQ.ClientID, dp.TENCQ);

                            HideDialog("divCQTT");
                        }

                        break;

                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #endregion

        #region Hợp đồng
        private void BindHD()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                //var list = hdDao.GetList(txtKeywordHD.Text.Trim(), false);
                //var list = hdDao.GetListNN(txtKeywordHD.Text.Trim(), false);

                if (_nvDao.Get(b).MAKV == "X")
                {
                    var list = hdDao.GetListNNKV(txtKeywordHD.Text.Trim(), false, _nvDao.Get(b).MAKV);

                    gvHopDong.DataSource = list;
                    gvHopDong.PagerInforText = list.Count.ToString();
                    gvHopDong.DataBind();

                    if (list.Count != 0)
                    {
                        lbTESTKHM.Text = "";
                    }
                    else
                    {
                        if (khDao.GetMADHSONO(txtKeywordHD.Text.Trim()) != null)
                        {
                            lbTESTKHM.Text = "Khách hàng khai thác rồi.";
                        }
                        else
                        {
                            lbTESTKHM.Text = "";
                        }                        
                    }
                    upnlHopDong.Update();
                }
                else
                {
                    var list = hdDao.GetListNNKV(txtKeywordHD.Text.Trim(), false, _nvDao.Get(b).MAKV);

                    gvHopDong.DataSource = list;
                    gvHopDong.PagerInforText = list.Count.ToString();
                    gvHopDong.DataBind();
                }
            }
            catch { }
        }

        protected void btnFilterHD_Click(object sender, EventArgs e)
        {
            try
            {
                BindHD();
                upnlHopDong.Update();
                CloseWaitingDialog();
            }
            catch { }
        }

        protected void btnBrowseSOHD_Click(object sender, EventArgs e)
        {
            try
            {
                BindHD();
                upnlHopDong.Update();
                UnblockDialog("divHopDong");
            }
            catch { }
        }

        protected void gvHopDong_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvHopDong.PageIndex = e.NewPageIndex;              
                BindHD();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvHopDong_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMADDK":
                        var hd = hdDao.Get(id);
                        var ddk = _ddkDao.Get(id);
                        if (hd != null)
                        {
                            UnblockWaitingDialog();

                            if (ddk != null)
                            {
                                txtTIENCOCLX.Text = Convert.ToInt32(ddk.TIENCOCLX).ToString();
                            }
                            else
                            {
                                txtTIENCOCLX.Text = "0";
                            }

                            BindHopDongToForm(hd);
                            CloseWaitingDialog();

                            HopDong = hd;

                            txtCHISOCUOI.Text = "0";
                            upnlCustomers.Update();
                            HideDialog("divHopDong");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindHopDongToForm(HOPDONG hd)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;            

            if (hd != null)
            {
                //SetControlValue(txtSOHD.ClientID, hd.SOHD);
                txtSOHD.Text = hd.SOHD.ToString();
                lbMADDK.Text = hd.MADDK.ToString();
                SetControlValue(txtSDT.ClientID, hd.DONDANGKY != null ? hd.DONDANGKY.DIENTHOAI : "");
                SetControlValue(txtTENKH.ClientID, hd.DONDANGKY != null ? hd.DONDANGKY.TENKH : "");
                //SetControlValue(txtDIACHILD.ClientID, (_ddkDao.Get(hd.MADDK) != null || _ddkDao.Get(hd.MADDK).DIACHILD != null) ? _ddkDao.Get(hd.MADDK).SONHA : "");                                
                SetControlValue(txtMSTHUE.ClientID, hd.MST);

                lbVITRI.Text = _tkDao.Get(hd.MADDK.ToString()) != null ? (_tkDao.Get(hd.MADDK.ToString()).DUONGHEMTK != null ? _tkDao.Get(hd.MADDK.ToString()).DUONGHEMTK : "") + "," +
                    (_tkDao.Get(hd.MADDK.ToString()).DIACHITK != null ? _tkDao.Get(hd.MADDK.ToString()).DIACHITK : "") + "," + 
                    (_tkDao.Get(hd.MADDK.ToString()).VITRIDHTK != null ? _tkDao.Get(hd.MADDK.ToString()).VITRIDHTK : "")  : "";

                txtVITRI.Text = _tkDao.Get(hd.MADDK.ToString()) != null ? (_tkDao.Get(hd.MADDK.ToString()).DUONGHEMTK != null ? _tkDao.Get(hd.MADDK.ToString()).DUONGHEMTK : "") + "," +
                    (_tkDao.Get(hd.MADDK.ToString()).DIACHITK != null ? _tkDao.Get(hd.MADDK.ToString()).DIACHITK : "") + "," +
                    (_tkDao.Get(hd.MADDK.ToString()).VITRIDHTK != null ? _tkDao.Get(hd.MADDK.ToString()).VITRIDHTK : "") : "";

                if (_nvDao.Get(b).MAKV == "X")
                {
                    txtDIACHILD.Text = (_ddkDao.Get(hd.MADDK) != null || _ddkDao.Get(hd.MADDK).DIACHILD != null) ? 
                        _ddkDao.Get(hd.MADDK).DIACHILD : "";
                    txtSONHA2.Text = hd.DONDANGKY != null ? hd.DONDANGKY.SONHA2 : "";
                    txtIDKHLX.Text = _khtlxDao.NewIDKHLX();
                }
                else
                {
                    txtDIACHILD.Text = (_ddkDao.Get(hd.MADDK) != null || _ddkDao.Get(hd.MADDK).DIACHILD != null) ?
                        _ddkDao.Get(hd.MADDK).SONHA : "";                                
                    txtSONHA2.Text = hd.DONDANGKY != null ? hd.DONDANGKY.SONHA2 : "";
                }

                var thicong = _tcDao.Get(hd.MADDK);
                txtNGAYHT.Text = thicong.NGAYHT.Value.ToString("dd/MM/yyyy");
                //DateTimeUtil.GetVietNamDate(txtNGAYHT.Text.Trim());
                //SetControlValue(txtMADH.ClientID, thicong.SOSERIAL);
                txtMADH.Text = thicong.MADH;

                var dh = dhDao.Get(thicong.MADH);
                lblSONO.Text = dh.SONO;
                txtCongXuatDHN.Text = dh.CONGSUAT;        
                SetControlValue(txtMALDH.ClientID, dh.MALDH);

                //get danh bo tu hop dong
                txtMADP.Text = hd.MADP.ToString();
                txtDUONGPHU.Text = "";
                txtMADB.Text = hd.MADB.ToString();
                txtSTT.Text = khDao.NewSTT(hd.MADP).ToString();

                if (hd.KHUVUC != null)
                {

                    var item5 = ddlKHUVUC.Items.FindByValue(hd.KHUVUC.MAKV);
                    if (item5 != null)
                        ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(item5);

                    var phuong = ddlPHUONG.Items.FindByValue(hd.MAPHUONG);
                    if (phuong != null)
                        ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(phuong);                   

                    upnlCustomers.Update();
                }                

                if (hd.HTTHANHTOAN != null)
                    SetControlValue(ddlHTTT.ClientID, hd.MAHTTT);

                if (hd.CODH != null)
                    SetControlValue(ddlKICHCODH.ClientID, hd.CODH);

                if (hd.LOAIHD != null)
                    SetControlValue(ddlLOAIKH.ClientID, hd.LOAIHD);

                if (hd.MDSD != null)
                    SetControlValue(ddlMDSD.ClientID, hd.MAMDSD);

                txtSOHO.Text = "1";
                txtSONK.Text = "1";
                txtSODINHMUC.Text = "1";
                //SetControlValue(txtNGAYHT.ClientID, hd.NGAYTAO.HasValue ? hd.NGAYTAO.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy"));

                txtCHISODAU.Text = "0";
                txtCHISOCUOI.Text = "0";
                txtTRUYTHU.Text = "0";

                if (_nvDao.Get(b).MAKV == "X")
                {                    
                    txtMADP.Text = "XA01";                   
                    txtMADB.Text = "000000";                    
                }
                else
                {
                    if (hd.MADP != null)
                    {
                        txtMADP.Text = hd.MADP;
                    }
                    if (hd.MADB != null)
                    {
                        txtMADB.Text = hd.MADB;
                    }

                   // SetControlValue(txtCHISOCUOI.ClientID, Convert.ToString(String.Format("{0:#.##}", thicong.CSDAU)));
                }

                //ho ngheo 
                var isCheckedHN = hd.DONDANGKY.ISHONGHEO.HasValue && hd.DONDANGKY.ISHONGHEO.Value;
                ckISHONGHEO.Checked = isCheckedHN;
                if (isCheckedHN)
                {
                    txtKYHOTROHN.Text = ddlTHANG.SelectedValue.ToString();
                    txtNAMHOTROHN.Text = txtNAM.Text.Trim();
                    txtDONVICAP.Text = hd.DONDANGKY.DONVICAPHN != null ? hd.DONDANGKY.DONVICAPHN : "";
                    txtMASOHN.Text = hd.DONDANGKY.MAHN != null ? hd.DONDANGKY.MAHN : "";
                    txtNGAPCAPHN.Text = hd.DONDANGKY.NGAYCAPHN != null ? String.Format("{0:dd/MM/yyyy}", hd.DONDANGKY.NGAYCAPHN.Value) : "";
                    txtNGAYKTHN.Text = hd.DONDANGKY.NGAYKETTHUCHN != null ? String.Format("{0:dd/MM/yyyy}", hd.DONDANGKY.NGAYKETTHUCHN.Value) : "";
                    txtNGAYKYSOHN.Text = hd.DONDANGKY.NGAYKYHN != null ? String.Format("{0:dd/MM/yyyy}", hd.DONDANGKY.NGAYKYHN.Value) : "";

                    txtKYHOTROHN.Enabled = true;
                    txtNAMHOTROHN.Enabled = true;
                    txtDONVICAP.Enabled = true;
                    txtMASOHN.Enabled = true;
                    txtNGAPCAPHN.Enabled = true;
                    txtNGAYKTHN.Enabled = true;
                    txtNGAYKYSOHN.Enabled = true;
                    ImageButton1.Visible = true;
                    ImageButton2.Visible = true;
                    ImageButton3.Visible = true;
                }
                else
                {
                    txtKYHOTROHN.Text = "";
                    txtNAMHOTROHN.Text = "";
                    txtDONVICAP.Text = "";
                    txtMASOHN.Text = "";
                    txtNGAPCAPHN.Text = "";
                    txtNGAYKTHN.Text = "";
                    txtNGAYKYSOHN.Text = "";

                    txtKYHOTROHN.Enabled = false;
                    txtNAMHOTROHN.Enabled = false;
                    txtDONVICAP.Enabled = false;
                    txtMASOHN.Enabled = false;
                    txtNGAPCAPHN.Enabled = false;
                    txtNGAYKTHN.Enabled = false;
                    txtNGAYKYSOHN.Enabled = false;
                    ImageButton1.Visible = false;
                    ImageButton2.Visible = false;
                    ImageButton3.Visible = false;
                }                
            }
        } 
        #endregion

        //protected void txtNGAYHT_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string dateht = txtNGAYHT.Text.Trim();
        //        string ngayht = dateht.Substring(0, 2);
        //        string thanght = dateht.Substring(2, 2);
        //        string namht = dateht.Substring(4, 2);

        //        if (int.Parse(namht) <= 10 && int.Parse(namht) >= 20)
        //        {
        //            ShowWarning("Ngày lắp đặt không hợp lệ 1");
        //            return;
        //        }
        //        else
        //        {
        //            txtNGAYHT.Text = ngayht + "/" + thanght + "/20" + namht;
        //        }
        //    }
        //    catch { ShowWarning("Ngày lắp đặt không hợp lệ 2"); } 
        //}

        protected void ckISHONGHEO_CheckedChanged(object sender, EventArgs e)
        {
            if (ckISHONGHEO.Checked)
            {
                txtKYHOTROHN.Enabled = true;
                txtNAMHOTROHN.Enabled = true;
                txtDONVICAP.Enabled = true;
                txtMASOHN.Enabled = true;
                txtNGAPCAPHN.Enabled = true;
                txtNGAYKTHN.Enabled = true;
                txtNGAYKYSOHN.Enabled = true;
                ImageButton1.Visible = true;
                ImageButton2.Visible = true;
                ImageButton3.Visible = true;

                txtKYHOTROHN.Text = DateTime.Now.Month.ToString();
                txtNAMHOTROHN.Text = DateTime.Now.Year.ToString();
                ddlTENXA.Enabled = true;
                txtDIACHIHN.Enabled = true;
            }
            else
            {
                txtKYHOTROHN.Enabled = false;
                txtNAMHOTROHN.Enabled = false;
                txtDONVICAP.Enabled = false;
                txtMASOHN.Enabled = false;
                txtNGAPCAPHN.Enabled = false;
                txtNGAYKTHN.Enabled = false;
                txtNGAYKYSOHN.Enabled = false;
                ImageButton1.Visible = false;
                ImageButton2.Visible = false;
                ImageButton3.Visible = false;

                txtKYHOTROHN.Text = "";
                txtNAMHOTROHN.Text = "";
                ddlTENXA.Enabled = false;
                txtDIACHIHN.Enabled = false;
            }
        }

        protected void ddlTENXA_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.GetKV(b);

                var tenxa = _xpDao.Get(ddlTENXA.SelectedValue, query.MAKV);
                txtDONVICAP.Text = tenxa.TENXA.ToString(); ;
            }
            catch { }
        }

        protected void cbISDINHMUC_CheckedChanged(object sender, EventArgs e)
        {
            if (cbISDINHMUC.Checked)
            {
                txtSOHO.Enabled = true;
                txtSONK.Enabled = true;
                txtSODINHMUC.Enabled = true;
            }
            else
            {
                txtSOHO.Enabled = false;
                txtSONK.Enabled = false;
                txtSODINHMUC.Enabled = false;
            }
        }

        protected void btTIMKHM_Click(object sender, EventArgs e)
        {
            try
            {
                BindKhachHangGrid();

                upnlCustomers.Update();
            }
            catch { }
        }

        protected void btCTKTKHM_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                int thang1 = int.Parse(ddlTHANG.SelectedValue);
                string nam = txtNAM.Text.Trim();

                var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
                //var kynay = new DateTime(2013, 6, 1); txtMADP.Text.Trim()
                //bool dung = _gcsDao.IsLockTinhCuocKy1(kynay1, ddlKHUVUC.SelectedValue, txtMADP.Text.Trim());
                bool dung = _gcsDao.IsLockTinhCuocKy(kynay1, ddlKHUVUC.SelectedValue);
                if (dung == true)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đã khoá sổ ghi chỉ số. Chọn kỳ cho đúng!");
                    return;
                }

                Message msg, msgkt;

                _rpClass.BienKHNuoc("", _nvDao.Get(b).MAKV, "", "", thangF, namF, "INTAMTOKHMLX");

                msg = khDao.InsertTamToKHM(1, 1, _nvDao.Get(b).MAKV, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                
                var kynay = new DateTime(namF, thangF, 1);
                //var kh = _khDao.GetKhachHangFromMadb(txtSODB.Text.Trim());
                msgkt = _gcsDao.KhoiTaoGhiChiSoToKHMLX(kynay, _nvDao.Get(b).MAKV);

                ShowInfor("Khai thác khách hàng mới thành công. Kiểm tra lại.");

                CloseWaitingDialog();
                upnlCustomers.Update();
            }
            catch { }
        }

        protected void ckDINHMUCTAM_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckDINHMUCTAM.Checked)
                {
                    txtSODINHMUCTAM.Enabled = true;
                }
                else
                {
                    txtSODINHMUCTAM.Enabled = false;
                }
            }
            catch { }
        }

        protected void cbSDInfo_INHOADON_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbSDInfo_INHOADON.Checked)
                {
                    txtTENKH_INHOADON.Enabled = true;
                    txtDIACHI_INHOADON.Enabled = true;
                }
                else
                {
                    txtTENKH_INHOADON.Enabled = false;
                    txtDIACHI_INHOADON.Enabled = false;

                    txtTENKH_INHOADON.Text = "";
                    txtDIACHI_INHOADON.Text = "";
                }
            }
            catch { }
        }

        protected void ckThuHo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckThuHo.Checked)
                {
                    ddlTHUHO.Enabled = true;
                }
                else
                {
                    ddlTHUHO.Enabled = false;
                }
            }
            catch { }
        }

        protected void ddlTHUHO_SelectedIndexChanged(object sender, EventArgs e)
        {            
            ThuHoToDotInHD(ddlTHUHO.SelectedValue);

            CloseWaitingDialog();
            upnlCustomers.Update();            
        }

        private void ThuHoToDotInHD(string mathuho)
        {
            try
            {
                if (mathuho != "KO")
                {
                    string nhothud1 = "NNNTD1";
                    var dotin = _dihdDao.GetKVDot(nhothud1, ddlKHUVUC.SelectedValue);

                    var madotinnt = ddlDOTINHD.Items.FindByValue(dotin.IDMADOTIN);
                    if (madotinnt != null)
                    {
                        ddlDOTINHD.SelectedIndex = ddlDOTINHD.Items.IndexOf(madotinnt);
                    }
                }
                else
                {
                    ddlDOTINHD.SelectedIndex = 0;
                }
            }
            catch { }
        }


    }
}