using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins;

internal class CityCustomSkinLoader : LevelCustomSkinLoader
{
	private List<GameObject> _houseObjects = new List<GameObject>();

	private List<GameObject> _groundObjects = new List<GameObject>();

	private List<GameObject> _wallObjects = new List<GameObject>();

	private List<GameObject> _gateObjects = new List<GameObject>();

	protected override string RendererIdPrefix => "city";

	public override IEnumerator LoadSkinsFromRPC(object[] data)
	{
		this.FindAndIndexLevelObjects();
		char[] randomIndices = ((string)data[0]).ToCharArray();
		string[] houseUrls = ((string)data[1]).Split(',');
		string[] miscUrls = ((string)data[2]).Split(',');
		for (int i = 0; i < this._houseObjects.Count; i++)
		{
			int num = int.Parse(randomIndices[i].ToString());
			BaseCustomSkinPart customSkinPart = this.GetCustomSkinPart(0, this._houseObjects[i]);
			if (!customSkinPart.LoadCache(houseUrls[num]))
			{
				yield return base.StartCoroutine(customSkinPart.LoadSkin(houseUrls[num]));
			}
		}
		foreach (GameObject groundObject in this._groundObjects)
		{
			BaseCustomSkinPart customSkinPart2 = this.GetCustomSkinPart(1, groundObject);
			if (!customSkinPart2.LoadCache(miscUrls[0]))
			{
				yield return base.StartCoroutine(customSkinPart2.LoadSkin(miscUrls[0]));
			}
		}
		foreach (GameObject wallObject in this._wallObjects)
		{
			BaseCustomSkinPart customSkinPart3 = this.GetCustomSkinPart(2, wallObject);
			if (!customSkinPart3.LoadCache(miscUrls[1]))
			{
				yield return base.StartCoroutine(customSkinPart3.LoadSkin(miscUrls[1]));
			}
		}
		foreach (GameObject gateObject in this._gateObjects)
		{
			BaseCustomSkinPart customSkinPart4 = this.GetCustomSkinPart(3, gateObject);
			if (!customSkinPart4.LoadCache(miscUrls[2]))
			{
				yield return base.StartCoroutine(customSkinPart4.LoadSkin(miscUrls[2]));
			}
		}
		FengGameManagerMKII.instance.unloadAssets();
	}

	protected BaseCustomSkinPart GetCustomSkinPart(int partId, GameObject levelObject)
	{
		List<Renderer> renderers = new List<Renderer>();
		switch ((CityCustomSkinPartId)partId)
		{
		case CityCustomSkinPartId.House:
			base.AddAllRenderers(renderers, levelObject);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 2000000);
		case CityCustomSkinPartId.Ground:
			base.AddAllRenderers(renderers, levelObject);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 500000);
		case CityCustomSkinPartId.Wall:
			base.AddAllRenderers(renderers, levelObject);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 500000);
		case CityCustomSkinPartId.Gate:
			base.AddAllRenderers(renderers, levelObject);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 2000000);
		default:
			return null;
		}
	}

	protected override void FindAndIndexLevelObjects()
	{
		this._houseObjects.Clear();
		this._groundObjects.Clear();
		this._wallObjects.Clear();
		this._gateObjects.Clear();
		Object[] array = Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			string text = gameObject.name;
			if (gameObject != null && text.Contains("Cube_") && gameObject.transform.parent.gameObject.tag != "Player")
			{
				if (text.EndsWith("001"))
				{
					this._groundObjects.Add(gameObject);
				}
				else if (text.EndsWith("006") || text.EndsWith("007") || text.EndsWith("015") || text.EndsWith("000"))
				{
					this._wallObjects.Add(gameObject);
				}
				else if (text.EndsWith("002") && gameObject.transform.position == Vector3.zero)
				{
					this._wallObjects.Add(gameObject);
				}
				else if (text.EndsWith("005") || text.EndsWith("003"))
				{
					this._houseObjects.Add(gameObject);
				}
				else if (text.EndsWith("002") && gameObject.transform.position != Vector3.zero)
				{
					this._houseObjects.Add(gameObject);
				}
				else if (text.EndsWith("019") || text.EndsWith("020"))
				{
					this._gateObjects.Add(gameObject);
				}
			}
		}
	}
}
