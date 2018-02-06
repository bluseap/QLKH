using System;
using System.Globalization;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using EOSCRM.Domain;

namespace EOSCRM.Web.Forms.GhiChiSo.Power
{
    public partial class KhoiTaoChiSoPo : Authentication
    {
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.GCS_KhoiTaoGhiChiSoPo, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                    if (loginInfo == null) return;
                    string b = loginInfo.Username;

                    if (b == "nguyen")
                    {
                        btnNguyen.Visible = true;

                    }
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
            Page.Title = Resources.Message.TITLE_GCS_KHOITAOKYGCSPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_KHOITAOKYGCSPO;
            }
        }

        #region Startup script registeration

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

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }
        #endregion

        private void LoadStaticReferences()
        {
            // load khu vuc
            var listKhuVuc = _kvpoDao.GetList();

            ddlKHUVUC.Items.Clear();
            ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));

            foreach (var kv in listKhuVuc)
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));

            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString();
        }

        protected void btnKhoiTao_Click(object sender, EventArgs e)
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

            var date = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);

            Message msg = null;

            if (ddlKHUVUC.SelectedIndex == 0)
            {
                // khoi tao ghi chi so cho tat ca cac khu vuc
                var listKhuVuc = _kvpoDao.GetList();

                foreach (var kv in listKhuVuc)
                {
                    msg = _gcspoDao.KhoiTaoGhiChiSo(date, kv.MAKVPO);

                    _rpClass.DSKhoaSoDotIn(date, ddlKHUVUC.SelectedValue, "", "KTAOGCSD");

                    if (msg.MsgType.Equals(MessageType.Error))
                    {
                        CloseWaitingDialog();
                        ShowError(ResourceLabel.Get(msg));
                        return;
                    }
                }

                if (msg != null && !msg.MsgType.Equals(MessageType.Error))
                {
                    CloseWaitingDialog();
                    ShowInfor(ResourceLabel.Get(msg));
                }

                CloseWaitingDialog();
            }
            else
            {
                msg = _gcspoDao.KhoiTaoGhiChiSo(date, ddlKHUVUC.SelectedValue);

                _rpClass.DSKhoaSoDotIn(date, ddlKHUVUC.SelectedValue, "", "KTAOGCSD");

                CloseWaitingDialog();

                if (msg.MsgType.Equals(MessageType.Error))
                {
                    ShowError(ResourceLabel.Get(msg));
                }
                else
                {
                    ShowInfor(ResourceLabel.Get(msg));
                }
            }
        }

        protected void btnNguyen_Click(object sender, EventArgs e)
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

            var date = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);

            Message msg = null;

            if (ddlKHUVUC.SelectedIndex == 0)
            {
                // khoi tao ghi chi so cho tat ca cac khu vuc
                var listKhuVuc = _kvpoDao.GetList();

                foreach (var kv in listKhuVuc)
                {
                    msg = _gcspoDao.KhoiTaoGhiChiSoN(date, kv.MAKVPO);

                    if (msg.MsgType.Equals(MessageType.Error))
                    {
                        CloseWaitingDialog();
                        ShowError(ResourceLabel.Get(msg));
                        return;
                    }
                }

                if (msg != null && !msg.MsgType.Equals(MessageType.Error))
                {
                    CloseWaitingDialog();
                    ShowInfor(ResourceLabel.Get(msg));
                }

                CloseWaitingDialog();
            }
            else
            {
                msg = _gcspoDao.KhoiTaoGhiChiSoN(date, ddlKHUVUC.SelectedValue);

                CloseWaitingDialog();

                if (msg.MsgType.Equals(MessageType.Error))
                {
                    ShowError(ResourceLabel.Get(msg));
                }
                else
                {
                    ShowInfor(ResourceLabel.Get(msg));
                }
            }
        }
    }
}