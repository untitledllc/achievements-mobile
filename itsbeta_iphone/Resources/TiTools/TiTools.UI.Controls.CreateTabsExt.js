/**
	@brief
		Создание вкладок tabs
	@parent
		Ti.UI.View
	@param params : {
		scroll : Bool // Если вкладки не влазиют, то разрешаем скролить да/нет
		left : int //
		right : int //
			leftTab : {
				top: int //
				right: int //
				bottom: int //
				height: int //
				width: int //
				backgroundImage: String //
			},
			centerTab : {
				top: int //
				left: int //
				right: int //
				bottom: int //
				height: int //
				width: int //
				backgroundImage: String //
			},
			rightTab : {
				top: int //
				left: int //
				bottom: int //
				height: int //
				width: int //
				backgroundImage: String //
			},
			tabs:
			[
				{
				top: int //
				bottom: int //
				height: int //
				width: int //
				backgroundImageNormal: String //
				backgroundImageSelect: String //
				
				leftImage: { // Если таб тянущийся то принемается такая конструкция 
					backgroundImage: ,
					height: ,
					width: ,
				},
				centerImage: {
					backgroundImage: ,
					height: ,
				},
				rightImage: {
					backgroundImage: ",
					height: ,
					width: ,
				}
			]
				
		}
	@events
	@methods
		setActiveTab: // Таб с таким индексом делает активным
		{
			index : int //
		}
		getActiveTab: // Возвращает индекс активного таба
		{
			return int //
		}
	@return
**/
function createTabsExt(params)
{
	var content = Ti.UI.createView(
		{
			left: params.left,
			right: params.right,
			height: Ti.UI.SIZE,
			width: Ti.UI.FILL,
			enable: true
		}
	);
	
	if(params.scroll == true)
	{
		var self = Ti.UI.createScrollView(
			{
				top: 0,
				contentWidth : 'auto',
				contentHeight : params.height,
				left: params.left,
				right: params.right,
				height: params.height,
				width:  Ti.UI.FILL
			}
		);
	}else
	{
		var self = Ti.UI.createView(
			{
				top: 0,
				left: params.left,
				right: params.right,
				height: Ti.UI.SIZE,
				width:  Ti.UI.FILL
			}
		);
	}
	
	content.add(self);
	
	var left = Ti.UI.createView(params.leftTab);
	left.left = 0;
	
	var right = Ti.UI.createView(params.rightTab);
	right.right = 0;
	
	if(params.tabs.length != 0){
		
		self.add(left);
		var leftOffset = left.width;
		var masTab = [];
		var masTabContent = [];
		
		for(var i = 0; i < params.tabs.length; i++)
		{
			var tab = Ti.UI.createView(params.tabs[i]);
			if(params.tabs[i].backgroundImageNormal != undefined)
			{
				tab.backgroundImage = tab.backgroundImageNormal;
			}
			else
			{
				if(params.tabs[i].leftImage != undefined)
				{
					var tabLeftImage = Ti.UI.createView(tab.leftImage);
					tabLeftImage.touchEnabled = false;
					tabLeftImage.backgroundImage = tabLeftImage.backgroundImageNormal;
					tabLeftImage.left = 0;
					tab.add(tabLeftImage);
					tab.tabLeftImage = tabLeftImage;
					
					var tabRightImage = Ti.UI.createView(tab.rightImage);
					tabRightImage.touchEnabled = false;
					tabRightImage.backgroundImage = tabRightImage.backgroundImageNormal;
					tabRightImage.right = 0;
					tab.add(tabRightImage);
					tab.tabRightImage = tabRightImage;
					
					var tabCenterImage = Ti.UI.createView(tab.centerImage);
					tabCenterImage.touchEnabled = false;
					tabCenterImage.backgroundImage = tabCenterImage.backgroundImageNormal;
					tabCenterImage.left = tabLeftImage.width;
					tabCenterImage.Right = tabRightImage.width;
					tab.add(tabCenterImage);
					tab.tabCenterImage = tabCenterImage;
				}
			}
			tab.number = i;
			tab.left = leftOffset;
			tab.addEventListener("click",function(event)
				{
					if(content.enable == true)
					{
						if(masTab[event.source.number].enable == true)
						{
							tabHide(content.getActiveTab());
							tabShow(event.source.number);
						}
					}
				}
			);
			leftOffset += tab.width;
			self.add(tab);
			var tabContent = Ti.UI.createView(
				{
					top: params.height,
					width: Ti.UI.FILL,
					height: Ti.UI.SIZE
				}
			);
			//tabContent.add(tab.content);
			content.add(tabContent);
			
			masTab[i] = tab;
			masTab[i].active = false;
			masTab[i].enable = true;
			masTabContent[i] = tabContent;
			masTabContent[i].hide();
			
			
			if(i < params.tabs.length - 1)
			{
				var center = Ti.UI.createView(params.centerTab);
				center.left = leftOffset;
				leftOffset += center.width;
				self.add(center);
			}
		}
		right.left = leftOffset;
		self.add(right);
	}
	
	tabShow(0);
	
	content.getActiveTab = function()
	{
		for(var j = 0; j < masTab.length; j++)
		{
			if(masTab[j].active == true)
			{
				return j;
			}
		}
	}
	
	content.setActiveTab = function(index)
	{
		tabHide(content.getActiveTab());
		tabShow(index);
	}
	
	content.setEnable = function(bool)
	{
		content.enable = bool;
	}
	
	content.getEnable = function()
	{
		return content.enable;
	}
	
	content.setEnableChange = function(index,bool)
	{
		masTab[index].enable = bool;
	}
	
	content.getEnableChange = function(index)
	{
		return masTab[index].enable;
	}
	
	function tabShow(index)
	{
		if(masTab[index].backgroundImageNormal != undefined)
		{
			masTab[index].backgroundImage = masTab[index].backgroundImageSelect;
		}
		else
		{
			masTab[index].tabLeftImage.backgroundImage = masTab[index].tabLeftImage.backgroundImageSelect;
			masTab[index].tabRightImage.backgroundImage = masTab[index].tabRightImage.backgroundImageSelect;
			masTab[index].tabCenterImage.backgroundImage = masTab[index].tabCenterImage.backgroundImageSelect;
		}
		masTab[index].active = true;
		masTabContent[index].show();
	}
	
	function tabHide(index)
	{
		if(masTab[index].backgroundImageNormal != undefined)
		{
			masTab[index].backgroundImage = masTab[index].backgroundImageNormal;
		}
		else
		{
			masTab[index].tabLeftImage.backgroundImage = masTab[index].tabLeftImage.backgroundImageNormal;
			masTab[index].tabRightImage.backgroundImage = masTab[index].tabRightImage.backgroundImageNormal;
			masTab[index].tabCenterImage.backgroundImage = masTab[index].tabCenterImage.backgroundImageNormal;
		}
		masTab[index].active = false;
		masTabContent[index].hide();
	}
	return content;
}
