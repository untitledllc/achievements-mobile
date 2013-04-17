/**
 * @author Gom_Dzhabbar
 */

var TiTools = undefined;
var ui = undefined;
var itsbeta = undefined;
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
	itsbeta = require("Utils/Itsbeta");
	
	// Загрузка контента окна
	ui = TiTools.UI.Loader.load("Views/addCode.js", window);
	
	// cancel
	decorateNavbarButton.call(
		ui.cancel, 
		function() {
			window.close();
		}
	);
	
	// done
	decorateNavbarButton.call(
		ui.done, 
		function() {
			actIndicator(true);
			itsbeta.postActivCode(ui.code.value);
		}
	);
}
//---------------------------------------------//
// Функции лентяйки
//---------------------------------------------//

function actIndicator(param)
{
	Ti.API.info('actIndicator')
	
	if(param == true)
	{
		ui.actView.show();
		ui.act.show();
	}
	else
	{
		ui.actView.hide();
		ui.act.hide();
	}
}
// Обработчик при открытии окна
function onWindowOpen(window, event)
{
	ui.code.hintText = L("label_add_code_hint");
	ui.code.focus();
	
	Ti.App.addEventListener("actHide",function(event)
	{
		actIndicator(false);
	});
	
	Ti.App.addEventListener("CloseAdd",function(event)
		{
			window.close();
		}
	);
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