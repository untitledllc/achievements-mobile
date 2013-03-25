/**
 * @author Gom_Dzhabbar
 */

var TiTools = undefined;
var ui = undefined;
var tempAchivs = [];
var typeProject = "null";
var achievements = undefined;
var projects = undefined;
var categories = undefined;
var info = undefined;
var counter = 0;

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
	categories = window.categories;
	projects = window.projects;
	counter = window.counter;
	info = window.info;
	
	//Ti.API.info(achievements);
	
	// Загрузка контента окна
	ui = TiTools.UI.Loader.load("Views/GeneralWindow.js", window);
	
	ui.counter.text = counter;
	
	ui.typeProject.addEventListener("click",function(event)
	{
		ui.typeProject.hide();
		ui.nameProject.hide();
		
		ui.nameProject.text = "Проект";
		
		if(ui.list != undefined)
		{
			if(ui.list.visible == false)
			{
				ui.rowTextAchivs.text = "Категории:";
				
				var massRow = [];
				for(var i = 0; i < categories.length; i++)
				{
					var category = categories[i];
					var row = TiTools.UI.Loader.load("Views/list.js", ui.placeList);
					massRow.push(row);
					
					row.rowTextAchivs.text = category.display_name;
					row.rowAchivs.api_name = category.api_name;
					row.rowAchivs.display_name = category.display_name;
										
					row.rowAchivs.addEventListener("click",function(event)
					{
						
						typeProject = event.source.api_name;
						ui.typeProject.text = event.source.display_name;
						
						ui.typeProject.show();
						ui.nameProject.show();
						ui.list.visible = false;
						for(var ii = 0; ii != i; ii++)
						{
							massRow[ii].rowAchivs.superview.remove(massRow[ii].rowAchivs);
						}
						
						delList(window,typeProject);
					});
				}
				ui.list.visible = true;
			}
			else
			{
				ui.typeProject.show();
				ui.nameProject.show();
				ui.list.visible = false;
			}
		}
	});
	
	ui.nameProject.addEventListener("click",function(event)
	{
		createListName(window,typeProject);
	});
	
	ui.add.addEventListener("click",function(event)
	{
		var winAdd = TiTools.UI.Controls.createWindow(
			{
				main : "Controllers/add.js",
				navBarHidden : true,
				achievements : achievements
			}
		);
		winAdd.initialize();
		winAdd.open();
	});
	
	ui.profile.addEventListener("click",function(event)
	{
		var winAdd = TiTools.UI.Controls.createWindow(
			{
				main : "Controllers/Profile.js",
				navBarHidden : true,
				info : info,
				achievements: achievements,
				counter : counter
			}
		);
		winAdd.initialize();
		winAdd.open();
	});
}
//---------------------------------------------//
// Функции лентяйки
//---------------------------------------------//

// Обработчик при открытии окна
function onWindowOpen(window, event)
{
	
	createListAchivs(window,"null");
	
	// var win = TiTools.UI.Controls.createWindow(
		// {
			// main : "Controllers/preViewAchivs.js",
			// navBarHidden : true,
			// nameAchivs: "Крутая ачивка",
			// textAchivs: "ярмляоырвапдолыфрвпдафлоырваплыроадлоывф"
		// }
	// );
	// win.initialize();
	// win.open();	
}
///-----сосдание списка ачивок-----//
function createListAchivs(window,categiry)
{
	for(var i = 0; i < achievements.length; i++)
	{
		for(var j = 0; j < achievements[i].projects.length; j++)
		{
			for(var k = 0; k < achievements[i].projects[j].achievements.length; k++)
			{
				if(achievements[i].projects[j].api_name == categiry || categiry == "null" || achievements[i].api_name == categiry)
				{
					Ti.API.info('+');
					
					var achievement = achievements[i].projects[j].achievements[k];
					var row = TiTools.UI.Loader.load("Views/ViewAchivs.js", ui.preAchivs);
					
					tempAchivs.push(row);
					
					row.date.text   = achievement.create_time;
					row.image.image = achievement.pic;
					row.name.text   = achievement.display_name;
					row.name.color  = achievement.color;
					row.desc.text   = achievement.desc;
					
					row.viewAchivs.data = {
						image: achievement.pic,
						nameAchivs: achievement.display_name,
						desc: achievement.desc,
						details: achievement.details,
						adv: achievement.adv,
						bonus: achievement.bonuses
					}
					
					row.viewAchivs.addEventListener("click",function(event)
					{
						var sourceData = event.source.data;
						var win = TiTools.UI.Controls.createWindow(
							{
								main : "Controllers/preViewAchivs.js",
								navBarHidden : true,
								nameAchivs: sourceData.nameAchivs,
								desc: sourceData.desc,
								details: sourceData.details,
								adv: sourceData.adv,
								image: sourceData.image,
								bonus: sourceData.bonus
							}
						);
						win.initialize();
						win.open();	
					});
				}
			}
		}
	}
}
function delList(window,categiry)
{
	Ti.API.info('start_____');
	for(var i = 0; i < tempAchivs.length; i++)
	{
		tempAchivs[i].viewAchivs.superview.remove(tempAchivs[i].viewAchivs);
		Ti.API.info('del');
	}
	tempAchivs = [];
	
	createListAchivs(window,categiry);
}
function createListName(window,category)
{
	Ti.API.info(category);
	
	if(ui.list.visible == false)
		{
			ui.typeProject.hide();
			ui.nameProject.hide();
			
			ui.rowTextAchivs.text = "Проекты:";
			
			var massRow = [];
			
			for(var i = 0; i < achievements.length; i++)
			{
				
				if(achievements[i].api_name == category || category == "null")
				{
					for(var j = 0; j < achievements[i].projects.length; j++)
					{
						
						var row = TiTools.UI.Loader.load("Views/list.js", ui.placeList);
						massRow.push(row);
						
						row.rowTextAchivs.text = achievements[i].projects[j].display_name;
						row.rowAchivs.api_name = achievements[i].projects[j].api_name;
						row.rowAchivs.display_name = achievements[i].projects[j].display_name;
						
						row.rowAchivs.addEventListener("click",function(event)
						{
							ui.typeProject.show();
							ui.nameProject.show();
							
							ui.list.visible = false;
							for(var ii = 0; ii != massRow.length; ii++)
							{
								massRow[ii].rowAchivs.superview.remove(massRow[ii].rowAchivs);
							}
							
							ui.nameProject.text = event.source.display_name;
							
							delList(window,event.source.api_name);
						});
					}
				}
			}
			
			if(massRow.length == 0)
			{
				var row = TiTools.UI.Loader.load("Views/list.js", ui.placeList);
					massRow.push(row);
					
					row.rowTextAchivs.text = "Нет проектов";
					
					row.rowAchivs.addEventListener("click",function(event)
					{
						ui.typeProject.show();
						ui.nameProject.show();
						
						ui.list.visible = false;
						for(var ii = 0; ii != i; ii++)
						{
							massRow[ii].rowAchivs.superview.remove(massRow[ii].rowAchivs);
						}
					});
			}
			
			ui.list.visible = true;
		}
		else
		{
			ui.typeProject.show();
			ui.nameProject.show();
			ui.list.visible = false;
		}
}
function saveIdUser(data)
{
	Ti.App.Properties.setString("id_user",JSON.parse(data).player_id);
	Ti.API.info('saveID');
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