
/**
	@brief
		Создание модального окна с выбором даты
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
		callBack : function(pickerFrom,pickerTo,index){}, // функция срабатывает при изменении пикера. Index -  1 или 2 номер пикера (задается системно)
		pickerFrom: {  // параметры пикера "С"  ограничения и начальные значения
			
		},
		pickerTo: { // параметры пикера "По"  ограничения и начальные значения
			
		},
		buttonStyle: { // параметры кнопок "Отмена" и  "Готово"
		
		}
	}
	@events
	@methods
	@return
**/
function createModalWindowDateWith(params)
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
	
	////
	var viewButton = Ti.UI.createView(
		{
			top: titleRow.height + separatorTitle.height,
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
	
	var buttonFrom = Ti.UI.createView(params.buttonStyle);
	buttonFrom.backgroundColor = params.buttonFromStyle.backgroundColorSelect;
	buttonFrom.top = params.separatorRow.height;
	buttonFrom.left = 0;
	buttonFrom.width = "50%";
	
	var labelCancel = Ti.UI.createLabel({
		height: Ti.UI.SIZE,
		width: Ti.UI.SIZE,
		text: "C",
	});
	
	buttonFrom.add(labelCancel);
	
	buttonFrom.addEventListener("click",function(event)
		{
			buttonFrom.backgroundColor = params.buttonFromStyle.backgroundColorSelect;
			buttonTo.backgroundColor = params.buttonToStyle.backgroundColorNormal;
			picker.show();
			picker2.hide();
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
	buttonFrom.add(separatorVertical);
	
	var buttonTo = Ti.UI.createView(params.buttonStyle);
	buttonTo.backgroundColor = params.buttonToStyle.backgroundColorNormal;
	buttonTo.top = params.separatorRow.height;
	buttonTo.right = 0;
	buttonTo.width = "50%";
	
	var labelOk = Ti.UI.createLabel({
		height: Ti.UI.SIZE,
		width: Ti.UI.SIZE,
		text: "По",
	});
	
	buttonTo.addEventListener("click",function(event)
		{
			buttonFrom.backgroundColor = params.buttonFromStyle.backgroundColorNormal;
			buttonTo.backgroundColor = params.buttonToStyle.backgroundColorSelect;
			picker.hide();
			picker2.show();
		}
	);
	
	buttonTo.add(labelOk);
	
	viewButton.add(buttonTo);
	viewButton.add(buttonFrom);
	
	content.add(viewButton);
	////
	params.pickerFrom.type = Ti.UI.PICKER_TYPE_DATE;
	var picker = Ti.UI.createPicker(params.pickerFrom);
	
	var returnDate = picker.value.toLocaleString();
	
	picker.addEventListener('change',function(e){
		params.collBack(picker,picker2,1);
		returnDate = e.value.toLocaleString();
	});
	
	content.add(picker);
	params.pickerTo.type = Ti.UI.PICKER_TYPE_DATE;
	var picker2 = Ti.UI.createPicker(params.pickerTo);
	
	var returnDate2 = picker2.value.toLocaleString();
	
	picker2.addEventListener('change',function(e){
		params.collBack(picker,picker2,2);
		returnDate2 = e.value.toLocaleString();
	});
	
	picker2.hide();
	
	content.add(picker2);
	///
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
	leftButton.top = params.separatorRow.height;
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
	rightButton.top = params.separatorRow.height;
	rightButton.right = 0;
	rightButton.width = "50%";
	
	var labelOk = Ti.UI.createLabel({
		height: Ti.UI.SIZE,
		width: Ti.UI.SIZE,
		text: "Готово",
	});
	
	rightButton.addEventListener("click",function(event)
		{
			
			alert([returnDate,returnDate2]);
		}
	);
	
	rightButton.add(labelOk);
	
	viewButton.add(leftButton);
	viewButton.add(rightButton);
	
	content.add(viewButton);
	
	return content;
}