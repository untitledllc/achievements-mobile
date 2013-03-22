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
var itsbeta;

var achievements = [];
var newCategories = [];
var newProjects = [];

var counter = 0;
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
	
	ui.infacebook.addEventListener("click", function(event)
	{
		Titanium.Facebook.appid = "264918200296425";
		Titanium.Facebook.permissions = ['publish_stream', 'read_stream'];
		
		// Ti.Facebook.logout();
		// // clear cookies
		// var client = Ti.Network.createHTTPClient();
		// client.clearCookies('https://login.facebook.com');
		
		Ti.API.info(Titanium.Facebook.loggedIn);
		
		function fQuery() 
		{
			var fbuid = Titanium.Facebook.uid; 
			var myQuery = "SELECT name, birthday_date,current_location FROM user WHERE uid = "+fbuid;
		
		
			Titanium.Facebook.request('fql.query', {query: myQuery},  function(x)
			{
				var results = JSON.parse(x.result);
				info = {
					name: results[0].name,
					birthday: results[0].birthday_date
					//city: results[0].current_location.city,
					//country: results[0].current_location.country
				}
				
				// get achievements by user id
				itsbeta.getAchievementsByUid(fbuid, saveAchivs);
			});
		};
		
		if(Titanium.Facebook.loggedIn == 0)
		{
			Ti.Facebook.authorize();
			Ti.Facebook.addEventListener('login',function(event){
				
				actIndicator(true);
				
				Ti.API.info("login");
				fQuery();
			});
		}
		else
		{
			actIndicator(true);
			fQuery();
		}
	});
	
 }
 
//---------------------------------------------//

function saveAchivs(data)
{
	indexCategories++;
	achievements = JSON.parse(data.responseText);
	
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
			counter : counter
		}
	);
	win.initialize();
	win.open();
}
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