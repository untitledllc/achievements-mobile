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

	// load the Scandit SDK module
	var scanditsdk = require("com.mirasense.scanditsdk"); 
	
	// qr handler
	decorateButton.call(
		ui.qr, 
		function() {
			// instantiate the Scandit SDK
			var scanner = scanditsdk.createView({
		 		width: "100%",
				height: "100%"
			}); 
			
			// Initialize the barcode scanner,
			scanner.init("18nHJpekEeKPUvet3JgAORWwUAXBuwUofxN/r9V85Co", 0); 
			
			// Set callback functions for when scanning succeeds and for when the
			// scanning is canceled.
			scanner.setSuccessCallback(function(e) {
				// if(e.symbology === "Qr") {
					scanWin.close();			
					alert("success (" + e.symbology + "): " + e.barcode);	
				//}
			     // actIndicator(true);
				 // itsbeta.postActiv(e.result);
			});
			scanner.setCancelCallback(function(e) { 
				alert("canceled");
			}); 
			
			// options
			scanner.setInfoBannerOffset(-300); 
			scanner.set1DScanningEnabled(false);
			scanner.set2DScanningEnabled(false);
			scanner.setEan13AndUpc12Enabled(false); 
			scanner.setEan8Enabled(false); 
			scanner.setUpceEnabled(false);
			scanner.setCode39Enabled(false);
			scanner.setCode128Enabled(false); 
			scanner.setItfEnabled(false); 
			scanner.setQrEnabled(true); 
			scanner.setDataMatrixEnabled(false);
			scanner.setInverseDetectionEnabled(false);
			
			// scanner window
			var scanWin = Titanium.UI.createWindow({
				navBarHidden: true
			});
			scanWin.addEventListener("open", function() {
				scanner.startScanning(); 
			});
			scanWin.addEventListener("close", function() {
				//scanner.stopScanning();
			})
			
			var navbar = TiTools.UI.Loader.load("Views/addQr.js");
			
			// cancel
			decorateNavbarButton.call(
				navbar.cancel, 
				function() {
					scanWin.close();
				}
			);
			
			scanWin.add(navbar.me);
			scanWin.add(scanner);
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