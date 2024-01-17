using UnityEngine;

namespace UI;

internal class SettingsKeybindsPanel : SettingsCategoryPanel
{
	protected string[] _categories = new string[6] { "General", "Human", "Titan", "Shifter", "Interaction", "RC Editor" };

	protected override bool CategoryPanel => true;

	protected override string DefaultCategoryPanel => "General";

	public void CreateGategoryDropdown(Transform panel)
	{
		ElementStyle style = new ElementStyle(24, 140f, this.ThemePanel);
		ElementFactory.CreateDropdownSetting(panel, style, base._currentCategoryPanelName, "Category", this._categories, "", 260f, 40f, 300f, null, delegate
		{
			base.RebuildCategoryPanel();
		});
	}

	protected override void RegisterCategoryPanels()
	{
		string[] categories = this._categories;
		foreach (string key in categories)
		{
			base._categoryPanelTypes.Add(key, typeof(SettingsKeybindsDefaultPanel));
		}
	}
}
