/**
	@brief
		Создание прогресс бара
	@parent
		Ti.UI.View
	@param params : {
			value : int // от 0 до 100 ! по умолчанию 0
			backgroundImage : {
				empty: // Изображения для пустого прогресс бара
				{
					leftImage : String // Левое изрображение, обозначающее первые проценты (статичное)
					centerImage : String // центральное изрображение, обозначающее проценты от 1 до 99  (тянется)	
					rightImage : String // Правое изрображение, обозначающее последние проценты (статичное)
				}
				select:	// Изображения заполняющие прогресс бар
				{
					leftImage : String // Левое изрображение, обозначающее первые проценты (статичное)
					centerImage : String // центральное изрображение, обозначающее проценты от 1 до 99  (тянется)	
					rightImage : String // Правое изрображение, обозначающее последние проценты (статичное)
				}
				left: String // отступ кнопки с лева
				right: String // отступ кнопки справа
				
				widthLeftView : int // длинна левого изображения
				widthRightView : int // длинна правого изображения
				
				height : int // общая высота прогресс бара
			}
		}
	@events
	@methods
		setValue: //  Приневает значение bool отключает и включает кнопку
		{ 
			arg: Bool // true || false
		}
		getValue: // возвращает значение int
		{
			
		}
	@return
**/
function createProgressBarExt(params)
{
	var self = Ti.UI.createView(
		{
			left: params.backgroundImage.left,
			right: params.backgroundImage.right,
			height: params.backgroundImage.height,
			width:  Ti.UI.FILL,
			disableVar : true
		}
	);
	
	var leftView = Ti.UI.createView({
		left: 0,
		height:  params.backgroundImage.height,
		width:  params.backgroundImage.widthLeftView,
		backgroundImage:  params.backgroundImage.empty.leftImage,
	});
	
	self.add(leftView);
	
	var centerViewEmpty = Ti.UI.createView({
		left: params.backgroundImage.widthLeftView,
		right: params.backgroundImage.widthRightView,
		height:  params.backgroundImage.height,
		backgroundImage:  params.backgroundImage.empty.centerImage,
	});
	
	self.add(centerViewEmpty);
	
	var centerViewFullContent = Ti.UI.createView({
		left: params.backgroundImage.widthLeftView,
		right: params.backgroundImage.widthRightView,
		height: params.backgroundImage.height,
	});
	
	self.add(centerViewFullContent);
	
	var centerViewFull = Ti.UI.createView({
		left: 0,
		height: params.backgroundImage.height,
	});
	
	centerViewFullContent.add(centerViewFull);
	
	var rightView = Ti.UI.createView({
		right: 0,
		height: params.backgroundImage.height,
		width: params.backgroundImage.widthRightView,
		backgroundImage: params.backgroundImage.empty.rightImage,
	});
	
	self.add(rightView);
	
	self.setValue = function(arg) 
	{
		if(arg >0 && arg < 100)
		{
			leftView.backgroundImage = params.backgroundImage.select.leftImage;
			if(arg == 99)
			{
				centerViewFull.width = "100%";
			}else
			{
				centerViewFull.width = arg - 1 + "%";
			}
			centerViewFull.backgroundImage = params.backgroundImage.select.centerImage;
			rightView.backgroundImage = params.backgroundImage.empty.rightImage;
		}
		if(arg == 0)
		{
			leftView.backgroundImage = params.backgroundImage.empty.leftImage;
		}
		if(arg >= 100)
		{
			centerViewFull.width = "100%";
			rightView.backgroundImage = params.backgroundImage.select.rightImage;
		}
	}
	
	self.getValue = function()
	{
		return params.value;
	}
	
	if(params.value == undefined){
		params.value = 0;
	}
	self.setValue(params.value);
	
	return self;
}