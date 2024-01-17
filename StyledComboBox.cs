using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class StyledComboBox : StyledItem
{
	[CompilerGenerated]
	private sealed class AddItemcAnonStoreyF
	{
		internal StyledComboBox fthis;

		internal int curIndex;

		internal StyledItem styledItem;

		internal void m0()
		{
			this.fthis.OnItemClicked(this.styledItem, this.curIndex);
		}
	}

	public delegate void SelectionChangedHandler(StyledItem item);

	public StyledComboBoxPrefab containerPrefab;

	private bool isToggled;

	public StyledItem itemMenuPrefab;

	public StyledItem itemPrefab;

	[HideInInspector]
	[SerializeField]
	private List<StyledItem> items = new List<StyledItem>();

	public SelectionChangedHandler OnSelectionChanged;

	[HideInInspector]
	[SerializeField]
	private StyledComboBoxPrefab root;

	[SerializeField]
	private int selectedIndex;

	public int SelectedIndex
	{
		get
		{
			return this.selectedIndex;
		}
		set
		{
			if (value >= 0 && value <= this.items.Count)
			{
				this.selectedIndex = value;
				this.CreateMenuButton(this.items[this.selectedIndex].GetText().text);
			}
		}
	}

	public StyledItem SelectedItem
	{
		get
		{
			if (this.selectedIndex >= 0 && this.selectedIndex <= this.items.Count)
			{
				return this.items[this.selectedIndex];
			}
			return null;
		}
	}

	private void AddItem(object data)
	{
		if (!(this.itemPrefab != null))
		{
			return;
		}
		Vector3[] array = new Vector3[4];
		this.itemPrefab.GetComponent<RectTransform>().GetLocalCorners(array);
		Vector3 position = array[0];
		float num = position.y - array[2].y;
		position.y = (float)this.items.Count * num;
		StyledItem styledItem = Object.Instantiate(this.itemPrefab, position, this.root.itemRoot.rotation) as StyledItem;
		RectTransform component = styledItem.GetComponent<RectTransform>();
		styledItem.Populate(data);
		component.SetParent(this.root.itemRoot.transform, worldPositionStays: false);
		component.pivot = new Vector2(0f, 1f);
		component.anchorMin = new Vector2(0f, 1f);
		component.anchorMax = Vector2.one;
		component.anchoredPosition = new Vector2(0f, position.y);
		this.items.Add(styledItem);
		component.offsetMin = new Vector2(0f, position.y + num);
		component.offsetMax = new Vector2(0f, position.y);
		this.root.itemRoot.offsetMin = new Vector2(this.root.itemRoot.offsetMin.x, (float)(this.items.Count + 2) * num);
		Button button = styledItem.GetButton();
		int curIndex = this.items.Count - 1;
		if (button != null)
		{
			button.onClick.AddListener(delegate
			{
				this.OnItemClicked(styledItem, curIndex);
			});
		}
	}

	public void AddItems(params object[] list)
	{
		this.ClearItems();
		for (int i = 0; i < list.Length; i++)
		{
			this.AddItem(list[i]);
		}
		this.SelectedIndex = 0;
	}

	private void Awake()
	{
		this.InitControl();
	}

	public void ClearItems()
	{
		for (int num = this.items.Count - 1; num >= 0; num--)
		{
			Object.DestroyObject(this.items[num].gameObject);
		}
	}

	private void CreateMenuButton(object data)
	{
		if (this.root.menuItem.transform.childCount > 0)
		{
			for (int num = this.root.menuItem.transform.childCount - 1; num >= 0; num--)
			{
				Object.DestroyObject(this.root.menuItem.transform.GetChild(num).gameObject);
			}
		}
		if (this.itemMenuPrefab != null && this.root.menuItem != null)
		{
			StyledItem obj = Object.Instantiate(this.itemMenuPrefab) as StyledItem;
			obj.Populate(data);
			obj.transform.SetParent(this.root.menuItem.transform, worldPositionStays: false);
			RectTransform component = obj.GetComponent<RectTransform>();
			component.pivot = new Vector2(0.5f, 0.5f);
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.offsetMin = Vector2.zero;
			component.offsetMax = Vector2.zero;
			this.root.gameObject.hideFlags = HideFlags.HideInHierarchy;
			Button button = obj.GetButton();
			if (button != null)
			{
				button.onClick.AddListener(TogglePanelState);
			}
		}
	}

	public void InitControl()
	{
		if (this.root != null)
		{
			Object.DestroyImmediate(this.root.gameObject);
		}
		if (this.containerPrefab != null)
		{
			RectTransform component = base.GetComponent<RectTransform>();
			this.root = Object.Instantiate(this.containerPrefab, component.position, component.rotation) as StyledComboBoxPrefab;
			this.root.transform.SetParent(base.transform, worldPositionStays: false);
			RectTransform component2 = this.root.GetComponent<RectTransform>();
			component2.pivot = new Vector2(0.5f, 0.5f);
			component2.anchorMin = Vector2.zero;
			component2.anchorMax = Vector2.one;
			component2.offsetMax = Vector2.zero;
			component2.offsetMin = Vector2.zero;
			this.root.gameObject.hideFlags = HideFlags.HideInHierarchy;
			this.root.itemPanel.gameObject.SetActive(this.isToggled);
		}
	}

	public void OnItemClicked(StyledItem item, int index)
	{
		this.SelectedIndex = index;
		this.TogglePanelState();
		if (this.OnSelectionChanged != null)
		{
			this.OnSelectionChanged(item);
		}
	}

	public void TogglePanelState()
	{
		this.isToggled = !this.isToggled;
		this.root.itemPanel.gameObject.SetActive(this.isToggled);
	}
}
