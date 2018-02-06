/*
 * Process checking item in grid
 * Khoa Nguyen
 */
function CheckAllItems(sender)
{
    ToggleAll(sender.checked);
}
function IsAllChecked()
{
    var items = document.getElementsByTagName("input");
    var chkAllTop = document.getElementById('chkAllTop');
    
    for (i=0; i<items.length; i++) {
        if (items[i].type == "checkbox") {
            if (items[i] != chkAllTop) {
                if (items[i].checked == false) {
                    return false;
                }
            }
        }
    }
    return true;
}
function ToggleAll(checked)
{
    var items = document.getElementsByTagName("input");
    for (i=0; i<items.length; i++) {
        if (items[i].type == "checkbox") {
            items[i].checked = checked;
        }
    }
}
function DoCheckItem()
{
    if (IsAllChecked()) {
        ToggleAll(true);
    } else {
        document.getElementById("chkAllTop").checked = false;
    }
}
 
 /*
 * Process checking item in multi grid
 * Khoa Nguyen
 */
function ToggleAllEx(checked,grdName) {
    var items = document.getElementsByTagName("input");
    for (i=0; i<items.length; i++) {
        var prefix = items[i].name.substring(0, grdName.length);
        if ((items[i].type == "checkbox") && (prefix == grdName)) {
            items[i].checked = checked;
        }
    }
}
function IsAllCheckedEx(checkAllId,grdName)
{
    var items = document.getElementsByTagName("input");
    var chkAllTop = document.getElementById(checkAllId);
    
    for (i=0; i<items.length; i++) {
        var prefix = items[i].name.substring(0, grdName.length);
        if ((items[i].type == "checkbox") && (prefix == grdName)) {
            if (items[i] != chkAllTop) {
                if (items[i].checked == false) {
                    return false;
                }
            }
        }
    }
    return true;
}
function CheckAllItemsEx(sender,grdName)
{
    ToggleAllEx(sender.checked,grdName);
}
function DoCheckItemEx(checkAllId,grdName)
{
    if (IsAllCheckedEx(checkAllId,grdName)) {
        document.getElementById(checkAllId).checked = true;
    } else {
        document.getElementById(checkAllId).checked = false;
    }
}
function CheckAllParentItemsEx(sender,grdParentName,grdSubName)
{
    ToggleAllEx(sender.checked,grdParentName);
    ToggleAllEx(sender.checked,grdSubName);
}
function DoCheckParentItemEx(sender,checkAllParentId,grdParentName,grdSubName)
{
    // update parent
    if (IsAllCheckedEx(checkAllParentId,grdParentName)) {
        document.getElementById(checkAllParentId).checked = true;
    } else {
        document.getElementById(checkAllParentId).checked = false;
    }
    // update sub
    ToggleAllEx(sender.checked,grdSubName);
}
function DoCheckSubItemEx(sender,checkAllParentId,checkAllSubId,grdParentName,grdSubName)
{
    if (IsAllCheckedEx(checkAllSubId,grdSubName)) {
        document.getElementById(checkAllSubId).checked = true;
        DoCheckParentItemEx(sender,checkAllParentId,grdParentName,grdSubName);
    }
    else
    {
        document.getElementById(checkAllSubId).checked = false;
        document.getElementById(checkAllParentId).checked = false;
    }
}
/*
 * End Khoa Nguyen
 *
 */
function SetSelectedIds(hiddenId,grdName,strName,action)
{
    var items = document.getElementsByTagName("input");
    var strIds = "";
    
    for (i=0; i<items.length; i++) {
        var prefix = items[i].name.substring(0, grdName.length);
        if ((items[i].type == "checkbox") && (prefix == grdName)) {
            if (items[i].checked == true) {
                if(strIds == "") {
                    strIds = items[i].value;
                }
                else {
                    strIds = strIds + "," + items[i].value;
                }
            }
        }
    }
    // Set value
    document.getElementById(hiddenId).value = strIds;
    
    // Check input
    if (strIds.length == 0) {
        alert('Please check ' + strName + ' to ' + action + '.');
        return false;
    } else {
        if (confirm('Are you sure you want to ' + action + ' checked ' + strName + '?') == true) {
            return true;
        } else {
            return false;
        }
    }
}
function CheckRecordSelected(action)
{    
    var frm = document.forms[0];    
    var len = frm.elements.length;    
    var i = 0;    
    for (i = 0; i < len; i++)
    {
        if (frm.elements[i].checked) {
            if (action == 'delete')
                return confirm('Xóa các record được chọn?'); 
        }             
    }

    if (action == 'delete') {
        alert('Vui lòng chọn record cần xóa trước.');
    }
    
    return false;
}
//Create by Huy.Ly
//Solution for delete record with UseSubmitBehavior="false"
function deleteRecord() {
    if (CheckRecordSelected('delete')) {
        __doPostBack('ctl00$ContentPlaceHolderMain$btn_Delete', '');
        return false;
    }
}

