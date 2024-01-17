using System.Collections.Generic;
using System.Text;

namespace SimpleJSONFixed;

public class JSONArray : JSONNode
{
	private List<JSONNode> m_List = new List<JSONNode>();

	private bool inline;

	public override bool Inline
	{
		get
		{
			return this.inline;
		}
		set
		{
			this.inline = value;
		}
	}

	public override JSONNodeType Tag => JSONNodeType.Array;

	public override bool IsArray => true;

	public override JSONNode this[int aIndex]
	{
		get
		{
			if (aIndex < 0 || aIndex >= this.m_List.Count)
			{
				return new JSONLazyCreator(this);
			}
			return this.m_List[aIndex];
		}
		set
		{
			if (value == null)
			{
				value = JSONNull.CreateOrGet();
			}
			if (aIndex < 0 || aIndex >= this.m_List.Count)
			{
				this.m_List.Add(value);
			}
			else
			{
				this.m_List[aIndex] = value;
			}
		}
	}

	public override JSONNode this[string aKey]
	{
		get
		{
			return new JSONLazyCreator(this);
		}
		set
		{
			if (value == null)
			{
				value = JSONNull.CreateOrGet();
			}
			this.m_List.Add(value);
		}
	}

	public override int Count => this.m_List.Count;

	public override IEnumerable<JSONNode> Children
	{
		get
		{
			foreach (JSONNode item in this.m_List)
			{
				yield return item;
			}
		}
	}

	public override Enumerator GetEnumerator()
	{
		return new Enumerator(this.m_List.GetEnumerator());
	}

	public override void Add(string aKey, JSONNode aItem)
	{
		if (aItem == null)
		{
			aItem = JSONNull.CreateOrGet();
		}
		this.m_List.Add(aItem);
	}

	public override JSONNode Remove(int aIndex)
	{
		if (aIndex < 0 || aIndex >= this.m_List.Count)
		{
			return null;
		}
		JSONNode result = this.m_List[aIndex];
		this.m_List.RemoveAt(aIndex);
		return result;
	}

	public override JSONNode Remove(JSONNode aNode)
	{
		this.m_List.Remove(aNode);
		return aNode;
	}

	public override void Clear()
	{
		this.m_List.Clear();
	}

	public override JSONNode Clone()
	{
		JSONArray jSONArray = new JSONArray();
		jSONArray.m_List.Capacity = this.m_List.Capacity;
		foreach (JSONNode item in this.m_List)
		{
			if (item != null)
			{
				jSONArray.Add(item.Clone());
			}
			else
			{
				jSONArray.Add(null);
			}
		}
		return jSONArray;
	}

	internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
	{
		aSB.Append('[');
		int count = this.m_List.Count;
		if (this.inline)
		{
			aMode = JSONTextMode.Compact;
		}
		for (int i = 0; i < count; i++)
		{
			if (i > 0)
			{
				aSB.Append(',');
			}
			if (aMode == JSONTextMode.Indent)
			{
				aSB.AppendLine();
			}
			if (aMode == JSONTextMode.Indent)
			{
				aSB.Append(' ', aIndent + aIndentInc);
			}
			this.m_List[i].WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
		}
		if (aMode == JSONTextMode.Indent)
		{
			aSB.AppendLine().Append(' ', aIndent);
		}
		aSB.Append(']');
	}
}
