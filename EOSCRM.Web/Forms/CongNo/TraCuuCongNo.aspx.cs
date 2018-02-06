using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using EOSCRM.Controls;
using EOSCRM.Util ;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.CongNo
{
    public partial class TraCuuCongNo : Authentication
    {
        private readonly CongNoDao _cnDao = new CongNoDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

        #region Properties
        protected int? THANGFILTER
        {
            get { return Session["NHAPCONGNO_THANGFILTER"] != null ? Int32.Parse(Session["NHAPCONGNO_THANGFILTER"].ToString()) : (int?)null; }
            set
            {
                Session["NHAPCONGNO_THANGFILTER"] = value;
                ddlTHANGFILTER.SelectedIndex = value.HasValue ? value.Value : 0;
            }
        }

        protected int? NAMFILTER
        {
            get { return Session["NHAPCONGNO_NAMFILTER"] != null ? Int32.Parse(Session["NHAPCONGNO_NAMFILTER"].ToString()) : (int?)null; }
            set 
            { 
                Session["NHAPCONGNO_NAMFILTER"] = value;
                txtNAMFILTER.Text = value.HasValue ? value.Value.ToString() : "";
            }
        }

        protected String MAKH
        {
            get { return Session["NHAPCONGNO_MAKH"] != null ? Session["NHAPCONGNO_MAKH"].ToString() : null; }
            set { Session["NHAPCONGNO_MAKH"] = value; txtMAKH.Text = value; }
        }

        protected String TENKH
        {
            get { return Session["NHAPCONGNO_TENKH"] != null ? Session["NHAPCONGNO_TENKH"].ToString() : null; }
            set { Session["NHAPCONGNO_TENKH"] = value; txtTENKH.Text = value; }
        }

        protected String SONHA
        {
            get { return Session["NHAPCONGNO_SONHA"] != null ? Session["NHAPCONGNO_SONHA"].ToString() : null; }
            set { Session["NHAPCONGNO_SONHA"] = value; txtSONHA.Text = value; }
        }

        protected String TENDUONGPHO
        {
            get { return Session["NHAPCONGNO_TENDUONGPHO"] != null ? Session["NHAPCONGNO_TENDUONGPHO"].ToString() : null; }
            set { Session["NHAPCONGNO_TENDUONGPHO"] = value; txtTENDP.Text = value; }
        }

        protected String MAKV
        {
            get { return Session["NHAPCONGNO_MAKV"] != null ? Session["NHAPCONGNO_MAKV"].ToString() : null; }
            set { 
                Session["NHAPCONGNO_MAKV"] = value;
                var selected = ddlKHUVUC.Items.FindByValue(MAKV);
                ddlKHUVUC.SelectedIndex = (selected != null) ? ddlKHUVUC.Items.IndexOf(selected) : 0;
            }
        }

        protected String SOPHIEUCN
        {
            get { return Session["NHAPCONGNO_SOPHIEUCN"] != null ? Session["NHAPCONGNO_SOPHIEUCN"].ToString() : null; }
            set { Session["NHAPCONGNO_SOPHIEUCN"] = value; txtSOPHIEUCN.Text = value; }
        } 
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.CN_TraCuuCongNo, Permission.Read);

                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindDataForGrid(gvList);
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_CN_TRACUUCONGNO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_CONGNO;
                header.TitlePage = Resources.Message.PAGE_CN_TRACUUCONGNO;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
        }

        private void LoadStaticReferences()
        {
            try
            {
                ddlTHANGFILTER.SelectedIndex = DateTime.Now.Month;
                txtNAMFILTER.Text = DateTime.Now.Year.ToString();

                var listKV = _kvDao.GetList();
                ddlKHUVUC.Items.Clear();
                ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in listKV)
                    ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));

                ddlKHUVUC.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        #region Startup script registeration
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
        #endregion

        /// <summary>
        /// Bind data for grid
        /// </summary>
        /// <param name="grid"></param>
        private void BindDataForGrid(Grid grid)
        {
            try
            {
                MAKH = txtMAKH.Text.Trim();
                TENKH = txtTENKH.Text.Trim();
                SONHA = txtSONHA.Text.Trim();
                TENDUONGPHO = txtTENDP.Text.Trim();
                MAKV = ddlKHUVUC.SelectedIndex > 0 ? ddlKHUVUC.SelectedValue : null;
                SOPHIEUCN = txtSOPHIEUCN.Text.Trim();

                DateTime? cnDate = null;
                int? thangCn = null, namCn = null;
                
                try { cnDate = DateTimeUtil.GetVietNamDate(txtNGAYCN.Text.Trim()); } catch { }
                try { namCn = Int32.Parse(txtNAMFILTER.Text.Trim()); } catch { }

                if (ddlTHANGFILTER.SelectedIndex > 0)
                    thangCn = ddlTHANGFILTER.SelectedIndex;

                var objList = _cnDao.GetList(namCn, thangCn, cnDate, MAKH, TENKH, SONHA, TENDUONGPHO, MAKV, SOPHIEUCN);
                grid.PagerInforText = objList.Count.ToString();
                grid.DataSource = objList;
                grid.DataBind();

                divList.Visible = objList.Count > 0;
                upnlGrid.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message,
                                                        ex.StackTrace));
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.CN_NhapCongNo, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowWarning(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds == null) || (string.Empty.Equals(strIds)))
                {
                    CloseWaitingDialog();
                    return;
                }
                
                var objs = new List<TIEUTHU>();
                var listIds = strIds.Split(',');

                // Kiem tra relation ship truoc khi delete
                foreach (var ma in listIds)
                {
                    var slistValue = ma.Split('|');

                    var congno = _cnDao.Get(slistValue[0], int.Parse(slistValue[1]), int.Parse(slistValue[2]));
                    if (congno != null)
                        objs.Add(congno);
                }
                
                var msg = _cnDao.DeleteListCongNo(objs, PageAction.Delete);
                if ((msg != null))
                {
                    if (msg.MsgType != MessageType.Error)
                    {
                        CloseWaitingDialog();
                        ShowInfor(ResourceLabel.Get(msg));

                        // Refresh grid view
                        BindDataForGrid(gvList);
                    }
                    else
                    {
                        ShowWarning(ResourceLabel.Get(msg));
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
                BindDataForGrid(gvList);
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
     
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindDataForGrid(gvList);
            CloseWaitingDialog();
        }
    }
}