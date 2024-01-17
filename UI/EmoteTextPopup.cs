using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class EmoteTextPopup : BasePopup
{
	private const float ShowTime = 3f;

	private Text _text;

	protected Transform _parent;

	protected float _currentShowTime;

	protected bool _isHiding;

	protected Transform _transform;

	protected Camera _camera;

	protected override float AnimationTime => 0.25f;

	protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;

	protected virtual Vector3 offset => Vector3.up * 2.5f;

	public override void Setup(BasePanel parent = null)
	{
		this._text = base.transform.Find("Panel/Text/Label").GetComponent<Text>();
		this._transform = base.transform;
	}

	public void Show(string text, Transform parent)
	{
		this._parent = parent;
		this._currentShowTime = 3f;
		this._isHiding = false;
		this._camera = Camera.main;
		this.SetEmote(text);
		this.SetPosition();
		base.Show();
	}

	protected virtual void SetEmote(string text)
	{
		this._text.text = text;
	}

	protected void SetPosition()
	{
		if (this._parent != null)
		{
			Vector3 position = this._parent.position + this.offset;
			Vector3 position2 = this._camera.WorldToScreenPoint(position);
			this._transform.position = position2;
		}
	}

	protected void LateUpdate()
	{
		this.SetPosition();
		this._currentShowTime -= Time.deltaTime;
		if (this._currentShowTime <= 0f && !this._isHiding)
		{
			this._isHiding = true;
			this.Hide();
		}
	}
}
