using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Util ;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using EOSCRM.Controls;

namespace EOSCRM.Web.Forms.CongNo
{
    public partial class KiemTraHoaDonTon : Authentication
    {
        private readonly CongNoDao _cnDao = new CongNoDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.CN_KiemTraHoaDonTon, Permission.Insert);

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
            Page.Title = Resources.Message.TITLE_CN_KIEMTRAHOADONTON;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_CONGNO;
                header.TitlePage = Resources.Message.PAGE_CN_KIEMTRAHOADONTON;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvMissingList);
        }

        private void LoadStaticReferences()
        {
            try
            {
                ddlTHANGFILTER.SelectedIndex = DateTime.Now.Month - 1;
                txtNAMFILTER.Text = DateTime.Now.Year.ToString();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private int GetThangTuMaString(string kytu)
        {
            switch (kytu.ToUpper())
            {
                case "A":
                    return 1;
                case "B":
                    return 2;
                case "C":
                    return 3;
                case "D":
                    return 4;
                case "E":
                    return 5;
                case "F":
                    return 6;
                case "G":
                    return 7;
                case "H":
                    return 8;
                case "I":
                    return 9;
                case "K":
                    return 10;
                case "L":
                    return 11;
                case "M":
                    return 12;
                default:
                    return 0;
            }
        }

        private int GetNamTuMaString(string kytu)
        {
            switch (kytu.ToUpper())
            {
                case "A":
                    return 2010;
                case "B":
                    return 2011;
                case "C":
                    return 2012;
                case "D":
                    return 2013;
                case "E":
                    return 2014;
                case "F":
                    return 2015;
                case "G":
                    return 2016;
                case "H":
                    return 2017;
                case "I":
                    return 2018;
                case "K":
                    return 2019;
                case "L":
                    return 2020;
                case "M":
                    return 2021;
                default:
                    return 0;
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

        private void BindDataForGrid(Grid grid)
        {

            try
            {
                int? thangCn, namCn;
                
                try
                {
                    var dp = new DuongPhoDao().Get(txtMADP.Text.Trim(), "");
                    if (dp == null) return;
                    
                    thangCn = ddlTHANGFILTER.SelectedIndex + 1;
                    namCn = Int32.Parse(txtNAMFILTER.Text.Trim());
                } 
                catch { return; }

                var objReversedList = _cnDao.GetReversedListForKiemTra(namCn.Value, thangCn.Value, txtMADP.Text.Trim());
                var ton = Math.Round(objReversedList.Sum(p => p.TONGTIEN_PS).Value, 0, MidpointRounding.AwayFromZero);

                var thu = _cnDao.GetTongTienHetNo(namCn.Value, thangCn.Value, txtMADP.Text.Trim());
                var tong = ton + thu;
                var chuanthu = _cnDao.GetChuanThu(namCn.Value, thangCn.Value, txtMADP.Text.Trim());
                var tonthieu = _cnDao.GetTonBiThieu(namCn.Value, thangCn.Value, txtMADP.Text.Trim());
                lblTON.Text = ton.ToString("0,0", CultureInfo.CreateSpecificCulture("el-GR"));
                lblDATHU.Text = thu.HasValue
                                        ? thu.Value.ToString("0,0", CultureInfo.CreateSpecificCulture("el-GR"))
                                        : "0";
                lblTONG.Text = tong.HasValue
                                        ? tong.Value.ToString("0,0", CultureInfo.CreateSpecificCulture("el-GR"))
                                        : "0";
                lblCHUANTHU.Text = chuanthu.HasValue 
                                        ? chuanthu.Value.ToString("0,0", CultureInfo.CreateSpecificCulture("el-GR"))
                                        : "0";
                lblTONTHIEU.Text = tonthieu.HasValue
                                        ? tonthieu.Value.ToString("0,0", CultureInfo.CreateSpecificCulture("el-GR"))
                                        : "0";
                divThongKe.Visible = chuanthu > 0;
                divCongNoList.Visible = objReversedList.Count > 0;

                grid.DataSource = objReversedList;
                grid.PagerInforText = objReversedList.Count.ToString();
                grid.DataBind();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message,
                                                        ex.StackTrace));
            }
        }

        private void BindDataForMissingGrid(Grid grid)
        {
            try
            {
                int? thangCn, namCn;
                try
                {
                    var dp = new DuongPhoDao().Get(txtMADP.Text.Trim(), "");
                    if (dp == null) return;

                    thangCn = ddlTHANGFILTER.SelectedIndex + 1;
                    namCn = Int32.Parse(txtNAMFILTER.Text.Trim());
                }
                catch { return; }

                var objMissingList = _cnDao.GetMissingListForKiemTra(namCn.Value, thangCn.Value, txtMADP.Text.Trim());

                divMissingList.Visible = objMissingList.Count > 0;

                grid.DataSource = objMissingList;
                grid.PagerInforText = objMissingList.Count.ToString();
                grid.DataBind();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message,
                                                        ex.StackTrace));
            }
        }

        private bool IsDataValid()
        {
            if(txtMAVACH .Text .Trim( ).Length != 8)
            {
                ShowInfor("Mã vạch không hợp lệ");
                txtMAVACH.Focus();
                return false;
            }

            return true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.CN_KiemTraHoaDonTon, Permission.Delete))
                {
                    CloseWaitingDialog();   
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<TIEUTHU>();
                    var listIds = strIds.Split(',');

                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {
                        String[] slistValue = ma.Split('|');

                        TIEUTHU congno = _cnDao.Get(slistValue[0], int.Parse(slistValue[1]), int.Parse(slistValue[2]));
                        if (congno != null)

                            objs.Add(congno);
                    }

                    CloseWaitingDialog();

                    var msg = _cnDao.DeleteReversedListForKiemTra(objs);
                    if ((msg != null))
                    {
                        if (msg.MsgType != MessageType.Error)
                        {
                            ShowInfor(ResourceLabel.Get(msg));

                            // Refresh grid view
                            BindDataForGrid(gvList);
                            BindDataForMissingGrid(gvMissingList);
                            upnlGrid.Update();
                            upnlMissingGrid.Update();
                        }
                        else
                               
                            ShowWarning(ResourceLabel.Get(msg));
                    }
                    else
                    {
                        CloseWaitingDialog();
                        ShowWarning(String.Format(Resources.Message.E_FAILED, "Xóa danh bộ kiểm tra công nợ"));
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
     
        protected void txtMAVACH_TextChanged(object sender, EventArgs e)
        {

            var barcode = txtMAVACH.Text.Trim();
            if (barcode.Length != 8)
            {
                ShowWarning("Mã vạch không hợp lệ.");
                return;
            }
            var thang = CommonFunc.MonthFromBarcode(barcode);
            var nam = CommonFunc.YearFromBarcode(barcode);
            var idkh = barcode.Substring(2, 6);         

            /*if (string.IsNullOrEmpty(txtMAVACH.Text.Trim()))
                return;
            var thang = GetThangTuMaString(txtMAVACH.Text.Trim().Substring(0,1));
            var nam = GetNamTuMaString(txtMAVACH.Text.Trim().Substring(1, 1));            
            if (!IsDataValid())
                return;
            var congno = _cnDao.Get(txtMAVACH.Text.Trim().Substring(2, 3),
                                        txtMAVACH.Text.Trim().Substring(5, 5),
                                        thang, nam);
            if (congno == null)
            {
                ShowInfor(string.Format("Công nợ không tồn tại, xem lại thông tin mã vạch. {0}/{1}/{2}/{3}",
                    thang,
                    nam,
                    txtMAVACH.Text.Trim().Substring(2, 3),
                    txtMAVACH.Text.Trim().Substring(5, 5)));
                return;
            }*/
            TIEUTHU congno = _cnDao.Get(idkh, thang, nam);
            if (congno == null)
            {
                ShowInfor("Công nợ không tồn tại, xem lại thông tin mã vạch." + thang.ToString() + "/" + nam.ToString() + "/" + txtMAVACH.Text.Trim().Substring(2, 4) + "/" + txtMAVACH.Text.Trim().Substring(6, 6));
                return;
            }
            

           if(congno.THUTQ != null && congno.THUTQ == true)
           {
               ShowInfor("Đối tượng đã được thu tại quầy, không thể quét ngược công nợ.");
               return;
           }

            if(congno.HETNO != null && congno.HETNO == true)
            {
                var mess = congno.NGAYCN.HasValue 
                    ? string.Format("Công nợ đã được thu tại kỳ : {0}/{1} với số phiếu : {2}", 
                        congno.NGAYCN.Value.Month, congno.NGAYCN.Value.Year, congno.SOPHIEUCN) 
                    : "Công nợ đã được thu.";

                ShowInfor(mess);
                return;
            }

            if (congno.HETNO != null && congno.CONNO_KIEMTRA == true)
            {
                ShowInfor("Công nợ đã được quét kiểm tra. Vui lòng lọc lại để kiểm tra.");
                return;
            }

            congno.CONNO_KIEMTRA = true;

            var msg = _cnDao.UpdateCongNo(congno);
            if (!msg.MsgType.Equals(MessageType.Error)) {
                txtMAVACH.Text = "";
                BindDataForGrid(gvList);
                BindDataForMissingGrid(gvMissingList);
                upnlGrid.Update();
                upnlMissingGrid.Update();
                txtMAVACH.Focus();
            }
            else {
                ShowInfor(ResourceLabel.Get(msg));
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindDataForGrid(gvList);
            BindDataForMissingGrid(gvMissingList);
            upnlGrid.Update();
            upnlMissingGrid.Update();
        }



        protected void gvMissingList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvMissingList.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindDataForMissingGrid(gvMissingList);
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            BindDataForMissingGrid(gvMissingList);
            upnlMissingGrid.Update();

            CloseWaitingDialog();
        }
    }
}