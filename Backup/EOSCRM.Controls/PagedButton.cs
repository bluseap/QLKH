using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EOSCRM.Controls
{
    public class PagedButton : Button
    {
        #region Private fields

        private readonly IPostBackContainer _container;
        private bool _enableCallback;
        private string _callbackArgument;

        #endregion

        public PagedButton(IPostBackContainer container)
		{
			_container = container;
		}

		public void EnableCallback(string argument)
		{
			_enableCallback = true;
			_callbackArgument = argument;
		}

		public override bool CausesValidation
		{
			get { return false; }
			set { throw new ApplicationException("Cannot set validation on pager buttons"); }
		}

		protected override void Render(HtmlTextWriter writer)
		{
			SetCallbackProperties();
			base.Render(writer);
		}

		private void SetCallbackProperties()
		{
		    if (!_enableCallback) return;

		    var container = _container as ICallbackContainer;
		    if (container == null) return;
		    var callbackScript = container.GetCallbackScript(this, _callbackArgument);
		    if (!string.IsNullOrEmpty(callbackScript)) 
                OnClientClick = callbackScript;
		}
    }
}
