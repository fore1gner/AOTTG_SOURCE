using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Atlas")]
public class UIAtlas : MonoBehaviour
{
	public enum Coordinates
	{
		Pixels,
		TexCoords
	}

	[Serializable]
	public class Sprite
	{
		public Rect inner = new Rect(0f, 0f, 1f, 1f);

		public string name = "Unity Bug";

		public Rect outer = new Rect(0f, 0f, 1f, 1f);

		public float paddingBottom;

		public float paddingLeft;

		public float paddingRight;

		public float paddingTop;

		public bool rotated;

		public bool hasPadding
		{
			get
			{
				if (this.paddingLeft == 0f && this.paddingRight == 0f && this.paddingTop == 0f)
				{
					return this.paddingBottom != 0f;
				}
				return true;
			}
		}
	}

	[SerializeField]
	[HideInInspector]
	private Material material;

	[HideInInspector]
	[SerializeField]
	private Coordinates mCoordinates;

	[HideInInspector]
	[SerializeField]
	private float mPixelSize = 1f;

	private int mPMA = -1;

	[HideInInspector]
	[SerializeField]
	private UIAtlas mReplacement;

	[SerializeField]
	[HideInInspector]
	private List<Sprite> sprites = new List<Sprite>();

	public Coordinates coordinates
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.coordinates;
			}
			return this.mCoordinates;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.coordinates = value;
			}
			else
			{
				if (this.mCoordinates == value)
				{
					return;
				}
				if (this.material == null || this.material.mainTexture == null)
				{
					Debug.LogError("Can't switch coordinates until the atlas material has a valid texture");
					return;
				}
				this.mCoordinates = value;
				Texture mainTexture = this.material.mainTexture;
				int i = 0;
				for (int count = this.sprites.Count; i < count; i++)
				{
					Sprite sprite = this.sprites[i];
					if (this.mCoordinates == Coordinates.TexCoords)
					{
						sprite.outer = NGUIMath.ConvertToTexCoords(sprite.outer, mainTexture.width, mainTexture.height);
						sprite.inner = NGUIMath.ConvertToTexCoords(sprite.inner, mainTexture.width, mainTexture.height);
					}
					else
					{
						sprite.outer = NGUIMath.ConvertToPixels(sprite.outer, mainTexture.width, mainTexture.height, round: true);
						sprite.inner = NGUIMath.ConvertToPixels(sprite.inner, mainTexture.width, mainTexture.height, round: true);
					}
				}
			}
		}
	}

	public float pixelSize
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.pixelSize;
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
			if (this.mPMA == -1)
			{
				Material material = this.spriteMaterial;
				this.mPMA = ((!(material == null) && !(material.shader == null) && material.shader.name.Contains("Premultiplied")) ? 1 : 0);
			}
			return this.mPMA == 1;
		}
	}

	public UIAtlas replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIAtlas uIAtlas = value;
			if (uIAtlas == this)
			{
				uIAtlas = null;
			}
			if (this.mReplacement != uIAtlas)
			{
				if (uIAtlas != null && uIAtlas.replacement == this)
				{
					uIAtlas.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsDirty();
				}
				this.mReplacement = uIAtlas;
				this.MarkAsDirty();
			}
		}
	}

	public List<Sprite> spriteList
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.spriteList;
			}
			return this.sprites;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteList = value;
			}
			else
			{
				this.sprites = value;
			}
		}
	}

	public Material spriteMaterial
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.spriteMaterial;
			}
			return this.material;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteMaterial = value;
				return;
			}
			if (this.material == null)
			{
				this.mPMA = 0;
				this.material = value;
				return;
			}
			this.MarkAsDirty();
			this.mPMA = -1;
			this.material = value;
			this.MarkAsDirty();
		}
	}

	public Texture texture
	{
		get
		{
			if (!(this.mReplacement == null))
			{
				return this.mReplacement.texture;
			}
			if (!(this.material == null))
			{
				return this.material.mainTexture;
			}
			return null;
		}
	}

	public static bool CheckIfRelated(UIAtlas a, UIAtlas b)
	{
		if (a == null || b == null)
		{
			return false;
		}
		if (!(a == b) && !a.References(b))
		{
			return b.References(a);
		}
		return true;
	}

	private static int CompareString(string a, string b)
	{
		return a.CompareTo(b);
	}

	public BetterList<string> GetListOfSprites()
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetListOfSprites();
		}
		BetterList<string> betterList = new BetterList<string>();
		int i = 0;
		for (int count = this.sprites.Count; i < count; i++)
		{
			Sprite sprite = this.sprites[i];
			if (sprite != null && !string.IsNullOrEmpty(sprite.name))
			{
				betterList.Add(sprite.name);
			}
		}
		return betterList;
	}

	public BetterList<string> GetListOfSprites(string match)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetListOfSprites(match);
		}
		if (string.IsNullOrEmpty(match))
		{
			return this.GetListOfSprites();
		}
		BetterList<string> betterList = new BetterList<string>();
		int i = 0;
		for (int count = this.sprites.Count; i < count; i++)
		{
			Sprite sprite = this.sprites[i];
			if (sprite != null && !string.IsNullOrEmpty(sprite.name) && string.Equals(match, sprite.name, StringComparison.OrdinalIgnoreCase))
			{
				betterList.Add(sprite.name);
				return betterList;
			}
		}
		char[] separator = new char[1] { ' ' };
		string[] array = match.Split(separator, StringSplitOptions.RemoveEmptyEntries);
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = array[j].ToLower();
		}
		int k = 0;
		for (int count2 = this.sprites.Count; k < count2; k++)
		{
			Sprite sprite2 = this.sprites[k];
			if (sprite2 == null || string.IsNullOrEmpty(sprite2.name))
			{
				continue;
			}
			string text = sprite2.name.ToLower();
			int num = 0;
			for (int l = 0; l < array.Length; l++)
			{
				if (text.Contains(array[l]))
				{
					num++;
				}
			}
			if (num == array.Length)
			{
				betterList.Add(sprite2.name);
			}
		}
		return betterList;
	}

	public Sprite GetSprite(string name)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetSprite(name);
		}
		if (!string.IsNullOrEmpty(name))
		{
			int i = 0;
			for (int count = this.sprites.Count; i < count; i++)
			{
				Sprite sprite = this.sprites[i];
				if (!string.IsNullOrEmpty(sprite.name) && name == sprite.name)
				{
					return sprite;
				}
			}
		}
		return null;
	}

	public void MarkAsDirty()
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.MarkAsDirty();
		}
		UISprite[] array = NGUITools.FindActive<UISprite>();
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			UISprite uISprite = array[i];
			if (UIAtlas.CheckIfRelated(this, uISprite.atlas))
			{
				UIAtlas atlas = uISprite.atlas;
				uISprite.atlas = null;
				uISprite.atlas = atlas;
			}
		}
		UIFont[] array2 = Resources.FindObjectsOfTypeAll(typeof(UIFont)) as UIFont[];
		int j = 0;
		for (int num2 = array2.Length; j < num2; j++)
		{
			UIFont uIFont = array2[j];
			if (UIAtlas.CheckIfRelated(this, uIFont.atlas))
			{
				UIAtlas atlas2 = uIFont.atlas;
				uIFont.atlas = null;
				uIFont.atlas = atlas2;
			}
		}
		UILabel[] array3 = NGUITools.FindActive<UILabel>();
		int k = 0;
		for (int num3 = array3.Length; k < num3; k++)
		{
			UILabel uILabel = array3[k];
			if (uILabel.font != null && UIAtlas.CheckIfRelated(this, uILabel.font.atlas))
			{
				UIFont font = uILabel.font;
				uILabel.font = null;
				uILabel.font = font;
			}
		}
	}

	private bool References(UIAtlas atlas)
	{
		if (atlas == null)
		{
			return false;
		}
		if (!(atlas == this))
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.References(atlas);
			}
			return false;
		}
		return true;
	}
}
