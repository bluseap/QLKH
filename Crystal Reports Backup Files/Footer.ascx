<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="EOSCRM.Web.UserControls.Footer" CodeBehind="Footer.ascx.cs" %>
<div id="cfooter">
    <div class="hori">
    </div>
    <div id="footer">
        <div id="flft" class="fl">
            <div class="copy">
                © Công Ty Cổ Phần Điện Nước An Giang - AN GIANG POWER AND WATER SUPPLY JOINT STOCK COMPANY - POWACO</div>
        </div>
        <div id="frgh" class="fr">
            <%--<a href="#">Return to top</a>--%></div>
    </div>
</div>

<script type="text/javascript">
		function px2Int(str)
		{
			return parseFloat(str.replace(/px/, ""));
		}
		function resizeccr(){			
			h_chead = $("#cheader").height();
			h_cmenu = $("#cmenu").height();			
			br_cmenu = px2Int($("#cmenu").css("border-top-width")) + px2Int($("#cmenu").css("border-bottom-width"));			
			h_content = px2Int($("#ccontent").css("padding-top")) + px2Int($("#ccontent").css("padding-bottom"));
			h_foot = $("#cfooter").height() + px2Int($("#cfooter").css("margin-top"));
			
			totalHeight = h_chead + h_cmenu + br_cmenu + h_content + h_foot + 32;
			$("#ccontent").css("height",$(window).height()-totalHeight);
		}
		$(document).ready(function(){
			ddsmoothmenu.init({
				mainmenuid: "cmenu", orientation: 'h', classname: 'ddsmoothmenu', contentsource: "markup"
			})
			resizeccr();
			$(window).resize(function() {
				resizeccr();
			});

		});	
</script>

