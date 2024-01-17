using UnityEngine;

public class PopuplistCharacterSelection : MonoBehaviour
{
	public GameObject ACL;

	public GameObject BLA;

	public GameObject GAS;

	public GameObject SPD;

	private void onCharacterChange()
	{
		string selection = base.GetComponent<UIPopupList>().selection;
		HeroStat heroStat;
		switch (selection)
		{
		case "Set 1":
		case "Set 2":
		case "Set 3":
		{
			HeroCostume heroCostume = CostumeConeveter.LocalDataToHeroCostume(selection.ToUpper());
			heroStat = ((heroCostume != null) ? heroCostume.stat : new HeroStat());
			break;
		}
		default:
			heroStat = HeroStat.getInfo(base.GetComponent<UIPopupList>().selection);
			break;
		}
		this.SPD.transform.localScale = new Vector3(heroStat.SPD, 20f, 0f);
		this.GAS.transform.localScale = new Vector3(heroStat.GAS, 20f, 0f);
		this.BLA.transform.localScale = new Vector3(heroStat.BLA, 20f, 0f);
		this.ACL.transform.localScale = new Vector3(heroStat.ACL, 20f, 0f);
	}
}
