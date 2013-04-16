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
var oldTime = undefined;
var tempNewAchivs = undefined;
var selectCategory = "null";
var selectProject = "null";
var subCategoryClick = false;
var newAchivsSсhema = [];
var placeListHeight = 0;
var emptyBlock;

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
	oldTime = window.oldTime;
	
	Ti.API.info('oldTime ' +  oldTime);
	
	// Загрузка контента окна
	ui = TiTools.UI.Loader.load("Views/GeneralWindow.js", window);
	itsbeta = require("Utils/Itsbeta");
	
	ui.counter.text = counter;
	
	//Ti.API.info(achievements);
	
	ui.typeProjectClick.addEventListener("click",function(event)
	{
		ui.typeProjectClick.backgroundImage = TiTools.Filesystem.preprocessPath("%ResourcesPath%images/others/Categories_press.png");
		
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
					display_name: L("label_all"),
					api_name: "null",
				};
				//--- делаем первую ячейку "все категории"
				createListRow(allRow, massRow);
				
				for(var i = 0; i < categories.length; i++)
				{
					category = categories[i];
					//--строю одну ячейку--
					createListRow(category, massRow, i);
					//---------------------
				}
				ui.list.visible = true;
				
				if(selectCategory != "null")
				{
					for(var n = 0, N = massRow.length; n < N; n++)
					{
						if(selectCategory == massRow[n].rowAchivs.api_name)
						{
							ui.placeList.add(massRow[n].rowAchivs);
							break;
						}
					}
					for(var n = 0, N = massRow.length; n < N; n++)
					{
						if(selectCategory != massRow[n].rowAchivs.api_name)
						{
							ui.placeList.add(massRow[n].rowAchivs);
						}
					}
				}
				
				if(placeListHeight >  Ti.Platform.displayCaps.platformHeight - 55)
				{
					ui.placeList.height = Ti.Platform.displayCaps.platformHeight - 75;
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
	
	ui.nameProjectClick.addEventListener("click", function(event)
	{
		if(subCategoryClick == false)
		{
			ui.nameProjectClick.backgroundImage = TiTools.Filesystem.preprocessPath("%ResourcesPath%images/others/Categories_press.png");
			
			subCategoryClick = true;
			
			ui.transparentView.show();
			ui.placeListViewCancel.show();
			createListName(window, typeProject);
		}
	});
	
	// add code
	var tempDecorateClickAdd = false;
	decorateButtonChildren.call(
		ui.addClick,
		function() {
			if(tempDecorateClickAdd == false)
			{
				tempDecorateClickAdd = true;
				var winAdd = TiTools.UI.Controls.createWindow({
					main: "Controllers/add.js",
					navBarHidden: true,
					achievements: achievements
				});
				winAdd.initialize();
				winAdd.addEventListener("close", function(event) {
					tempDecorateClickAdd = false;
				});
				winAdd.open();
			}
		}
	);
	// profile
	var tempDecorateClickProfile = false;
	decorateButtonChildren.call(
		ui.profileClick,
		function() {
			if(tempDecorateClickProfile == false)
			{
				tempDecorateClickProfile = true;
				var winAdd = TiTools.UI.Controls.createWindow({
					main: "Controllers/Profile.js",
					backgroundColor: "#fff",
					navBarHidden: true,
					info: info,
					achievements: achievements,
					counter: counter
				});
				winAdd.initialize();
				winAdd.addEventListener("close", function(event) {
					tempDecorateClickProfile = false;
				});
				winAdd.open();	
			}
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
		if(offset < -80.0 && !pulling && !reloading) {
			pulling = true;
			pullToRefresh.status.text = L("label_release_to_refresh");
		}
		else if((offset > -80.0 && offset < 0) && pulling && !reloading) {
			pulling = false;
			pullToRefresh.status.text = L("label_pull_to_refresh");
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
			//pullToRefresh.status.text = "Refreshing";
			pullToRefresh.status.hide();
			pullToRefresh.refreshing.show();
			beginReloading();
		}
	});
	
	//---------------------------------------------//
	
	function beginReloading() {			
		setTimeout(endReloading, 5000);
		Ti.App.fireEvent("pull");
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
		pullToRefresh.status.text = L("label_pull_to_refresh");
		pullToRefresh.refreshing.hide();
		pullToRefresh.status.show();
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
		//ui.preAchivs.hide();
		actIndicator(true);
		tempNewAchivs = event.data;
		
		// -- не дописана, оптимизаия ----////
		//reloadAdd(tempNewAchivs);
		
		// for(var i = 0; i < tempAchivs.length; i++)
		// {
			// tempAchivs[i].viewAchivs.superview.remove(tempAchivs[i].viewAchivs);
		// }
		for(var i = ui.preAchivs.children.length; i > 1; i--)
			{
				ui.preAchivs.remove(ui.preAchivs.children[i-1]);
			}
		tempAchivs = [];
		//-----------------
		// get achievements by user id
		
		itsbeta.getAchievementsByUid(info.fbuid, reSaveAchivs);
		
	});
	
	Ti.App.addEventListener("pull",function(event){
		
		itsbeta.getAchievementsRefresh(info.fbuid, insertPull ,oldTime);
		
	});
	
	createListAchivs(window,"null");
}
///-----сосдание списка ачивок-----//
function createListAchivs(window,categiry)
{
	ui.preAchivs.hide();
	actIndicator(true);
	
	ui.nameProject.text = L("label_subcategories");
	ui.typeProject.text = L("label_categories");
	
	for(var i = 0, K = achievements.length; i < K; i++)
	{
		if(achievements[i].projectsApiName == categiry || categiry == "null" || achievements[i].categoryApiName == categiry)
		{
			var achievement = achievements[i];
			
			// ---- список имен ачивок и время их выдачи --- //
			lastAchivs.push({
				date: achievement.create_time,
				api_name: achievement.achievApiName
			});
			// ---------------------------------------------//
			
			var row = TiTools.UI.Loader.load("Views/ViewAchivs.js", ui.preAchivs);
			
			row.date.text = TiTools.DateTime.format(new Date(achievement.create_time), "$dd.$mm.$yyyy");
			row.api_name = achievement.achievApiName,
			row.image.image = achievement.achievPic;
			row.name.text = achievement.achievDisplayName;
			row.name.color = achievement.color;
			row.desc.text = achievement.achievDesc;
			row.category = achievement.categoryApiName;
			row.project = achievement.projectsApiName;
			
			tempAchivs.push(row);
			
			for(var n = 0, N = achievement.achievBonuses.length; n < N; n++)
			{
				var bonus = preViewBonus(achievement.achievBonuses[n].bonus_type);
				row.addBonus.add(bonus);
			}
			
			row.viewAchivs.data = {
				image: achievement.achievPic,
				nameAchivs: achievement.achievDisplayName,
				desc: achievement.achievDesc,
				details: achievement.achievDetails,
				adv: achievement.achievAdv,
				bonus: achievement.achievBonuses
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
	
	emptyBlock = TiTools.UI.Controls.createView({
		height: 0
	});
	ui.preAchivs.add(emptyBlock);
	
	updateEmptyBlockHeight();
	
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
}

// если есть новые ачивки после обновления струкрурируем их и пересоздаем скрол
 
function insertPull(data)
{
	//Ti.API.info( JSON.parse(data.responseText));
	achievements = JSON.parse(data.responseText)
	var tempNewAchivs = [];
	
	for(var i = 0, I = achievements.length; i < I ; i++)
	{
		var achivs = achievements[i];
		
		categories.push({
			api_name: achivs.api_name,
			display_name: achivs.display_name
		});
		
		for(var j = 0, J = achivs.projects.length; j < J; j++)
		{
			var achivsProject = achivs.projects[j];
			
			counter += achivsProject.achievements.length;
			
			projects.push({
				api_name: achivsProject.api_name,
				display_name: achivsProject.display_name,
				total_badge: achivsProject.total_badge
			});
			
			for(var k = 0, K = achivsProject.achievements.length; k < K; k++)
			{
				var achivsProjectAchiv = achivsProject.achievements[k];
				
				tempNewAchivs.push({
					categoryApiName: achivs.api_name,
					categoryDisplayName: achivs.display_name,
					projectsApiName: achivsProject.api_name,
					projectsDisplayName: achivsProject.display_name,
					color: achivsProject.color,
					total_badges: achivsProject.total_badges,
					achievApiName: achivsProjectAchiv.api_name,
					create_time: achivsProjectAchiv.create_time,
					achievDisplayName: achivsProjectAchiv.display_name,
					achievBadgeName: achivsProjectAchiv.badge_name,
					achievDesc: achivsProjectAchiv.desc,
					achievDetails: achivsProjectAchiv.details,
					achievAdv: achivsProjectAchiv.adv,
					achievPic: achivsProjectAchiv.pic,
					fb_id : achivsProjectAchiv.fb_id,
					achievBonuses: achivsProjectAchiv.bonuses,
				});
			}
		}
		if(i+1 == I)
		{
			if(tempNewAchivs.length > 1)
			{
				//alert("ЕСТЬ НОВЫЕ АЧИВКИ!");
				ui.preAchivs.hide();
				actIndicator(true);
				
				for(var i = ui.preAchivs.children.length; i > 1; i--)
				{
					ui.preAchivs.remove(ui.preAchivs.children[i-1]);
				}
				
				tempAchivs 	= [];
				
				itsbeta.getAchievementsByUid(info.fbuid, reSaveAchivs);
			}
			else
			{
				alert("нет новых ачивок");
			}
		}
		
	}
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
			
			var tempListProject = [];
			
			for(var i = 0, I = achievements.length; i < I; i++)
			{
				var flagProject = false;
				
				if(achievements[i].categoryApiName == category || category == "null")
				{
					for(var j = 0; j < tempListProject.length; j++)
					{
						if(achievements[i].projectsApiName == tempListProject[j])
						{
							flagProject = true;
							break;
						}
					}
					
					if(flagProject == true)
					{
						continue;
					}
					
					tempListProject.push(achievements[i].projectsApiName);
					
					if(selectProject == "null")
					{
						var row = TiTools.UI.Loader.load("Views/list.js", ui.placeList);
						massRow.push(row);
					}
					else
					{
						var row = TiTools.UI.Loader.load("Views/list.js");
						massRow.push(row);
					}
					
					placeListHeight += 50;
					
					if(selectProject == achievements[i].projectsApiName)
					{
						ui.placeList.lastRow = row.rowAchivs;
						row.rowAchivs.backgroundImage = null;
						row.rowAchivs.backgroundColor = "#7ed6f9";
					}
					row.rowTextAchivs.text = achievements[i].projectsDisplayName;
					row.rowAchivs.api_name = achievements[i].projectsApiName;
					row.rowAchivs.display_name = achievements[i].projectsDisplayName;
					
					row.rowAchivs.addEventListener("singletap",function(event)
					{
						if(ui.placeList.lastRow != undefined)
						{
							ui.placeList.lastRow.backgroundImage = TiTools.Filesystem.preprocessPath("%ResourcesPath%images/navbar/Selects.Bg.png");
						}
						event.source.backgroundImage = null;
						event.source.backgroundColor = "#7ed6f9";
						
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
							
							for(var i = 0, I = ui.placeList.children.length; I > 0; I--)
							{
								ui.placeList.remove(ui.placeList.children[I-1]);
							}
							
							hideAchivs();
						};
						
						animationEnd.addEventListener('complete',animationHandler);
						ui.placeListView.animate(animationEnd);
					});
				}
			}
			
			if(selectProject != "null")
			{
				for(var n = 0, N = massRow.length; n < N; n++)
				{
					if(selectProject == massRow[n].rowAchivs.api_name)
					{
						ui.placeList.add(massRow[n].rowAchivs);
						break;
					}
				}
				for(var n = 0, N = massRow.length; n < N; n++)
				{
					if(selectProject != massRow[n].rowAchivs.api_name)
					{
						ui.placeList.add(massRow[n].rowAchivs);
					}
				}
			}
			
			ui.transparentView.hide();
			
			if(placeListHeight >  Ti.Platform.displayCaps.platformHeight - 55)
			{
				ui.placeList.height = Ti.Platform.displayCaps.platformHeight - 75;
			}
			else
			{
				ui.placeList.height = placeListHeight;
			}
			
			Ti.API.info(ui.placeList.height);
			
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
	counter = 0;
	
	newAchivsSсhema = [];
	
	for(var i = 0, I = achievements.length; i < I ; i++)
	{
		var achivs = achievements[i];
		
		categories.push({
			api_name: achivs.api_name,
			display_name: achivs.display_name
		});
		
		for(var j = 0, J = achivs.projects.length; j < J; j++)
		{
			var achivsProject = achivs.projects[j];
			
			counter += achivsProject.achievements.length;
			
			projects.push({
				api_name: achivsProject.api_name,
				display_name: achivsProject.display_name,
				total_badge: achivsProject.total_badge
			});
			
			for(var k = 0, K = achivsProject.achievements.length; k < K; k++)
			{
				var achivsProjectAchiv = achivsProject.achievements[k];
				
				newAchivsSсhema.push({
					categoryApiName: achivs.api_name,
					categoryDisplayName: achivs.display_name,
					projectsApiName: achivsProject.api_name,
					projectsDisplayName: achivsProject.display_name,
					color: achivsProject.color,
					total_badges: achivsProject.total_badges,
					achievApiName: achivsProjectAchiv.api_name,
					create_time: achivsProjectAchiv.create_time,
					achievDisplayName: achivsProjectAchiv.display_name,
					achievBadgeName: achivsProjectAchiv.badge_name,
					achievDesc: achivsProjectAchiv.desc,
					achievDetails: achivsProjectAchiv.details,
					achievAdv: achivsProjectAchiv.adv,
					achievPic: achivsProjectAchiv.pic,
					fb_id : achivsProjectAchiv.fb_id,
					achievBonuses: achivsProjectAchiv.bonuses,
				});
			}
		}
	}
	
	newAchivsSсhema.sort(
		function(a, b)
		{
			var date1 = Date();
			var date2 = Date();
			
			date1 = a.create_time;
			date2 = b.create_time;
			
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
	//Ti.API.info(newAchivsSсhema);
	
	
	ui.counter.text = counter;
	achievements = newAchivsSсhema;
	
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
// update empty block height	
function updateEmptyBlockHeight()
{
	var totalBlocksHeight = 0;
		badges 			  = ui.preAchivs.children;
	
	for(var i=0, len=badges.length; i<len-1; i++) {
		var badge = badges[i];
		if(badge.height !== 0) {
			totalBlocksHeight += badge.toImage().height;
		}
	}
	
	var emptyBlockHeight = Ti.Platform.displayCaps.platformHeight - totalBlocksHeight - 20;
	
	if(emptyBlockHeight > 0) {
		emptyBlock.updateLayout({
			height: emptyBlockHeight
		});
	} else {
		emptyBlock.updateLayout({
			height: 0
		});
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
function createListRow(category,massRow)
{
	if(selectCategory == "null")
	{
		var row = TiTools.UI.Loader.load("Views/list.js", ui.placeList);
		massRow.push(row);
	}
	else
	{
		var row = TiTools.UI.Loader.load("Views/list.js");
		massRow.push(row);
	}
	
	placeListHeight += 50;
	
	if(selectCategory == category.api_name)
	{
		ui.placeList.lastRow = row.rowAchivs;
		row.rowAchivs.backgroundImage = null;
		row.rowAchivs.backgroundColor = "#7ed6f9";
	}
	
	row.rowTextAchivs.text = category.display_name;
	row.rowAchivs.api_name = category.api_name;
	row.rowAchivs.display_name = category.display_name;
						
	row.rowAchivs.addEventListener("singletap",function(event)
	{
		if(ui.placeList.lastRow != undefined)
		{
			ui.placeList.lastRow.backgroundImage = TiTools.Filesystem.preprocessPath("%ResourcesPath%images/navbar/Selects.Bg.png");
		}
		event.source.backgroundImage = null;
		event.source.backgroundColor = "#7ed6f9";
		
		ui.typeProjectClick.backgroundImage = null;
		
		ui.nameProject.text = L("label_subcategories");
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
			
			ui.placeListView.animate(animationEnd);
			Ti.API.info('++');
			
			for(var i = ui.placeList.children.length; i > 0; i--)
			{
				ui.placeList.remove(ui.placeList.children[i-1]);
			}
			
			hideAchivs();
		};
		
		animationEnd.addEventListener('complete',animationHandler);
		ui.placeListView.animate(animationEnd);
	});
}
function undefClick()
{	
	ui.typeProjectClick.backgroundImage = null;
	ui.nameProjectClick.backgroundImage = null;
	ui.placeListViewCancel.hide();
		
	var animationHandler = function() {
		animationEnd.removeEventListener('complete',animationHandler);
		
		subCategoryClick = false;
		
		ui.transparentView.hide();
		
		ui.list.visible = false;
		// for(var ii = 0, length = massRow.length; ii < length; ii++)
		// {
			// massRow[ii].rowAchivs.superview.remove(massRow[ii].rowAchivs);
		// }
		
		for(var i = ui.placeList.children.length; i > 0; i--)
		{
			ui.placeList.remove(ui.placeList.children[i-1]);
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
	updateEmptyBlockHeight();
}
//--reload -- добавляем новую ачивку ---------------------------------------------------//
// function reloadAdd(data)
// {
	// var categoryDetected = false;
	// var projectDetected = false;
// 	
	// for(var i = 0, I = categories.length; i < I; i++)
	// {
		// if(categories[i].api_name == data.api_name)
		// {
			// categoryDetected = true;
			// break;
		// }
		// if(i+1 == categories.length)
		// {
			// categories.push({
				// api_name: data.api_name,
				// display_name: data.api_name
			// });
		// }
	// }
// 	
	// for(var j = 0, J = projects.length; j < J; j++)
	// {
		// if(projects[j].api_name == data.project.api_name)
		// {
			// projectDetected = true;
			// break;
		// }
		// if(i+1 == projects.length)
		// {
			// projects.push({
				// api_name: data.api_name,
				// display_name: data.api_name,
				// total_badge: 10
			// });
		// }
	// }
// 	
	// // --- поиск места куда вставить ачивку ----////
// 	
	// // for(var i = 0; i < achievements.length; i++)
	// // {
		// // if(achievements[i].api_name)
		// // {
			// // for(var j = 0; j < achievements[i].projects.length; j++)
			// // {
				// // for(var k = 0; k < achievements[i].projects[j].achievements.length; k++)
				// // {
					// // if(achievements[i].projects[j].api_name == categiry || categiry == "null" || achievements[i].api_name == categiry)
					// // {
					// // }
				// // }
			// // }
		// // }
	// // }
// 	
	// //// ------ создание  ачивки -----////
	// var row = TiTools.UI.Loader.load("Views/ViewAchivs.js", ui.preAchivs);
// 	
	// var achievement = data;
// 	
	// //row.date.text = TiTools.DateTime.format(new Date(achievement.create_time), "$dd.$mm.$yyyy");
// 	
	// row.image.image = achievement.pic;
	// row.name.text = achievement.display_name;
	// row.name.color = achievement.project.color;
	// row.desc.text = achievement.desc;
	// row.category = achievement.api_name;
	// row.project = achievement.project.api_name;
// 	
	// tempAchivs.push(row);
// 	
	// for(var n = 0, N = achievement.bonuses.length; n < N; n++)
	// {
		// var bonus = preViewBonus(achievement.bonuses[n].bonus_type);
		// row.addBonus.add(bonus);
	// }
// 	
	// row.viewAchivs.data = {
		// image: achievement.pic,
		// nameAchivs: achievement.display_name,
		// desc: achievement.desc,
		// details: achievement.details,
		// adv: achievement.adv,
		// bonus: achievement.bonuses
	// }
// 	
	// row.viewAchivs.addEventListener("singletap",function(event)
	// {
		// if(singlTap == false)
		// {
			// singlTap = true;
// 			
			// var sourceData = event.source.data;
			// var win = TiTools.UI.Controls.createWindow({
				// main: "Controllers/preViewAchivs.js",
				// navBarHidden: true,
				// nameAchivs: sourceData.nameAchivs,
				// desc: sourceData.desc,
				// details: sourceData.details,
				// adv: sourceData.adv,
				// image: sourceData.image,
				// bonus: sourceData.bonus
			// });
// 			
			// win.addEventListener("close",function() {
				// singlTap = false;
			// });
// 			
			// win.initialize();
			// win.open();	
		// }
	// });
	// ///------------------------------/////
// 	
// }
///--- 10 последних ачивок --- ///
function lastAchivsFunction(massRow)
{
	if(selectProject == "null")
	{
		var row = TiTools.UI.Loader.load("Views/list.js", ui.placeList);
		massRow.push(row);
	}
	else
	{
		var row = TiTools.UI.Loader.load("Views/list.js");
		massRow.push(row);
	}
	
	row.rowTextAchivs.text = L("label_last");
	row.rowAchivs.api_name = "last";
	row.rowAchivs.display_name =  L("label_last");
	
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