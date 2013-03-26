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
	
	ui.logOut.addEventListener("click",function(event)
	{
		Ti.Facebook.logout();
		// clear cookies
		var client = Ti.Network.createHTTPClient();
		client.clearCookies('https://login.facebook.com');
		
		Ti.App.fireEvent("logout");
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
		var statView = TiTools.UI.Loader.load("Views/Statistic.js", ui.list);
		statView.category.text = achievements[i].display_name;
			
		for(var j = 0; j < achievements[i].projects.length; j++)
		{
			
			Ti.API.info(achievements[i].projects[j].api_name);
			
			tempAchivs = [];
			tempAchivs.push(achievements[i].projects[j].achievements[0].badge_name);
			
			for(var k = 0; k < achievements[i].projects[j].achievements.length; k++)
			{
				var flagPush = true;
				
				for(var index = 0; index < tempAchivs.length; index++)
				{
					Ti.API.info(tempAchivs[index] +"  "+ achievements[i].projects[j].achievements[k].badge_name);
					if(tempAchivs[index] == achievements[i].projects[j].achievements[k].badge_name)
					{
						flagPush = false;
					}
				}
				
				if(flagPush == true)
				{
					tempAchivs.push(achievements[i].projects[j].achievements[k].badge_name);
					Ti.API.info('+')
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