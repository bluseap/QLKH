using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOSCRM.Util ;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.UserControls;
using EOSCRM.Web.Shared;
using EOSCRM.Controls;


namespace EOSCRM.Web.Forms.CongNo
{
    public partial class ThuTaiQuay : Authentication
    {
        private readonly CongNoDao _cnDao = new CongNoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly DuongPhoDao _dpDao = new DuongPhoDao();
        private readonly ThuTaiQuayDao _thutaiquayDao =new ThuTaiQuayDao();

        #region Properties

        protected DateTime? FromDate
        {
            get
            {               
                try
                {
                    return DateTimeUtil.GetVietNamDate(txtTuNgay.Text .Trim());
                }
                catch
                {
                    return null;
                }
            }
        }
        protected DateTime? ToDate
        {
            get
            {
                try
                {
                    return DateTimeUtil.GetVietNamDate(txtDenNgay.Text .Trim());
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.CN_NhapCongNo, Permission.Read);
                AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    //BindDataForGrid(gvList);
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_CN_THUTAIQUAY;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_CONGNO;
                header.TitlePage = Resources.Message.PAGE_CN_THUTAIQUAY;
            }

            txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //ddlTHANG.Text = DateTime.Now.Month.ToString();
            txtNAM.Text = DateTime.Now.Year.ToString();
        }

        private void LoadStaticReferences()
        {
            //TODO: Load các đối tượng có liên quan lên UI
            try
            {
                var kvList = _kvDao.GetList();

                ddlKHUVUC.Items.Clear();
                ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in kvList)
                {
                    ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                }
                           
                txtNGAYCN.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtNAM.Text = DateTime.Now.Year.ToString();
                if(DateTime .Now.Month.ToString().Length <2)
                {
                    ddlTHANG.Text = "0" + DateTime.Now.Month.ToString();
                }else
                {
                    ddlTHANG.Text = DateTime.Now.Month.ToString();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void ShowInfor(string message)
        {
            RegisterStartupScript("jsShowInfor" + Guid.NewGuid(), "showInfor('" + message + "');");
        }
   
        #region Common methods

        /// <summary>
        /// Bind data for grid
        /// </summary>
        /// <param name="grdView"></param>
        private void BindDataForGrid(Grid grid)
        {
            try
            {
                int thang = int.Parse(ddlTHANG.Text.Trim());
                int nam = 0;
                try
                {
                    nam = int.Parse(txtNAM.Text.Trim());
                }catch
                {
                    ShowInfor("Năm không hợp lệ");
                    return;
                }

                var objList = _thutaiquayDao.GetList(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim(), FromDate, ToDate,
                                                     thang, nam);    
                
                grid.DataSource = objList;
                grid.PagerInforText = objList.Count.ToString();
                grid.DataBind();
                upnlGrid.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message,
                                                        ex.StackTrace));
            }
        }

        /// <summary>
        /// Kiểm tra dữ liệu trên form
        /// </summary>
        /// <returns></returns>
        public bool IsDataValid()
        {
            //TODO: check validate data

            if (string.Empty.Equals(txtSODB.Text))
            {
                ShowInfor(String.Format(Resources.Message.E_INVALID_DATA, "Số danh bộ "));
              
                return false;
            }

            if (!string.Empty.Equals(txtSODB.Text.Trim()))
            {

                TIEUTHU tieuthu = new CongNoDao().Get(txtSODB.Text.Trim(), int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()));
                if (tieuthu == null)
                {
                    ShowInfor("Khách hàng không tồn tại trong kỳ tiêu thụ");

                    return false;
                }

                if(tieuthu.TONGTIEN == 0 )
                {
                    ShowInfor("Khách hàng có số tiến tiêu thụ là 0 đồng");

                    return false;
                }
            }

            try
            {
                int.Parse(txtNAM.Text.Trim());
            }
            catch
            {
                ShowInfor("Năm không đúng");
                txtNAM.Focus();
                return false;
            }         

            try
            {
                DateTimeUtil.GetVietNamDate(txtNGAYCN.Text.Trim());
            }catch
            {
                ShowInfor("Ngày tháng công nợ không đúng");
                txtNGAYCN.Focus();
                return false;
            }

