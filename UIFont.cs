using System.Collections.Generic;
using System.Text;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Font")]
[ExecuteInEditMode]
public class UIFont : MonoBehaviour
{
	public enum Alignment
	{
		Left,
		Center,
		Right
	}

	public enum SymbolStyle
	{
		None,
		Uncolored,
		Colored
	}

	[HideInInspector]
	[SerializeField]
	private UIAtlas mAtlas;

	private static CharacterInfo mChar;

	private List<Color> mColors = new List<Color>();

	[HideInInspector]
	[SerializeField]
	private Font mDynamicFont;

	[HideInInspector]
	[SerializeField]
	private float mDynamicFontOffset;

	[SerializeField]
	[HideInInspector]
	private int mDynamicFontSize = 16;

	[SerializeField]
	[HideInInspector]
	private FontStyle mDynamicFontStyle;

	[SerializeField]
	[HideInInspector]
	private BMFont mFont = new BMFont();

	[SerializeField]
	[HideInInspector]
	private Material mMat;

	[SerializeField]
	[HideInInspector]
	private float mPixelSize = 1f;

	private int mPMA = -1;

	[HideInInspector]
	[SerializeField]
	private UIFont mReplacement;

	[HideInInspector]
	[SerializeField]
	private int mSpacingX;

	[SerializeField]
	[HideInInspector]
	private int mSpacingY;

	private UIAtlas.Sprite mSprite;

	private bool mSpriteSet;

	[SerializeField]
	[HideInInspector]
	private List<BMSymbol> mSymbols = new List<BMSymbol>();

	[HideInInspector]
	[SerializeField]
	private Rect mUVRect = new Rect(0f, 0f, 1f, 1f);

