/**
 * @author Gom_Dzhabbar
 */

var TiTools = undefined;
var ui = undefined;
var itsbeta = undefined;
//---------------------------------------------//
// Глобальные переменные для окна
//---------------------------------------------//


//---------------------------------------------//

Ti.include("Utils/Helper.js")

//---------------------------------------------//
// Обязательные функции
//---------------------------------------------//

// Инициализация контроллера окна
function onInitController(window, params)
{
	TiTools = require("TiTools/TiTools");
	itsbeta = require("Utils/Itsbeta");
	
	// Загрузка контента окна
	ui = TiTools.UI.Loader.load("Views/add.js", window);
	
	// back
	decorateNavbarButton.call(
		ui.back, 
		function() {
			window.close();
		}
	);
	
	// code
	decorateButton.call(
		ui.code, 
		function() {
			var winCode = TiTools.UI.Controls.createWindow({
				main: "Controllers/addCode.js",
				navBarHidden: true,
			});
			winCode.initialize();
			winCode.open({modal: true});
		}
	);
	
	//---------------------------------------------//
	// QR code scanner
	//---------------------------------------------//
	
	var navbar = TiTools.UI.Loader.load("Views/addQr.js");
	
	// cancel
	decorateNavbarButton.call(
		navbar.cancel, 
		function() {
			scanWin.close();
		}
	);
	
	// scanner window
	var scanWin = Titanium.UI.createWindow({
		backgroundColor: "#fff",	
		navBarHidden: true
	});
	
	scanWin.addEventListener("open", function() {
		initScanner.call(scanner);
		scanner.startScanning(); 
	});
	scanWin.addEventListener("close", function() {
		//scanner.stopScanning();
		scanner.reset();
	});
	scanWin.add(navbar.me);
	scanWin.add(TiTools.UI.Controls.createLabel({
		color: "#625f5e",
		font: {fontSize: 20},
		textid: "label_add_load"
	}));

	// load the Scandit SDK module
	var scanditsdk = require("com.mirasense.scanditsdk"); 
	
	// instantiate the Scandit SDK
	var scanner = scanditsdk.createView({
 		width: "100%",
		height: "100%"
	}); 
	
	scanWin.add(scanner);
	
	function initScanner()
	{
		// Initialize the barcode scanner,
		this.init("18nHJpekEeKPUvet3JgAORWwUAXBuwUofxN/r9V85Co", 0); 
		
		// Set callback functions for when scanning succeeds and for when the
		// scanning is canceled.
		this.setSuccessCallback(function(e) {
			scanWin.close();			
			actIndicator(true);
			itsbeta.postActiv(e.barcode);
		});
		
		// options
		this.setInfoBannerOffset(-300); 
		this.set1DScanningEnabled(false);
		this.set2DScanningEnabled(false);
		this.setEan13AndUpc12Enabled(false); 
		this.setEan8Enabled(false); 
		this.setUpceEnabled(false);
		this.setCode39Enabled(false);
		this.setCode128Enabled(false); 
		this.setItfEnabled(false); 
		this.setQrEnabled(true); 
		this.setDataMatrixEnabled(false);
		this.setInverseDetectionEnabled(false);
	}
	
	// qr handler
	decorateButton.call(
		ui.qr, 
		function() {
			scanWin.open({modal: true}); 
		}
	);
}
//---------------------------------------------//
// Функции лентяйки
//---------------------------------------------//

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

// Обработчик при открытии окна
function onWindowOpen(window, event)
{
	Ti.App.addEventListener("actHide",function(event)
	{
		actIndicator(false);
	});
	
	Ti.App.addEventListener("CloseAdd",function(event)
		{
			window.close();
		}
	);
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