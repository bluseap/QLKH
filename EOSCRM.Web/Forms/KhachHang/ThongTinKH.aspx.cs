using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Globalization;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Data;

namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class ThongTinKH : Authentication
    {
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly LoaiPhanHoiDao _lphDao = new LoaiPhanHoiDao();
        private readonly ThongTinKHDao _ttkhDao = new ThongTinKHDao();        

        private THONGTINKH ObjTTKH
        {
            get
            {
                if (!ValidateData())
                    return null;

                var ttkh = (string.IsNullOrEmpty(lbIDTTKH.Text.Trim()) || lbIDTTKH.Text == "") ? new THONGTINKH() : _ttkhDao.Get(lbIDTTKH.Text.Trim());
                if (ttkh == null)
                    return null;

                ttkh.SODB = txtMADDK.Text.Trim();
                ttkh.MAKV = ddlKHUVUCMOI.SelectedValue;
                ttkh.TENKH = txtTENKHMOI.Text.Trim();
                ttkh.DIACHI = txtDIACHILD.Text.Trim();
                ttkh.MALOAI = ddlLOAIPHANHOI.SelectedValue;//loai phan hoi
                ttkh.NGAYNHAN = DateTimeUtil.GetVietNamDate(txtNGAYNHAN.Text.Trim());
                ttkh.NGAYNHAP = DateTime.Now;
                ttkh.MANV = LoginInfo.MANV;
                ttkh.GHICHU = txtGHICHU.Text.Trim();

                return ttkh;

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
                Authenticate(Functions.KH_ThongTinKH, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindDataForGrid();
                }
                else
                {
                    var dt = (DataTable)Session[SessionKey.KH_THONGTINKH];
                    Report(dt);
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }        

        public bool ValidateData()
        {
            if (!string.IsNullOrEmpty(txtNGAYNHAN.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYNHAN.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày nhận thông tin "), txtNGAYNHAN.ClientID);
                    return false;
                }
            }

            if (txtMADDK.Text == "")
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Phải nhập danh bộ. "), txtMADDK.ClientID);
                return false;
            }

            return true;
        }        

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_THONGTINKH;
            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_THONGTINKH;
            }
            
            CommonFunc.SetPropertiesForGrid(gvList);
        }

        private void LoadStaticReferences()
        {
            try
            {
                var lph = _lphDao.GetList();

                txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");

                timkv();

                txtMADDK.Text = "";
                txtTENKHMOI.Text = "";
                txtDIACHILD.Text = "";
                txtNGAYNHAN.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtGHICHU.Text = "";
                //lbIDTTKH.Text = "";
                reloadm.Text = "0";

                /*ddlLOAIPHANHOI.Items.Clear();
                foreach (var ph in lph)
                {
                    ddlLOAIPHANHOI.Items.Add(new ListItem(ph.MALOAI,ph.TENLOAI));
                }*/


                ddlLOAIPHANHOI.DataSource = lph;
                ddlLOAIPHANHOI.DataTextField = "TENLOAI";
                ddlLOAIPHANHOI.DataValueField = "MALOAI";
                ddlLOAIPHANHOI.DataBind();
               

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void Clear()
        {
            var lph = _lphDao.GetList();  

            UpdateMode = Mode.Create;

            txtMADDK.Text = "";
            txtTENKHMOI.Text = "";
            txtDIACHILD.Text = "";
            txtNGAYNHAN.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtGHICHU.Text = "";
            lbIDTTKH.Text = "";

            ddlLOAIPHANHOI.SelectedIndex = 0;
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
                    ddlKHUVUCMOI.Items.Clear();                    
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {                        
                        ddlKHUVUCMOI.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);                    
                    ddlKHUVUCMOI.Items.Clear();
                    foreach (var kv in kvList)
                    {                        
                        ddlKHUVUCMOI.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateData())
                {
                    CloseWaitingDialog();
                    return;
                }

                var info = ObjTTKH;
                if (info == null)
                {
                    CloseWaitingDialog();
                    return;
                }                

                Message msg;
                Filtered = FilteredMode.None;

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    if (!HasPermission(Functions.KH_ThongTinKH, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    _ttkhDao.Insert(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                                       
                    msg = null;

                }
                // update
                else
                {
                    if (!HasPermission(Functions.KH_ThongTinKH, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }
                    //lbIDTDH; txtMADDK;
                    msg = _ttkhDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV,
                        lbIDTTKH.Text.Trim());

                }

                CloseWaitingDialog();

                Clear();
                BindDataForGrid();
                upnlGrid.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindItem(THONGTINKH obj)
        {
            if (obj == null)
                return;

            txtMADDK.Text = obj.SODB;
            txtTENKHMOI.Text = obj.TENKH;
            txtDIACHILD.Text = obj.DIACHI;

            var item9 = ddlLOAIPHANHOI.Items.FindByValue(obj.MALOAI);
            if (item9 != null)
                ddlLOAIPHANHOI.SelectedIndex = ddlLOAIPHANHOI.Items.IndexOf(item9);
                
            
            txtNGAYNHAN.Text = obj.NGAYNHAN.Value.ToString("dd/MM/yyyy");
            txtGHICHU.Text = obj.GHICHU;
            lbIDTTKH.Text = obj.IDTTKH.ToString();


            reloadm.Text = "0";           


            upnlInfor.Update();
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var obj = _ttkhDao.Get(id);
                        if (obj == null)
                        {
                            CloseWaitingDialog();
                            return;
                        }
                        BindItem(obj);
                        UpdateMode = Mode.Update;
                        CloseWaitingDialog();
                        upnlInfor.Update();
                        break;
                    
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BaoCaoInTHD(string idthd)
        {
            /*
            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch
                {

                }
            }


            DataTable dt = new ReportClass().InThayHopDong(idthd).Tables[0];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/DonLapDatMoi/INHOPDONG.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divCR.Visible = true;
            upnlCrystalReport.Update();

            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
            */
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {               
                gvList.PageIndex = e.NewPageIndex;            
                BindDataForGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnIDTTKH") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        private void BindDataForGrid()
        {
            if (Filtered == FilteredMode.None)
            {
                var objList = _ttkhDao.GetListKV(ddlKHUVUCMOI.SelectedValue);

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
            }
            else
            {
                var objList = _ttkhDao.GetListKV(ddlKHUVUCMOI.SelectedValue);

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();                
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            CloseWaitingDialog();
        }

        protected void btLOC_Click(object sender, EventArgs e)
        {
            try
            {
                var objList = _ttkhDao.GetListDBTEN(txtMADDK.Text.Trim(), txtTENKHMOI.Text.Trim(), ddlKHUVUCMOI.SelectedValue);

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();

                gvList.Visible = true;                
                upnlGrid.Update();

                CloseWaitingDialog();
            }
            catch { }            
        }

        protected void btBAOCAO_Click(object sender, EventArgs e)
        {
            BaoCao();
        }

        private void BaoCao()
        {
            try
            {
                var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
                var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

                var ds = new ReportClass().DSQuiTrinhNuocBien(TuNgay, DenNgay, ddlKHUVUCMOI.SelectedValue, "", "", "", "DSTHONGTINKHN");

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                Report(ds.Tables[0]);

                gvList.Visible = false;
                upnlGrid.Update();

                CloseWaitingDialog();
            }
            catch { }
        }

        private void Report(DataTable dt)
        {
            if (dt == null)
                return;

            #region FreeMemory
            var rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }             
                catch {     }
            }
            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/QuanLyKhachHang/DSThongTinKH.rpt");
            rp.Load(path);

            var txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            if (txtXN != null)
                txtXN.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + ddlKHUVUCMOI.SelectedItem.ToString().ToUpper();

            var txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            if (txtTuNgay1 != null)
                txtTuNgay1.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim();

            var txtNGAY = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            if (txtNGAY != null)
                txtNGAY.Text = "An Giang, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divCR.Visible = true;

            upnlCrystalReport.Update();

            Session[SessionKey.KH_THONGTINKH] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }


    }
}