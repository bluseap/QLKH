using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Util;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;

using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class TachDuongPho : Authentication
    {
        private readonly GhiChiSoDao _gcsDao = new GhiChiSoDao();
        private readonly TachDuongNDao _tdnDao = new TachDuongNDao();
        private readonly UpDuongPhoDao _updpDao = new UpDuongPhoDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DuongPhoDao _dpDao = new DuongPhoDao();
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();

        private UPDUONGPHO ObjUPDP
        {
            get
            {
                //if (!ValidateData())
                //    return null;

                var updp = (string.IsNullOrEmpty(lbMAUPDP.Text.Trim()) || lbMAUPDP.Text == "") ?
                        new UPDUONGPHO() : _updpDao.Get(lbMAUPDP.Text.Trim());
                if (updp == null)
                    return null;

                updp.MAKV = ddlKHUVUC.SelectedValue;
                updp.MAPB = "KD";
                //updp.TENFILE = "";
                //updp.TENPATH = "";
                updp.GHICHU = "";
                //updp.MANVN = "";
                updp.NGAYN = DateTime.Now;

                return updp;
            }
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
                Authenticate(Functions.KH_TachDuongPho, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();

                    BindUpLoadFile();

                    BindTachDuongN();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }        
        }
              
        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_TACHDUONGPHO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_TACHDUONGPHO;
            }

            CommonFunc.SetPropertiesForGrid(gvUploadFle);
            CommonFunc.SetPropertiesForGrid(gvUpDPN);   
        }

        private void LoadStaticReferences()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                string makv = _nvDao.Get(b).MAKV;

                timkv();

                ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
                txtNAM.Text = DateTime.Now.Year.ToString();

                var duongpho = _dpDao.GetListKV(makv);
                ddlDUONGPHO.Items.Clear();
                ddlDUONGPHO.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var dp in duongpho)
                {
                    ddlDUONGPHO.Items.Add(new ListItem(dp.MADP + ": " + dp.TENDP, dp.MADP));
                }

                var dotin = _diDao.GetListKVNN(makv);
                ddlDOTGCS.Items.Clear();
                ddlDOTGCS.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var d in dotin)
                {
                    ddlDOTGCS.Items.Add(new ListItem(_dmdiDao.Get(d.MADOTIN).TENDOTIN, d.IDMADOTIN));
                }
            }
            catch { }
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

        protected void btTAIDUONGPHO_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlDUONGPHO.SelectedValue == "%")
                {
                    ShowError("Chọn đường phố tải về.");
                    CloseWaitingDialog();
                    return;
                }

                int thangF = int.Parse(ddlTHANG.SelectedValue);
                string namF = txtNAM.Text.Trim();
                var kynayF = new DateTime(int.Parse(namF), thangF, 1);

                var listDP = _rpClass.UpLoadFileDuongPho("", "", ddlKHUVUC.SelectedValue, ddlDUONGPHO.SelectedValue, "", "",
                        kynayF, DateTime.Now, "DSKVDPTAIVE");

                using (DataSet ds2 = listDP)
                {
                    if (ds2 != null && ds2.Tables.Count > 0)
                    {
                        using (ExcelPackage xp = new ExcelPackage())
                        {
                            foreach (DataTable dt2 in ds2.Tables)
                            {
                                ExcelWorksheet ws = xp.Workbook.Worksheets.Add(dt2.TableName);

                                int rowstart = 0;
                                int colstart = 1;
                                int rowend = rowstart;
                                int colend = colstart + dt2.Columns.Count;

                                //ws.Cells[rowstart, colstart, rowend, colend].Merge = true;
                                //ws.Cells[rowstart, colstart, rowend, colend].Value = dt2.TableName;
                                //ws.Cells[rowstart, colstart, rowend, colend].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                //ws.Cells[rowstart, colstart, rowend, colend].Style.Font.Bold = true;
                                //ws.Cells[rowstart, colstart, rowend, colend].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                //ws.Cells[rowstart, colstart, rowend, colend].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                                rowstart += 1;                                

                                rowend = rowstart + dt2.Rows.Count;
                                ws.Cells[rowstart, colstart].LoadFromDataTable(dt2, true);
                                int i = 1;
                                foreach (DataColumn dc in dt2.Columns)
                                {
                                    i++;
                                    if (dc.DataType == typeof(decimal))
                                        //ws.Column(i).Style.Numberformat.Format = "#0.00";
                                        ws.Column(i).Style.Numberformat.Format = "#0";
                                }
                                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                                ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Top.Style =
                                   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Bottom.Style =
                                   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Left.Style =
                                   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            }
                            Response.AddHeader("content-disposition", "attachment;filename=" + ds2.DataSetName + ".xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                            Response.BinaryWrite(xp.GetAsByteArray());
                            Response.End();
                        }
                    }
                }

                //string filePath = "UpLoadFile/powaco/filemau/MauDuongPho.xlsx";
                //Response.ContentType = "image/jpg";
                //Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filePath + "\"");
                //Response.TransmitFile(Server.MapPath("~/" + filePath));
                //Response.End();
            }
            catch { }
        }

        protected void btUpDuongPho_Click(object sender, EventArgs e)
        {
            try
            {
                int thangHT = DateTime.Now.Month;
                int namHT = DateTime.Now.Year;
                var kyHT = new DateTime(namHT, thangHT, 1);

                int thangF = Convert.ToInt16(ddlTHANG.SelectedValue);
                int namF = Convert.ToInt32(txtNAM.Text.Trim());
                var kyF = new DateTime(namF, thangF, 1);

                //bool dungF = _gcsDao.IsLockTinhCuocKy(kyF, ddlKHUVUC.SelectedValue);
                //if (dungF == true)
                //{
                //    ShowError("Đã khóa sổ kỳ ghi.");
                //    CloseWaitingDialog();
                //    return;
                //}

                //bool lockAll = _gcsDao.IsLockAll(kyF, ddlKHUVUC.SelectedValue);
                //if (lockAll == true)
                //{
                //    ShowError("Đã khóa sổ kỳ ghi.");
                //    CloseWaitingDialog();
                //    return;
                //}

                var updp = ObjUPDP;

                string tenfileupdp = DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                        DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();

                string filename = tenfileupdp + Path.GetFileName(fileUploadDuongPho.FileName);

                if (filename.Substring(filename.Length - 4, 4).ToUpper() != "XLSX")
                {
                    ShowError("File Excel 2007 trở lên, có đuôi .xlsx ! Kiểm tra lại.");
                    CloseWaitingDialog();
                    return;
                }

                fileUploadDuongPho.SaveAs(Server.MapPath("~/UpLoadFile/powaco/filemau/") + filename);

                updp.MAUPDP = _updpDao.NewId();
                updp.TENFILE = filename;
                updp.TENPATH = "UpLoadFile/powaco/filemau/" + filename;                
                updp.MANVN = LoginInfo.MANV;

                _updpDao.Insert(updp, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                BindUpLoadFile();

                if (IsKhoaSoTachDuong(updp.TENPATH) == false)
                {
                    ShowError("Có đợt ghi chỉ số đã khóa sổ. Kiểm tra lại từng khách hàng.");
                    CloseWaitingDialog();
                    return;
                }
                else
                {
                    ImportExcelTachDP(updp.TENPATH);
                }                               

                //BindTachDuongN();               
                
                //_rpClass.UpLoadFileDuongPho("", "", ddlKHUVUC.SelectedValue, ddlDUONGPHO.SelectedValue, "", "", kyF, DateTime.Now, "UPKHTTDSTD");

                ShowInfor("Upload đường phố thành công. Bấm vào bắt đầu tách đường để hoàn thành.");

                CloseWaitingDialog();
                ClearFrom();

                upnlThongTin.Update();
                upTachDuong.Update();
            }
            catch { }
        }

        private void BindUpLoadFile()
        {
            try
            {
                var listUploadFile = _updpDao.GetListKV(ddlKHUVUC.SelectedValue);

                gvUploadFle.DataSource = listUploadFile;
                gvUploadFle.PagerInforText = listUploadFile.Count.ToString();
                gvUploadFle.DataBind();

                upnlThongTin.Update();
            }
            catch { }
        }

        #region gvUploadFile      

        protected void gvUploadFle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvUploadFle.PageIndex = e.NewPageIndex;
                BindUpLoadFile();                
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvUploadFle_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();  

                switch (e.CommandName)
                {
                    case "SMaupDP":
                        lbMAUPDP.Text = id;                        

                        upnlThongTin.Update();
                        CloseWaitingDialog();
                        break;

                    //case "DownloadMaupDP":

                    //    lbMAUPDP.Text = id;

                    //    HideDialog("divUploadFile");

                    //    upUploadFle.Update();
                    //    upnlThongTin.Update();
                    //    CloseWaitingDialog();

                    //    string filePath = "UpLoadFile/powaco/filemau/MauDuongPho.xlsx";

                    //    Response.ContentType = "image/jpg";
                    //    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filePath + "\"");
                    //    Response.TransmitFile(Server.MapPath("~/" + filePath));
                    //    Response.End();
                       

                    //    break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvUploadFle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lkMAUPDP = e.Row.FindControl("lkMAUPDP") as LinkButton;
            if (lkMAUPDP == null) return;
            lkMAUPDP.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lkMAUPDP) + "')");         
        }
        #endregion

        protected void lkDownloadDP_Click(object sender, EventArgs e)
        {
            try
            {
                HideDialog("divUploadFile");                              

                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string filePath = gvUploadFle.DataKeys[gvrow.RowIndex].Value.ToString();

                DowloadFileDp(filePath);

                CloseWaitingDialog();                
                upnlThongTin.Update();  
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void DowloadFileDp(string path)
        {
            try
            {
                Response.Clear();

                Response.ContentType = "image/jpg";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + path + "\"");
                Response.TransmitFile(Server.MapPath("~/" + path));

                Response.Flush();
                //Response.End();
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();

                CloseWaitingDialog();                
                upnlThongTin.Update();  
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void lkXemFileUp_Click(object sender, EventArgs e)
        {
            try
            {     
                BindUpLoadFile();

                CloseWaitingDialog();                
                upnlThongTin.Update();                  
            }
            catch { }
        }

        private void BindTachDuongN()
        {
            try
            {
                int thangF = Convert.ToInt32(ddlTHANG.SelectedValue);
                int namF = Convert.ToInt32(txtNAM.Text.Trim());

                if (ddlDOTGCS.SelectedValue == "%")
                {
                    var ltd = _tdnDao.GetListKyMAKV(thangF, namF, ddlKHUVUC.SelectedValue);

                    gvUpDPN.DataSource = ltd;
                    gvUpDPN.PagerInforText = ltd.Count.ToString();
                    gvUpDPN.DataBind();
                }
                else
                {
                    //var ltd = _tdnDao.GetListKyMAKV(thangF, namF, ddlKHUVUC.SelectedValue);
                    var ltd = _tdnDao.GetListKyMAKVDotIn(thangF, namF, ddlKHUVUC.SelectedValue, ddlDOTGCS.SelectedValue);

                    gvUpDPN.DataSource = ltd;
                    gvUpDPN.PagerInforText = ltd.Count.ToString();
                    gvUpDPN.DataBind();
                }
                upTachDuong.Update();
            }
            catch { }
        }

        #region gvUpDPN
        protected void gvUpDPN_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvUpDPN.PageIndex = e.NewPageIndex;
                BindTachDuongN();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvUpDPN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SIDTACH":
                        lbdivIDMATACH.Text = id;                        

                        var tdn = _tdnDao.Get(id);
                        if (tdn != null)
                        {
                            lbTENKH.Text = tdn.TENKH;
                            txtMADPCUTD.Text = tdn.MADPCU;
                            txtMADBCUTD.Text = tdn.MADBCU;
                            txtMADPMOITD.Text = tdn.MADPMOI;
                            txtMADBMOITD.Text = tdn.MADBMOI;
                        }

                        updivTachDuong.Update();
                        UnblockDialog("divTachDuong");                        

                        upTachDuong.Update();
                        CloseWaitingDialog();

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvUpDPN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var IDTACH = e.Row.FindControl("IDTACH") as LinkButton;
            if (IDTACH == null) return;
            IDTACH.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(IDTACH) + "')");
        }
        #endregion

        private void ImportExcelTachDP(string patchexcel)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string manv = loginInfo.Username;

                FileInfo excel = new FileInfo(Server.MapPath("~/" + patchexcel));

                using (var package = new ExcelPackage(excel))
                {
                    var workbook = package.Workbook;
                    var worksheet = workbook.Worksheets.First();

                    //*** Retrieve to List
                    //var ls = new List<ExcelTachDuongNuoc>();

                    int totalRows = worksheet.Dimension.End.Row;

                    for (int i = 2; i <= totalRows; i++)
                    {
                        //ls.Add(new ExcelCapbac
                        //{
                        //    MACB = worksheet.Cells[i, 1].Text.ToString(),
                        //    TENCB = worksheet.Cells[i, 2].Text.ToString(),                            
                        //});   

                        int thangF = Convert.ToInt32(worksheet.Cells[i, 7].Text.ToString());
                        int namF = Convert.ToInt32(worksheet.Cells[i, 8].Text.ToString());
                        var kyF = new DateTime(namF, thangF, 1);

                        bool dungF = _gcsDao.IsLockTinhCuocKy(kyF, ddlKHUVUC.SelectedValue);
                        if (dungF == true)
                        {
                            ShowError("Đã khóa sổ kỳ ghi. Kiểm tra file Excel, chọn kỳ cho đúng.");
                            CloseWaitingDialog();
                            return;
                        }

                        var tachduong = new TACHDUONGN
                        {
                            IDTACH = _tdnDao.NewId(),
                            IDKH = worksheet.Cells[i, 1].Text.Trim().ToString(),
                            TENKH = worksheet.Cells[i, 2].Text.Trim().ToString(),
                            MADPCU = worksheet.Cells[i, 3].Text.Trim().ToString(),
                            MADBCU = worksheet.Cells[i, 4].Text.Trim().ToString(),
                            MADPMOI = worksheet.Cells[i, 5].Text.Trim().ToString(),
                            MADBMOI = worksheet.Cells[i, 6].Text.Trim().ToString(),
                            THANG = Convert.ToInt32(worksheet.Cells[i, 7].Text.ToString()),
                            NAM = Convert.ToInt32(worksheet.Cells[i, 8].Text.ToString()),

                            MAKV = ddlKHUVUC.SelectedValue,
                            NGAY = DateTime.Now,
                            MANVN = manv,
                            GHICHU = "Tách đường."
                        };         
                        _tdnDao.Insert(tachduong);
                    }                   
                }
            }
            catch { }
        }

        protected void btDSTachDuong_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsDuongPho() == true)
                {
                    ShowError("Chưa có đường phố mới. Nhập đường phố mới");
                    return;
                }                

                int thangF = int.Parse(ddlTHANG.SelectedValue);
                string namF = txtNAM.Text.Trim();
                var kynayF = new DateTime(int.Parse(namF), thangF, 1);

                var listTACHDP = _rpClass.UpLoadFileDuongPho(ddlDOTGCS.SelectedValue, "", ddlKHUVUC.SelectedValue, ddlDUONGPHO.SelectedValue, "", "",
                        kynayF, DateTime.Now, "DSTDNTDCT");

                using (DataSet ds2 = listTACHDP)
                {
                    if (ds2 != null && ds2.Tables.Count > 0)
                    {
                        using (ExcelPackage xp = new ExcelPackage())
                        {
                            foreach (DataTable dt2 in ds2.Tables)
                            {
                                ExcelWorksheet ws = xp.Workbook.Worksheets.Add(dt2.TableName);

                                int rowstart = 2;
                                int colstart = 2;
                                int rowend = rowstart;
                                int colend = colstart + dt2.Columns.Count;

                                ws.Cells[rowstart, colstart, rowend, colend].Merge = true;
                                ws.Cells[rowstart, colstart, rowend, colend].Value = "DANH SÁCH TÁCH ĐƯỜNG NƯỚC (KỲ " + ddlTHANG.SelectedValue +
                                        "/" + txtNAM.Text.Trim() + ")";
                                ws.Cells[rowstart, colstart, rowend, colend].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                ws.Cells[rowstart, colstart, rowend, colend].Style.Font.Bold = true;
                                ws.Cells[rowstart, colstart, rowend, colend].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[rowstart, colstart, rowend, colend].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                                rowstart += 2;
                                rowend = rowstart + dt2.Rows.Count;
                                ws.Cells[rowstart, colstart].LoadFromDataTable(dt2, true);
                                int i = 1;
                                foreach (DataColumn dc in dt2.Columns)
                                {
                                    i++;
                                    if (dc.DataType == typeof(decimal))
                                        //ws.Column(i).Style.Numberformat.Format = "#0.00";
                                        ws.Column(i).Style.Numberformat.Format = "#0";
                                }
                                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                                ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Top.Style =
                                   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Bottom.Style =
                                   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Left.Style =
                                   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            }
                            Response.AddHeader("content-disposition", "attachment;filename=" + ds2.DataSetName + ".xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                            Response.BinaryWrite(xp.GetAsByteArray());
                            Response.End();
                        }
                    }
                }
            }
            catch { }
        }

        protected void btLuuSuaTDN_Click(object sender, EventArgs e)
        {
            try
            {
                int thangF = Convert.ToInt16(ddlTHANG.SelectedValue);
                int namF = Convert.ToInt32(txtNAM.Text.Trim());
                var kyF = new DateTime(namF, thangF, 1);
                bool dungF = _gcsDao.IsLockTinhCuocKy(kyF, ddlKHUVUC.SelectedValue);
                if (dungF == true)
                {
                    ShowError("Đã khóa sổ kỳ ghi.");
                    CloseWaitingDialog();
                    return;
                }

                var tdn = _tdnDao.Get(lbdivIDMATACH.Text.Trim());                
                tdn.MADPMOI = txtMADPMOITD.Text.Trim();
                tdn.MADBMOI = txtMADBMOITD.Text.Trim();

                Message msg;

                msg = _tdnDao.Update(tdn, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    HideDialog("divTachDuong");

                    BindTachDuongN();

                    CloseWaitingDialog();
                    ClearFrom();
                    upTachDuong.Update();

                    ShowInfor(ResourceLabel.Get(msg));
                }
                else
                {
                    CloseWaitingDialog();
                    ShowError("<strong>Lỗi xảy ra</strong>. <br/><br/>Dò lỗi: <br />" +
                        ResourceLabel.Get(msg), txtMADBMOITD.ClientID);
                }                 
            }
            catch { }
        }        

        protected void btLOC_Click(object sender, EventArgs e)
        {
            try
            {
                BindTachDuongN();

                CloseWaitingDialog();
                ClearFrom();
                upTachDuong.Update();
            }
            catch { }
        }

        protected void btDSTachDuongKH_Click(object sender, EventArgs e)
        {
            try
            {
                int thangF = int.Parse(ddlTHANG.SelectedValue);
                string namF = txtNAM.Text.Trim();
                var kynayF = new DateTime(int.Parse(namF), thangF, 1);      

                //bool dungF = _gcsDao.IsLockTinhCuocKy(kynayF, ddlKHUVUC.SelectedValue);
                //if (dungF == true)
                //{
                //    ShowError("Đã khóa sổ kỳ ghi.");
                //    CloseWaitingDialog();
                //    return;
                //}

                _rpClass.UpLoadFileDuongPho("", "", ddlKHUVUC.SelectedValue, ddlDUONGPHO.SelectedValue, "", "",
                        kynayF, DateTime.Now, "DSTACHDUONGTOKH");

                BindTachDuongN();

                //ShowInfor("Tách đường thành công. Kiểm tra lại DS tách đường.");

                if (IsDuongPho() == true)
                {
                    ShowError("Chưa có đường phố mới. Nhập đường phố mới");
                }
                else
                {
                    ShowInfor("Tách đường thành công. Kiểm tra lại DS tách đường.");
                }

                CloseWaitingDialog();
                ClearFrom();
                upTachDuong.Update();
            }
            catch { }
        }

        private bool IsDuongPho()
        {
            int thangF = int.Parse(ddlTHANG.SelectedValue);
            string namF = txtNAM.Text.Trim();
            var kynayF = new DateTime(int.Parse(namF), thangF, 1);     

            var madpmoi = _rpClass.UpLoadFileDuongPho("", "", ddlKHUVUC.SelectedValue, ddlDUONGPHO.SelectedValue, "", "",
                        kynayF, DateTime.Now, "THEMMADPMOI");

            DataTable madpmoiTable = madpmoi.Tables[0];

            if (madpmoiTable.Rows[0]["KETQUA"].ToString() == "DUNG")
            {
                return true;
            }
            else 
            {
                return false;
            }            
        }

        private bool IsKhoaSoTachDuong(string patchexcel)
        {
            FileInfo excel = new FileInfo(Server.MapPath("~/" + patchexcel));

            int thangF = int.Parse(ddlTHANG.SelectedValue);
            string namF = txtNAM.Text.Trim();
            var kynayF = new DateTime(int.Parse(namF), thangF, 1);

            using (var package = new ExcelPackage(excel))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.First();

                int totalRows = worksheet.Dimension.End.Row;

                for (int i = 2; i <= totalRows; i++)
                {
                    bool dungMADPCU = _gcsDao.IsLockTinhCuocKy1(kynayF, ddlKHUVUC.SelectedValue, worksheet.Cells[i, 3].Text.Trim().ToString());
                    bool dungMADPMOI = _gcsDao.IsLockTinhCuocKy1(kynayF, ddlKHUVUC.SelectedValue, worksheet.Cells[i, 5].Text.Trim().ToString());

                    if (dungMADPCU == true || dungMADPMOI == true)
                    {
                        return false;
                    }
                    
                     //var tachduong = new TACHDUONGN
                     //   {
                     //       IDTACH = _tdnDao.NewId(),
                     //       IDKH = worksheet.Cells[i, 1].Text.Trim().ToString(),
                     //       TENKH = worksheet.Cells[i, 2].Text.Trim().ToString(),
                     //       MADPCU = worksheet.Cells[i, 3].Text.Trim().ToString(),
                     //       MADBCU = worksheet.Cells[i, 4].Text.Trim().ToString(),
                     //       MADPMOI = worksheet.Cells[i, 5].Text.Trim().ToString(),
                     //       MADBMOI = worksheet.Cells[i, 6].Text.Trim().ToString(),
                     //       THANG = Convert.ToInt32(worksheet.Cells[i, 7].Text.ToString()),
                     //       NAM = Convert.ToInt32(worksheet.Cells[i, 8].Text.ToString()),

                     //       MAKV = ddlKHUVUC.SelectedValue,
                     //       NGAY = DateTime.Now,
                     //       MANVN = manv,
                     //       GHICHU = "Tách đường."
                     //   };         
                    //_tdpoDao.Insert(tachduong);
                }
            }
            return true;
        }

        private void ClearFrom()
        {
            try
            {

            }
            catch { }
        }      
              

    }

    public class ExcelTachDuongNuoc
    {
        public string IDTACH { get; set; }
        public string IDKH { get; set; }
        //public string MAKV { get; set; }
        //public string TENKH { get; set; }
        //public string MADPCU { get; set; }
        //public string MADBCU { get; set; }
        //public int STTCU { get; set; }
        //public string MADPMOI { get; set; }
        //public string MADBMOI { get; set; }

    }

}