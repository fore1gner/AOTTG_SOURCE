using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UITexture))]
public class DownloadTexture : MonoBehaviour
{
	private Material mMat;

	private Texture2D mTex;

	public string url = "http://www.tasharen.com/misc/logo.png";

	private void OnDestroy()
	{
		if (this.mMat != null)
		{
			Object.Destroy(this.mMat);
		}
		if (this.mTex != null)
		{
			Object.Destroy(this.mTex);
		}
	}

	private IEnumerator Start()
	{
		WWW wWW = new WWW(this.url);
		yield return wWW;
		this.mTex = wWW.texture;
		if (!(this.mTex == null))
		{
			UITexture component = this.GetComponent<UITexture>();
			if (component.material != null)
			{
				this.mMat = new Material(component.material);
			}
			else
			{
				this.mMat = new Material(Shader.Find("Unlit/Transparent Colored"));
			}
			component.material = this.mMat;
			this.mMat.mainTexture = this.mTex;
			component.MakePixelPerfect();
		}
		wWW.Dispose();
	}
}
