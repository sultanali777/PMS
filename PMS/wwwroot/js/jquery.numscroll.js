/*!
 * jquery.numscroll.js -- æ•°å­—æ»šåŠ¨ç´¯åŠ åŠ¨ç”»æ’ä»¶  (Digital rolling cumulative animation)
 * version 3.0.0
 * 2019-08-08
 * author: KevinTseng < 921435247@qq.com@qq.com >
 * APIæ–‡æ¡£: https://github.com/chaorenzeng/jquery.numscroll.js.git
 * äº¤æµQç¾¤: 814798690
 */

(function($) {
	
	$.fn.numScroll = function(options) {
		var settings = $.extend({
			'number': '0', //æ•°å€¼
			'step': 1, //æ­¥é•¿
			'time': 2000, //é™åˆ¶ç”¨æ—¶(ä¸º0æ—¶ä¸é™åˆ¶) Limited use time (0 time is not limited)
			'delay': 0, //å»¶è¿Ÿå¼€å§‹(ms) delay(ms)
			'symbol': false ,//æ˜¯å¦æ˜¾ç¤ºåˆ†éš”ç¬¦ display separators
			'fromZero': true, //æ˜¯å¦ä»Ž0å¼€å§‹è®¡æ•°ï¼ˆä¸ºå¦æ—¶ä»ŽåŽŸæœ‰å€¼å¼€å§‹è®¡æ•°ï¼‰ Whether to start counting from zero(If not, count from the original value)
		}, options);
		settings.number = settings.number.toString(); //æ•°å€¼è½¬å­—ç¬¦ä¸²
		
		return this.each(function(){
			//åˆå§‹åŒ–é…ç½®
			var $this = $(this),
				oldNum = $this.text() || '0';
			//åˆ†éš”ç¬¦æ˜¾ç¤ºåˆ¤æ–­
			if (settings.number.indexOf(',') > 0) {
				//æ•°å€¼å«æœ‰åˆ†éš”ç¬¦ï¼Œåˆ™é»˜è®¤ä¸ºéœ€è¦æ˜¾ç¤ºåˆ†éš”ç¬¦
				settings.symbol = true;
			}
			if (options && options.symbol===false) {
				//æ‰‹åŠ¨è®¾ç½®ä¸æ˜¾ç¤ºåˆ†éš”ç¬¦æ—¶ï¼Œä¸æ˜¾ç¤ºåˆ†éš”ç¬¦
				settings.symbol = false;
			}
			//æ˜¾ç¤ºåˆå§‹å€¼
			var targetNum = settings.number.replace(/,/g, '') || 0,
				oldRealNum = oldNum.replace(/,/g, '');
			if(settings.symbol){
				$this.text(oldNum);
			}else{
				$this.text(oldRealNum);
			}
			//åˆ¤æ–­ä»Ž0å¼€å§‹è®¡æ•°æˆ–ä»ŽåŽŸæœ‰å€¼å¼€å§‹è®¡æ•°
			if (settings.fromZero) {
				oldRealNum = 0;
			}
			//éžæ•°å€¼å¤„ç†
			if(isNaN(oldRealNum)){
				oldRealNum = 0;
			}
			if(isNaN(targetNum)){
				return;
			}
			//åˆå§‹å€¼ç›®æ ‡å€¼å‡†å¤‡
			targetNum = parseFloat(targetNum);
			oldRealNum= parseFloat(oldRealNum);
			var tempNum = oldRealNum,
				numIsInt = isInt(targetNum),
				numIsFloat = isFloat(targetNum),
				step = !settings.time?1:Math.abs(targetNum-oldRealNum) * 10 / settings.time,
				numScroll;
			//æ›´æ–°æ–¹æ³•
			function numInitUpdate() {
				var showNum = '';
				//æ•´åž‹æˆ–æµ®ç‚¹åž‹
				if (numIsInt) {
					showNum = Math.floor(tempNum);
				} else if (numIsFloat != -1) {
					showNum = tempNum.toFixed(numIsFloat)
				} else {
					showTarget(targetNum);
					clearInterval(numScroll);
					return;
				}
				//åƒä½ç¬¦æ˜¾ç¤º
				if (settings.symbol) {
					showNum = formatSymbol(showNum);
				}
				$this.text(showNum);
			}
			
			//æœ€ç»ˆæ˜¾ç¤º
			function showTarget(num) {
				var targetNum = num.toString().replace(/,/g, '');
				if (settings.symbol) {
					targetNum = formatSymbol(targetNum);
				}
				$this.text(targetNum);
			}
			
			//å®šæ—¶å¼€å§‹
			setTimeout(function() {
				numScroll = setInterval(function() {
					numInitUpdate();
					if(oldRealNum < targetNum){
						//å¢ž
						tempNum += step;
						if (tempNum > targetNum) {
							showTarget(targetNum);
							clearInterval(numScroll);
						}
					}else{
						//å‡
						tempNum -= step;
						if (tempNum < targetNum) {
							showTarget(targetNum);
							clearInterval(numScroll);
						}
					}
				}, 1);
			}, settings.delay);
			
		})
	};

	/*	
	 * åˆ¤æ–­æ•°å€¼æ˜¯å¦ä¸ºæ•´æ•°
	 * @param num {Number} æ•°å€¼
	 * @return {Boolean} çœŸå‡
	 */
	function isInt(num) {
		var res = false;
		try {
			if (String(num).indexOf(".") == -1 && String(num).indexOf(",") == -1) {
				res = parseInt(num) % 1 === 0 ? true : false;
			}
		} catch (e) {
			res = false;
		}
		return res;
	}

	/*	
	 * åˆ¤æ–­æ•°å€¼æ˜¯å¦ä¸ºå°æ•°
	 * @param num {Number} æ•°å€¼
	 * @return {Number} å°æ•°ä½æ•°(-1æ—¶ä¸æ˜¯å°æ•°)
	 */
	function isFloat(num) {
		var res = -1;
		try {
			if (String(num).indexOf(".") != -1) {
				var index = String(num).indexOf(".") + 1; //èŽ·å–å°æ•°ç‚¹çš„ä½ç½®
				var count = String(num).length - index; //èŽ·å–å°æ•°ç‚¹åŽçš„ä¸ªæ•°
				if (index > 0) {
					res = count;
				}
			}
		} catch (e) {
			res = -1;
		}
		return res;
	}

	/*	
	 * æ˜¾ç¤ºæ•°å€¼åƒåˆ†ä½åˆ†éš”ç¬¦
	 * @param num {Number} æ•°å€¼
	 * @return {String} å«åˆ†éš”ç¬¦æ•°å€¼
	 */
	function formatSymbol(num) {
		var res = '';
		var str = num + '',
			strLeft = '',
			strRight = '';
		var floatNum = isFloat(num);
		if (floatNum != -1) {
			//æœ‰å°æ•°æ—¶è¿›è¡Œåˆ‡å‰²
			var splitStr = str.split('.');
			strLeft = splitStr[0];
			strRight = splitStr[1];
		} else {
			strLeft = str;
		}
		//æ•´æ•°éƒ¨åˆ†æ¯éš”3ä½æ·»åŠ åˆ†éš”ç¬¦
		res = strLeft.split("").reverse().reduce(function(prev, next, index) {
			return ((index % 3) ? next : (next + ',')) + prev;
		})
		//æ‹¼æŽ¥å°æ•°éƒ¨åˆ†
		if (strRight != '') {
			res = res + '.' + strRight;
		}
		return res;
	}
})(jQuery);