using Settings;
using UnityEngine;

public class TITAN_CONTROLLER : MonoBehaviour
{
	public bool bite;

	public bool bitel;

	public bool biter;

	public bool chopl;

	public bool chopr;

	public bool choptl;

	public bool choptr;

	public bool cover;

	public Camera currentCamera;

	public float currentDirection;

	public bool grabbackl;

	public bool grabbackr;

	public bool grabfrontl;

	public bool grabfrontr;

	public bool grabnapel;

	public bool grabnaper;

	public bool isAttackDown;

	public bool isAttackIIDown;

	public bool isHorse;

	public bool isJumpDown;

	public bool isSuicide;

	public bool isWALKDown;

	public bool sit;

	public float targetDirection;

	private void Start()
	{
		this.currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		int num;
		int num2;
		float num5;
		if (this.isHorse)
		{
			num = (SettingsManager.InputSettings.General.Forward.GetKey() ? 1 : (SettingsManager.InputSettings.General.Back.GetKey() ? (-1) : 0));
			num2 = (SettingsManager.InputSettings.General.Left.GetKey() ? (-1) : (SettingsManager.InputSettings.General.Right.GetKey() ? 1 : 0));
			if (num2 != 0 || num != 0)
			{
				float y = this.currentCamera.transform.rotation.eulerAngles.y;
				float num3 = Mathf.Atan2(num, num2) * 57.29578f;
				num3 = 0f - num3 + 90f;
				float num4 = y + num3;
				this.targetDirection = num4;
			}
			else
			{
				this.targetDirection = -874f;
			}
			this.isAttackDown = false;
			this.isAttackIIDown = false;
			if (this.targetDirection != -874f)
			{
				this.currentDirection = this.targetDirection;
			}
			num5 = this.currentCamera.transform.rotation.eulerAngles.y - this.currentDirection;
			if (num5 >= 180f)
			{
				num5 -= 360f;
			}
			if (SettingsManager.InputSettings.Human.HorseJump.GetKey())
			{
				this.isAttackDown = true;
			}
			this.isWALKDown = SettingsManager.InputSettings.Human.HorseWalk.GetKey();
			return;
		}
		num = (SettingsManager.InputSettings.General.Forward.GetKey() ? 1 : (SettingsManager.InputSettings.General.Back.GetKey() ? (-1) : 0));
		num2 = (SettingsManager.InputSettings.General.Left.GetKey() ? (-1) : (SettingsManager.InputSettings.General.Right.GetKey() ? 1 : 0));
		if (num2 != 0 || num != 0)
		{
			float y2 = this.currentCamera.transform.rotation.eulerAngles.y;
			float num3 = Mathf.Atan2(num, num2) * 57.29578f;
			num3 = 0f - num3 + 90f;
			float num4 = y2 + num3;
			this.targetDirection = num4;
		}
		else
		{
			this.targetDirection = -874f;
		}
		this.isAttackDown = false;
		this.isJumpDown = false;
		this.isAttackIIDown = false;
		this.isSuicide = false;
		this.grabbackl = false;
		this.grabbackr = false;
		this.grabfrontl = false;
		this.grabfrontr = false;
		this.grabnapel = false;
		this.grabnaper = false;
		this.choptl = false;
		this.chopr = false;
		this.chopl = false;
		this.choptr = false;
		this.bite = false;
		this.bitel = false;
		this.biter = false;
		this.cover = false;
		this.sit = false;
		if (this.targetDirection != -874f)
		{
			this.currentDirection = this.targetDirection;
		}
		num5 = this.currentCamera.transform.rotation.eulerAngles.y - this.currentDirection;
		if (num5 >= 180f)
		{
			num5 -= 360f;
		}
		if (SettingsManager.InputSettings.Titan.AttackPunch.GetKey())
		{
			this.isAttackDown = true;
		}
		if (SettingsManager.InputSettings.Titan.AttackSlam.GetKey())
		{
			this.isAttackIIDown = true;
		}
		if (SettingsManager.InputSettings.Titan.Jump.GetKey())
		{
			this.isJumpDown = true;
		}
		if (SettingsManager.InputSettings.General.ChangeCharacter.GetKey())
		{
			this.isSuicide = true;
		}
		if (SettingsManager.InputSettings.Titan.CoverNape.GetKey())
		{
			this.cover = true;
		}
		if (SettingsManager.InputSettings.Titan.Sit.GetKey())
		{
			this.sit = true;
		}
		if (SettingsManager.InputSettings.Titan.AttackGrabFront.GetKey() && num5 >= 0f)
		{
			this.grabfrontr = true;
		}
		if (SettingsManager.InputSettings.Titan.AttackGrabFront.GetKey() && num5 < 0f)
		{
			this.grabfrontl = true;
		}
		if (SettingsManager.InputSettings.Titan.AttackGrabBack.GetKey() && num5 >= 0f)
		{
			this.grabbackr = true;
		}
		if (SettingsManager.InputSettings.Titan.AttackGrabBack.GetKey() && num5 < 0f)
		{
			this.grabbackl = true;
		}
		if (SettingsManager.InputSettings.Titan.AttackGrabNape.GetKey() && num5 >= 0f)
		{
			this.grabnaper = true;
		}
		if (SettingsManager.InputSettings.Titan.AttackGrabNape.GetKey() && num5 < 0f)
		{
			this.grabnapel = true;
		}
		if (SettingsManager.InputSettings.Titan.AttackSlap.GetKey() && num5 >= 0f)
		{
			this.choptr = true;
		}
		if (SettingsManager.InputSettings.Titan.AttackSlap.GetKey() && num5 < 0f)
		{
			this.choptl = true;
		}
		if (SettingsManager.InputSettings.Titan.AttackBite.GetKey() && num5 > 7.5f)
		{
			this.biter = true;
		}
		if (SettingsManager.InputSettings.Titan.AttackBite.GetKey() && num5 < -7.5f)
		{
			this.bitel = true;
		}
		if (SettingsManager.InputSettings.Titan.AttackBite.GetKey() && num5 >= -7.5f && num5 <= 7.5f)
		{
			this.bite = true;
		}
		this.isWALKDown = SettingsManager.InputSettings.Titan.Walk.GetKey();
	}
}
