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

namespace EOSCRM.Web.Forms.KhachHang.Power.BaoCaoPo
{
    public partial class DSKHDieuKienPo : Authentication
    {
        private readonly StoredProcedureDao _spDao = new StoredProcedureDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();

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
                Authenticate(Functions.KH_BaoCaoPo_DSKHDieuKienPo, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindUpLoadFile();
                    //BindTachDuongN();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_BAOCAOPO_DSKHDUONGPHOTHEOTRU;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAOPO_DSKHDUONGPHOTHEOTRU;
            }
            CommonFunc.SetPropertiesForGrid(gvUploadFle);
            //CommonFunc.SetPropertiesForGrid(gvUpDPN);
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
                ddlMaDuongPho.Items.Clear();
                ddlMaDuongPho.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var dp in duongpho)
                {
                    ddlMaDuongPho.Items.Add(new ListItem(dp.MADPPO + ": " + dp.TENDP, dp.MADPPO));
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

        protected void btnDsDuongPhoSoTru_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlMaDuongPho.SelectedValue == "%")
                {
                    ShowError("Chọn đường phố tải về.");
                    CloseWaitingDialog();
                    return;
                }

                int thangF = int.Parse(ddlTHANG.SelectedValue);
                string namF = txtNAM.Text.Trim();
                var kynayF = new DateTime(int.Parse(namF), thangF, 1);

                var listDP = _spDao.Get_KhachHangPo_ByDuongPhoId(ddlKHUVUC.SelectedValue, ddlMaDuongPho.SelectedValue, kynayF);

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
            }
            catch { }
        }

        protected void btUpDuongPhoTheoTru_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                int thangHT = DateTime.Now.Month;
                int namHT = DateTime.Now.Year;
                var kyHT = new DateTime(namHT, thangHT, 1);

                int thangF = Convert.ToInt16(ddlTHANG.SelectedValue);
                int namF = Convert.ToInt32(txtNAM.Text.Trim());
                var kyF = new DateTime(namF, thangF, 1);                      

                string tenfileupdp = DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                        DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();

                string filename = tenfileupdp + Path.GetFileName(fileUploadDuongPhoTheoTru.FileName);

                if (filename.Substring(filename.Length - 4, 4).ToUpper() != "XLSX")
                {
                    ShowError("File Excel 2007 trở lên, có đuôi .xlsx ! Kiểm tra lại.");
                    CloseWaitingDialog();
                    return;
                }

                fileUploadDuongPhoTheoTru.SaveAs(Server.MapPath("~/UpLoadFile/powaco/filemaupo/") + filename);
                string filepath = "UpLoadFile/powaco/filemaupo/" + filename;

                var result = _spDao.Insert_UploadFile(filename, filepath, ddlKHUVUC.SelectedValue, b);

                if (result.Tables[0].Rows[0]["KetQua"].ToString() == "Ok")
                {
                    BindUpLoadFile();
                    ExcelToXML(filepath, ddlKHUVUC.SelectedValue, b);                    
                }
                else
                {
                    ShowError("Lỗi File upload. Kiểm tra lại!");
                }

                CloseWaitingDialog();        
                upDSKHDieuKienPo.Update();                
            }
            catch { }
        }

        private void BindUpLoadFile()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var khuvuc = _nvDao.GetKV(b);
                string makvpo = _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO;

                var result = _spDao.Get_UploadFile_ByMakv(makvpo);

                if (result != null)
                {
                    gvUploadFle.DataSource = result.Tables[0];
                    gvUploadFle.PagerInforText = result.Tables[0].Rows.Count.ToString();
                    gvUploadFle.DataBind();
                }
                else
                {
                    gvUploadFle.DataSource = null;
                    gvUploadFle.PagerInforText = "0";
                    gvUploadFle.DataBind();
                }

                CloseWaitingDialog();
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

                        upDSKHDieuKienPo.Update();
                        CloseWaitingDialog();
                        break;
                     
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

        private void ExcelToXML(string patchexcel, string makv, string manv)
        {
            try
            {
                FileInfo excel = new FileInfo(Server.MapPath("~/" + patchexcel));

                using (var package = new ExcelPackage(excel))
                {
                    var workbook = package.Workbook;
                    var worksheet = workbook.Worksheets.First();

                    var xml = "";
                    xml = xml + "<tables>";

                    int totalRows = worksheet.Dimension.End.Row;
                    for (int i = 2; i <= totalRows; i++)
                    {
                        xml += "<items>";
                        xml += "<IDKHPO>" + worksheet.Cells[i, 1].Text.Trim().ToString() + "</IDKHPO>";
                        xml += "<TENKH>" + worksheet.Cells[i, 2].Text.Trim().ToString() + "</TENKH>";
                        xml += "<MADPPO>" + worksheet.Cells[i, 3].Text.Trim().ToString() + "</MADPPO>";
                        xml += "<MADBPO>" + worksheet.Cells[i, 4].Text.Trim().ToString() + "</MADBPO>";
                        xml += "<SOTRU>" + worksheet.Cells[i, 5].Text.Trim().ToString() + "</SOTRU>";
                        xml += "</items>";
                    }
                    xml = xml + "</tables>";
                    //ShowInfor(xml);
                    
                    var result = _spDao.Insert_KhachHangPo_SoTruXML(xml, makv, manv);

                    if (result.Tables[0].Rows[0]["KetQua"].ToString() == "Ok")
                    {
                        ShowInfor("Update theo trụ thành công.");
                    }
                    else
                    {
                        ShowError("Lỗi File upload. Kiểm tra lại!");
                    }

                    CloseWaitingDialog();
                }
            }
            catch { }
        }

        protected void btnLuuDsDuongPhoTheoTru_Click(object sender, EventArgs e)
        {
            
        }


    }
}