//Create by Huy.Ly
//Solution for delete record with UseSubmitBehavior="false"
function checkSelectedIds(id, grd, name, action, button) {
    if (SetSelectedIds(id, grd, name, action)) {
        __doPostBack('ctl00$ContentPlaceHolderMain$' + button, '');
    }
}

// Check all checkbox item that have the name in ids
// ids is list server ID of checkbox. Exp: "chkRead, chkUpdate"
function SelectAll(ids, value) 
{   
    var frm = document.forms[0];
    var idArr = ids.split(",");
    for (i=0;i<frm.elements.length;i++) 
    {
        for(j=0;j<idArr.length;j++)
        {
            if (frm.elements[i].type == "checkbox" && !frm.elements[i].disabled && frm.elements[i].name.indexOf(idArr[j], 0) != -1) 
            {
                frm.elements[i].checked = value;
            }
        }
    }
} 

// controlId is ID of the check all checkbox that will change when check items change
function ChangeCheck(controlId, value, ids)
{
    if(value==true)
    {
        var frm = document.forms[0];
        var idArr = ids.split(",");
        
        for (i=0;i<frm.elements.length;i++) 
        {
            for(j=0;j<idArr.length;j++)
            {
                if (frm.elements[i].type == "checkbox" 
                    && frm.elements[i].name.indexOf(idArr[j],0) != -1
                    && frm.elements[i].checked == false) 
                {
                    document.getElementById(controlId).checked = false;
                    return;
                }
            }
        }
        document.getElementById(controlId).checked = true;            
    }
    else
    {
        document.getElementById(controlId).checked = false;
    }
}

// Check or uncheck all item that exist in same row of sender
// send der is checbox control
function CheckAllInRow(sender)
{
    var row = sender.parentNode.parentNode;
    var items = row.getElementsByTagName("input");    
    for (i=0;i<items.length;i++) 
    {            
        if (items[i].type == "checkbox") 
        {
            items[i].checked = sender.checked;
        }            
    }
}

// tagetId is ID of the main checkbox in row
// parentTier is the tier number for item to targetId's row 
// sender is checkbox that call this function
function UpdateRowState(sender, targetId, parentTier)
{
    var row = sender.parentNode;
    var checkCount = 0;
    
    for(i=0;i< parentTier;i++)
    {
        row = row.parentNode;
    }
    var items = row.getElementsByTagName("input");   
    
    for (i=0;i<items.length;i++) 
    {            
        if (items[i].type == "checkbox" && items[i].name.indexOf(targetId, 0) == -1 && items[i].checked == true) 
        {
            checkCount++;
        }                            
    }
    for (i=0;i<items.length;i++) 
    {            
        if (items[i].type == "checkbox" && items[i].name.indexOf(targetId, 0) != -1) 
        {            
            items[i].checked = !(checkCount == 0);
        }                    
    }
}

//KeyPressValidated
function KeyPressValidated(e){    
    if(window.event){
        return CheckPatterm(e.keyCode);
    }
    else if(e.which){
        if(e.which == 8){
            return true;
        }
        else{
            return CheckPatterm(e.which);
        }
    }
}
function CheckPatterm(value){    
    var flag = false;
    if(value > 47 && value < 58){
        flag = true;
    }
    switch(value){
        case 32:
            flag = true;
            break;
        case 40:
            flag = true;
            break;
        case 41:
            flag = true;
            break;
        case 45:
            flag = true;
            break;
    }
    return flag;
}
function KeyPressValidatedNumber(e){       
    if(window.event){        
        if(e.keyCode == 46){
            return true;
        }
        if(e.keyCode > 47 && e.keyCode < 58){
            return true;
        }
        else{
            return false;
        }
    }
    else if(e.which){        
        if(e.which == 8 || e.which == 46){
            return true;
        }
        else{
            if(e.which > 47 && e.which < 58){
                return true;
            }
            else{
                return false;
            }
        }
    }
}

