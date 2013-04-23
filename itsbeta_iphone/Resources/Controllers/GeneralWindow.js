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

var table = undefined;
var tableData = [];
var rowPull = undefined;

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
	
	tableData = [];
	
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
	
	// сделали табличку ----------------------------------------//
	
	table = Ti.UI.createTableView({
		backgroundColor: "#f7f7f7",
		separatorStyle: Titanium.UI.iPhone.TableViewSeparatorStyle.NONE
	});
	ui.preAchivs.add(table);
	
	//---------------------------------------------------------//
	
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
	
	// // ----- PULL TO REFRESH ----- //
// 	
	// var achivsWrapper = ui.preAchivs,
		// pullToRefresh = TiTools.UI.Loader.load('Views/PullToRefresh.js'),
		// pulling       = false,
		// reloading     = false,
		// offset        = 0;
// 		
	// achivsWrapper.add(pullToRefresh.me);
	// pullToRefresh.hypno.start(); 
// 		
	// // event handlers
	// achivsWrapper.addEventListener('scroll', function(e) {
		// offset = e.y;
		// if(offset < -80.0 && !pulling && !reloading) {
			// pulling = true;
			// pullToRefresh.status.text = L("label_release_to_refresh");
		// }
		// else if((offset > -80.0 && offset < 0) && pulling && !reloading) {
			// pulling = false;
			// pullToRefresh.status.text = L("label_pull_to_refresh");
		// }
	// });
// 	
	// achivsWrapper.addEventListener('dragEnd', function() {	
		// if(pulling && !reloading) {
			// achivsWrapper.top = Math.abs(offset)+6;
			// achivsWrapper.animate({
					// top: 86,
					// duration: 200
				// }, 
				// function() {
			  		// achivsWrapper.top = 86;
				// }
			// );
			// reloading = true;
			// pulling = false;
			// //pullToRefresh.status.text = "Refreshing";
			// pullToRefresh.status.hide();
			// pullToRefresh.refreshing.show();
			// beginReloading();
		// }
	// });
// 	
	// //---------------------------------------------//
	
	// function beginReloading() {			
		// setTimeout(endReloading, 5000);
		// Ti.App.fireEvent("pull");
	// }
// 	
	// function endReloading() {			
		// achivsWrapper.animate({
				// top: 6
			// }, 
			// function() {
				// achivsWrapper.top = 6;
			// }
		// );
// 		
		// reloading = false;
		// pullToRefresh.status.text = L("label_pull_to_refresh");
		// pullToRefresh.refreshing.hide();
		// pullToRefresh.status.show();
	// }
	
	// ----- END PULL TO REFRESH ----- //
}
//---------------------------------------------//
// Функции лентяйки
//---------------------------------------------//