	public UIAtlas atlas
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.atlas;
			}
			return this.mAtlas;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.atlas = value;
			}
			else
			{
				if (!(this.mAtlas != value))
				{
					return;
				}
				if (value == null)
				{
					if (this.mAtlas != null)
					{
						this.mMat = this.mAtlas.spriteMaterial;
					}
					if (this.sprite != null)
					{
						this.mUVRect = this.uvRect;
					}
				}
				this.mPMA = -1;
				this.mAtlas = value;
				this.MarkAsDirty();
			}
		}
	}

	public BMFont bmFont
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.bmFont;
			}
			return this.mFont;
		}
	}

	public Font dynamicFont
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.dynamicFont;
			}
			return this.mDynamicFont;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.dynamicFont = value;
			}
			else if (this.mDynamicFont != value)
			{
				if (this.mDynamicFont != null)
				{
					this.material = null;
				}
				this.mDynamicFont = value;
				this.MarkAsDirty();
			}
		}
	}

	public int dynamicFontSize
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.dynamicFontSize;
			}
			return this.mDynamicFontSize;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.dynamicFontSize = value;
				return;
			}
			value = Mathf.Clamp(value, 4, 128);
			if (this.mDynamicFontSize != value)
			{
				this.mDynamicFontSize = value;
				this.MarkAsDirty();
			}
		}
	}

	public FontStyle dynamicFontStyle
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.dynamicFontStyle;
			}
			return this.mDynamicFontStyle;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.dynamicFontStyle = value;
			}
			else if (this.mDynamicFontStyle != value)
			{
				this.mDynamicFontStyle = value;
				this.MarkAsDirty();
			}
		}
	}

	private Texture dynamicTexture
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.dynamicTexture;
			}
			if (this.isDynamic)
			{
				return this.mDynamicFont.material.mainTexture;
			}
			return null;
		}
	}

	public bool hasSymbols
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.hasSymbols;
			}
			return this.mSymbols.Count != 0;
		}
	}

	public int horizontalSpacing
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.horizontalSpacing;
			}
			return this.mSpacingX;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.horizontalSpacing = value;
			}
			else if (this.mSpacingX != value)
			{
				this.mSpacingX = value;
				this.MarkAsDirty();
			}
		}
	}

	public bool isDynamic => this.mDynamicFont != null;

	public bool isValid
	{
		get
		{
			if (!(this.mDynamicFont != null))
			{
				return this.mFont.isValid;
			}
			return true;
		}
	}

	public Material material
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.material;
			}
			if (this.mAtlas != null)
			{
				return this.mAtlas.spriteMaterial;
			}
			if (this.mMat != null)
			{
				if (this.mDynamicFont != null && this.mMat != this.mDynamicFont.material)
				{
					this.mMat.mainTexture = this.mDynamicFont.material.mainTexture;
				}
				return this.mMat;
			}
			if (this.mDynamicFont != null)
			{
				return this.mDynamicFont.material;
			}
			return null;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.material = value;
			}
			else if (this.mMat != value)
			{
				this.mPMA = -1;
				this.mMat = value;
				this.MarkAsDirty();
			}
		}
	}

	public float pixelSize
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.pixelSize;
			}
			if (this.mAtlas != null)
			{
				return this.mAtlas.pixelSize;
			}
			return this.mPixelSize;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.pixelSize = value;
				return;
			}
			if (this.mAtlas != null)
			{
				this.mAtlas.pixelSize = value;
				return;
			}
			float num = Mathf.Clamp(value, 0.25f, 4f);
			if (this.mPixelSize != num)
			{
				this.mPixelSize = num;
				this.MarkAsDirty();
			}
		}
	}

	public bool premultipliedAlpha
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.premultipliedAlpha;
			}
			if (this.mAtlas != null)
			{
				return this.mAtlas.premultipliedAlpha;
			}
			if (this.mPMA == -1)
			{
				Material material = this.material;
				this.mPMA = ((!(material == null) && !(material.shader == null) && material.shader.name.Contains("Premultiplied")) ? 1 : 0);
			}
			return this.mPMA == 1;
		}
	}

	public UIFont replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIFont uIFont = value;
			if (uIFont == this)
			{
				uIFont = null;
			}
			if (this.mReplacement != uIFont)
			{
				if (uIFont != null && uIFont.replacement == this)
				{
					uIFont.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsDirty();
				}
				this.mReplacement = uIFont;
				this.MarkAsDirty();
			}
		}
	}

	public int size
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.size;
			}
			if (this.isDynamic)
			{
				return this.mDynamicFontSize;
			}
			return this.mFont.charSize;
		}
	}

	public UIAtlas.Sprite sprite
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.sprite;
			}
			if (!this.mSpriteSet)
			{
				this.mSprite = null;
			}
			if (this.mSprite == null)
			{
				if (this.mAtlas != null && !string.IsNullOrEmpty(this.mFont.spriteName))
				{
					this.mSprite = this.mAtlas.GetSprite(this.mFont.spriteName);
					if (this.mSprite == null)
					{
						this.mSprite = this.mAtlas.GetSprite(base.name);
					}
					this.mSpriteSet = true;
					if (this.mSprite == null)
					{
						this.mFont.spriteName = null;
					}
				}
				int i = 0;
				for (int count = this.mSymbols.Count; i < count; i++)
				{
					this.symbols[i].MarkAsDirty();
				}
			}
			return this.mSprite;
		}
	}

	public string spriteName
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.spriteName;
			}
			return this.mFont.spriteName;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteName = value;
			}
			else if (this.mFont.spriteName != value)
			{
				this.mFont.spriteName = value;
				this.MarkAsDirty();
			}
		}
	}

	public List<BMSymbol> symbols
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.symbols;
			}
			return this.mSymbols;
		}
	}

	public int texHeight
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.texHeight;
			}
			if (this.mFont != null)
			{
				return this.mFont.texHeight;
			}
			return 1;
		}
	}

	public Texture2D texture
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.texture;
			}
			Material material = this.material;
			if (!(material == null))
			{
				return material.mainTexture as Texture2D;
			}
			return null;
		}
	}

	public int texWidth
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.texWidth;
			}
			if (this.mFont != null)
			{
				return this.mFont.texWidth;
			}
			return 1;
		}
	}

	public Rect uvRect
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.uvRect;
			}
			if (this.mAtlas != null && this.mSprite == null && this.sprite != null)
			{
				Texture texture = this.mAtlas.texture;
				if (texture != null)
				{
					this.mUVRect = this.mSprite.outer;
					if (this.mAtlas.coordinates == UIAtlas.Coordinates.Pixels)
					{
						this.mUVRect = NGUIMath.ConvertToTexCoords(this.mUVRect, texture.width, texture.height);
					}
					if (this.mSprite.hasPadding)
					{
						Rect rect = this.mUVRect;
						this.mUVRect.xMin = rect.xMin - this.mSprite.paddingLeft * rect.width;
						this.mUVRect.yMin = rect.yMin - this.mSprite.paddingBottom * rect.height;
						this.mUVRect.xMax = rect.xMax + this.mSprite.paddingRight * rect.width;
						this.mUVRect.yMax = rect.yMax + this.mSprite.paddingTop * rect.height;
					}
					if (this.mSprite.hasPadding)
					{
						this.Trim();
					}
				}
			}
			return this.mUVRect;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.uvRect = value;
			}
			else if (this.sprite == null && this.mUVRect != value)
			{
				this.mUVRect = value;
				this.MarkAsDirty();
			}
		}
	}

	public int verticalSpacing
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.verticalSpacing;
			}
			return this.mSpacingY;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.verticalSpacing = value;
			}
			else if (this.mSpacingY != value)
			{
				this.mSpacingY = value;
				this.MarkAsDirty();
			}
		}
	}

	public void AddSymbol(string sequence, string spriteName)
	{
		this.GetSymbol(sequence, createIfMissing: true).spriteName = spriteName;
		this.MarkAsDirty();
	}

	private void Align(BetterList<Vector3> verts, int indexOffset, Alignment alignment, int x, int lineWidth)
	{
		if (alignment == Alignment.Left)
		{
			return;
		}
		int num = this.size;
		if (num <= 0)
		{
			return;
		}
		float num2 = 0f;
		if (alignment == Alignment.Right)
		{
			num2 = Mathf.RoundToInt(lineWidth - x);
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			num2 /= (float)this.size;
		}
		else
		{
			num2 = Mathf.RoundToInt((float)(lineWidth - x) * 0.5f);
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			num2 /= (float)this.size;
			if ((lineWidth & 1) == 1)
			{
				num2 += 0.5f / (float)num;
			}
		}
		for (int i = indexOffset; i < verts.size; i++)
		{
			Vector3 vector = verts.buffer[i];
			vector.x += num2;
			verts.buffer[i] = vector;
		}
	}

	public Vector2 CalculatePrintedSize(string text, bool encoding, SymbolStyle symbolStyle)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.CalculatePrintedSize(text, encoding, symbolStyle);
		}
		Vector2 zero = Vector2.zero;
		bool flag = this.isDynamic;
		if (flag || (this.mFont != null && this.mFont.isValid && !string.IsNullOrEmpty(text)))
		{
			if (encoding)
			{
				text = NGUITools.StripSymbols(text);
			}
			if (flag)
			{
				this.mDynamicFont.textureRebuildCallback = OnFontChanged;
				this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize, this.mDynamicFontStyle);
				this.mDynamicFont.textureRebuildCallback = null;
			}
			int length = text.Length;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = this.size;
			int num6 = num5 + this.mSpacingY;
			bool flag2 = encoding && symbolStyle != 0 && this.hasSymbols;
			for (int i = 0; i < length; i++)
			{
				char c = text[i];
				if (c == '\n')
				{
					if (num2 > num)
					{
						num = num2;
					}
					num2 = 0;
					num3 += num6;
					num4 = 0;
				}
				else if (c < ' ')
				{
					num4 = 0;
				}
				else if (!flag)
				{
					BMSymbol bMSymbol = ((!flag2) ? null : this.MatchSymbol(text, i, length));
					if (bMSymbol == null)
					{
						BMGlyph glyph = this.mFont.GetGlyph(c);
						if (glyph != null)
						{
							num2 += this.mSpacingX + ((num4 == 0) ? glyph.advance : (glyph.advance + glyph.GetKerning(num4)));
							num4 = c;
						}
					}
					else
					{
						num2 += this.mSpacingX + bMSymbol.width;
						i += bMSymbol.length - 1;
						num4 = 0;
					}
				}
				else if (this.mDynamicFont.GetCharacterInfo(c, out UIFont.mChar, this.mDynamicFontSize, this.mDynamicFontStyle))
				{
					num2 += this.mSpacingX + (int)UIFont.mChar.width;
				}
			}
			float num7 = ((num5 <= 0) ? 1f : (1f / (float)num5));
			zero.x = num7 * ((num2 <= num) ? ((float)num) : ((float)num2));
			zero.y = num7 * (float)(num3 + num6);
		}
		return zero;
	}

	public static bool CheckIfRelated(UIFont a, UIFont b)
	{
		if (a == null || b == null)
		{
			return false;
		}
		if (!a.isDynamic || !b.isDynamic || !(a.dynamicFont.fontNames[0] == b.dynamicFont.fontNames[0]))
		{
			if (!(a == b) && !a.References(b))
			{
				return b.References(a);
			}
			return true;
		}
		return true;
	}

	private static void EndLine(ref StringBuilder s)
	{
		int num = s.Length - 1;
		if (num > 0 && s[num] == ' ')
		{
			s[num] = '\n';
		}
		else
		{
			s.Append('\n');
		}
	}

	public string GetEndOfLineThatFits(string text, float maxWidth, bool encoding, SymbolStyle symbolStyle)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetEndOfLineThatFits(text, maxWidth, encoding, symbolStyle);
		}
		int num = Mathf.RoundToInt(maxWidth * (float)this.size);
		if (num < 1)
		{
			return text;
		}
		int length = text.Length;
		int num2 = num;
		BMGlyph bMGlyph = null;
		int num3 = length;
		bool flag = encoding && symbolStyle != 0 && this.hasSymbols;
		bool flag2 = this.isDynamic;
		if (flag2)
		{
			this.mDynamicFont.textureRebuildCallback = OnFontChanged;
			this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize, this.mDynamicFontStyle);
			this.mDynamicFont.textureRebuildCallback = null;
		}
		while (num3 > 0 && num2 > 0)
		{
			char c = text[--num3];
			BMSymbol bMSymbol = ((!flag) ? null : this.MatchSymbol(text, num3, length));
			int num4 = this.mSpacingX;
			if (!flag2)
			{
				if (bMSymbol != null)
				{
					num4 += bMSymbol.advance;
				}
				else
				{
					BMGlyph glyph = this.mFont.GetGlyph(c);
					if (glyph == null)
					{
						bMGlyph = null;
						continue;
					}
					num4 += glyph.advance + (bMGlyph?.GetKerning(c) ?? 0);
					bMGlyph = glyph;
				}
			}
			else if (this.mDynamicFont.GetCharacterInfo(c, out UIFont.mChar, this.mDynamicFontSize, this.mDynamicFontStyle))
			{
				num4 += (int)UIFont.mChar.width;
			}
			num2 -= num4;
		}
		if (num2 < 0)
		{
			num3++;
		}
		return text.Substring(num3, length - num3);
	}

	private BMSymbol GetSymbol(string sequence, bool createIfMissing)
	{
		int i = 0;
		for (int count = this.mSymbols.Count; i < count; i++)
		{
			BMSymbol bMSymbol = this.mSymbols[i];
			if (bMSymbol.sequence == sequence)
			{
				return bMSymbol;
			}
		}
		if (createIfMissing)
		{
			BMSymbol bMSymbol2 = new BMSymbol
			{
				sequence = sequence
			};
			this.mSymbols.Add(bMSymbol2);
			return bMSymbol2;
		}
		return null;
	}

	public void MarkAsDirty()
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.MarkAsDirty();
		}
		this.RecalculateDynamicOffset();
		this.mSprite = null;
		UILabel[] array = NGUITools.FindActive<UILabel>();
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			UILabel uILabel = array[i];
			if (uILabel.enabled && NGUITools.GetActive(uILabel.gameObject) && UIFont.CheckIfRelated(this, uILabel.font))
			{
				UIFont font = uILabel.font;
				uILabel.font = null;
				uILabel.font = font;
			}
		}
		int j = 0;
		for (int count = this.mSymbols.Count; j < count; j++)
		{
			this.symbols[j].MarkAsDirty();
		}
	}

	private BMSymbol MatchSymbol(string text, int offset, int textLength)
	{
		int count = this.mSymbols.Count;
		if (count != 0)
		{
			textLength -= offset;
			for (int i = 0; i < count; i++)
			{
				BMSymbol bMSymbol = this.mSymbols[i];
				int length = bMSymbol.length;
				if (length == 0 || textLength < length)
				{
					continue;
				}
				bool flag = true;
				for (int j = 0; j < length; j++)
				{
					if (text[offset + j] != bMSymbol.sequence[j])
					{
						flag = false;
						break;
					}
				}
				if (flag && bMSymbol.Validate(this.atlas))
				{
					return bMSymbol;
				}
			}
		}
		return null;
	}

	private void OnFontChanged()
	{
		this.MarkAsDirty();
	}

	public void Print(string text, Color32 color, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, bool encoding, SymbolStyle symbolStyle, Alignment alignment, int lineWidth, bool premultiply)
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.Print(text, color, verts, uvs, cols, encoding, symbolStyle, alignment, lineWidth, premultiply);
		}
		else
		{
			if (text == null)
			{
				return;
			}
			if (!this.isValid)
			{
				Debug.LogError("Attempting to print using an invalid font!");
				return;
			}
			bool flag = this.isDynamic;
			if (flag)
			{
				this.mDynamicFont.textureRebuildCallback = OnFontChanged;
				this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize, this.mDynamicFontStyle);
				this.mDynamicFont.textureRebuildCallback = null;
			}
			this.mColors.Clear();
			this.mColors.Add(color);
			int num = this.size;
			Vector2 vector = ((num <= 0) ? Vector2.one : new Vector2(1f / (float)num, 1f / (float)num));
			int num2 = verts.size;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = num + this.mSpacingY;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			Vector2 zero3 = Vector2.zero;
			Vector2 zero4 = Vector2.zero;
			float num8 = this.uvRect.width / (float)this.mFont.texWidth;
			float num9 = this.mUVRect.height / (float)this.mFont.texHeight;
			int length = text.Length;
			bool flag2 = encoding && symbolStyle != 0 && this.hasSymbols && this.sprite != null;
			for (int i = 0; i < length; i++)
			{
				char c = text[i];
				if (c == '\n')
				{
					if (num4 > num3)
					{
						num3 = num4;
					}
					if (alignment != 0)
					{
						this.Align(verts, num2, alignment, num4, lineWidth);
						num2 = verts.size;
					}
					num4 = 0;
					num5 += num7;
					num6 = 0;
					continue;
				}
				if (c < ' ')
				{
					num6 = 0;
					continue;
				}
				if (encoding && c == '[')
				{
					int num10 = NGUITools.ParseSymbol(text, i, this.mColors, premultiply);
					if (num10 > 0)
					{
						color = this.mColors[this.mColors.Count - 1];
						i += num10 - 1;
						continue;
					}
				}
				if (!flag)
				{
					BMSymbol bMSymbol = ((!flag2) ? null : this.MatchSymbol(text, i, length));
					if (bMSymbol == null)
					{
						BMGlyph glyph = this.mFont.GetGlyph(c);
						if (glyph == null)
						{
							continue;
						}
						if (num6 != 0)
						{
							num4 += glyph.GetKerning(num6);
						}
						if (c == ' ')
						{
							num4 += this.mSpacingX + glyph.advance;
							num6 = c;
							continue;
						}
						zero.x = vector.x * (float)(num4 + glyph.offsetX);
						zero.y = (0f - vector.y) * (float)(num5 + glyph.offsetY);
						zero2.x = zero.x + vector.x * (float)glyph.width;
						zero2.y = zero.y - vector.y * (float)glyph.height;
						zero3.x = this.mUVRect.xMin + num8 * (float)glyph.x;
						zero3.y = this.mUVRect.yMax - num9 * (float)glyph.y;
						zero4.x = zero3.x + num8 * (float)glyph.width;
						zero4.y = zero3.y - num9 * (float)glyph.height;
						num4 += this.mSpacingX + glyph.advance;
						num6 = c;
						if (glyph.channel == 0 || glyph.channel == 15)
						{
							for (int j = 0; j < 4; j++)
							{
								cols.Add(color);
							}
						}
						else
						{
							Color color2 = color;
							color2 *= 0.49f;
							switch (glyph.channel)
							{
							case 1:
								color2.b += 0.51f;
								break;
							case 2:
								color2.g += 0.51f;
								break;
							case 4:
								color2.r += 0.51f;
								break;
							case 8:
								color2.a += 0.51f;
								break;
							}
							for (int k = 0; k < 4; k++)
							{
								cols.Add(color2);
							}
						}
					}
					else
					{
						zero.x = vector.x * (float)(num4 + bMSymbol.offsetX);
						zero.y = (0f - vector.y) * (float)(num5 + bMSymbol.offsetY);
						zero2.x = zero.x + vector.x * (float)bMSymbol.width;
						zero2.y = zero.y - vector.y * (float)bMSymbol.height;
						Rect rect = bMSymbol.uvRect;
						zero3.x = rect.xMin;
						zero3.y = rect.yMax;
						zero4.x = rect.xMax;
						zero4.y = rect.yMin;
						num4 += this.mSpacingX + bMSymbol.advance;
						i += bMSymbol.length - 1;
						num6 = 0;
						if (symbolStyle == SymbolStyle.Colored)
						{
							for (int l = 0; l < 4; l++)
							{
								cols.Add(color);
							}
						}
						else
						{
							Color32 item = Color.white;
							item.a = color.a;
							for (int m = 0; m < 4; m++)
							{
								cols.Add(item);
							}
						}
					}
					verts.Add(new Vector3(zero2.x, zero.y));
					verts.Add(new Vector3(zero2.x, zero2.y));
					verts.Add(new Vector3(zero.x, zero2.y));
					verts.Add(new Vector3(zero.x, zero.y));
					uvs.Add(new Vector2(zero4.x, zero3.y));
					uvs.Add(new Vector2(zero4.x, zero4.y));
					uvs.Add(new Vector2(zero3.x, zero4.y));
					uvs.Add(new Vector2(zero3.x, zero3.y));
				}
				else if (this.mDynamicFont.GetCharacterInfo(c, out UIFont.mChar, this.mDynamicFontSize, this.mDynamicFontStyle))
				{
					zero.x = vector.x * ((float)num4 + UIFont.mChar.vert.xMin);
					zero.y = (0f - vector.y) * ((float)num5 - UIFont.mChar.vert.yMax + this.mDynamicFontOffset);
					zero2.x = zero.x + vector.x * UIFont.mChar.vert.width;
					zero2.y = zero.y - vector.y * UIFont.mChar.vert.height;
					zero3.x = UIFont.mChar.uv.xMin;
					zero3.y = UIFont.mChar.uv.yMin;
					zero4.x = UIFont.mChar.uv.xMax;
					zero4.y = UIFont.mChar.uv.yMax;
					num4 += this.mSpacingX + (int)UIFont.mChar.width;
					for (int n = 0; n < 4; n++)
					{
						cols.Add(color);
					}
					if (UIFont.mChar.flipped)
					{
						uvs.Add(new Vector2(zero3.x, zero4.y));
						uvs.Add(new Vector2(zero3.x, zero3.y));
						uvs.Add(new Vector2(zero4.x, zero3.y));
						uvs.Add(new Vector2(zero4.x, zero4.y));
					}
					else
					{
						uvs.Add(new Vector2(zero4.x, zero3.y));
						uvs.Add(new Vector2(zero3.x, zero3.y));
						uvs.Add(new Vector2(zero3.x, zero4.y));
						uvs.Add(new Vector2(zero4.x, zero4.y));
					}
					verts.Add(new Vector3(zero2.x, zero.y));
					verts.Add(new Vector3(zero.x, zero.y));
					verts.Add(new Vector3(zero.x, zero2.y));
					verts.Add(new Vector3(zero2.x, zero2.y));
				}
			}
			if (alignment != 0 && num2 < verts.size)
			{
				this.Align(verts, num2, alignment, num4, lineWidth);
				num2 = verts.size;
			}
		}
	}

	public bool RecalculateDynamicOffset()
	{
		if (this.mDynamicFont != null)
		{
			this.mDynamicFont.RequestCharactersInTexture("j", this.mDynamicFontSize, this.mDynamicFontStyle);
			this.mDynamicFont.GetCharacterInfo('j', out var info, this.mDynamicFontSize, this.mDynamicFontStyle);
			float num = (float)this.mDynamicFontSize + info.vert.yMax;
			if (!object.Equals(this.mDynamicFontOffset, num))
			{
				this.mDynamicFontOffset = num;
				return true;
			}
		}
		return false;
	}

	private bool References(UIFont font)
	{
		if (font == null)
		{
			return false;
		}
		if (!(font == this))
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.References(font);
			}
			return false;
		}
		return true;
	}

	public void RemoveSymbol(string sequence)
	{
		BMSymbol symbol = this.GetSymbol(sequence, createIfMissing: false);
		if (symbol != null)
		{
			this.symbols.Remove(symbol);
		}
		this.MarkAsDirty();
	}

	public void RenameSymbol(string before, string after)
	{
		BMSymbol symbol = this.GetSymbol(before, createIfMissing: false);
		if (symbol != null)
		{
			symbol.sequence = after;
		}
		this.MarkAsDirty();
	}

	private void Trim()
	{
		Texture texture = this.mAtlas.texture;
		if (texture != null && this.mSprite != null)
		{
			Rect rect = NGUIMath.ConvertToPixels(this.mUVRect, this.texture.width, this.texture.height, round: true);
			Rect rect2 = ((this.mAtlas.coordinates != UIAtlas.Coordinates.TexCoords) ? this.mSprite.outer : NGUIMath.ConvertToPixels(this.mSprite.outer, texture.width, texture.height, round: true));
			int xMin = Mathf.RoundToInt(rect2.xMin - rect.xMin);
			int yMin = Mathf.RoundToInt(rect2.yMin - rect.yMin);
			int xMax = Mathf.RoundToInt(rect2.xMax - rect.xMin);
			int yMax = Mathf.RoundToInt(rect2.yMax - rect.yMin);
			this.mFont.Trim(xMin, yMin, xMax, yMax);
		}
	}

	public bool UsesSprite(string s)
	{
		if (!string.IsNullOrEmpty(s))
		{
			if (s.Equals(this.spriteName))
			{
				return true;
			}
			int i = 0;
			for (int count = this.symbols.Count; i < count; i++)
			{
				BMSymbol bMSymbol = this.symbols[i];
				if (s.Equals(bMSymbol.spriteName))
				{
					return true;
				}
			}
		}
		return false;
	}

	public string WrapText(string text, float maxWidth, int maxLineCount)
	{
		return this.WrapText(text, maxWidth, maxLineCount, encoding: false, SymbolStyle.None);
	}

	public string WrapText(string text, float maxWidth, int maxLineCount, bool encoding)
	{
		return this.WrapText(text, maxWidth, maxLineCount, encoding, SymbolStyle.None);
	}

	public string WrapText(string text, float maxWidth, int maxLineCount, bool encoding, SymbolStyle symbolStyle)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.WrapText(text, maxWidth, maxLineCount, encoding, symbolStyle);
		}
		int num = Mathf.RoundToInt(maxWidth * (float)this.size);
		if (num < 1)
		{
			return text;
		}
		StringBuilder s = new StringBuilder();
		int length = text.Length;
		int num2 = num;
		int num3 = 0;
		int i = 0;
		int j = 0;
		bool flag = true;
		bool flag2 = maxLineCount != 1;
		int num4 = 1;
		bool flag3 = encoding && symbolStyle != 0 && this.hasSymbols;
		bool flag4 = this.isDynamic;
		if (flag4)
		{
			this.mDynamicFont.textureRebuildCallback = OnFontChanged;
			this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize, this.mDynamicFontStyle);
			this.mDynamicFont.textureRebuildCallback = null;
		}
		for (; j < length; j++)
		{
			char c = text[j];
			if (c == '\n')
			{
				if (!flag2 || num4 == maxLineCount)
				{
					break;
				}
				num2 = num;
				if (i < j)
				{
					s.Append(text.Substring(i, j - i + 1));
				}
				else
				{
					s.Append(c);
				}
				flag = true;
				num4++;
				i = j + 1;
				num3 = 0;
				continue;
			}
			if (c == ' ' && num3 != 32 && i < j)
			{
				s.Append(text.Substring(i, j - i + 1));
				flag = false;
				i = j + 1;
				num3 = c;
			}
			if (encoding && c == '[' && j + 2 < length)
			{
				if (text[j + 1] == '-' && text[j + 2] == ']')
				{
					j += 2;
					continue;
				}
				if (j + 7 < length && text[j + 7] == ']' && NGUITools.EncodeColor(NGUITools.ParseColor(text, j + 1)) == text.Substring(j + 1, 6).ToUpper())
				{
					j += 7;
					continue;
				}
			}
			BMSymbol bMSymbol = ((!flag3) ? null : this.MatchSymbol(text, j, length));
			int num5 = this.mSpacingX;
			if (!flag4)
			{
				if (bMSymbol != null)
				{
					num5 += bMSymbol.advance;
				}
				else
				{
					BMGlyph bMGlyph = ((bMSymbol != null) ? null : this.mFont.GetGlyph(c));
					if (bMGlyph == null)
					{
						continue;
					}
					num5 += ((num3 == 0) ? bMGlyph.advance : (bMGlyph.advance + bMGlyph.GetKerning(num3)));
				}
			}
			else if (this.mDynamicFont.GetCharacterInfo(c, out UIFont.mChar, this.mDynamicFontSize, this.mDynamicFontStyle))
			{
				num5 += Mathf.RoundToInt(UIFont.mChar.width);
			}
			num2 -= num5;
			if (num2 < 0)
			{
				if (!flag && flag2 && num4 != maxLineCount)
				{
					for (; i < length && text[i] == ' '; i++)
					{
					}
					flag = true;
					num2 = num;
					j = i - 1;
					num3 = 0;
					if (!flag2 || num4 == maxLineCount)
					{
						break;
					}
					num4++;
					UIFont.EndLine(ref s);
					continue;
				}
				s.Append(text.Substring(i, Mathf.Max(0, j - i)));
				if (!flag2 || num4 == maxLineCount)
				{
					i = j;
					break;
				}
				UIFont.EndLine(ref s);
				flag = true;
				num4++;
				if (c == ' ')
				{
					i = j + 1;
					num2 = num;
				}
				else
				{
					i = j;
					num2 = num - num5;
				}
				num3 = 0;
			}
			else
			{
				num3 = c;
			}
			if (!flag4 && bMSymbol != null)
			{
				j += bMSymbol.length - 1;
				num3 = 0;
			}
		}
		if (i < j)
		{
			s.Append(text.Substring(i, j - i));
		}
		return s.ToString();
	}
}
