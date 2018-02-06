using System;
using System.Web.UI.WebControls;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Net;

using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;


namespace EOSCRM.Web.NhanSu
{
    public partial class NVLyLich : Authentication
    {
        private readonly NhanVienDao _nvdao = new NhanVienDao();
        private readonly PhongBanDao _pbDao = new PhongBanDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly LDanTocDao _ldtDao = new LDanTocDao();
        private readonly LTonGiaoDao _ltgDao = new LTonGiaoDao();
        private readonly LTPXTDao _ltpxtDao = new LTPXTDao();
        private readonly LLOAIBCDao _llbcDao = new LLOAIBCDao();
        private readonly LCheDoHocDao _lcdhDao = new LCheDoHocDao();
        private readonly LTrinhDoDao _ltdDao = new LTrinhDoDao();
        private readonly ERPPOClass _erpClass = new ERPPOClass();
        private readonly LQTCongTacDao _lqtctDao = new LQTCongTacDao();
        private readonly LNhapNguDao _lnnDao = new LNhapNguDao();
        private readonly LSucKhoeDao _lskDao = new LSucKhoeDao();
        private readonly LDaoTaoBDDao _ldtbdDao = new LDaoTaoBDDao();
        private readonly LLoaiDaoTaoDao _lldtDao = new LLoaiDaoTaoDao();
        private readonly LKhenThuongKLDao _lktklDao = new LKhenThuongKLDao();
        private readonly LLoaiKLKTDao _llklktDao = new LLoaiKLKTDao();
        private readonly LNVLyLichDao _lnvllDao = new LNVLyLichDao();
        private readonly LQanHeGDDao _lqhgdDao = new LQanHeGDDao();
        private readonly LLoaiQHGDDao _llqhgdDao = new LLoaiQHGDDao();

        string tenbangm1, tenbangm2, duongdanm1, duongdanm2;

        #region UpdateMode, Filtered
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

        private FilteredMode Filtered
        {
            get
            {
                try
                {
                    if (Session[SessionKey.FILTEREDMODE] != null)
                    {
                        var mode = Convert.ToInt32(Session[SessionKey.FILTEREDMODE]);
                        return (mode == FilteredMode.Filtered.GetHashCode()) ? FilteredMode.Filtered : FilteredMode.None;
                    }
                    return FilteredMode.None;
                }
                catch (Exception)
                {
                    return FilteredMode.None;
                }
            }
            set
            {
                Session[SessionKey.FILTEREDMODE] = value.GetHashCode();
            }
        }
        #endregion

        #region Các bảng

        private LQUANHEGD ObjQUANHEGD
        {
            get
            {
                if (!ValiQHGD())
                    return null;

                var qhgd = (string.IsNullOrEmpty(lbMAQHGD.Text.Trim()) || lbMAQHGD.Text == "") ?
                    new LQUANHEGD() : _lqhgdDao.GetNVTD(lbMANV.Text.Trim(), lbMAQHGD.Text.Trim());
                if (qhgd == null)
                    return null;

                //con lai maqtct, manvll, manvn, ngayn, xoa
                qhgd.MALQHGD = ddlQUANHEGD.SelectedValue;
                qhgd.TEN = txtTENQHGD.Text.Trim();
                if (!txtNGAYSINHQHGD.Text.Trim().Equals(String.Empty))
                    qhgd.NGAYSINH = DateTimeUtil.GetVietNamDate(txtNGAYSINHQHGD.Text.Trim());
                qhgd.NAMSINH = txtNAMSINHQHGD.Text.Trim();

                qhgd.MADT = ddlDTQHGD.SelectedValue;
                qhgd.MATG = ddlTGQHGD.SelectedValue;
                qhgd.QUEQUAN = txtQUEQUANQHGD.Text.Trim();
                qhgd.NGHE = txtNGHENGHIEQHGD.Text.Trim();
                qhgd.DVCONGTAC = txtDVCONGTACQHGD.Text.Trim();
                qhgd.GHICHU = txtGHICHUQHGD.Text.Trim();                

                return qhgd;
            }
        }

        private LNVLYLICH ObjNVLL
        {
            get
            {
                if (!ValiNVLL())
                    return null;

                var nvll = (string.IsNullOrEmpty(lvMANVLL.Text.Trim()) || lvMANVLL.Text == "") ?
                    new LNVLYLICH() : _lnvllDao.GetMaNV(lvMANVLL.Text.Trim());
                if (nvll == null)
                    return null;

                //con lai maqtct, manvll, manvn, ngayn, xoa
                nvll.HOTENKS = txtHOTENKS.Text.Trim();
                nvll.TENTHUONGGOI = txtTENTHUONG.Text.Trim();
                nvll.TENGOIKHAC = txtTENKHAC.Text.Trim();

                if (!txtNGAYSINH.Text.Trim().Equals(String.Empty))
                    nvll.NGAYSINH = DateTimeUtil.GetVietNamDate(txtNGAYSINH.Text.Trim());
                nvll.NAMSINH = txtNAMSINH.Text.Trim();

                nvll.GIOITINH = ddlGIOITINH.SelectedValue == "NAM" ? false : true;//nu = 1 nam = 0
                nvll.NOISINH = txtNOISINH.Text.Trim();    
                nvll.QUEQUAN = txtQUEQUAN.Text.Trim();
                nvll.NOIO = txtNOIO.Text.Trim();
                nvll.MADT = ddlDANTOC.SelectedValue;
                nvll.MATG = ddlTONGIAO.SelectedValue;
                nvll.MATPXT = ddlTPXT.Text.Trim();
                nvll.NGHETRUOCTD = txtNGHETRUOCTD.Text.Trim();
                nvll.SOTRUONGCT = txtSOTRUONGCT.Text.Trim();

                return nvll;
            }
        }

        private LKHENTKL ObjKhenTKL
        {
            get
            {
                if (!ValiKhenTKL())
                    return null;

                var ktkl = (string.IsNullOrEmpty(lbMAKTKL.Text.Trim()) || lbMAKTKL.Text == "") ?
                    new LKHENTKL() : _lktklDao.GetNVTD(lbMANV.Text.Trim(), lbMAKTKL.Text.Trim());
                if (ktkl == null)
                    return null;

                //con lai maqtct, manvll, manvn, ngayn, xoa
                ktkl.MALKLKT = ddlKHENTHUONGKL.SelectedValue;
                if (!txtTUNGAYKTKL.Text.Trim().Equals(String.Empty))
                    ktkl.TUNGAY = DateTimeUtil.GetVietNamDate(txtTUNGAYKTKL.Text.Trim());
                if (!txtDENGAYKTKL.Text.Trim().Equals(String.Empty))
                    ktkl.DENNGAY = DateTimeUtil.GetVietNamDate(txtDENGAYKTKL.Text.Trim());

                ktkl.SOQD= txtSOQUYETDINH.Text.Trim();
                ktkl.NOIDUNG = txtNDKTKL.Text.Trim();

                return ktkl;
            }
        }

