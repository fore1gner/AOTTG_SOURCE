using System.Collections.Generic;
using UnityEngine;
using Xft;

namespace CustomSkins;

internal class WeaponTrailCustomSkinPart : BaseCustomSkinPart
{
	private List<XWeaponTrail> _weaponTrails;

	public WeaponTrailCustomSkinPart(BaseCustomSkinLoader loader, List<XWeaponTrail> weaponTrails, string rendererId, int maxSize, Vector2? textureScale = null)
		: base(loader, null, rendererId, maxSize, textureScale)
	{
		this._weaponTrails = weaponTrails;
	}

	protected override bool IsValidPart()
	{
		if (this._weaponTrails.Count > 0)
		{
			return this._weaponTrails[0] != null;
		}
		return false;
	}

	protected override void DisableRenderers()
	{
		foreach (XWeaponTrail weaponTrail in this._weaponTrails)
		{
			weaponTrail.enabled = false;
		}
	}

	protected override void SetMaterial(Material material)
	{
		foreach (XWeaponTrail weaponTrail in this._weaponTrails)
		{
			weaponTrail.MyMaterial = material;
		}
	}

	protected override Material SetNewTexture(Texture2D texture)
	{
		this._weaponTrails[0].MyMaterial.mainTexture = texture;
		if (base._textureScale != base._defaultTextureScale)
		{
			Vector2 mainTextureScale = this._weaponTrails[0].MyMaterial.mainTextureScale;
			this._weaponTrails[0].MyMaterial.mainTextureScale = new Vector2(mainTextureScale.x * base._textureScale.x, mainTextureScale.y * base._textureScale.y);
		}
		this.SetMaterial(this._weaponTrails[0].MyMaterial);
		return this._weaponTrails[0].MyMaterial;
	}
}
