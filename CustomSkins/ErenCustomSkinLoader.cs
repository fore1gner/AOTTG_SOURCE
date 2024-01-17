using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins;

internal class ErenCustomSkinLoader : BaseCustomSkinLoader
{
	protected override string RendererIdPrefix => "eren";

	public override IEnumerator LoadSkinsFromRPC(object[] data)
	{
		string url = (string)data[0];
		BaseCustomSkinPart customSkinPart = this.GetCustomSkinPart(0);
		if (!customSkinPart.LoadCache(url))
		{
			yield return base.StartCoroutine(customSkinPart.LoadSkin(url));
		}
		FengGameManagerMKII.instance.unloadAssets();
	}

	protected override BaseCustomSkinPart GetCustomSkinPart(int partId)
	{
		List<Renderer> renderers = new List<Renderer>();
		if (partId == 0)
		{
			base.AddAllRenderers(renderers, base._owner);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 2000000);
		}
		return null;
	}
}