        private LTRINHDO ObjTrinhDo
        {
            get
            {
                if (!ValiTrinhDo())
                    return null;

                var trdo = (string.IsNullOrEmpty(lbMATD.Text.Trim()) || lbMATD.Text == "") ?
                    new LTRINHDO() : _ltdDao.GetNVTD(lbMANV.Text.Trim(), lbMATD.Text.Trim());
                if (trdo == null)
                    return null;

                //con lai manv, matd, manvn, ngayn, ngayup, xoa
                trdo.MALOAIBC = ddlLOAIBANG.SelectedValue;
                trdo.CHUYENNGANH = txtCHUYENNGANH.Text.Trim();
                if (!txtNGAYCAPBANG.Text.Trim().Equals(String.Empty))
                    trdo.NGAYCAP = DateTimeUtil.GetVietNamDate(txtNGAYCAPBANG.Text.Trim());
                trdo.TENTRUONG = txtTENTRUONG.Text.Trim();
                trdo.MACHEDOHOC = ddlCHEDOHOC.SelectedValue;

                if (UpBANGM1.PostedFile.ContentLength > 0)
                {
                    if (!TestTenBang())
                        return null;

                    tenbangm1 = Path.GetFileName(UpBANGM1.PostedFile.FileName);
                    duongdanm1 = "~/UpLoadFile/bangcap/" + tenbangm1;
                    trdo.HINHBC1 = duongdanm1;
                }
                if (UpBANGM2.PostedFile.ContentLength > 0)
                {
                    if (!TestTenBang())
                        return null;

                    tenbangm2 = Path.GetFileName(UpBANGM2.PostedFile.FileName);
                    duongdanm2 = "~/UpLoadFile/bangcap/" + tenbangm2;
                    trdo.HINHBC2 = duongdanm2;
                }                
                

                return trdo;
            }
        }

        private LQTCONGTAC ObjCongTac
        {
            get
            {
                if (!ValiCongTac())
                    return null;

                var congtac = (string.IsNullOrEmpty(lbMAQTCT.Text.Trim()) || lbMAQTCT.Text == "") ?
                    new LQTCONGTAC() : _lqtctDao.GetNVTD(lbMANV.Text.Trim(), lbMAQTCT.Text.Trim());
                if (congtac == null)
                    return null;

                //con lai maqtct, manvll, manvn, ngayn, xoa
                if (!txtNGAYBDCT.Text.Trim().Equals(String.Empty))
                    congtac.NGAYBDCT = DateTimeUtil.GetVietNamDate( "11/" + txtNGAYBDCT.Text.Trim());
                if (!txtNGAYKTCT.Text.Trim().Equals(String.Empty))
                    congtac.NGAYKTCT = DateTimeUtil.GetVietNamDate("11/" + txtNGAYKTCT.Text.Trim());
                congtac.NOIDUNG = txtNOIDUNGCT.Text.Trim();                


                return congtac;
            }
        }

        private LNHAPNGU ObjNhapNgu
        {
            get
            {
                if (!ValiNhapNgu())
                    return null;

                var nhapngu = (string.IsNullOrEmpty(lbMANNGU.Text.Trim()) || lbMANNGU.Text == "") ?
                    new LNHAPNGU() : _lnnDao.GetNVTD(lbMANV.Text.Trim(), lbMANNGU.Text.Trim());
                if (nhapngu == null)
                    return null;

                //con lai maqtct, manvll, manvn, ngayn, xoa
                if (!txtNGAYBDNN.Text.Trim().Equals(String.Empty))
                    nhapngu.NGAYBDNN = DateTimeUtil.GetVietNamDate(txtNGAYBDNN.Text.Trim());
                if (!txtNGAYKTNN.Text.Trim().Equals(String.Empty))
                    nhapngu.NGAYKTNN = DateTimeUtil.GetVietNamDate(txtNGAYKTNN.Text.Trim());
                nhapngu.QUANHAM = txtQUANHAM.Text.Trim();
                nhapngu.NOIDUNG = txtNOIDUNGNHAPNGU.Text.Trim();

                return nhapngu;
            }
        }

        private LSUCKHOE ObjSucKhoe
        {
            get
            {
                //if (!ValidateData())
                //    return null;

                var suckhoe = (string.IsNullOrEmpty(lbMASUCKHOE.Text.Trim()) || lbMASUCKHOE.Text == "") ?
                    new LSUCKHOE() : _lskDao.GetNVTD(lbMANV.Text.Trim(), lbMASUCKHOE.Text.Trim());
                if (suckhoe == null)
                    return null;

                //con lai maqtct, manvll, manvn, ngayn, xoa
                suckhoe.TINHTRANG = txtTINHTRANGSK.Text.Trim();
                suckhoe.BANHMANTINH = txtBENHMANTINH.Text.Trim();
                suckhoe.CHIEUCAO = Convert.ToDecimal(txtCHIEUCAO.Text.Trim());
                suckhoe.CANNANG = Convert.ToDecimal(txtCANNANG.Text.Trim());

                return suckhoe;
            }
        }

        private LDAOTAOBD ObjDaoTaoBD
        {
            get
            {
                if (!ValiDaoTaoBD())
                    return null;

                var dtbd = (string.IsNullOrEmpty(lbMADTBD.Text.Trim()) || lbMADTBD.Text == "") ?
                    new LDAOTAOBD() : _ldtbdDao.GetNVTD(lbMANV.Text.Trim(), lbMADTBD.Text.Trim());
                if (dtbd == null)
                    return null;

                //con lai maqtct, manvll, manvn, ngayn, xoa
                dtbd.MALOAIDTBD = ddlLOAIDTBD.SelectedValue;
                dtbd.MALOAIBC = ddlBCDTBD.SelectedValue;
                dtbd.CHUYENNGANH = txtCNDTBD.Text.Trim();

                if (!txtTGHOC.Text.Trim().Equals(String.Empty))
                    dtbd.NGAYBD = DateTimeUtil.GetVietNamDate(txtTGHOC.Text.Trim());
                if (!txtTGKTHOC.Text.Trim().Equals(String.Empty))
                    dtbd.NGAYKT = DateTimeUtil.GetVietNamDate(txtTGKTHOC.Text.Trim());                

                dtbd.MACHEDOHOC = ddlLOAIBANGDTBD.SelectedValue;
                dtbd.TENTRUONG = txtTENTRUONGDTBD.Text.Trim();
                dtbd.DCDAOTAOBD = txtDIACHIDTBD.Text.Trim();
                dtbd.NOIDUNGDTBD = txtNOIDUNGDTBD.Text.Trim();
                

                return dtbd;
            }
        }

        #endregion

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((ERPPO)Page.Master).SetControlValue(id, value);
        }

        private void ShowError(string message, string controlId)
        {
            ((ERPPO)Page.Master).ShowError(message, controlId);
        }

        private void ShowError(string message)
        {
            ((ERPPO)Page.Master).ShowError(message);
        }

        private void ShowInfor(string message)
        {
            ((ERPPO)Page.Master).ShowInfor(message);
        }

