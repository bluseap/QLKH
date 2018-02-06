namespace EOSCRM.Web.Common
{
    /// <summary>
    /// Many components or classes may access session or request
    /// variables. To avoid typing mistakes, we preferred to 
    /// centralize those const strings, and to rely on the compiler
    /// to check coherence.
    /// </summary>
    public class WebConstants
    {
        #region WebConstatns for POWACO web application

        public const string TOTAL_ROWS = "__TotalRows__";
        public const string TOTAL_ROWS_ASSIGN = "__TotalRowsAssign__";
        public const string RESULT_PAGER = "Results: {0} - {1} of {2}";
        public const string IMAGE_URL_ASCENDING = "~/content/images/common/asc.gif";
        public const string IMAGE_URL_DESCENDING = "~/content/images/common/desc.gif";
        public const string TEXT_ASCENDING = "Ascending Order";
        public const string TEXT_DESCENDING = "Descending Order";

        public const string SORT_DIRECTION = "__Direction__";
        public const string SORT_EXPRESSION = "__SortExpression__";
        public const string GRIDVIEW_DATASOURCE = "__SourceGrid__";

        #endregion
    }
}