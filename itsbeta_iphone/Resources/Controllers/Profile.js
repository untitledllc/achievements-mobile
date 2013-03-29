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

Ti.include("Utils/Helper.js");

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
	
	// back
	decorateNavbarButton.call(
		ui.back, 
		function() {
			window.close();
		}
	);
	
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
		var achievement = achievements[i];
		var projects = achievement.projects;
		
		var statView = TiTools.UI.Loader.load("Views/Statistic.js", ui.list);
		statView.category.text = (i+1) + ". " + achievements[i].display_name + ":";
			
		for(var j = 0; j < projects.length; j++)
		{
			var project = projects[j];
			
			tempAchivs = [];
			tempAchivs.push(project.achievements[0].badge_name);
			
			for(var k = 0; k < project.achievements.length; k++)
			{
				var projectAchievement = project.achievements[k];
				var flagPush = true;
				
				for(var index = 0; index < tempAchivs.length; index++)
				{
					if(tempAchivs[index] == projectAchievement.badge_name)
					{
						flagPush = false;
					}
				}
				
				if(flagPush == true)
				{
					tempAchivs.push(projectAchievement.badge_name);
				}
				
				for(var n = 0; n < projectAchievement.bonuses.length; n++)
				{
					if(projectAchievement.bonuses[n].bonus_type == "bonus")
					{
						bonus++;
					}
					else
					{
						sub++;
					}
				}
			}
			
			statView.item.add(createLabelStat(project.api_name, Math.ceil(100*tempAchivs.length/project.total_badges))) // +" ("+ tempAchivs.length + "/" + project.total_badges + ")"));
		}
	}
	ui.all.text = ui.all.text + all;
	ui.bonus.text = ui.bonus.text + bonus;
	ui.sub.text = ui.sub.text + sub ;
}
//------Создание лабелки с отображением имени проекта и статистикой полученый ачивок---//
function createLabelStat(title, progress)
{
	var row = TiTools.UI.Loader.load("Views/profileBadgeRow.js");
	row.title.text = title;
	row.progress.value = progress;
	
	return row.me;
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