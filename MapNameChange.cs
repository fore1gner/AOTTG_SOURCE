using UnityEngine;

public class MapNameChange : MonoBehaviour
{
	private void OnSelectionChange()
	{
		LevelInfo ınfo = LevelInfo.getInfo(base.GetComponent<UIPopupList>().selection);
		if (ınfo != null)
		{
			GameObject.Find("LabelLevelInfo").GetComponent<UILabel>().text = ınfo.desc;
		}
		if (!base.GetComponent<UIPopupList>().items.Contains("Custom"))
		{
			base.GetComponent<UIPopupList>().items.Add("Custom");
			base.GetComponent<UIPopupList>().textScale *= 0.8f;
		}
		if (!base.GetComponent<UIPopupList>().items.Contains("Custom (No PT)"))
		{
			base.GetComponent<UIPopupList>().items.Add("Custom (No PT)");
		}
	}
}
