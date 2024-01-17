using System.Collections.Generic;

public class HeroStat
{
	public int ACL;

	public static HeroStat ARMIN;

	public int BLA;

	public static HeroStat EREN;

	public int GAS;

	public static HeroStat[] heroStats;

	private static bool init;

	public static HeroStat JEAN;

	public static HeroStat LEVI;

	public static HeroStat MARCO;

	public static HeroStat MIKASA;

	public string name;

	public static HeroStat PETRA;

	public static HeroStat SASHA;

	public string skillId = "petra";

	public int SPD;

	public static Dictionary<string, HeroStat> stats;

	public static HeroStat getInfo(string name)
	{
		HeroStat.initDATA();
		return HeroStat.stats[name];
	}

	private static void initDATA()
	{
		if (!HeroStat.init)
		{
			HeroStat.init = true;
			HeroStat.MIKASA = new HeroStat();
			HeroStat.LEVI = new HeroStat();
			HeroStat.ARMIN = new HeroStat();
			HeroStat.MARCO = new HeroStat();
			HeroStat.JEAN = new HeroStat();
			HeroStat.EREN = new HeroStat();
			HeroStat.PETRA = new HeroStat();
			HeroStat.SASHA = new HeroStat();
			HeroStat.MIKASA.name = "MIKASA";
			HeroStat.MIKASA.skillId = "mikasa";
			HeroStat.MIKASA.SPD = 125;
			HeroStat.MIKASA.GAS = 75;
			HeroStat.MIKASA.BLA = 75;
			HeroStat.MIKASA.ACL = 135;
			HeroStat.LEVI.name = "LEVI";
			HeroStat.LEVI.skillId = "levi";
			HeroStat.LEVI.SPD = 95;
			HeroStat.LEVI.GAS = 100;
			HeroStat.LEVI.BLA = 100;
			HeroStat.LEVI.ACL = 150;
			HeroStat.ARMIN.name = "ARMIN";
			HeroStat.ARMIN.skillId = "armin";
			HeroStat.ARMIN.SPD = 75;
			HeroStat.ARMIN.GAS = 150;
			HeroStat.ARMIN.BLA = 125;
			HeroStat.ARMIN.ACL = 85;
			HeroStat.MARCO.name = "MARCO";
			HeroStat.MARCO.skillId = "marco";
			HeroStat.MARCO.SPD = 110;
			HeroStat.MARCO.GAS = 100;
			HeroStat.MARCO.BLA = 115;
			HeroStat.MARCO.ACL = 95;
			HeroStat.JEAN.name = "JEAN";
			HeroStat.JEAN.skillId = "jean";
			HeroStat.JEAN.SPD = 100;
			HeroStat.JEAN.GAS = 150;
			HeroStat.JEAN.BLA = 80;
			HeroStat.JEAN.ACL = 100;
			HeroStat.EREN.name = "EREN";
			HeroStat.EREN.skillId = "eren";
			HeroStat.EREN.SPD = 100;
			HeroStat.EREN.GAS = 90;
			HeroStat.EREN.BLA = 90;
			HeroStat.EREN.ACL = 100;
			HeroStat.PETRA.name = "PETRA";
			HeroStat.PETRA.skillId = "petra";
			HeroStat.PETRA.SPD = 80;
			HeroStat.PETRA.GAS = 110;
			HeroStat.PETRA.BLA = 100;
			HeroStat.PETRA.ACL = 140;
			HeroStat.SASHA.name = "SASHA";
			HeroStat.SASHA.skillId = "sasha";
			HeroStat.SASHA.SPD = 140;
			HeroStat.SASHA.GAS = 100;
			HeroStat.SASHA.BLA = 100;
			HeroStat.SASHA.ACL = 115;
			HeroStat value = new HeroStat
			{
				skillId = "petra",
				SPD = 100,
				GAS = 100,
				BLA = 100,
				ACL = 100
			};
			HeroStat heroStat = new HeroStat();
			HeroStat.SASHA.name = "AHSS";
			heroStat.skillId = "sasha";
			heroStat.SPD = 100;
			heroStat.GAS = 100;
			heroStat.BLA = 100;
			heroStat.ACL = 100;
			HeroStat.stats = new Dictionary<string, HeroStat>();
			HeroStat.stats.Add("MIKASA", HeroStat.MIKASA);
			HeroStat.stats.Add("LEVI", HeroStat.LEVI);
			HeroStat.stats.Add("ARMIN", HeroStat.ARMIN);
			HeroStat.stats.Add("MARCO", HeroStat.MARCO);
			HeroStat.stats.Add("JEAN", HeroStat.JEAN);
			HeroStat.stats.Add("EREN", HeroStat.EREN);
			HeroStat.stats.Add("PETRA", HeroStat.PETRA);
			HeroStat.stats.Add("SASHA", HeroStat.SASHA);
			HeroStat.stats.Add("CUSTOM_DEFAULT", value);
			HeroStat.stats.Add("AHSS", heroStat);
		}
	}
}
