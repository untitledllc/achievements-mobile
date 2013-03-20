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
	var all = window.achivs.length;
	var bonus = 0;
	var sub = 0;
	
	for(var i = 0; i < window.achivs.length; i++)
	{
		if(window.achivs[i].achievements.bonuses != undefined)
		{
			for(var j = 0; j < window.achivs[i].achievements.bonuses.length; j++)
			{
				Ti.API.info(window.achivs[i].achievements.bonuses[j].bonus_type);
				
				if(window.achivs[i].achievements.bonuses[j].bonus_type == "bonus")
				{
					bonus++;
				}
				else
				{
					sub++;
				}
			}
		}
	}
	ui.all.text = ui.all.text + all;
	ui.bonus.text = ui.bonus.text + bonus;
	ui.sub.text = ui.sub.text + sub ;
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