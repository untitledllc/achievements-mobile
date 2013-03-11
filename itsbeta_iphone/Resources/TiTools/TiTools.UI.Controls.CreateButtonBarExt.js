/**
	@brief
		Создание модального окна с ButtonBar
	@parent
		Ti.UI.View
	@param params : {
		view:{ // размеры панели с кнопками
			
		},
		masButton:[
			{
				createButtonExt(); // описание выше
			}
		]
		
	}
	@events
		control.addEventListener("clickButtonBar",function(event){ // возвращает индекс нажатой кнопки
			alert(event.index);
		});
	@methods
		getActive(index) //
		setActive(index) //
		
	@return
**/

Ti.include("TiTools.UI.Controls.CreateButton3Ext.js");

function createButtonBarExt(params)
{
	var content = Ti.UI.createView(params.view);
	content.touchEnabled = false;
	content.touchEnabled =false;
	content.enableContent = true;
	(params.masButton.length != 0)
	{
		var widthOffSet = 100/params.masButton.length;
	}
	
	for(var i = 0; i < params.masButton.length; i++)
	{
		var but = createButton3Ext(params.masButton[i]);
		but.left = widthOffSet * i + "%";
		but.width = widthOffSet + "%";
		but.index = i;
		but.active = false;
		but.enable = true;
		but.addEventListener("touchstart",function(event)
			{
				for(var i = 0; i < content.children.length; i++)
				{
					if(event.source.index != i)
					{
						content.children[i].changeImageView("normal");
						content.children[i].switched = false;
					}
				}
				
				if(TiTools.Platform.isAndroid == true)
				{
					Ti.App.fireEvent("clickButtonBar",{index : event.source.index});
				}
			}
		);
		content.add(but);
	}
	content.enable = function(index,bool)
	{
		if(content.children[index] != undefined)
		{
			if(bool == true)
			{
				content.children[index].disableView(false);
			}else
			{
				content.children[index].disableView(true);
			}
		}
	}
	
	content.isEnable = function(index)
	{
		if(content.children[index] != undefined)
		{
			if(content.children[index].isDisabledView())
			{
				return false;
			}else
			{
				return true;
			}
		}
	}
	
	content.enableAll = function(bool)
	{
			for(var i = 0; i < content.children.length; i++)
			{
				content.children[i].disableView(!bool);
			}
			content.enableContent = bool;
	}
	
	content.isEnableAll = function()
	{
		return content.enableContent;
	}
	
	content.changeMod = function(string,index)
	{
		content.children[index].changeImageView(string);
	}
	
	return content;
}