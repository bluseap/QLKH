using System;
using System.Globalization;
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
    public partial class InHoaDonLe : Authentication
    {
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly TieuThuDao ttDao = new TieuThuDao();
        private readonly KhachHangDao khDao = new KhachHangDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.GCS_InHoaDonLe, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_GCS_INHOADONLE;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_INHOADONLE;
            }

            CommonFunc.SetPropertiesForGrid(gvDuongPho);
            CommonFunc.SetPropertiesForGrid(gvList);
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

        private void LoadStaticReferences()
        {
            // load khu vuc
            var listKhuVuc = new KhuVucDao().GetList();

            ddlKHUVUC.Items.Clear();
            ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));

            foreach(var kv in listKhuVuc)
            {
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }

            ClearForm();
        }

        private void ClearForm()
        {
            /*
             * clear phần thông tin hồ sơ
             */
            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString();

            txtMADP.Text = "";
            txtDUONGPHU.Text = "";
            lblTENDUONG.Text = "";
        }

        private void BindDataToGrid()
        {
            //TODO: lấy danh sách khách hàng theo điều kiện lọc ở đây
            String makv = null;
            if (ddlKHUVUC.SelectedIndex != 0)
                makv = ddlKHUVUC.Text.Trim();

            var danhsach = ttDao.SearchTieuThu(ddlTHANG.SelectedIndex + 1, int.Parse(txtNAM.Text.Trim()), makv,
                                               txtMADP.Text.Trim(), txtMAKH.Text.Trim(), txtTENKH.Text.Trim(), int.Parse(txtLoTrinhDau.Text.Trim()), int.Parse(txtLoTrinhCuoi.Text .Trim()));
            gvList.DataSource = danhsach;
            gvList.PagerInforText = danhsach.Count.ToString();
            gvList.DataBind();
            divList.Visible = danhsach.Count > 0;

            upnlGrid.Update();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // load lo trinh tinh cuoc by khu vuc and duong pho
            try
            {
                DateTime.ParseExact(txtNAM.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
                int.Parse(txtLoTrinhDau.Text.Trim());
                int.Parse(txtLoTrinhCuoi.Text.Trim());
            }
            catch
            {
                CloseWaitingDialog();
                ShowError("Vui lòng chọn năm hoặc lộ trình hợp lệ.", txtNAM.ClientID);
                return;
            }

            BindDataToGrid();
            CloseWaitingDialog();
        }
        
        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvList.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindDataToGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        #region Đường phố
        private void BindDuongPho()
        {
            var list = dpDao.GetList("%", txtKeywordDP.Text.Trim());
            gvDuongPho.DataSource = list;
            gvDuongPho.PagerInforText = list.Count.ToString();
            gvDuongPho.DataBind();
        }

        private void UpdateKhuVuc(DUONGPHO dp)
        {
            SetLabel(lblTENDUONG.ClientID, dp.TENDP);

            var kv = ddlKHUVUC.Items.FindByValue(dp.MAKV);
            if (kv != null)
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);
        }

        protected void btnBrowseDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            upnlDuongPho.Update();
            UnblockDialog("divDuongPho");
        }

        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            CloseWaitingDialog();
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
                            upnlTinhCuoc.Update();

                            HideDialog("divDuongPho");
                            CloseWaitingDialog();

                            txtMADP.Focus();
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
        #endregion

        private byte[] imageToByte(System.Drawing.Image img)
        {
            MemoryStream objMS = new MemoryStream();
            img.Save(objMS, System.Drawing.Imaging.ImageFormat.Png);
            return objMS.ToArray();
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var strIds = Request["listIds"];
            if ((strIds != null) && (!string.Empty.Equals(strIds)))
            {
                var listIds = strIds.Split(',');

                if (listIds.Length > 0)
                {
                    var idkhsrt = "";

                    foreach (var idkh in listIds)
                    {
                        idkhsrt += "'" + idkh + "',";
                    }

                    idkhsrt = idkhsrt.Substring(0, idkhsrt.Length - 1);

                    var dtDSINHOADON =
                        new ReportClass().InHoaDonLeTN1(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()),
                                                       idkhsrt).Tables[0];

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
                else
                {
                    CloseWaitingDialog();
                    ShowError("Không tìm thấy dữ liệu để làm báo cáo");
                }
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Vui lòng chọn dữ liệu để làm báo cáo");
            }
        }

        protected void btnInLoTrinh_Click(object sender, EventArgs e)
        {
            int nam, thang, ltd, ltc;
            
            // load lo trinh tinh cuoc by khu vuc and duong pho
            try
            {
                DateTime.ParseExact(txtNAM.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
                nam = int.Parse(txtNAM.Text.Trim());
                thang = int.Parse(ddlTHANG.SelectedValue.Trim());
                ltd = int.Parse(txtLoTrinhDau.Text.Trim());
                ltc = int.Parse(txtLoTrinhCuoi.Text.Trim());
            }
            catch
            {
                CloseWaitingDialog();
                ShowError("Vui lòng chọn năm hoặc lộ trình hợp lệ.", txtNAM.ClientID);
                return;
            }

            var madp = txtMADP.Text.Trim();
            var duongphu = txtDUONGPHU.Text.Trim();

            var dp = dpDao.Get(madp, duongphu);
            if(dp == null)
            {
                CloseWaitingDialog();
                ShowError("Vui lòng chọn đường phố hợp lệ.", txtMADP.ClientID);
                return;
            }

            var dtDSINHOADON = new ReportClass().InHoaDonLeTheoLoTrinhDT(thang, nam, madp, duongphu, ltd, ltc).Tables[0];

            if (dtDSINHOADON.Rows.Count > 0)
            {
                Session["DSINHOADON"] = dtDSINHOADON;
                CloseWaitingDialog();
                Page.Response.Redirect(ResolveUrl("~") + "Forms/GhiChiSo/BaoCao/rpInHoaThuy.aspx");
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Không tìm thấy dữ liệu để làm báo cáo");
            }

            CloseWaitingDialog();
        }

        protected void btnInLeTheoDanhSach_Click(object sender, EventArgs e)
        {
            var listIds = txtDanhSach.Text.Trim().Split(',');

            if (listIds.Length > 0)
            {
                var idkhsrt = "";

                foreach (var sodb in listIds)
                {
                    // get idkh from madb
                    var kh = khDao.GetKhachHangFromMadb(sodb.Trim());
                   
                    if (kh != null)
                        //idkhsrt += "'" + kh.IDKH + "',";
                        idkhsrt += "'" + kh.IDKH + "',";                        
                                                
                                              
                }
                if(idkhsrt.Length > 0)
                    idkhsrt = idkhsrt.Substring(0, idkhsrt.Length - 1);

                if (idkhsrt.Length == 0)
                {
                    CloseWaitingDialog();
                    ShowError("Không tìm thấy dữ liệu để làm báo cáo 1");
                    return;
                }

                /*var dtDSINHOADON =
                    new ReportClass().InHoaDonLeTn(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()),
                                                   idkhsrt).Tables[0];*/
                var dtDSINHOADON =
                    new ReportClass().InHoaDonLeTN1(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()),
                                                   idkhsrt).Tables[0];

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
                    ShowError("Không tìm thấy dữ liệu để làm báo cáo 2");
                }
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Không tìm thấy dữ liệu để làm báo cáo 3");
            }
        }
    }
}
