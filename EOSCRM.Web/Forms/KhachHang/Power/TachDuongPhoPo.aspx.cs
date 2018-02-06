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

namespace EOSCRM.Web.Forms.KhachHang.Power
{
    public partial class TachDuongPhoPo : Authentication
    {
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly TachDuongPoDao _tdpoDao = new TachDuongPoDao();
        private readonly UpDuongPhoPoDao _updppoDao = new UpDuongPhoPoDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();

        private UPDUONGPHOPO ObjUPDP
        {
            get
            {
                //if (!ValidateData())
                //    return null;

                var updp = (string.IsNullOrEmpty(lbMAUPDP.Text.Trim()) || lbMAUPDP.Text == "") ?
                        new UPDUONGPHOPO() : _updppoDao.Get(lbMAUPDP.Text.Trim());
                if (updp == null)
                    return null;

                updp.MAKVPO = ddlKHUVUC.SelectedValue;
                updp.MAPBPO = "KD";
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
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void SetLabel(string id, string value)
        {
            ((PO)Page.Master).SetLabel(id, value);
        }

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

        private void ShowWarning(string message)
        {
            ((PO)Page.Master).ShowWarning(message);
        }

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
        }

        private void UnblockWaitingDialog()
        {
            ((PO)Page.Master).UnblockWaitingDialog();
        }

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_TachDuongPhoPo, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_KH_TACHDUONGPHOPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_TACHDUONGPHOPO;
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
                string makvpo = _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO;

                timkv();

                ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
                txtNAM.Text = DateTime.Now.Year.ToString();

                var duongpho = _dppoDao.GetListKV(makvpo);
                ddlDUONGPHO.Items.Clear();
                ddlDUONGPHO.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var dp in duongpho)
                {
                    ddlDUONGPHO.Items.Add(new ListItem(dp.MADPPO + ": " + dp.TENDP, dp.MADPPO));
                }

                var dotin = _diDao.GetListKVPO(makvpo);
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
                    var kvList = _kvpoDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    var kvList = _kvpoDao.GetListPo(d);
                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
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
                        kynayF, DateTime.Now, "DSKVDPTAIVEPO");

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

                bool dungF = _gcspoDao.IsLockTinhCuocKy(kyF, ddlKHUVUC.SelectedValue);
                if (dungF == true)
                {
                    ShowError("Đã khóa sổ kỳ ghi.");
                    CloseWaitingDialog();
                    return;
                }

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

                fileUploadDuongPho.SaveAs(Server.MapPath("~/UpLoadFile/powaco/filemaupo/") + filename);

                updp.MAUPDP = _updppoDao.NewId();
                updp.TENFILE = filename;
                updp.TENPATH = "UpLoadFile/powaco/filemaupo/" + filename;
                updp.MANVN = LoginInfo.MANV;

