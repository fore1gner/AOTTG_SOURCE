using UnityEngine;

namespace UI;

internal class VerticalLineScaler : BaseScaler
{
	public override void ApplyScale()
	{
		float currentCanvasScale = UIManager.CurrentCanvasScale;
		RectTransform component = base.GetComponent<RectTransform>();
		float num = 1f;
		if (num * currentCanvasScale < 1f)
		{
			num = 1f / currentCanvasScale;
		}
		component.sizeDelta = new Vector2(num, component.sizeDelta.y);
	}
}
