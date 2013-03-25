function decorateButton(onSingleTap)
{
	var me = this;
	
	me.addEventListener('touchstart', function()
	{
		me.backgroundImage = me.backgroundImage.replace('Normal', 'Pressed');		
	});
	
	me.addEventListener("singletap", onSingleTap);
	
	me.addEventListener('touchend', function()
	{
		me.backgroundImage = me.backgroundImage.replace('Pressed', 'Normal');
	});
}