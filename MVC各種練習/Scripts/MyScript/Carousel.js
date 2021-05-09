// obj 使用物件傳參數 { items: i, buttons: n,  url: u, loops: l,color:c }，最少要有items
// i 所有要移動的元素ClassName
// n 所有下一個按鈕ClassName
// l 是否單一左右循環(預設true)
// u 到controller取值的url
// c 第2種點擊輪播點擊後按鈕變色

; (function () {
	'use strict';
	var Carousel = function (obj) {
		if (obj.items === undefined) return;//沒有items時終止
		if (!(this instanceof Carousel)) return new Carousel(obj);
		var c = this;

		c.items = document.querySelectorAll(obj.items);//所有要移動的元素
		c.parent = c.items[0].parentNode;//父元素
		c.nextBTN = document.querySelectorAll(obj.buttons);//下一個按鈕
		c.currentWidth;//當前元素的寬度值
		c.unit;//單位
		c.isClick = true;//是否可以點擊防止連點
		c.clickNum = 0;//點擊次數(右-1、左+1)或點擊編號
		c.loops = (obj.loops !== undefined) ? obj.loops : true;//是否單一左右循環(預設true)
		c.color = (obj.loops !== undefined && obj.color !== undefined) ? obj.color : "yellow";

		//初始化
		; (function () {
			var width = window.getComputedStyle(c.items[0]).getPropertyValue("width");//當前元素的寬度(含單位)
			c.currentWidth = Number(width.replace(/px|%/, ""));//當前元素的寬度值
			c.unit = (width.indexOf("px") > 0) ? "px" : "%";//單位

			c.parent.style.position = "relative";//給父元素添加基本的樣式

			//給元素添加基本的樣式
			for (let i = 0; i < c.items.length; i++) {
				c.items[i].style.position = "absolute";
				c.items[i].style.transition = "left 0.5s ease-in-out";
				c.items[i].style.left = i * c.currentWidth + c.unit;
			}

			//2種按鈕事件
			if (c.nextBTN.length > 0) {
				if (c.loops) {
					c.nextBTN[0].onclick = function () {
						c._rightMove();
					}

					c.nextBTN[1].onclick = function () {
						c._leftMove();
					}
				} else {
					for (let i = 0; i < c.nextBTN.length; i++) {
						c.nextBTN[i].style.backgroundColor = (i === 0) ? c.color : "";

						c.nextBTN[i].onclick = function () {
							var index = Object.keys(c.nextBTN).map(function (k) { return c.nextBTN[k] }).indexOf(this);

							if (c.isClick && c.clickNum !== index) {
								for (let j = 0; j < c.nextBTN.length; j++) {
									c.nextBTN[j].style.backgroundColor = "";
								}

								c.isClick = false;
								if (obj.url !== undefined) {
									c._getData((c.clickNum > index), index);//到controller取值
								} else {
									c._getData2((c.clickNum > index));//沒有傳入u參數時取第一個元素生成
								}
								c.clickNum = index;
								this.style.backgroundColor = c.color;
							}
						}
					}
				}
			}

			//位移到正確位置(RWD使用)
			window.addEventListener("resize", function () {
				let width = window.getComputedStyle(c.items[0]).getPropertyValue("width");//當前元素的寬度
				c.currentWidth = Number(width.replace(/px|%/, ""));//當前元素的寬度值
				c.unit = (width.indexOf("px") > 0) ? "px" : "%";//單位

				for (let i = 0; i < c.items.length; i++) {
					let newLeft = c.currentWidth * i + c.unit;//隨著螢幕大小等比轉換位置
					c.items[i].style.left = newLeft;
				}
			});
		})();

		//右移動
		c._rightMove = function () {
			if (c.isClick) {
				c.isClick = false;

				if (obj.url !== undefined) {
					c._getData(false, c.items.length + c.clickNum);//到controller取值
					c.clickNum += 1;
				} else {
					c._getData2(false);//沒有傳入u參數時取第一個元素生成
				}
			}
		};

		//左移動
		c._leftMove = function () {
			if (c.isClick) {
				c.isClick = false;

				if (obj.url !== undefined) {
					c.clickNum -= 1;
					c._getData(true, c.clickNum);//到controller取值
				} else {
					c._getData2(true);//沒有傳入u參數時取第一個元素生成
				}
			}
		};

		//移動到下一個
		c._nextItem = function (dir) {
			setTimeout(function () {
				c.isClick = true;

				//移除出界的元素
				if (dir) {
					c.parent.removeChild(c.items[c.items.length - 1]);
				} else {
					c.parent.removeChild(c.items[0]);
				}

				c.items = document.querySelectorAll(obj.items);
			}, 500);

			for (let i = 0; i < c.items.length; i++) {
				let nowLeft = Number((c.items[i].style.left).replace(/px|%/, ""));//現在的left值

				//判斷方向
				if (dir) {
					let newLeft = c.currentWidth + nowLeft + c.unit;//新的left值
					c.items[i].style.left = newLeft;
				} else {
					let newLeft = c.currentWidth * -1 + nowLeft + c.unit;//新的left值
					c.items[i].style.left = newLeft;
				}
			}
		}

		//創建下一個元素
		c._createEle = function (dir, data) {
			let newLeft = ((dir) ? c.currentWidth * -1 : c.currentWidth * c.items.length) + c.unit;//生成位置left值

			let doc = new DOMParser().parseFromString(data, 'text/html');//轉換成html DOM節點
			let ele = doc.querySelector(obj.items);
			ele.style.transition = "left 0.5s ease-in-out";
			ele.style.position = "absolute";
			ele.style.left = newLeft;
			//console.log(doc, ele);

			//決定生成的位置
			if (dir) {
				c.parent.insertBefore(ele, c.parent.children[0]);
			} else {
				c.parent.appendChild(ele);
			}
			c.items = document.querySelectorAll(obj.items);

			ele.clientWidth;//讓瀏覽器重繪
			c._nextItem(dir);
		}

		//到controller取值
		c._getData = function (dir, index) {
			let xhr = new XMLHttpRequest();

			xhr.open("post", obj.url, true);
			xhr.setRequestHeader("Content-Type", "application/json");

			xhr.onload = function () {
				c._createEle(dir, xhr.response);//預先創建下一個元素
				//console.log(xhr.response);
			}

			xhr.send(JSON.stringify({ num: index }));
		}

		//沒有傳入url時取第一個元素生成
		c._getData2 = function (dir) {
			var clone = c.items[0].outerHTML//取得元素字串
			c._createEle(dir, clone);//預先創建下一個元素
		}

		return {
			RightMove: c._rightMove,
			LeftMove: c._leftMove
		};
	};

	window.Carousel = Carousel;
})();

//第1種點擊輪播
//搭配ajax(post)有url時
//new Carousel({ items: ".li",buttons:".btn", url: "/Carousel/P" });

//沒url時
//new Carousel({ items: ".li",buttons:".btn" });

//可自定義是哪個按鈕
//let a = new Carousel({ items: ".li", url: "/Carousel/P"  });
//let bb = document.querySelectorAll(".btn");
//bb[0].onclick = a.RightMove;
//bb[1].onclick = a.LeftMove;

//可自定義是哪個按鈕
let a = new Carousel({ items: ".li" });
let bb = document.querySelectorAll(".btn");
bb[0].onclick = a.RightMove;
bb[1].onclick = a.LeftMove;

//第2種點擊輪播
//new Carousel({ items: ".u", buttons: ".b", loops: false, color: "red" });
new Carousel({ items: ".u", buttons: ".b", loops: false });