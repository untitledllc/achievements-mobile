//---------------------------------------------//

function Storage(name)
{
	this.name = 'storage.' + name;
	this.content = undefined;
	this.reload = function()
	{
		this.content = undefined;
		if(Ti.App.Properties.hasProperty(name) == true)
		{
			this.content = JSON.parse(Ti.App.Properties.getString(name));
		}
	};
	this.clear = function()
	{
		Ti.App.Properties.removeProperty(name);
	};
	this.flush = function()
	{
		Ti.App.Properties.setString(name, JSON.stringify(this.content));
	};
	this.reload();
	return this;
}

//---------------------------------------------//

module.exports = {
	create : function(params)
	{
		return new Storage(params);
	}
};
