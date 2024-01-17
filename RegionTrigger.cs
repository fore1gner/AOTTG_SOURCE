using UnityEngine;

internal class RegionTrigger : MonoBehaviour
{
	public string myName;

	public RCEvent playerEventEnter;

	public RCEvent playerEventExit;

	public RCEvent titanEventEnter;

	public RCEvent titanEventExit;

	public void CopyTrigger(RegionTrigger copyTrigger)
	{
		this.playerEventEnter = copyTrigger.playerEventEnter;
		this.titanEventEnter = copyTrigger.titanEventEnter;
		this.playerEventExit = copyTrigger.playerEventExit;
		this.titanEventExit = copyTrigger.titanEventExit;
		this.myName = copyTrigger.myName;
	}

	private void OnTriggerEnter(Collider other)
	{
		GameObject gameObject = other.transform.gameObject;
		if (gameObject.layer == 8)
		{
			if (this.playerEventEnter == null)
			{
				return;
			}
			HERO component = gameObject.GetComponent<HERO>();
			if (component != null)
			{
				string key = (string)FengGameManagerMKII.RCVariableNames["OnPlayerEnterRegion[" + this.myName + "]"];
				if (FengGameManagerMKII.playerVariables.ContainsKey(key))
				{
					FengGameManagerMKII.playerVariables[key] = component.photonView.owner;
				}
				else
				{
					FengGameManagerMKII.playerVariables.Add(key, component.photonView.owner);
				}
				this.playerEventEnter.checkEvent();
			}
		}
		else
		{
			if (gameObject.layer != 11 || this.titanEventEnter == null)
			{
				return;
			}
			TITAN component2 = gameObject.transform.root.gameObject.GetComponent<TITAN>();
			if (component2 != null)
			{
				string key = (string)FengGameManagerMKII.RCVariableNames["OnTitanEnterRegion[" + this.myName + "]"];
				if (FengGameManagerMKII.titanVariables.ContainsKey(key))
				{
					FengGameManagerMKII.titanVariables[key] = component2;
				}
				else
				{
					FengGameManagerMKII.titanVariables.Add(key, component2);
				}
				this.titanEventEnter.checkEvent();
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		GameObject gameObject = other.transform.root.gameObject;
		if (gameObject.layer == 8)
		{
			if (this.playerEventExit == null)
			{
				return;
			}
			HERO component = gameObject.GetComponent<HERO>();
			if (component != null)
			{
				string key = (string)FengGameManagerMKII.RCVariableNames["OnPlayerLeaveRegion[" + this.myName + "]"];
				if (FengGameManagerMKII.playerVariables.ContainsKey(key))
				{
					FengGameManagerMKII.playerVariables[key] = component.photonView.owner;
				}
				else
				{
					FengGameManagerMKII.playerVariables.Add(key, component.photonView.owner);
				}
			}
		}
		else
		{
			if (gameObject.layer != 11 || this.titanEventExit == null)
			{
				return;
			}
			TITAN component2 = gameObject.GetComponent<TITAN>();
			if (component2 != null)
			{
				string key = (string)FengGameManagerMKII.RCVariableNames["OnTitanLeaveRegion[" + this.myName + "]"];
				if (FengGameManagerMKII.titanVariables.ContainsKey(key))
				{
					FengGameManagerMKII.titanVariables[key] = component2;
				}
				else
				{
					FengGameManagerMKII.titanVariables.Add(key, component2);
				}
				this.titanEventExit.checkEvent();
			}
		}
	}
}
