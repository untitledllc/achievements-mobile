//---------------------------------------------//

function serialize(node)
{
	return JSON.stringify(node);
}

//---------------------------------------------//

function deserialize(string)
{
	return JSON.parse(string);
}

//---------------------------------------------//

module.exports = {
	serialize : serialize,
	deserialize : deserialize
};
