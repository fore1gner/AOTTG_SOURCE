using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class EditProfilePopup : BasePopup
{
	protected override string Title => string.Empty;

	protected override float Width => 730f;

	protected override float Height => 660f;

	protected override bool CategoryPanel => true;

	protected override bool CategoryButtons => true;

	protected override string DefaultCategoryPanel => "Profile";

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		this.SetupBottomButtons();
	}

	protected override void SetupTopButtons()
	{
		ElementStyle style = new ElementStyle(28, 120f, this.ThemePanel);
		string[] array = new string[2] { "Profile", "Stats" };
		foreach (string buttonName in array)
		{
			GameObject gameObject = ElementFactory.CreateCategoryButton(base.TopBar, style, UIManager.GetLocaleCommon(buttonName), delegate
			{
				this.SetCategoryPanel(buttonName);
			});
			base._topButtons.Add(buttonName, gameObject.GetComponent<Button>());
		}
		base.SetupTopButtons();
	}

	protected override void RegisterCategoryPanels()
	{
		base._categoryPanelTypes.Add("Profile", typeof(EditProfileProfilePanel));
		base._categoryPanelTypes.Add("Stats", typeof(EditProfileStatsPanel));
	}

	protected override void SetupPopups()
	{
		base.SetupPopups();
	}

	private void SetupBottomButtons()
	{
		ElementStyle style = new ElementStyle(this.ButtonFontSize, 120f, this.ThemePanel);
		string[] array = new string[1] { "Save" };
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
		if (name == "Save")
		{
			SettingsManager.ProfileSettings.Save();
			this.Hide();
		}
	}
}
