/**
	@brief
		Создание модального окна с выбором даты или времени
	@parent
		Ti.UI.View
	@param params : {
		view : { // параметры основного окна
			
		},
		titleViewStyle: { // параметры view где будет расположен заголовок
			
		},
		titleStyle: { // параметры Заголовка (Label)
			
		},
		separatorTitle: { // Параметры разделителя после заголовка
			
		},
		separatorRow:{ // Разделитель между кнопками которые находятся в модальном окне
			
		},
		buttonFromStyle:{ // Параметры кнопки "С"
			
		},
		buttonToStyle: { // Параметры кнопки "По"
			
		},
		callBack : function(piker){}, // функция срабатывает при изменении пикера
		picker: {  // параметры пикера  ограничения и начальные значения
			
		},
		buttonStyle: { // параметры кнопок "Отмена" и  "Готово"
		
		}
	}
	@events
	@methods
	@return
**/
function createModalWindowDateOrTime(params)
{
	var content = Ti.UI.createView(params.view);
	var titleRow = Ti.UI.createView(params.titleViewStyle);
	var titleLabel = Ti.UI.createLabel(params.titleStyle);
	titleRow.add(titleLabel);
	content.add(titleRow);
	
	var heightOffset = titleRow.height;
	
	var separatorTitle = Ti.UI.createView(params.separatorTitle);
	separatorTitle.top = heightOffset;
	content.add(separatorTitle);
	
	if(separatorTitle.height != undefined)
	{
		heightOffset += separatorTitle.height;
	}
	
	
	var selectImage = Ti.UI.createView(params.selectImage);
	if(selectImage.right == undefined)
	{
		selectImage.right = "5%";
	}
	
	heightOffset = 0;
	params.picker.format24 = true;
	
	var picker = Ti.UI.createPicker(params.picker);
	
	var returnDate = picker.value.toLocaleString();
	
	picker.addEventListener('change',function(e){
		returnDate = e.value.toLocaleString();
	});
	
	if(params.picker.typePicker == "date")
	{
		picker.type = Ti.UI.PICKER_TYPE_DATE;
	}
	if(params.picker.typePicker == "time")
	{
		picker.type = Ti.UI.PICKER_TYPE_TIME
	}
	
	picker.addEventListener('change',function(e){
		if(params.collBack != undefined)
		{
			params.callBack(picker);
		}
		returnDate = e.value.toLocaleString();
	});
	
	content.add(picker);
	
	var viewButton = Ti.UI.createView(
		{
			bottom: 0,
			width: Ti.UI.FILL,
			height: Ti.UI.SIZE,
		}
	);
	
	var separatorHorizontal = Ti.UI.createView(
		{
			top: 0,
			width: Ti.UI.FILL,
			height: params.separatorRow.height,
			backgroundColor: params.separatorRow.backgroundColor,
		}
	);
	
	viewButton.add(separatorHorizontal);
	
	var leftButton = Ti.UI.createView(params.buttonStyle);
	leftButton.left = 0;
	leftButton.width = "50%";
	
	var labelCancel = Ti.UI.createLabel({
		height: Ti.UI.SIZE,
		width: Ti.UI.SIZE,
		text: "Отмена",
	});
	
	leftButton.add(labelCancel);
	
	leftButton.addEventListener("click",function(event)
		{
			
		}
	);
	
	var separatorVertical = Ti.UI.createView(
		{
			right: 0,
			width: params.separatorRow.height,
			height: params.buttonStyle.height,
			backgroundColor: params.separatorRow.backgroundColor,
		}
	);
	leftButton.add(separatorVertical);
	
	var rightButton = Ti.UI.createView(params.buttonStyle);
	rightButton.right = 0;
	rightButton.width = "50%";
	
	var labelOk = Ti.UI.createLabel({
		height: Ti.UI.SIZE,
		width: Ti.UI.SIZE,
		text: "Готово",
	});
	
	rightButton.addEventListener("click",function(event)
		{
			
			alert(returnDate);
		}
	);
	
	rightButton.add(labelOk);
	
	viewButton.add(leftButton);
	viewButton.add(rightButton);
	
	content.add(viewButton);
	
	return content;
}
