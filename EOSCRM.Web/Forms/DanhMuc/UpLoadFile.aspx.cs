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

namespace EOSCRM.Web.Forms.DanhMuc
{
    public partial class UpLoadFile : Authentication
    {
        private readonly CapBacDao _cbDao = new CapBacDao();
        private readonly UpLoadFileDao _uploadDao = new UpLoadFileDao();
        private readonly UpSentNVDao _upsnvDao = new UpSentNVDao();
        private readonly UpSentPBDao _upspbDao = new UpSentPBDao();
        private readonly UpSentKVDao _upskvDao = new UpSentKVDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly PhongBanDao _pbDao = new PhongBanDao();
        private readonly ReportClass _rpC = new ReportClass();

        string khuvuc, manv, filename, duongdan;
        

        private UPLOADFILE ItemObj
        {
            get
            {
                LoadKhuVucUpdate();
                filename = Path.GetFileName(fileUpload1.PostedFile.FileName);
                duongdan = "UpLoadFile/" + khuvuc + "/" + filename;

                string id = _uploadDao.NewId();

                var up = _uploadDao.Get(id) ?? new UPLOADFILE();

                up.MAUPLOAD = id;
                up.MANV = manv;
                up.TENFILE = filename;
                up.TENPATH = duongdan;
                up.DATE = DateTime.Now;


                //insert nhan vien sent
                string masnv = _upsnvDao.NewId();
                string maup = id;
                string manv2 = manv;
                string manvs = manv;
                int magui = 1;
                if (_upsnvDao.GetMS(maup, manvs) == null)
                {
                    _rpC.UpSentNV1(magui, masnv, maup, manv2, manvs);
                }

                return up;
            }
        }

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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PrepareUI();
                Authenticate(Functions.DM_UpLoadFile, Permission.Read);
               
                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();

                    LoadKhuVucUpdate();

                    BindDataForGrid();                   
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_DM_UPDATEFILE;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_UPDATEFILE;
            }

