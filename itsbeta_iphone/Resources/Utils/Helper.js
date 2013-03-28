function decorateButton(onSingleTap) {
	var me = this;
	
	me.addEventListener('touchstart', function() {
		me.backgroundImage = me.backgroundImage.replace('Normal', 'Pressed');		
	});
	
	me.addEventListener("singletap", onSingleTap);
	
	me.addEventListener('touchend', function() {
		me.backgroundImage = me.backgroundImage.replace('Pressed', 'Normal');
	});
}

function decorateNavbarButton(onSingleTap) {
	var me       = this,
		children = me.children;
	
	me.addEventListener('touchstart', function() {
		for(var key in children) {
			children[key].backgroundImage = children[key].backgroundImage.replace('Normal', 'Pressed');
		}	
	});
	
	me.addEventListener("singletap", onSingleTap);
	
	me.addEventListener('touchend', function() {
		for(var key in children) {
			children[key].backgroundImage = children[key].backgroundImage.replace('Pressed', 'Normal');
		}	
	});
}