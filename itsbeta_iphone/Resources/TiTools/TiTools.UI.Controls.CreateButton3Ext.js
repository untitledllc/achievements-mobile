/**
	@brief
		Создание расширенной кнопки с 3 частями
	@parent
		Ti.UI.View
	@param params : {
			type : String // Тип кнопки: click - станндарт, toggle - нажал, отжал. (по умолчанию click)
			//  Если существует цвет заднего фона, то изображения игнорируются 
			backgroundColorNormal: String // не нажатый режим
			backgroundColorSelect: String // нажатый режим
			BackgroundColorDisable: String // кнопка отлючена
			backgroundImage : {
				normal: // кнопка не нажатая
				{
					leftImage : String // адрес левой части кнопки
					centerInage : String // адрес центральной части кнопки
					rightImage : String // адрес правая части кнопки
				}
				select:	// кнопка нажата
				{
					leftImage : String // адрес левой части кнопки
					centerInage : String // адрес центральной части кнопки
					rightImage : String // адрес правая части кнопки
				}
				disable: //кнопка отключена
				{
					leftImage : String // адрес левой части кнопки
					centerInage : String // адрес центральной части кнопки
					rightImage : String // адрес правая части кнопки
				}
				left: String // отступ кнопки с лева
				right: String // отступ кнопки справа
				height : px // Общая Высота кнопки (фиксированная)
				widthLeft : px // Длинна левой части кнопки (фиксированная)
				widthRight : px // Длинна правой части кнопки (фиксированная)
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
function createButton3Ext(params)
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
			height: params.backgroundImage.height,
			left: params.backgroundImage.left,
			right: params.backgroundImage.right,
			width: Ti.UI.FILL,
			disableVar : true
		}
	);
	
	var leftView = Ti.UI.createView(
		{
			left: 0,
			height: params.backgroundImage.height,
			width: params.backgroundImage.widthLeft,
			touchEnabled : false
		}
	);
	self.add(leftView); 
	
	var centerView = Ti.UI.createView(
		{
			left: params.backgroundImage.widthLeft,
			right: params.backgroundImage.widthRight,
			height: params.backgroundImage.height,
			touchEnabled : false
		}
	);
	self.add(centerView);
	
	var rightView = Ti.UI.createView(
		{
			right: 0,
			height: params.backgroundImage.height,
			width: params.backgroundImage.widthRight,
			touchEnabled : false
		}
	);
	self.add(rightView);
	
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
			contentLeftImage.touchEnabled = false;
			self.add(contentLeftImage);
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
			contentRightImage.touchEnabled = false;
			self.add(contentRightImage);
			rightOffset = params.content.rightImage.right + params.content.rightImage.width;
		}
		
		if(params.content.title != undefined )
		{
			if(params.content.title.left == undefined && leftOffset != 0)
			{
				params.content.title.left = 0;
			}
			if(leftOffset != 0)
			{
				params.content.title.left += leftOffset;
			}
			if(params.content.title.right == undefined && rightOffset != 0)
			{
				params.content.title.right = 0;
			}
			if(rightOffset != 0)
			{
				params.content.title.right += rightOffset;
			}
			var contentTitle = Ti.UI.createLabel(params.content.title);
			contentTitle.touchEnabled = false;
			self.add(contentTitle);
		}
	}
	else
	{
		if(TiTools.Object.isArray(params.content) == true)
		{
			for(var i = 0; i < params.content.length; i++)
			{
				self.add(params.content[i]);
			}
		}
	}
	
	self.changeImageView = function(param)
	{
		if(disabled == true)
		{
			return;
		}
		switch(param)
		{
			case "normal":
				if(params.backgroundColorNormal != undefined)
				{
					self.backgroundColor = params.backgroundColorNormal;
					break;
				}
				leftView.backgroundImage = params.backgroundImage.normal.leftImage
				centerView.backgroundImage = params.backgroundImage.normal.centerImage
				rightView.backgroundImage = params.backgroundImage.normal.rightImage
				break
			case "select":
				if(params.backgroundColorSelect != undefined)
				{
					self.backgroundColor = params.backgroundColorSelect;
					break;
				}
				leftView.backgroundImage = params.backgroundImage.select.leftImage
				centerView.backgroundImage = params.backgroundImage.select.centerImage
				rightView.backgroundImage = params.backgroundImage.select.rightImage
				break
			case "disable":
				if(params.backgroundColorDisable != undefined)
				{
					self.backgroundColor = params.backgroundColorDisable;
					break;
				}
				leftView.backgroundImage = params.backgroundImage.disable.leftImage
				centerView.backgroundImage = params.backgroundImage.disable.centerImage
				rightView.backgroundImage = params.backgroundImage.disable.rightImage
				break
		}
	};
	
	self.changeImageView("normal");
	
	// local variables
	self.switched = false;
	var disabled = false;
	
	/// event
	self.addEventListener("touchstart",
		function(event)
		{
			if(params.type == "click")
			{
				self.changeImageView("select");
			}
			else if(params.type == "toggle")
			{
				if(self.switched == false)
				{
					self.switched = true;
					self.changeImageView("select");
				}
				else
				{
					self.switched = false;
					self.changeImageView("normal");
				}
			}
		}
	);
	self.addEventListener("touchend",
		function(event)
		{
			if(self.switched == false)
			{
				self.changeImageView("normal");
			}
		}
	);
	self.addEventListener("touchcancel",
		function(event)
		{
			if(self.switched == false)
			{
				self.changeImageView("normal");
			}
		}
	);
	
	self.disableView = function(arg)
	{
		if(disabled != arg)
		{
			if(arg == true)
			{
				self.changeImageView("disable");
				disabled = true;
			}
			else
			{
				disabled = false;
				self.changeImageView("normal");
			}
		}
	};
	self.isDisabledView = function()
	{
		return disabled;
	};
	
	return self;
	
	// other functions
	
}