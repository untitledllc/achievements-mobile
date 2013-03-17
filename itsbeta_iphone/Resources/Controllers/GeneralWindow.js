/**
 * @author Gom_Dzhabbar
 */

var TiTools = undefined;
var ui = undefined;
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
	ui.typePriject.addEventListener("click",function(event)
	{
		ui.typePriject.hide();
		ui.nameProject.hide();
		
		if(ui.list != undefined)
		{
			if(ui.list.visible == false)
			{
				ui.rowTextAchivs.text = "typePriject";
				
				var massRow = [];
				for(var i = 0; i < window.categories.length; i++)
				{
					var row = TiTools.UI.Loader.load("Views/list.js", ui.placeList);
					massRow.push(row);
					
					row.rowTextAchivs.text = window.categories[i].display_name;
					
					row.rowAchivs.addEventListener("click",function(event)
					{
						ui.typePriject.show();
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
				ui.typePriject.show();
				ui.nameProject.show();
				ui.list.visible = false;
			}
		}
	});
	
	ui.nameProject.addEventListener("click",function(event)
	{
		if(ui.list.visible == false)
		{
			ui.typePriject.hide();
			ui.nameProject.hide();
			
			ui.rowTextAchivs.text = "NamePriject";
			
			var massRow = [];
			for(var i = 0; i < 10; i++)
			{
				var row = TiTools.UI.Loader.load("Views/list.js", ui.placeList);
				massRow.push(row);
				
				row.rowTextAchivs.text = window.projects[i].display_name;
				
				row.rowAchivs.addEventListener("click",function(event)
				{
					
					ui.typePriject.show();
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
			ui.typePriject.show();
			ui.nameProject.show();
			ui.list.visible = false;
		}
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
	
	
	for(var i = 0; i < window.achievements.length; i++)
	{
		var row = TiTools.UI.Loader.load("Views/ViewAchivs.js", ui.preAchivs);
		row.image.image = window.achievements[i].pic;
		row.name.text = window.achievements[i].display_name;
		row.desc.text = window.achievements[i].desc;
		row.viewAchivs.data = {
			image: window.achievements[i].pic,
			nameAchivs: window.achievements[i].display_name,
			textAchivs: window.achievements[i].details
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