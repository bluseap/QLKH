using System;
using System.Globalization;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using EOSCRM.Domain;

namespace EOSCRM.Web.Forms.GhiChiSo
{
    public partial class KhoiTaoGhiChiSo : Authentication
    {
        private readonly KhuVucDao kvDao = new KhuVucDao();
        private readonly GhiChiSoDao gcsDao = new GhiChiSoDao();
        private readonly ReportClass _rpClass = new ReportClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.GCS_KhoiTaoGhiChiSo, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_GCS_KHOITAOKYGCS;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_KHOITAOKYGCS;
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
            txtNAM.Text = DateTime.Now.Year.ToString();
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

            if(ddlKHUVUC.SelectedIndex == 0)
            {               

                // khoi tao ghi chi so cho tat ca cac khu vuc
                var listKhuVuc = kvDao.GetList();
                
                foreach(var kv in listKhuVuc)
                {
                    msg = gcsDao.KhoiTaoGhiChiSo(date, kv.MAKV);

                    _rpClass.DSKhoaSoDotIn(date, ddlKHUVUC.SelectedValue, "", "KTAOGCSD");

                    //_rpClass.UpHoNgheoHetHan(date, kv.MAKV);//update ho ngheo het han

                    if(msg.MsgType.Equals(MessageType.Error))
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

                _rpClass.BienKHNuoc("", ddlKHUVUC.SelectedValue, "", "", 1, 1, "UPDMUCTAMKHM");//up dinh muc tam khm 

                CloseWaitingDialog();
            }
            else
            {               

                msg = gcsDao.KhoiTaoGhiChiSo(date, ddlKHUVUC.SelectedValue);

                _rpClass.DSKhoaSoDotIn(date, ddlKHUVUC.SelectedValue, "", "KTAOGCSD");

                //_rpClass.UpHoNgheoHetHan(date, ddlKHUVUC.SelectedValue);//update ho ngheo het han

                _rpClass.BienKHNuoc("", ddlKHUVUC.SelectedValue, "", "", int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), "UPDMUCTAMKHM");//up dinh muc tam khm 

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
                var listKhuVuc = kvDao.GetList();

                foreach (var kv in listKhuVuc)
                {
                    msg = gcsDao.KhoiTaoGhiChiSoN(date, kv.MAKV);

                    _rpClass.UpHoNgheoHetHan(date, kv.MAKV);//update ho ngheo het han

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

                _rpClass.BienKHNuoc("", ddlKHUVUC.SelectedValue, "", "", 1, 1, "UPDMUCTAMKHM");//up dinh muc tam khm 

                CloseWaitingDialog();
            }
            else
            {
                msg = gcsDao.KhoiTaoGhiChiSoN(date, ddlKHUVUC.SelectedValue);

                _rpClass.UpHoNgheoHetHan(date, ddlKHUVUC.SelectedValue);//update ho ngheo het han

                _rpClass.BienKHNuoc("", ddlKHUVUC.SelectedValue, "", "", 1, 1, "UPDMUCTAMKHM");//up dinh muc tam khm 

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

        protected void ddlKHUVUC_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



    }
}
