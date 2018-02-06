using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Util ;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;


namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class PhatSinhTang : Authentication
    {
        private readonly PhatSinhTangDao _pstDao = new PhatSinhTangDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly DuongPhoDao _dpDao = new DuongPhoDao();
        private readonly MucDichSuDungDao _mdsdDao = new MucDichSuDungDao();


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

        private PHATSINHTANG PSTObj
        {
            get
            {
                if (!ValidateData())
                    return null;

                var pst = _pstDao.Get(txtMAPS.Text.Trim()) ?? new PHATSINHTANG();

                pst.MAPS = txtMAPS.Text.Trim();
               pst.THANG = int.Parse(cboTHANG.Text .Trim());
               pst.NAM = int.Parse(txtNAM.Text .Trim());
               pst.CSC = int.Parse(txtCSD.Text.Trim());
               pst.CSD = int.Parse(txtCSD.Text.Trim());
               pst.TENKH = txtHOTEN.Text.Trim();
               pst.DIACHI = txtDIACHI.Text .Trim();
               pst.DONGIA = decimal.Parse(txtDonGia.Text.Trim());
               pst.MADP = ddlDuongPho.Text.Trim();
               pst.DUONGPHU = "";
               pst.ISTHUE = false;
               pst.MST = txtMST.Text.Trim();
               pst.MAKV = ddlKHUVUC.Text .Trim();
               pst.MAMDSD = ddlMDSD.Text .Trim();
               pst.KLTIEUTHU = int.Parse(txtKLTieuThu.Text .Trim());
               pst.TIENNUOC = decimal.Parse(txtThanhTien.Text .Trim());
               pst.VAT = decimal.Parse(txtVAT.Text.Trim());
               pst.TIENTHUE = decimal.Parse(txtTienThue.Text.Trim());
               pst.PHI = decimal.Parse(txtTienPhi.Text .Trim());
               pst.TIENPHI = decimal.Parse(txtTienPhi.Text .Trim());
               pst.TONGTIEN = decimal.Parse(txtTongTien.Text.Trim());
               pst.LYDO = txtLyDo.Text .Trim();
               pst.MANVNHAP = LoginInfo.MANV;
                pst.NGAYNHAP = DateTime.Now;

                return pst;
            }
        }


        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Authenticate(Functions.KH_PhatSinhTang, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    // Bind data for grid view
                    LoadStaticReferences();

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
            Page.Title = Resources.Message.TITLE_KH_PHATSINHTANG;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_PHATSINHTANG;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
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

        private void FocusAndSelect(string controlId)
        {
            ((EOS)Page.Master).FocusAndSelect(controlId);
        }
        #endregion


        
        private void BindDataForGrid()
        {
            try
            {
                var objList = _pstDao.GetList();

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        
        private void LoadStaticReferences()
        {
            // bind khu vuc
            var kvList = _kvDao.GetList();
            ddlKHUVUC.Items.Clear();
            ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
            foreach(var kv in kvList)
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));

            ClearForm();

            var mdsdList = _mdsdDao.GetList();
            ddlMDSD.DataSource = mdsdList;
            ddlMDSD.DataTextField = "TENMDSD";
            ddlMDSD.DataValueField = "MAMDSD";
            ddlMDSD.DataBind();

            txtNAM.Text = DateTime.Now.Year.ToString();
            cboTHANG.SelectedIndex = DateTime.Now.Month - 1;
        }

        private void LoadDynamicReferences()
        {
            var dpList = _dpDao.GetList(ddlKHUVUC.SelectedValue);
            ddlDuongPho.Items.Clear();
            ddlDuongPho.Items.Add(new ListItem("Tất cả", "%"));

            foreach (var dp in dpList)
                ddlDuongPho.Items.Add(new ListItem(dp.TENDP, dp.MADP));
        }

        public bool ValidateData()
        {
            //TODO: check validate data
            try
            {
                int.Parse(cboTHANG.Text.Trim());
                int.Parse(txtNAM.Text.Trim());
                int.Parse(txtCSD.Text.Trim());
                int.Parse(txtCSD.Text.Trim());
                decimal.Parse(txtDonGia.Text.Trim());
                int.Parse(txtKLTieuThu.Text.Trim());
                decimal.Parse(txtThanhTien.Text.Trim());
                decimal.Parse(txtVAT.Text.Trim());
                decimal.Parse(txtTienThue.Text.Trim());
                decimal.Parse(txtTienPhi.Text.Trim());
                decimal.Parse(txtTienPhi.Text.Trim());
                decimal.Parse(txtTongTien.Text.Trim());
            }
            catch
            {
                ShowError("Các thông số nhập vào không đúng, vui lòng xem lại.", txtHOTEN.ClientID);
                return false;
            }

            if (string.IsNullOrEmpty(txtHOTEN.Text.Trim()))
            {
                ShowError("Vui lòng nhập tên khách hàng.", txtHOTEN.ClientID);
                return false;
            }

            if(ddlDuongPho.SelectedIndex == 0)
            {
                ShowError("Vui lòng chọn đường phố.");
                return false;
            }

            if (ddlKHUVUC.SelectedIndex == 0)
            {
                ShowError("Vui lòng chọn khu vực.");
                return false;
            }

            return true;

           
        }
         
        private void ClearForm()
        {
            txtMAPS.Text = _pstDao.NewId();
            txtMAPS.ReadOnly = false;

            ddlKHUVUC.SelectedIndex = 0;

            txtHOTEN.Text = "";
            txtDIACHI.Text = "";

            ddlMDSD.SelectedIndex = 0;
            txtMST.Text = "";

            txtCSC.Text = @"0";
            txtCSD.Text = @"0";
            txtKLTieuThu.Text = @"0";
            txtDonGia.Text = @"0";
            txtThanhTien.Text = @"0";
            txtVAT.Text = @"0";
            txtTienThue.Text = @"0";
            txtPhi.Text = @"0";
            txtTienPhi.Text = @"0";
            txtTongTien.Text = @"0";
            txtLyDo.Text = @"Phát sinh tăng";
            
            LoadDynamicReferences();
        }

        

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvList.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindDataForGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindPSTToForm(PHATSINHTANG pst)
        {
            SetReadonly(txtMAPS.ClientID, true);
            SetControlValue(txtMAPS.ClientID, pst.MAPS);

            cboTHANG.SelectedIndex = pst.THANG.Value - 1;
            SetControlValue(txtNAM.ClientID, pst.NAM.ToString());

            var kv = ddlKHUVUC.Items.FindByValue(pst.MAKV);
            if (kv != null)
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);

            var dp = ddlDuongPho.Items.FindByValue(pst.MADP);
            if (dp != null)
                ddlDuongPho.SelectedIndex = ddlDuongPho.Items.IndexOf(dp);

            var mdsd = ddlMDSD.Items.FindByValue(pst.MAMDSD);
            if (mdsd != null)
                ddlMDSD.SelectedIndex = ddlMDSD.Items.IndexOf(mdsd);
            
            SetControlValue(txtHOTEN.ClientID, pst.TENKH);
            SetControlValue(txtDIACHI.ClientID, pst.DIACHI);
            SetControlValue(txtMST.ClientID, pst.MST);

            SetControlValue(txtCSD.ClientID, pst.CSD.Value.ToString());
            SetControlValue(txtCSC.ClientID, pst.CSC.Value.ToString());
            SetControlValue(txtKLTieuThu.ClientID, pst.KLTIEUTHU.Value.ToString());
            SetControlValue(txtDonGia.ClientID, pst.DONGIA.Value.ToString());
            SetControlValue(txtThanhTien.ClientID, pst.TIENNUOC.Value.ToString());
            SetControlValue(txtVAT.ClientID, pst.VAT.Value.ToString());
            SetControlValue(txtTienThue.ClientID, pst.TIENTHUE.Value.ToString());
            SetControlValue(txtPhi.ClientID, pst.PHI.Value.ToString());
            SetControlValue(txtTienPhi.ClientID, pst.TIENPHI.Value.ToString());
            SetControlValue(txtTongTien.ClientID, pst.TONGTIEN.Value.ToString());
            SetControlValue(txtLyDo.ClientID, pst.LYDO);
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var pst = _pstDao.Get(id);
                        UpdateMode = Mode.Update;
                        if(pst != null)
                        {
                            BindPSTToForm(pst);
                        }
                        upnlInfor.Update();
                        UpdateMode = Mode.Update;
                        CloseWaitingDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void ddlKHUVUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDynamicReferences();
            CloseWaitingDialog();
        }



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            UpdateMode = Mode.Create;
            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var pst = PSTObj;

            if (pst == null) {
                CloseWaitingDialog();
                return;
            }

            Message msg;

            if(UpdateMode == Mode.Create)
            {
                //Authenticate(Functions.KH_PhatSinhTang, Permission.Insert);

                var existed = _pstDao.Get(pst.MAPS);
                if(existed != null)
                {
                    CloseWaitingDialog();
                    ShowError("Mã phát sinh tăng đã tồn tại. Vui lòng chọn mã khác.", txtMAPS.ClientID);
                    return;
                }

                msg = _pstDao.Insert(pst);
            }
            else
            {
                //Authenticate(Functions.KH_PhatSinhTang, Permission.Update);

                msg = _pstDao.Update(pst);
            }

            CloseWaitingDialog();

            if (msg != null)
            {
                if (msg.MsgType != MessageType.Error)
                {
                    //CloseWaitingDialog();
                    ShowInfor(ResourceLabel.Get(msg));

                    ClearForm();
                    UpdateMode = Mode.Create;

                    BindDataForGrid();
                    upnlGrid.Update();
                }
                else
                {
                    //CloseWaitingDialog();
                    ShowError(ResourceLabel.Get(msg));
                }
            }
            else
            {
                //CloseWaitingDialog();
                var task = UpdateMode == Mode.Create ? "Thêm mới khách hàng phát sinh tăng" : "Cập nhật khách hàng phát sinh tăng";
                ShowError(String.Format(Resources.Message.E_FAILED, task));
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindDataForGrid();
            CloseWaitingDialog();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                /*if (!HasPermission(Functions.KH_PhatSinhTang, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }*/

                DeleteList();
                ClearForm();
                BindDataForGrid();
                upnlGrid.Update();

                CloseWaitingDialog();

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void DeleteList()
        {
            try
            {
                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    //TODO: check relation before update list

                    var objs = new List<PHATSINHTANG>();
                    var listIds = strIds.Split(',');

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _pstDao.Get(ma)));

                    if (objs.Count > 0)
                    {
                        var msg = _pstDao.DeleteList(objs);
                        if (msg != null)
                        {
                            switch (msg.MsgType)
                            {
                                case MessageType.Error:
                                    ShowError(ResourceLabel.Get(msg));
                                    break;

                                case MessageType.Info:
                                    ShowInfor(ResourceLabel.Get(msg));
                                    break;

                                case MessageType.Warning:
                                    ShowWarning(ResourceLabel.Get(msg));
                                    break;
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

        protected void linkBtnHiddenKhoiLuong_Click(object sender, EventArgs e)
        {
            TinhTien();
            FocusAndSelect(txtDonGia.ClientID);
        }

        private void TinhTien()
        {
            var kl = 0;
            var dg = 0;
            var vat = 0;
            var phi = 0;

            // ReSharper disable EmptyGeneralCatchClause
            try { kl = int.Parse(txtKLTieuThu.Text.Trim()); } catch { }
            try { dg = int.Parse(txtDonGia.Text.Trim()); } catch { }
            try { vat = int.Parse(txtVAT.Text.Trim()); } catch { }
            try { phi = int.Parse(txtPhi.Text.Trim()); } catch { }
            // ReSharper restore EmptyGeneralCatchClause
            dg = dg > 0 ? dg : 0;
            kl = kl > 0 ? kl : 0;
            vat = vat > 0 ? vat : 0;
            phi = phi > 0 ? phi : 0;


            var tiennuoc = Math.Round((decimal)kl * dg * 100 / (100 + vat), 2, MidpointRounding.AwayFromZero);
            var thue = (decimal)kl * dg - tiennuoc;
            var tienphi = kl * phi;
            var tongtien = thue + tiennuoc + tienphi;

            txtThanhTien.Text = tiennuoc.ToString();
            txtTienThue.Text = thue.ToString();
            txtTienPhi.Text = tienphi.ToString();
            txtTongTien.Text = tongtien.ToString();

            upnlInfor.Update();
        }

        protected void linkBtnHiddenDonGia_Click(object sender, EventArgs e)
        {
            TinhTien();
            FocusAndSelect(txtThanhTien.ClientID);
        }

        protected void linkBtnHiddenChiSoCuoi_Click(object sender, EventArgs e)
        {
            TinhTien(); 
            FocusAndSelect(txtKLTieuThu.ClientID);
        }

        protected void linkBtnHiddenChiSoDau_Click(object sender, EventArgs e)
        {
            TinhTien();
            FocusAndSelect(txtCSC.ClientID);
        }

        protected void linkBtnHiddenVAT_Click(object sender, EventArgs e)
        {
            TinhTien();
            FocusAndSelect(txtPhi.ClientID);
        }

        protected void linkBtnHiddenPhi_Click(object sender, EventArgs e)
        {
            TinhTien();
            FocusAndSelect(txtTongTien.ClientID);
        }
    }
}