        private void HideDialog(string divId)
        {
            ((ERPPO)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((ERPPO)Page.Master).UnblockDialog(divId);
        }

        private void CloseWaitingDialog()
        {
            ((ERPPO)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.NV_NVLyLich, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    // Bind data for grid view
                    LoadStaticReferences();
                    //BindDataForGrid();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        #region ValiData

        private bool ValiQHGD()
        {
            if (string.Empty.Equals(txtTENQHGD.Text.Trim()))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Họ tên người có quan hệ."), txtTENQHGD.ClientID);
                return false;
            }

            /*if (!string.Empty.Equals(txtNGAYSINH.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYSINH.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Từ khen thưởng"), txtNGAYSINH.ClientID);
                    return false;
                }
            }*/

            return true;
        }

        private bool ValiNVLL()
        {            
            if (string.Empty.Equals(txtHOTENKS.Text.Trim()))
            {                
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Lý lịch nhân viên đã có."), txtNGAYSINH.ClientID);
                    return false;              
            }

            /*if (!string.Empty.Equals(txtNGAYSINH.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYSINH.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Từ khen thưởng"), txtNGAYSINH.ClientID);
                    return false;
                }
            }*/

            return true;
        }

        private bool ValiKhenTKL()
        {

            if (!string.Empty.Equals(txtTUNGAYKTKL.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtTUNGAYKTKL.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Từ khen thưởng"), txtTUNGAYKTKL.ClientID);
                    return false;
                }
            }            

            return true;
        }

        private bool ValiTrinhDo() 
        {
            if (!string.Empty.Equals(txtNGAYCAPBANG.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYCAPBANG.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày cấp bằng"), txtNGAYCAPBANG.ClientID);
                    return false;
                }
            }

            return true;
        }

        private bool ValiCongTac() 
        {

            if (!string.Empty.Equals(txtNGAYBDCT.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate("11/" + txtNGAYBDCT.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày BĐ công tác"), txtNGAYBDCT.ClientID);
                    return false;
                }
            }

            if (!string.Empty.Equals(txtNGAYKTCT.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate("11/" +txtNGAYKTCT.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày KT công tác"), txtNGAYKTCT.ClientID);
                    return false;
                }
            }

            return true;
        }

        private bool ValiNhapNgu()  
        {

            if (!string.Empty.Equals(txtNGAYBDNN.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYBDNN.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày BĐ nhập ngũ"), txtNGAYBDNN.ClientID);
                    return false;
                }
            }

            if (!string.Empty.Equals(txtNGAYKTNN.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYKTNN.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày KT nhập ngũ"), txtNGAYKTNN.ClientID);
                    return false;
                }
            }

            return true;
        }       

        private bool ValiDaoTaoBD()
        {

            //txtTGHOC
            if (!string.Empty.Equals(txtTGHOC.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtTGHOC.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày bắt đầu học ĐT - BD"), txtTGHOC.ClientID);
                    return false;
                }
            }                        

            return true;
        }
        #endregion

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_NS_LYLICHNV;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_NHANSU;
                header.TitlePage = Resources.Message.PAGE_NS_LYLICHNV;
            }

            CommonFunc.SetPropertiesForGrid(gvNhanVien);
            CommonFunc.SetPropertiesForGrid(gvTrinhDo);
            CommonFunc.SetPropertiesForGrid(gvQuaTrinhCT);
            CommonFunc.SetPropertiesForGrid(gvQuanDoi);
            CommonFunc.SetPropertiesForGrid(gvSucKhoe);
            CommonFunc.SetPropertiesForGrid(gvDaoTaoBD);
            CommonFunc.SetPropertiesForGrid(gvKhenThuongKL);
            CommonFunc.SetPropertiesForGrid(gvGiaDinh);
            CommonFunc.SetPropertiesForGrid(gvLyLich);            
        }

        private void LoadStaticReferences()
        {  
            var dantoc = _ldtDao.GetList();
            ddlDANTOC.Items.Clear();
            foreach (var pb in dantoc)
            {
                ddlDANTOC.Items.Add(new ListItem(pb.TENDT, pb.MADT));
            }

            var tongiao = _ltgDao.GetList();
            ddlTONGIAO.Items.Clear();
            foreach (var pb in tongiao)
            {
                ddlTONGIAO.Items.Add(new ListItem(pb.TENTG, pb.MATG));
            }

            var tpxt = _ltpxtDao.GetList();
            ddlTPXT.Items.Clear();
            foreach (var pb in tpxt)
            {
                ddlTPXT.Items.Add(new ListItem(pb.TENTPXT, pb.MATPXT));
            }

            var loaibc = _llbcDao.GetList();
            ddlLOAIBANG.Items.Clear();
            foreach (var pb in loaibc)
            {
                ddlLOAIBANG.Items.Add(new ListItem(pb.TENLOAIBC, pb.MALOAIBC));
            }

            var loaibcdtbd = _llbcDao.GetList();
            ddlBCDTBD.Items.Clear();
            foreach (var pb in loaibcdtbd)
            {
                ddlBCDTBD.Items.Add(new ListItem(pb.TENLOAIBC, pb.MALOAIBC));
            }

            var chedohoc = _lcdhDao.GetList();
            ddlCHEDOHOC.Items.Clear();
            foreach (var pb in chedohoc)
            {
                ddlCHEDOHOC.Items.Add(new ListItem(pb.TENCDHOC, pb.MACHEDOHOC));
            }
            //dao tao-boi duong
            var loaidtbd = _lldtDao.GetList();
            ddlLOAIDTBD.Items.Clear();
            foreach (var pb in loaidtbd)
            {
                ddlLOAIDTBD.Items.Add(new ListItem(pb.TENLOAI, pb.MALOAIDTBD));
            }
            //ddlLOAIBANGDTBD
            var chedohocdt = _lcdhDao.GetList();
            ddlLOAIBANGDTBD.Items.Clear();
            foreach (var pb in chedohocdt)
            {
                ddlLOAIBANGDTBD.Items.Add(new ListItem(pb.TENCDHOC, pb.MACHEDOHOC));
            }

            var loaiktkl = _llklktDao.GetList();
            ddlKHENTHUONGKL.Items.Clear();
            foreach (var pb in loaiktkl)
            {
                ddlKHENTHUONGKL.Items.Add(new ListItem(pb.TENLOAI, pb.MALKLKT));
            }

            var moiquanhe = _llqhgdDao.GetList();
            ddlQUANHEGD.Items.Clear();
            foreach (var pb in moiquanhe)
            {
                ddlQUANHEGD.Items.Add(new ListItem(pb.TENQHGD, pb.MALQHGD));
            }

            var dantocm = _ldtDao.GetList();
            ddlDTQHGD.Items.Clear();
            foreach (var pb in dantocm)
            {
                ddlDTQHGD.Items.Add(new ListItem(pb.TENDT, pb.MADT));
            }

            var tongiaom = _ltgDao.GetList();
            ddlTGQHGD.Items.Clear();
            foreach (var pb in tongiaom)
            {
                ddlTGQHGD.Items.Add(new ListItem(pb.TENTG, pb.MATG));
            }
      
        }

        #region Clear form

        private void ClearQuanHeGD()
        {
            UpdateMode = Mode.Create;

            ddlQUANHEGD.SelectedIndex = 0;
            txtTENQHGD.Text = "";
            txtNGAYSINHQHGD.Text = "";
            txtNAMSINHQHGD.Text = "";
            ddlDTQHGD.SelectedIndex = 0;
            ddlTGQHGD.SelectedIndex = 0;
            txtQUEQUANQHGD.Text = "";
            txtNGHENGHIEQHGD.Text = "";
            txtDVCONGTACQHGD.Text = "";
            txtGHICHUQHGD.Text = "";
            lbMAQHGD.Text = "";
        }

        private void ClearNVLyLich()
        {
            UpdateMode = Mode.Create;            
           
            txtHOTENKS.Text = "";
            txtTENTHUONG.Text = "";
            txtTENKHAC.Text = "";
            txtNGAYSINH.Text = "";
            txtNAMSINH.Text = "";
            ddlGIOITINH.SelectedIndex = 0;
            txtNOISINH.Text = "";
            txtQUEQUAN.Text = "";
            txtNOIO.Text = "";
            ddlDANTOC.SelectedIndex = 0;
            ddlTONGIAO.SelectedIndex = 0;
            ddlTPXT.SelectedIndex = 0;
            txtNGHETRUOCTD.Text = "";
            txtSOTRUONGCT.Text = "";
            lvMANVLL.Text = "";
        }

        private void ClearKhenTKL()
        {
            UpdateMode = Mode.Create;            

            ddlKHENTHUONGKL.SelectedIndex = 0;
            txtTUNGAYKTKL.Text = "";
            txtDENGAYKTKL.Text = "";
            txtSOQUYETDINH.Text = "";
            txtNDKTKL.Text = "";
            lbMAKTKL.Text = "";
        }

        private void ClearDaoTaoBD()
        {
            UpdateMode = Mode.Create;

            ddlLOAIDTBD.SelectedIndex = 0;
            ddlBCDTBD.SelectedIndex = 0;
            txtCNDTBD.Text = "";
            txtTGHOC.Text = "";
            txtTGKTHOC.Text = "";
            ddlLOAIBANGDTBD.SelectedIndex = 0;
            txtTENTRUONGDTBD.Text = "";
            txtDIACHIDTBD.Text = "";
            txtNOIDUNGDTBD.Text = "";
            lbMADTBD.Text = "";
        }

        private void Clear()
        {
            UpdateMode = Mode.Create;

            lbMANV.Text = "";
            lbTENKV.Text = "";
            lbTENPHONGBAN.Text = "";
        }

        private void ClearTrinhDo()
        {
            UpdateMode = Mode.Create;

            ddlLOAIBANG.SelectedIndex = 0;
            txtCHUYENNGANH.Text = "";
            txtNGAYCAPBANG.Text = "";
            txtTENTRUONG.Text = "";
            ddlCHEDOHOC.SelectedIndex = 0;
            imgBANGM1.ImageUrl = "";
            imgBANGM1.Visible = false;
            imgBANGM2.ImageUrl = "";
            imgBANGM2.Visible = false;
            lbMATD.Text = "";
        }

        private void ClearCongTac()
        {
            UpdateMode = Mode.Create;

            txtNGAYBDCT.Text = "";
            txtNGAYKTCT.Text = "";
            txtNOIDUNGCT.Text = "";
            lbMAQTCT.Text = "";
        }

        private void ClearNhapNgu()
        {
            UpdateMode = Mode.Create;

            txtNGAYBDNN.Text = "";
            txtNGAYKTNN.Text = "";
            txtQUANHAM.Text = "";
            txtNOIDUNGNHAPNGU.Text = "";
            lbMANNGU.Text = "";
        }

        private void ClearSucKhoe()
        {
            UpdateMode = Mode.Create;

            txtTINHTRANGSK.Text = "";
            txtBENHMANTINH.Text = "";
            txtCHIEUCAO.Text = "";
            txtCANNANG.Text = "";
            lbMASUCKHOE.Text = "";
        }

        #endregion

        /*public void timkv()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvdao.GetListKV(b);
            foreach (var a in query)
            {
                string d = a.MAKV;

                if (a.MAKV == "99")
                {
                    var kvList = _kvDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
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
        }*/        

        protected void ckNAMSINH_CheckedChanged(object sender, EventArgs e)
        {
            if (ckNAMSINH.Checked)
            {
                txtNAMSINH.Visible = true;
                txtNGAYSINH.Visible = false;
            }
            else
            {
                txtNAMSINH.Visible = false;
                txtNGAYSINH.Visible = true;
            }
        }

        protected void ckNAMSINHQHGD_CheckedChanged(object sender, EventArgs e)
        {
            if (ckNAMSINHQHGD.Checked)
            {
                txtNAMSINHQHGD.Visible = true;
                txtNGAYSINHQHGD.Visible = false;
            }
            else
            {
                txtNAMSINHQHGD.Visible = false;
                txtNGAYSINHQHGD.Visible = true;
            }
        }

        protected void btnFilterNV_Click(object sender, EventArgs e)
        {
            BindNhanVien();
            CloseWaitingDialog();
        }        

        protected void btnBrowseNV_Click(object sender, EventArgs e)
        {
            //BindNhanVien();       
            upnlNhanVien.Update();
            UnblockDialog("divNhanVien"); 
            CloseWaitingDialog();
        }

        #region Bind

        private void BindGiaDinh()
        {
            var list = _lqhgdDao.GetListNV(lbMANV.Text.Trim());
            gvGiaDinh.DataSource = list;
            gvGiaDinh.PagerInforText = list.Count.ToString();
            gvGiaDinh.DataBind();

            upGiaDinh.Update();
        }

        private void BindNVLyLich()
        {
            var list = _lnvllDao.GetListNV(lbMANV.Text.Trim());
            gvLyLich.DataSource = list;
            gvLyLich.PagerInforText = list.Count.ToString();
            gvLyLich.DataBind();

            upLyLich.Update();
        }

        private void BindNhanVien()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var kv = _nvdao.Get(b);

            if (kv.MAKV.Equals("99"))
            {
                var list = _nvdao.Search(txtKeywordNV.Text.Trim());
                gvNhanVien.DataSource = list;
                gvNhanVien.PagerInforText = list.Count.ToString();
                gvNhanVien.DataBind();
            }
            else
            {
                var list = _nvdao.SearchKV1(txtKeywordNV.Text.Trim(), kv.MAKV);
                gvNhanVien.DataSource = list;
                gvNhanVien.PagerInforText = list.Count.ToString();
                gvNhanVien.DataBind();
            }
        }        

        private void BindKhenThuongKL()
        {
            var list = _lktklDao.GetListNV(lbMANV.Text.Trim());
            gvKhenThuongKL.DataSource = list;
            gvKhenThuongKL.PagerInforText = list.Count.ToString();
            gvKhenThuongKL.DataBind();

            upKhenThuongKL.Update();
        }

        private void BindDaoTaoBD()
        {
            var list = _ldtbdDao.GetListNV(lbMANV.Text.Trim());
            gvDaoTaoBD.DataSource = list;
            gvDaoTaoBD.PagerInforText = list.Count.ToString();
            gvDaoTaoBD.DataBind();

            upDaoTaoBD.Update();
        }

        private void BindTrinhDo()
        {
            var list = _ltdDao.GetListNV(lbMANV.Text.Trim());
            gvTrinhDo.DataSource = list;
            gvTrinhDo.PagerInforText = list.Count.ToString();
            gvTrinhDo.DataBind();

            upTrinhDo.Update();
        }

        private void BindCongTac()
        {
            var list = _lqtctDao.GetListNV(lbMANV.Text.Trim());
            gvQuaTrinhCT.DataSource = list;
            gvQuaTrinhCT.PagerInforText = list.Count.ToString();
            gvQuaTrinhCT.DataBind();

            upQuaTrinhCT.Update();
        }

        private void BindNhapNgu()
        {
            var list = _lnnDao.GetListNV(lbMANV.Text.Trim());
            gvQuanDoi.DataSource = list;
            gvQuanDoi.PagerInforText = list.Count.ToString();
            gvQuanDoi.DataBind();

            upQuanDoi.Update();
        }

        private void BindSucKhoe()
        {
            var list = _lskDao.GetListNV(lbMANV.Text.Trim());
            gvSucKhoe.DataSource = list;
            gvSucKhoe.PagerInforText = list.Count.ToString();
            gvSucKhoe.DataBind();

            upSucKhoe.Update();
        }
#endregion

        #region Binddata

        private void BindQuanHeGDData(LQUANHEGD obj)
        {
            try
            {
                lbMAQHGD.Text = obj.MAQHGD;

                var lqhgd = ddlQUANHEGD.Items.FindByValue(obj.MALQHGD);
                if (lqhgd != null)
                    ddlQUANHEGD.SelectedIndex = ddlQUANHEGD.Items.IndexOf(lqhgd);

                txtTENQHGD.Text = obj.TEN;
                txtNGAYSINHQHGD.Text = obj.NGAYSINH != null ? String.Format("{0:dd/MM/yyyy}", obj.NGAYSINH.Value) : "";
                txtNAMSINHQHGD.Visible = true;
                txtNAMSINHQHGD.Text = obj.NAMSINH;
                ddlDTQHGD.SelectedIndex = 0;
                ddlTGQHGD.SelectedIndex = 0;
                txtQUEQUANQHGD.Text = obj.QUEQUAN;
                txtNGHENGHIEQHGD.Text = obj.NGHE;
                txtDVCONGTACQHGD.Text = obj.DVCONGTAC;
                txtGHICHUQHGD.Text = obj.GHICHU;

                upinfoGiaDinh.Update();
            }
            catch { }
        }

        private void BindNVLyLichData(LNVLYLICH obj)
        {
            try
            {
                lvMANVLL.Text = obj.MANVLL;              
                
                txtHOTENKS.Text = obj.HOTENKS;
                txtTENTHUONG.Text = obj.TENTHUONGGOI;
                txtTENKHAC.Text = obj.TENGOIKHAC;
                txtNGAYSINH.Text = (obj.NGAYSINH != null) ? String.Format("{0:dd/MM/yyyy}", obj.NGAYSINH.Value) : "";
                txtNAMSINH.Visible = true;
                txtNAMSINH.Text = obj.NAMSINH != null ? obj.NAMSINH : "";                
                if (obj.GIOITINH.Equals(false))
                    ddlGIOITINH.SelectedIndex = 0;
                else
                    ddlGIOITINH.SelectedIndex = 1;
                txtNOISINH.Text = obj.NOISINH;
                txtQUEQUAN.Text = obj.QUEQUAN;
                txtNOIO.Text = obj.NOIO;

                var dt = ddlDANTOC.Items.FindByValue(obj.MADT);
                if (dt != null)
                    ddlDANTOC.SelectedIndex = ddlDANTOC.Items.IndexOf(dt);

                var tg = ddlTONGIAO.Items.FindByValue(obj.MATG);
                if (tg != null)
                    ddlTONGIAO.SelectedIndex = ddlTONGIAO.Items.IndexOf(tg);

                var xt = ddlTPXT.Items.FindByValue(obj.MATPXT);
                if (xt != null)
                    ddlTPXT.SelectedIndex = ddlTPXT.Items.IndexOf(xt);

                txtNGHETRUOCTD.Text = obj.NGHETRUOCTD;
                txtSOTRUONGCT.Text = obj.SOTRUONGCT;

                upinfoLyLich.Update();
            }
            catch { }
        }

        private void BindKhenTKLData(LKHENTKL obj)
        {
            try
            {
                lbMAKTKL.Text = obj.MAKTKL;

                var lklkt = ddlKHENTHUONGKL.Items.FindByValue(obj.MALKLKT);
                if (lklkt != null)
                    ddlKHENTHUONGKL.SelectedIndex = ddlKHENTHUONGKL.Items.IndexOf(lklkt);

                txtTUNGAYKTKL.Text = obj.TUNGAY != null ? String.Format("{0:dd/MM/yyyy}", obj.TUNGAY.Value) : "";
                txtDENGAYKTKL.Text = obj.DENNGAY != null ? String.Format("{0:dd/MM/yyyy}", obj.DENNGAY.Value) : "";
                txtSOQUYETDINH.Text = obj.SOQD;
                txtNDKTKL.Text = obj.NOIDUNG;

                upinfoKhenThuongKL.Update();
            }
            catch { }
        }

        private void BindDaoTaoBDData(LDAOTAOBD obj)
        {
            try
            {
                lbMADTBD.Text = obj.MADTBD;

                var ldtbd = ddlLOAIDTBD.Items.FindByValue(obj.MALOAIDTBD);
                if (ldtbd != null)
                    ddlLOAIDTBD.SelectedIndex = ddlLOAIDTBD.Items.IndexOf(ldtbd);

                var ldb = ddlLOAIDTBD.Items.FindByValue(obj.MALOAIBC);
                if (ldb != null)
                    ddlBCDTBD.SelectedIndex = ddlBCDTBD.Items.IndexOf(ldb);                

                txtCNDTBD.Text = obj.CHUYENNGANH;
                txtTGHOC.Text = obj.NGAYBD != null ? String.Format("{0:dd/MM/yyyy}", obj.NGAYBD.Value) : "";
                txtTGKTHOC.Text = obj.NGAYKT != null ? String.Format("{0:dd/MM/yyyy}", obj.NGAYKT.Value) : "";

                var cdh = ddlLOAIDTBD.Items.FindByValue(obj.MACHEDOHOC);
                if (cdh != null)
                    ddlLOAIBANGDTBD.SelectedIndex = ddlLOAIBANGDTBD.Items.IndexOf(cdh);                 

                txtTENTRUONGDTBD.Text = obj.TENTRUONG;
                txtDIACHIDTBD.Text = obj.DCDAOTAOBD;
                txtNOIDUNGDTBD.Text = obj.NOIDUNGDTBD;
                
                upinfoDaoTaoBD.Update();
            }
            catch { }
        }

        private void BindTrinhDoData(LTRINHDO obj)
        {
            try
            {
                lbMATD.Text = obj.MATD;
                var loaibang = ddlLOAIBANG.Items.FindByValue(obj.MALOAIBC);
                if (loaibang != null)
                    ddlLOAIBANG.SelectedIndex = ddlLOAIBANG.Items.IndexOf(loaibang);

                txtCHUYENNGANH.Text = obj.CHUYENNGANH;
                txtNGAYCAPBANG.Text = obj.NGAYCAP != null ? String.Format("{0:dd/MM/yyyy}", obj.NGAYCAP.Value) : "";
                txtTENTRUONG.Text = obj.TENTRUONG;

                var chedohoc = ddlCHEDOHOC.Items.FindByValue(obj.MACHEDOHOC);
                if (chedohoc != null)
                    ddlCHEDOHOC.SelectedIndex = ddlCHEDOHOC.Items.IndexOf(chedohoc);

                if (obj.HINHBC1 != null)
                {
                    imgBANGM1.Visible = true;
                    imgBANGM1.ImageUrl = obj.HINHBC1.ToString();
                }
                else { imgBANGM1.ImageUrl = ""; }

                if (obj.HINHBC2 != null)
                {
                    imgBANGM2.Visible = true;
                    imgBANGM2.ImageUrl = obj.HINHBC2.ToString();
                }
                else { imgBANGM2.ImageUrl = ""; }
               
                upinfoTrinhDo.Update();
            }
            catch { }
        }

        private void BindCongTacData(LQTCONGTAC obj)
        {
            try
            {
                lbMAQTCT.Text = obj.MAQTCT;

                txtNGAYBDCT.Text = obj.NGAYBDCT != null ? String.Format("{0:MM/yyyy}", obj.NGAYBDCT.Value) : "";
                txtNGAYKTCT.Text = obj.NGAYKTCT != null ? String.Format("{0:MM/yyyy}", obj.NGAYKTCT.Value) : "";
                txtNOIDUNGCT.Text = obj.NOIDUNG;
                
                upinfoQuaTrinhCT.Update();
            }
            catch { }
        }

        private void BindNhapNguData(LNHAPNGU obj)
        {
            try
            {
                lbMANNGU.Text = obj.MANNGU;

                txtNGAYBDNN.Text = obj.NGAYBDNN != null ? String.Format("{0:dd/MM/yyyy}", obj.NGAYBDNN.Value) : "";
                txtNGAYKTNN.Text = obj.NGAYKTNN != null ? String.Format("{0:dd/MM/yyyy}", obj.NGAYKTNN.Value) : "";
                txtQUANHAM.Text = obj.QUANHAM;
                txtNOIDUNGNHAPNGU.Text = obj.NOIDUNG;

                upinfoQuanDoi.Update();
            }
            catch { }
        }

        private void BindSucKhoeData(LSUCKHOE obj)
        {
            try
            {
                lbMASUCKHOE.Text = obj.MASUCKHOE;

                txtTINHTRANGSK.Text = obj.TINHTRANG;
                txtBENHMANTINH.Text = obj.BANHMANTINH;
                txtCHIEUCAO.Text = obj.CHIEUCAO.ToString();
                txtCANNANG.Text = obj.CANNANG.ToString();

                upinfoSucKhoe.Update();
            }
            catch { }
        }
#endregion

        #region grid view

        protected void gvGiaDinh_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvGiaDinh.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindGiaDinh();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvGiaDinh_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var objDb = _lqhgdDao.Get(id);

                        if (objDb != null)
                        {
                            BindQuanHeGDData(objDb);
                            UpdateMode = Mode.Update;
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

        protected void gvLyLich_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            /*
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
            */
        }

        protected void gvLyLich_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvLyLich.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindNVLyLich();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvLyLich_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var objDb = _lnvllDao.Get(id);

                        if (objDb != null)
                        {
                            BindNVLyLichData(objDb);
                            UpdateMode = Mode.Update;
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

        protected void gvNhanVien_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvNhanVien_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "SelectMANV":
                        var nv = _nvdao.Get(id);
                        if (nv != null)
                        {
                            //SetControlValue(txtUSERNAME.ClientID, nv.MANV);
                            lbMANV.Text = nv.MANV;

                            lbTENKV.Text = _kvDao.Get(nv.MAKV).TENKV.ToString();
                            lbTENPHONGBAN.Text = _pbDao.Get(nv.MAPB).TENPB.ToString();
                            txtHOTENKS.Text = nv.HOTEN;

                            if (_lnvllDao.Get(nv.MANV) != null)
                            {
                                var objDb = _lnvllDao.Get(nv.MANV);
                                if (objDb != null)                                
                                {
                                    BindNVLyLichData(objDb);
                                    UpdateMode = Mode.Update;
                                }
                            }

                            upinfoLyLich.Update();

                            //up nhan vien
                            upinfoNhanVien.Update();

                            BindTrinhDo();
                            BindCongTac();
                            BindNhapNgu();
                            BindSucKhoe();
                            BindDaoTaoBD();
                            BindKhenThuongKL();
                            BindGiaDinh();
                            BindNVLyLich();

                            HideDialog("divNhanVien");
                            CloseWaitingDialog();

                            txtHOTENKS.Focus();
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

        protected void gvNhanVien_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvNhanVien.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindNhanVien();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvTrinhDo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvTrinhDo.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindTrinhDo();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvTrinhDo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var objDb = _ltdDao.Get(id);

                        if (objDb != null)
                        {
                            BindTrinhDoData(objDb);
                            UpdateMode = Mode.Update;
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

        protected void gvQuanDoi_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvQuanDoi.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindNhapNgu();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvQuanDoi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var objDb = _lnnDao.Get(id);

                        if (objDb != null)
                        {
                            BindNhapNguData(objDb);
                            UpdateMode = Mode.Update;
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

        protected void gvQuaTrinhCT_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvQuaTrinhCT.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindCongTac();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvQuaTrinhCT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var objDb = _lqtctDao.Get(id);

                        if (objDb != null)
                        {
                            BindCongTacData(objDb);
                            UpdateMode = Mode.Update;
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

        protected void gvSucKhoe_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvSucKhoe.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindSucKhoe();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvSucKhoe_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var objDb = _lskDao.Get(id);

                        if (objDb != null)
                        {
                            BindSucKhoeData(objDb);
                            UpdateMode = Mode.Update;
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

        protected void gvDaoTaoBD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvDaoTaoBD.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindDaoTaoBD();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDaoTaoBD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var objDb = _ldtbdDao.Get(id);

                        if (objDb != null)
                        {
                            BindDaoTaoBDData(objDb);
                            UpdateMode = Mode.Update;
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

        protected void gvKhenThuongKL_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvKhenThuongKL.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindKhenThuongKL();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvKhenThuongKL_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var objDb = _lktklDao.Get(id);

                        if (objDb != null)
                        {
                            BindKhenTKLData(objDb);
                            UpdateMode = Mode.Update;
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
        
        #endregion

        private bool TestTenBang()
        {
            var trinhdo = _ltdDao.GetList();
            foreach (var hinhbc in trinhdo)
            {
                if (hinhbc.HINHBC1 != null)
                {
                    string[] arrListStrM1 = hinhbc.HINHBC1.Split(new char[] { '/' });
                    //int cc = arrListStrM1.Count() ;
                    string filename = arrListStrM1[arrListStrM1.Count()-1];
                    if (UpBANGM1.PostedFile.FileName == filename)
                    {
                        ShowError("Đổi lại tên file hình bằng cấp. Trùng tên bằng.");
                        return false;
                    }
                }

                if (hinhbc.HINHBC2 != null)
                {
                    string[] arrListStrM2 = hinhbc.HINHBC2.Split(new char[] { '/' });
                    //int cc = arrListStrM1.Count() ;
                    string filename = arrListStrM2[arrListStrM2.Count() - 1];
                    if (UpBANGM2.PostedFile.FileName == filename)
                    {
                        ShowError("Đổi lại tên file hình bằng cấp. Trùng tên bằng.");
                        return false;
                    }
                }
                    
            }

            return true;            
        }

        #region  btn Save

        protected void btnSaveLyLich_Click(object sender, EventArgs e)
        {
            try
            {
                var nvll = ObjNVLL;
                if (nvll == null)
                {
                    CloseWaitingDialog();
                    return;
                }
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                Message msg;
                Filtered = FilteredMode.None;

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    //con lai maqtct, manvll, manvn, ngayn, xoa
                    //nvll.MANVLL = _lqtctDao.NewId();
                    nvll.MANVLL = lbMANV.Text.Trim();
                    nvll.MANVN = b;
                    nvll.NGAYN = DateTime.Now;
                    nvll.XOA = false;

                    msg = _lnvllDao.Insert(nvll);
                }
                else //update
                {
                    if (!HasPermission(Functions.NV_NVLyLich, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    nvll.MANVN = b;
                    nvll.NGAYUP = DateTime.Now;

                    _erpClass.HisLyLich(nvll.MANVLL, nvll.MANVLL, "nvlylich");

                    msg = _lnvllDao.Update(nvll);

                }

                CloseWaitingDialog();

                if (msg == null) return;

                if (msg.MsgType != MessageType.Error)
                {
                    ShowInfor(ResourceLabel.Get(msg));

                    ClearNVLyLich();

                    BindNVLyLich();
                    upinfoLyLich.Update();
                    upLyLich.Update();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }

            }
            catch { }
        }

        protected void btnSaveTrinhDo_Click(object sender, EventArgs e)
        {
            try
            {
                var trdo = ObjTrinhDo;
                if (trdo == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                Message msg;
                Filtered = FilteredMode.None;

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    if (UpBANGM1.PostedFile.ContentLength > 0)
                    {
                        if (!TestTenBang())
                            return;

                        tenbangm1 = Path.GetFileName(UpBANGM1.PostedFile.FileName);
                        duongdanm1 = "UpLoadFile/bangcap/" + tenbangm1;
                        UpBANGM1.SaveAs(Server.MapPath("~/" + duongdanm1));
                    }

                    if (UpBANGM2.PostedFile.ContentLength > 0)
                    {
                        if (!TestTenBang())
                            return;

                        tenbangm2 = Path.GetFileName(UpBANGM2.PostedFile.FileName);
                        duongdanm2 = "UpLoadFile/bangcap/" + tenbangm2;
                        UpBANGM2.SaveAs(Server.MapPath("~/" + duongdanm2));
                    }

                    //con lai manv, matd, manvn, ngayn, ngayup
                    trdo.MATD = _ltdDao.NewId();
                    trdo.MANVLL = lbMANV.Text.Trim();
                    trdo.MANVN = b;
                    trdo.NGAYN = DateTime.Now;
                    trdo.XOA = false;

                    msg = _ltdDao.Insert(trdo);
                }
                else //update
                {
                    if (!HasPermission(Functions.NV_NVLyLich, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    if (UpBANGM1.PostedFile.ContentLength > 0)
                    {
                        if (!TestTenBang())
                            return;

                        tenbangm1 = Path.GetFileName(UpBANGM1.PostedFile.FileName);
                        duongdanm1 = "UpLoadFile/bangcap/" + tenbangm1;
                        UpBANGM1.SaveAs(Server.MapPath("~/" + duongdanm1));
                    }

                    if (UpBANGM2.PostedFile.ContentLength > 0)
                    {
                        if (!TestTenBang())
                            return;

                        tenbangm2 = Path.GetFileName(UpBANGM2.PostedFile.FileName);
                        duongdanm2 = "UpLoadFile/bangcap/" + tenbangm2;
                        UpBANGM2.SaveAs(Server.MapPath("~/" + duongdanm2));
                    }

                    trdo.MANVN = b;
                    trdo.NGAYUP = DateTime.Now;

                    _erpClass.HisLyLich(trdo.MATD, trdo.MANVLL, "trinhdo");

                    msg = _ltdDao.Update(trdo);

                }

                CloseWaitingDialog();

                if (msg == null) return;

                if (msg.MsgType != MessageType.Error)
                {
                    ShowInfor(ResourceLabel.Get(msg));
                    ClearTrinhDo();

                    // Refresh grid view
                    //BindDataForGrid();

                    BindTrinhDo();
                    upinfoTrinhDo.Update();
                    upTrinhDo.Update();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }

            }
            catch { }
        }

        protected void btnSaveQTCT_Click(object sender, EventArgs e)
        {
            try
            {
                var congtac = ObjCongTac;
                if (congtac == null)
                {
                    CloseWaitingDialog();
                    return;
                }
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                Message msg;
                Filtered = FilteredMode.None;

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    //con lai maqtct, manvll, manvn, ngayn, xoa
                    congtac.MAQTCT = _lqtctDao.NewId();
                    congtac.MANVLL = lbMANV.Text.Trim();
                    congtac.MANVN = b;
                    congtac.NGAYN = DateTime.Now;
                    congtac.XOA = false;

                    msg = _lqtctDao.Insert(congtac);
                }
                else //update
                {
                    if (!HasPermission(Functions.NV_NVLyLich, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }                    

                    congtac.MANVN = b;
                    congtac.NGAYUP = DateTime.Now;

                    _erpClass.HisLyLich(congtac.MAQTCT, congtac.MANVLL, "congtac");

                    msg = _lqtctDao.Update(congtac);
                    
                }

                CloseWaitingDialog();

                if (msg == null) return;

                if (msg.MsgType != MessageType.Error)
                {
                    ShowInfor(ResourceLabel.Get(msg));
                   
                    ClearCongTac();                   

                    BindCongTac();
                    upinfoQuaTrinhCT.Update();
                    upQuaTrinhCT.Update();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }

            }
            catch { }
        }

        protected void btnSaveQuanDoi_Click(object sender, EventArgs e)
        {
            try
            {
                var nhapngu = ObjNhapNgu;
                if (nhapngu == null)
                {
                    CloseWaitingDialog();
                    return;
                }
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                Message msg;
                Filtered = FilteredMode.None;

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    //con lai maqtct, manvll, manvn, ngayn, xoa
                    nhapngu.MANNGU = _lnnDao.NewId();
                    nhapngu.MANVLL = lbMANV.Text.Trim();
                    nhapngu.MANVN = b;
                    nhapngu.NGAYN = DateTime.Now;
                    nhapngu.XOA = false;

                    msg = _lnnDao.Insert(nhapngu);
                }
                else //update
                {
                    if (!HasPermission(Functions.NV_NVLyLich, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    nhapngu.MANVN = b;
                    nhapngu.NGAYUP = DateTime.Now;

                    _erpClass.HisLyLich(nhapngu.MANNGU, nhapngu.MANVLL, "nhapngu");

                    msg = _lnnDao.Update(nhapngu);
                   
                }

                CloseWaitingDialog();

                if (msg == null) return;

                if (msg.MsgType != MessageType.Error)
                {
                    ShowInfor(ResourceLabel.Get(msg));

                    ClearNhapNgu();

                    BindNhapNgu();
                    upinfoQuanDoi.Update();
                    upQuanDoi.Update();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }

            }
            catch { }
        }

        protected void btnSaveSoTruong_Click(object sender, EventArgs e)
        {
            try
            {
                var suckhoe = ObjSucKhoe;
                if (suckhoe == null)
                {
                    CloseWaitingDialog();
                    return;
                }
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                Message msg;
                Filtered = FilteredMode.None;

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    //con lai maqtct, manvll, manvn, ngayn, xoa
                    suckhoe.MASUCKHOE = _lskDao.NewId();
                    suckhoe.MANVLL = lbMANV.Text.Trim();
                    suckhoe.MANVN = b;
                    suckhoe.NGAYN = DateTime.Now;
                    suckhoe.XOA = false;

                    msg = _lskDao.Insert(suckhoe);
                }
                else //update
                {
                    if (!HasPermission(Functions.NV_NVLyLich, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    suckhoe.MANVN = b;
                    suckhoe.NGAYUP = DateTime.Now;

                    _erpClass.HisLyLich(suckhoe.MASUCKHOE, suckhoe.MANVLL, "suckhoe");

                    msg = _lskDao.Update(suckhoe);

                }

                CloseWaitingDialog();

                if (msg == null) return;

                if (msg.MsgType != MessageType.Error)
                {
                    ShowInfor(ResourceLabel.Get(msg));

                    ClearSucKhoe();

                    BindSucKhoe();
                    upinfoSucKhoe.Update();
                    upSucKhoe.Update();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }

            }
            catch { }
        }        

        protected void btnSaveDTBD_Click(object sender, EventArgs e)
        {
            try
            {
                var daotao = ObjDaoTaoBD;
                if (daotao == null)
                {
                    CloseWaitingDialog();
                    return;
                }
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                Message msg;
                Filtered = FilteredMode.None;

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    //con lai maqtct, manvll, manvn, ngayn, xoa
                    daotao.MADTBD = _ldtbdDao.NewId();
                    daotao.MANVLL = lbMANV.Text.Trim();
                    daotao.MANVN = b;
                    daotao.NGAYN = DateTime.Now;
                    daotao.XOA = false;

                    msg = _ldtbdDao.Insert(daotao);
                }
                else //update
                {
                    if (!HasPermission(Functions.NV_NVLyLich, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    daotao.MANVN = b;
                    daotao.NGAYUP = DateTime.Now;

                    _erpClass.HisLyLich(daotao.MADTBD, daotao.MANVLL, "daotao");

                    msg = _ldtbdDao.Update(daotao);

                }

                CloseWaitingDialog();

                if (msg == null) return;

                if (msg.MsgType != MessageType.Error)
                {
                    ShowInfor(ResourceLabel.Get(msg));

                    ClearDaoTaoBD();

                    BindDaoTaoBD();
                    upinfoDaoTaoBD.Update();
                    upDaoTaoBD.Update();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }

            }
            catch { }
        }

        protected void btnSaveKTKL_Click(object sender, EventArgs e)
        {
            try
            {
                var ktkl = ObjKhenTKL;
                if (ktkl == null)
                {
                    CloseWaitingDialog();
                    return;
                }
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                Message msg;
                Filtered = FilteredMode.None;

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    //con lai maqtct, manvll, manvn, ngayn, xoa
                    ktkl.MAKTKL = _lktklDao.NewId();
                    ktkl.MANVLL = lbMANV.Text.Trim();
                    ktkl.MANVN = b;
                    ktkl.NGAYN = DateTime.Now;
                    ktkl.XOA = false;

                    msg = _lktklDao.Insert(ktkl);
                }
                else //update
                {
                    if (!HasPermission(Functions.NV_NVLyLich, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    ktkl.MANVN = b;
                    ktkl.NGAYUP = DateTime.Now;

                    _erpClass.HisLyLich(ktkl.MAKTKL, ktkl.MANVLL, "khenthuongkl");

                    msg = _lktklDao.Update(ktkl);

                }

                CloseWaitingDialog();

                if (msg == null) return;

                if (msg.MsgType != MessageType.Error)
                {
                    ShowInfor(ResourceLabel.Get(msg));

                    ClearKhenTKL();

                    BindKhenThuongKL();
                    upinfoKhenThuongKL.Update();
                    upKhenThuongKL.Update();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }

            }
            catch { }
        }

        protected void btnSaveQUANHEGD_Click(object sender, EventArgs e)
        {
            try
            {
                var quanhegd = ObjQUANHEGD;
                if (quanhegd == null)
                {
                    CloseWaitingDialog();
                    return;
                }
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                Message msg;
                Filtered = FilteredMode.None;

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    //con lai maqtct, manvll, manvn, ngayn, xoa
                    quanhegd.MAQHGD = _lqhgdDao.NewId();
                    quanhegd.MANVLL = lbMANV.Text.Trim();
                    quanhegd.MANVN = b;
                    quanhegd.NGAYN = DateTime.Now;
                    quanhegd.XOA = false;

                    msg = _lqhgdDao.Insert(quanhegd);
                }
                else //update
                {
                    if (!HasPermission(Functions.NV_NVLyLich, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    quanhegd.MANVN = b;
                    quanhegd.NGAYUP = DateTime.Now;

                    _erpClass.HisLyLich(quanhegd.MAQHGD, quanhegd.MANVLL, "quanhegd");

                    msg = _lqhgdDao.Update(quanhegd);
                    
                }

                CloseWaitingDialog();

                if (msg == null) return;

                if (msg.MsgType != MessageType.Error)
                {
                    ShowInfor(ResourceLabel.Get(msg));

                    ClearQuanHeGD();                   

                    BindGiaDinh();
                    upinfoGiaDinh.Update();
                    upGiaDinh.Update();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }

            }
            catch { }
        }
        #endregion

        protected void btnDeleteTrinhDo_Click(object sender, EventArgs e)
        {

        }

        protected void txtTENTHUONG_TextChanged(object sender, EventArgs e)
        {

        }

        

    }
}