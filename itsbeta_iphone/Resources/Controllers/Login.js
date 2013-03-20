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
var newCategories = [];
var newProjects = [];
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
					birthday: results[0].birthday_date,
					city: results[0].current_location.city,
					country: results[0].current_location.country
				}
				
				//-------------------
				
				var query = {
					params: JSON.stringify({
						access_token: access_token,
						type: "fb_user_id",
						id: fbuid
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
			}
		}
	}
	else
	{
		for(var i = 0; i < temp.length; i++)
		{
			projects.push(temp[i]);
		}
		
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
	Ti.API.info("temp length = " + temp.length)
	
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
		if(temp.length != 0)
		{
			if( temp[0].projects[0].achievements.length != 0)
			{
				newProjects.push(
					{
						api_name : temp[0].projects[0].api_name,
						display_name : temp[0].projects[0].display_name,
						categories : temp[0].api_name
					});
					
				newCategories.push({
						api_name : temp[0].api_name,
						display_name : temp[0].display_name,
						total_badge : temp[0].projects[0].total_badge
					});
			}
			
			for(var i = 0; i < temp[0].projects[0].achievements.length; i++)
			{
				achievements.push(
					{
						achievements : temp[0].projects[0].achievements[i],
						projects : 
							{
								api_name : temp[0].projects[0].api_name,
								display_name : temp[0].projects[0].display_name,
								color: temp[0].projects[0].color
							},
						categories : 
							{
								api_name : temp[0].api_name,
								display_name : temp[0].display_name,
								total_badge : temp[0].projects[0].total_badge
							}
					}
				);
				if(i + 1 == temp.length)
				{
					queryItsbeta(query,saveAchivs);
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
			if( temp[0].projects[0].achievements.length != 0)
			{
				newProjects.push(
					{
						api_name : temp[0].projects[0].api_name,
						display_name : temp[0].projects[0].display_name,
						categories : temp[0].api_name
					});
					
				newCategories.push({
						api_name : temp[0].api_name,
						display_name : temp[0].display_name,
						total_badge : temp[0].projects[0].total_badge
					});
			}
			
			for(var i = 0; i < temp[0].projects[0].achievements.length; i++)
			{
				achievements.push(temp[0]);
			}
		}
		
		//alert("всего ачивок " + achievements.length);
		
		actIndicator(false);
		
		var win = TiTools.UI.Controls.createWindow(
			{
				main : "Controllers/GeneralWindow.js",
				navBarHidden : true,
				info: info,
				achievements : achievements,
				projects: newProjects,
				categories : newCategories
			}
		);
		win.initialize();
		win.open();
	}
	
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