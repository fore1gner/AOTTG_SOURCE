using System.Collections.Generic;
using UnityEngine;

public abstract class UIItemSlot : MonoBehaviour
{
	public UIWidget background;

	public AudioClip errorSound;

	public AudioClip grabSound;

	public UISprite icon;

	public UILabel label;

	private static InvGameItem mDraggedItem;

	private InvGameItem mItem;

	private string mText = string.Empty;

	public AudioClip placeSound;

	protected abstract InvGameItem observedItem { get; }

	private void OnClick()
	{
		if (UIItemSlot.mDraggedItem != null)
		{
			this.OnDrop(null);
		}
		else if (this.mItem != null)
		{
			UIItemSlot.mDraggedItem = this.Replace(null);
			if (UIItemSlot.mDraggedItem != null)
			{
				NGUITools.PlaySound(this.grabSound);
			}
			this.UpdateCursor();
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (UIItemSlot.mDraggedItem == null && this.mItem != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			UIItemSlot.mDraggedItem = this.Replace(null);
			NGUITools.PlaySound(this.grabSound);
			this.UpdateCursor();
		}
	}

	private void OnDrop(GameObject go)
	{
		InvGameItem ınvGameItem = this.Replace(UIItemSlot.mDraggedItem);
		if (UIItemSlot.mDraggedItem == ınvGameItem)
		{
			NGUITools.PlaySound(this.errorSound);
		}
		else if (ınvGameItem != null)
		{
			NGUITools.PlaySound(this.grabSound);
		}
		else
		{
			NGUITools.PlaySound(this.placeSound);
		}
		UIItemSlot.mDraggedItem = ınvGameItem;
		this.UpdateCursor();
	}

	private void OnTooltip(bool show)
	{
		InvGameItem ınvGameItem = ((!show) ? null : this.mItem);
		if (ınvGameItem != null)
		{
			InvBaseItem baseItem = ınvGameItem.baseItem;
			if (baseItem != null)
			{
				string text = "[" + NGUITools.EncodeColor(ınvGameItem.color) + "]" + ınvGameItem.name + "[-]\n";
				string text2 = text + "[AFAFAF]Level " + ınvGameItem.itemLevel + " " + baseItem.slot;
				List<InvStat> list = ınvGameItem.CalculateStats();
				int i = 0;
				for (int count = list.Count; i < count; i++)
				{
					InvStat ınvStat = list[i];
					if (ınvStat.amount != 0)
					{
						text2 = ((ınvStat.amount >= 0) ? (text2 + "\n[00FF00]+" + ınvStat.amount) : (text2 + "\n[FF0000]" + ınvStat.amount));
						if (ınvStat.modifier == InvStat.Modifier.Percent)
						{
							text2 += "%";
						}
						text2 = text2 + " " + ınvStat.id.ToString() + "[-]";
					}
				}
				if (!string.IsNullOrEmpty(baseItem.description))
				{
					text2 = text2 + "\n[FF9900]" + baseItem.description;
				}
				UITooltip.ShowText(text2);
				return;
			}
		}
		UITooltip.ShowText(null);
	}

	protected abstract InvGameItem Replace(InvGameItem item);

	private void Update()
	{
		InvGameItem ınvGameItem = this.observedItem;
		if (this.mItem == ınvGameItem)
		{
			return;
		}
		this.mItem = ınvGameItem;
		InvBaseItem ınvBaseItem = ınvGameItem?.baseItem;
		if (this.label != null)
		{
			string text = ınvGameItem?.name;
			if (string.IsNullOrEmpty(this.mText))
			{
				this.mText = this.label.text;
			}
			this.label.text = ((text == null) ? this.mText : text);
		}
		if (this.icon != null)
		{
			if (ınvBaseItem == null || ınvBaseItem.iconAtlas == null)
			{
				this.icon.enabled = false;
			}
			else
			{
				this.icon.atlas = ınvBaseItem.iconAtlas;
				this.icon.spriteName = ınvBaseItem.iconName;
				this.icon.enabled = true;
				this.icon.MakePixelPerfect();
			}
		}
		if (this.background != null)
		{
			this.background.color = ınvGameItem?.color ?? Color.white;
		}
	}

	private void UpdateCursor()
	{
		if (UIItemSlot.mDraggedItem != null && UIItemSlot.mDraggedItem.baseItem != null)
		{
			UICursor.Set(UIItemSlot.mDraggedItem.baseItem.iconAtlas, UIItemSlot.mDraggedItem.baseItem.iconName);
		}
		else
		{
			UICursor.Clear();
		}
	}
}
