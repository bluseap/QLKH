using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Web.Common;

namespace EOSCRM.Web.UserControls
{
    public partial class Header : UserControl
    {
        private readonly FunctionDao dao = new FunctionDao();
        protected string loginName, hoten;
        protected string modulename = "";
        protected string titlepage = "";

        protected string tencongty = "CÔNG TY CỔ PHẦN ĐIỆN NƯỚC AN GIANG";


        public string ModuleName
        {
            get { return modulename; }
            set { modulename = value; }
        }

        public string TenCongTy
        {
            get { return tencongty; }
            set { tencongty = value; }
        }

        public string TitlePage
        {
            get { return titlepage; }
            set { titlepage = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;

            if (loginInfo == null) return;

            loginName = loginInfo.Username;
            hoten = loginInfo.HoTen;
            LoadMenu();
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect(WebUrlConstants.HOME_PAGE);
        }

        protected void lnkbtnLogOut_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect(WebUrlConstants.LOGIN_PAGE);
        }

        private void LoadMenu()
        {
            var list = dao.GetList(0);
            repeater.DataSource = list;
            repeater.DataBind();
        }

        protected void repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (!e.Item.ItemType.Equals(ListItemType.Item) &&
                !e.Item.ItemType.Equals(ListItemType.AlternatingItem)) return;

            var item = (CrmFunction)(e.Item.DataItem);
            if (item == null) return;
            var list = dao.GetList(item.Id, loginName);

            var child = (Repeater)e.Item.FindControl("childRepeater");

            if (child == null) return;

            child.DataSource = list;
            child.DataBind();

            if (list.Count == 0)
                e.Item.Visible = false;
        }

        protected void childRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (!e.Item.ItemType.Equals(ListItemType.Item) &&
                !e.Item.ItemType.Equals(ListItemType.AlternatingItem)) return;

            var item = (CrmFunction)(e.Item.DataItem);
            if (item == null) return;
            var list = dao.GetList(item.Id, loginName);

            var child = (Repeater)e.Item.FindControl("childRepeater2");

            if (child == null) return;

            child.DataSource = list;
            child.DataBind();

            if (list.Count == 0)
                child.Visible = false;
        }

        protected void childRepeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (!e.Item.ItemType.Equals(ListItemType.Item) &&
                !e.Item.ItemType.Equals(ListItemType.AlternatingItem)) return;

            var item = (CrmFunction)(e.Item.DataItem);
            if (item == null) return;
            var list = dao.GetList(item.Id, loginName);

            var child = (Repeater)e.Item.FindControl("childRepeater3");

            if (child == null) return;

            child.DataSource = list;
            child.DataBind();

            if (list.Count == 0)
                child.Visible = false;
        }
    }
}