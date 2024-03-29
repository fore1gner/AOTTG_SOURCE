using UnityEngine;

namespace CustomSkins;

internal class HookCustomSkinPart : BaseCustomSkinPart
{
	public Material HookMaterial;

	public bool Transparent;

	public HookCustomSkinPart(BaseCustomSkinLoader loader, string rendererId, int maxSize, Vector2? textureScale = null)
		: base(loader, null, rendererId, maxSize, textureScale)
	{
	}

	protected override bool IsValidPart()
	{
		return true;
	}

	protected override void DisableRenderers()
	{
		this.Transparent = true;
	}

	protected override void SetMaterial(Material material)
	{
		this.HookMaterial = material;
	}

	protected override Material SetNewTexture(Texture2D texture)
	{
		Material material = new Material(Shader.Find("Transparent/Diffuse"));
		material.mainTexture = texture;
		this.SetMaterial(material);
		return material;
	}
}
