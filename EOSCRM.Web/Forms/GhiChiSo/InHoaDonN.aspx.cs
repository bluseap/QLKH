using System;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using EOSCRM.Controls;
using System.IO;
using System.Data;

namespace EOSCRM.Web.Forms.GhiChiSo
{
    public partial class InHoaDonN : Authentication
    {
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly DotInHDDao _dihdDao = new DotInHDDao();
        private readonly SoHoaDonDao _shdDao = new SoHoaDonDao();
        private readonly SoHoaDonILDao _shdilDao = new SoHoaDonILDao();

        private SOHOADON ItemObj
        {
            get
            {
                //if (!ValidateData())
                //    return null;

                var ss = new SOHOADON
                {
                    THANG = int.Parse(ddlTHANG.SelectedValue),
                    NAM = int.Parse(txtNAM.Text.Trim()),
                    MAKV = ddlKHUVUC.SelectedValue,
                    SOHDDAU = int.Parse(txtHDDAU.Text.Trim()),
                    SOHDCUOI = int.Parse(txtHDCUOI.Text.Trim()),
                    NGAY=DateTime.Now,
                    GHICHU=txtGCSHD.Text.Trim()
                };

                return ss;
            }
        }

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
                Authenticate(Functions.GCS_InHoaDon, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();                
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }        

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_GCS_INHOADONN;
            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_INHOADONN;
            }

