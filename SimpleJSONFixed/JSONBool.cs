using System.Text;

namespace SimpleJSONFixed;

public class JSONBool : JSONNode
{
	private bool m_Data;

	public override JSONNodeType Tag => JSONNodeType.Boolean;

	public override bool IsBoolean => true;

	public override string Value
	{
		get
		{
			return this.m_Data.ToString();
		}
		set
		{
			if (bool.TryParse(value, out var result))
			{
				this.m_Data = result;
			}
		}
	}

	public override bool AsBool
	{
		get
		{
			return this.m_Data;
		}
		set
		{
			this.m_Data = value;
		}
	}

	public override Enumerator GetEnumerator()
	{
		return default(Enumerator);
	}

	public JSONBool(bool aData)
	{
		this.m_Data = aData;
	}

	public JSONBool(string aData)
	{
		this.Value = aData;
	}

	public override JSONNode Clone()
	{
		return new JSONBool(this.m_Data);
	}

	internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
	{
		aSB.Append(this.m_Data ? "true" : "false");
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (obj is bool)
		{
			return this.m_Data == (bool)obj;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return this.m_Data.GetHashCode();
	}

	public override void Clear()
	{
		this.m_Data = false;
	}
}