// Обработчик при открытии окна
function onWindowOpen(window, event)
{
	//-----создаем ячейку для пула рефреша -------///
	rowPull  = Ti.UI.createView({
		height: 200,
		width: Ti.UI.FILL,
		backgroundColor: "#ededed",
	});
	
	rowPull2  = Ti.UI.createView({
		bottom: 0,
		height: 70,
		width: Ti.UI.FILL,
		backgroundColor: "blue",
	});
	
	rowPull.add(rowPull2);
	
	table.headerPullView = rowPull;
	
	var pulling = false;
	var reloading = false;
	var offset = 0;
	var pullToRefresh = TiTools.UI.Loader.load('Views/PullToRefresh.js');
	
	rowPull2.add(pullToRefresh.me);
	
	pullToRefresh.hypno.start(); 
	
	table.addEventListener('scroll',function(e)
	{
		offset = e.contentOffset.y;
		if (offset <= -75.0 && !pulling)
		{
			pulling = true;
			 pullToRefresh.status.text = L("label_release_to_refresh");
		}
		else if (pulling && offset > -75.0 && offset < 0)
		{
			pulling = false;
			pullToRefresh.status.text = L("label_pull_to_refresh");
		}
	});
	table.addEventListener('dragEnd',function(e)
	{
		Ti.API.info('pulling ' + pulling);
		Ti.API.info('reloading ' + reloading);
		Ti.API.info('e.contentOffset.y ' + offset);
		
		if (pulling && !reloading && offset <= -75.0)
		{
			reloading = true;
			pulling = false;
			pullToRefresh.status.hide();
			table.setContentInsets({top:65},{animated:true});
			pullToRefresh.refreshing.show();
			beginReloading();
		}
	});
	
	function beginReloading()
	{
		// just mock out the reload
		setTimeout(endReloading,2000);
		itsbeta.getAchievementsRefresh(info.fbuid, insertPull ,oldTime);
	}
	 
	function endReloading()
	{
	 
		// when you're done, just reset
		table.setContentInsets({top:0},{animated:true});
		reloading = false;
		pullToRefresh.refreshing.hide();
		pullToRefresh.status.text = L("label_pull_to_refresh");
		pullToRefresh.status.show();
	}
	//--------------------------------------------///
	
	Ti.App.addEventListener("actHide",function(event)
	{
		actIndicator(false);
	});
	
	Ti.App.addEventListener("logout",function(event){
		Ti.App.removeEventListener("reload",reload);
		//Ti.App.removeEventListener("pull",pull);
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
	
	var reload = function(event)
	{
		ui.preAchivs.hide();
		actIndicator(true);
		tempNewAchivs = event.data;
		Ti.API.info(event.data);
		// поиск категории из нашего списка, если есть то пропускаем, иначе добавляем
		if(searchApiName(projects,event.data.project.api_name) == 0)
		{
			Ti.API.info('project ---')
			projects.push({
				api_name: event.data.project.api_name,
				display_name: event.data.project.display_name,
				total_badge: event.data.total_badge
			});
			
			if(searchApiName(categories,event.data.category.api_name) == 0)
			{
				Ti.API.info('categori ---')
				categories.push({
					api_name: event.data.category.api_name,
					display_name: event.data.category.display_name
				});
			}
		}
		
		var achievement = {
			categoryApiName: event.data.category.api_name,
			categoryDisplayName: event.data.category.display_name,
			projectsApiName: event.data.project.api_name,
			projectsDisplayName: event.data.project.display_name,
			color: event.data.project.color,
			total_badges: event.data.total_badges,
			achievApiName: event.data.api_name,
			create_time: event.data.create_time,
			achievDisplayName: event.data.display_name,
			achievBadgeName: event.data.api_name,
			achievDesc: event.data.desc,
			achievDetails: event.data.details,
			achievAdv: event.data.adv,
			achievPic: event.data.pic,
			fb_id : event.data.fb_id,
			achievBonuses: event.data.bonuses,
		};
		
		achievements.unshift(achievement);
		var row = createTableViewRow(achievement)
		
		table.insertRowBefore(0,row);
		
		tableData.unshift(row);
		
		counter++;
		ui.counter.text = counter;
		
		if(tempNewAchivs != undefined)
		{
			sleep(500);
			
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
			
			Ti.API.info('open');
			
			tempNewAchivs = undefined;
		}
		
		ui.preAchivs.show();
		actIndicator(false);
	}
	
	Ti.App.addEventListener("reload",reload);
	
	// var pull = function(event)
	// {
		// itsbeta.getAchievementsRefresh(info.fbuid, insertPull ,oldTime);
	// }
	
	//Ti.App.addEventListener("pull",pull);
	
	createListAchivs(window,"null");
}
///-----сосдание списка ачивок-----//
function createListAchivs(window,categiry)
{
	ui.preAchivs.hide();
	actIndicator(true);
	
	ui.nameProject.text = L("label_subcategories");
	ui.typeProject.text = L("label_categories");
	
	table.addEventListener("singletap",function(event)
	{
		if(event.index != undefined)
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
		}
	});
	
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
			// создаем ячейку и ложем в табличку
			tableData.push(createTableViewRow(achievement));
			
		}
		if(i+1 == achievements.length)
		{
			table.data = tableData;
			
			Ti.API.info('Закончили построение ачивок \n открывается окно полученой ачивки');
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
}
//---------------createTableViewRow-------------//
function createTableViewRow(achievement)
{
	var rowTemp = Ti.UI.createTableViewRow({
		className: 'row',
		objName: 'row',
		touchEnabled: true,
		height: Ti.UI.SIZE,
		selectedBackgroundColor: "transparent",
		achievement : achievement
	});
	
	var row = TiTools.UI.Loader.load("Views/ViewAchivs.js",rowTemp);
	
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
		bonus: achievement.achievBonuses,
		row : rowTemp
	};
	
	return rowTemp;
}
//----------------------------------------------//
// если есть новые ачивки после обновления струкрурируем их и пересоздаем скрол
 
