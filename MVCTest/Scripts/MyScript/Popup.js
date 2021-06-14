//傳入物件{url:"",width:0,height:0}

; (function () {
	'use strict';
	var Popup = function (set) {
		if (!(this instanceof Popup)) return new Poopup(set);
		var p = this;

		p.url = (typeof set === "object" && set.url !== undefined) ? set.url : '';
		p.w = (typeof set === "object" && set.width !== undefined) ? set.width : 800;
		p.h = (typeof set === "object" && set.height !== undefined) ? set.height : 600;
		p.isLoad = false;

		(function () {
			var body = document.querySelector("body");
			var div = document.createElement("div");
			var iframe = document.createElement("iframe");
			div.setAttribute("style", "position:absolute;top:0;left:0;display:flex;justify-content: center;align-items: center;width:100%;height:100%;background-color:rgba(204, 204, 204, 0.5);");
			div.onclick = function () {
				this.remove();
			};

			iframe.src = p.url;
			iframe.width = p.w;
			iframe.height = p.h;
			iframe.frameBorder = 0;
			iframe.scrolling = "auto";
			iframe.onload = function () {//iframe載入完後
				//叉叉按鈕
				window.frames[0].document.querySelector("#backBTN").onclick = function () {
					div.remove();
				};

				window.frames[0].document.querySelector("form").onsubmit = function () {
					div.style.display = "none";
					p.isLoad = true;
				};

				if (p.isLoad) {
					window.location.reload(true);
					p.isLoad = false;
				}
			};

			div.appendChild(iframe);
			body.appendChild(div);
		})();

	};

	window.Popup = Popup;
})();