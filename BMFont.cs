using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BMFont
{
	[HideInInspector]
	[SerializeField]
	private int mBase;

	private Dictionary<int, BMGlyph> mDict = new Dictionary<int, BMGlyph>();

	[HideInInspector]
	[SerializeField]
	private int mHeight;

	[HideInInspector]
	[SerializeField]
	private List<BMGlyph> mSaved = new List<BMGlyph>();

	[HideInInspector]
	[SerializeField]
	private int mSize;

	[HideInInspector]
	[SerializeField]
	private string mSpriteName;

	[HideInInspector]
	[SerializeField]
	private int mWidth;

	public int baseOffset
	{
		get
		{
			return this.mBase;
		}
		set
		{
			this.mBase = value;
		}
	}

	public int charSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			this.mSize = value;
		}
	}

	public int glyphCount
	{
		get
		{
			if (this.isValid)
			{
				return this.mSaved.Count;
			}
			return 0;
		}
	}

	public bool isValid => this.mSaved.Count > 0;

	public string spriteName
	{
		get
		{
			return this.mSpriteName;
		}
		set
		{
			this.mSpriteName = value;
		}
	}

	public int texHeight
	{
		get
		{
			return this.mHeight;
		}
		set
		{
			this.mHeight = value;
		}
	}

	public int texWidth
	{
		get
		{
			return this.mWidth;
		}
		set
		{
			this.mWidth = value;
		}
	}

	public void Clear()
	{
		this.mDict.Clear();
		this.mSaved.Clear();
	}

	public BMGlyph GetGlyph(int index)
	{
		return this.GetGlyph(index, createIfMissing: false);
	}

	public BMGlyph GetGlyph(int index, bool createIfMissing)
	{
		BMGlyph value = null;
		if (this.mDict.Count == 0)
		{
			int i = 0;
			for (int count = this.mSaved.Count; i < count; i++)
			{
				BMGlyph bMGlyph = this.mSaved[i];
				this.mDict.Add(bMGlyph.index, bMGlyph);
			}
		}
		if (!this.mDict.TryGetValue(index, out value) && createIfMissing)
		{
			value = new BMGlyph
			{
				index = index
			};
			this.mSaved.Add(value);
			this.mDict.Add(index, value);
		}
		return value;
	}

	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		if (this.isValid)
		{
			int i = 0;
			for (int count = this.mSaved.Count; i < count; i++)
			{
				this.mSaved[i]?.Trim(xMin, yMin, xMax, yMax);
			}
		}
	}
}
