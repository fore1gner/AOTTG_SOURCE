using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins;

internal class TitanCustomSkinLoader : BaseCustomSkinLoader
{
	protected override string RendererIdPrefix => "titan";

	public override IEnumerator LoadSkinsFromRPC(object[] data)
	{
		if ((bool)data[0])
		{
			string url = (string)data[1];
			BaseCustomSkinPart customSkinPart = this.GetCustomSkinPart(0);
			if (!customSkinPart.LoadCache(url))
			{
				yield return base.StartCoroutine(customSkinPart.LoadSkin(url));
			}
		}
		else
		{
			string url2 = (string)data[1];
			string eyeUrl = (string)data[2];
			BaseCustomSkinPart customSkinPart2 = this.GetCustomSkinPart(1);
			if (!customSkinPart2.LoadCache(url2))
			{
				yield return base.StartCoroutine(customSkinPart2.LoadSkin(url2));
			}
			BaseCustomSkinPart customSkinPart3 = this.GetCustomSkinPart(2);
			if (!customSkinPart3.LoadCache(eyeUrl))
			{
				yield return base.StartCoroutine(customSkinPart3.LoadSkin(eyeUrl));
			}
		}
		FengGameManagerMKII.instance.unloadAssets();
	}

	protected override BaseCustomSkinPart GetCustomSkinPart(int partId)
	{
		TITAN component = base._owner.GetComponent<TITAN>();
		List<Renderer> renderers = new List<Renderer>();
		switch ((TitanCustomSkinPartId)partId)
		{
		case TitanCustomSkinPartId.Hair:
			base.AddRendererIfExists(renderers, component.GetComponent<TITAN_SETUP>().part_hair);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 500000);
		case TitanCustomSkinPartId.Body:
			base.AddRenderersMatchingName(renderers, base._owner, "hair");
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 1000000);
		case TitanCustomSkinPartId.Eye:
			base.AddRenderersContainingName(renderers, base._owner, "eye");
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 500000, new Vector2(4f, 8f));
		default:
			return null;
		}
	}
}
