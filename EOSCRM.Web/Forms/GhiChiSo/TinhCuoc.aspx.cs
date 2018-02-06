using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.GhiChiSo
{
    public partial class TinhCuoc : Authentication
    {
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DMDotInHDDao _dmdinDao = new DMDotInHDDao();
        private readonly DotInHDDao _dinDao = new DotInHDDao();
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly GhiChiSoDao gcsDao = new GhiChiSoDao();
        private readonly KhuVucDao kvDao = new KhuVucDao();
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        protected List<TINHCUOCPO> DataLOCKDOTIN
        {
            get { return Session["TINHCUOC_DATALOCKDOTIN"] as List<TINHCUOCPO>; }
            set { Session["TINHCUOC_DATALOCKDOTIN"] = value; }
        }

        protected List<TINHCUOC> DataSource
        {
            get { return Session["TINHCUOC_DATASOURCE"] as List<TINHCUOC>; }
            set { Session["TINHCUOC_DATASOURCE"] = value; }
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
                Authenticate(Functions.GCS_KhoaGhiChiSo, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_GCS_KHOAGHICHISO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_KHOAGHICHISO;
            }

            CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }

        private void LoadStaticReferences()
        {
            // load khu vuc
            var listKhuVuc = kvDao.GetList();

            ddlKHUVUC.Items.Clear();
            ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));

            foreach(var kv in listKhuVuc)
            {
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }

            if (ddlKHUVUC.SelectedValue == "%")
            {
                ddlDOTIN.Items.Clear();
                ddlDOTIN.Items.Add(new ListItem("Tất cả","%"));
            }
            else
            {
                var dotin = _dinDao.GetListKVNN(ddlKHUVUC.SelectedValue);

                ddlDOTIN.Items.Clear();
                foreach(var dot in dotin)
                {
                    ddlDOTIN.Items.Add(new ListItem(_dmdinDao.Get(dot.MADOTIN).TENDOTIN , dot.IDMADOTIN));
                }
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // load lo trinh tinh cuoc by khu vuc and duong pho
            try {
                DateTime.ParseExact(txtNAM.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
            }
            catch {
                CloseWaitingDialog();
                ShowError("Vui lòng chọn năm hợp lệ.", txtNAM.ClientID);
                return;
            }

            var kyghi = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);

            BindTinhCuocDataListDotIn(kyghi, ddlDOTIN.SelectedValue);

            BindTinhCuocDataList(kyghi);

            BindDotInHDList(kyghi, ddlKHUVUC.SelectedValue);

            CloseWaitingDialog();
        }

        private void BindDotInHDList(DateTime kyghi, string makvpo)
        {
            try
            {
                var makv = ddlKHUVUC.SelectedValue;
                var madp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim(), makv) == null
                               ? null
                               : txtMADP.Text.Trim();
                var duongphu = madp == null ? null : txtDUONGPHU.Text.Trim();

                var list = _rpClass.DSKhoaSoDotIn(kyghi, makvpo, "", "DSDOTIN");                

                dlDotInHD.DataSource = list;
                dlDotInHD.DataBind();

                upgvDotInThuHo.Update();
            }
            catch { }
        }

        private void BindTinhCuocDataListDotIn(DateTime kyghi, string iddotin)
        {
            var makv = ddlKHUVUC.SelectedValue;
            var madp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim(), makv) == null
                           ? null
                           : txtMADP.Text.Trim();
            var duongphu = madp == null ? null : txtDUONGPHU.Text.Trim();

            var list = gcsDao.GetListForTinhCuocDotIn(kyghi, makv, madp, duongphu, iddotin);
            DataSource = list;
            dlTinhCuocDotIn.DataSource = list;
            dlTinhCuocDotIn.DataBind();

            upgvDOTIN.Update();
        }

        private void BindTinhCuocDataList(DateTime kyghi)
        {
            //var makv = ddlKHUVUC.SelectedValue;
            //var madp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim(), makv) == null
            //               ? null
            //               : txtMADP.Text.Trim();
            //var duongphu = madp == null ? null : txtDUONGPHU.Text.Trim();

            //var list = gcsDao.GetListForTinhCuoc(kyghi, makv, madp, duongphu);
            //DataSource = list;
            //dlTinhCuoc.DataSource = list;
            //dlTinhCuoc.DataBind();

            //upnlGrid.Update();
        }

        protected void btnTinhCuoc_Click(object sender, EventArgs e)
        {
            var kyghidotin = new DateTime(Int32.Parse(txtNAM.Text.Trim()), Int32.Parse(ddlTHANG.SelectedValue), 1);
            for (var i = 0; i < dlDotInHD.Items.Count; i++)
            {
                var item = dlDotInHD.Items[i];
                //var dataItem = DataSource[item.ItemIndex];
                var cb = item.FindControl("chkEnabledDOTIN") as HtmlInputCheckBox;

                if (cb == null) continue;
                if (cb.Attributes["disabled"] == null && cb.Checked)
                {
                    //updateList.Add(dataItem);
                    //string a = item.ItemIndex.ToString();
                    //ShowInfor(a);
                    var msgdi = _gcspoDao.LockDotInHD(kyghidotin, ddlKHUVUC.SelectedValue);
                }
            }

            BindDotInHDList(kyghidotin, ddlKHUVUC.SelectedValue);
            upgvDotInThuHo.Update();


            var updateList = new List<TINHCUOC>();

            //for (var i = 0; i < dlTinhCuoc.Items.Count; i++)
            for (var i = 0; i < dlTinhCuocDotIn.Items.Count; i++)
            {
                //var item = dlTinhCuoc.Items[i];
                var item = dlTinhCuocDotIn.Items[i];
                var dataItem = DataSource[item.ItemIndex];
                //var cb = item.FindControl("chkEnabled") as HtmlInputCheckBox;
                var cb = item.FindControl("chkEnabledDI") as HtmlInputCheckBox;

                if (cb == null) continue;

                if (cb.Attributes["disabled"] == null && cb.Checked) 
                    updateList.Add(dataItem);
            }

            //TODO: tinh cuoc's here
            var msg = gcsDao.TinhCuoc(updateList);

            if (msg != null && msg.MsgType != MessageType.Error) 
            {
                try
                {
                    DateTime.ParseExact(txtNAM.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
                }
                catch
                {
                    CloseWaitingDialog();
                    ShowError("Vui lòng chọn năm hợp lệ.", txtNAM.ClientID);
                    return;
                }

                var kyghi = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);

                BindTinhCuocDataListDotIn(kyghi, ddlDOTIN.SelectedValue);
                //BindTinhCuocDataList(kyghi);

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = "999999",
                    IPAddress = CommonFunc.GetLanIPAddressM(),
                    MANV = LoginInfo.MANV,
                    UserAgent = CommonFunc.GetComputerName(),
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.I.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = CHUCNANGKYDUYET.KH05.ToString(),
                    MOTA = "Khoá sổ kỳ ghi chỉ số"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                CloseWaitingDialog();
                ShowInfor("Cập nhật khóa ghi chỉ số thành công.");
            }
            else 
            {
                ShowInfor("Cập nhật khóa ghi chỉ số không thành công.");
            }
        }

        //protected void dlTinhCuoc_ItemDataBound(object sender, DataListItemEventArgs e)
        //{
        //    if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
        //        return;

        //    var item = (TINHCUOC)e.Item.DataItem;

        //    var cb = e.Item.FindControl("chkEnabled") as HtmlInputCheckBox;
        //    var enabled = e.Item.FindControl("imgUnlock") as ImageButton;
            
        //    if (cb == null || enabled == null) return;
        //    if (item.ENABLED == false)
        //        cb.Attributes.Add("disabled", "disabled");
        //    enabled.Visible = item.SHOWUNLOCKED;
        //}

        //protected void dlTinhCuoc_ItemCommand(object source, DataListCommandEventArgs e)
        //{
        //    try {
        //        var id = e.CommandArgument.ToString();

        //        switch (e.CommandName) {
        //                //Can't use just Edit and Delete or need to bypass RowEditing and Deleting
        //            case "Unlock":
        //                CloseWaitingDialog();
        //                var res = id.Split('-');
        //                var duongphu = res[1].Equals("NULL") ? "" : res[1];
        //                var dp = dpDao.Get(res[0], duongphu, res[2]);
        //                var kv = kvDao.Get(res[2]);
        //                var kyghi = new DateTime(Int32.Parse(res[4]), Int32.Parse(res[3]), 1);
        //                if(dp == null || kv == null)
        //                {
        //                    CloseWaitingDialog();
        //                    return;
        //                }
        //                var msg = gcsDao.UnlockTinhCuoc(kyghi, dp);

        //                CloseWaitingDialog();

        //                if(msg != null) {
        //                    if (msg.MsgType != MessageType.Error) {
        //                        ShowInfor(ResourceLabel.Get(msg));
        //                        BindTinhCuocDataList(kyghi);
        //                    }
        //                    else
        //                        ShowError(ResourceLabel.Get(msg));
        //                } 
        //                break;
        //        }
        //    }
        //    catch (Exception ex) {
        //        DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
        //    }
        //}

        protected void dlTinhCuocDotIn_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var item = (TINHCUOC)e.Item.DataItem;

            var cb = e.Item.FindControl("chkEnabledDI") as HtmlInputCheckBox;
            var enabled = e.Item.FindControl("imgUnlock") as ImageButton;

            if (cb == null || enabled == null) return;
            if (item.ENABLED == false)
                cb.Attributes.Add("disabled", "disabled");
            enabled.Visible = item.SHOWUNLOCKED;
        }

        protected void dlTinhCuocDotIn_ItemCommand(object source, DataListCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    //Can't use just Edit and Delete or need to bypass RowEditing and Deleting
                    case "UnlockDI":
                        CloseWaitingDialog();
                        var res = id.Split('-');
                        var duongphu = res[1].Equals("NULL") ? "" : res[1];
                        var dp = dpDao.Get(res[0], duongphu, res[2]);
                        var kv = kvDao.Get(res[2]);
                        var kyghi = new DateTime(Int32.Parse(res[4]), Int32.Parse(res[3]), 1);
                        if (dp == null || kv == null)
                        {
                            CloseWaitingDialog();
                            return;
                        }
                        var msg = gcsDao.UnlockTinhCuoc(kyghi, dp);

                        BindTinhCuocDataListDotIn(kyghi, ddlDOTIN.SelectedValue);
                        CloseWaitingDialog();

                        if (msg != null)
                        {
                            if (msg.MsgType != MessageType.Error)
                            {
                                ShowInfor(ResourceLabel.Get(msg));
                                BindTinhCuocDataList(kyghi);
                            }
                            else
                                ShowError(ResourceLabel.Get(msg));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        #region Đường phố
        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            CloseWaitingDialog();
        }

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

        protected void ddlKHUVUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlKHUVUC.SelectedValue == "%")
            {
                ddlDOTIN.Items.Clear();
                ddlDOTIN.Items.Add(new ListItem("Tất cả", "%"));
            }
            else
            {
                var dotin = _dinDao.GetListKVNN(ddlKHUVUC.SelectedValue);

                ddlDOTIN.Items.Clear();
                foreach (var dot in dotin)
                {
                    ddlDOTIN.Items.Add(new ListItem(_dmdinDao.Get(dot.MADOTIN).TENDOTIN, dot.IDMADOTIN));
                }
            }

            upnlTinhCuoc.Update();
        }

        protected void dlDotInHD_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var cb = e.Item.FindControl("chkEnabledDOTIN") as HtmlInputCheckBox;
            var enabled = e.Item.FindControl("imgUnlock") as ImageButton;

            if (cb == null || enabled == null)
                return;          

            System.Data.DataRowView row = (System.Data.DataRowView)e.Item.DataItem;
            string messageSubject1 = row["MADP"].ToString();
            string messageSubject2 = row["LOCKTINHCUOC"].ToString();          

            if (messageSubject2 == "False")
            {
                cb.Attributes.Add("enabled", "enabled");
                enabled.Visible = false;
            }
            else
            {
                cb.Attributes.Add("disabled", "disabled");
                enabled.Visible = true;
            }           
        }

        protected void dlDotInHD_ItemCommand(object source, DataListCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    //Can't use just Edit and Delete or need to bypass RowEditing and Deleting
                    case "Unlock":
                        CloseWaitingDialog();
                        var res = id.Split('-');
                        
                        var kyghi = new DateTime(Int32.Parse(res[3]), Int32.Parse(res[2]), 1);                        

                        var msg = _gcspoDao.UnlockDotInHD(res[0].ToString(), kyghi, ddlKHUVUC.SelectedValue);

                        CloseWaitingDialog();

                        if (msg != null)
                        {
                            if (msg.MsgType != MessageType.Error)
                            {
                                ShowInfor(ResourceLabel.Get(msg));
                                BindDotInHDList(kyghi, ddlKHUVUC.SelectedValue);
                            }
                            else
                                ShowError(ResourceLabel.Get(msg));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

    }
}