            CommonFunc.SetPropertiesForGrid(gvTTTT);
        }

        private void LoadStaticReferences()
        {
            // load khu vuc
            var listKhuVuc = new KhuVucDao().GetList();

            ddlKHUVUC.Items.Clear();
            ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));

            foreach (var kv in listKhuVuc)
            {
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }
            divUpdateHDIL.Visible = false;
            divupnlGridSHDIL.Visible = false;

            var kvIn = _dihdDao.GetListKVNN(ddlKHUVUC.SelectedValue);
            ddlDOTGCS.Items.Clear();
            ddlDOTGCS.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var dotin in kvIn)
            {
                ddlDOTGCS.Items.Add(new ListItem(_dmdiDao.Get(dotin.MADOTIN).TENDOTIN, dotin.IDMADOTIN));
            }

            ClearForm();

            upnlGridSHDIL.Update();            
        }

        private void ClearForm()
        {            
            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString();           
        }

        protected void txtHDCONG_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var hddau = int.Parse(txtHDDAU.Text.Trim());
                var hdcong = int.Parse(txtHDCONG.Text.Trim());
                var hdcuoi = hddau + hdcong;

                txtHDCUOI.Text = hdcuoi.ToString();
                lblSOHDCL.Text = (hdcuoi - hddau + 1).ToString();
               
            }
            catch (System.Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private byte[] imageToByte(System.Drawing.Image img)
        {
            MemoryStream objMS = new MemoryStream();
            img.Save(objMS, System.Drawing.Imaging.ImageFormat.Png);
            return objMS.ToArray();
        }

        protected void btnBaoCaoThuy_Click(object sender, EventArgs e)
        {            
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string manvnhap = loginInfo.Username;

            var list = _shdDao.GetListSHD(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC.SelectedValue);
            var hddau = int.Parse(txtHDDAU.Text.Trim());
            var hdcuoi = int.Parse(txtHDCUOI.Text.Trim());
            
            foreach (var hd in list)
            {
                if (hddau >= hd.SOHDDAU && hddau <= hd.SOHDCUOI)
                {
                    ShowInfor("Đã in hoá đơn rồi.");
                    return ;
                }

                if (hdcuoi >= hd.SOHDDAU && hdcuoi <= hd.SOHDCUOI)
                {
                    ShowInfor("Đã in hoá đơn rồi!");
                    return;
                }
            }

            if (hddau > hdcuoi)
            {
                ShowInfor("Số hoá đơn đầu phải nhỏ hơn hoá đơn cuối.");
                return;
            }
            else
            {
                var info = ItemObj;
                if (info == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                info.TENMAYIN = txtTenMayIn.Text.Trim();
                info.MANVN = manvnhap;
                info.IDMADOTIN = ddlDOTGCS.SelectedValue;

                int tonsohoadon = (hdcuoi - hddau) + 1;
                info.TONSOHOADON = tonsohoadon; // so hoa don dang in

                int tonghoadon = Convert.ToInt32(lbSoHoaDonSum.Text.Trim());
                info.TONGHOADON = tonghoadon; // tong so hoa don in KLTT > 0

                int tonghoadondain = Convert.ToInt32(lbSoHoaDonDaInSum.Text.Trim());
                info.TONGHOADONDAIN = tonghoadondain; // tong hoa don da in

                int tonghoadonchuain = Convert.ToInt32(lbSoHoaDonChuaInSum.Text.Trim());
                info.TONGHOADONCONLAI = tonghoadonchuain; // tong hoa don chua in               

                info.TONGHOADONCONLAICHUAIN = tonghoadonchuain - tonsohoadon;  // tong hoa don con lai chua in

                Message msg;

                msg = _shdDao.Insert(info);
            }


            var ketqua = _rpClass.TinhTienTheoBac(Convert.ToInt16(ddlTHANG.SelectedValue), Convert.ToInt16(txtNAM.Text.Trim()), ddlKHUVUC.SelectedValue,
                        ddlDOTGCS.SelectedValue, hddau, hdcuoi,  "", "UPKHDANGIN");

            DataTable dtth = ketqua.Tables[0];

            if (dtth.Rows[0]["KETQUA"].ToString() != "DUNG")
            {
                CloseWaitingDialog();
                ShowError("Lỗi tính khối lượng tiêu thụ. Kiểm tra lại.", "");
                return;
            }


            var dtDSINHOADON =
                new ReportClass().InHoaDonN(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC.SelectedValue,int.Parse(txtHDDAU.Text.Trim()),int.Parse(txtHDCUOI.Text.Trim())).
                    Tables[0];

            dtDSINHOADON.Columns.Add("IMG", typeof(byte[]));
            Barcode128 code128 = new Barcode128();
            code128.CodeType = Barcode.CODE128;
            code128.BarHeight = 100;
            code128.X = 18;
            code128.N = 1f;
            code128.StartStopText = true;

            if (dtDSINHOADON.Rows.Count > 0)
            {
                foreach (System.Data.DataRow row in dtDSINHOADON.Rows)
                {
                    // noi dung data
                    //string data =  row["BARCODE"];

                    code128.Code = row["BARCODE"].ToString();
                    System.Drawing.Image bc = code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White);
                    byte[] ar = imageToByte(bc);
                    row["IMG"] = ar;

                }
                Session["DSINHOADON"] = dtDSINHOADON;
                CloseWaitingDialog();
                Page.Response.Redirect(ResolveUrl("~") + "Forms/GhiChiSo/BaoCao/rpInHoaThuy.aspx");
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Không tìm thấy dữ liệu để làm báo cáo");
            }

        }        

        private void BindGridSHDIL()
        {
            var list = _shdilDao.GetList(lblMASOHD.Text.Trim());
            gvSoHoaDonIL.DataSource = list;
            gvSoHoaDonIL.PagerInforText = list.Count.ToString();
            gvSoHoaDonIL.DataBind();

            upnlGridSHDIL.Update();
        }        

        protected void gvSoHoaDonIL_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {               
                gvSoHoaDonIL.PageIndex = e.NewPageIndex;
                BindGridSHDIL();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindGridSHD()
        {
            int thang, nam;
            try
            {
                DateTime.ParseExact(txtNAM.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
                thang = int.Parse(ddlTHANG.SelectedValue);
                nam = int.Parse(txtNAM.Text.Trim());
            }
            catch
            {
                return;
            }

            var list = _shdDao.GetListSHD(thang, nam, ddlKHUVUC.SelectedValue);
            gvSoHoaDon.DataSource = list;
            gvSoHoaDon.PagerInforText = list.Count.ToString();
            gvSoHoaDon.DataBind();

            upnlGridSHD.Update();
        }

        protected void gvSoHoaDon_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSoHoaDon.PageIndex = e.NewPageIndex;
                BindGridSHD();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvSoHoaDon_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectHD":
                        HideSHD();
                        var hd = _shdDao.Get(id);
                        lblMASOHD.Text = hd.MASOHD.ToString();
                        lblSOHDDAUIL.Text = hd.SOHDDAU.ToString();
                        lblSOHDCUOIIL.Text = hd.SOHDCUOI.ToString();
                        BindGridSHDIL();
                        CloseWaitingDialog();
                        break;

                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridSHD();

            //var dtDSINHOADON1 =
            //    new ReportClass().InHoaDonN1(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC.SelectedValue)
            //      .Tables[0];

            //foreach (System.Data.DataRow row in dtDSINHOADON1.Rows)
            //{
            //    lblSOHDCL.Text = row["SOHDCL"].ToString();
            //}            
        }

        public void HideSHD()
        {
            divupnlTinhCuoc.Visible = false;
            upnlTinhCuoc.Update();
            divUpdateHDIL.Visible = true;
            UpdateHDIL.Update();
            divupnlGridSHD.Visible = false;
            divupnlGridSHDIL.Visible = true;

        }
        public void NotHideSHD()
        {
            divupnlTinhCuoc.Visible = true;
            upnlTinhCuoc.Update();
            divUpdateHDIL.Visible = false;
            UpdateHDIL.Update();
            divupnlGridSHD.Visible = true;
            divupnlGridSHDIL.Visible = false;

        }

        protected void btnLocIL_Click(object sender, EventArgs e)
        {
            BindGridSHDIL();
        }

        protected void btnINLAI_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string manvnhap = loginInfo.Username;

            var list = _shdilDao.GetListSHDIL(int.Parse(lblMASOHD.Text.Trim()));
            var hddauil = int.Parse(txtSOHDDAUIL.Text.Trim());
            var hdcuoiil = int.Parse(txtSOHDCUOIIL.Text.Trim());

            if (hddauil >= int.Parse(lblSOHDDAUIL.Text.Trim()) && hddauil <= int.Parse(lblSOHDCUOIIL.Text.Trim()))
            {
                /*foreach(var il in list)
                {
                    if(hddauil >= il.SOHDDAUIL && hddauil <= il.SOHDCUOIIL)
                    {
                        ShowInfor("Hoá đơn đã in lại rồi.");
                        return;
                    }
                    if (hdcuoiil >= il.SOHDDAUIL && hdcuoiil <= il.SOHDCUOIIL)
                    {
                        ShowInfor("Hoá đơn đã in lại rồi!");
                        return;
                    }
                    
                }*/
                if (hddauil > hdcuoiil)
                {
                    ShowInfor("Số hoá đơn đầu phải nhỏ hơn HĐ cuối.");
                    return;
                }
                else
                {
                    var info = new SOHOADONIL
                    {
                        MASOHD = int.Parse(lblMASOHD.Text.Trim()),
                        SOHDDAUIL = int.Parse(txtSOHDDAUIL.Text.Trim()),
                        SOHDCUOIIL = int.Parse(txtSOHDCUOIIL.Text.Trim()),
                        NGAY = DateTime.Now,
                        GHICHU = "",
                        MANVN = manvnhap,
                        TENMAYIN = txtTenMayInInLai.Text.Trim()                        
                    };

                    if (info == null)
                    {
                        CloseWaitingDialog();
                        return;
                    }

                    Message msg;

                    msg = _shdilDao.Insert(info);
                }

                var dtDSINHOADON = new ReportClass().InHoaDonN(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC.SelectedValue, 
                        int.Parse(txtSOHDDAUIL.Text.Trim()), int.Parse(txtSOHDCUOIIL.Text.Trim())).Tables[0];

                dtDSINHOADON.Columns.Add("IMG", typeof(byte[]));
                Barcode128 code128 = new Barcode128();
                code128.CodeType = Barcode.CODE128;
                code128.BarHeight = 100;
                code128.X = 18;
                code128.N = 1f;
                code128.StartStopText = true;

                if (dtDSINHOADON.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow row in dtDSINHOADON.Rows)
                    {
                        code128.Code = row["BARCODE"].ToString();
                        System.Drawing.Image bc = code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White);
                        byte[] ar = imageToByte(bc);
                        row["IMG"] = ar;

                        // noi dung data
                        //string data =  row["BARCODE"];

                        //code128.Code = row["BARCODE"].ToString();
                        //System.Drawing.Image bc = code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White);
                        //byte[] ar = imageToByte(bc);
                        //row["IMG"] = ar;

                        
                        //code128.Code = row["BARCODE"].ToString();

                        //System.Drawing.Image bc = System.Drawing.Image.FromFile("http://powaco.com.vn/UpLoadFile/powaco/hinh/1465206069223-957962213.jpg");
                        //System.Drawing.Image bc = System.Drawing.Image.FromFile(Server.MapPath("~/UpLoadFile/powaco/hinh/chau1.jpg"), true);

                        //bc.ImageUrl = "D:\a6101892710Penguins.jpg";

                       // byte[] ar = imageToByte(bc);
                        //row["IMG"] = ar;
                       

                    }
                    Session["DSINHOADON"] = dtDSINHOADON;
                    CloseWaitingDialog();
                    Page.Response.Redirect(ResolveUrl("~") + "Forms/GhiChiSo/BaoCao/rpInHoaThuy.aspx");
                }
                else
                {
                    CloseWaitingDialog();
                    ShowError("Không tìm thấy dữ liệu để làm báo cáo");
                }
            }
            else
            {
                ShowInfor("Hoá đơn đã in rồi.");
                return ;
            }            
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            NotHideSHD();
            LoadStaticReferences();
        }

        protected void ddlKHUVUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlKHUVUC.SelectedValue != "%")
            {
                ThuHoToDotInHD(ddlKHUVUC.SelectedValue);
            }            
        }

        private void ThuHoToDotInHD(string mathuho)
        {
            try
            {
                if (mathuho != "%")
                {
                    var kvIn = _dihdDao.GetListKVNN(ddlKHUVUC.SelectedValue);
                    ddlDOTGCS.Items.Clear();
                    ddlDOTGCS.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var dotin in kvIn)
                    {
                        ddlDOTGCS.Items.Add(new ListItem(_dmdiDao.Get(dotin.MADOTIN).TENDOTIN, dotin.IDMADOTIN));
                    }
                }
                else
                {
                    ddlDOTGCS.SelectedIndex = 0;
                }
            }
            catch { }
        }       

        private void IsVisibleSumSoHoaDon(bool para)
        {
            lbSoHoaDon.Visible = para; 
            lbSoKhachHang.Visible = para; 
            lbSoHoaDonDaIn.Visible = para; 
            lbSoHoaDonChuaIn.Visible = para;

            lbSoHoaDonSum.Visible = para;
            lbSoKhachHangSum.Visible = para;
            lbSoHoaDonDaInSum.Visible = para;
            lbSoHoaDonChuaInSum.Visible = para;

            btXemSoHoaDonChuaIn.Visible = para;
        }

        protected void ddlDOTGCS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDOTGCS.SelectedValue != "%")
            {
                IsVisibleSumSoHoaDon(true);

                LoadSumSoHoaDonTheoDotIn("nguyen");
            }
            else
            {
                IsVisibleSumSoHoaDon(false);

                LoadSumSoHoaDonTheoDotIn("%");
            }

            upnlTinhCuoc.Update();

        }

        private void LoadSumSoHoaDonTheoDotIn(string para)
        {
            try
            {
                if (para != "%")
                {
                    var ketqua = _rpClass.TinhTienTheoBac(Convert.ToInt16(ddlTHANG.SelectedValue), Convert.ToInt16(txtNAM.Text.Trim()), ddlKHUVUC.SelectedValue,
                        ddlDOTGCS.SelectedValue, 0, 0, "", "SumSoHoaDonKH");

                    DataTable dtth = ketqua.Tables[0];

                    var sokhachhang = dtth.Rows[0]["SOKHACHHANG"].ToString();
                    var sohoadon = dtth.Rows[0]["SOHOADON"].ToString();
                    var sohoadondain = dtth.Rows[0]["SOHOADONDAIN"].ToString();
                    var sohoadonchuain = dtth.Rows[0]["SOHOADONCHUAIN"].ToString();

                    lbSoKhachHangSum.Text = sokhachhang;
                    lbSoHoaDonSum.Text = sohoadon;

                    lbSoHoaDonDaInSum.Text = sohoadondain;
                    lbSoHoaDonChuaInSum.Text = sohoadonchuain;
                }
                else
                {
                    lbSoHoaDonSum.Text = "0";
                    lbSoKhachHangSum.Text = "0";
                    lbSoHoaDonDaInSum.Text = "0";
                    lbSoHoaDonChuaInSum.Text = "0";
                }
            }
            catch { }
        }

        protected void btXemSoHoaDonChuaIn_Click(object sender, EventArgs e)
        {
            try
            {
                BindSoHoaDonTieuthu();
                UpdivSoThuTuHoaDonChuaIn.Update();

                UnblockDialog("divSoThuTuHoaDonChuaIn");

                CloseWaitingDialog();
            }
            catch { }
        }

        protected void txtHDCUOI_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var hddau = int.Parse(txtHDDAU.Text.Trim());
                var hdcuoi = int.Parse(txtHDCUOI.Text.Trim());

                lblSOHDCL.Text = (hdcuoi - hddau + 1).ToString();
            }
            catch { }
        }

        private void BindSoHoaDonTieuthu()
        {        
            var dotinhd = _dihdDao.Get(ddlDOTGCS.SelectedValue);

            if (dotinhd != null && dotinhd.MADOTIN == "NNNTD1")
            {
                var list = _khDao.GetListTieuThuSoHoaDonChuaInDotInNhoThu(Convert.ToInt16(txtNAM.Text.Trim()), Convert.ToInt16(ddlTHANG.SelectedValue), ddlKHUVUC.SelectedValue,
                        ddlDOTGCS.SelectedValue);

                gvTTTT.DataSource = list;
                gvTTTT.PagerInforText = list.Count.ToString();
                gvTTTT.DataBind();
            }
            else
            {
                var list = _khDao.GetListTieuThuSoHoaDonChuaInDotIn(Convert.ToInt16(txtNAM.Text.Trim()), Convert.ToInt16(ddlTHANG.SelectedValue), ddlKHUVUC.SelectedValue,
                        ddlDOTGCS.SelectedValue);

                gvTTTT.DataSource = list;
                gvTTTT.PagerInforText = list.Count.ToString();
                gvTTTT.DataBind();
            }
        }

        #region gvTTTT
        protected void gvTTTT_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTTTT.PageIndex = e.NewPageIndex;
                BindSoHoaDonTieuthu();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }        
        #endregion


    }
}
