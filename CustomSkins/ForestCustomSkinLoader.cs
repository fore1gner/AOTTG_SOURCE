using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomSkins;

internal class ForestCustomSkinLoader : LevelCustomSkinLoader
{
	private List<GameObject> _treeObjects = new List<GameObject>();

	private List<GameObject> _groundObjects = new List<GameObject>();

	protected override string RendererIdPrefix => "forest";

	public override IEnumerator LoadSkinsFromRPC(object[] data)
	{
		this.FindAndIndexLevelObjects();
		char[] randomIndices = ((string)data[0]).ToCharArray();
		int[] trunkRandomIndices = this.SplitRandomIndices(randomIndices, 0);
		int[] leafRandomIndices = this.SplitRandomIndices(randomIndices, 1);
		string[] trunkUrls = ((string)data[1]).Split(',');
		string[] leafUrls = ((string)data[2]).Split(',');
		string groundUrl = leafUrls[8];
		for (int i = 0; i < this._treeObjects.Count; i++)
		{
			int num = trunkRandomIndices[i];
			int num2 = leafRandomIndices[i];
			string url = trunkUrls[num];
			string leafUrl = leafUrls[num2];
			BaseCustomSkinPart customSkinPart = this.GetCustomSkinPart(0, this._treeObjects[i]);
			BaseCustomSkinPart leafPart = this.GetCustomSkinPart(1, this._treeObjects[i]);
			if (!customSkinPart.LoadCache(url))
			{
				yield return base.StartCoroutine(customSkinPart.LoadSkin(url));
			}
			if (!leafPart.LoadCache(leafUrl))
			{
				yield return base.StartCoroutine(leafPart.LoadSkin(leafUrl));
			}
		}
		foreach (GameObject groundObject in this._groundObjects)
		{
			BaseCustomSkinPart customSkinPart2 = this.GetCustomSkinPart(2, groundObject);
			if (!customSkinPart2.LoadCache(groundUrl))
			{
				yield return base.StartCoroutine(customSkinPart2.LoadSkin(groundUrl));
			}
		}
		FengGameManagerMKII.instance.unloadAssets();
	}

	protected BaseCustomSkinPart GetCustomSkinPart(int partId, GameObject levelObject)
	{
		List<Renderer> renderers = new List<Renderer>();
		switch ((ForestCustomSkinPartId)partId)
		{
		case ForestCustomSkinPartId.TreeTrunk:
			base.AddRenderersContainingName(renderers, levelObject, "Cube");
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 2000000);
		case ForestCustomSkinPartId.TreeLeaf:
			base.AddRenderersContainingName(renderers, levelObject, "Plane_031");
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 500000);
		case ForestCustomSkinPartId.Ground:
			base.AddAllRenderers(renderers, levelObject);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 500000);
		default:
			return null;
		}
	}

	protected override void FindAndIndexLevelObjects()
	{
		this._treeObjects.Clear();
		this._groundObjects.Clear();
		Object[] array = Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject != null)
			{
				if (gameObject.name.Contains("TREE"))
				{
					this._treeObjects.Add(gameObject);
				}
				else if (gameObject.name.Contains("Cube_001") && gameObject.transform.parent.gameObject.tag != "Player")
				{
					this._groundObjects.Add(gameObject);
				}
			}
		}
	}

	private int[] SplitRandomIndices(char[] randomIndices, int offset)
	{
		List<int> list = new List<int>();
		for (int i = offset; i < randomIndices.Length; i += 2)
		{
			if (i < randomIndices.Length)
			{
				list.Add(int.Parse(randomIndices[i].ToString()));
			}
		}
		return Enumerable.ToArray(list);
	}
}
