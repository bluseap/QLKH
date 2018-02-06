using System;
using System.Globalization;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace EOSCRM.Controls
{
    [ToolboxData("<{0}:Grid runat=server></{0}:Grid>")]
    public class Grid : System.Web.UI.WebControls.GridView
    {
        public bool UseCustomPager
        {
            get { return (bool?)ViewState["UseCustomPager"] ?? false; }
            set { ViewState["UseCustomPager"] = value; }
        }

        public string PagerInforText
        {
            get { return ViewState["PagerInforText"]!=null ? ViewState["PagerInforText"].ToString() : ""; }
            set { ViewState["PagerInforText"] = value; }
        }

        protected override void InitializePager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
        {
            if (UseCustomPager)
                CreateCustomPager(row, columnSpan, pagedDataSource);
            else
                base.InitializePager(row, columnSpan, pagedDataSource);
        }

        protected virtual void CreateCustomPager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
        {
            var pageCount = pagedDataSource.PageCount;
            var pageIndex = pagedDataSource.CurrentPageIndex + 1;
            var pageButtonCount = PagerSettings.PageButtonCount;

            var cell = new TableCell();
            row.Cells.Add(cell);
            if (columnSpan > 1) cell.ColumnSpan = columnSpan;

            cell.Attributes["class"] = "footer";
            if (pageCount <= 1) return;

            var crmTable = new HtmlTable();
            crmTable.Attributes["class"] = "pager";
            var crmRow = new HtmlTableRow();
            crmTable.Controls.Add(crmRow); // row: crmgridrow
            var crmCell = new HtmlTableCell();
            crmCell.Attributes["class"] = "cell numeric";
            crmRow.Cells.Add(crmCell); // cell: crmgridcell

            cell.Controls.Add(crmTable); // table: crmgridtable


            var min = pageIndex - pageButtonCount;
            var max = pageIndex + pageButtonCount;

            if (max > pageCount)
                min -= max - pageCount;
            else if (min < 1)
                max += 1 - min;


            // first, previous button
            var crmArrPart1 = new HtmlGenericControl("div");
            crmArrPart1.Attributes["class"] = "wrap part1";
            crmCell.Controls.Add(crmArrPart1); // div: crmarrpart1   

            var first = BuildButton(1, "first", "Page", "First");
            var prev = BuildButton(pageIndex - 2, "prev", "Page", "Prev");
            if (pageIndex < 2)
                prev = BuildButton(1, "prev", "Page", "First");


            crmArrPart1.Controls.Add(first);
            crmArrPart1.Controls.Add(prev);

            // numeric button
            var crmArrNumPart = new HtmlGenericControl("div");
            crmArrNumPart.Attributes["class"] = "wrap number";
            crmCell.Controls.Add(crmArrNumPart); // div: crmnumpart   

            // Create page buttons
            var needDiv = false;
            for (var i = 1; i <= pageCount; i++)
            {
                if (i <= 2 || i > pageCount - 2 || (min <= i && i <= max))
                {
                    var text = i.ToString(NumberFormatInfo.InvariantInfo);
                    var page = i == pageIndex
                                   ? BuildLinkButton(-1, "<span>" + text + "</span>", "current", "Page", "-1")
                                   : BuildLinkButton(i - 1, "<span>" + text + "</span>", "", "Page", text);
                    crmArrNumPart.Controls.Add(page);
                    needDiv = true;
                }
                else if (needDiv)
                {
                    var page = BuildSpan("&hellip;", null);
                    crmArrNumPart.Controls.Add(page);
                    needDiv = false;
                }
            }

            // next, last button
            var crmArrPart2 = new HtmlGenericControl("div");
            crmArrPart2.Attributes["class"] = "wrap part2";
            crmCell.Controls.Add(crmArrPart2); // div: crmarrpart2  

            var next = BuildButton(pageIndex, "next", "Page", "Next");
            var last = BuildButton(pageCount, "last", "Page", "Last");

            crmArrPart2.Controls.Add(next);
            crmArrPart2.Controls.Add(last);

            // info part
            var crmInfoPart = new HtmlGenericControl("div");
            crmInfoPart.Attributes["class"] = "wrap infor";

            var infor = BuildSpan(string.Format("<strong>{0}</strong> {1} trên <strong>{2}</strong> trang", 
                                        PagerInforText,
                                        PagerSettings.FirstPageText, 
                                        pageCount), null);
            
            crmInfoPart.Controls.Add(infor);

            crmCell.Controls.Add(crmInfoPart);  // div: crminfopart  
            
        }

        private string ParentBuildCallbackArgument(int pageIndex)
        {
            var m = typeof(System.Web.UI.WebControls.GridView).GetMethod("BuildCallbackArgument", 
                                                BindingFlags.NonPublic |  BindingFlags.Instance, 
                                                null,
                                                new Type[] { typeof(int) }, 
                                                null);
            if (m == null) return "";
            return (string)m.Invoke(this, new object[] { pageIndex });
        }

        private Control BuildLinkButton(int pageIndex, string text, string css, string commandName, string commandArgument)
        {
            var link = new PagedLinkButton(this) { Text = text };

            if (!String.IsNullOrEmpty(css)) link.Attributes["class"] = css;

            if (pageIndex != -1)
            {
                link.EnableCallback(ParentBuildCallbackArgument(pageIndex));
                link.CommandName = commandName;
                link.CommandArgument = commandArgument;
            }
            else
            {
                link.OnClientClick = "return false;";
            }

            return link;
        }

        private Control BuildButton(int pageIndex, string css, string commandName, string commandArgument)
        {
            var link = new PagedButton(this) { };

            if (!String.IsNullOrEmpty(css)) link.Attributes["class"] = css;

            link.UseSubmitBehavior = false;
            link.EnableCallback(ParentBuildCallbackArgument(pageIndex));
            link.CommandName = commandName;
            link.CommandArgument = commandArgument;


            return link;
        }


        private Control BuildSpan(string text, string cssClass)
        {
            var span = new HtmlGenericControl("span");
            if (!String.IsNullOrEmpty(cssClass)) span.Attributes["class"] = cssClass;
            span.InnerHtml = text;
            return span;
        }
    }
}
