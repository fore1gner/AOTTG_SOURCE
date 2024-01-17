using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class QuestPopup : BasePopup
{
	public StringSetting TierSelection = new StringSetting("Bronze");

	public StringSetting CompletedSelection = new StringSetting("In Progress");

	protected override string Title => string.Empty;

	protected override float Width => 990f;

	protected override float Height => 740f;

	protected override bool CategoryPanel => true;

	protected override bool CategoryButtons => true;

	protected override string DefaultCategoryPanel => "Daily";

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		this.SetupBottomButtons();
	}

	public void CreateAchievmentDropdowns(Transform panel)
	{
		ElementStyle style = new ElementStyle(24, 0f, this.ThemePanel);
		ElementFactory.CreateDropdownSetting(panel, style, this.TierSelection, "", new string[3] { "Bronze", "Silver", "Gold" }, "", 180f, 40f, 300f, null, delegate
		{
			base.RebuildCategoryPanel();
		});
		ElementFactory.CreateDropdownSetting(panel, style, this.CompletedSelection, "", new string[2] { "In Progress", "Completed" }, "", 180f, 40f, 300f, null, delegate
		{
			base.RebuildCategoryPanel();
		});
	}

	protected override void SetupTopButtons()
	{
		ElementStyle style = new ElementStyle(28, 120f, this.ThemePanel);
		string[] array = new string[3] { "Daily", "Weekly", "Achievments" };
		foreach (string buttonName in array)
		{
			GameObject gameObject = ElementFactory.CreateCategoryButton(title: (!(buttonName == "Daily") && !(buttonName == "Weekly")) ? UIManager.GetLocaleCommon(buttonName) : UIManager.GetLocale("MainMenu", "QuestsPopup", buttonName), parent: base.TopBar, style: style, onClick: delegate
			{
				this.SetCategoryPanel(buttonName);
			});
			base._topButtons.Add(buttonName, gameObject.GetComponent<Button>());
		}
		base.SetupTopButtons();
	}

	protected override void RegisterCategoryPanels()
	{
		base._categoryPanelTypes.Add("Daily", typeof(QuestDailyPanel));
		base._categoryPanelTypes.Add("Weekly", typeof(QuestWeeklyPanel));
		base._categoryPanelTypes.Add("Achievments", typeof(QuestAchievmentsPanel));
	}

	protected override void SetupPopups()
	{
		base.SetupPopups();
	}

	private void SetupBottomButtons()
	{
		ElementStyle style = new ElementStyle(this.ButtonFontSize, 120f, this.ThemePanel);
		string[] array = new string[1] { "Back" };
		foreach (string buttonName in array)
		{
			ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocaleCommon(buttonName), 0f, 0f, delegate
			{
				this.OnBottomBarButtonClick(buttonName);
			});
		}
	}

	private void OnBottomBarButtonClick(string name)
	{
		if (name == "Back")
		{
			this.Hide();
		}
	}
}
