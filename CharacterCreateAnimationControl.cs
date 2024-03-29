using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharacterCreateAnimationControl : MonoBehaviour
{
	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmap0;

	private string currentAnimation;

	private float interval = 10f;

	private HERO_SETUP setup;

	private float timeElapsed;

	private void play(string id)
	{
		this.currentAnimation = id;
		base.animation.Play(id);
	}

	public void playAttack(string id)
	{
		switch (id)
		{
		case "mikasa":
			this.currentAnimation = "attack3_1";
			break;
		case "levi":
			this.currentAnimation = "attack5";
			break;
		case "sasha":
			this.currentAnimation = "special_sasha";
			break;
		case "jean":
			this.currentAnimation = "grabbed_jean";
			break;
		case "marco":
			this.currentAnimation = "special_marco_0";
			break;
		case "armin":
			this.currentAnimation = "special_armin";
			break;
		case "petra":
			this.currentAnimation = "special_petra";
			break;
		}
		base.animation.Play(this.currentAnimation);
	}

	private void Start()
	{
		this.setup = base.gameObject.GetComponent<HERO_SETUP>();
		this.currentAnimation = "stand_levi";
		this.play(this.currentAnimation);
	}

	public void toStand()
	{
		if (this.setup.myCostume.sex == SEX.FEMALE)
		{
			this.currentAnimation = "stand";
		}
		else
		{
			this.currentAnimation = "stand_levi";
		}
		base.animation.CrossFade(this.currentAnimation, 0.1f);
		this.timeElapsed = 0f;
	}

	private void Update()
	{
		if (this.currentAnimation == "stand" || this.currentAnimation == "stand_levi")
		{
			this.timeElapsed += Time.deltaTime;
			if (this.timeElapsed > this.interval)
			{
				this.timeElapsed = 0f;
				if (Random.Range(1, 1000) < 350)
				{
					this.play("salute");
				}
				else if (Random.Range(1, 1000) < 350)
				{
					this.play("supply");
				}
				else
				{
					this.play("dodge");
				}
			}
		}
		else if (base.animation[this.currentAnimation].normalizedTime >= 1f)
		{
			if (this.currentAnimation == "attack3_1")
			{
				this.play("attack3_2");
			}
			else if (this.currentAnimation == "special_sasha")
			{
				this.play("run_sasha");
			}
			else
			{
				this.toStand();
			}
		}
	}
}
