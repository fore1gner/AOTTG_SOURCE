using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins;

internal class HumanHairCustomSkinPart : BaseCustomSkinPart
{
	private CostumeHair HairInfo;

	public HumanHairCustomSkinPart(BaseCustomSkinLoader loader, List<Renderer> renderers, string rendererId, int maxSize, CostumeHair hairInfo, Vector2? textureScale = null)
		: base(loader, renderers, rendererId, maxSize, textureScale)
	{
		this.HairInfo = hairInfo;
	}

	protected override Material SetNewTexture(Texture2D texture)
	{
		if (this.HairInfo.id >= 0)
		{
			base._renderers[0].material = CharacterMaterials.materials[this.HairInfo.texture];
		}
		return base.SetNewTexture(texture);
	}
}
