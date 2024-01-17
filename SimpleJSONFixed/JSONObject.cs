using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleJSONFixed;

public class JSONObject : JSONNode
{
	private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

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

	public override JSONNodeType Tag => JSONNodeType.Object;

	public override bool IsObject => true;

	public override JSONNode this[string aKey]
	{
		get
		{
			if (this.m_Dict.ContainsKey(aKey))
			{
				return this.m_Dict[aKey];
			}
			return new JSONLazyCreator(this, aKey);
		}
		set
		{
			if (value == null)
			{
				value = JSONNull.CreateOrGet();
			}
			if (this.m_Dict.ContainsKey(aKey))
			{
				this.m_Dict[aKey] = value;
			}
			else
			{
				this.m_Dict.Add(aKey, value);
			}
		}
	}

	public override JSONNode this[int aIndex]
	{
		get
		{
			if (aIndex < 0 || aIndex >= this.m_Dict.Count)
			{
				return null;
			}
			return this.m_Dict.ElementAt(aIndex).Value;
		}
		set
		{
			if (value == null)
			{
				value = JSONNull.CreateOrGet();
			}
			if (aIndex >= 0 && aIndex < this.m_Dict.Count)
			{
				string key = this.m_Dict.ElementAt(aIndex).Key;
				this.m_Dict[key] = value;
			}
		}
	}

	public override int Count => this.m_Dict.Count;

	public override IEnumerable<JSONNode> Children
	{
		get
		{
			foreach (KeyValuePair<string, JSONNode> item in this.m_Dict)
			{
				yield return item.Value;
			}
		}
	}

	public override Enumerator GetEnumerator()
	{
		return new Enumerator(this.m_Dict.GetEnumerator());
	}

	public override void Add(string aKey, JSONNode aItem)
	{
		if (aItem == null)
		{
			aItem = JSONNull.CreateOrGet();
		}
		if (aKey != null)
		{
			if (this.m_Dict.ContainsKey(aKey))
			{
				this.m_Dict[aKey] = aItem;
			}
			else
			{
				this.m_Dict.Add(aKey, aItem);
			}
		}
		else
		{
			this.m_Dict.Add(Guid.NewGuid().ToString(), aItem);
		}
	}

	public override JSONNode Remove(string aKey)
	{
		if (!this.m_Dict.ContainsKey(aKey))
		{
			return null;
		}
		JSONNode result = this.m_Dict[aKey];
		this.m_Dict.Remove(aKey);
		return result;
	}

	public override JSONNode Remove(int aIndex)
	{
		if (aIndex < 0 || aIndex >= this.m_Dict.Count)
		{
			return null;
		}
		KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.ElementAt(aIndex);
		this.m_Dict.Remove(keyValuePair.Key);
		return keyValuePair.Value;
	}

	public override JSONNode Remove(JSONNode aNode)
	{
		try
		{
			KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.Where((KeyValuePair<string, JSONNode> k) => k.Value == aNode).First();
			this.m_Dict.Remove(keyValuePair.Key);
			return aNode;
		}
		catch
		{
			return null;
		}
	}

	public override void Clear()
	{
		this.m_Dict.Clear();
	}

	public override JSONNode Clone()
	{
		JSONObject jSONObject = new JSONObject();
		foreach (KeyValuePair<string, JSONNode> item in this.m_Dict)
		{
			jSONObject.Add(item.Key, item.Value.Clone());
		}
		return jSONObject;
	}

	public override bool HasKey(string aKey)
	{
		return this.m_Dict.ContainsKey(aKey);
	}

	public override JSONNode GetValueOrDefault(string aKey, JSONNode aDefault)
	{
		if (this.m_Dict.TryGetValue(aKey, out var value))
		{
			return value;
		}
		return aDefault;
	}

	internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
	{
		aSB.Append('{');
		bool flag = true;
		if (this.inline)
		{
			aMode = JSONTextMode.Compact;
		}
		foreach (KeyValuePair<string, JSONNode> item in this.m_Dict)
		{
			if (!flag)
			{
				aSB.Append(',');
			}
			flag = false;
			if (aMode == JSONTextMode.Indent)
			{
				aSB.AppendLine();
			}
			if (aMode == JSONTextMode.Indent)
			{
				aSB.Append(' ', aIndent + aIndentInc);
			}
			aSB.Append('"').Append(JSONNode.Escape(item.Key)).Append('"');
			if (aMode == JSONTextMode.Compact)
			{
				aSB.Append(':');
			}
			else
			{
				aSB.Append(" : ");
			}
			item.Value.WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
		}
		if (aMode == JSONTextMode.Indent)
		{
			aSB.AppendLine().Append(' ', aIndent);
		}
		aSB.Append('}');
	}
}
