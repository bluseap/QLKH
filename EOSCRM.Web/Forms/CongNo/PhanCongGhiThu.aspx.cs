using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using EOSCRM.Domain;
using EOSCRM.Util ;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.CongNo
{
    public partial class PhanCongGhiThu : Authentication
    {
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly DuongPhoDao _dpDao = new DuongPhoDao();

        protected List<DUONGPHOGT> DataSource
        {
            get { return Session["PHANCONGGHITHU_DATASOURCE"] as List<DUONGPHOGT>; }
            set { Session["PHANCONGGHITHU_DATASOURCE"] = value; }
        }

        protected List<NHANVIEN> ListNhanVien
        {
            get { return Session["PHANCONGGHITHU_LISTNHANVIEN"] as List<NHANVIEN>; }
            set { Session["PHANCONGGHITHU_LISTNHANVIEN"] = value; }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.CN_PhanCongGhiThu, Permission.Read);

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
            Page.Title = Resources.Message.TITLE_CN_PHANCONGGHITHU;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_CONGNO;
                header.TitlePage = Resources.Message.PAGE_CN_PHANCONGGHITHU;
            }

            CommonFunc.SetPropertiesForGrid(gvDuongPho);
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

                var nvList = _nvDao.GetListByCV(MACV.GT.ToString());

                ddlNHANVIEN.Items.Clear();
                ddlNHANVIEN.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var nv in nvList)
                {
                    ddlNHANVIEN.Items.Add(new ListItem(nv.HOTEN, nv.MANV));
                }

                ListNhanVien = nvList;

                ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
                txtNAM.Text = DateTime.Now.Year.ToString();


            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
   
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try {
                DateTime.ParseExact(txtNAM.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
            }
            catch
            {
                CloseWaitingDialog();
                ShowInfor("Năm không hợp lệ.");
                return;
            }

            var date = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);

            var makv = ddlKHUVUC.SelectedIndex > 0 ? ddlKHUVUC.SelectedValue : null;
            var manv = ddlNHANVIEN.SelectedIndex > 0 ? ddlNHANVIEN.SelectedValue : null;

            var list = _dpDao.GetListGhiThu(makv, manv, date.Year, date.Month);
            DataSource = list;
            gvDuongPho.DataSource = list;
            gvDuongPho.PagerInforText = list.Count.ToString();
            gvDuongPho.DataBind();


            divGhiThu.Visible = list.Count > 0;

            upnlGhiThu.Update();

            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!HasPermission(Functions.CN_PhanCongGhiThu, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                var objs = new List<DUONGPHOGT>();

                foreach (GridViewRow row in gvDuongPho.Rows)
                {
                    if (!row.RowType.Equals(DataControlRowType.DataRow)) continue;
                    
                    var hfGT = row.FindControl("hfGT") as HiddenField;
                    if (hfGT == null) continue;
// ReSharper disable RedundantExplicitArrayCreation
                    var delimiter = new string[] { DELIMITER.Delimiter };
// ReSharper restore RedundantExplicitArrayCreation

                    var gt = hfGT.Value.Split(delimiter, StringSplitOptions.None);

                    objs.Add(new DUONGPHOGT
                                 {
                                     NAM = int.Parse(gt[0]),
                                     THANG = int.Parse(gt[1]),
                                     MADP = gt[2],
                                     DUONGPHU = (gt[3] == DELIMITER.NullString) ? "" : gt[3],
                                     MAKV = gt[4],
                                     MANVG = (gt[5] == DELIMITER.NullString) ? null : gt[5],
                                     MANVT = (gt[6] == DELIMITER.NullString) ? null : gt[6]
                                 });
                }

                
                //TODO: update list
                var msg = _dpDao.UpdateGhiThu(objs);

                CloseWaitingDialog();

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    ShowInfor("Cập nhật danh sách ghi thu thành công.");
                }
                else
                {
                    ShowWarning("Cập nhật danh sách ghi thu không thành công");
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDuongPho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var ddlMANVG = e.Row.FindControl("ddlMANVG") as DropDownList;
            var ddlMANVT = e.Row.FindControl("ddlMANVT") as DropDownList;
            var hfGT = e.Row.FindControl("hfGT") as HiddenField;
            if (ddlMANVG == null || ddlMANVT == null || hfGT == null) return;

            var onChangeEventHandler = "javascript:onChangeEventHandler(\"" + ddlMANVG.ClientID +
                                                                "\", \"" + ddlMANVT.ClientID +
                                                                "\", \"" + hfGT.ClientID +
                                                                "\");";
            ddlMANVG.Attributes.Add("onchange", onChangeEventHandler);
            ddlMANVT.Attributes.Add("onchange", onChangeEventHandler);

            var datasource = DataSource;
            if(datasource == null) return;

            var index = e.Row.RowIndex;
            if (e.Row.RowIndex >= datasource.Count) return;

            var data = datasource[index];

            var nvList = ListNhanVien;
            if (nvList == null) return;

            ddlMANVG.Items.Clear();
            ddlMANVG.Items.Add(new ListItem("Chưa xác định", DELIMITER.NullString));
            foreach (var nv in nvList)
            {
                ddlMANVG.Items.Add(new ListItem(nv.HOTEN, nv.MANV));
            }

            ddlMANVT.Items.Clear();
            ddlMANVT.Items.Add(new ListItem("Chưa xác định", DELIMITER.NullString));
            foreach (var nv in nvList)
            {
                ddlMANVT.Items.Add(new ListItem(nv.HOTEN, nv.MANV));
            }

            var manvg = ddlMANVG.Items.FindByValue(data.MANVG);
            if (manvg != null)
                ddlMANVG.SelectedIndex = ddlMANVG.Items.IndexOf(manvg);

            var manvt = ddlMANVT.Items.FindByValue(data.MANVT);
            if (manvt != null)
                ddlMANVT.SelectedIndex = ddlMANVT.Items.IndexOf(manvt);
        }
    }
}