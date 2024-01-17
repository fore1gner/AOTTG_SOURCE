using System.Text;

namespace SimpleJSONFixed;

internal class JSONLazyCreator : JSONNode
{
	private JSONNode m_Node;

	private string m_Key;

	public override JSONNodeType Tag => JSONNodeType.None;

	public override JSONNode this[int aIndex]
	{
		get
		{
			return new JSONLazyCreator(this);
		}
		set
		{
			this.Set(new JSONArray()).Add(value);
		}
	}

	public override JSONNode this[string aKey]
	{
		get
		{
			return new JSONLazyCreator(this, aKey);
		}
		set
		{
			this.Set(new JSONObject()).Add(aKey, value);
		}
	}

	public override int AsInt
	{
		get
		{
			this.Set(new JSONNumber(0.0));
			return 0;
		}
		set
		{
			this.Set(new JSONNumber(value));
		}
	}

	public override float AsFloat
	{
		get
		{
			this.Set(new JSONNumber(0.0));
			return 0f;
		}
		set
		{
			this.Set(new JSONNumber(value));
		}
	}

	public override double AsDouble
	{
		get
		{
			this.Set(new JSONNumber(0.0));
			return 0.0;
		}
		set
		{
			this.Set(new JSONNumber(value));
		}
	}

	public override long AsLong
	{
		get
		{
			if (JSONNode.longAsString)
			{
				this.Set(new JSONString("0"));
			}
			else
			{
				this.Set(new JSONNumber(0.0));
			}
			return 0L;
		}
		set
		{
			if (JSONNode.longAsString)
			{
				this.Set(new JSONString(value.ToString()));
			}
			else
			{
				this.Set(new JSONNumber(value));
			}
		}
	}

	public override ulong AsULong
	{
		get
		{
			if (JSONNode.longAsString)
			{
				this.Set(new JSONString("0"));
			}
			else
			{
				this.Set(new JSONNumber(0.0));
			}
			return 0uL;
		}
		set
		{
			if (JSONNode.longAsString)
			{
				this.Set(new JSONString(value.ToString()));
			}
			else
			{
				this.Set(new JSONNumber(value));
			}
		}
	}

	public override bool AsBool
	{
		get
		{
			this.Set(new JSONBool(aData: false));
			return false;
		}
		set
		{
			this.Set(new JSONBool(value));
		}
	}

	public override JSONArray AsArray => this.Set(new JSONArray());

	public override JSONObject AsObject => this.Set(new JSONObject());

	public override Enumerator GetEnumerator()
	{
		return default(Enumerator);
	}

	public JSONLazyCreator(JSONNode aNode)
	{
		this.m_Node = aNode;
		this.m_Key = null;
	}

	public JSONLazyCreator(JSONNode aNode, string aKey)
	{
		this.m_Node = aNode;
		this.m_Key = aKey;
	}

	private T Set<T>(T aVal) where T : JSONNode
	{
		if (this.m_Key == null)
		{
			this.m_Node.Add(aVal);
		}
		else
		{
			this.m_Node.Add(this.m_Key, aVal);
		}
		this.m_Node = null;
		return aVal;
	}

	public override void Add(JSONNode aItem)
	{
		this.Set(new JSONArray()).Add(aItem);
	}

	public override void Add(string aKey, JSONNode aItem)
	{
		this.Set(new JSONObject()).Add(aKey, aItem);
	}

	public static bool operator ==(JSONLazyCreator a, object b)
	{
		if (b == null)
		{
			return true;
		}
		return (object)a == b;
	}

	public static bool operator !=(JSONLazyCreator a, object b)
	{
		return !(a == b);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return true;
		}
		return (object)this == obj;
	}

	public override int GetHashCode()
	{
		return 0;
	}

	internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
	{
		aSB.Append("null");
	}
}
