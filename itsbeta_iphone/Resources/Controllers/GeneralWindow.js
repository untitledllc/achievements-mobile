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
var tempNewAchivs = undefined;
var selectCategory = "null";
var selectProject = "null";
var subCategoryClick = false;
var placeListHeight = 0;

var lastAchivs = [];
var singlTap = false;
//---------------------------------------------//
// Глобальные переменные для окна
//---------------------------------------------//
var animation = Titanium.UI.createAnimation();
var animationEnd = Titanium.UI.createAnimation();
animation.top = 55;
animation.duration = 500;

animationEnd.top = -395;
animationEnd.duration = 500;

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
	
	// Загрузка контента окна
	ui = TiTools.UI.Loader.load("Views/GeneralWindow.js", window);
	itsbeta = require("Utils/Itsbeta");
	
	ui.counter.text = counter;
	
	Ti.API.info(achievements);
	
	ui.typeProjectClick.addEventListener("click",function(event)
	{
		//вызываем прозрачную панель для борьбы с многокликом//
		ui.transparentView.show();
		//---------------------------------------------------//
		
		ui.placeListViewCancel.show();
		
		placeListHeight = 0;
		
		if(ui.list != undefined)
		{
			if(ui.list.visible == false)
			{
				massRow = [];
				
				var allRow = {
					display_name: "Все",
					api_name: "null",
				};
				//--- делаем первую ячейку "все категории"
				createListRow(allRow, massRow);
				
				placeListHeight += 50;
				
				for(var i = 0; i < categories.length; i++)
				{
					placeListHeight += 50;
					
					category = categories[i];
					//--строю одну ячейку--
					createListRow(category, massRow, i);
					//---------------------
				}
				ui.list.visible = true;
				
				if(placeListHeight > 400)
				{
					ui.placeList.height = 440;
				}
				else
				{
					ui.placeList.height = placeListHeight;
				}
				
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
	
	ui.placeListView.addEventListener("singletap", undefClick);
	ui.placeListViewCancel.addEventListener("singletap", undefClick);
	
	ui.nameProjectClick.addEventListener("singletap", function(event)
	{
		if(subCategoryClick == false)
		{
			subCategoryClick = true;
			
			ui.transparentView.show();
			ui.placeListViewCancel.show();
			createListName(window, typeProject);
		}
	});
	
	// add code
	decorateButtonChildren.call(
		ui.addClick,
		function() {
			var winAdd = TiTools.UI.Controls.createWindow({
				main: "Controllers/add.js",
				navBarHidden: true,
				achievements: achievements
			});
			winAdd.initialize();
			winAdd.open();	
		}
	);
	
	// profile
	decorateButtonChildren.call(
		ui.profileClick,
		function() {
			var winAdd = TiTools.UI.Controls.createWindow({
				main: "Controllers/Profile.js",
				backgroundColor: "#fff",
				navBarHidden: true,
				info: info,
				achievements: achievements,
				counter: counter
			});
			winAdd.initialize();
			winAdd.open();	
		}
	);
	
	// ----- PULL TO REFRESH ----- //
	
	var achivsWrapper = ui.preAchivs,
		pullToRefresh = TiTools.UI.Loader.load('Views/PullToRefresh.js'),
		pulling       = false,
		reloading     = false,
		offset        = 0;
		
	achivsWrapper.add(pullToRefresh.me);
	pullToRefresh.hypno.start(); 
		
	// event handlers
	achivsWrapper.addEventListener('scroll', function(e) {
		offset = e.y;
		if(offset < -75.0 && !pulling && !reloading) {
			pulling = true;
			pullToRefresh.status.text = "Release to refresh";
		}
		else if((offset > -75.0 && offset < 0) && pulling && !reloading) {
			pulling = false;
			pullToRefresh.status.text = "Pull to refresh";
		}
	});
	
	achivsWrapper.addEventListener('dragEnd', function() {	
		if(pulling && !reloading) {
			achivsWrapper.top = Math.abs(offset)+6;
			achivsWrapper.animate({
					top: 86,
					duration: 200
				}, 
				function() {
			  		achivsWrapper.top = 86;
				}
			);
			reloading = true;
			pulling = false;
			pullToRefresh.status.text = "Refreshing";
			beginReloading();
		}
	});
	
	//---------------------------------------------//
	
	function beginReloading() {			
		setTimeout(endReloading, 5000);
	}
	
	function endReloading() {			
		achivsWrapper.animate({
				top: 6
			}, 
			function() {
				achivsWrapper.top = 6;
			}
		);
		
		reloading = false;
		pullToRefresh.status.text = "Pull to refresh";
	}
	
	// ----- END PULL TO REFRESH ----- //
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
		var win = TiTools.UI.Controls.createWindow({
			main : "Controllers/preViewAchivs.js",
			navBarHidden : true,
			nameAchivs: start.display_name,
			desc: start.desc,
			details: start.details,
			adv: start.adv,
			image: start.pic,
			bonus: start.bonuses
		});
		win.initialize();
		win.open();	
	}
	
	Ti.App.addEventListener("reload",function(event){
		// ---- delete ----
		ui.preAchivs.hide();
		actIndicator(true);
		tempNewAchivs = event.data;
		
		// -- не дописана, оптимизаия ----////
		//reloadAdd(tempNewAchivs);
		
		for(var i = 0; i < tempAchivs.length; i++)
		{
			tempAchivs[i].viewAchivs.superview.remove(tempAchivs[i].viewAchivs);
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
	ui.preAchivs.hide();
	actIndicator(true);
	
	ui.nameProject.text = "Подкатегории";
	ui.typeProject.text = "Категории";
	
	for(var i = 0, I = achievements.length; i < I; i++)
	{
		for(var j = 0, J = achievements[i].projects.length; j < J; j++)
		{
			for(var k = 0, K = achievements[i].projects[j].achievements.length; k < K; k++)
			{
				if(achievements[i].projects[j].api_name == categiry || categiry == "null" || achievements[i].api_name == categiry)
				{
					var achievement = achievements[i].projects[j].achievements[k];
					
					// ---- список имен ачивок и время их выдачи --- //
					lastAchivs.push({
						date: achievement.create_time,
						api_name: achievement.api_name
					});
					// ---------------------------------------------//
					
					var row = TiTools.UI.Loader.load("Views/ViewAchivs.js", ui.preAchivs);
					
					row.date.text = TiTools.DateTime.format(new Date(achievement.create_time), "$dd.$mm.$yyyy");
					row.api_name = achievement.api_name,
					row.image.image = achievement.pic;
					row.name.text = achievement.display_name;
					row.name.color = achievements[i].projects[j].color;
					row.desc.text = achievement.desc;
					row.category = achievements[i].api_name;
					row.project = achievements[i].projects[j].api_name;
					
					tempAchivs.push(row);
					
					for(var n = 0, N = achievement.bonuses.length; n < N; n++)
					{
						var bonus = preViewBonus(achievement.bonuses[n].bonus_type);
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
						if(singlTap == false)
						{
							singlTap = true;
							
							var sourceData = event.source.data;
							var win = TiTools.UI.Controls.createWindow({
								main: "Controllers/preViewAchivs.js",
								navBarHidden: true,
								nameAchivs: sourceData.nameAchivs,
								desc: sourceData.desc,
								details: sourceData.details,
								adv: sourceData.adv,
								image: sourceData.image,
								bonus: sourceData.bonus
							});
							
							win.addEventListener("close",function() {
								singlTap = false;
							});
							
							win.initialize();
							win.open();	
						}
					});
				}
			}
		}
		if(i+1 == achievements.length)
		{
			ui.preAchivs.show();
			actIndicator(false);
			
			Ti.App.fireEvent("actHide");
			
			if(tempNewAchivs != undefined)
			{
				var win = TiTools.UI.Controls.createWindow({
					main: "Controllers/preViewAchivs.js",
					navBarHidden: true,
					nameAchivs: tempNewAchivs.display_name,
					desc: tempNewAchivs.desc,
					details: tempNewAchivs.details,
					adv: tempNewAchivs.adv,
					image: tempNewAchivs.pic,
					bonus: tempNewAchivs.bonuses
				});
				win.initialize();
				win.open();
				
				tempNewAchivs = undefined;
			}
		}
	}
	///---------сортируем по дате -----////
	lastAchivs.sort(
		function(a, b)
		{
			var date1 = Date();
			var date2 = Date();
			
			date1 = a.date;
			date2 = b.date;
			
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
	Ti.API.info(lastAchivs);
}
function delList(window, categiry)
{
	ui.preAchivs.hide();
	
	for(var i = 0; i < tempAchivs.length; i++)
	{
		tempAchivs[i].viewAchivs.superview.remove(tempAchivs[i].viewAchivs);
	}
	tempAchivs = [];
	
	createListAchivs(window,categiry);
}
function createListName(window,category)
{
	if(ui.list.visible == false)
		{
			placeListHeight = 0;
			massRow = [];
			
			if(category == "null")
			{
				lastAchivsFunction(massRow);
				placeListHeight += 50;
			}
			
			for(var i = 0, I = achievements.length; i < I; i++)
			{
				if(achievements[i].api_name == category || category == "null")
				{
					for(var j = 0, J = achievements[i].projects.length; j < J; j++)
					{
						var row = TiTools.UI.Loader.load("Views/list.js", ui.placeList);
						massRow.push(row);
						
						placeListHeight += 50;
						
						row.rowTextAchivs.text = achievements[i].projects[j].display_name;
						row.rowAchivs.api_name = achievements[i].projects[j].api_name;
						row.rowAchivs.display_name = achievements[i].projects[j].display_name;
						
						row.rowAchivs.addEventListener("singletap",function(event)
						{
							subCategoryClick = false;
							
							selectProject = event.source.api_name;
							actIndicator(true);
							
							var animationHandler = function() {
								animationEnd.removeEventListener('complete',animationHandler);
								
								
								
								ui.transparentView.hide();
								
								ui.typeProject.show();
								ui.nameProject.show();
								
								ui.list.visible = false;
								
								ui.nameProject.text = event.source.display_name;
								
								hideAchivs();
							};
							
							animationEnd.addEventListener('complete',animationHandler);
							ui.placeListView.animate(animationEnd);
						});
					}
				}
			}
			
			ui.transparentView.hide();
			
			if(placeListHeight > 400)
			{
				ui.placeList.height = 440;
			}
			else
			{
				ui.placeList.height = placeListHeight;
			}
			
			ui.placeListView.animate(animation);
			
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
	for(var i = 0, I = achievements.length; i < I; i++)
	{
		var achievement = achievements[i];
		
		categories.push(
			{
				api_name: achievement.api_name,
				display_name: achievement.display_name
			}
		);
		
		for(var j = 0, J = achievement.projects.length; j < J; j++)
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
	ui.counter.text = counter;
	createListAchivs(window,"null");
}
function preViewBonus(type)
{
	var iconUrl = "", bgUrl = "";
	
	switch(type)
	{
		case "discount":
			iconUrl = "images/icons/Percents.png";
			bgUrl = "images/bg/Bonus.Discount.List.png";
			break;
		case "present":
			iconUrl = "images/icons/Present.png";
			bgUrl = "images/bg/Bonus.Present.List.png";
			break;
		case "bonus":
			iconUrl = "images/icons/Clover.png";
			bgUrl = "images/bg/Bonus.Bonus.List.png";
			break;
		default:
			return null;
	}
	
	var bonus = TiTools.UI.Controls.createView({
		top: 10,
		right: 0,
		height: 23,
		width: Ti.UI.SIZE,
		backgroundImage: bgUrl
	});
	
	var icon = TiTools.UI.Controls.createImageView({
		image: iconUrl,
		width: 23,
		height: 23
	});
	
	bonus.add(icon);
	
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
function createListRow(category,massRow)
{
	var row = TiTools.UI.Loader.load("Views/list.js", ui.placeList);
	massRow.push(row);
	
	row.rowTextAchivs.text = category.display_name;
	row.rowAchivs.api_name = category.api_name;
	row.rowAchivs.display_name = category.display_name;
						
	row.rowAchivs.addEventListener("singletap",function(event)
	{
		ui.nameProject.text = "Подкатегории";
		selectProject = "null";
		
		actIndicator(true);
		
		var animationHandler = function() {
			animationEnd.removeEventListener('complete',animationHandler);
		
			selectCategory = event.source.api_name;
			typeProject = event.source.api_name;
			ui.typeProject.text = event.source.display_name;
			
			ui.typeProject.show();
			ui.nameProject.show();
			ui.list.visible = false;
			// for(var ii = 0; ii < massRow.length; ii++)
			// {
				// massRow[ii].rowAchivs.superview.remove(massRow[ii].rowAchivs);
			// }
			
			ui.placeListView.animate(animationEnd);
			Ti.API.info('++');
			//delList(window,typeProject);
			
			hideAchivs();
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
		
		subCategoryClick = false;
		
		ui.transparentView.hide();
		
		ui.list.visible = false;
		for(var ii = 0, length = massRow.length; ii < length; ii++)
		{
			massRow[ii].rowAchivs.superview.remove(massRow[ii].rowAchivs);
		}
		
		ui.placeListView.animate(animationEnd);
	};
	
	animationEnd.addEventListener('complete',animationHandler);
	
	ui.placeListView.animate(animationEnd);
}
function hideAchivs()
{
	ui.preAchivs.hide();
	
	if(selectProject == "last")
	{
		for(var i = 0, I = tempAchivs.length; i < I; i++)
		{
			tempAchivs[i].viewAchivs.height = 0;
		}
		for(var j = 0, J = lastAchivs.length; j < J && j < 10; j++)
		{
			for(var i = 0, I = tempAchivs.length; i < I;  i++)
			{
				if(tempAchivs[i].api_name == lastAchivs[j].api_name)
				{
					tempAchivs[i].viewAchivs.height = Ti.UI.SIZE;
				}
			}
			
			if(j+1 == tempAchivs.length || j+1 == 10)
			{
				ui.preAchivs.updateLayout({contentHeight: Ti.UI.SIZE});
				ui.preAchivs.show();
				actIndicator(false);
			}
		}
	}
	else
	{
		var heigthScroll = 0;
		
		for(var i = 0, I = tempAchivs.length; i < I; i++)
		{
			if(tempAchivs[i].category == selectCategory || selectCategory == "null")
			{
				if(tempAchivs[i].project == selectProject || selectProject == "null")
				{
					tempAchivs[i].viewAchivs.height = Ti.UI.SIZE;
					heigthScroll++;
				}
				else
				{
					tempAchivs[i].viewAchivs.height = 0;
				}
			}
			else
			{
				tempAchivs[i].viewAchivs.height = 0;
			}
			if(i+1 == tempAchivs.length)
			{
				heigthScroll++;
				ui.preAchivs.updateLayout({contentHeight: Ti.UI.SIZE});
				
				ui.preAchivs.show();
				actIndicator(false);
			}
		}
	}
}
//--reload -- добавляем новую ачивку ---------------------------------------------------//
function reloadAdd(data)
{
	var categoryDetected = false;
	var projectDetected = false;
	
	for(var i = 0, I = categories.length; i < I; i++)
	{
		if(categories[i].api_name == data.api_name)
		{
			categoryDetected = true;
			break;
		}
		if(i+1 == categories.length)
		{
			categories.push({
				api_name: data.api_name,
				display_name: data.api_name
			});
		}
	}
	
	for(var j = 0, J = projects.length; j < J; j++)
	{
		if(projects[j].api_name == data.project.api_name)
		{
			projectDetected = true;
			break;
		}
		if(i+1 == projects.length)
		{
			projects.push({
				api_name: data.api_name,
				display_name: data.api_name,
				total_badge: 10
			});
		}
	}
	
	// --- поиск места куда вставить ачивку ----////
	
	// for(var i = 0; i < achievements.length; i++)
	// {
		// if(achievements[i].api_name)
		// {
			// for(var j = 0; j < achievements[i].projects.length; j++)
			// {
				// for(var k = 0; k < achievements[i].projects[j].achievements.length; k++)
				// {
					// if(achievements[i].projects[j].api_name == categiry || categiry == "null" || achievements[i].api_name == categiry)
					// {
					// }
				// }
			// }
		// }
	// }
	
	//// ------ создание  ачивки -----////
	var row = TiTools.UI.Loader.load("Views/ViewAchivs.js", ui.preAchivs);
	
	var achievement = data;
	
	//row.date.text = TiTools.DateTime.format(new Date(achievement.create_time), "$dd.$mm.$yyyy");
	
	row.image.image = achievement.pic;
	row.name.text = achievement.display_name;
	row.name.color = achievement.project.color;
	row.desc.text = achievement.desc;
	row.category = achievement.api_name;
	row.project = achievement.project.api_name;
	
	tempAchivs.push(row);
	
	for(var n = 0, N = achievement.bonuses.length; n < N; n++)
	{
		var bonus = preViewBonus(achievement.bonuses[n].bonus_type);
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
		if(singlTap == false)
		{
			singlTap = true;
			
			var sourceData = event.source.data;
			var win = TiTools.UI.Controls.createWindow({
				main: "Controllers/preViewAchivs.js",
				navBarHidden: true,
				nameAchivs: sourceData.nameAchivs,
				desc: sourceData.desc,
				details: sourceData.details,
				adv: sourceData.adv,
				image: sourceData.image,
				bonus: sourceData.bonus
			});
			
			win.addEventListener("close",function() {
				singlTap = false;
			});
			
			win.initialize();
			win.open();	
		}
	});
	///------------------------------/////
	
}
///--- 10 последних ачивок --- ///
function lastAchivsFunction(massRow)
{
	var row = TiTools.UI.Loader.load("Views/list.js", ui.placeList);
	massRow.push(row);
	
	row.rowTextAchivs.text = "Последние";
	row.rowAchivs.api_name = "last";
	row.rowAchivs.display_name = "Последние";
	
	row.rowAchivs.addEventListener("singletap",function(event)
	{
		selectProject = event.source.api_name;
		
		actIndicator(true);
		
		var animationHandler = function() {
			animationEnd.removeEventListener('complete',animationHandler);
			
			ui.transparentView.hide();
			
			ui.typeProject.show();
			ui.nameProject.show();
			
			ui.list.visible = false;
			
			ui.nameProject.text = event.source.display_name;
			
			hideAchivs();
		};
		
		animationEnd.addEventListener('complete',animationHandler);
		ui.placeListView.animate(animationEnd);
	});
	
}
//--------------------------------------------------------------------------------------//
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