                _updppoDao.Insert(updp, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                BindUpLoadFile();

                ImportExcelTachDP(updp.TENPATH);

                //BindTachDuongN();

                ShowInfor("Upload đường phố điện thành công. Bấm vào bắt đầu tách đường để hoàn thành.");

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
                var listUploadFile = _updppoDao.GetListKV(ddlKHUVUC.SelectedValue);

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
                    var ltd = _tdpoDao.GetListKyMAKV(thangF, namF, ddlKHUVUC.SelectedValue);
                    //var ltd = _tdpoDao.GetListKyMAKVDotIn(thangF, namF, ddlKHUVUC.SelectedValue, ddlDOTGCS.SelectedValue);

                    gvUpDPN.DataSource = ltd;
                    gvUpDPN.PagerInforText = ltd.Count.ToString();
                    gvUpDPN.DataBind();
                }
                else
                {
                    //var ltd = _tdpoDao.GetListKyMAKV(thangF, namF, ddlKHUVUC.SelectedValue);
                    var ltd = _tdpoDao.GetListKyMAKVDotIn(thangF, namF, ddlKHUVUC.SelectedValue, ddlDOTGCS.SelectedValue);

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

                        var tdn = _tdpoDao.Get(id);
                        if (tdn != null)
                        {
                            lbTENKH.Text = tdn.TENKH;
                            txtMADPCUTD.Text = tdn.MADPPOCU;
                            txtMADBCUTD.Text = tdn.MADBPOCU;
                            txtMADPMOITD.Text = tdn.MADPPOMOI;
                            txtMADBMOITD.Text = tdn.MADBPOMOI;
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

                        var tachduong = new TACHDUONGPO
                        {
                            IDTACH = _tdpoDao.NewId(),
                            IDKHPO = worksheet.Cells[i, 1].Text.Trim().ToString(),
                            TENKH = worksheet.Cells[i, 2].Text.Trim().ToString(),
                            MADPPOCU = worksheet.Cells[i, 3].Text.Trim().ToString(),
                            MADBPOCU = worksheet.Cells[i, 4].Text.Trim().ToString(),
                            MADPPOMOI = worksheet.Cells[i, 5].Text.Trim().ToString(),
                            MADBPOMOI = worksheet.Cells[i, 6].Text.Trim().ToString(),
                            THANG = Convert.ToInt32(worksheet.Cells[i, 7].Text.ToString()),
                            NAM = Convert.ToInt32(worksheet.Cells[i, 8].Text.ToString()),

                            MAKVPO = ddlKHUVUC.SelectedValue,
                            NGAY = DateTime.Now,
                            MANVN = manv,
                            GHICHU = "Tách đường."
                        };
                        _tdpoDao.Insert(tachduong);
                    }
                }
            }
            catch { }
        }

        protected void btDSTachDuong_Click(object sender, EventArgs e)
        {
            try
            {
                int thangF = int.Parse(ddlTHANG.SelectedValue);
                string namF = txtNAM.Text.Trim();
                var kynayF = new DateTime(int.Parse(namF), thangF, 1);

                var listTACHDP = _rpClass.UpLoadFileDuongPho(ddlDOTGCS.SelectedValue, "", ddlKHUVUC.SelectedValue, ddlDUONGPHO.SelectedValue, "", "",
                        kynayF, DateTime.Now, "DSTDNTDCTPO");

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
                                ws.Cells[rowstart, colstart, rowend, colend].Value = "DANH SÁCH TÁCH ĐƯỜNG ĐIỆN (KỲ " + ddlTHANG.SelectedValue +
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
                bool dungF = _gcspoDao.IsLockTinhCuocKy(kyF, ddlKHUVUC.SelectedValue);
                if (dungF == true)
                {
                    ShowError("Đã khóa sổ kỳ ghi.");
                    CloseWaitingDialog();
                    return;
                }

                var tdn = _tdpoDao.Get(lbdivIDMATACH.Text.Trim());
                tdn.MADPPOMOI = txtMADPMOITD.Text.Trim();
                tdn.MADBPOMOI = txtMADBMOITD.Text.Trim();

                Message msg;

                msg = _tdpoDao.Update(tdn, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

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

                bool dungF = _gcspoDao.IsLockTinhCuocKy(kynayF, ddlKHUVUC.SelectedValue);
                if (dungF == true)
                {
                    ShowError("Đã khóa sổ kỳ ghi.");
                    CloseWaitingDialog();
                    return;
                }

                _rpClass.UpLoadFileDuongPho("", "", ddlKHUVUC.SelectedValue, ddlDUONGPHO.SelectedValue, "", "",
                        kynayF, DateTime.Now, "DSTACHDUONGTOKHPO");

                BindTachDuongN();

                ShowInfor("Tách đường phố điện thành công. Kiểm tra lại DS tách đường.");

                CloseWaitingDialog();
                ClearFrom();
                upTachDuong.Update();
            }
            catch { }
        }

        private void ClearFrom()
        {
            try
            {

            }
            catch { }
        }

    }
}