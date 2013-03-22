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
	
	// Загрузка контента окна
	ui = TiTools.UI.Loader.load("Views/add.js", window);
	
	ui.back.addEventListener("click",function(event)
	{
		window.close();
	});
	
	ui.code.addEventListener("click",function(event)
	{
		var winCode = TiTools.UI.Controls.createWindow(
			{
				main : "Controllers/addCode.js",
				navBarHidden : true,
			}
		);
		winCode.initialize();
		winCode.open();
	});
	
	//---------------------------------------------//
	// QR code scanner
	//---------------------------------------------//
	var Barcode = require('ti.barcode');
	Barcode.allowRotation = true;
	Barcode.displayedMessage = '';
	Barcode.useLED = true;
	
	var overlay = Ti.UI.createView({
	    backgroundColor: 'transparent',
	    top: 0, right: 0, bottom: 0, left: 0
	});
	
	var toggleLEDButton = Ti.UI.createButton({
	    title: Barcode.useLED ? 'LED is On' : 'LED is Off',
	    textAlign: 'center',
	    color: '#000', backgroundColor: '#fff', style: 0,
	    font: { fontWeight: 'bold', fontSize: 16 },
	    borderColor: '#000', borderRadius: 10, borderWidth: 1,
	    opacity: 0.5,
	    width: 220, height: 30,
	    bottom: 10
	});
	toggleLEDButton.addEventListener('click', function () {
	    Barcode.useLED = !Barcode.useLED;
	    toggleLEDButton.title = Barcode.useLED ? 'LED is On' : 'LED is Off';
	});
	overlay.add(toggleLEDButton);
	
	var cancelButton = Ti.UI.createButton({
	    title: 'Cancel', textAlign: 'center',
	    color: '#000', backgroundColor: '#fff', style: 0,
	    font: { fontWeight: 'bold', fontSize: 16 },
	    borderColor: '#000', borderRadius: 10, borderWidth: 1,
	    opacity: 0.5,
	    width: 220, height: 30,
	    top: 20
	});
	cancelButton.addEventListener('click', function () {
	    Barcode.cancel();
	});
	overlay.add(cancelButton);
	
	ui.qr.addEventListener('click', function () {
	    Barcode.capture({
	        animate: true,
	        overlay: overlay,
	        showCancel: false,
	        showRectangle: false,
	        keepOpen: true/*,
	        acceptedFormats: [
	            Barcode.FORMAT_QR_CODE
	        ]*/
	    });
	});
	
	Barcode.addEventListener('error', function (e) {
		Ti.UI.createAlertDialog({
	        message: e.message,
	        title: "ERROR"
        }).show();
	});
	Barcode.addEventListener('cancel', function (e) {
     	Ti.UI.createAlertDialog({
	        message: "CANCEL",
	        title: "CANCEL"
        }).show();
	});
	Barcode.addEventListener('success', function (e) {
    	if(e.contentType === Barcode.URL) {
		    Ti.UI.createAlertDialog({
		        message: e.result,
		        title: "SUCCESS"
	        }).show();
		    Barcode.cancel();
    	}
	});
	
 }
//---------------------------------------------//
// Функции лентяйки
//---------------------------------------------//

// Обработчик при открытии окна
function onWindowOpen(window, event)
{
	
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