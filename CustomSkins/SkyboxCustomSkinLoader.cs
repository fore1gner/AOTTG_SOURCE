using System.Collections;
using UnityEngine;

namespace CustomSkins;

internal class SkyboxCustomSkinLoader : BaseCustomSkinLoader
{
	public static Material SkyboxMaterial;

	protected override string RendererIdPrefix => "skybox";

	public override IEnumerator LoadSkinsFromRPC(object[] data)
	{
		SkyboxCustomSkinLoader.SkyboxMaterial = new Material(Shader.Find("RenderFX/Skybox"));
		SkyboxCustomSkinLoader.SkyboxMaterial.CopyPropertiesFromMaterial(Camera.main.GetComponent<Skybox>().material);
		foreach (int customSkinPartId in base.GetCustomSkinPartIds(typeof(SkyboxCustomSkinPartId)))
		{
			BaseCustomSkinPart customSkinPart = this.GetCustomSkinPart(customSkinPartId);
			string url = (string)data[customSkinPartId];
			if (!customSkinPart.LoadCache(url))
			{
				yield return base.StartCoroutine(customSkinPart.LoadSkin(url));
			}
		}
	}

	protected override BaseCustomSkinPart GetCustomSkinPart(int partId)
	{
		return new SkyboxCustomSkinPart(this, SkyboxCustomSkinLoader.SkyboxMaterial, this.PartIdToTextureName((SkyboxCustomSkinPartId)partId), base.GetRendererId(partId), 2000000);
	}

	public string PartIdToTextureName(SkyboxCustomSkinPartId partId)
	{
		return "_" + partId.ToString() + "Tex";
	}
}
