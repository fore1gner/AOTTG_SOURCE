using UnityEngine;

public class OnClickLoadSomething : MonoBehaviour
{
	public enum ResourceTypeOption : byte
	{
		Scene,
		Web
	}

	public string ResourceToLoad;

	public ResourceTypeOption ResourceTypeToLoad;

	public void OnClick()
	{
		switch (this.ResourceTypeToLoad)
		{
		case ResourceTypeOption.Scene:
			Application.LoadLevel(this.ResourceToLoad);
			break;
		case ResourceTypeOption.Web:
			Application.OpenURL(this.ResourceToLoad);
			break;
		}
	}
}
