using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Sprite")]
public class UISprite : UIWidget
{
	public enum FillDirection
	{
		Horizontal,
		Vertical,
		Radial90,
		Radial180,
		Radial360
	}

	public enum Type
	{
		Simple,
		Sliced,
		Tiled,
		Filled
	}

	[SerializeField]
	[HideInInspector]
	private UIAtlas mAtlas;

	[SerializeField]
	[HideInInspector]
	private float mFillAmount = 1f;

	[HideInInspector]
	[SerializeField]
	private bool mFillCenter = true;

	[SerializeField]
	[HideInInspector]
	private FillDirection mFillDirection = FillDirection.Radial360;

	protected Rect mInner;

	protected Rect mInnerUV;

	[SerializeField]
	[HideInInspector]
	private bool mInvert;

	protected Rect mOuter;

	protected Rect mOuterUV;

	protected Vector3 mScale = Vector3.one;

	protected UIAtlas.Sprite mSprite;

	[SerializeField]
	[HideInInspector]
	private string mSpriteName;

	private bool mSpriteSet;

	[HideInInspector]
	[SerializeField]
	private Type mType;

	public UIAtlas atlas
	{
		get
		{
			return this.mAtlas;
		}
		set
		{
			if (this.mAtlas != value)
			{
				this.mAtlas = value;
				this.mSpriteSet = false;
				this.mSprite = null;
				this.material = ((this.mAtlas == null) ? null : this.mAtlas.spriteMaterial);
				if (string.IsNullOrEmpty(this.mSpriteName) && this.mAtlas != null && this.mAtlas.spriteList.Count > 0)
				{
					this.SetAtlasSprite(this.mAtlas.spriteList[0]);
					this.mSpriteName = this.mSprite.name;
				}
				if (!string.IsNullOrEmpty(this.mSpriteName))
				{
					string text = this.mSpriteName;
					this.mSpriteName = string.Empty;
					this.spriteName = text;
					base.mChanged = true;
					this.UpdateUVs(force: true);
				}
			}
		}
	}

	public override Vector4 border
	{
		get
		{
			if (this.type != Type.Sliced)
			{
				return base.border;
			}
			UIAtlas.Sprite atlasSprite = this.GetAtlasSprite();
			if (atlasSprite == null)
			{
				return Vector2.zero;
			}
			Rect rect = atlasSprite.outer;
			Rect rect2 = atlasSprite.inner;
			Texture texture = this.mainTexture;
			if (this.atlas.coordinates == UIAtlas.Coordinates.TexCoords && texture != null)
			{
				rect = NGUIMath.ConvertToPixels(rect, texture.width, texture.height, round: true);
				rect2 = NGUIMath.ConvertToPixels(rect2, texture.width, texture.height, round: true);
			}
			return new Vector4(rect2.xMin - rect.xMin, rect2.yMin - rect.yMin, rect.xMax - rect2.xMax, rect.yMax - rect2.yMax) * this.atlas.pixelSize;
		}
	}

