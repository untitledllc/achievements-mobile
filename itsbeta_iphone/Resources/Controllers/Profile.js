/**
 * @author Gom_Dzhabbar
 */

var TiTools = undefined;
var ui = undefined;
var achievements = undefined;
var projects = undefined;
var categories = undefined;
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
	
	achievements = window.achievements;
	
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
	var all = window.counter;
	var bonus = 0;
	var sub = 0;
	var tempAchivs = [];
	
	for(var i = 0; i < achievements.length; i++)
	{
		for(var j = 0; j < achievements[i].projects.length; j++)
		{
			
			Ti.API.info(achievements[i].projects[j].api_name);
			
			var statView = TiTools.UI.Loader.load("Views/Statistic.js", ui.list);
			statView.category.text = achievements[i].display_name;
			
			
			tempAchivs = [];
			tempAchivs.push(achievements[i].projects[j].achievements[0].badge_name);
			
			for(var k = 0; k < achievements[i].projects[j].achievements.length; k++)
			{
				
				for(var index = 0; index < tempAchivs.length; index++)
				{
					if(tempAchivs[index] != achievements[i].projects[j].achievements[k].badge_name)
					{
						tempAchivs.push(achievements[i].projects[j].achievements[k].badge_name);
						break;
					}
				}
				
				for(var n = 0; n < achievements[i].projects[j].achievements[k].bonuses.length; n++)
				{
					if(achievements[i].projects[j].achievements[k].bonuses[n].bonus_type == "bonus")
					{
						bonus++;
					}
					else
					{
						sub++;
					}
				}
			}
			
			statView.item.add(createLabelStat(achievements[i].projects[j].api_name +" ("+ tempAchivs.length + "/" + achievements[i].projects[j].total_badges + ")"))
		}
	}
	ui.all.text = ui.all.text + all;
	ui.bonus.text = ui.bonus.text + bonus;
	ui.sub.text = ui.sub.text + sub ;
	
	
}
//------Создание лабелки с отображением имени проекта и статистикой полученый ачивок---//
function createLabelStat(text)
{
	var label = Ti.UI.createLabel({
		height: Ti.UI.SIZE,
		width: Ti.UI.SIZE,
		text: text
	});
	
	return label;
}
//-------------------------------------------------------------------------------------//
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