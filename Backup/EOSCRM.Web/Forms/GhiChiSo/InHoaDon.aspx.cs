using System;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using EOSCRM.Controls;
using System.IO;

namespace EOSCRM.Web.Forms.GhiChiSo
{
    public partial class InHoaDon : Authentication
    {
        private readonly DuongPhoDao dpDao = new DuongPhoDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.GCS_InHoaDon, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    SearchDuong();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_GCS_INHOADON;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_INHOADON;
            }

            CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }

        private void LoadStaticReferences()
        {
            // load khu vuc
            var listKhuVuc = new KhuVucDao().GetList();

            ddlKHUVUC.Items.Clear();
            ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));

            foreach(var kv in listKhuVuc)
            {
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }

            ClearForm();
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

        private void SearchDuong()
        {
            var list = dpDao.GetList(ddlKHUVUC.SelectedValue, txtMADP.Text.Trim());
            dpDataList.DataSource = list;
            dpDataList.DataBind();
            upnlGrid.Update();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // load lo trinh tinh cuoc by khu vuc and duong pho
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

            SearchDuong();
            CloseWaitingDialog();
        }

        /*
        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var str_madp = "";

            for (var i = 0; i < dpDataList.Items.Count; i++)
            {
                var cb = dpDataList.Items[i].FindControl("chkDuongPho") as HtmlInputCheckBox;
                if (cb == null || !cb.Checked) continue;

                var madp = cb.Attributes["title"].Trim();
                var duongphu = cb.Attributes["lang"].Trim();

                if (duongphu.Length > 0)
                {
                    str_madp += " (DP.MADP= '" + madp + "' and DP.DUONGPHU = '" + duongphu + "') OR";
                }
                else
                {
                    str_madp += " (DP.MADP= '" + madp + "') OR";
                }
            }

            str_madp = "(" + str_madp + ") and";
            
            str_madp = (str_madp == "() and") ? 
                "" : 
                str_madp.Replace("OR) and", ")  ");

            if(string .IsNullOrEmpty(str_madp.Trim()))
            {
                CloseWaitingDialog();
                ShowError("Vui lòng chọn đường phố để báo cáo");
                return;
            }

            var dtDSINHOADON =
                new ReportClass().InHoaDonTn(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), str_madp).
                    Tables[0];

            if (dtDSINHOADON.Rows.Count > 0)
            {
                Session["DSINHOADON"] = dtDSINHOADON;
                CloseWaitingDialog();
                Page.Response.Redirect(ResolveUrl("~") + "Forms/GhiChiSo/BaoCao/rpInHoaDon.aspx");
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Không tìm thấy dữ liệu để làm báo cáo");
            }
        }
        */

        #region Đường phố
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

        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            CloseWaitingDialog();
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

        private byte[] imageToByte(System.Drawing.Image img)
        {
            MemoryStream objMS = new MemoryStream();
            img.Save(objMS, System.Drawing.Imaging.ImageFormat.Png);
            return objMS.ToArray();
        }

        protected void btnBaoCaoThuy_Click(object sender, EventArgs e)
        {
            var str_madp = "";

            for (var i = 0; i < dpDataList.Items.Count; i++)
            {
                var cb = dpDataList.Items[i].FindControl("chkDuongPho") as HtmlInputCheckBox;
                if (cb == null || !cb.Checked) continue;

                var madp = cb.Attributes["title"].Trim();
                var duongphu = cb.Attributes["lang"].Trim();

                if (duongphu.Length > 0)
                {
                    str_madp += " (DP.MADP= '" + madp + "' and DP.DUONGPHU = '" + duongphu + "') OR";
                }
                else
                {
                    str_madp += " (DP.MADP= '" + madp + "') OR";
                }
            }

            str_madp = "(" + str_madp + ") and";

            str_madp = (str_madp == "() and") ?
                "" :
                str_madp.Replace("OR) and", ")  ");

            if (string.IsNullOrEmpty(str_madp.Trim()))
            {
                CloseWaitingDialog();
                ShowError("Vui lòng chọn đường phố để báo cáo");
                return;
            }

            var dtDSINHOADON =
                new ReportClass().InHoaDonTn(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), str_madp).
                    Tables[0];

            dtDSINHOADON.Columns.Add("IMG", typeof(byte[]));

            Barcode128 code128 = new Barcode128();
            code128.CodeType = Barcode.CODE128;
            code128.BarHeight = 100;
            code128.X = 18;
            code128.N = 1f;
            code128.StartStopText = true;

            if (dtDSINHOADON.Rows.Count > 0)
            {
                foreach (System.Data.DataRow row in dtDSINHOADON.Rows)
                {
                    // noi dung data
                    //string data =  row["BARCODE"];

                    code128.Code = row["BARCODE"].ToString();
                    System.Drawing.Image bc = code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White);
                    byte[] ar = imageToByte(bc);
                    row["IMG"] = ar;

                }

                Session["DSINHOADON"] = dtDSINHOADON;
                CloseWaitingDialog();
                Page.Response.Redirect(ResolveUrl("~") + "Forms/GhiChiSo/BaoCao/rpInHoaThuy.aspx");
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Không tìm thấy dữ liệu để làm báo cáo");
            }
        }
    }
}
