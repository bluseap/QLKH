Object.extend(Math, {
	formatNumber:function(n, options){
		var options = Object.extend({
			decimals:0,
			currency:false,
			currencySymbol: '$ ',
			formatWhole:true,
			wholeDelimiter:',',
			decimalDelimiter:'.'
		},options);

		var nArr = new Array();
		nArr = String(n).split('.');
		
		var whole = (typeof nArr[0]!='undefined')?nArr[0]:'0';
		if(options.formatWhole){
			var exp = /(\d+)(\d{3})/;
			while (exp.test(whole)) {
				whole = whole.replace(exp, '$1' + options.wholeDelimiter + '$2');
			}
		}

		if(typeof nArr[1]!='undefined'){
			var remainder = nArr[1];
		}else{
			var remainder = '';
			for(var i=0;i<options.decimals;i++){remainder += '0'}
		}
		
		var pfix = options.currency?options.currencySymbol:'';
		
		if(options.decimals<=0) return pfix + whole;
		
		var a = new Array();
		for(var i = 0; i < options.decimals; i++){
			if(remainder.charAt(i) != '') a[i] = remainder.charAt(i);
			else a[i] = '0';
		}
		
		return pfix + whole + options.decimalDelimiter + a.join("");
	}
});