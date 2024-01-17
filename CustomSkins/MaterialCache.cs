using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins;

internal class MaterialCache
{
	private static Dictionary<string, Material> _IdToMaterial = new Dictionary<string, Material>();

	public static Material TransparentMaterial;

	public static void Init()
	{
		MaterialCache.TransparentMaterial = new Material(Shader.Find("Transparent/Diffuse"));
		Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
		texture2D.SetPixel(0, 0, new Color(0f, 0f, 0f, 0f));
		texture2D.Apply();
		MaterialCache.TransparentMaterial.mainTexture = texture2D;
	}

	public static void Clear()
	{
		MaterialCache._IdToMaterial.Clear();
	}

	public static bool ContainsKey(string rendererId, string url)
	{
		return MaterialCache._IdToMaterial.ContainsKey(MaterialCache.GetId(rendererId, url));
	}

	public static Material GetMaterial(string rendererId, string url)
	{
		return MaterialCache._IdToMaterial[MaterialCache.GetId(rendererId, url)];
	}

	public static void SetMaterial(string rendererId, string url, Material material)
	{
		string ıd = MaterialCache.GetId(rendererId, url);
		if (MaterialCache._IdToMaterial.ContainsKey(ıd))
		{
			MaterialCache._IdToMaterial[ıd] = material;
		}
		else
		{
			MaterialCache._IdToMaterial.Add(ıd, material);
		}
	}

	private static string GetId(string rendererId, string url)
	{
		return rendererId + "," + url;
	}
}
