using UnityEngine;

public class PanelGroupManager
{
	public GameObject[] panelGroup;

	public void ActivePanel(int index)
	{
		GameObject[] array = this.panelGroup;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		this.panelGroup[index].SetActive(value: true);
	}
}
