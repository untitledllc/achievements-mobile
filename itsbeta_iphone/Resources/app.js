var TiTools = require('TiTools/TiTools');

//---------------------------------------------//

TiTools.initLibraries(); //Первичная загрузка всех библиотек

//---------------------------------------------//


//---------------------------------------------//

if(TiTools.Platform.isAndroid == true)
{
	//TiTools.UI.Preset.load("Presets/Common.js");
	switch(TiTools.Platform.screenMode) // Определение группы в которую входит экран
	{
		case TiTools.Platform.SCREEN_MODE_UNKNOWN: // Если не получилось определить группу экрана
		break;
		case TiTools.Platform.SCREEN_MODE_SMALL:
		break;
		case TiTools.Platform.SCREEN_MODE_NORMAL:
		break;
		case TiTools.Platform.SCREEN_MODE_LARGE: 
		break;
		case TiTools.Platform.SCREEN_MODE_EXTRA_LARGE:
		break;
	}
}
else if(TiTools.Platform.isIOS == true)
{
	if(TiTools.Platform.isIPhone == true)
	{
		//TiTools.UI.Preset.load("Presets/iPhone/Common.js");
	}
}

//---------------------------------------------//

Ti.UI.setBackgroundColor("#fff");

//---------------------------------------------//

var window = TiTools.UI.Controls.createWindow(
	{
		main : "Controllers/Login.js",
		navBarHidden : true,
		exitOnClose : true
	}
);
window.initialize();
window.open();

//---------------------------------------------//

TiTools.Global.set("htmlWrapBefore", '<html><head><style> @font-face {font-family: HelveticaN; src: url(Helvetica.dfont);} body{  background-color: #f1efe4; margin: 0; font-family: Helvetica; }a{color: #7ed6f9; font-size: 15 } p{color:#777575; font-size: 15} </style> <script type="text/javascript">window.onload=function(){var links=document.querySelectorAll("a"),i=0,len=links.length;while(i<len){links[i++].addEventListener("click", function(evt){evt.preventDefault();this.href && Ti.App.fireEvent("linkClicked",{url:this.href});},false);}};</script></head><body>');
TiTools.Global.set("htmlWrapAfter", '</body></html>');

Ti.App.addEventListener("linkClicked", function(param)
{
	Ti.Platform.openURL(param.url);
});

//---------------------------------------------//