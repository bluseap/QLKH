using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Web.Common;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

using EOSCRM.Domain;


namespace EOSCRM.Web.Forms.GhiChiSo.BaoCao
{
    public partial class TongHopChuanThu : Authentication
    {
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();

        #region Startup script registeration
        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.GCS_BaoCao_TongHopChuanThu, Permission.Read);
                PrepareUI();
                if (!Page.IsPostBack)
                {
                    LoadReferences();                    
                }
                else 
                {
                    if (Session[SessionKey.GCS_BAOCAO_TONGHOPCHUANTHUBIEUDO] == "GCS_BAOCAO_TONGHOPCHUANTHUBIEUDO")
                    {
                        var dt = (DataTable)Session[SessionKey.GCS_BAOCAO_TONGHOPCHUANTHUBIEUDO];
                        TongHopChuanThuBieuDo(dt);
                    }
                    
                    if (Session[SessionKey.GCS_BAOCAO_TONGHOPCHUANTHU] == "GCS_BAOCAO_TONGHOPCHUANTHU")
                    {
                        var dt = (DataTable)Session[SessionKey.GCS_BAOCAO_TONGHOPCHUANTHU];
                        Report(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_GCS_BAOCAO_TONGHOPCHUANTHU;
            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_BAOCAO_TONGHOPCHUANTHU;
            }
        }

        private void LoadReferences()
        {
            /*
            var kvList = _kvDao.GetList();

            cboKhuVuc.Items.Clear();
            cboKhuVuc.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var kv in kvList)
            {
                cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }
            */

            timkv();

            txtNAM.Text = DateTime.Now.Year.ToString();
            cboTHANG.SelectedIndex = DateTime.Now.Month - 1;

            txtNguoiLap.Text = LoginInfo.NHANVIEN.HOTEN;

            txtTuNam.Text = DateTime.Now.Year.ToString();
            ddlTuKy.SelectedIndex = DateTime.Now.Month - 1;

            txtDenNam.Text = DateTime.Now.Year.ToString();
            ddlDenKy.SelectedIndex = DateTime.Now.Month - 1;


        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var ds =
                new ReportClass().BangTongHopChuanThu(int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue);

            if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
            Report(ds.Tables[0]);

            CloseWaitingDialog();
        }

        private void Report(DataTable dt)
        {
            if (dt == null)
                return;

            #region FreeMemory
            var rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
// ReSharper disable EmptyGeneralCatchClause
                catch
// ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            #endregion FreeMemory
            
            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/TongHopChuanThu.rpt");
            rp.Load(path);


            var txtNgay = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgay != null)
                txtNgay.Text = string.Format("TP.Long Xuyên, ngày {0} tháng {1} năm {2}", 
                    DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            #region Sét giá trị tổng cộng

            var ttsohd = 0;
            var ttm3Tt = 0;
            decimal tiennuoc = 0;
            decimal thue = 0;
            decimal phibvmt = 0;
            var ttsohd0m3 = 0;
            var ttm3tthd = 0;

            foreach (DataRow row in dt.Rows)
            {
// ReSharper disable EmptyGeneralCatchClause
                try { ttm3Tt = ttm3Tt + (int) row["TTM3TT"]; } catch {}
                try { ttsohd = ttsohd + (int) row["TTSOHD"]; } catch {}
                try { tiennuoc = tiennuoc + (decimal) row["TIENNUOC"]; } catch {}
                try { thue = thue + (decimal) row["THUE"]; } catch {}
                try { phibvmt = phibvmt + (decimal) row["PHIBVMT"]; } catch {}
                try { ttsohd0m3 = ttsohd0m3 + (int) row["TTSOHD0M3"]; } catch {}
                try { ttm3tthd = ttm3tthd + (int) row["M3SH1"]; } catch {}
// ReSharper restore EmptyGeneralCatchClause
            }

            //var tongcong = tiennuoc + thue + phibvmt;
            var tongcong = tiennuoc;

            var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            if (txtKy != null)
                txtKy.Text = string.Format("Kỳ hóa đơn: {0}/{1}", cboTHANG.SelectedValue, txtNAM.Text.Trim());

            var txtTtsohd = rp.ReportDefinition.ReportObjects["txtTTSOHD"] as TextObject;
            if (txtTtsohd != null)
                txtTtsohd.Text = ttsohd.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            var txtTtm3Tt = rp.ReportDefinition.ReportObjects["txtTTM3TT"] as TextObject;
            if (txtTtm3Tt != null)
                txtTtm3Tt.Text = ttm3Tt.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            //var txtTiennuoc = rp.ReportDefinition.ReportObjects["txtTIENNUOC"] as TextObject;
            //if (txtTiennuoc != null)
            //    txtTiennuoc.Text = tiennuoc.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            //var txtThue = rp.ReportDefinition.ReportObjects["txtTHUE"] as TextObject;
            //if (txtThue != null)
            //    txtThue.Text = thue.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            //var txtPhibvmt = rp.ReportDefinition.ReportObjects["txtPHIBVMT"] as TextObject;
            //if (txtPhibvmt != null)
            //    txtPhibvmt.Text = phibvmt.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            var txtTongcong = rp.ReportDefinition.ReportObjects["txtTONGCONG"] as TextObject;
            if (txtTongcong != null)
                txtTongcong.Text = tongcong.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            //var txtTTM3TTHD = rp.ReportDefinition.ReportObjects["txtTTM3TTHD"] as TextObject;
            //if (txtTTM3TTHD != null)
            //    txtTTM3TTHD.Text = ttm3tthd.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            //var txtTTSOHD0M3 = rp.ReportDefinition.ReportObjects["txtTTSOHD0M3"] as TextObject;
            //if (txtTTSOHD0M3 != null)
            //    txtTTSOHD0M3.Text = ttsohd0m3.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            #endregion
            
            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divCR.Visible = true;

            Session[SessionKey.GCS_BAOCAO_TONGHOPCHUANTHU] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        public void timkv()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvDao.GetListKV(b);
            foreach (var a in query)
            {
                string d = a.MAKV;

                if (a.MAKV == "99" && b == "nguyen")
                {
                    var kvList = _kvDao.GetList();
                    cboKhuVuc.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                    
                }
                else if (a.MAKV == "99")
                {
                    var kvList = _kvDao.GetList();
                    cboKhuVuc.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }

                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    cboKhuVuc.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }

            if (b == "nguyen")
            {
                VisibleBieuDo(true);
            }
            else
            {
                VisibleBieuDo(false);
            }
        }

        protected void btnKyChart_Click(object sender, EventArgs e)
        {
            try
            {
                var tuky = DateTimeUtil.GetVietNamDate("01/" + ddlTuKy.SelectedValue + "/" + txtTuNam.Text.Trim());
                var denky = DateTimeUtil.GetVietNamDate("01/" + ddlDenKy.SelectedValue + "/" + txtDenNam.Text.Trim());

                var ds = new ReportClass().BANGTONGHOPCHUANTHUBIEUDO(int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), tuky, denky,
                    cboKhuVuc.SelectedValue, "", "", "", "BIEUDOCOTKY");

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                TongHopChuanThuBieuDo(ds.Tables[0]);

                CloseWaitingDialog();
            }
            catch { }
        }

        private void TongHopChuanThuBieuDo(DataTable dt)
        {
            if (dt == null)
                return;

            #region FreeMemory
            var rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/TongHopChuanThuChart.rpt");
            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divCR.Visible = true;

            Session[SessionKey.GCS_BAOCAO_TONGHOPCHUANTHUBIEUDO] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void VisibleBieuDo(bool para)
        {
            try
            {
                lbTuKy.Visible = para;
                lbDenKy.Visible = para; 

                ddlTuKy.Visible = para;
                ddlDenKy.Visible = para; 

                txtTuNam.Visible = para;       
                txtDenNam.Visible = para;

                btnKyChart.Visible = para;
            }
            catch { }
        }

    }
}