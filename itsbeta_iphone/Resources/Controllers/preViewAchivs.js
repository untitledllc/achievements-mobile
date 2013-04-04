/**
 * @author Gom_Dzhabbar
 */

var TiTools = undefined;

//---------------------------------------------//
// Глобальные переменные для окна
//---------------------------------------------//

//---------------------------------------------//
// Обязательные функции
//---------------------------------------------//

// Инициализация контроллера окна
function onInitController(window, params)
{
	TiTools = require("TiTools/TiTools");
	
	// Загрузка контента окна
	ui = TiTools.UI.Loader.load("Views/preViewAchivs.js", window);
	ui.image.image = window.image;
	ui.nameAchivs.text = window.nameAchivs;
	ui.textAchivs.text = window.textAchivs;
	
	ui.act.show();
	
	// close button
	decorateButton.call(
		ui.close,
		function() {
			window.close();
		}
	);
	
	ui.shadowClose.addEventListener("click",function(event)
	{
		window.close();
	});
	
 }
//---------------------------------------------//
// Функции лентяйки
//---------------------------------------------//

// Обработчик при открытии окна
function onWindowOpen(window, event)
{
	var webview = Titanium.UI.createWebView({
		left: 5,
		height: Ti.UI.SIZE,
		width: 220,
		html:  TiTools.Global.get("htmlWrapBefore") + window.details + window.adv + TiTools.Global.get("htmlWrapAfter"),
		disableBounce: true
	});
	
	webview.addEventListener("load",function(){
		ui.shadowClose.show();
		ui.act.hide();
	});
	
	var bonus = window.bonus
	for(var i = 0; i < bonus.length; i++)
	{
		var row = TiTools.UI.Loader.load("Views/ViewBonus.js", ui.bonus);
		row.desc.text = bonus[i].bonus_desc;
		
		switch(bonus[i].bonus_type)
		{
			case "discount": 
				row.type.text = "Скидка";
				row.me.backgroundImage = "images/bg/Bonus.Discount.Popup.png";
				break;
			case "present": 
				row.type.text = "Подарок";
				row.me.backgroundImage = "images/bg/Bonus.Present.Popup.png";
				break;
			case "bonus": 
				row.type.text = "Бонус";
				row.me.backgroundImage = "images/bg/Bonus.Bonus.Popup.png";
				break;
			default: 
				row.type.text = "-";
				break;
		}
	}
	
	ui.bonus.add(TiTools.UI.Controls.createView({
		height: 15
	}));
	
	ui.textAchivs.add(webview);
}

// Обработчик при закрытии окна
function onWindowClose(window, event)
{
	
}
//---------------------------------------------//

module.exports = {
	onInitController : onInitController, // Обязательный параметр
	onWindowOpen : onWindowOpen,
	onWindowClose : onWindowClose
};