function insertPull(data)
{
	Ti.API.info( "insertPull");
	var tempAchievements = JSON.parse(data.responseText)
	var tempNewAchivs = [];
	for(var i = 0, I = tempAchievements.length; i < I ; i++)
	{
		var achivs = tempAchievements[i];
		
		for(var j = 0, J = achivs.projects.length; j < J; j++)
		{
			var achivsProject = achivs.projects[j];
			
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
				Ti.API.info('есть новые ачивки');
				
				for(var n = 0, N = tempNewAchivs.length; n < N; n++)
				{
					if(searchApiName(projects,tempNewAchivs[n].projectsApiName) == 0)
					{
						Ti.API.info('project ---')
						projects.push({
							api_name: tempNewAchivs[n].projectsApiName,
							display_name: tempNewAchivs[n].projectsDisplayName,
							total_badge: tempNewAchivs[n].total_badges
						});
						
						if(searchApiName(categories,tempNewAchivs[n].categoryApiName) == 0)
						{
							Ti.API.info('categori ---')
							categories.push({
								api_name: tempNewAchivs[n].categoryApiName,
								display_name: tempNewAchivs[n].categoryDisplayName
							});
						}
					}
				}
				
				//---- сортируем ----//
				Ti.API.info('сортируем');
				tempNewAchivs.sort(
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
				
				oldTime = tempNewAchivs[0].create_time;
				
				for(var n = 0, N = tempNewAchivs.length - 2; n <= N; N--)
				{
					Ti.API.info('вставили ячейку ' + N);
					var row = createTableViewRow(tempNewAchivs[N]);
					
					tableData.unshift(row);
					achievements.unshift(tempNewAchivs[N]);
					
					table.insertRowBefore(0,row);
				}
				
			}
			else
			{
				Ti.API.info('нет новых очивок');
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
						ui.placeList.lastRow = row.rowTextAchivs;
						row.rowTextAchivs.color = "#7ed6f9";
					}
					row.rowTextAchivs.text = achievements[i].projectsDisplayName;
					row.rowAchivs.api_name = achievements[i].projectsApiName;
					row.rowAchivs.display_name = achievements[i].projectsDisplayName;
					row.rowAchivs.rowTextAchivs = row.rowTextAchivs;
					
					row.rowAchivs.addEventListener("singletap",function(event)
					{
						if(ui.placeList.lastRow != undefined)
						{
							ui.placeList.lastRow.color = "#646464";
						}
						event.source.rowTextAchivs.color = "#7ed6f9";
						
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
	Ti.API.info(JSON.parse(data.responseText));
	
	/*Ti.API.info('Выполняется удаление ачивок.')
	tempAchivs = [];
	Ti.API.info('ui.preAchivs.children.length = ' + ui.preAchivs.children.length );
	
	for(var ii = ui.preAchivs.children.length; ii > 1; ii--)
		{
			ui.preAchivs.remove(ui.preAchivs.children[ii-1]);
			
			if(ii-1 == 1)
			{
				Ti.API.info('Удаление закончилось \n Идет построение ачивок.')
				ii = 2;
				
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
				
				oldTime = newAchivsSсhema[0].create_time;
				ui.counter.text = counter;
				achievements = newAchivsSсhema;
				
				createListAchivs(window,"null");
			}
		}*/
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
		top: 5,
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
		ui.placeList.lastRow = row.rowTextAchivs;
		row.rowTextAchivs.color = "#7ed6f9";
	}
	
	row.rowTextAchivs.text = category.display_name;
	row.rowAchivs.api_name = category.api_name;
	row.rowAchivs.display_name = category.display_name;
	row.rowAchivs.rowTextAchivs = row.rowTextAchivs;
						
	row.rowAchivs.addEventListener("singletap",function(event)
	{
		if(ui.placeList.lastRow != undefined)
		{
			ui.placeList.lastRow.color = "#646464";
		}
		event.source.rowTextAchivs.color = "#7ed6f9";
		
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
	var tempTableData = tableData;
	var newData = [];
	
	if(selectProject == "last")
	{
		for(var i = 0, I = tempTableData.length; i < I && i < 10; i++)
		{
			newData.push(tempTableData[i]);
			
			if(i+1 == I || i + 1 == 10)
			{
				table.data = newData;
				table.scrollToIndex(0);
				actIndicator(false);
			}
		}
	}
	else
	{
		for(var i = 0, I = tempTableData.length; i < I; i++)
		{
			if(tempTableData[i].achievement.categoryApiName == selectCategory || selectCategory == "null")
			{
				if(tempTableData[i].achievement.projectsApiName == selectProject || selectProject == "null")
				{
					newData.push(tempTableData[i]);
				}
			}
			if(i+1 == I)
			{
				table.data = newData;
				table.scrollToIndex(0);
				actIndicator(false);
			}
		}
	}
	
}
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
function searchApiName(a,apiName)
{
	for(var i = 0, I = a.length;i < I; i++)
	{
		if(a[i].api_name == apiName)
		{
			return 1;
		}
		
		if(i+1 == I)
		{
			return 0;
		}
	}
}
function sleep(time) {
	var start = new Date().getTime();
	while (true) {
		var elapsed = new Date().getTime() - start;
		if (elapsed >= time) {
			break;
		}
	}
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