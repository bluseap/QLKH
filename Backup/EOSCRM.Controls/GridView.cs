using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EOSCRM.Controls
{
    [ToolboxData("<{0}:GridView runat=server></{0}:GridView>")]
    public class GridView : System.Web.UI.WebControls.GridView, IPageableItemContainer
    {
        /// <summary>
        /// TotalRowCountAvailable event key
        /// </summary>
        private static readonly object EventTotalRowCountAvailable = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataBinding"></param>
        /// <returns></returns>
        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            var rows = base.CreateChildControls(dataSource, dataBinding);

            //  if the paging feature is enabled, determine
            //  the total number of rows in the datasource
            if (AllowPaging)
            {
                //  if we are databinding, use the number of rows that were created,
                //  otherwise cast the datasource to an Collection and use that as the count
                var totalRowCount = dataBinding ? rows : ((ICollection)dataSource).Count;

                //  raise the row count available event
                var pageableItemContainer = this as IPageableItemContainer;
                
                OnTotalRowCountAvailable(
                    new PageEventArgs(
                        pageableItemContainer.StartRowIndex,
                        pageableItemContainer.MaximumRows,
                        totalRowCount
                    )
                );

                //  make sure the top and bottom pager rows are not visible
                if (TopPagerRow != null)
                {
                    TopPagerRow.Visible = false;
                }

                if (BottomPagerRow != null)
                {
                    BottomPagerRow.Visible = false;
                }
            }

            return rows;
        }

        #region IPageableItemContainer Interface

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <param name="databind"></param>
        void IPageableItemContainer.SetPageProperties(
            int startRowIndex, int maximumRows, bool databind)
        {
            var newPageIndex = (startRowIndex / maximumRows);
            PageSize = maximumRows;

            if (PageIndex == newPageIndex) return;

            var isCanceled = false;
            if (databind)
            {
                //  create the event args and raise the event
                var args = new GridViewPageEventArgs(newPageIndex);

                OnPageIndexChanging(args);

                isCanceled = args.Cancel;
                newPageIndex = args.NewPageIndex;
            }

            //  if the event wasn't cancelled
            //  go ahead and change the paging values
            if (!isCanceled)
            {
                PageIndex = newPageIndex;

                if (databind)
                {
                    OnPageIndexChanged(EventArgs.Empty);
                }
            }

            if (databind)
            {
                RequiresDataBinding = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        int IPageableItemContainer.StartRowIndex
        {
            get { return PageSize * PageIndex; }
        }

        /// <summary>
        /// 
        /// </summary>
        int IPageableItemContainer.MaximumRows
        {
            get { return PageSize; }
        }

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<PageEventArgs> IPageableItemContainer.TotalRowCountAvailable
        {
            add { Events.AddHandler(EventTotalRowCountAvailable, value); }
            remove { Events.RemoveHandler(EventTotalRowCountAvailable, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTotalRowCountAvailable(PageEventArgs e)
        {
            var handler = (EventHandler<PageEventArgs>)Events[EventTotalRowCountAvailable];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion
    }
}
