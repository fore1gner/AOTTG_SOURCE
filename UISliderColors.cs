using UnityEngine;

[AddComponentMenu("NGUI/Examples/Slider Colors")]
[RequireComponent(typeof(UISlider))]
[ExecuteInEditMode]
public class UISliderColors : MonoBehaviour
{
	public Color[] colors = new Color[3]
	{
		Color.red,
		Color.yellow,
		Color.green
	};

	private UISlider mSlider;

	public UISprite sprite;

	private void Start()
	{
		this.mSlider = base.GetComponent<UISlider>();
		this.Update();
	}

	private void Update()
	{
		if (!(this.sprite != null) || this.colors.Length == 0)
		{
			return;
		}
		float num = this.mSlider.sliderValue * (float)(this.colors.Length - 1);
		int num2 = Mathf.FloorToInt(num);
		Color color = this.colors[0];
		if (num2 >= 0)
		{
			if (num2 + 1 >= this.colors.Length)
			{
				color = ((num2 >= this.colors.Length) ? this.colors[this.colors.Length - 1] : this.colors[num2]);
			}
			else
			{
				float t = num - (float)num2;
				color = Color.Lerp(this.colors[num2], this.colors[num2 + 1], t);
			}
		}
		color.a = this.sprite.color.a;
		this.sprite.color = color;
	}
}
