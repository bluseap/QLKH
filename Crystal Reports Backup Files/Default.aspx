<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="true"
    Inherits="EOSCRM.Web.Forms.Default.Default" CodeBehind="Default.aspx.cs" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Import Namespace="EOSCRM.Web.Common" %>
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">  
    <link type="text/css" href="<%= ResolveUrl("~")%>content/css/grid.css" rel="stylesheet" />

    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
    <script type="text/javascript">

        //if (screen.width <= 800) {
        //    window.location = "http://powaco.com.vn/WebMobi/KhachHang/MNhapDLDM.aspx";
        //}

        $(document).ready(function () {
            // Editable vars
            var clickSpeed = 500; // the speed of animation when clicking a nav button
            var autoSpeed = 2000; // when autoscrolling to right
            var repeatSpeed = 2000; // time when to repeat the autoScroll

            // Non editable vars
            var scroll
            var timerID = null;

            // Set the left css style - so that we have the first element hidden on the left
            // using setTimeout - for Chrome
            setTimeout(function () {
                var w = $('#carousel_ul li:first').outerWidth() + 10;
                $('#carousel_ul').css({ 'left': w * -1 });
            }, 1);

            //when user clicks the image for sliding right        
            $('#right_scroll img').click(function () {
                scrollRigth(clickSpeed);
            });

            //when user clicks the image for sliding left
            $('#left_scroll img').click(function () {
                clearInterval(timerID); // clear the interval - disable autoscrolling
                disableNavButtons(); // disable Nav Buttons when scrolling is happening

                var item_width = $('#carousel_ul li').outerWidth() + 10;

                // same as for sliding right except that it's current left indent + the item width (for the sliding right it's - item_width) 
                var left_indent = parseInt($('#carousel_ul').css('left')) + item_width;
                var $last = $('#carousel_ul li:last');

                $('#carousel_ul:not(:animated)').animate({ 'left': left_indent }, clickSpeed, function () {
                    var width = $last.outerWidth() + 10;
                    $('#carousel_ul li:first').before($last);
                    $('#carousel_ul').css({ 'left': width * -1 });

                    enableNavButtons(); // Enable Nav Buttons back
                    timerID = null;
                    startAutoScroll(); // reset timer
                });
            });

            function scrollRigth(speed) {
                clearInterval(timerID); // clear the interval - disable autoscrolling
                disableNavButtons(); // disable Nav Buttons when scrolling is happening

                // get the width of the second element - the first item is hidden, and we need to scroll it using the second item
                var item_width = $($('#carousel_ul li')[1]).outerWidth() + 10;

                // calculae the new left indent of the unordered list
                var left_indent = parseInt($('#carousel_ul').css('left')) - item_width;

                // get the first list item and put a clone of it after the last list item'
                var $first = $('#carousel_ul li:first');
                $('#carousel_ul li:last').after($first.clone());

                //make the sliding effect using jquery's anumate function '
                $('#carousel_ul:not(:animated)').animate({ 'left': left_indent }, speed, function () {
                    //and get the left indent to the new value
                    $('#carousel_ul').css({ 'left': item_width * -1 });
                    $first.remove(); // remove the original item that was cloned

                    enableNavButtons();	// Enable Nav Buttons back
                    timerID = null;
                    startAutoScroll(); // reset timer
                });
            }

            function enableNavButtons() {
                $('#right_scroll img').removeAttr('disabled');
                $('#left_scroll img').removeAttr('disabled');
            }

            function disableNavButtons() {
                $('#right_scroll img').attr('disabled', true);
                $('#left_scroll img').attr('disabled', true);
            }

            function startAutoScroll() {
                if (timerID === null) { // to avoid multiple registration
                    timerID = setInterval(function () { scrollRigth(autoSpeed); }, repeatSpeed);
                }
            }
            startAutoScroll();
        });

    </script>

    <style type="text/css">

    #carousel_inner {
    float:left; /* important for inline positioning */
    width:970px;  /*important (this width = width of list item(including margin) * items shown */ 
    overflow: hidden;  /* important (hide the items outside the div) */
    /* non-important styling bellow */
    background: #F0F0F0;
    }

    #carousel_ul {
    position:relative;
    list-style-type: none; /* removing the default styling for unordered list items */
    margin: 0px;
    padding: 0px;
    width:9999px; /* important */
    /* non-important styling bellow */
    padding-bottom:10px;
    }

    #carousel_ul li{
    float: left; /* important for inline positioning of the list items */                                    
    /* just styling bellow*/
    padding:0px;
    height:140px;
    background: #000000;
    margin-top:10px;
    margin-bottom:10px; 
    margin-left:5px; 
    margin-right:5px; 
    }

    #carousel_ul li img {
    .margin-bottom:-4px; /* IE is making a 4px gap bellow an image inside of an anchor (<a href...>) so this is to fix that*/
    /* styling */
    cursor:pointer;
    cursor: hand; 
    border:0px; 
    }
    #left_scroll, #right_scroll{
    float:left; 
    height:160px; 
    width:15px; 
    background: #C0C0C0; 
    }
    #left_scroll img, #right_scroll img{
    /*styling*/
    cursor: pointer;
    cursor: hand;
    }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="Server">
    <body>
    <!--<asp:Image ID="Image1" runat="server" 
        ImageUrl="~/content/images/login/tgnuoc.jpg" Height=480 Width=1008 />
    -->
        <div id="main" align="center">
                  <img src="/content/images/login/hp2015.gif" align="middle" ></img>
        </div> 
    <table cellspacing="0" cellpadding="0" width="100%">            
            <tr>            
              <td>
                  <div id="carousel_container">
                      <div id="left_scroll"><img src="/content/images/moi/left.png"></div>
                        <div id="carousel_inner">
                            <ul id="carousel_ul">                      
			                    <li><a href="#"><img src="/content/images/moi/01.jpg"></a></li>
                                <li><a href="#"><img src="/content/images/moi/02.jpg"></a></li>
                                <li><a href="#"><img src="/content/images/moi/03.jpg"></a></li>
                                <li><a href="#"><img src="/content/images/moi/04.jpg"></a></li>
                                <li><a href="#"><img src="/content/images/moi/07.jpg"></a></li>
			                    <li><a href="#"><img src="/content/images/moi/08.jpg"></a></li>
                                <li><a href="#"><img src="/content/images/moi/10.jpg"></a></li>
			                    <li><a href="#"><img src="/content/images/moi/14.jpg"></a></li>
                                <li><a href="#"><img src="/content/images/moi/15.jpg"></a></li>
			                    <li><a href="#"><img src="/content/images/moi/20.jpg"></a></li>            
                            </ul>
                        </div>
                      <div id="right_scroll"><img src="/content/images/moi/right.png"></div>
                  </div>              
            </tr>            
    </table>
    </body>
</asp:Content>
