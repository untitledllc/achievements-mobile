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
	
	decorateButton.call(
		ui.infacebook, 
		function(event) // onSingleTap handler
		{
			categories = [];
			projects = [];
			achievements = [];
			counter = 0;
			
			Titanium.Facebook.appid = "264918200296425";
			Titanium.Facebook.permissions = ['publish_stream', 'read_stream'];
			
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
						
						Ti.API.info(results);
						
						info = {
							fbuid : fbuid,
							accessToken : accessToken
						};
						if(results[0].name != undefined)
						{
							info.name = results[0].name;
						}
						if(results[0].birthday_date != undefined)
						{
							info.birthday = results[0].birthday_date;
						}
						if(results[0].current_location.city != undefined)
						{
							info.city = results[0].current_location.city;
						}
						if(results[0].current_location.country != undefined)
						{
							info.country = results[0].current_location.country;
						}
						
						Ti.API.info(info);
							
						TiTools.Global.set("info", info);
					}
					catch(e)
					{
						alert("Ошибка загрузки данных из Facebook.");
						
						info = {
							fbuid : fbuid,
							accessToken : accessToken,
							name: "",
							birthday: "",
							city: "",
							country: ""
						};
						TiTools.Global.set("info", info);
					}
					
					// get achievements by user id
					itsbeta.firstStart(info);
					Ti.App.addEventListener("complite",function(event)
					{
						itsbeta.getAchievementsByUid(fbuid, saveAchivs);
					});
				});
			};
			
			if(!Titanium.Facebook.loggedIn)
			{
				Ti.Facebook.authorize();
				Ti.Facebook.addEventListener('login',function(e) 
				{
					if (e.success) {
						actIndicator(true);
						fQuery();
					} else if (e.error) {
						alert(e.error);
					} else if (e.cancelled) {
						actIndicator(false);
					}
				});
			}
			else
			{
				actIndicator(true);
				fQuery();
			}
		}
	);
	
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
	indexCategories++;
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
		
		categories.push(
			{
				api_name: achievement.api_name,
				display_name: achievement.display_name
			}
		);
		
		for(var j = 0; j < achievement.projects.length; j++)
		{
			
			var project = achievement.projects[j];
			counter += project.achievements.length;
			
			projects.push(
				{
					api_name: project.api_name,
					display_name: project.display_name,
					total_badge: project.total_badge
				}
			);
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