function KeyPressValidatedEmail(e){   
    if(window.event){        
        return CheckPattermEmail(e.keyCode);
    }
    else if(e.which){
        return CheckPattermEmail(e.which);
    }
}

function CheckPattermEmail(value){
    var flag = true;
    switch(value){
        case 32:
            flag = false;
            break;
        case 33:
            flag = false;
            break;
        case 34:
            flag = false;
            break;
        case 35:
            flag = false;
            break;
        case 36:
            flag = false;
            break;
        case 37:
            flag = false;
            break;
        case 38:
            flag = false;
            break;
        case 39:
            flag = false;
            break;
        case 40:
            flag = false;
            break;
        case 41:
            flag = false;
            break;
        case 42:
            flag = false;
            break;
        case 43:
            flag = false;
            break;
        case 44:
            flag = false;
            break;
        case 45:
            flag = false;
            break;
        case 47:
            flag = false;
            break;
        case 58:
            flag = false;
            break;
        case 59:
            flag = false;
            break;
        case 60:
            flag = false;
            break;
        case 61:
            flag = false;
            break;
        case 62:
            flag = false;
            break;
        case 63:
            flag = false;
            break;
        case 91:
            flag = false;
            break;
        case 92:
            flag = false;
            break;
        case 93:
            flag = false;
            break;
        case 94:
            flag = false;
            break;
        case 96:
            flag = false;
            break;
        case 123:
            flag = false;
            break;
        case 124:
            flag = false;
            break;
        case 125:
            flag = false;
            break;
        case 126:
            flag = false;
            break;
    }
    return flag;
}

function CheckSpecialCharater(e){    
    if(window.event){        
        return IsSpecialCharacter(e.keyCode);
    }
    else if(e.which){
        return IsSpecialCharacter(e.which);
    }    
}
function RemoveDigit(id){
    var value = document.getElementById(id).value;
    var newvalue = "";
    for(i = 0; i < value.length; i++){
        if(value.charAt(i) != '0' && value.charAt(i) != '1' && value.charAt(i) != '2' && 
                value.charAt(i) != '3' && value.charAt(i) != '4' && value.charAt(i) != '5' && value.charAt(i) != '6' && 
                value.charAt(i) != '7' && value.charAt(i) != '8' && value.charAt(i) != '9'){
            newvalue = newvalue + value.charAt(i);
        }       
    }
    document.getElementById(id).value = newvalue;
}
function IsSpecialCharacter(value){
    var flag = true;        
    switch(value){
        case 33:
            flag = false;
            break;
        case 34:
            flag = false;
            break;
        case 35:
            flag = false;
            break;
        case 36:
            flag = false;
            break;
        case 37:
            flag = false;
            break;
        case 38:
            flag = false;
            break;
        case 38:
            flag = false;
            break;
        case 40:
            flag = false;
            break;
        case 41:
            flag = false;
            break;
        case 42:
            flag = false;
            break;
        case 43:
            flag = false;
            break;
        case 44:
            flag = false;
            break;
        case 46:
            flag = false;
            break;
        case 47:
            flag = false;
            break;
        case 58:
            flag = false;
            break;
        case 59:
            flag = false;
            break;
        case 60:
            flag = false;
            break;
        case 61:
            flag = false;
            break;
        case 62:
            flag = false;
            break;
        case 63:
            flag = false;
            break;
        case 64:
            flag = false;
            break;
        case 91:
            flag = false;
            break;
        case 92:
            flag = false;
            break;
        case 93:
            flag = false;
            break;
        case 94:
            flag = false;
            break;
        case 95:
            flag = false;
            break;
        case 96:
            flag = false;
            break;
        case 123:
            flag = false;
            break;
        case 124:
            flag = false;
            break;
        case 125:
            flag = false;
            break;
        case 126:
            flag = false;
            break;
    }    
    return flag;
}

function CompareDate(id1, id2, message){    
    var flag = true;
    var arrString1 = document.getElementById(id1).value.split("/");
    if(arrString1.length != 3){
        return;
    }
    var arrString2 = document.getElementById(id2).value.split("/");
    if(arrString2.length != 3){
        return;
    }
    var year1 = parseInt(arrString1[2]);
    var day1 = parseInt(arrString1[1]);
    var month1 = parseInt(arrString1[0]);
    var year2 = parseInt(arrString2[2]);
    var day2 = parseInt(arrString2[1]);
    var month2 = parseInt(arrString2[0]);
    if(year1 < year2){
        flag = false;
    }
    else{
        if(year1 == year2){
            if(month1 < month2){
                flag = false;
            }
            if(month1 == month2){
                if(day1 < day2){
                    flag = false;
                }
            }
        }
    }
    if(flag == false){
        alert(message);
        document.getElementById(id1).focus();
    }        
}

