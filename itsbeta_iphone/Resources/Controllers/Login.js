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
var achievements = [];
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
	ui = TiTools.UI.Loader.load("Views/Login.js", window);
	
	ui.infacebook.addEventListener("click", function(event)
	{
		
		
		Titanium.Facebook.appid = "264918200296425";
		Titanium.Facebook.permissions = ['publish_stream', 'read_stream'];
		
		Ti.Facebook.logout();
		// clear cookies
		var client = Ti.Network.createHTTPClient();
		client.clearCookies('https://login.facebook.com');
		
		//Ti.API.info(Titanium.Facebook.loggedIn);
		
 
		function fQuery() 
		{
			var fbuid = Titanium.Facebook.uid; 
			var myQuery = "SELECT name, birthday_date,current_location FROM user WHERE uid = "+fbuid;
		
		
		Titanium.Facebook.request('fql.query', {query: myQuery},  function(x)
			{
				var results = JSON.parse(x.result);
				info = {
					name: results[0].name,
					birthday: results[0].birthday_date,
					city: results[0].current_location.city,
					country: results[0].current_location.country
				}
				
				Ti.API.info(info);
				//-------------------
				
				var query = {
					params: JSON.stringify({
						access_token: access_token,
						type: "fb_user_id",
						id: "100004228292121"
					}),
					url: "/playerid.json"
				};
				
				queryItsbeta(query,saveIdUser);
				
			});
		};
		
		if(Titanium.Facebook.loggedIn == 0)
		{
			Ti.Facebook.authorize();
			Ti.Facebook.addEventListener('login',function(event){
				Ti.API.info("login");
				fQuery();
			});
		}
		else
		{
			fQuery();
		}
	});
	
 }
 
//---------------------------------------------//

function queryItsbeta(params,collback)
{
	Ti.API.info('itsbeta');
	TiTools.HTTP.response(
			{
				reguest: {
				method: 'POST',
					url: 'http://www.itsbeta.com/s/info/' + params.url,
					header: [
						{
							type: 'Content-Type',
							value: 'application/json; charset=utf-8'
						}
					],
					post: params.params
				},
				success: function(success)
						{
							Ti.API.info('ok');
							//Ti.API.info(success.responseText);
							collback(success.responseText);
						},
				failure: function(failure)
						{
							
							Ti.API.info('error');
							//Ti.API.info(failure.responseText);
							collback(failure.responseText);
						}
			}
		);
}
//-----collback-----
function saveIdUser(data)
{
	Ti.App.Properties.setString("id_user",JSON.parse(data).player_id);
	Ti.API.info('saveID');
	Ti.API.info(JSON.parse(data).player_id);
	id_user = JSON.parse(data).player_id;
	
	Ti.API.info('загрузка категорий');
	var query = {
		params: JSON.stringify({
			access_token : access_token,
		}),
		url : "/categories.json"
	};
	
	queryItsbeta(query,saveCategories);
}

function saveCategories(data)
{
	categories = JSON.parse(data);
	
	Ti.API.info(categories);
	
	Ti.App.Properties.setString("categories",data);
	
	
	Ti.API.info('Загрузка проектов')
	var query = {
		params: JSON.stringify({
			access_token : access_token,
			category_name : categories[indexCategories].api_name
		}),
		url : "/projects.json"
	};
	
	queryItsbeta(query,saveProjects);
	
	
}
function saveProjects(data)
{
	indexCategories++;
	var temp = JSON.parse(data);
	
	if(indexCategories < categories.length)
	{
		Ti.API.info('+');
		
		var query = {
			params: JSON.stringify({
				access_token : access_token,
				category_name : categories[indexCategories].api_name
			}),
			url : "/projects.json"
		};
		
		
		for(var i = 0; i < temp.length; i++)
		{
			projects.push(temp[i]);
			if(i + 1==temp.length)
			{
				queryItsbeta(query,saveProjects);
				Ti.API.info(projects.length);
			}
		}
	}
	else
	{
		for(var i = 0; i < temp.length; i++)
		{
			projects.push(temp[i]);
		}
		
		alert("всего проектов " + projects.length);
		
		Ti.API.info(projects);
		
		Ti.API.info('Загрузка ачивок')
		
		indexCategories = 0;
		
		var query = {
			params: JSON.stringify({
				access_token : access_token,
				player_id : id_user,
				project_id : projects[indexCategories].id
			}),
			url : "/achievements.json"
		};
		queryItsbeta(query,saveAchivs);
	}
	
}
function saveAchivs(data)
{
	indexCategories++;
	var temp = JSON.parse(data);
	Ti.API.info(indexCategories);
	
	if(indexCategories < projects.length)
	{
		Ti.API.info('+');
		
		var query = {
			params: JSON.stringify({
				access_token : access_token,
				player_id : id_user,
				project_id : projects[indexCategories].id
			}),
			url : "/achievements.json"
		};
		//Ti.API.info(temp);
		if(temp.length != 0)
		{
			for(var i = 0; i < temp[0].projects[0].achievements.length; i++)
			{
				//Ti.API.info(temp[0].projects[0].achievements[i]);
				
				achievements.push(temp[0].projects[0].achievements[i]);
				if(i + 1 == temp.length)
				{
					queryItsbeta(query,saveAchivs);
					Ti.API.info(achievements.length);
				}
			}
		}
		else
		{
			queryItsbeta(query,saveAchivs);
		}
	}
	else
	{
		if(temp.length != 0)
		{
			for(var i = 0; i < temp[0].projects[0].achievements.length; i++)
			{
				achievements.push(temp[0].projects[0].achievements[i]);
			}
		}
		
		alert("всего ачивок " + achievements.length);
		Ti.API.info(achievements);
		
		var win = TiTools.UI.Controls.createWindow(
			{
				main : "Controllers/GeneralWindow.js",
				navBarHidden : true,
				info: info,
				achievements : achievements,
				projects: projects,
				categories : categories
			}
		);
		win.initialize();
		win.open();
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