/**
 * @author Gom_Dzhabbar
*/

var TiTools = undefined;
var info = undefined;
var access_token = "8e6b3a7b47c3346cb7e4db42c88519bc";
var categories = [];
var projects = [];
var indexCategories = 0;
var id_user = undefined;
var accessTokenFb = undefined;

var itsbeta;

var achievements = [];
var newCategories = [];
var newProjects = [];
var oldTime = undefined;
var counter = 0;

var ui = undefined;

var singlTap = false;

//---------------------------------------------//

Ti.include("Utils/Helper.js");

//---------------------------------------------//
// Глобальные переменные для окна
//---------------------------------------------//
// Обязательные функции
//---------------------------------------------//

// Инициализация контроллера окна
function onInitController(window, params)
{
	TiTools = require("TiTools/TiTools");
	itsbeta = require("Utils/Itsbeta");
	
    // Загрузка контента окна
	ui = TiTools.UI.Loader.load("Views/Login.js", window);
	
	Titanium.Facebook.appid = "264918200296425";
	Titanium.Facebook.permissions = ['publish_stream', 'read_stream'];
	
	if(Titanium.Facebook.loggedIn)
	{
		start();
	}
	
	var tempDecorateClickFb = false;
	decorateButton.call(
		ui.infacebook, 
		function()
		{
			if(tempDecorateClickFb == false)
			{
				tempDecorateClickFb = true;
				start();
				tempDecorateClickFb = false;
			}
		}
	);
	
	function start(event) // onSingleTap handler
		{
			if(singlTap == false)
			{
				singlTap = true;
				
				if(Titanium.Network.online == true)
				{
					function fQuery() 
					{
						var fbuid = Titanium.Facebook.uid; 
						accessToken = Titanium.Facebook.accessToken;
						var myQuery = "SELECT name, birthday_date,current_location FROM user WHERE uid = "+fbuid;
					
						Titanium.Facebook.request('fql.query', {query: myQuery},  function(x)
						{
							try
							{
								var results = JSON.parse(x.result);
								var profile = results[0];
								
								info = {
									fbuid: fbuid,
									accessToken: accessToken
								};
									
								info.name     = (profile.name) ? profile.name : null;	
								info.birthday = (profile.birthday_date) ? profile.birthday_date : null;	
								if(profile.current_location != undefined) {
									info.city    = (profile.current_location.city) ? profile.current_location.city : null;	
									info.country = (profile.current_location.country) ? profile.current_location.country : null;
								}
								
								TiTools.Global.set("info", info);
							}
							catch(e)
							{
								info = {
									fbuid: fbuid,
									accessToken: accessToken,
									name: "",
									birthday: "",
									city: "",
									country: ""
								};
								TiTools.Global.set("info", info);
							}
							
							// get achievements by user id
							itsbeta.firstStart(info);
							
							var first = function()
							{
								Ti.App.removeEventListener("complite",first);
								
								itsbeta.getAchievementsByUid(fbuid, saveAchivs);
							}
							Ti.App.addEventListener("complite",first);
						});
					};
					
					if(!Titanium.Facebook.loggedIn)
					{
						actIndicator(true);
						singlTap = false;
						Ti.Facebook.authorize();
						
						var log = function(e) 
						{
							Ti.Facebook.removeEventListener('login',log);
							if (e.success) {
								fQuery();
							} else if (e.error) {
								alert(e.error);
								actIndicator(false);
							} else if (e.cancelled) {
								actIndicator(false);
							}
						}
						
						Ti.Facebook.addEventListener('login',log);
					}
					else
					{
						singlTap = false;
						actIndicator(true);
						fQuery();
					}
				}
				else
				{
					Ti.UI.createAlertDialog({
						title: "Информация!",
						message: "Отсутствует интернет!"
					}).show();
					actIndicator(false);
				}
			}
		}
	
	// --- слушатель индикатора, что бы закрывать из других окн --- //
	Ti.App.addEventListener("hideActive",function(event)
		{
			actIndicator(false);
		}
	);
}
//---------------------------------------------//

