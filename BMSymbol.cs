using System;
using UnityEngine;

[Serializable]
public class BMSymbol
{
	private int mAdvance;

	private int mHeight;

	private bool mIsValid;

	private int mLength;

	private int mOffsetX;

	private int mOffsetY;

	private UIAtlas.Sprite mSprite;

	private Rect mUV;

	private int mWidth;

	public string sequence;

	public string spriteName;

	public int advance => this.mAdvance;

	public int height => this.mHeight;

	public int length
	{
		get
		{
			if (this.mLength == 0)
			{
				this.mLength = this.sequence.Length;
			}
			return this.mLength;
		}
	}

	public int offsetX => this.mOffsetX;

	public int offsetY => this.mOffsetY;

	public Rect uvRect => this.mUV;

	public int width => this.mWidth;

	public void MarkAsDirty()
	{
		this.mIsValid = false;
	}

	public bool Validate(UIAtlas atlas)
	{
		if (atlas == null)
		{
			return false;
		}
		if (!this.mIsValid)
		{
			if (string.IsNullOrEmpty(this.spriteName))
			{
				return false;
			}
			this.mSprite = ((atlas == null) ? null : atlas.GetSprite(this.spriteName));
			if (this.mSprite != null)
			{
				Texture texture = atlas.texture;
				if (texture == null)
				{
					this.mSprite = null;
				}
				else
				{
					Rect rect = (this.mUV = this.mSprite.outer);
					if (atlas.coordinates == UIAtlas.Coordinates.Pixels)
					{
						this.mUV = NGUIMath.ConvertToTexCoords(this.mUV, texture.width, texture.height);
					}
					else
					{
						rect = NGUIMath.ConvertToPixels(rect, texture.width, texture.height, round: true);
					}
					this.mOffsetX = Mathf.RoundToInt(this.mSprite.paddingLeft * rect.width);
					this.mOffsetY = Mathf.RoundToInt(this.mSprite.paddingTop * rect.width);
					this.mWidth = Mathf.RoundToInt(rect.width);
					this.mHeight = Mathf.RoundToInt(rect.height);
					this.mAdvance = Mathf.RoundToInt(rect.width + (this.mSprite.paddingRight + this.mSprite.paddingLeft) * rect.width);
					this.mIsValid = true;
				}
			}
		}
		return this.mSprite != null;
	}
}
