let items = document.getElementsByClassName("li");//所有要移動的元素
let parent = document.querySelector(".ul");//父元素
let nextBTN = document.querySelector("#a");//左箭頭
let nextBTN2 = document.querySelector("#b");//右箭頭
let currentWidth;//當前元素的寬度值
let unit;//單位
let clickNum = 0;//點擊次數(右-1、左+1)
const itemCount = 7;//輪播元素總數(5個顯示，左右各一個預備)

//初始化
(function () {
	let width = window.getComputedStyle(items[0]).getPropertyValue("width");//當前元素的寬度(含單位)
	currentWidth = Number(width.replace(/px|%/, ""));//當前元素的寬度值
	unit = (width.indexOf("px") > 0) ? "px" : "%";//單位
	//console.log(currentWidth, unit)

	for (let i = 0; i < items.length; i++) {
		items[i].style.transition = "left 0.5s ease-in-out";
		items[i].style.left = (i - 1) * currentWidth + unit;
	}

	nextBTN.onclick = function () {
		getData(false,itemCount + clickNum);
		clickNum+=1;
		nextItem(false);
	}
	nextBTN2.onclick = function () {
		clickNum -= 1;
		getData(true,clickNum);
		nextItem(true);
	}
})();

//下一個
function nextItem(dir) {
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
//window.onresize = function () {
//    let width = window.getComputedStyle(items[0]).getPropertyValue("width");//當前元素的寬度
//    currentWidth = Number(width.replace(/px|%/, ""));//當前元素的寬度值
//    unit = (width.indexOf("px") > 0) ? "px" : "%";//單位

//    for (let i = 0; i < items.length; i++) {
//        let newLeft = currentWidth * (i - 1) + unit;//隨著螢幕大小等比轉換位置
//        items[i].style.left = newLeft;
//    };
//}

//預先創建下一個元素
function createEle(dir, data) {
	let newLeft = ((dir) ? currentWidth * -1 : currentWidth * 5) + unit;//生成位置left值
	let content = '<h3>' + data.locationName + '</h3><p>' + data.weatherElement[0].time[0].parameter.parameterName + '</p>';

	let ele = document.createElement("li");
	ele.className = "li";
	ele.style.transition = "left 0.5s ease-in-out";
	ele.style.left = newLeft;
	ele.innerHTML = content;

	//決定生成的位置、移除出界的元素
	if (dir) {
		parent.insertBefore(ele, parent.children[0]);
		parent.removeChild(items[items.length - 1]);
	} else {
		parent.appendChild(ele);
		parent.removeChild(items[0]);
	}
}

//到controller取值
function getData(dir,index) {
	let xhr = new XMLHttpRequest();

	xhr.open("post", "/Carousel/P", true);
	xhr.setRequestHeader("Content-Type", "application/json");

	xhr.onload = function () {
		let data = JSON.parse(xhr.response);
		createEle(dir, data);//預先創建下一個元素
		//console.log(data);
	}

	xhr.send(JSON.stringify({ clickNum:index}));
}