function saveAchivs(data)
{
	categories = [];
	projects = [];
	achievements = [];
	counter = 0;
	
	try
	{
		achievements = JSON.parse(data.responseText);
	}
	catch(e)
	{
		actIndicator(false);
		alert("Ошибка загрузки достижений.");
		return;
	}
	
	//---- собираем список категорий и список проектов -----//
	
	// for(var i = 0; i < achievements.length; i++)
	// {
		// var achievement = achievements[i];
// 		
		// categories.push({
			// api_name: achievement.api_name,
			// display_name: achievement.display_name
		// });
// 		
		// for(var j = 0; j < achievement.projects.length; j++)
		// {
			// var project = achievement.projects[j];
			// counter += project.achievements.length;
// 			
			// projects.push({
				// api_name: project.api_name,
				// display_name: project.display_name,
				// total_badge: project.total_badge
			// });
		// }
	// }
	
	///------------------ новая схема хранения ачивок ------///
		
	var newAchivsSсhema = [];
	
	for(var i = 0, I = achievements.length; i < I ; i++)
	{
		var achivs = achievements[i];
		
		categories.push({
			api_name: achivs.api_name,
			display_name: achivs.display_name
		});
		
		for(var j = 0, J = achivs.projects.length; j < J; j++)
		{
			var achivsProject = achivs.projects[j];
			
			counter += achivsProject.achievements.length;
			
			projects.push({
				api_name: achivsProject.api_name,
				display_name: achivsProject.display_name,
				total_badge: achivsProject.total_badge
			});
			
			for(var k = 0, K = achivsProject.achievements.length; k < K; k++)
			{
				var achivsProjectAchiv = achivsProject.achievements[k];
				
				newAchivsSсhema.push({
					categoryApiName: achivs.api_name,
					categoryDisplayName: achivs.display_name,
					projectsApiName: achivsProject.api_name,
					projectsDisplayName: achivsProject.display_name,
					color: achivsProject.color,
					total_badges: achivsProject.total_badges,
					achievApiName: achivsProjectAchiv.api_name,
					create_time: achivsProjectAchiv.create_time,
					achievDisplayName: achivsProjectAchiv.display_name,
					achievBadgeName: achivsProjectAchiv.badge_name,
					achievDesc: achivsProjectAchiv.desc,
					achievDetails: achivsProjectAchiv.details,
					achievAdv: achivsProjectAchiv.adv,
					achievPic: achivsProjectAchiv.pic,
					fb_id : achivsProjectAchiv.fb_id,
					achievBonuses: achivsProjectAchiv.bonuses,
				});
			}
		}
		if(i+1 == I)
		{
			
			newAchivsSсhema.sort(
				function(a, b)
				{
					var date1 = Date();
					var date2 = Date();
					
					date1 = a.create_time;
					date2 = b.create_time;
					
					if(date1 != date2)
					{
						if(date1 < date2)
						{
							return 1;
						}
						else
						{
							return -1;
						}
					}
					return 0;
				}
			);
			
			//Ti.API.info(newAchivsSсhema);
			
			//-----------------------------------------------------////
			
			//------------------------------------------------------//
			
			Ti.API.info(newAchivsSсhema.length); 
			oldTime = newAchivsSсhema[0].create_time;
			
			var win = TiTools.UI.Controls.createWindow(
				{
					main: "Controllers/GeneralWindow.js",
					navBarHidden: true,
					info: info,
					achievements: newAchivsSсhema,
					categories: categories,
					projects: projects,
					counter: counter,
					backgroundColor: "white",
					oldTime: oldTime
				}
			);
			win.initialize();
			win.open();
		}
	}
	
	
	
	actIndicator(false);
}

//---------------------------------------------//

function actIndicator(param)
{
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
	onInitController: onInitController, // Обязательный параметр
	onWindowOpen: onWindowOpen,
	onWindowClose: onWindowClose
};