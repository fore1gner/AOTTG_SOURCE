using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Slider")]
public class UISlider : IgnoreTimeScale
{
	public enum Direction
	{
		Horizontal,
		Vertical
	}

	public delegate void OnValueChange(float val);

	public static UISlider current;

	public Direction direction;

	public GameObject eventReceiver;

	public Transform foreground;

	public string functionName = "OnSliderChange";

	private Vector2 mCenter = Vector3.zero;

	private BoxCollider mCol;

	private UISprite mFGFilled;

	private Transform mFGTrans;

	private UIWidget mFGWidget;

	private bool mInitDone;

	private Vector2 mSize = Vector2.zero;

	private Transform mTrans;

	public int numberOfSteps;

	public OnValueChange onValueChange;

	[HideInInspector]
	[SerializeField]
	private float rawValue = 1f;

	public Transform thumb;

	public Vector2 fullSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			if (this.mSize != value)
			{
				this.mSize = value;
				this.ForceUpdate();
			}
		}
	}

	public float sliderValue
	{
		get
		{
			float num = this.rawValue;
			if (this.numberOfSteps > 1)
			{
				num = Mathf.Round(num * (float)(this.numberOfSteps - 1)) / (float)(this.numberOfSteps - 1);
			}
			return num;
		}
		set
		{
			this.Set(value, force: false);
		}
	}

	private void Awake()
	{
		this.mTrans = base.transform;
		this.mCol = base.collider as BoxCollider;
	}

	public void ForceUpdate()
	{
		this.Set(this.rawValue, force: true);
	}

	private void Init()
	{
		this.mInitDone = true;
		if (this.foreground != null)
		{
			this.mFGWidget = this.foreground.GetComponent<UIWidget>();
			this.mFGFilled = ((this.mFGWidget == null) ? null : (this.mFGWidget as UISprite));
			this.mFGTrans = this.foreground.transform;
			if (this.mSize == Vector2.zero)
			{
				this.mSize = this.foreground.localScale;
			}
			if (this.mCenter == Vector2.zero)
			{
				this.mCenter = this.foreground.localPosition + this.foreground.localScale * 0.5f;
			}
		}
		else if (this.mCol != null)
		{
			if (this.mSize == Vector2.zero)
			{
				this.mSize = this.mCol.size;
			}
			if (this.mCenter == Vector2.zero)
			{
				this.mCenter = this.mCol.center;
			}
		}
		else
		{
			Debug.LogWarning("UISlider expected to find a foreground object or a box collider to work with", this);
		}
	}

	private void OnDrag(Vector2 delta)
	{
		this.UpdateDrag();
	}

	private void OnDragThumb(GameObject go, Vector2 delta)
	{
		this.UpdateDrag();
	}

	private void OnKey(KeyCode key)
	{
		float num = (((float)this.numberOfSteps <= 1f) ? 0.125f : (1f / (float)(this.numberOfSteps - 1)));
		if (this.direction == Direction.Horizontal)
		{
			switch (key)
			{
			case KeyCode.LeftArrow:
				this.Set(this.rawValue - num, force: false);
				break;
			case KeyCode.RightArrow:
				this.Set(this.rawValue + num, force: false);
				break;
			}
		}
		else
		{
			switch (key)
			{
			case KeyCode.DownArrow:
				this.Set(this.rawValue - num, force: false);
				break;
			case KeyCode.UpArrow:
				this.Set(this.rawValue + num, force: false);
				break;
			}
		}
	}

	private void OnPress(bool pressed)
	{
		if (pressed && UICamera.currentTouchID != -100)
		{
			this.UpdateDrag();
		}
	}

	private void OnPressThumb(GameObject go, bool pressed)
	{
		if (pressed)
		{
			this.UpdateDrag();
		}
	}

	private void Set(float input, bool force)
	{
		if (!this.mInitDone)
		{
			this.Init();
		}
		float num = Mathf.Clamp01(input);
		if (num < 0.001f)
		{
			num = 0f;
		}
		float num2 = this.sliderValue;
		this.rawValue = num;
		float num3 = this.sliderValue;
		if (!force && num2 == num3)
		{
			return;
		}
		Vector3 localScale = this.mSize;
		if (this.direction == Direction.Horizontal)
		{
			localScale.x *= num3;
		}
		else
		{
			localScale.y *= num3;
		}
		if (this.mFGFilled != null && this.mFGFilled.type == UISprite.Type.Filled)
		{
			this.mFGFilled.fillAmount = num3;
		}
		else if (this.foreground != null)
		{
			this.mFGTrans.localScale = localScale;
			if (this.mFGWidget != null)
			{
				if (num3 > 0.001f)
				{
					this.mFGWidget.enabled = true;
					this.mFGWidget.MarkAsChanged();
				}
				else
				{
					this.mFGWidget.enabled = false;
				}
			}
		}
		if (this.thumb != null)
		{
			Vector3 localPosition = this.thumb.localPosition;
			if (this.mFGFilled != null && this.mFGFilled.type == UISprite.Type.Filled)
			{
				if (this.mFGFilled.fillDirection == UISprite.FillDirection.Horizontal)
				{
					localPosition.x = ((!this.mFGFilled.invert) ? localScale.x : (this.mSize.x - localScale.x));
				}
				else if (this.mFGFilled.fillDirection == UISprite.FillDirection.Vertical)
				{
					localPosition.y = ((!this.mFGFilled.invert) ? localScale.y : (this.mSize.y - localScale.y));
				}
				else
				{
					Debug.LogWarning("Slider thumb is only supported with Horizontal or Vertical fill direction", this);
				}
			}
			else if (this.direction == Direction.Horizontal)
			{
				localPosition.x = localScale.x;
			}
			else
			{
				localPosition.y = localScale.y;
			}
			this.thumb.localPosition = localPosition;
		}
		UISlider.current = this;
		if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName) && Application.isPlaying)
		{
			this.eventReceiver.SendMessage(this.functionName, num3, SendMessageOptions.DontRequireReceiver);
		}
		if (this.onValueChange != null)
		{
			this.onValueChange(num3);
		}
		UISlider.current = null;
	}

	private void Start()
	{
		this.Init();
		if (Application.isPlaying && this.thumb != null && this.thumb.collider != null)
		{
			UIEventListener uIEventListener = UIEventListener.Get(this.thumb.gameObject);
			uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressThumb));
			uIEventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uIEventListener.onDrag, new UIEventListener.VectorDelegate(OnDragThumb));
		}
		this.Set(this.rawValue, force: true);
	}

	private void UpdateDrag()
	{
		if (this.mCol != null && UICamera.currentCamera != null && UICamera.currentTouch != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			if (new Plane(this.mTrans.rotation * Vector3.back, this.mTrans.position).Raycast(ray, out var enter))
			{
				Vector3 vector = this.mTrans.localPosition + (Vector3)(this.mCenter - this.mSize * 0.5f);
				Vector3 vector2 = this.mTrans.localPosition - vector;
				Vector3 vector3 = this.mTrans.InverseTransformPoint(ray.GetPoint(enter)) + vector2;
				this.Set((this.direction != 0) ? (vector3.y / this.mSize.y) : (vector3.x / this.mSize.x), force: false);
			}
		}
	}
}
