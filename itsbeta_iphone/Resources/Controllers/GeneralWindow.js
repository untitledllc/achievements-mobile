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
var itsbeta = undefined;
var massRow = [];
var category = [];
//---------------------------------------------//
// Глобальные переменные для окна
//---------------------------------------------//
var animation = Titanium.UI.createAnimation();
var animationEnd = Titanium.UI.createAnimation();
animation.top = 107;
animation.duration = 500;

animationEnd.top = -248;
animationEnd.duration = 500;

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
	
	Ti.API.info(achievements);
	
	// Загрузка контента окна
	ui = TiTools.UI.Loader.load("Views/GeneralWindow.js", window);
	itsbeta = require("Utils/Itsbeta");
	
	ui.counter.text = counter;
	
	ui.typeProject.addEventListener("click",function(event)
	{
		//вызываем прозрачную панель для борьбы с многокликом//
		ui.transparentView.show();
		//---------------------------------------------------//
		
		//ui.typeProject.hide();
		//ui.nameProject.hide();
		
		//ui.nameProject.text = "Проект";
		//ui.typeProject.text = "Категория";
		
		ui.placeListViewCancel.show();
		
		if(ui.list != undefined)
		{
			if(ui.list.visible == false)
			{
				//ui.rowTextAchivs.text = "Категории:";
				massRow = [];
				
				category.display_name;
				category.api_name;
				category.display_name;
				
				var allRow = {
					display_name : "Все категории",
					api_name : "null",
				};
				//--- делаем первую ячейку "все категории"
				createListRow(allRow,massRow);
				
				for(var i = 0; i < categories.length; i++)
				{
					category = categories[i];
					//--строю одну ячейку--
					createListRow(category,massRow,i);
					//---------------------
				}
				ui.list.visible = true;
				ui.placeListView.animate(animation);
				ui.transparentView.hide();
			}
			else
			{
				ui.typeProject.show();
				ui.nameProject.show();
				ui.list.visible = false;
			}
		}
	});
	
	ui.placeListView.addEventListener("singletap",function(event)
	{
		undefClick();
	});
	
	ui.placeListViewCancel.addEventListener("singletap",function(event)
	{
		undefClick();
	});
	
	ui.nameProject.addEventListener("singletap",function(event)
	{
		ui.transparentView.show();
		ui.placeListViewCancel.show();
		createListName(window,typeProject);
	});
	
	ui.add.addEventListener("singletap",function(event)
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
	
	ui.profile.addEventListener("singletap",function(event)
	{
		var winAdd = TiTools.UI.Controls.createWindow(
			{
				main : "Controllers/Profile.js",
				backgroundColor: "#fff",
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
	
	Ti.App.addEventListener("actHide",function(event)
	{
		actIndicator(false);
	});
	
	Ti.App.addEventListener("logout",function(event){
		window.close();
	});
	
	var start = TiTools.Global.get("startAchivs");
	if(start != undefined)
	{
		Ti.API.info('OPEN');
		Ti.API.info(start);
		var win = TiTools.UI.Controls.createWindow(
			{
				main : "Controllers/preViewAchivs.js",
				navBarHidden : true,
				nameAchivs: start.display_name,
				desc: start.desc,
				details: start.details,
				adv: start.adv,
				image: start.pic,
				bonus: start.bonuses
			}
		);
		win.initialize();
		win.open();	
	}
	
	Ti.App.addEventListener("reload",function(event){
		// ---- delete ----
		for(var i = 0; i < tempAchivs.length; i++)
		{
			tempAchivs[i].viewAchivs.superview.remove(tempAchivs[i].viewAchivs);
			Ti.API.info('del');
		}
		tempAchivs = [];
		//-----------------
		
		// get achievements by user id
		itsbeta.getAchievementsByUid(info.fbuid, reSaveAchivs);
		
	});
	
	createListAchivs(window,"null");
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
					var achievement = achievements[i].projects[j].achievements[k];
					var row = TiTools.UI.Loader.load("Views/ViewAchivs.js", ui.preAchivs);
					
					tempAchivs.push(row);
					
					row.date.text = TiTools.DateTime.format(new Date(achievement.create_time), "$dd.$mm.$yyyy");
					row.image.image = achievement.pic;
					row.name.text = achievement.display_name;
					row.name.color = achievements[i].projects[j].color;
					row.desc.text = achievement.desc;
					
					for(var n = 0; n < achievement.bonuses.length; n++)
					{
						var tempColor;
						
						if( achievement.bonuses[n].bonus_type == "bonus")
						{
							tempColor = "red";
						}
						if( achievement.bonuses[n].bonus_type == "present")
						{
							tempColor = "green";
						}
						if( achievement.bonuses[n].bonus_type == "discount")
						{
							tempColor = "gray";
						}
						
						var bonus = preViewBonus({
							text: achievement.bonuses[n].bonus_type,
							color: tempColor
						});
						
						row.addBonus.add(bonus);
					}
					
					row.viewAchivs.data = {
						image: achievement.pic,
						nameAchivs: achievement.display_name,
						desc: achievement.desc,
						details: achievement.details,
						adv: achievement.adv,
						bonus: achievement.bonuses
					}
					
					row.viewAchivs.addEventListener("singletap",function(event)
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
	Ti.App.fireEvent("actHide");
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
	//Ti.API.info(category);
	
	if(ui.list.visible == false)
		{
			//ui.typeProject.hide();
			//ui.nameProject.hide();
			
			//ui.rowTextAchivs.text = "Проекты:";
			
			massRow = [];
			
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
						
						row.rowAchivs.addEventListener("singletap",function(event)
						{
							actIndicator(true);
							
							var animationHandler = function() {
								animationEnd.removeEventListener('complete',animationHandler);
								
								ui.transparentView.hide();
								
								ui.typeProject.show();
								ui.nameProject.show();
								
								ui.list.visible = false;
								for(var ii = 0; ii != massRow.length; ii++)
								{
									massRow[ii].rowAchivs.superview.remove(massRow[ii].rowAchivs);
								}
								
								ui.nameProject.text = event.source.display_name;
								
								delList(window,event.source.api_name);
							};
							
							animationEnd.addEventListener('complete',animationHandler);
							ui.placeListView.animate(animationEnd);
						});
					}
				}
			}
			
			ui.transparentView.hide();
			ui.placeListView.animate(animation);
			
			// if(massRow.length == 0)
			// {
				// var row = TiTools.UI.Loader.load("Views/list.js", ui.placeList);
					// massRow.push(row);
// 					
					// row.rowTextAchivs.text = "Нет проектов";
// 					
					// row.rowAchivs.addEventListener("click",function(event)
					// {
						// ui.typeProject.show();
						// ui.nameProject.show();
// 						
						// ui.list.visible = false;
						// for(var ii = 0; ii != i; ii++)
						// {
							// massRow[ii].rowAchivs.superview.remove(massRow[ii].rowAchivs);
						// }
					// });
			// }
			
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
function reSaveAchivs(data)
{
	achievements = JSON.parse(data.responseText);
	categories = [];
	projects = [];
	
	//---- собираем список категорий и список проектов -----//
	counter = 0;
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
			
			//Ti.API.info(achievement.display_name + "  --  " + project.display_name);
			
			projects.push(
				{
					api_name: project.api_name,
					display_name: project.display_name,
					total_badge: project.total_badge
				}
			);
		}
	}
	ui.counter.text = counter;
	createListAchivs(window,"null");
}
function preViewBonus(params)
{
	var bonus = Ti.UI.createView({
		top: 10,
		height: 20,
		width: 20,
		backgroundColor: params.color,
	});
	
	var label = Ti.UI.createLabel({
		height: Ti.UI.SIZE,
		width: Ti.UI.SIZE,
		text: params.text
	});
	
	bonus.add(label);
	
	return bonus;
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
function searchRow()
{
	// for(var i = 0; i < achievements.length; i++)
	// {
		// for(var j = 0; j < achievements[i].projects.length; j++)
		// {
			// for(var k = 0; k < achievements[i].projects[j].achievements.length; k++)
			// {
				// if(achievements[i].projects[j].achievements[k].badge_name != "itsbeta")
				// {
					// createListAchivs(window,"null");
					// return;
				// }
			// }
		// }
	// }
	//itsbeta.firstStart(info,itsbeta.getAchievementsByUid(info.fbuid, reSaveAchivs));
}
function createListRow(category,massRow)
{
	var row = TiTools.UI.Loader.load("Views/list.js", ui.placeList);
	massRow.push(row);
	
	row.rowTextAchivs.text = category.display_name;
	row.rowAchivs.api_name = category.api_name;
	row.rowAchivs.display_name = category.display_name;
						
	row.rowAchivs.addEventListener("singletap",function(event)
	{
		actIndicator(true);
		
		
		var animationHandler = function() {
			animationEnd.removeEventListener('complete',animationHandler);
		
			typeProject = event.source.api_name;
			ui.typeProject.text = event.source.display_name;
			
			ui.typeProject.show();
			ui.nameProject.show();
			ui.list.visible = false;
			for(var ii = 0; ii < massRow.length; ii++)
			{
				massRow[ii].rowAchivs.superview.remove(massRow[ii].rowAchivs);
			}
			
			ui.placeListView.animate(animationEnd);
			Ti.API.info('++');
			delList(window,typeProject);
		};
		
		animationEnd.addEventListener('complete',animationHandler);
		ui.placeListView.animate(animationEnd);
		
	});
	
}
function undefClick()
{
	ui.placeListViewCancel.hide();
		
		var animationHandler = function() {
			animationEnd.removeEventListener('complete',animationHandler);
			
			ui.transparentView.hide();
			
			ui.list.visible = false;
			for(var ii = 0; ii < massRow.length; ii++)
			{
				massRow[ii].rowAchivs.superview.remove(massRow[ii].rowAchivs);
			}
			
			ui.placeListView.animate(animationEnd);
		};
		
		animationEnd.addEventListener('complete',animationHandler);
		
		ui.placeListView.animate(animationEnd);
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