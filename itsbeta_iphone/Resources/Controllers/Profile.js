/**
 * @author Gom_Dzhabbar
 */

var TiTools = undefined;
var ui = undefined;
//---------------------------------------------//
// Глобальные переменные для окна
//---------------------------------------------//


//---------------------------------------------//


//---------------------------------------------//
// Обязательные функции
//---------------------------------------------//

// Инициализация контроллера окна
function onInitController(window, params)
{
	TiTools = require("TiTools/TiTools");
	
	// Загрузка контента окна
	ui = TiTools.UI.Loader.load("Views/Profile.js", window);
	ui.profileName.text = window.info.name;
	ui.profileInfo.text = window.info.birthday + ", " + window.info.city + ", " + window.info.country;
	
	ui.back.addEventListener("click",function(event)
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