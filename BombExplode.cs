using System.Collections;
using Photon;
using Settings;
using UnityEngine;

public class BombExplode : Photon.MonoBehaviour
{
	public static float _sizeMultiplier = 1.1f;

	public void Awake()
	{
		if (base.photonView != null)
		{
			PhotonPlayer owner = base.photonView.owner;
			float num = Mathf.Clamp(RCextensions.returnFloatFromObject(owner.customProperties[PhotonPlayerProperty.RCBombRadius]), 20f, 60f) * 2f * BombExplode._sizeMultiplier;
			ParticleSystem component = base.GetComponent<ParticleSystem>();
			if (SettingsManager.AbilitySettings.UseOldEffect.Value)
			{
				component.Stop();
				component.Clear();
				component = base.transform.Find("OldExplodeEffect").GetComponent<ParticleSystem>();
				component.gameObject.SetActive(value: true);
				num /= BombExplode._sizeMultiplier;
			}
			if (SettingsManager.AbilitySettings.ShowBombColors.Value)
			{
				component.startColor = BombUtil.GetBombColor(owner);
			}
			component.startSize = num;
			if (base.photonView.isMine)
			{
				base.StartCoroutine(this.WaitAndDestroyCoroutine(1.5f));
			}
		}
	}

	private IEnumerator WaitAndDestroyCoroutine(float time)
	{
		yield return new WaitForSeconds(time);
		PhotonNetwork.Destroy(base.gameObject);
	}
}
