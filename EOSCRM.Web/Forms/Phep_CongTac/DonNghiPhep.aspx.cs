using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.UserControls;
using EOSCRM.Web.Shared;


namespace EOSCRM.Web.Forms.Phep_CongTac
{
    public partial class DonNghiPhep : Authentication
    {
        private readonly NghiPhepDao _npDao = new NghiPhepDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly PhongBanDao _pbDao = new PhongBanDao();
        private readonly CongViecDao _cvDao = new CongViecDao();

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
                Authenticate(Functions.NGHIPHEPCONGTAC_DonNghiPhep, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    //LoadStaticReferences();
                    LoadNN();
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_NPCT_DONNGHIPHEP;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_NPCT;
                header.TitlePage = Resources.Message.PAGE_NPCT_DONNGHIPHEP;
            }
            
            CommonFunc.SetPropertiesForGrid(gvNghiPhep);            
        }

        private void LoadNN()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var list = _nvDao.Get(b);
            var l_pb = _pbDao.Get(list.MAPB);
            var l_cv = _cvDao.Get(list.MACV);

            SetLabel(lblTENNV.ClientID, list.HOTEN);
            SetLabel(lblTENPHONG.ClientID, l_pb.TENPB);
            SetLabel(lblCONGVIEC.ClientID, l_cv.TENCV);
            SetLabel(lblSODT.ClientID, list.SDT);

            //txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            
        }

        private void BindGrid()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var list = _npDao.GetNV(b);
            gvNghiPhep.DataSource = list;
            //gvNghiPhep.PagerInforText = list.Count.ToString();
            gvNghiPhep.DataBind();

            upnlGrid.Update();
        }

        #region gv nghi phep
        protected void gvNghiPhep_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvNghiPhep.PageIndex = e.NewPageIndex;                
                BindGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvNghiPhep_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            /*
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "SelectKH":
                        var res = id.Split('-');
                        var khachhang = _khDao.Get(res[0]);
                        try
                        {
                            var ttsd = _khDao.GetTTSD(int.Parse(res[1]));
                            if (ttsd != null)
                            {
                                SetControlValue(txtGhiChu.ClientID, ttsd.GHICHU);
                            }
                        }
                        catch   {     }

                        if (khachhang != null)
                        {
                            BindStatus(khachhang);
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
            */
        }

        protected void gvNghiPhep_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvNghiPhep_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion

        protected void txtDenNgay_TextChanged(object sender, EventArgs e)
        {
            DateTime tungay = Convert.ToDateTime(txtTuNgay);
            DateTime denngay = Convert.ToDateTime(txtDenNgay);
            TimeSpan Time = denngay - tungay;
            int TongSoNgay = Time.Days;
            //lblNGAYNGHI.Text = TongSoNgay.ToString();
        }
        

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }        



    }
}
