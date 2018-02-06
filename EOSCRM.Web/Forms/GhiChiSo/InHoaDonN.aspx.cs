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

namespace EOSCRM.Web.Forms.GhiChiSo
{
    public partial class InHoaDonN : Authentication
    {
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

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_GCS_INHOADONN;
            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_INHOADONN;
            }
            //CommonFunc.SetPropertiesForGrid(gvDuongPho);
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
            upnlGridSHDIL.Update();
            ClearForm();
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
                ShowInfor("Số HĐ đầu phải nhỏ hơn HĐ cuối.");
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
                Message msg;
                msg = _shdDao.Insert(info);
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

        private void BindGridSHDIL()
        {

            var list = _shdilDao.GetList(lblMASOHD.Text.Trim());
            gvSoHoaDonIL.DataSource = list;
            gvSoHoaDonIL.PagerInforText = list.Count.ToString();
            gvSoHoaDonIL.DataBind();

            upnlGridSHDIL.Update();
        }

        protected void gvSoHoaDon_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvSoHoaDon.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindGridSHD();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvSoHoaDonIL_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvSoHoaDonIL.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindGridSHDIL();
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
            var dtDSINHOADON1 =
                new ReportClass().InHoaDonN1(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC.SelectedValue)
                  .Tables[0];
            foreach (System.Data.DataRow row in dtDSINHOADON1.Rows)
            {
                lblSOHDCL.Text = row["SOHDCL"].ToString();
            }
            
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
                    ShowInfor("Số HĐ đầu phải nhỏ hơn HĐ cuối.");
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
                        GHICHU = ""
                    };
                    if (info == null)
                    {
                        CloseWaitingDialog();
                        return;
                    }
                    Message msg;
                    msg = _shdilDao.Insert(info);
                }

                var dtDSINHOADON =
                    new ReportClass().InHoaDonN(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC.SelectedValue, int.Parse(txtSOHDDAUIL.Text.Trim()), int.Parse(txtSOHDCUOIIL.Text.Trim())).
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

                        //code128.Code = row["BARCODE"].ToString();
                        //System.Drawing.Image bc = code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White);
                        //byte[] ar = imageToByte(bc);
                        //row["IMG"] = ar;

                        
                        code128.Code = row["BARCODE"].ToString();

                        //System.Drawing.Image bc = System.Drawing.Image.FromFile("http://powaco.com.vn/UpLoadFile/powaco/hinh/1465206069223-957962213.jpg");
                        System.Drawing.Image bc = System.Drawing.Image.FromFile(Server.MapPath("~/UpLoadFile/powaco/hinh/chau1.jpg"), true);

                        //bc.ImageUrl = "D:\a6101892710Penguins.jpg";

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



    }
}