            CommonFunc.SetPropertiesForGrid(gvDetails);
            CommonFunc.SetPropertiesForGrid(gvNhanVien);
        }

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
        }

        private void SetReadonly(string id, bool isReadonly)
        {
            ((EOS)Page.Master).SetReadonly(id, isReadonly);
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

        private void ShowWarning(string message)
        {
            ((EOS)Page.Master).ShowWarning(message);
        }

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }

        private void HideDialog(string divId)
        {
            ((EOS)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
        }

        #endregion

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            
            CloseWaitingDialog();
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {           
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string filePath = gvDetails.DataKeys[gvrow.RowIndex].Value.ToString();

                Response.ContentType = "image/jpg";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filePath + "\"");
                Response.TransmitFile(Server.MapPath("~/"+filePath));
                Response.End();   
        }       

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //string filename = Path.GetFileName(fileUpload1.PostedFile.FileName);
            //fileUpload1.SaveAs(Server.MapPath("UpLoadFile/" + filename));            

            var info = ItemObj;
            if (info == null)
            {
                CloseWaitingDialog();
                return;
            }

            Message msg;

            // insert new
            if (UpdateMode == Mode.Create)
            {
                if (!HasPermission(Functions.DM_UpLoadFile, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                /*var tontai = _uploadDao.Get(id);
                if (tontai != null)
                {
                    CloseWaitingDialog();
                    ShowError("Mã công việc đã tồn tại", txtMACV.ClientID);
                    return;
                }*/

                LoadKhuVucUpdate();
                
                filename = Path.GetFileName(fileUpload1.PostedFile.FileName);
                duongdan = "UpLoadFile/" + khuvuc + "/" + filename;
                
                //kiem tra file can gui
                if (string.Empty.Equals(filename))
                {                   
                    ShowError("Chọn file cần upload.");
                    return;                   
                }

                var tenfile = _uploadDao.GetTenFile(filename);
                if (tenfile != null)
                {
                    if (tenfile.TENFILE == filename)
                    {
                        ShowInfor("Trùng trên file. Hãy đổi tên file.");
                        return;
                    }
                    else
                    {
                        fileUpload1.SaveAs(Server.MapPath("~/" + duongdan));
                        msg = _uploadDao.Insert(info);
                        // string file_name = Path.GetFileName(FileUpload1.FileName);
                        //FileUpload1.SaveAs(Server.MapPath("~/FoodImage/1/") + file_name);
                        //Label1.Text = "File Upload";
                    }
                }
                else
                {
                    fileUpload1.SaveAs(Server.MapPath("~/" + duongdan));
                    msg = _uploadDao.Insert(info);                       
                }
            }            

            CloseWaitingDialog();
            
            BindDataForGrid();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {           
                CloseWaitingDialog();           
        }

        private void LoadStaticReferences()
        {
           var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            timkv();

            if (b != "nguyen")
            {
                btImportExcel.Visible = false;
                linkTKMAU.Visible = false;
                btPrint.Visible = false;
            }
            else
            {
                btImportExcel.Visible = true;
                linkTKMAU.Visible = true;
                btPrint.Visible = true;
            }

            // bind phong ban
            var pbList = _pbDao.GetList();
            ddlPHONGBAN.Items.Clear();
            ddlPHONGBAN.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var pb in pbList)
            {
                ddlPHONGBAN.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
            }

            txtMANV.Text = "";
            txtTENNV.Text = "";

            BindDataForGrid();
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
        }

        private void LoadKhuVucUpdate()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            //string b = loginInfo.Username;
            manv = loginInfo.Username;

            var query = _nvDao.GetListKV(manv);
            foreach (var a in query)
            {
                string d = a.MAKV;
                if (a.MAKV == "99")
                {
                    khuvuc = "powaco";
                }
                if (a.MAKV == "O")
                {
                    khuvuc = "chauthanh";
                }
                if (a.MAKV == "N")
                {
                    khuvuc = "chauphu";
                }
                if (a.MAKV == "K")
                {
                    khuvuc = "chomoi";
                }
                if (a.MAKV == "L")
                {
                    khuvuc = "triton";
                }
                if (a.MAKV == "P")
                {
                    khuvuc = "phutan";
                }
                if (a.MAKV == "Q")
                {
                    khuvuc = "anphu";
                }
                if (a.MAKV == "T")
                {
                    khuvuc = "tanchau";
                }
                if (a.MAKV == "U")
                {
                    khuvuc = "thoaison";
                }
                if (a.MAKV == "S")
                {
                    khuvuc = "chaudoc";
                }
                if (a.MAKV == "M")
                {
                    khuvuc = "tinhbien";
                }
                if (a.MAKV == "X")
                {
                    khuvuc = "longxuyen";
                }
            }
            
        }       

        private void BindDataForGrid()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var objList = _uploadDao.GetListMANVS(b);

                gvDetails.DataSource = objList;
                //gvDetails.PagerInforText = objList.Count.ToString();
                gvDetails.DataBind();

                //upnlGrid.Update();
                
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnBrowseNhanVien_Click(object sender, EventArgs e)
        {
            BindNhanVien();
            upnlNhanVien.Update();
            UnblockDialog("divNhanVien");
        }

        private void BindNhanVien()
        {
            var list = _nvDao.Search(txtKeywordNV.Text.Trim());
            gvNhanVien.DataSource = list;
            gvNhanVien.PagerInforText = list.Count.ToString();
            gvNhanVien.DataBind();
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
                        var nv = _nvDao.Get(id);
                        if (nv != null)
                        {
                            txtMANV.Text = nv.MANV.ToString();
                            SetControlValue(txtTENNV.ClientID, nv.HOTEN);
                            txtMANV.Focus();

                            upnlInfor.Update();
                        }
                        HideDialog("divNhanVien");
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
                gvNhanVien.PageIndex = e.NewPageIndex;               
                BindNhanVien();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnFilterNV_Click(object sender, EventArgs e)
        {
            BindNhanVien();
            CloseWaitingDialog();
        }

        protected void linkBtnMANV_Click(object sender, EventArgs e)
        {
            var nv = _nvDao.Get(txtMANV.Text.Trim());
            if (nv != null)
            {
                txtTENNV.Text = nv.HOTEN;
                txtTENNV.Focus();
            }

            CloseWaitingDialog();
        }

        protected void linkSETNFILE_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.Empty.Equals(txtMANV.Text.Trim()) && ddlKHUVUC.SelectedValue == "%" && ddlPHONGBAN.SelectedValue == "%")
                {
                    ShowError("Chọn nơi cần gửi file.");
                    return;
                }

                if (!string.Empty.Equals(txtMANV.Text.Trim()) && ddlKHUVUC.SelectedValue == "%" && ddlPHONGBAN.SelectedValue == "%")
                {
                   //nhan vien 1
                    SentFileNV();
                    LoadStaticReferences();
                    Page.Response.Redirect(ResolveUrl("~") + "Forms/DanhMuc/uploadfile.aspx", false);
                    
                }
                if (string.Empty.Equals(txtMANV.Text.Trim()) && ddlKHUVUC.SelectedValue != "%" && ddlPHONGBAN.SelectedValue == "%")
                {
                    //khu vuc 2
                    SentFileKV();
                    LoadStaticReferences();
                    Page.Response.Redirect(ResolveUrl("~") + "Forms/DanhMuc/uploadfile.aspx", false);
                   
                }
                if (string.Empty.Equals(txtMANV.Text.Trim()) && ddlKHUVUC.SelectedValue == "%" && ddlPHONGBAN.SelectedValue != "%")
                {
                    //phong ban 3
                    SentFilePB();
                    LoadStaticReferences();
                    Page.Response.Redirect(ResolveUrl("~") + "Forms/DanhMuc/uploadfile.aspx", false);
                }

                if (!string.Empty.Equals(txtMANV.Text.Trim()) && ddlKHUVUC.SelectedValue != "%" && ddlPHONGBAN.SelectedValue == "%")
                {
                    //nhan vien va khu vuc 12
                    SentFileNV();
                    SentFileKV();
                    LoadStaticReferences();
                    Page.Response.Redirect(ResolveUrl("~") + "Forms/DanhMuc/uploadfile.aspx", false); CloseWaitingDialog();
                }
                if (!string.Empty.Equals(txtMANV.Text.Trim()) && ddlKHUVUC.SelectedValue == "%" && ddlPHONGBAN.SelectedValue != "%")
                {
                    //nhan vien va phong ban 13
                    SentFileNV();
                    SentFilePB();
                    LoadStaticReferences();
                    Page.Response.Redirect(ResolveUrl("~") + "Forms/DanhMuc/uploadfile.aspx", false);                   
                }
                if (string.Empty.Equals(txtMANV.Text.Trim()) && ddlKHUVUC.SelectedValue != "%" && ddlPHONGBAN.SelectedValue != "%")
                {
                    //khuvuc va phong ban 23
                    SentFileKV();
                    SentFilePB();
                    LoadStaticReferences();
                    Page.Response.Redirect(ResolveUrl("~") + "Forms/DanhMuc/uploadfile.aspx", false);                    
                }

                if (!string.Empty.Equals(txtMANV.Text.Trim()) && ddlKHUVUC.SelectedValue != "%" && ddlPHONGBAN.SelectedValue != "%")
                {
                    //khuvuc va phong ban 23
                    SentFileKV();
                    SentFilePB();
                    SentFileKV();
                    LoadStaticReferences();
                    Page.Response.Redirect(ResolveUrl("~") + "Forms/DanhMuc/uploadfile.aspx", false);                   
                }
               
                //upnlGrid.Update();

                CloseWaitingDialog();

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }           
        }

        private void SentFilePB()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<UPLOADFILE>();
                    var listIds = strIds.Split(',');

                    //Add ma vao danh sách cần sent
                    objs.AddRange(listIds.Select(ma => _uploadDao.Get(ma)));

                    if (objs.Count > 0)
                    {
                        for (int i = 0; i < objs.Count; i++)
                        {
                            string masnv = _upspbDao.NewId();
                            string maup = objs[i].MAUPLOAD;
                            string manv = b;
                            //string manvs = txtMANV.Text.Trim();
                            string mapbs = ddlPHONGBAN.SelectedValue;
                            //string makvs = ddlKHUVUC.SelectedValue;
                            int magui = 3;

                            if (_upspbDao.GetMS(maup, mapbs) == null)
                            {
                                _rpC.UpSentNV1(magui, masnv, maup, manv, mapbs);
                            }
                            else { ShowInfor("File" + objs[i].TENFILE + "này đã có. Chọn lại."); }
                        }
                    }
                    else
                    {
                        ShowInfor("Hãy chọn file cần gửi.");
                    }

                }

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void SentFileNV()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<UPLOADFILE>();
                    var listIds = strIds.Split(',');

                    //Add ma vao danh sách cần sent
                    objs.AddRange(listIds.Select(ma => _uploadDao.Get(ma)));

                    if (objs.Count > 0)
                    {
                        for (int i = 0; i < objs.Count; i++)
                        {
                            string masnv = _upsnvDao.NewId();
                            string maup = objs[i].MAUPLOAD;
                            string manv = b;
                            string manvs = txtMANV.Text.Trim();
                            //string mapbs = ddlPHONGBAN.SelectedValue;
                            //string makvs = ddlKHUVUC.SelectedValue;
                            int magui = 1;

                            if (_upsnvDao.GetMS(maup, manvs) == null)
                            {
                                _rpC.UpSentNV1(magui, masnv, maup, manv, manvs);
                            }
                            else { ShowInfor("File" + objs[i].TENFILE + "này đã có. Chọn lại."); }
                        }
                    }
                    else
                    {
                        ShowInfor("Hãy chọn file cần gửi.");
                    }

                }

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void SentFileKV()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<UPLOADFILE>();
                    var listIds = strIds.Split(',');                                        

                    //Add ma vao danh sách cần sent
                    objs.AddRange(listIds.Select(ma => _uploadDao.Get(ma)));

                    if (objs.Count > 0)
                    {
                        for (int i = 0; i < objs.Count; i++)
                        {
                            string masnv = _upskvDao.NewId();
                            string maup = objs[i].MAUPLOAD;
                            string manv = b;
                            //string manvs = txtMANV.Text.Trim();
                            //string mapbs = ddlPHONGBAN.SelectedValue;
                            string makvs = ddlKHUVUC.SelectedValue;
                            int magui = 2;

                            if (_upskvDao.GetMS(maup, makvs) == null)
                            {
                                _rpC.UpSentNV1(magui, masnv, maup, manv, makvs);
                            }
                            else { ShowInfor("File" + objs[i].TENFILE + "này đã có. Chọn lại."); }
                        }
                    }
                    else
                    {
                        ShowInfor("Hãy chọn file cần gửi.");
                    }
                }

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btImportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo excel = new FileInfo(Server.MapPath("~/UpLoadFile/importexcel/CAPBAC.xlsx"));

                using (var package = new ExcelPackage(excel))
                {
                    var workbook = package.Workbook;  
                    var worksheet = workbook.Worksheets.First();

                    //*** Retrieve to List
                    var ls = new List<ExcelCapbac>();

                    int totalRows = worksheet.Dimension.End.Row;

                    for (int i = 2; i <= totalRows; i++)
                    {
                        //ls.Add(new ExcelCapbac
                        //{
                        //    MACB = worksheet.Cells[i, 1].Text.ToString(),
                        //    TENCB = worksheet.Cells[i, 2].Text.ToString(),                            
                        //});   
                     
                        var capbac = new CAPBAC
                        {
                            MACB = worksheet.Cells[i, 1].Text.ToString(),
                            TENCB = worksheet.Cells[i, 2].Text.ToString(),            
                        };
                        _cbDao.Insert(capbac);
                    }                   

                    //*** Display to GridView
                    //this.myGridView.DataSource = ls;
                    //this.myGridView.DataBind();
                }
            }
            catch { }
       }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch { }
        }  

    }

    public class ExcelCapbac
    {
        public string MACB { get; set; }

        public string TENCB { get; set; }
        
    }

}