using System;
using System.Web;
using System.Drawing;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Net.NetworkInformation;

using System.Collections;
using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using Table = System.Web.UI.WebControls.Table;


namespace EOSCRM.Web.Common
{
    public class ResourceLabel
    {
        public static string Get(Message msg)
        {
            try
            {
                return String.Format(Resources.Message.ResourceManager.GetString(msg.MsgCode), msg.Holders);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string Get(string resourceCode)
        {
            return Get(new Message(resourceCode, MessageType.Info));
        }
    }    

    public class CommonFunc
    {
        public static string UniqueIDWithDollars(Control ctrl)
        {
            var sId = ctrl.UniqueID;
            if (sId == null){
                return null;
            }

            if (sId.IndexOf(':') >= 0) {
                return sId.Replace(':', '$');
            }

            return sId;
        }
       
        public static void SetPropertiesForGrid(GridView grdView)
        {
            try
            {
                grdView.AutoGenerateColumns = false;
                grdView.AllowPaging = true;
                grdView.AllowSorting = false;
                
                grdView.CssClass = "crmgrid";
                grdView.RowStyle.CssClass = "row";
                grdView.AlternatingRowStyle.CssClass = "altrow";
                grdView.HeaderStyle.CssClass = "header";
                grdView.EmptyDataText = "Không có dữ liệu.";
                grdView.EmptyDataRowStyle.CssClass = "emptyrow";

               
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public static string GetValueFromGrid(GridView grdView, string colIndexName, string strCompareValue, string colResultValue)
        {
            var strRes = string.Empty;

            try
            {
                if (grdView != null)
                {
                    for (int i = 0; i < grdView.Rows.Count; i++)
                    {
                        string indexValue = ((HtmlInputHidden)grdView.Rows[i].FindControl(colIndexName)).Value.Trim();

                        if (indexValue.Equals(strCompareValue))
                        {
                            strRes = ((HtmlInputHidden)grdView.Rows[i].FindControl(colResultValue)).Value.Trim();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strRes;
        }
       
        public static void AddResultToPager(GridViewRow pagerRow, string strResult)
        {
            Table tbl = new Table();
            TableRow row = new TableRow();
            TableCell col = new TableCell();
            col.Text = strResult;
            row.Cells.Add(col);
            tbl.Rows.Add(row);            
            tbl.CssClass = "ResultPager";
            pagerRow.Cells[0].Controls.AddAt(0, tbl);
        }

        public static string GetResult(GridView grd, int pageSize, int totalRows)
        {
            string result = ""; 
            int pageIndex = grd.PageIndex;
            int startRows = pageIndex * pageSize + 1;
            int endRows = (pageIndex + 1) * pageSize;
            if (endRows > totalRows)
            {
                endRows = totalRows;
            }
            result = string.Format(WebConstants.RESULT_PAGER, startRows, endRows, totalRows);
            return result;
        }

        public static void AddSortImage(GridViewRow headerRow, int iCol, string direction)
        {            
            if (iCol == -1)
            {
                return;
            }
            System.Web.UI.WebControls.Image sortImage = new System.Web.UI.WebControls.Image();
            if (Constants.Desc.Equals(direction))
            {
                sortImage.ImageUrl = WebConstants.IMAGE_URL_ASCENDING;
                sortImage.AlternateText = WebConstants.TEXT_DESCENDING;
            }
            else if (Constants.Asc.Equals(direction))
            {
                sortImage.ImageUrl = WebConstants.IMAGE_URL_DESCENDING;
                sortImage.AlternateText = WebConstants.TEXT_ASCENDING;
            }
            headerRow.Cells[iCol].Controls.Add(sortImage);
        }
       
        public static int GetSortColumnIndex(Hashtable hasTable, string strExpression)
        {
            return int.Parse(hasTable[strExpression].ToString());
        }
      
        public static string RevertSortDirection(string direction)
        {
            return (Constants.Desc.Equals(direction)) ? Constants.Asc : Constants.Desc;            
        }
        
        public static string GetComputerName()
        {
            string sName = "Unknow";
            try
            {
               sName = Dns.GetHostName();
            }catch
            {
            }
            return sName;
        }

        public static string GetDeviceName()
        {
            string sName = "Unknow";
            try
            {
                string[] computer_name = System.Net.Dns.GetHostEntry(System.Web.HttpContext.Current.Request.ServerVariables["remote_addr"]).HostName.Split(new Char[] { '.' });
                sName = computer_name[0].ToString();
            }
            catch
            {
            }
            return sName;
        }  

        public static void SetPropertiesForReport(ReportDocument rp, string nguoilap)
        {
            var txtNguoiLap = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if(txtNguoiLap != null)
                txtNguoiLap.Text = nguoilap;
        }

        public static int MonthFromBarcode(string barcode)
        {
            var monthString = barcode.Substring(1, 1);
            const string Digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            return Digits.IndexOf(monthString, StringComparison.OrdinalIgnoreCase) + 1;
        }

        public static int YearFromBarcode(string barcode)
        {
            var yearString = barcode.Substring(0, 1);
            const string Digits = "0123456789";
            return Digits.IndexOf(yearString, StringComparison.OrdinalIgnoreCase) + 2012;
        }

        public static string GetIpAdddressComputerName()
        {
            try
            {
                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();
                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
                IPAddress[] addr = ipEntry.AddressList;
                return addr[addr.Length - 1].ToString();
            }
            catch
            {
                return "Unknow";
            }
        }

        public static string GetLanIPAddressM()
        {
            try
            {
                /*string ipaddress;
                ipaddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (ipaddress == "" || ipaddress == null)
                    ipaddress = Request.ServerVariables["REMOTE_ADDR"];
                Response.Write("IP Address : " + ipaddress);*/

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                //if (ip == null)
                //{
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    return ip;
                //}
            }
            catch
            {
                return "Unknow";
            }
        }
       

        public static string ClientIP()
        {
            var curRequest = HttpContext.Current.Request;
            return curRequest.Params["REMOTE_ADDR"];
        }

        public static string ClientHost()
        {
            try
            {
                var clientIP = HttpContext.Current.Request.UserHostName;
                if (clientIP == null) return "";
                //var myIP = IPAddress.Parse(clientIP);
                var myIP = PhysicalAddress.Parse(clientIP);
                //var GetIPHost = Dns.GetHostEntry(myIP);
                var GetIPHost = Dns.GetHostEntry(myIP.ToString());


                //var compName = GetIPHost.HostName.Split('.').ToList();
                var compName = GetIPHost.ToString();
                //return compName.First();
                return compName;
            }
            catch (Exception)
            {
                return "";
            }

        }

        public static string GetMACAddress0()
        {
            //NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;

            var adapter = nics[0];
            sMacAddress = adapter.GetPhysicalAddress().ToString();
            /*foreach (NetworkInterface adapter in g)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    //IPInterfaceProperties properties = adapter.GetIPProperties();
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    //sMacAddress = adapter.GetPhysicalAddress().ToString();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                    
                }
            } */
            return sMacAddress;
        }

        public static string GetMACAddress1()
        {
            //NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;

            var adapter = nics[1];
            sMacAddress = adapter.GetPhysicalAddress().ToString();
           
            return sMacAddress;
        }

    }
}