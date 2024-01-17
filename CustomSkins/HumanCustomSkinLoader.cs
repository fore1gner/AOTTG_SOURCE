using System.Collections;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using Xft;

namespace CustomSkins;

internal class HumanCustomSkinLoader : BaseCustomSkinLoader
{
	private int _horseViewId;

	public HookCustomSkinPart HookL;

	public HookCustomSkinPart HookR;

	public float HookLTiling = 1f;

	public float HookRTiling = 1f;

	protected override string RendererIdPrefix => "human";

	public override IEnumerator LoadSkinsFromRPC(object[] data)
	{
		this._horseViewId = (int)data[0];
		string[] skinUrls = ((string)data[1]).Split(',');
		foreach (int partId in base.GetCustomSkinPartIds(typeof(HumanCustomSkinPartId)))
		{
			if ((partId == 0 && this._horseViewId < 0) || (partId == 12 && !base._owner.GetComponent<HERO>().IsMine()) || (partId == 10 && !SettingsManager.CustomSkinSettings.Human.GasEnabled.Value))
			{
				continue;
			}
			if (partId == 16 && skinUrls.Length > partId)
			{
				float.TryParse(skinUrls[partId], out this.HookLTiling);
			}
			else if (partId == 18 && skinUrls.Length > partId)
			{
				float.TryParse(skinUrls[partId], out this.HookRTiling);
			}
			else if ((partId != 15 || SettingsManager.CustomSkinSettings.Human.HookEnabled.Value) && (partId != 17 || SettingsManager.CustomSkinSettings.Human.HookEnabled.Value))
			{
				BaseCustomSkinPart part = this.GetCustomSkinPart(partId);
				if (skinUrls.Length > partId && !part.LoadCache(skinUrls[partId]))
				{
					yield return base.StartCoroutine(part.LoadSkin(skinUrls[partId]));
				}
				switch (partId)
				{
				case 15:
					this.HookL = (HookCustomSkinPart)part;
					break;
				case 17:
					this.HookR = (HookCustomSkinPart)part;
					break;
				}
			}
		}
		FengGameManagerMKII.instance.unloadAssets();
	}

	protected override BaseCustomSkinPart GetCustomSkinPart(int partId)
	{
		HERO component = base._owner.GetComponent<HERO>();
		List<Renderer> renderers = new List<Renderer>();
		switch ((HumanCustomSkinPartId)partId)
		{
		case HumanCustomSkinPartId.Horse:
			base.AddRenderersMatchingName(renderers, PhotonView.Find(this._horseViewId).gameObject, "HORSE");
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 1000000);
		case HumanCustomSkinPartId.Hair:
			base.AddRendererIfExists(renderers, component.setup.part_hair);
			base.AddRendererIfExists(renderers, component.setup.part_hair_1);
			return new HumanHairCustomSkinPart(this, renderers, base.GetRendererId(partId), 1000000, component.setup.myCostume.hairInfo);
		case HumanCustomSkinPartId.Eye:
			base.AddRendererIfExists(renderers, component.setup.part_eye);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 500000, new Vector2(8f, 8f));
		case HumanCustomSkinPartId.Glass:
			base.AddRendererIfExists(renderers, component.setup.part_glass);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 500000, new Vector2(8f, 8f));
		case HumanCustomSkinPartId.Face:
			base.AddRendererIfExists(renderers, component.setup.part_face);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 500000, new Vector2(8f, 8f));
		case HumanCustomSkinPartId.Skin:
			base.AddRendererIfExists(renderers, component.setup.part_hand_l);
			base.AddRendererIfExists(renderers, component.setup.part_hand_r);
			base.AddRendererIfExists(renderers, component.setup.part_head);
			base.AddRendererIfExists(renderers, component.setup.part_chest);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 1000000);
		case HumanCustomSkinPartId.Costume:
			base.AddRendererIfExists(renderers, component.setup.part_arm_l);
			base.AddRendererIfExists(renderers, component.setup.part_arm_r);
			base.AddRendererIfExists(renderers, component.setup.part_leg);
			base.AddRendererIfExists(renderers, component.setup.part_chest_2);
			base.AddRendererIfExists(renderers, component.setup.part_chest_3);
			base.AddRendererIfExists(renderers, component.setup.part_upper_body);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 2000000, null, useTransparentMaterial: true);
		case HumanCustomSkinPartId.Logo:
			base.AddRendererIfExists(renderers, component.setup.part_cape);
			base.AddRendererIfExists(renderers, component.setup.part_brand_1);
			base.AddRendererIfExists(renderers, component.setup.part_brand_2);
			base.AddRendererIfExists(renderers, component.setup.part_brand_3);
			base.AddRendererIfExists(renderers, component.setup.part_brand_4);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 500000);
		case HumanCustomSkinPartId.GearL:
			base.AddRendererIfExists(renderers, component.setup.part_3dmg);
			base.AddRendererIfExists(renderers, component.setup.part_3dmg_belt);
			base.AddRendererIfExists(renderers, component.setup.part_3dmg_gas_l);
			base.AddRendererIfExists(renderers, component.setup.part_blade_l);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 1000000);
		case HumanCustomSkinPartId.GearR:
			base.AddRendererIfExists(renderers, component.setup.part_3dmg_gas_r);
			base.AddRendererIfExists(renderers, component.setup.part_blade_r);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 1000000);
		case HumanCustomSkinPartId.Gas:
			base.AddRendererIfExists(renderers, component.transform.Find("3dmg_smoke").gameObject);
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 500000);
		case HumanCustomSkinPartId.Hoodie:
			if (component.setup.part_chest_1 != null && component.setup.part_chest_1.name.Contains("character_cap"))
			{
				base.AddRendererIfExists(renderers, component.setup.part_chest_1);
			}
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 500000);
		case HumanCustomSkinPartId.WeaponTrail:
		{
			List<XWeaponTrail> list = new List<XWeaponTrail>();
			list.Add(component.leftbladetrail);
			list.Add(component.leftbladetrail2);
			list.Add(component.rightbladetrail);
			list.Add(component.rightbladetrail2);
			return new WeaponTrailCustomSkinPart(this, list, base.GetRendererId(partId), 500000);
		}
		case HumanCustomSkinPartId.ThunderspearL:
			if (component.ThunderSpearLModel != null)
			{
				base.AddRendererIfExists(renderers, component.ThunderSpearLModel);
			}
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 1000000);
		case HumanCustomSkinPartId.ThunderspearR:
			if (component.ThunderSpearRModel != null)
			{
				base.AddRendererIfExists(renderers, component.ThunderSpearRModel);
			}
			return new BaseCustomSkinPart(this, renderers, base.GetRendererId(partId), 1000000);
		case HumanCustomSkinPartId.HookL:
		case HumanCustomSkinPartId.HookR:
			return new HookCustomSkinPart(this, base.GetRendererId(partId), 500000);
		default:
			return null;
		}
	}
}
