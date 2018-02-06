﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.ThiCongCongTrinh.Power.BaoCaoPo
{
    public partial class InPhieuLDNCPo : Authentication
    {
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();        
        private readonly PhuongPoDao _ppoDao = new PhuongPoDao();        
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();        
        private readonly HopDongPoDao _hdpoDao = new HopDongPoDao();

        private readonly ThiCongDao _tcDao = new ThiCongDao();
        private readonly DateTimeUtil _dateDao = new DateTimeUtil();
        private readonly ReportClass _rpDao = new ReportClass();
        private readonly NhanVienDao _nvDao = new NhanVienDao();        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    //TODO: Load references
                    LoadReferences();
                }
                else
                {
                    if (reloadm.Text == "1")
                    {
                        ReLoadBaoCao();
                    }
                    else if (reloadm.Text == "2")
                    {
                        ReLoadBaoCaoMau();
                    }
                    else if (reloadm.Text == "nc")
                    {
                        ReBacCaoLapDatNiemChi();
                    }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void LoadReferences()
        {
            txtHOTENNV1.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
            txtNGAYIN.Text = DateTime.Now.ToString("dd/MM/yyyy");
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

        private void SetReadonly(string id, bool isReadonly)
        {
            ((PO)Page.Master).SetReadonly(id, isReadonly);
        }

        private void SetControlValue(string id, string value)
        {
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
        }

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        #endregion

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_TC_INPHIEULDPO;

            var header = (Header)Master.FindControl("header");
            if (header == null) return;

            header.ModuleName = Resources.Message.MODULE_THICONG;
            header.TitlePage = Resources.Message.PAGE_TC_INPHIEULDPO;

            //CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            if (_hdpoDao.Get(txtMADDK.Text.Trim()) != null && _tcDao.Get(txtMADDK.Text.Trim()) != null)
            {
                LayBaoCaoTrang();
            }
            else
            {
                BacCaoLapDatNiemChi();
            }

            //LayBaoCaoTrang();
        }

        protected void BacCaoLapDatNiemChi()
        {
            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch
                {

                }
            }

            DataTable dt = new ReportClass().INPHIEULAPNIEMCHIDHD(txtMADDK.Text.Trim()).Tables[0];
            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/KiemDinhDongHo/LAPDATNIEMCHID.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            var tenkv = _kvpoDao.GetPo(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();


            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtHOTENNV1.Text.ToString();

            divCR.Visible = true;
            upnlCrystalReport.Update();


            reloadm.Text = "nc";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        protected void ReBacCaoLapDatNiemChi()
        {
            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch
                {

                }
            }

            DataTable dt = new ReportClass().INPHIEULAPNIEMCHIDHD(txtMADDK.Text.Trim()).Tables[0];
            rp = new ReportDocument();           
            var path = Server.MapPath("~/Reports/KiemDinhDongHo/LAPDATNIEMCHID.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            var tenkv = _kvpoDao.GetPo(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();


            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtHOTENNV1.Text.ToString();

            divCR.Visible = true;
            upnlCrystalReport.Update();


            reloadm.Text = "nc";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        protected void LayBaoCaoTrang()
        {
            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch {   }
            }

            DataTable dt = new ReportClass().INPHIEULAPDHD(txtMADDK.Text.Trim()).Tables[0];
            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/KiemDinhDongHo/PHIEULAPDATDIEN.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            var tenkv = _kvpoDao.GetPo(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var tcn = _tcDao.Get(txtMADDK.Text.Trim());
            //String ngay3 = Convert.ToString(tcn.NGAYHT);
            //String ngaygan = ngay3.Substring(0, 10);
            String ngaygan = tcn.NGAYHT.Value.ToString("dd/MM/yyyy");
            //DateTime ngaygan = DateTimeUtil.GetVietNamDate(Convert.ToString(tcn.NGAYHT));
            //TextObject txtNGAYGAN = rp.ReportDefinition.ReportObjects["txtNGAYGAN"] as TextObject;
            //if (txtNGAYGAN != null)
            //    txtNGAYGAN.Text = ngaygan.ToString();

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtHOTENNV1.Text.ToString();

            divCR.Visible = true;
            upnlCrystalReport.Update();

            
            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }


        private void ReLoadBaoCao()
        {
            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch
                {

                }
            }

            DataTable dt = new ReportClass().INPHIEULAPDHD(txtMADDK.Text.Trim()).Tables[0];
            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/KiemDinhDongHo/PHIEULAPDATDIEN.rpt");

           
            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            var tenkv = _kvpoDao.GetPo(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var tcn = _tcDao.Get(txtMADDK.Text.Trim());
            String ngay3 = Convert.ToString(tcn.NGAYHT);
            //String ngaygan = ngay3.Substring(0, 10);
            //DateTime ngaygan = DateTimeUtil.GetVietNamDate(Convert.ToString(tcn.NGAYHT));
            String ngaygan = tcn.NGAYHT.Value.ToString("dd/MM/yyyy");
            //TextObject txtNGAYGAN = rp.ReportDefinition.ReportObjects["txtNGAYGAN"] as TextObject;
            //if (txtNGAYGAN != null)
             //   txtNGAYGAN.Text = ngaygan.ToString();

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtHOTENNV1.Text.ToString();

            divCR.Visible = true;
            upnlCrystalReport.Update();

            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
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

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var madon = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "EditItem":
                        if (!string.Empty.Equals(madon))
                        {
                            var don = _ddkpoDao.Get(madon);
                            if (don == null) return;

                            var ds = _rpDao.INHOPDONG(madon.ToString());

                            if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                            //Report(ds.Tables[0]);
                            txtMADDK.Text = madon;

                            /*
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

                            var dt = _rpDao.INHOPDONG(madon);                                
                            rp = new ReportDocument();
                            var path = Server.MapPath("../../../Reports/DonLapDatMoi/INHOPDONG.rpt");
                            rp.Load(path);
                            rp.SetDataSource(dt);
                            rpViewer.ReportSource = rp;
                            rpViewer.DataBind();
                            divCR.Visible = true;
                            upnlCrystalReport.Update();
                            Session["DSBAOCAO"] = dt;
                            Session[Constants.REPORT_FREE_MEM] = rp;
                            */

                            upnlInfor.Update();
                            HideDialog("divHopDong");


                            CloseWaitingDialog();
                            //SetDDKToForm(don);
                        }
                        CloseWaitingDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindDataForGrid()
        {
            try
            {
                // ReSharper restore EmptyGeneralCatchClause

                //hien theo phong ban, khu vuc
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.Get(b);//nhan vien khu vuc ??

                var objList = _ddkpoDao.GetListHD(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), query.MAKV.ToString());

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
                //}
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {

        }

        protected void linkPLDATMAU_Click(object sender, EventArgs e)
        {
            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch
                {

                }
            }

            DataTable dt = new ReportClass().INPHIEULAPDHN(txtMADDK.Text.Trim()).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/KiemDinhDongHo/PHIEULAPDATNUOCMAU.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();
            /*
            var tenkv = _kvDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();
            */
            var tcn = _tcDao.Get(txtMADDK.Text.Trim());
            String ngay3 = Convert.ToString(tcn.NGAYHT);
            String ngaygan = ngay3.Substring(0, 10);
            //DateTime ngaygan = DateTimeUtil.GetVietNamDate(Convert.ToString(tcn.NGAYHT));
            /*
            TextObject txtNGAYGAN = rp.ReportDefinition.ReportObjects["txtNGAYGAN"] as TextObject;
            if (txtNGAYGAN != null)
                txtNGAYGAN.Text = ngaygan.ToString();
            */
            divCR.Visible = true;
            upnlCrystalReport.Update();

            reloadm.Text = "2";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        private void ReLoadBaoCaoMau()
        {
            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch
                {

                }
            }

            DataTable dt = new ReportClass().INPHIEULAPDHN(txtMADDK.Text.Trim()).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/KiemDinhDongHo/PHIEULAPDATNUOCMAU.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();
            /*
            var tenkv = _kvDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();
            */
            var tcn = _tcDao.Get(txtMADDK.Text.Trim());
            String ngay3 = Convert.ToString(tcn.NGAYHT);
            String ngaygan = ngay3.Substring(0, 10);
            //DateTime ngaygan = DateTimeUtil.GetVietNamDate(Convert.ToString(tcn.NGAYHT));
            /*
            TextObject txtNGAYGAN = rp.ReportDefinition.ReportObjects["txtNGAYGAN"] as TextObject;
            if (txtNGAYGAN != null)
                txtNGAYGAN.Text = ngaygan.ToString();
            */
            divCR.Visible = true;
            upnlCrystalReport.Update();

            //reloadm.Text = "2";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

    }
}