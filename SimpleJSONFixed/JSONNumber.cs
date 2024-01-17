using System;
using System.Globalization;
using System.Text;

namespace SimpleJSONFixed;

public class JSONNumber : JSONNode
{
	private double m_Data;

	public override JSONNodeType Tag => JSONNodeType.Number;

	public override bool IsNumber => true;

	public override string Value
	{
		get
		{
			return this.m_Data.ToString(CultureInfo.InvariantCulture);
		}
		set
		{
			if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
			{
				this.m_Data = result;
			}
		}
	}

	public override double AsDouble
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

	public override long AsLong
	{
		get
		{
			return (long)this.m_Data;
		}
		set
		{
			this.m_Data = value;
		}
	}

	public override ulong AsULong
	{
		get
		{
			return (ulong)this.m_Data;
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

	public JSONNumber(double aData)
	{
		this.m_Data = aData;
	}

	public JSONNumber(string aData)
	{
		this.Value = aData;
	}

	public override JSONNode Clone()
	{
		return new JSONNumber(this.m_Data);
	}

	internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
	{
		aSB.Append(this.Value);
	}

	private static bool IsNumeric(object value)
	{
		if (!(value is int) && !(value is uint) && !(value is float) && !(value is double) && !(value is decimal) && !(value is long) && !(value is ulong) && !(value is short) && !(value is ushort) && !(value is sbyte))
		{
			return value is byte;
		}
		return true;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (base.Equals(obj))
		{
			return true;
		}
		JSONNumber jSONNumber = obj as JSONNumber;
		if (jSONNumber != null)
		{
			return this.m_Data == jSONNumber.m_Data;
		}
		if (JSONNumber.IsNumeric(obj))
		{
			return Convert.ToDouble(obj) == this.m_Data;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return this.m_Data.GetHashCode();
	}

	public override void Clear()
	{
		this.m_Data = 0.0;
	}
}
