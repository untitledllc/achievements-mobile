/**
 * @author Gom_Dzhabbar
 */

var TiTools = undefined,
	ui = undefined,
	achievements = undefined,
	projects = undefined,
	categories = undefined;
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
	//ui.profileInfo.text = window.info.birthday + ", " + window.info.city + ", " + window.info.country;
	
	var info = window.info;
	var pName = "";
	if(info.birthday) {
		pName += info.birthday + ", ";
	}
	if(info.city) {
		pName += info.city + ", ";
	}
	if(info.country) {
		pName += info.country;
	}
	
	ui.profileInfo.text = pName;
	
	// back
	decorateNavbarButton.call(
		ui.back, 
		function() {
			window.close();
		}
	);
	
	// log out
	decorateButton.call(
		ui.logOut, 
		function() {
			
			var a = Titanium.UI.createAlertDialog({
				title: " ",
				message: "Выйти из вашего профиля?",
				buttonNames: ["Да","Нет"],
				cancel: 1,
			});
			
			a.show();
			
			a.addEventListener("click",function(event)
			{
				if(event.index == 0)
				{
					Ti.Facebook.logout();
					// clear cookies
					var client = Ti.Network.createHTTPClient();
					client.clearCookies('https://login.facebook.com');
					
					Ti.App.fireEvent("logout");
					window.close();
				}
			});
		}
	);
}
//---------------------------------------------//
// Функции лентяйки
//---------------------------------------------//

// Обработчик при открытии окна
function onWindowOpen(window, event)
{
	var all = window.counter,
		bonus = 0,
		sub = 0,
		tempAchivs = [];
	
	for(var i = 0; i < achievements.length; i++)
	{
		var achievement = achievements[i];
		var projects = achievement.projects;
		
		// var statView = TiTools.UI.Loader.load("Views/Statistic.js", ui.list);
		// statView.category.text = (i+1) + ". " + achievements[i].display_name + ":";
			
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
			
			// statView.item.add(createProfileBadgeRow(project.display_name + " - " + project.total_badges)); //, Math.ceil(100*tempAchivs.length/project.total_badges)));
		}
	}
	ui.all.text = ui.all.text + all;
	ui.bonus.text = ui.bonus.text + bonus;
	ui.sub.text = ui.sub.text + sub ;
}

// create profile badge row
function createProfileBadgeRow(title)
{
	var row = TiTools.UI.Loader.load("Views/profileBadgeRow.js");
	row.title.text = title;
	// row.progress.value = progress;
	
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