	public float fillAmount
	{
		get
		{
			return this.mFillAmount;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mFillAmount != num)
			{
				this.mFillAmount = num;
				base.mChanged = true;
			}
		}
	}

	public bool fillCenter
	{
		get
		{
			return this.mFillCenter;
		}
		set
		{
			if (this.mFillCenter != value)
			{
				this.mFillCenter = value;
				this.MarkAsChanged();
			}
		}
	}

	public FillDirection fillDirection
	{
		get
		{
			return this.mFillDirection;
		}
		set
		{
			if (this.mFillDirection != value)
			{
				this.mFillDirection = value;
				base.mChanged = true;
			}
		}
	}

	public Rect innerUV
	{
		get
		{
			this.UpdateUVs(force: false);
			return this.mInnerUV;
		}
	}

	public bool invert
	{
		get
		{
			return this.mInvert;
		}
		set
		{
			if (this.mInvert != value)
			{
				this.mInvert = value;
				base.mChanged = true;
			}
		}
	}

	public bool isValid => this.GetAtlasSprite() != null;

	public override Material material
	{
		get
		{
			Material material = base.material;
			if (material == null)
			{
				material = ((this.mAtlas == null) ? null : this.mAtlas.spriteMaterial);
				this.mSprite = null;
				this.material = material;
				if (material != null)
				{
					this.UpdateUVs(force: true);
				}
			}
			return material;
		}
	}

	public Rect outerUV
	{
		get
		{
			this.UpdateUVs(force: false);
			return this.mOuterUV;
		}
	}

	public override bool pixelPerfectAfterResize => this.type == Type.Sliced;

	public override Vector4 relativePadding
	{
		get
		{
			if (this.isValid && this.type == Type.Simple)
			{
				return new Vector4(this.mSprite.paddingLeft, this.mSprite.paddingTop, this.mSprite.paddingRight, this.mSprite.paddingBottom);
			}
			return base.relativePadding;
		}
	}

	public string spriteName
	{
		get
		{
			return this.mSpriteName;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(this.mSpriteName))
				{
					this.mSpriteName = string.Empty;
					this.mSprite = null;
					base.mChanged = true;
					this.mSpriteSet = false;
				}
			}
			else if (this.mSpriteName != value)
			{
				this.mSpriteName = value;
				this.mSprite = null;
				base.mChanged = true;
				this.mSpriteSet = false;
				if (this.isValid)
				{
					this.UpdateUVs(force: true);
				}
			}
		}
	}

	public virtual Type type
	{
		get
		{
			return this.mType;
		}
		set
		{
			if (this.mType != value)
			{
				this.mType = value;
				this.MarkAsChanged();
			}
		}
	}

	protected bool AdjustRadial(Vector2[] xy, Vector2[] uv, float fill, bool invert)
	{
		if (fill < 0.001f)
		{
			return false;
		}
		if (invert || fill <= 0.999f)
		{
			float num = Mathf.Clamp01(fill);
			if (!invert)
			{
				num = 1f - num;
			}
			num *= 1.570796f;
			float num2 = Mathf.Sin(num);
			float num3 = Mathf.Cos(num);
			if (num2 > num3)
			{
				num3 *= 1f / num2;
				num2 = 1f;
				if (!invert)
				{
					xy[0].y = Mathf.Lerp(xy[2].y, xy[0].y, num3);
					xy[3].y = xy[0].y;
					uv[0].y = Mathf.Lerp(uv[2].y, uv[0].y, num3);
					uv[3].y = uv[0].y;
				}
			}
			else if (num3 > num2)
			{
				num2 *= 1f / num3;
				num3 = 1f;
				if (invert)
				{
					xy[0].x = Mathf.Lerp(xy[2].x, xy[0].x, num2);
					xy[1].x = xy[0].x;
					uv[0].x = Mathf.Lerp(uv[2].x, uv[0].x, num2);
					uv[1].x = uv[0].x;
				}
			}
			else
			{
				num2 = 1f;
				num3 = 1f;
			}
			if (invert)
			{
				xy[1].y = Mathf.Lerp(xy[2].y, xy[0].y, num3);
				uv[1].y = Mathf.Lerp(uv[2].y, uv[0].y, num3);
			}
			else
			{
				xy[3].x = Mathf.Lerp(xy[2].x, xy[0].x, num2);
				uv[3].x = Mathf.Lerp(uv[2].x, uv[0].x, num2);
			}
		}
		return true;
	}

	protected void FilledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		float x = 0f;
		float y = 0f;
		float num = 1f;
		float num2 = -1f;
		float num3 = this.mOuterUV.xMin;
		float num4 = this.mOuterUV.yMin;
		float num5 = this.mOuterUV.xMax;
		float num6 = this.mOuterUV.yMax;
		if (this.mFillDirection == FillDirection.Horizontal || this.mFillDirection == FillDirection.Vertical)
		{
			float num7 = (num5 - num3) * this.mFillAmount;
			float num8 = (num6 - num4) * this.mFillAmount;
			if (this.fillDirection == FillDirection.Horizontal)
			{
				if (this.mInvert)
				{
					x = 1f - this.mFillAmount;
					num3 = num5 - num7;
				}
				else
				{
					num *= this.mFillAmount;
					num5 = num3 + num7;
				}
			}
			else if (this.fillDirection == FillDirection.Vertical)
			{
				if (this.mInvert)
				{
					num2 *= this.mFillAmount;
					num4 = num6 - num8;
				}
				else
				{
					y = 0f - (1f - this.mFillAmount);
					num6 = num4 + num8;
				}
			}
		}
		Vector2[] array = new Vector2[4];
		Vector2[] array2 = new Vector2[4];
		array[0] = new Vector2(num, y);
		array[1] = new Vector2(num, num2);
		array[2] = new Vector2(x, num2);
		array[3] = new Vector2(x, y);
		array2[0] = new Vector2(num5, num6);
		array2[1] = new Vector2(num5, num4);
		array2[2] = new Vector2(num3, num4);
		array2[3] = new Vector2(num3, num6);
		Color color = base.color;
		color.a *= base.mPanel.alpha;
		Color32 item = ((!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color));
		if (this.fillDirection == FillDirection.Radial90)
		{
			if (!this.AdjustRadial(array, array2, this.mFillAmount, this.mInvert))
			{
				return;
			}
		}
		else
		{
			if (this.fillDirection == FillDirection.Radial180)
			{
				Vector2[] array3 = new Vector2[4];
				Vector2[] array4 = new Vector2[4];
				for (int i = 0; i < 2; i++)
				{
					array3[0] = new Vector2(0f, 0f);
					array3[1] = new Vector2(0f, 1f);
					array3[2] = new Vector2(1f, 1f);
					array3[3] = new Vector2(1f, 0f);
					array4[0] = new Vector2(0f, 0f);
					array4[1] = new Vector2(0f, 1f);
					array4[2] = new Vector2(1f, 1f);
					array4[3] = new Vector2(1f, 0f);
					if (this.mInvert)
					{
						if (i > 0)
						{
							this.Rotate(array3, i);
							this.Rotate(array4, i);
						}
					}
					else if (i < 1)
					{
						this.Rotate(array3, 1 - i);
						this.Rotate(array4, 1 - i);
					}
					float from;
					float to;
					if (i == 1)
					{
						from = ((!this.mInvert) ? 1f : 0.5f);
						to = ((!this.mInvert) ? 0.5f : 1f);
					}
					else
					{
						from = ((!this.mInvert) ? 0.5f : 1f);
						to = ((!this.mInvert) ? 1f : 0.5f);
					}
					array3[1].y = Mathf.Lerp(from, to, array3[1].y);
					array3[2].y = Mathf.Lerp(from, to, array3[2].y);
					array4[1].y = Mathf.Lerp(from, to, array4[1].y);
					array4[2].y = Mathf.Lerp(from, to, array4[2].y);
					float fill = this.mFillAmount * 2f - (float)i;
					bool flag = i % 2 == 1;
					if (!this.AdjustRadial(array3, array4, fill, !flag))
					{
						continue;
					}
					if (this.mInvert)
					{
						flag = !flag;
					}
					if (flag)
					{
						for (int j = 0; j < 4; j++)
						{
							from = Mathf.Lerp(array[0].x, array[2].x, array3[j].x);
							to = Mathf.Lerp(array[0].y, array[2].y, array3[j].y);
							float x2 = Mathf.Lerp(array2[0].x, array2[2].x, array4[j].x);
							float y2 = Mathf.Lerp(array2[0].y, array2[2].y, array4[j].y);
							verts.Add(new Vector3(from, to, 0f));
							uvs.Add(new Vector2(x2, y2));
							cols.Add(item);
						}
						continue;
					}
					for (int num9 = 3; num9 > -1; num9--)
					{
						from = Mathf.Lerp(array[0].x, array[2].x, array3[num9].x);
						to = Mathf.Lerp(array[0].y, array[2].y, array3[num9].y);
						float x3 = Mathf.Lerp(array2[0].x, array2[2].x, array4[num9].x);
						float y3 = Mathf.Lerp(array2[0].y, array2[2].y, array4[num9].y);
						verts.Add(new Vector3(from, to, 0f));
						uvs.Add(new Vector2(x3, y3));
						cols.Add(item);
					}
				}
				return;
			}
			if (this.fillDirection == FillDirection.Radial360)
			{
				float[] array5 = new float[16]
				{
					0.5f, 1f, 0f, 0.5f, 0.5f, 1f, 0.5f, 1f, 0f, 0.5f,
					0.5f, 1f, 0f, 0.5f, 0f, 0.5f
				};
				Vector2[] array6 = new Vector2[4];
				Vector2[] array7 = new Vector2[4];
				for (int k = 0; k < 4; k++)
				{
					array6[0] = new Vector2(0f, 0f);
					array6[1] = new Vector2(0f, 1f);
					array6[2] = new Vector2(1f, 1f);
					array6[3] = new Vector2(1f, 0f);
					array7[0] = new Vector2(0f, 0f);
					array7[1] = new Vector2(0f, 1f);
					array7[2] = new Vector2(1f, 1f);
					array7[3] = new Vector2(1f, 0f);
					if (this.mInvert)
					{
						if (k > 0)
						{
							this.Rotate(array6, k);
							this.Rotate(array7, k);
						}
					}
					else if (k < 3)
					{
						this.Rotate(array6, 3 - k);
						this.Rotate(array7, 3 - k);
					}
					for (int l = 0; l < 4; l++)
					{
						int num10 = ((!this.mInvert) ? (k * 4) : ((3 - k) * 4));
						float from2 = array5[num10];
						float to2 = array5[num10 + 1];
						float from3 = array5[num10 + 2];
						float to3 = array5[num10 + 3];
						array6[l].x = Mathf.Lerp(from2, to2, array6[l].x);
						array6[l].y = Mathf.Lerp(from3, to3, array6[l].y);
						array7[l].x = Mathf.Lerp(from2, to2, array7[l].x);
						array7[l].y = Mathf.Lerp(from3, to3, array7[l].y);
					}
					float fill2 = this.mFillAmount * 4f - (float)k;
					bool flag2 = k % 2 == 1;
					if (!this.AdjustRadial(array6, array7, fill2, !flag2))
					{
						continue;
					}
					if (this.mInvert)
					{
						flag2 = !flag2;
					}
					if (flag2)
					{
						for (int m = 0; m < 4; m++)
						{
							float x4 = Mathf.Lerp(array[0].x, array[2].x, array6[m].x);
							float y4 = Mathf.Lerp(array[0].y, array[2].y, array6[m].y);
							float x5 = Mathf.Lerp(array2[0].x, array2[2].x, array7[m].x);
							float y5 = Mathf.Lerp(array2[0].y, array2[2].y, array7[m].y);
							verts.Add(new Vector3(x4, y4, 0f));
							uvs.Add(new Vector2(x5, y5));
							cols.Add(item);
						}
						continue;
					}
					for (int num11 = 3; num11 > -1; num11--)
					{
						float x6 = Mathf.Lerp(array[0].x, array[2].x, array6[num11].x);
						float y6 = Mathf.Lerp(array[0].y, array[2].y, array6[num11].y);
						float x7 = Mathf.Lerp(array2[0].x, array2[2].x, array7[num11].x);
						float y7 = Mathf.Lerp(array2[0].y, array2[2].y, array7[num11].y);
						verts.Add(new Vector3(x6, y6, 0f));
						uvs.Add(new Vector2(x7, y7));
						cols.Add(item);
					}
				}
				return;
			}
		}
		for (int n = 0; n < 4; n++)
		{
			verts.Add(array[n]);
			uvs.Add(array2[n]);
			cols.Add(item);
		}
	}

	public UIAtlas.Sprite GetAtlasSprite()
	{
		if (!this.mSpriteSet)
		{
			this.mSprite = null;
		}
		if (this.mSprite == null && this.mAtlas != null)
		{
			if (!string.IsNullOrEmpty(this.mSpriteName))
			{
				UIAtlas.Sprite sprite = this.mAtlas.GetSprite(this.mSpriteName);
				if (sprite == null)
				{
					return null;
				}
				this.SetAtlasSprite(sprite);
			}
			if (this.mSprite == null && this.mAtlas.spriteList.Count > 0)
			{
				UIAtlas.Sprite sprite2 = this.mAtlas.spriteList[0];
				if (sprite2 == null)
				{
					return null;
				}
				this.SetAtlasSprite(sprite2);
				if (this.mSprite == null)
				{
					Debug.LogError(this.mAtlas.name + " seems to have a null sprite!");
					return null;
				}
				this.mSpriteName = this.mSprite.name;
			}
			if (this.mSprite != null)
			{
				this.material = this.mAtlas.spriteMaterial;
				this.UpdateUVs(force: true);
			}
		}
		return this.mSprite;
	}

	public override void MakePixelPerfect()
	{
		if (!this.isValid)
		{
			return;
		}
		this.UpdateUVs(force: false);
		switch (this.type)
		{
		case Type.Sliced:
		{
			Vector3 localPosition = base.cachedTransform.localPosition;
			localPosition.x = Mathf.RoundToInt(localPosition.x);
			localPosition.y = Mathf.RoundToInt(localPosition.y);
			localPosition.z = Mathf.RoundToInt(localPosition.z);
			base.cachedTransform.localPosition = localPosition;
			Vector3 localScale = base.cachedTransform.localScale;
			localScale.x = Mathf.RoundToInt(localScale.x * 0.5f) << 1;
			localScale.y = Mathf.RoundToInt(localScale.y * 0.5f) << 1;
			localScale.z = 1f;
			base.cachedTransform.localScale = localScale;
			return;
		}
		case Type.Tiled:
		{
			Vector3 localPosition2 = base.cachedTransform.localPosition;
			localPosition2.x = Mathf.RoundToInt(localPosition2.x);
			localPosition2.y = Mathf.RoundToInt(localPosition2.y);
			localPosition2.z = Mathf.RoundToInt(localPosition2.z);
			base.cachedTransform.localPosition = localPosition2;
			Vector3 localScale2 = base.cachedTransform.localScale;
			localScale2.x = Mathf.RoundToInt(localScale2.x);
			localScale2.y = Mathf.RoundToInt(localScale2.y);
			localScale2.z = 1f;
			base.cachedTransform.localScale = localScale2;
			return;
		}
		}
		Texture texture = this.mainTexture;
		Vector3 localScale3 = base.cachedTransform.localScale;
		if (texture != null)
		{
			Rect rect = NGUIMath.ConvertToPixels(this.outerUV, texture.width, texture.height, round: true);
			float pixelSize = this.atlas.pixelSize;
			localScale3.x = (float)Mathf.RoundToInt(rect.width * pixelSize) * Mathf.Sign(localScale3.x);
			localScale3.y = (float)Mathf.RoundToInt(rect.height * pixelSize) * Mathf.Sign(localScale3.y);
			localScale3.z = 1f;
			base.cachedTransform.localScale = localScale3;
		}
		int num = Mathf.RoundToInt(Mathf.Abs(localScale3.x) * (1f + this.mSprite.paddingLeft + this.mSprite.paddingRight));
		int num2 = Mathf.RoundToInt(Mathf.Abs(localScale3.y) * (1f + this.mSprite.paddingTop + this.mSprite.paddingBottom));
		Vector3 localPosition3 = base.cachedTransform.localPosition;
		localPosition3.x = Mathf.CeilToInt(localPosition3.x * 4f) >> 2;
		localPosition3.y = Mathf.CeilToInt(localPosition3.y * 4f) >> 2;
		localPosition3.z = Mathf.RoundToInt(localPosition3.z);
		if (num % 2 == 1 && (base.pivot == Pivot.Top || base.pivot == Pivot.Center || base.pivot == Pivot.Bottom))
		{
			localPosition3.x += 0.5f;
		}
		if (num2 % 2 == 1 && (base.pivot == Pivot.Left || base.pivot == Pivot.Center || base.pivot == Pivot.Right))
		{
			localPosition3.y += 0.5f;
		}
		base.cachedTransform.localPosition = localPosition3;
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		switch (this.type)
		{
		case Type.Simple:
			this.SimpleFill(verts, uvs, cols);
			break;
		case Type.Sliced:
			this.SlicedFill(verts, uvs, cols);
			break;
		case Type.Tiled:
			this.TiledFill(verts, uvs, cols);
			break;
		case Type.Filled:
			this.FilledFill(verts, uvs, cols);
			break;
		}
	}

	protected override void OnStart()
	{
		if (this.mAtlas != null)
		{
			this.UpdateUVs(force: true);
		}
	}

	protected void Rotate(Vector2[] v, int offset)
	{
		for (int i = 0; i < offset; i++)
		{
			Vector2 vector = new Vector2(v[3].x, v[3].y);
			v[3].x = v[2].y;
			v[3].y = v[2].x;
			v[2].x = v[1].y;
			v[2].y = v[1].x;
			v[1].x = v[0].y;
			v[1].y = v[0].x;
			v[0].x = vector.y;
			v[0].y = vector.x;
		}
	}

	protected void SetAtlasSprite(UIAtlas.Sprite sp)
	{
		base.mChanged = true;
		this.mSpriteSet = true;
		if (sp != null)
		{
			this.mSprite = sp;
			this.mSpriteName = this.mSprite.name;
		}
		else
		{
			this.mSpriteName = ((this.mSprite == null) ? string.Empty : this.mSprite.name);
			this.mSprite = sp;
		}
	}

	protected void SimpleFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Vector2 item = new Vector2(this.mOuterUV.xMin, this.mOuterUV.yMin);
		Vector2 item2 = new Vector2(this.mOuterUV.xMax, this.mOuterUV.yMax);
		verts.Add(new Vector3(1f, 0f, 0f));
		verts.Add(new Vector3(1f, -1f, 0f));
		verts.Add(new Vector3(0f, -1f, 0f));
		verts.Add(new Vector3(0f, 0f, 0f));
		uvs.Add(item2);
		uvs.Add(new Vector2(item2.x, item.y));
		uvs.Add(item);
		uvs.Add(new Vector2(item.x, item2.y));
		Color color = base.color;
		color.a *= base.mPanel.alpha;
		Color32 item3 = ((!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color));
		cols.Add(item3);
		cols.Add(item3);
		cols.Add(item3);
		cols.Add(item3);
	}

	protected void SlicedFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (this.mOuterUV == this.mInnerUV)
		{
			this.SimpleFill(verts, uvs, cols);
			return;
		}
		Vector2[] array = new Vector2[4];
		Vector2[] array2 = new Vector2[4];
		Texture texture = this.mainTexture;
		array[0] = Vector2.zero;
		array[1] = Vector2.zero;
		array[2] = new Vector2(1f, -1f);
		array[3] = new Vector2(1f, -1f);
		if (texture == null)
		{
			for (int i = 0; i < 4; i++)
			{
				array2[i] = Vector2.zero;
			}
		}
		else
		{
			float pixelSize = this.atlas.pixelSize;
			float num = (this.mInnerUV.xMin - this.mOuterUV.xMin) * pixelSize;
			float num2 = (this.mOuterUV.xMax - this.mInnerUV.xMax) * pixelSize;
			float num3 = (this.mInnerUV.yMax - this.mOuterUV.yMax) * pixelSize;
			float num4 = (this.mOuterUV.yMin - this.mInnerUV.yMin) * pixelSize;
			Vector3 localScale = base.cachedTransform.localScale;
			localScale.x = Mathf.Max(0f, localScale.x);
			localScale.y = Mathf.Max(0f, localScale.y);
			Vector2 vector = new Vector2(localScale.x / (float)texture.width, localScale.y / (float)texture.height);
			Vector2 vector2 = new Vector2(num / vector.x, num3 / vector.y);
			Vector2 vector3 = new Vector2(num2 / vector.x, num4 / vector.y);
			Pivot pivot = base.pivot;
			if (pivot == Pivot.TopRight || pivot == Pivot.Right || pivot == Pivot.BottomRight)
			{
				array[0].x = Mathf.Min(0f, 1f - (vector3.x + vector2.x));
				array[1].x = array[0].x + vector2.x;
				array[2].x = array[0].x + Mathf.Max(vector2.x, 1f - vector3.x);
				array[3].x = array[0].x + Mathf.Max(vector2.x + vector3.x, 1f);
			}
			else
			{
				array[1].x = vector2.x;
				array[2].x = Mathf.Max(vector2.x, 1f - vector3.x);
				array[3].x = Mathf.Max(vector2.x + vector3.x, 1f);
			}
			if ((uint)(pivot - 6) <= 2u)
			{
				array[0].y = Mathf.Max(0f, -1f - (vector3.y + vector2.y));
				array[1].y = array[0].y + vector2.y;
				array[2].y = array[0].y + Mathf.Min(vector2.y, -1f - vector3.y);
				array[3].y = array[0].y + Mathf.Min(vector2.y + vector3.y, -1f);
			}
			else
			{
				array[1].y = vector2.y;
				array[2].y = Mathf.Min(vector2.y, -1f - vector3.y);
				array[3].y = Mathf.Min(vector2.y + vector3.y, -1f);
			}
			array2[0] = new Vector2(this.mOuterUV.xMin, this.mOuterUV.yMax);
			array2[1] = new Vector2(this.mInnerUV.xMin, this.mInnerUV.yMax);
			array2[2] = new Vector2(this.mInnerUV.xMax, this.mInnerUV.yMin);
			array2[3] = new Vector2(this.mOuterUV.xMax, this.mOuterUV.yMin);
		}
		Color color = base.color;
		color.a *= base.mPanel.alpha;
		Color32 item = ((!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color));
		for (int j = 0; j < 3; j++)
		{
			int num5 = j + 1;
			for (int k = 0; k < 3; k++)
			{
				if (this.mFillCenter || j != 1 || k != 1)
				{
					int num6 = k + 1;
					verts.Add(new Vector3(array[num5].x, array[k].y, 0f));
					verts.Add(new Vector3(array[num5].x, array[num6].y, 0f));
					verts.Add(new Vector3(array[j].x, array[num6].y, 0f));
					verts.Add(new Vector3(array[j].x, array[k].y, 0f));
					uvs.Add(new Vector2(array2[num5].x, array2[k].y));
					uvs.Add(new Vector2(array2[num5].x, array2[num6].y));
					uvs.Add(new Vector2(array2[j].x, array2[num6].y));
					uvs.Add(new Vector2(array2[j].x, array2[k].y));
					cols.Add(item);
					cols.Add(item);
					cols.Add(item);
					cols.Add(item);
				}
			}
		}
	}

	protected void TiledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture texture = this.material.mainTexture;
		if (!(texture != null))
		{
			return;
		}
		Rect rect = this.mInner;
		if (this.atlas.coordinates == UIAtlas.Coordinates.TexCoords)
		{
			rect = NGUIMath.ConvertToPixels(rect, texture.width, texture.height, round: true);
		}
		Vector2 vector = base.cachedTransform.localScale;
		float pixelSize = this.atlas.pixelSize;
		float num = Mathf.Abs(rect.width / vector.x) * pixelSize;
		float num2 = Mathf.Abs(rect.height / vector.y) * pixelSize;
		if (num < 0.01f || num2 < 0.01f)
		{
			Debug.LogWarning("The tiled sprite (" + NGUITools.GetHierarchy(base.gameObject) + ") is too small.\nConsider using a bigger one.");
			num = 0.01f;
			num2 = 0.01f;
		}
		Vector2 vector2 = new Vector2(rect.xMin / (float)texture.width, rect.yMin / (float)texture.height);
		Vector2 vector3 = new Vector2(rect.xMax / (float)texture.width, rect.yMax / (float)texture.height);
		Vector2 vector4 = vector3;
		Color color = base.color;
		color.a *= base.mPanel.alpha;
		Color32 item = ((!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color));
		for (float num3 = 0f; num3 < 1f; num3 += num2)
		{
			float num4 = 0f;
			vector4.x = vector3.x;
			float num5 = num3 + num2;
			if (num5 > 1f)
			{
				vector4.y = vector2.y + (vector3.y - vector2.y) * (1f - num3) / (num5 - num3);
				num5 = 1f;
			}
			for (; num4 < 1f; num4 += num)
			{
				float num6 = num4 + num;
				if (num6 > 1f)
				{
					vector4.x = vector2.x + (vector3.x - vector2.x) * (1f - num4) / (num6 - num4);
					num6 = 1f;
				}
				verts.Add(new Vector3(num6, 0f - num3, 0f));
				verts.Add(new Vector3(num6, 0f - num5, 0f));
				verts.Add(new Vector3(num4, 0f - num5, 0f));
				verts.Add(new Vector3(num4, 0f - num3, 0f));
				uvs.Add(new Vector2(vector4.x, 1f - vector2.y));
				uvs.Add(new Vector2(vector4.x, 1f - vector4.y));
				uvs.Add(new Vector2(vector2.x, 1f - vector4.y));
				uvs.Add(new Vector2(vector2.x, 1f - vector2.y));
				cols.Add(item);
				cols.Add(item);
				cols.Add(item);
				cols.Add(item);
			}
		}
	}

	public override void Update()
	{
		base.Update();
		if (base.mChanged || !this.mSpriteSet)
		{
			this.mSpriteSet = true;
			this.mSprite = null;
			base.mChanged = true;
			this.UpdateUVs(force: true);
		}
		else
		{
			this.UpdateUVs(force: false);
		}
	}

	public virtual void UpdateUVs(bool force)
	{
		if ((this.type == Type.Sliced || this.type == Type.Tiled) && base.cachedTransform.localScale != this.mScale)
		{
			this.mScale = base.cachedTransform.localScale;
			base.mChanged = true;
		}
		if (!(this.isValid && force))
		{
			return;
		}
		Texture texture = this.mainTexture;
		if (texture != null)
		{
			this.mInner = this.mSprite.inner;
			this.mOuter = this.mSprite.outer;
			this.mInnerUV = this.mInner;
			this.mOuterUV = this.mOuter;
			if (this.atlas.coordinates == UIAtlas.Coordinates.Pixels)
			{
				this.mOuterUV = NGUIMath.ConvertToTexCoords(this.mOuterUV, texture.width, texture.height);
				this.mInnerUV = NGUIMath.ConvertToTexCoords(this.mInnerUV, texture.width, texture.height);
			}
		}
	}
}
