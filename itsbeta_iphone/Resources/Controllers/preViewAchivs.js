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
	
	ui.close.addEventListener("click",function(event)
	{
		window.close();
	});
	
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