function KeyPressValidatedDate(e){    
    if(window.event){        
        if(e.keyCode == 45){
            return true;
        }
        if(e.keyCode > 46 && e.keyCode < 58){
            return true;
        }
        else{
            return false;
        }
    }
    else if(e.which){        
        if(e.which == 8 || e.which == 45){
            return true;
        }
        else{
            if(e.which > 46 && e.which < 58){
                return true;
            }
            else{
                return false;
            }
        }
    }
}

// Declaring valid date character, minimum year and maximum year
var dtCh= "/";
var minYear=1900;
var maxYear=2100;

function isInteger(s){
	var i;
    for (i = 0; i < s.length; i++){   
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}

function stripCharsInBag(s, bag){
	var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++){   
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}

function daysInFebruary (year){
	// February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
}
function DaysArray(n) {
	for (var i = 1; i <= n; i++) {
		this[i] = 31
		if (i==4 || i==6 || i==9 || i==11) {this[i] = 30}
		if (i==2) {this[i] = 29}
   } 
   return this
}

function isDate(dtStr){
	var daysInMonth = DaysArray(12);
	var pos1=dtStr.indexOf(dtCh);
	var pos2=dtStr.indexOf(dtCh,pos1+1);
	var strDay=dtStr.substring(0,pos1);
	var strMonth=dtStr.substring(pos1+1,pos2);
	var strYear=dtStr.substring(pos2+1);
	strYr=strYear;
	if (strDay.charAt(0)=="0" && strDay.length>1) strDay=strDay.substring(1);
	if (strMonth.charAt(0)=="0" && strMonth.length>1) strMonth=strMonth.substring(1);
	for (var i = 1; i <= 3; i++) {
		if (strYr.charAt(0)=="0" && strYr.length>1) 
		    strYr=strYr.substring(1);
	}
	month=parseInt(strMonth);
	day=parseInt(strDay);
	year=parseInt(strYr);
	if (pos1==-1 || pos2==-1){
		alert("Vui lòng nhập đúng định dạng dd/MM/yyyy");
		return false;
	}
	if (strMonth.length<1 || month<1 || month>12){
		alert("Vui lòng nhập tháng hợp lệ.");
		return false;
	}
	if (strDay.length<1 || day<1 || day>31 || (month==2 && day>daysInFebruary(year)) || day > daysInMonth[month]){
		alert("Vui lòng nhập ngày hợp lệ.");
		return false;
	}
	if (strYear.length != 4 || year==0 || year<minYear || year>maxYear){
		alert("Vui lòng nhập năm hợp lệ nằm giữa "+minYear+" và "+maxYear);
		return false;
	}
	if (dtStr.indexOf(dtCh,pos2+1)!=-1 || isInteger(stripCharsInBag(dtStr, dtCh))==false){
		alert("Vui lòng nhập ngày tháng hợp lệ.");
		return false;
	}
    return true;
}

function isNotNull(id){
    if(document.getElementById(id).value ==""){
        document.getElementById(id).value = "0";        
    }
    if(isNaN(document.getElementById(id).value)){
        document.getElementById(id).value = "0";
    }
    document.getElementById(id).style.border = "dotted 0px black";
    return false;
}

function acceptFocus(id){
    document.getElementById(id).style.border = "dotted 1px black";
    document.getElementById(id).style.height = "14px";
    document.getElementById(id).select();
    //alert(document.getElementById(id).style.border);
}

function showOrhide(id){
    if(document.getElementById(id).style.display == "block"){
        document.getElementById(id).style.display = "none";
    }
    else{
        document.getElementById(id).style.display = "block";
    }
}

function applyAll(l1, l2, l3, l4, l5, l6, l7, l8, l9, l10, l11, l12){    
    document.getElementById(l2).value = document.getElementById(l1).value;
    document.getElementById(l3).value = document.getElementById(l1).value;
    document.getElementById(l4).value = document.getElementById(l1).value;
    document.getElementById(l5).value = document.getElementById(l1).value;
    document.getElementById(l6).value = document.getElementById(l1).value;
    document.getElementById(l7).value = document.getElementById(l1).value;
    document.getElementById(l8).value = document.getElementById(l1).value;
    document.getElementById(l9).value = document.getElementById(l1).value;
    document.getElementById(l10).value = document.getElementById(l1).value;
    document.getElementById(l11).value = document.getElementById(l1).value;
    document.getElementById(l12).value = document.getElementById(l1).value;
}

function editItem(id, v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, l1, l2, l3, l4, l5, l6, l7, l8, l9, l10, l11, l12){
    var items = document.getElementsByTagName("input");
    var flag = false;
    for (i=0;i<items.length;i++) 
    {            
        if (items[i].type == "checkbox") 
        {
            if(items[i].checked == true){                
                flag = true;
                break;
            }
        }            
    }
    if(flag){
        document.getElementById(id).style.display = "block";
        scrollControl(id);
        document.getElementById(l1).value = v1;
        document.getElementById(l2).value = v2;
        document.getElementById(l3).value = v3;
        document.getElementById(l4).value = v4;
        document.getElementById(l5).value = v5;
        document.getElementById(l6).value = v6;
        document.getElementById(l7).value = v7;
        document.getElementById(l8).value = v8;
        document.getElementById(l9).value = v9;
        document.getElementById(l10).value = v10;
        document.getElementById(l11).value = v11;
        document.getElementById(l12).value = v12;
    }
    else{
        document.getElementById(id).style.display = "none";
    }    
}
function scrollControl(id){
    if(window.screen.height<850){ 
        document.getElementById(id).style.top = (document.documentElement.scrollTop + 150) + "px"; 
    }
    else{
        document.getElementById(id).style.top = (document.documentElement.scrollTop + 250) + "px";
    }    
}
function unEditItem(id){
    var items = document.getElementsByTagName("input");
    var flag = false;
    for (i=0;i<items.length;i++) 
    {            
        if (items[i].type == "checkbox") 
        {
            items[i].checked = false;
        }            
    }
    document.getElementById(id).style.display = "none";
}

//HUYDENY
////////////////////////
// JScript File
var popUpWin = 0;
function popUpWindow(URLStr, width, height) {
    var left = (screen.width / 2) - width / 2;
    var top = (screen.height / 2) - height / 2;
    open(URLStr, '', 'toolbar=no,location=no,directories=no,status=no,menubar=no,resizable=no,scrollbar=yes,copyhistory=yes,width=' + width + ',height=' + height + ',left=' + left + ', top=' + top + ',screenX=' + left + ',screenY=' + top + '');
}
function popUp(URLStr, width, height) {
    var left = (screen.width / 2) - width / 2;
    var top = (screen.height / 2) - height / 2;
    if (popUpWin) {
        if (!popUpWin.closed) popUpWin.close();
    }
    popUpWin = open(URLStr, 'popUpWin', 'scrollbars=yes,width=' + width + ',height=' + height + ',left=' + left + ', top=' + top + ',screenX=' + left + ',screenY=' + top + '');
}

function LTrim(value) {
    var re = /\s*((\S+\s*)*)/;
    return value.replace(re, "$1");
}

// Removes ending whitespaces
function RTrim(value) {
    var re = /((\s*\S+)*)\s*/;
    return value.replace(re, "$1");
}

// Removes leading and ending whitespaces
function trim(value) {
    return LTrim(RTrim(value));
}

function ToggleGroup(groupDiv) {
    if (document.getElementById(groupDiv).style.display == "")
        document.getElementById(groupDiv).style.display = "none";
    else
        document.getElementById(groupDiv).style.display = "";
}

/**
 * Handler for checking on parent checkbox event
 *
 * @param   sender  :   checkbox conrol
 * @param   prefix  :   prefix of checkbox's name 
 * @param   parent  :   parent checkbox conrol
 * @param   subdiv  :   div contains child gridview  
 * @param   hiddenfield  :   hidden field contains user data  
 *
 **/
function CheckParentItem(sender, parent, prefix, subdiv, hiddenfield) {

    //TODO: check if all check boxes in sub div is checked,
    //      if true: set the "chkAllTopGrid" check box to checked
    //      if false: set the "chkAllTopGrid" check box to unchecked
    if (IsAllCheckedInSameLevel(parent, prefix)) {
        document.getElementById(parent).checked = true;
    }
    else {
        document.getElementById(parent).checked = false;
    }
    
    //TODO: toggle all checkboxes in child gridview
    ToggleCheckBoxes(sender, subdiv);

    //TODO: add to hidden field
    AppendHiddenField(hiddenfield);
}

function CheckChildItem(sender, top, parent, parentprefix, prefix, subdiv, hiddenfield) {
    //TODO: check if all check boxes in sub div is checked,
    //      if true: set the "chkAllTopGrid" check box to checked
    //      if false: set the "chkAllTopGrid" check box to unchecked
    if (IsAllCheckedInSameLevel(parent, prefix)) {
        document.getElementById(parent).checked = true;
        CheckParentItem(sender, top, parentprefix, subdiv, hiddenfield);
    }
    else {
        document.getElementById(parent).checked = false;
        document.getElementById(top).checked = false;

        //TODO: add to hidden field
        AppendHiddenField(hiddenfield);
    }
}

/**
 * Toogle all checkboxes "checked" state to theirs parent "checked" state
 *
 * @param   sender  :   parent checkbox conrol
 * @param   subdiv  :   div contains child gridview  
 *
 **/
function ToggleCheckBoxes(sender, subdiv) {
    var items = document.getElementById(subdiv).getElementsByTagName('input');
    for (i = 0; i < items.length; i++) {
        if ((items[i].type == "checkbox")) {
            items[i].checked = sender.checked;
        }
    }
}

/**
 * Append "checked" checkboxes to hidden field
 * @param   hiddenField :    hidden field control's id 
 *
 **/
function AppendHiddenField(hiddenField) {
    var objs = document.getElementsByClassName('checkedPhuong');
    var val = '';
    if (objs) {
        for (var i = 0; i < objs.length; i++) {
            if (objs[i].checked == true) {
                val += ', ' + objs[i].value;
            }
        }
    }

    // if textarea contains semicolon character at the beginning, remove it
    if (val.substring(0, 2) == ', ') {
        val = val.substring(2);
    }

    // update text area
    var list = document.getElementById(hiddenField);
    list.value = val;
}

/**
 * Check whether all same level check boxes are checked
 * @param   parent  :   parent control
 * @param   prefix  :   control's name prefix *
 *
 **/
function IsAllCheckedInSameLevel(parent, prefix) {
    var items = document.getElementsByTagName("input");
    var chkAllTop = document.getElementById(parent);

    for (i = 0; i < items.length; i++) {
        var pref = items[i].name.substring(0, prefix.length);
        if ((items[i].type == "checkbox") && (pref == prefix)) {
            if (items[i].checked == false) {
                return false;
            }
        }
    }
    
    return true;
}

function CheckTop(sender, gridid, hiddenField) {
    ToggleCheckBoxes(sender, gridid);   

    //TODO: add to hidden field
    AppendHiddenField(hiddenField);
}


function IsNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}






