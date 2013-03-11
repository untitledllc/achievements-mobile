var TiTools = require("TiTools/TiTools");

//---------------------------------------------//

TiTools.loadLibrary('TiTools/TiTools.String', 'String');

//---------------------------------------------//

function serialize(node)
{
	return '';
}

//---------------------------------------------//

function deserialize(string)
{
	var xml = Ti.XML.parseString(string);
	if(xml != undefined)
	{
		return deserializeNode(xml);
	}
	return undefined;
}

//---------------------------------------------//

function deserializeNode(node)
{
	var result = {
		name : node.nodeName,
		value : TiTools.String.trim(node.nodeValue),
		attributes : [],
		child : []
	};
	switch(node.nodeType)
	{
		case node.ELEMENT_NODE:
			var attributes = node.attributes;
			if(attributes != undefined)
			{
				for(var i = 0; i < attributes.length; i++)
				{
					var attribute = attributes.item(i);
					if(attribute != undefined)
					{
						result.attributes.push(
							{
								name : attribute.nodeName,
								value : attribute.nodeValue
							}
						);
					}
				}
			}
		break;
		default:
		break;
	}
	var child = node.firstChild;
	while(child != undefined)
	{
		switch(child.nodeType)
		{
			case child.TEXT_NODE:
				result.value += TiTools.String.trim(child.nodeValue);
			break;
			default:
				result.child.push(deserializeNode(child));
			break;
		}
		child = child.nextSibling;
	}
	return result;
}

//---------------------------------------------//

module.exports = {
	serialize : serialize,
	deserialize : deserialize,
	deserializeNode : deserializeNode 
};
