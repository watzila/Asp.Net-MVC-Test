// i 所有要移動的元素ClassName
// p 父元素ClassName
// n 所有下一個按鈕ClassName
// l 是否單一左右循環
// u 到controller取值的url

var Carousel = function (i, p, n, l, u) {
	let items = document.getElementsByClassName(i);//所有要移動的元素
	let parent = document.querySelector("." + p);//父元素
	let nextBTN = document.querySelectorAll("." + n);//下一個按鈕
	let currentWidth;//當前元素的寬度值
	let unit;//單位
	let isClick = true;//是否可以點擊防止連點
	let clickNum = 0;//點擊次數(右-1、左+1)或點擊編號
	let loops = l;//是否單一左右循環

	//初始化
	; (function () {
		let width = window.getComputedStyle(items[0]).getPropertyValue("width");//當前元素的寬度(含單位)
		currentWidth = Number(width.replace(/px|%/, ""));//當前元素的寬度值
		unit = (width.indexOf("px") > 0) ? "px" : "%";//單位

		parent.style.position = "relative";

		for (let i = 0; i < items.length; i++) {
			items[i].style.position = "absolute";
			items[i].style.transition = "left 0.5s ease-in-out";
			items[i].style.left = i * currentWidth + unit;
		}

		if (nextBTN.length > 0) {
			if (loops) {
				nextBTN[0].onclick = function () {
					if (isClick) {
						isClick = false;

						if (u != undefined) {
							getData(false, items.length + clickNum);//到controller取值
							clickNum += 1;
						} else {
							getData2(false);//沒有傳入u參數時取第一個元素生成
						}
					}
				}

				nextBTN[1].onclick = function () {
					if (isClick) {
						isClick = false;

						if (u != undefined) {
							clickNum -= 1;
							getData(true, clickNum);//到controller取值
						} else {
							getData2(true);//沒有傳入u參數時取第一個元素生成
						}
					}
				}
			} else {
				for (let i = 0; i < nextBTN.length; i++) {
					nextBTN[i].onclick = function () {
						if (isClick && clickNum != i) {
							for (let j = 0; j < nextBTN.length; j++) {
								nextBTN[j].style = " ";
							}

							isClick = false;
							getData((clickNum > this.innerText), this.innerText);//到controller取值
							clickNum = this.innerText;
							this.style.backgroundColor = "yellow";
						}
					}
				}
			}
		}
	})();

	//移動到下一個
	function nextItem(dir) {
		setTimeout(function () {
			isClick = true;

			//移除出界的元素
			if (dir) {
				parent.removeChild(items[items.length - 1]);
			} else {
				parent.removeChild(items[0]);
			}
		}, 500);

		for (let i = 0; i < items.length; i++) {
			let nowLeft = Number((items[i].style.left).replace(/px|%/, ""));//現在的left值

			//判斷方向
			if (dir) {
				let newLeft = currentWidth + nowLeft + unit;//新的left值
				items[i].style.left = newLeft;
			} else {
				let newLeft = currentWidth * -1 + nowLeft + unit;//新的left值
				items[i].style.left = newLeft;
			}
		}
	}

	//位移到正確位置(RWD使用)
	window.addEventListener("resize", function () {
		let width = window.getComputedStyle(items[0]).getPropertyValue("width");//當前元素的寬度
		currentWidth = Number(width.replace(/px|%/, ""));//當前元素的寬度值
		unit = (width.indexOf("px") > 0) ? "px" : "%";//單位

		for (let i = 0; i < items.length; i++) {
			let newLeft = currentWidth * i + unit;//隨著螢幕大小等比轉換位置
			items[i].style.left = newLeft;
		}
	});

	//創建下一個元素
	function createEle(dir, data) {
		let newLeft = ((dir) ? currentWidth * -1 : currentWidth * items.length) + unit;//生成位置left值

		let doc = new DOMParser().parseFromString(data, 'text/html');//轉換成html DOM節點
		let ele = doc.querySelector("." + i);
		ele.style.transition = "left 0.5s ease-in-out";
		ele.style.position = "absolute";
		ele.style.left = newLeft;
		//console.log(doc, ele);

		//決定生成的位置
		if (dir) {
			parent.insertBefore(ele, parent.children[0]);
		} else {
			parent.appendChild(ele);
		}

		ele.clientWidth;//讓瀏覽器重繪
		nextItem(dir);
	}

	//到controller取值
	function getData(dir, index) {
		let xhr = new XMLHttpRequest();

		xhr.open("post", u, true);
		xhr.setRequestHeader("Content-Type", "application/json");

		xhr.onload = function () {
			createEle(dir, xhr.response);//預先創建下一個元素
			//console.log(xhr.response);
		}

		xhr.send(JSON.stringify({ num: index }));
	}

	//沒有傳入u參數時取第一個元素生成
	function getData2(dir) {
		var clone = items[0].outerHTML//取得元素字串
		createEle(dir, clone);//預先創建下一個元素
	}
};

new Carousel("li", "ul", "btn", true, "/Carousel/P");
//new Carousel("li", "ul", "btn", true);