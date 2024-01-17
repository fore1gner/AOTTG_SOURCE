using System.Collections;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using Utility;

namespace CustomSkins;

internal class BaseCustomSkinPart
{
	protected BaseCustomSkinLoader _loader;

	protected List<Renderer> _renderers;

	protected string _rendererId;

	protected int _maxSize;

	protected Vector2 _textureScale;

	protected readonly Vector2 _defaultTextureScale = new Vector2(1f, 1f);

	protected bool _useTransparentMaterial;

	public BaseCustomSkinPart(BaseCustomSkinLoader loader, List<Renderer> renderers, string rendererId, int maxSize, Vector2? textureScale = null, bool useTransparentMaterial = false)
	{
		this._loader = loader;
		this._renderers = renderers;
		this._rendererId = rendererId;
		this._maxSize = maxSize;
		if (!textureScale.HasValue)
		{
			this._textureScale = this._defaultTextureScale;
		}
		else
		{
			this._textureScale = textureScale.Value;
		}
		this._useTransparentMaterial = useTransparentMaterial;
	}

	public bool LoadCache(string url)
	{
		if (url.ToLower() == BaseCustomSkinLoader.TransparentURL)
		{
			this.DisableRenderers();
			return true;
		}
		if (!this.IsValidPart() || !TextureDownloader.ValidTextureURL(url))
		{
			return true;
		}
		if (MaterialCache.ContainsKey(this._rendererId, url))
		{
			this.SetMaterial(MaterialCache.GetMaterial(this._rendererId, url));
			return true;
		}
		return false;
	}

	public IEnumerator LoadSkin(string url)
	{
		url = url.Trim();
		if (this.IsValidPart() && TextureDownloader.ValidTextureURL(url))
		{
			bool value = SettingsManager.GraphicsSettings.MipmapEnabled.Value;
			CoroutineWithData cwd = new CoroutineWithData(this._loader, TextureDownloader.DownloadTexture(this._loader, url, value, this._maxSize));
			yield return cwd.Coroutine;
			if (this.IsValidPart())
			{
				Material material = this.SetNewTexture((Texture2D)cwd.Result);
				MaterialCache.SetMaterial(this._rendererId, url, material);
			}
		}
	}

	protected virtual bool IsValidPart()
	{
		if (this._renderers.Count > 0)
		{
			return this._renderers[0] != null;
		}
		return false;
	}

	protected virtual void DisableRenderers()
	{
		if (this._useTransparentMaterial)
		{
			this.SetMaterial(MaterialCache.TransparentMaterial);
			return;
		}
		foreach (Renderer renderer in this._renderers)
		{
			renderer.enabled = false;
		}
	}

	protected virtual void SetMaterial(Material material)
	{
		foreach (Renderer renderer in this._renderers)
		{
			renderer.material = material;
		}
	}

	protected virtual Material SetNewTexture(Texture2D texture)
	{
		this._renderers[0].material.mainTexture = texture;
		if (this._textureScale != this._defaultTextureScale)
		{
			Vector2 mainTextureScale = this._renderers[0].material.mainTextureScale;
			this._renderers[0].material.mainTextureScale = new Vector2(mainTextureScale.x * this._textureScale.x, mainTextureScale.y * this._textureScale.y);
			this._renderers[0].material.mainTextureOffset = new Vector2(0f, 0f);
		}
		this.SetMaterial(this._renderers[0].material);
		return this._renderers[0].material;
	}
}
