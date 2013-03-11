/**
	@brief
		Создание модального окна выбор их списка
	@parent
		Ti.UI.View
	@param params : {
			view:{ // параметры окна (отступы, размер, цвет, прозрачность)
				
			},
			titleViewStyle: { // параметры шапки где находится заголовок
				
			},
			titleStyle:{ // параметры загаловка
				
			},
			separatorTitle:{ // Параметры разделителя после заголовка (гиризонтальный)
				height : px // обязательный параметр, по умолчанию ноль
			},
			separatorRow:{ // параметры разделитеся между ячейками (горизонтальный)
				height: px // высоты разделителя
			},
			selectImage:{ // вюшка показывающая выделенную ячейку (калочка)
				right : % || px // поумолчанию 5%
			},
			fieldPrint : // если передаете свой массив, тут указываете какое поле выводить на печать. По умолчанию ищет поле "name" 
			list[ : // Содержит массив элементов
				{
					name : String // имя
					id : int : // индификатор этой записи
				}
			],
			textStyle:{ //параметры текста в нутри ячейки
				
			},
			rowStyle:{ // параметры ячейки
				
			},
			buttonStyle: { // параметры кнопок Отмена и Готово
				
			}
		}
	@events
	@methods
	@return
**/
function createModalWindowList(params)
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
	
	var scrollView = Ti.UI.createScrollView(
		{
			top: heightOffset,
			bottom: params.buttonStyle.height,
			wigth: Ti.UI.Fill,
			height: Ti.UI.SIZE,
		}
	);
	
	content.add(scrollView);
	
	var selectImage = Ti.UI.createView(params.selectImage);
	if(selectImage.right == undefined)
	{
		selectImage.right = "5%";
	}
	
	heightOffset = 0;
	
	var field = params.fieldPrint;
	
	for(var i = 0; i < params.list.length; i++)
	{
		var row = Ti.UI.createView(params.rowStyle);
		row.top = heightOffset;
		var textRow = Ti.UI.createLabel(params.textStyle);
		var tempList = params.list[i];
		textRow.text = tempList[field];
		row.data = params.list[i];
		row.add(textRow);
		
		var separator = Ti.UI.createView(params.separatorRow);
		if(separator.bottom == undefined)
		{
			separator.bottom = 0;
		}
		row.add(separator);
		
		scrollView.add(row);
		heightOffset += row.height;
		
		row.addEventListener("click",function(event)
			{
				event.source.add(selectImage);
				selectImage.data = event.source.data;
			}
		);
	}
	
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
			alert(selectImage.data);
		}
	);
	
	rightButton.add(labelOk);
	
	viewButton.add(leftButton);
	viewButton.add(rightButton);
	
	content.add(viewButton);
	
	return content;
}