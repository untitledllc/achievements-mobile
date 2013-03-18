/**
 * @author Gom_Dzhabbar
 */

var TiTools = undefined;
var ui = undefined;
var tempAchivs = [];
var typeProject = "null";
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
	
	Ti.API.info(window.info);
	
	// Загрузка контента окна
	ui = TiTools.UI.Loader.load("Views/GeneralWindow.js", window);
	ui.counter.text = window.achievements.length;
	ui.typeProject.addEventListener("click",function(event)
	{
		ui.typeProject.hide();
		ui.nameProject.hide();
		
		if(ui.list != undefined)
		{
			if(ui.list.visible == false)
			{
				ui.rowTextAchivs.text = "typeProject";
				
				var massRow = [];
				for(var i = 0; i < window.categories.length; i++)
				{
					var row = TiTools.UI.Loader.load("Views/list.js", ui.placeList);
					massRow.push(row);
					
					row.rowTextAchivs.text = window.categories[i].display_name;
					row.rowAchivs.api_name = window.categories[i].api_name;
					row.rowAchivs.display_name = window.categories[i].display_name;
										
					row.rowAchivs.addEventListener("click",function(event)
					{
						Ti.API.info(event);
						
						typeProject = event.source.api_name;
						ui.typeProject.text = event.source.display_name;
						
						Ti.API.info(typeProject);
						
						ui.typeProject.show();
						ui.nameProject.show();
						ui.list.visible = false;
						for(var ii = 0; ii != i; ii++)
						{
							massRow[ii].rowAchivs.superview.remove(massRow[ii].rowAchivs);
						}
						
						delListAll(window,typeProject);
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
				info : window.info
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
	for(var i = 0; i < window.achievements.length; i++)
	{
		Ti.API.info(window.achievements[i].projects.api_name + "    " + categiry);
		
		if(window.achievements[i].projects.api_name == categiry || categiry == "null")
		{
			Ti.API.info('+');
			
			var row = TiTools.UI.Loader.load("Views/ViewAchivs.js", ui.preAchivs);
			
			tempAchivs.push(row);
			
			row.date.text = window.achievements[i].achievements.create_time;
			row.image.image = window.achievements[i].achievements.pic;
			row.name.text = window.achievements[i].achievements.display_name;
			row.desc.text = window.achievements[i].achievements.desc;
			
			row.viewAchivs.data = {
				image: window.achievements[i].achievements.pic,
				nameAchivs: window.achievements[i].achievements.display_name,
				textAchivs: window.achievements[i].achievements.details
			}
			
			row.viewAchivs.addEventListener("click",function(event)
			{
				var win = TiTools.UI.Controls.createWindow(
					{
						main : "Controllers/preViewAchivs.js",
						navBarHidden : true,
						nameAchivs: event.source.data.nameAchivs,
						textAchivs: event.source.data.textAchivs,
						image: event.source.data.image
					}
				);
				win.initialize();
				win.open();	
			});
		}
	}
}
function delListAll(window,categiry)
{
	Ti.API.info('start_____');
	for(var i = 0; i < tempAchivs.length; i++)
	{
		tempAchivs[i].viewAchivs.superview.remove(tempAchivs[i].viewAchivs);
		Ti.API.info('del');
	}
	tempAchivs = [];
	
	createListAchivsAll(window,categiry);
	
	
}
function createListAchivsAll(window,categiry)
{
	for(var i = 0; i < window.achievements.length; i++)
	{
		Ti.API.info(window.achievements[i].projects.api_name + "    " + categiry);
		
		if(window.achievements[i].categories.api_name == categiry || categiry == "null")
		{
			Ti.API.info('+');
			
			var row = TiTools.UI.Loader.load("Views/ViewAchivs.js", ui.preAchivs);
			
			tempAchivs.push(row);
			
			row.date.text = window.achievements[i].achievements.create_time;
			row.image.image = window.achievements[i].achievements.pic;
			row.name.text = window.achievements[i].achievements.display_name;
			row.desc.text = window.achievements[i].achievements.desc;
			
			row.viewAchivs.data = {
				image: window.achievements[i].achievements.pic,
				nameAchivs: window.achievements[i].achievements.display_name,
				textAchivs: window.achievements[i].achievements.details
			}
			
			row.viewAchivs.addEventListener("click",function(event)
			{
				var win = TiTools.UI.Controls.createWindow(
					{
						main : "Controllers/preViewAchivs.js",
						navBarHidden : true,
						nameAchivs: event.source.data.nameAchivs,
						textAchivs: event.source.data.textAchivs,
						image: event.source.data.image
					}
				);
				win.initialize();
				win.open();	
			});
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
	if(ui.list.visible == false)
		{
			ui.typeProject.hide();
			ui.nameProject.hide();
			
			ui.rowTextAchivs.text = "NamePriject";
			
			var massRow = [];
			
			for(var i = 0; i < window.projects.length; i++)
			{
				Ti.API.info(window.projects[i]);
				if(window.projects[i].categories == category || category == "null")
				{
					var row = TiTools.UI.Loader.load("Views/list.js", ui.placeList);
					massRow.push(row);
					
					row.rowTextAchivs.text = window.projects[i].display_name;
					row.rowAchivs.api_name = window.projects[i].api_name;
					row.rowAchivs.display_name = window.projects[i].display_name;
					
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
	Ti.API.info(JSON.parse(data).player_id);
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