            try
            {
                decimal.Parse(txtSoTien.Text.Trim());
            }
            catch
            {
                ShowInfor("Số tiền không hợp lệ");
                txtSoTien.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Tra màn hình lại ban đầu
        /// </summary>
        private void ClearContent()
        {
            txtSODB.Text = "";
            lblSoDb.Text = "";
            txtTENKH.Text = "";
            txtSoTien.Text = "";
            txtGhiChuCn.Text = "";
        }
        #endregion

#pragma warning disable 114,108
        private void RegisterStartupScript(string key, string script)
#pragma warning restore 114,108
        {
            ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), key, script, true);
        }

        private void CloseDialog(string dialogId)
        {
            RegisterStartupScript("jsCloseDialg", "closeDialog('" + dialogId + "');");
        }  

        private void SetControlValue(string id, string value)
        {
            RegisterStartupScript(string.Format("jsSetValueForControl-{0}-{1}", id, Guid.NewGuid()),
                string.Format("setControlValue('{0}', '{1}');", id, value));
        }

        private void SetLabel(string id, string value)
        {
            RegisterStartupScript("jssetLabelText" + id, "setLabelText('" + id + "', '" + value + "');");
        }
    
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsDataValid())
                    return;

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var listIds = strIds.Split(',');

                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {

                    
                    }
                }

                // Refresh grid view
                BindDataForGrid(gvList);

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearContent();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.CN_ThuTaiQuay, Permission.Delete))
                {
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<THUTAIQUAY>();
                    var listIds = strIds.Split(',');

                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {
                      
                        THUTAIQUAY thutaiquay = _thutaiquayDao.Get(int.Parse(ma));
                        if (thutaiquay != null)
                        {
                            var msg = _thutaiquayDao.Delete(thutaiquay);
                            if ((msg != null) && (msg.MsgType != MessageType.Error))
                            {
                                ShowInfor(ResourceLabel.Get(msg));

                                // Refresh grid view
                                BindDataForGrid(gvList);
                                return;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var madon = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditHoSo":
                        // Authenticate
                        //Authenticate(Functions.SC_GiaiQuyetDonSuaChua , Permission.Read);

                        //var don = _objDao.Get(madon);
                        //if (don == null) return;

                        //var row = (GridViewRow)((Control)e.CommandSource).Parent.Parent;
                        //row.RowState = DataControlRowState.Edit;

                        //UpdateMode = Mode.Update;
                        //Giaiquyetthongtinsuachua = don;

           
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvList.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindDataForGrid(gvList);
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }           
      
        protected void btn_ThemMoi_Click(object sender, EventArgs e)
        {
            // Authenticate
            if (!HasPermission(Functions.CN_ThuTaiQuay, Permission.Delete))
            {
                ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                return;
            }

            if (!IsDataValid())
                return;


            TIEUTHU congno = _cnDao.Get(txtSODB.Text.Trim(), int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()));
            if(congno == null)
            {
                ShowInfor("Công nợ không tồn tại trong kỳ này.");
                return;
            }

            if (congno.HETNO == true)
            {
                ShowInfor("Công nợ đã được thu.");
                return;
            }

            var thutaiquaylist = _thutaiquayDao.GetList(txtSODB.Text.Trim(), int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()));
            decimal tongtiendathu = 0;
            if (thutaiquaylist.Count > 0)
                tongtiendathu = (decimal)thutaiquaylist.Sum(p => p.SOTIEN);
            if (congno.TONGTIEN < tongtiendathu + decimal .Parse(txtSoTien.Text .Trim()))
            {
                ShowInfor("Tổng số tiền vượt quá số tiền phải thu.");
                return;
            }
            
            THUTAIQUAY thutaiquay = new THUTAIQUAY();
            thutaiquay.IDKH = txtSODB.Text.Trim();
            thutaiquay.SOTIEN = decimal.Parse(txtSoTien.Text.Trim());
            thutaiquay.GHICHU = txtGhiChuCn.Text.Trim();
            thutaiquay.MANVNHAP = LoginInfo.MANV;
            thutaiquay.NAM = int.Parse(txtNAM.Text.Trim());
            thutaiquay.NGAYNHAP = DateTimeUtil.GetVietNamDate(txtNGAYCN.Text.Trim());
            thutaiquay.THANG = int.Parse(ddlTHANG.Text.Trim());

            var msg = _thutaiquayDao.Insert(thutaiquay);

            if (!msg.MsgType.Equals(MessageType.Error))
            {
                ClearContent();
                BindDataForGrid(gvList);
            }

            ShowInfor(ResourceLabel.Get(msg));
        }
       
        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            BindDataForGrid(gvList);
        }
        
        #region Đường phố
        private void BindDuongPho()
        {
            var list = _dpDao.GetList(ddlKHUVUC.SelectedValue, txtKeyword.Text.Trim());
            gvDuongPho.DataSource = list;
            gvDuongPho.PagerInforText = list.Count.ToString();
            gvDuongPho.DataBind();
        }

        protected void btnBrowseDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            upnlDuongPho.Update();
            RegisterStartupScript("jsUnblockDPDialog", "unblockDialog('divDuongPho');");
        }

        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            RegisterStartupScript("jsCloseWaitingDialog" + Guid.NewGuid(), "closeWaitingDialog();");
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
                        var dp = _dpDao.Get(res[0], res[1]);
                        if (dp != null)
                        {
                            SetControlValue(txtMADP.ClientID, dp.MADP);
                            SetControlValue(txtDUONGPHU.ClientID, dp.DUONGPHU);
                            CloseDialog("divDuongPho");
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
        #endregion
        #region Startup script registeration
      

        private void ShowError(string message, string controlId)
        {
            ((EOS)Page.Master).ShowError(message, controlId);
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

        #region Khách hàng
        protected void btnFilterKH_Click(object sender, EventArgs e)
        {
            BindKhachHang();
            upnlKhachHang.Update();
            CloseWaitingDialog();

            
        }

        protected void btnBrowseKH_Click(object sender, EventArgs e)
        {
            //TODO: do not bind khach hang first, wait for filter
            //BindKhachHang();
            //upnlKhachHang.Update();
            UnblockDialog("divKhachHang");
        }

        private void BindKhachHang()
        {
            var danhsach = _khDao.SearchKhachHang(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(),
                                                           txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
                                                           txtSONHA.Text.Trim(), txtTENDP.Text.Trim(),
                                                           ddlKHUVUC.SelectedValue.Trim());
            gvDanhSach.DataSource = danhsach;
            gvDanhSach.PagerInforText = danhsach.Count.ToString();
            cpeFilter.Collapsed = true;
            gvDanhSach.DataBind();
            tdDanhSach.Visible = true;

            upnlKhachHang.Update();
        }

        protected void gvDanhSach_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "SelectSODB":
                        var khachhang = _khDao.GetKhachHangFromMadb(id);
                        if (khachhang != null)
                        {
                            SetControlValue(txtSODB.ClientID, id);
                            HideDialog("divKhachHang");
                            CloseWaitingDialog();
                            txtSODB.Focus();
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvDanhSach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvDanhSach.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindKhachHang();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #endregion
        /*protected void gvDanhSach_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "SelectSODB":
                        var khachhang = _khDao.GetKhachHangFromMadb(id);
                        if (khachhang != null)
                        {
                            int thang = int.Parse(ddlTHANG.Text.Trim());
                            int nam = int.Parse(txtNAM.Text.Trim());
                            //Lấy số tiền
                            TIEUTHU congno = _cnDao.Get(khachhang.IDKH, thang, nam);

                            SetLabel(lblSoDb.ClientID, id);
                            SetControlValue(txtTHONGTINKH.ClientID, khachhang.TENKH);
                            SetControlValue(txtIDKH.ClientID, khachhang.IDKH);
                            if (congno != null)
                            {
                                //Trường hợp công nợ đã được thu hết nợ thì không cho thu nữa
                                if (congno.HETNO == true)
                                    SetControlValue(txtSoTien.ClientID, "0");
                                else
                                {
                                    //Trường hợp công nợ chưa được thu thì phải lấy tổng tiền phải thu trừ cho tổng tiền các lần thu tiền từ trước
                                    var thutaiquaylist = _thutaiquayDao.GetList(khachhang.IDKH, thang, nam);
                                    decimal tongtiendathu = 0;
                                    if (thutaiquaylist.Count > 0)
                                        tongtiendathu = (decimal) thutaiquaylist.Sum(p => p.SOTIEN);
                                    if (congno.TONGTIEN - tongtiendathu > 0)
                                        SetControlValue(txtSoTien.ClientID,
                                                        (congno.TONGTIEN - tongtiendathu).ToString());
                                    else
                                    {
                                        SetControlValue(txtSoTien.ClientID, "0");
                                    }
                                }
                            }
                            else
                            {
                                //Trường hợp không tồn tại công nợ
                                SetControlValue(txtSoTien.ClientID, "0");
                            }
                            CloseDialog("divKhachHang");
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message,
                                                 ex.StackTrace));
            }
        }*/

       
        

        
    }
}