function showInfor(message) {
    jAlert('infor', message, 'Thông báo');
}

function showError(message) {
    jAlert('error', message, 'Lỗi');
}

function showErrorWithFocus(message, controlId) {
    jAlert('error', message, 'Lỗi', function(r) {
        $("#" + controlId + "").focus();
    });
}

function FocusAndSelect(controlId) {
    $("#" + controlId).focus();
    $("#" + controlId).select();
}

function showErrorWithFocusSelect(message, controlId) {
    jAlert('error', message, 'Lỗi', function(r) {
        FocusAndSelect(controlId);
    });
}

function showWarning(message) {
    jAlert('warning', message, 'Cảnh báo');
}

function setTextBoxText(textboxId, value) {
    $("#" + textboxId + "").val(value);
}

function setLabelText(labelId, value) {
    $("#" + labelId + "").html(value);
}

function setControlValue(controlId, value) {
    $("#" + controlId + "").val(value);
}

function setReadonly(controlId) {
    $("#" + controlId + "").attr("readonly", true);
}

function removeReadonly(controlId) {
    if ($("#" + controlId + "").attr("readonly") == true) {
        $("#" + controlId + "").val('');
        $("#" + controlId + "").removeAttr("readonly"); 
    }
}

function alertWithFocusSelect(message, controlId) {
    alert(message);    
    FocusAndSelect(controlId);
}

function isUnsignedInteger(n) {
    return (n.toString().search(/^[0-9]+$/) == 0);
}

/***
control behaviour in ghi chi so
*/
