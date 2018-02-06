using System;
using System.Globalization;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.GhiChiSo
{
    public partial class KhoiTaoGhiChiSoBoSung : Authentication
    {
        private readonly KhuVucDao kvDao = new KhuVucDao();
        private readonly GhiChiSoDao gcsDao = new GhiChiSoDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.GCS_KhoiTaoGhiChiSoBoSung, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_GCS_KHOITAOKYGCSBOSUNG;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_KHOITAOKYGCSBOSUNG;
            }
        }

        #region Startup script registeration

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

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        private void LoadStaticReferences()
        {
            // load khu vuc
            var listKhuVuc = kvDao.GetList();

            ddlKHUVUC.Items.Clear();
            ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));

            foreach(var kv in listKhuVuc)
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));

            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
        }

        protected void btnKhoiTao_Click(object sender, EventArgs e)
        {
            try {
                DateTime.ParseExact(txtNAM.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
            }
            catch {
                CloseWaitingDialog();
                ShowError("Vui lòng chọn năm hợp lệ.", txtNAM.ClientID);
                return;
            }

            var date = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);

            Message msg = null;

            if (ddlKHUVUC.SelectedIndex == 0)
            {
                // khoi tao ghi chi so cho tat ca cac khu vuc
                var listKhuVuc = kvDao.GetList();

                foreach (var kv in listKhuVuc)
                {
                    msg = gcsDao.KhoiTaoGhiChiSoBoSung(date, kv.MAKV);

                    if (msg.MsgType.Equals(MessageType.Error))
                    {
                        CloseWaitingDialog();
                        ShowError(ResourceLabel.Get(msg));
                        return;
                    }
                }

                CloseWaitingDialog();

                if (msg != null && !msg.MsgType.Equals(MessageType.Error))
                {
                    ShowInfor(ResourceLabel.Get(msg));
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }
            }
            else
            {
                msg = gcsDao.KhoiTaoGhiChiSoBoSung(date, ddlKHUVUC.SelectedValue);

                CloseWaitingDialog();

                if (msg != null && !msg.MsgType.Equals(MessageType.Error))
                {
                    ShowInfor(ResourceLabel.Get(msg));
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }
            }
        }
    }
}
