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
	
	decorateButton.call(
		ui.infacebook, 
		function(event)
		{
			start(event);
		}
	);
	
	function start(event) // onSingleTap handler
		{
			if(singlTap == false)
			{
				singlTap = true;
				
				if(Titanium.Network.online == true)
				{
					
					Ti.API.info(Titanium.Facebook.loggedIn);
					
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
								
								Ti.API.info(info)
								
								TiTools.Global.set("info", info);
							}
							catch(e)
							{
								alert("Ошибка загрузки данных из Facebook.");
								
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
								
								Ti.API.info('!!!');
								
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
	
	for(var i = 0; i < achievements.length; i++)
	{
		var achievement = achievements[i];
		
		categories.push({
			api_name: achievement.api_name,
			display_name: achievement.display_name
		});
		
		for(var j = 0; j < achievement.projects.length; j++)
		{
			var project = achievement.projects[j];
			counter += project.achievements.length;
			
			projects.push({
				api_name: project.api_name,
				display_name: project.display_name,
				total_badge: project.total_badge
			});
		}
	}
	
	//------------------------------------------------------//
	
	var win = TiTools.UI.Controls.createWindow(
		{
			main : "Controllers/GeneralWindow.js",
			navBarHidden : true,
			info: info,
			achievements : achievements,
			categories : categories,
			projects : projects,
			counter : counter,
			backgroundColor: "white"
		}
	);
	win.initialize();
	win.open();
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
	onInitController : onInitController, // Обязательный параметр
	onWindowOpen : onWindowOpen,
	onWindowClose : onWindowClose
};