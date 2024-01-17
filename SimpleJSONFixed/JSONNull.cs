using System.Text;

namespace SimpleJSONFixed;

public class JSONNull : JSONNode
{
	private static JSONNull m_StaticInstance = new JSONNull();

	public static bool reuseSameInstance = true;

	public override JSONNodeType Tag => JSONNodeType.NullValue;

	public override bool IsNull => true;

	public override string Value
	{
		get
		{
			return "null";
		}
		set
		{
		}
	}

	public override bool AsBool
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static JSONNull CreateOrGet()
	{
		if (JSONNull.reuseSameInstance)
		{
			return JSONNull.m_StaticInstance;
		}
		return new JSONNull();
	}

	private JSONNull()
	{
	}

	public override Enumerator GetEnumerator()
	{
		return default(Enumerator);
	}

	public override JSONNode Clone()
	{
		return JSONNull.CreateOrGet();
	}

	public override bool Equals(object obj)
	{
		if ((object)this == obj)
		{
			return true;
		}
		return obj is JSONNull;
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
