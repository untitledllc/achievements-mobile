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
	var Barcode = require('ti.barcode');
	Barcode.allowRotation = true;
	Barcode.displayedMessage = '';
	Barcode.useLED = true;
	
	// var overlay = Ti.UI.createView({
	    // backgroundColor: 'red',
	    // width: "80%",
		// height: "80%",
	    // top: 20, right: 20, bottom: 20, left: 20
	// });
	
	var overlay = TiTools.UI.Loader.load("Views/addQr.js");
	
	// var toggleLEDButton = Ti.UI.createButton({
	    // title: Barcode.useLED ? 'LED is On' : 'LED is Off',
	    // textAlign: 'center',
	    // color: '#000', backgroundColor: '#fff', style: 0,
	    // font: { fontWeight: 'bold', fontSize: 16 },
	    // borderColor: '#000', borderRadius: 10, borderWidth: 1,
	    // opacity: 0.5,
	    // width: 220, height: 30,
	    // bottom: 10
	// });
	// toggleLEDButton.addEventListener('click', function () {
	    // Barcode.useLED = !Barcode.useLED;
	    // toggleLEDButton.title = Barcode.useLED ? 'LED is On' : 'LED is Off';
	// });
	// overlay.add(toggleLEDButton);
// 	
	// var cancelButton = Ti.UI.createButton({
	    // title: 'Cancel', textAlign: 'center',
	    // color: '#000', backgroundColor: '#fff', style: 0,
	    // font: { fontWeight: 'bold', fontSize: 16 },
	    // borderColor: '#000', borderRadius: 10, borderWidth: 1,
	    // opacity: 0.5,
	    // width: 220, height: 30,
	    // top: 20
	// });
	overlay.cancel.addEventListener('click', function () {
	    Barcode.cancel();
	});
	//overlay.add(cancelButton);
	
	// qr handler
	decorateButton.call(
		ui.qr, 
		function() {
			// Barcode.capture({
		        // animate: true,
		        // overlay: overlay.me,
		        // showCancel: false,
		        // showRectangle: false,
		        // keepOpen: true/*,
		        // acceptedFormats: [
		            // Barcode.FORMAT_QR_CODE
		        // ]*/
		    // });
		    var winCode = TiTools.UI.Controls.createWindow({
				main: "Controllers/addQr.js",
				navBarHidden: true,
			});
			winCode.initialize();
			winCode.open({modal: true});
		}
	);
	
	Barcode.addEventListener('error', function (e) {
		Ti.UI.createAlertDialog({
	        message: e.message,
	        title: "ERROR"
        }).show();
	});
	Barcode.addEventListener('success', function (e) {
    	if(e.contentType === Barcode.URL) {
		    // Ti.UI.createAlertDialog({
		        // message: e.result,
		        // title: "SUCCESS"
	        // }).show();
		    Barcode.cancel();
		    actIndicator(true);
		    itsbeta.postActiv(e.result);
    	}
	});
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