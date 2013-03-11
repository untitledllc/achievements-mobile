/**
	@brief
		Создание расширенной кнопки c 9 частями
	@parent
		Ti.UI.View
	@param params : {
			type : String // Тип кнопки: click - станндарт, toggle - нажал, отжал. (по умолчанию click)
			backgroundImage : {
				normal: // кнопка не нажатая
				{
					leftTopImage: String // адрес левой верхней части
					leftCenterImage: String // адрес левой центральной части
					leftDownImage: String // адрес левой нижней части					
					centerTopImage: String // адрес центрально верхней части
					centerCenterImage: String // адрес центрального изображения
					centerDownImage: String // адрес центрально нижней части
					rightTopImage: String // адрес правой верхней части
					rightCenterImage: String // адрес правой центральной части
					rightDownImage: String // адрес право нижней части
				}
				select:	// кнопка нажата
				{
					leftTopImage: String // адрес левой верхней части
					leftCenterImage: String // адрес левой центральной части
					leftDownImage: String // адрес левой нижней части					
					centerTopImage: String // адрес центрально верхней части
					centerCenterImage: String // адрес центрального изображения
					centerDownImage: String // адрес центрально нижней части
					rightTopImage: String // адрес правой верхней части
					rightCenterImage: String // адрес правой центральной части
					rightDownImage: String // адрес право нижней части
				}
				disable: //кнопка отключена
				{
					leftTopImage: String // адрес левой верхней части
					leftCenterImage: String // адрес левой центральной части
					leftDownImage: String // адрес левой нижней части					
					centerTopImage: String // адрес центрально верхней части
					centerCenterImage: String // адрес центрального изображения
					centerDownImage: String // адрес центрально нижней части
					rightTopImage: String // адрес правой верхней части
					rightCenterImage: String // адрес правой центральной части
					rightDownImage: String // адрес право нижней части
				}
				left: String // отступ кнопки с лева
				right: String // отступ кнопки справа
				
				widthLeftTop: int // Длинна левого верхнего изображения
				widthRightTop: int // Длинна правого верхнего изображения
				widthLeftCenter: int // Длинна левого центрального изображения
				widthRightCenter: int // Длинна правого центрального изображения
				widthLeftDown: int // Длинна левого нижнего изображения
				widthRightDown: int // Длинна правого нижнего изображения
				
				heightLeftTop: int // Высота левого верхнего изображения
				heightRightTop: int // Высота правого верхнего изображения
				heightCenterTop: int // Высота центрального верхнего изображения
				heightCenterDown: int // Высота центрального нижнего изображения
				heightLeftDown: int // Высота левого нижнего изображения
				heightRightDown: int // Высота правого нижнего изображения

			}
			content: // контент содержащийся в кнопке. Можнт быть Объектом или МАССИВОМ собственных объектов!
			{
				leftImage : // левое изображение
				{
					backgroundColor : String // адрес картинки
					height : int // высота изображения
					width : int // длинна изображения
					left : px // отступ с лева
					right : px // отступ с права
					top : px // отступ с верху
					bottom : px // отступ с низу
	 			},
	 			title : // текст кнопки
	 			{
					height : int // высота изображения
					width : int // длинна изображения
					left : px // отступ с лева
					right : px // отступ с права
					top : px // отступ с верху
					bottom : px // отступ с низу
					color : String // цвет надписи,
					textAlign : String // Форматирование текста,
					font : {
						fontFamily : String // Шрифт,
						fontSize : int // размер шрифта,
					},
	 			},
	 			rightImage : // правое изображение
	 			{
					backgroundColor : String // адрес картинки
					height : int // высота изображения
					width : int // длинна изображения
					left : px // отступ с лева
					right : px // отступ с права
					top : px // отступ с верху
					bottom : px // отступ с низу
	 			}
 			}
			
		}
	@events
	@methods
		disableView: //  Приневает значение bool отключает и включает кнопку
		{ 
			arg: Bool // true || false
		}
		isDisableView: // возвращает значение bool, отключена кнопка или нет
		{
			
		}
	@return
**/
function createButton9Ext(params)
{
	if(params.type == undefined)
	{
		params.type = "click";
	}
	if((params.type != "click") && (params.type != "toggle"))
	{
		throw String(TiTools.Locate.getString('TITOOLS_THROW_UNSUPPORTED_BUTTON_TYPE') + '\n' + params.type);
	}
	
	var self = Ti.UI.createView(
		{
			left: params.backgroundImage.left,
			right: params.backgroundImage.right,
			height: Ti.UI.FILL,
			width: Ti.UI.FILL,
			disableVar : true
		}
	);
	///
	var leftTopView = Ti.UI.createView(
		{
			top: 0,
			left: 0,
			height: params.backgroundImage.heightLeftTop,
			width: params.backgroundImage.widthLeftTop,
			backgroundImage: params.backgroundImage.normal.leftTopImage,
			touchEnabled : false
		}
	);
	self.add(leftTopView); 
	
	var leftCenterView = Ti.UI.createView(
		{
			left: 0,
			top: params.backgroundImage.heightLeftTop,
			bottom: params.backgroundImage.heightLeftDown,
			width: params.backgroundImage.widthLeftCenter,
			backgroundImage: params.backgroundImage.normal.leftCenterImage,
			touchEnabled : false
		}
	);
	self.add(leftCenterView);
	
	var leftDownView = Ti.UI.createView(
		{
			left: 0,
			bottom: 0,
			height: params.backgroundImage.heightLeftDown,
			width: params.backgroundImage.widthLeftDown,
			backgroundImage: params.backgroundImage.normal.leftDownImage,
			touchEnabled : false
		}
	);
	self.add(leftDownView);
	///
	var centerTopView = Ti.UI.createView(
		{
			top: 0,
			left: params.backgroundImage.widthLeftTop,
			right: params.backgroundImage.widthRightTop,
			height: params.backgroundImage.heightCenterTop,
			backgroundImage: params.backgroundImage.normal.centerTopImage,
			touchEnabled : false
		}
	);
	self.add(centerTopView); 
	
	var centerCenterView = Ti.UI.createView(
		{
			top: params.backgroundImage.heightCenterTop,
			left: params.backgroundImage.widthLeftCenter,
			right: params.backgroundImage.widthRightCenter,
			bottom: params.backgroundImage.heightCenterDown,
			backgroundImage: params.backgroundImage.normal.centerCenterImage,
			touchEnabled : false
		}
	);
	self.add(centerCenterView);
	
	var centerDownView = Ti.UI.createView(
		{
			left: params.backgroundImage.widthLeftDown,
			right: params.backgroundImage.widthRightDown,
			bottom: 0,
			height: params.backgroundImage.heightCenterDown,
			backgroundImage: params.backgroundImage.normal.centerDownImage,
			touchEnabled : false
		}
	);
	self.add(centerDownView);
	///
	var rightTopView = Ti.UI.createView(
		{
			top: 0,
			right: 0,
			height: params.backgroundImage.heightRightTop,
			width: params.backgroundImage.widthRightTop,
			backgroundImage: params.backgroundImage.normal.rightTopImage,
			touchEnabled : false
		}
	);
	self.add(rightTopView); 
	
	var rightCenterView = Ti.UI.createView(
		{
			top: params.backgroundImage.heightRightTop,
			right: 0,
			bottom: params.backgroundImage.heightRightDown,
			width: params.backgroundImage.widthRightCenter,
			backgroundImage: params.backgroundImage.normal.rightCenterImage,
			touchEnabled : false
		}
	);
	self.add(rightCenterView);
	
	var rightDownView = Ti.UI.createView(
		{
			right: 0,
			bottom: 0,
			height: params.backgroundImage.heightRightDown,
			width: params.backgroundImage.widthRightDown,
			backgroundImage: params.backgroundImage.normal.rightDownImage,
			touchEnabled : false
		}
	);
	self.add(rightDownView);
	///
	var content = Ti.UI.createView(
		{
			height: Ti.UI.FILL,
			width: Ti.UI.FILL
		}
	);
	self.add(content);
	
	if(TiTools.Object.isObject(params.content) == true)
	{
		var leftOffset = 0;
		var rightOffset = 0;
		if(params.content.leftImage != undefined)
		{
			if(params.content.leftImage.left == undefined)
			{
				params.content.leftImage.left = 0;
			}
			params.content.leftImage.right = undefined;
			var contentLeftImage = Ti.UI.createView(params.content.leftImage);
			content.add(contentLeftImage);
			leftOffset += params.content.leftImage.left + params.content.leftImage.width;
		}
		
		if(params.content.rightImage != undefined)
		{
			if(params.content.rightImage.right == undefined)
			{
				params.content.rightImage.right = 0;
			}
			params.content.rightImage.left = undefined;
			var contentRightImage = Ti.UI.createView(params.content.rightImage);
			content.add(contentRightImage);
			rightOffset = params.content.rightImage.right + params.content.rightImage.width;
		}
		
		if(params.content.title != undefined)
		{
			if(params.content.title.left == undefined)
			{
				params.content.title.left = 0;
			}
			params.content.title.left += leftOffset;
			if(params.content.title.right == undefined)
			{
				params.content.title.right = 0;
			}
			params.content.title.right += rightOffset;
			var contentTitle = Ti.UI.createLabel(params.content.title);
			content.add(contentTitle);
		}
	}
	else
	{
		if(TiTools.Object.isArray(params.content) == true)
		{
			for(var i = 0; i < params.content.length; i++)
			{
				content.add(params.content[i]);
			}
		}
	}
	changeImageView("normal");
	
	// local variables
	var switched = false;
	var disabled = false;
	
	/// event
	self.addEventListener("touchstart",
		function(event)
		{
			if(params.type == "click")
			{
				changeImageView("select");
			}
			else if(params.type == "toggle")
			{
				if(switched == false)
				{
					switched = true;
					changeImageView("select");
				}
				else
				{
					switched = false;
					changeImageView("normal");
				}
			}
		}
	);
	self.addEventListener("touchend",
		function(event)
		{
			if(switched == false)
			{
				changeImageView("normal");
			}
		}
	);
	self.addEventListener("touchcancel",
		function(event)
		{
			if(switched == false)
			{
				changeImageView("normal");
			}
		}
	);
	
	self.disableView = function(arg)
	{
		if(disabled != arg)
		{
			if(arg == true)
			{
				changeImageView("disable");
				disabled = true;
			}
			else
			{
				disabled = false;
				changeImageView("normal");
			}
		}
	};
	self.isDisabledView = function()
	{
		return disabled;
	};
	
	return self;
	
	// other functions
	function changeImageView(param)
	{
		if(disabled == true)
		{
			return;
		}
		switch(param)
		{
			case "normal":
				leftTopView.backgroundImage = params.backgroundImage.normal.leftTopImage
				leftCenterView.backgroundImage = params.backgroundImage.normal.leftCenterImage
				leftDownView.backgroundImage = params.backgroundImage.normal.leftDownImage
				centerTopView.backgroundImage = params.backgroundImage.normal.centerTopImage
				centerCenterView.backgroundImage = params.backgroundImage.normal.centerCenterImage
				centerDownView.backgroundImage = params.backgroundImage.normal.centerDownImage
				rightTopView.backgroundImage = params.backgroundImage.normal.rightTopImage
				rightCenterView.backgroundImage = params.backgroundImage.normal.rightCenterImage
				rightDownView.backgroundImage = params.backgroundImage.normal.rightDownImage
				break
			case "select":
				leftTopView.backgroundImage = params.backgroundImage.select.leftTopImage
				leftCenterView.backgroundImage = params.backgroundImage.select.leftCenterImage
				leftDownView.backgroundImage = params.backgroundImage.select.leftDownImage
				centerTopView.backgroundImage = params.backgroundImage.select.centerTopImage
				centerCenterView.backgroundImage = params.backgroundImage.select.centerCenterImage
				centerDownView.backgroundImage = params.backgroundImage.select.centerDownImage
				rightTopView.backgroundImage = params.backgroundImage.select.rightTopImage
				rightCenterView.backgroundImage = params.backgroundImage.select.rightCenterImage
				rightDownView.backgroundImage = params.backgroundImage.select.rightDownImage
				break
			case "disable":
				leftTopView.backgroundImage = params.backgroundImage.disable.leftTopImage
				leftCenterView.backgroundImage = params.backgroundImage.disable.leftCenterImage
				leftDownView.backgroundImage = params.backgroundImage.disable.leftDownImage
				centerTopView.backgroundImage = params.backgroundImage.disable.centerTopImage
				centerCenterView.backgroundImage = params.backgroundImage.disable.centerCenterImage
				centerDownView.backgroundImage = params.backgroundImage.disable.centerDownImage
				rightTopView.backgroundImage = params.backgroundImage.disable.rightTopImage
				rightCenterView.backgroundImage = params.backgroundImage.disable.rightCenterImage
				rightDownView.backgroundImage = params.backgroundImage.disable.rightDownImage
				break
		}
	};
}
