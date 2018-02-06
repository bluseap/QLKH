using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.ThietKe
{
    public partial class PhanCongKhaoSatThietKe : Authentication
    {
        private readonly NhanVienDao nvDao = new NhanVienDao();
        private readonly PhuongDao pDao = new PhuongDao();
        private readonly PhanCongDao pcDao = new PhanCongDao();
        private List<PHANCONG> pcList = new List<PHANCONG>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TK_PhanCongKhaoSatThietKe, Permission.Read);

                PrepareUI();

                if (!Page.IsPostBack)
                {
                    //TODO: bind data
                    BindData();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindData()
        {
            //TODO: lấy danh sách phân công, bind vào hidden field và gridview
            pcList = pcDao.GetList();
            var res = "";
            foreach (var pc in pcList)
            {
                res += pc.MANV + "-" + pc.MAPHUONG + ", ";
            }

            if (res.Length > 2)
                res = res.Substring(0, res.Length - 2);
           
            hfPhanCong.Value = res;

            //TODO: lấy danh sách kỹ sư cần được phân công, hiện thời lấy tất cả, sau này sẽ lọc lại
            var list = nvDao.GetList();
            gvPhanCong.DataSource = list;
            gvPhanCong.DataBind();
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_TK_PHANCONGKHAOSAT;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_PHANCONGKHAOSAT;
            }
        }

        protected string IsChecked(string manv, string maphuong)
        {
            var result = "";
            foreach (var pc in pcList)
            {
                if(pc.MANV.ToLower().Equals(manv.ToLower()) &&
                    pc.MAPHUONG.ToLower().Equals(maphuong.ToLower()))
                {
                    result = "checked";
                    break;
                }
            }

            return result;
        }

        protected string HasChecked(string manv)
        {
            // check if all child checkboxes is checked
            var haschecked = false;

            var list = pDao.GetList();

            foreach (var p in list)
            {
                if (IsChecked(manv, p.MAPHUONG) == "") continue;
                
                haschecked = true;
                break;
            }

            return haschecked ? "" : " style=\"display: none\"";
        }

        protected string IsAllCheck()
        {
            var allchecked = true;
            var list = nvDao.GetList();

             foreach (var nv in list)
             {
                 if (IsParentCheck(nv.MANV) != "") continue;
                 allchecked = false;
                 break;
             }

             return allchecked ? "checked" : "";
        }

        protected string IsParentCheck(string manv)
        {
            // check if all child checkboxes is checked
            var allchecked = true;

            var list = pDao.GetList();

            foreach (var p in list)
            {
                if (IsChecked(manv, p.MAPHUONG) != "") continue;
                allchecked = false;
                break;
            }

            return allchecked ? "checked" : "";
        }

        protected void gvPhanCong_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                var hiddenId = (HtmlInputHidden)e.Row.FindControl("MaNV");
                if (hiddenId != null)
                {

                    var grdMultiLoad = ((GridView)e.Row.FindControl("grdMultiLoad"));


                    grdMultiLoad.EmptyDataText = "";
                    grdMultiLoad.DataSource = RefinePhuong(hiddenId.Value);
                    grdMultiLoad.DataBind();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private DataTable RefinePhuong(string maNV)
        {
            var table = new DataTable();

            // Declare DataColumn and DataRow variables.

            // Create new DataColumn, set DataType, ColumnName and add to DataTable.    
            var column = new DataColumn
                                    {
                                        DataType = Type.GetType("System.String"),
                                        ColumnName = "manv"
                                    };
            table.Columns.Add(column);

            column = new DataColumn
            {
                DataType = Type.GetType("System.String"),
                ColumnName = "maphuong"
            };
            table.Columns.Add(column);

            column = new DataColumn
            {
                DataType = Type.GetType("System.String"),
                ColumnName = "tenphuong"
            };
            table.Columns.Add(column);

            column = new DataColumn
            {
                DataType = Type.GetType("System.String"),
                ColumnName = "tenkv"
            };
            table.Columns.Add(column);

            var nv = nvDao.Get(LoginInfo.Username);
            if (nv == null) return table;

            //var list = pDao.GetList(nv.MAKV);
            var list = pDao.GetList( );
            foreach (var p in list)
            {
                var row = table.NewRow();
                row["manv"] = maNV;
                row["maphuong"] = p.MAPHUONG;
                row["tenphuong"] = p.TENPHUONG;
                row["tenkv"] = p.KHUVUC != null ? p.KHUVUC.TENKV : "";
                table.Rows.Add(row);
            }

            return table;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // get all user
            var pcs = hfPhanCong.Value.Split(',');

            var list = new List<PHANCONGTHEOPHUONG>();

            var delimiter = new string[] { DELIMITER.Delimiter };

            foreach (var pc in pcs)
            {

                var pair = pc.Trim().Split(delimiter, StringSplitOptions.None);
                if(pair.Length == 2)
                {
                    list.Add(new PHANCONGTHEOPHUONG
                                 {
                                     MANV = pair[0],
                                     MAPHUONG = pair[1]
                                 });
                }
            }

            pcDao.UpdatePhanCong(list);
            
            Page.Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Request.Url.AbsoluteUri);
        }

        
    }
}
