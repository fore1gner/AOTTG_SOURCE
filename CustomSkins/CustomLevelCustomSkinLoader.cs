using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins;

internal class CustomLevelCustomSkinLoader : LevelCustomSkinLoader
{
	private List<GameObject> _groundObjects = new List<GameObject>();

	protected override string RendererIdPrefix => "customlevel";

	public override IEnumerator LoadSkinsFromRPC(object[] data)
	{
		this.FindAndIndexLevelObjects();
		string groundUrl = (string)data[6];
		foreach (GameObject groundObject in this._groundObjects)
		{
			BaseCustomSkinPart customSkinPart = this.GetCustomSkinPart(0, groundObject);
			if (!customSkinPart.LoadCache(groundUrl))
			{
				yield return base.StartCoroutine(customSkinPart.LoadSkin(groundUrl));
			}
		}
		FengGameManagerMKII.instance.unloadAssets();
	}

	protected BaseCustomSkinPart GetCustomSkinPart(int partId, GameObject levelObject)
	{
		List<Renderer> renderers = new List<Renderer>();
		if (partId == 0)
		{
			base.AddAllRenderers(renderers, levelObject);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 500000);
		}
		return null;
	}

	protected override void FindAndIndexLevelObjects()
	{
		this._groundObjects.Clear();
		Object[] array = Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject != null && gameObject.name.Contains("Cube_001") && gameObject.transform.parent.gameObject.tag != "Player" && gameObject.renderer != null)
			{
				this._groundObjects.Add(gameObject);
			}
		}
	}
}
