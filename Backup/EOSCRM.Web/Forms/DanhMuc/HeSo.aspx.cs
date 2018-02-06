using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Util ;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;


namespace EOSCRM.Web.Forms.DanhMuc
{
    public partial class HeSo : Authentication
    {
        private readonly HeSoDao _objDao = new HeSoDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.DM_HeSo, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
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
            Page.Title = Resources.Message.TITLE_DM_HESOTK;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_HESOTK;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
        }

        #region Startup script registeration
        private void ShowError(string message)
        {
            ((EOS)Page.Master).ShowError(message);
        }

        private void ShowError(string message, string controlId)
        {
            ((EOS)Page.Master).ShowError(message, controlId);
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



        private void BindDataForGrid()
        {
            var objList = _objDao.GetList();

            gvList.DataSource = objList;
            gvList.PagerInforText = objList.Count.ToString();
            gvList.DataBind();
        }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Authenticate
            if (!HasPermission(Functions.DM_HeSo, Permission.Update))
            {
                CloseWaitingDialog();
                ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                return;
            }
            
            var objList = new List<HESO>();

            foreach(GridViewRow row in gvList.Rows)
            {
                decimal? giatri = null;
                DateTime? ngayad = null;

                var hdMAHS = (HiddenField) row.FindControl("hdfMAHS");
                var txtTENHS = (TextBox) row.FindControl("txtTENHS");
                var txtGIATRI = (TextBox) row.FindControl("txtGIATRI");
                var txtNGAYAD = (TextBox) row.FindControl("txtNGAYAD");


                if (hdMAHS == null || txtTENHS == null || txtGIATRI == null || txtNGAYAD == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                if(string.IsNullOrEmpty(txtTENHS.Text.Trim()))
                {
                    CloseWaitingDialog();
                    ShowError("Tên hệ số không hợp lệ", txtTENHS.ClientID);
                    return;
                }

                try
                {
                    giatri = decimal.Parse(txtGIATRI.Text.Trim());
                }
                catch
                {
                    CloseWaitingDialog();
                    ShowError("Giá trị hệ số không hợp lệ", txtGIATRI.ClientID);
                    return;
                }

                try
                {
                    ngayad = DateTimeUtil.GetVietNamDate(txtNGAYAD.Text.Trim());
                }
                catch
                {
                    CloseWaitingDialog();
                    ShowError("Ngày áp dụng không hợp lệ", txtNGAYAD.ClientID);
                    return;
                }

                var obj = new HESO
                              {
                                  MAHS = hdMAHS.Value,
                                  TENHS = txtTENHS.Text.Trim(),
                                  GIATRI = giatri,
                                  NGAYAD = ngayad
                              };
                objList.Add(obj);
            }

            var msg = _objDao.UpdateList(objList, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(),
                                         LoginInfo.MANV);
            CloseWaitingDialog();

            if (msg == null) {return;}

            if (msg.MsgType != MessageType.Error)
            {
                ShowInfor(ResourceLabel.Get(msg));

                // Refresh grid view
                BindDataForGrid();
            }
            else
            {
                ShowError(ResourceLabel.Get(msg));
            }


            /*
            var info = DVTObj;
            if (info == null)
            {
                CloseWaitingDialog();
                return;
            }

            Message msg;
            
            // insert new
            if (UpdateMode == Mode.Create)
            {
                Authenticate(Functions.DM_DonViTinh, Permission.Insert);

                var tontai = _objDao.Get(txtMADVT.Text.Trim());
                if (tontai != null)
                {
                    CloseWaitingDialog();
                    ShowError("Mã đơn vị tính đã tồn tại", txtMADVT.ClientID);
                    return;
                }

                msg = _objDao.Insert(info);
            }
            // update
            else
            {
                Authenticate(Functions.DM_DonViTinh, Permission.Update);
                msg = _objDao.Update(info);
            }

            CloseWaitingDialog();

            if (msg == null) return;

            if (msg.MsgType != MessageType.Error)
            {
                ShowInfor(ResourceLabel.Get(msg));

                ClearForm();

                // Refresh grid view
                BindDataForGrid();

                upnlGrid.Update();
            }
            else
            {
                ShowError(ResourceLabel.Get(msg));
            }*/
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            BindDataForGrid();
            CloseWaitingDialog();
        }



        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var txtTENHS = e.Row.FindControl("txtTENHS") as TextBox;
            if (txtTENHS == null) return;

            var txtGIATRI = e.Row.FindControl("txtGIATRI") as TextBox;
            if (txtGIATRI == null) return;

            var txtNGAYAD = e.Row.FindControl("txtNGAYAD") as TextBox;
            if (txtNGAYAD == null) return;
            
            var source = gvList.DataSource as List<HESO>;
            if (source == null) return;

            var hs = source[e.Row.RowIndex + gvList.PageSize * gvList.PageIndex];
            if(hs == null) return;

            txtTENHS.Text = hs.TENHS;
            txtGIATRI.Text = hs.GIATRI.HasValue ? hs.GIATRI.Value.ToString("") : "";
            txtNGAYAD.Text = hs.NGAYAD.HasValue ? hs.NGAYAD.Value.ToString("dd/MM/yyyy") : "";
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

        
    }
}