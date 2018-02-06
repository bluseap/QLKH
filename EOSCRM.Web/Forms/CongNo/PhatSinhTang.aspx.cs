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


namespace EOSCRM.Web.Forms.CongNo
{
    public partial class PhatSinhTang : Authentication
    {
        private readonly PhatSinhTangDao _pstDao = new PhatSinhTangDao();


        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Authenticate(Functions.CN_PhatSinhTang, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_CN_PHATSINHTANG;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_CONGNO;
                header.TitlePage = Resources.Message.PAGE_CN_PHATSINHTANG;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
        }

        #region Startup script registeration
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
            txtNAM.Text = DateTime.Now.Year.ToString();
            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
        }

        private void BindDataForGrid()
        {

            try
            {
                /*
                int thang, nam;

                try
                {
                    nam = int.Parse(txtNAM.Text.Trim());
                    thang = int.Parse(ddlTHANG.SelectedValue);
                }
                catch
                {
                    gvList.DataSource = null;
                    gvList.PagerInforText = "0";
                    gvList.DataBind();
                    return;
                }

                var objList = _pstDao.GetList(nam, thang, txtSOPHIEU.Text.Trim());
                */
                var objList = _pstDao.GetList();

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();

                upnlGrid.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message,
                                                        ex.StackTrace));
            }
        }

        public bool IsDataValid()
        {
            //TODO: check validate data

            if (txtSODB.Text.Trim().Length != 7 && txtSODB.Text.Trim().Length != 8)
            {
                ShowInfor("Mã khách hàng không hợp lệ.");
                txtSODB.Focus();
                return false;
            }

            try
            {
                int.Parse(txtNAM.Text.Trim());
            }
            catch
            {
                ShowInfor("Năm không hợp lệ.");
                txtNAM.Focus();
                return false;
            }

            if(string.IsNullOrEmpty(txtSOPHIEU.Text.Trim()))
            {
                ShowInfor("Số phiếu không hợp lệ.");
                txtSOPHIEU.Focus();
                return false;
            }

            try
            {
                DateTimeUtil.GetVietNamDate(txtNGAYCN.Text.Trim());
            }
            catch
            {
                ShowInfor("Ngày tháng công nợ không đúng");
                txtNGAYCN.Focus();
                return false;
            }

            return true;
        }



        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                /*if (!HasPermission(Functions.CN_PhatSinhTang, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }*/

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<PHATSINHTANG>();
                    var listIds = strIds.Split(',');

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _pstDao.Get(ma)));

                    CloseWaitingDialog();

                    var msg = _pstDao.DeleteListCongNo(objs);
                    if ((msg != null))
                    {
                        if (msg.MsgType != MessageType.Error)
                        {
                            ShowInfor(ResourceLabel.Get(msg));

                            // Refresh grid view
                            BindDataForGrid();
                        }
                        else

                            ShowError(ResourceLabel.Get(msg));
                    }
                    else
                    {
                        ShowError(String.Format(Resources.Message.E_FAILED, "Xóa công nợ phát sinh tăng"));
                    }

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
                BindDataForGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void txtSODB_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSODB.Text.Trim()))
            {
                return;
            }
            if (!IsDataValid())
                return;

            var pst = _pstDao.Get(txtSODB.Text.Trim(), int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue));
            if (pst == null) {
                ShowError("Công nợ không tồn tại, xem lại thông tin mã khách hàng.");
                return;
            }

            if (pst.MANVNHAPCN != null && pst.SOPHIEUCN != null)
            {
                ShowError(string.Format("Công nợ phát sinh tăng đã được thu tại kỳ {0}/{1} với số phiếu {2}.", pst.THANG, pst.NAM, pst.SOPHIEUCN));
                return;
            }


            pst.NGAYNHAPCN = DateTime.Now;
            pst.NGAYCN = DateTimeUtil.GetVietNamDate(txtNGAYCN.Text.Trim());
            pst.MANVNHAPCN = LoginInfo.MANV;
            pst.GHICHUCN = txtGhiChuCn.Text.Trim();
            pst.SOPHIEUCN = txtSOPHIEU.Text.Trim();

            var msg = _pstDao.UpdateCongNo(pst);
            if (!msg.MsgType.Equals(MessageType.Error))
            {
                txtSODB.Text = "";
                txtGhiChuCn.Text = "";
                BindDataForGrid();
                txtSODB.Focus();
            }
            else
            {
                ShowInfor(ResourceLabel.Get(msg));
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            CloseWaitingDialog();
            BindDataForGrid();
        }

        protected void btnInPhieu_Click(object sender, EventArgs e)
        {
            /*
            int thang = 0;
            int nam = 0;
            try
            {
                DateTime kycn = DateTimeUtil.GetVietNamDate(txtNGAYCN.Text.Trim());
                thang = kycn.Month;
                nam = kycn.Year;
            }
            catch
            {
                CloseWaitingDialog();
                ShowInfor("Kỳ công nợ không đúng");
            }


            var dtDSINHOADON = new ReportClass().PhieuThu(thang, nam, txtSOPHIEU.Text.Trim()).Tables[0];

            if (dtDSINHOADON.Rows.Count > 0)
            {
                Session["DSPHIEUTHU"] = dtDSINHOADON;
                CloseWaitingDialog();
                Page.Response.Redirect(ResolveUrl("~") + "Forms/CongNo/BaoCao/rpPhieuThu.aspx");
            }
            else
            {
                CloseWaitingDialog();
                ShowInfor("Không tìm thấy dữ liệu để làm báo cáo");
            }
            */

            CloseWaitingDialog();
            ShowInfor("Phần in phiếu thu này sẽ được làm sau");
        }
    }
}