using UnityEngine;

namespace UI;

internal abstract class BaseScaler : MonoBehaviour
{
	protected virtual void Awake()
	{
		this.ApplyScale();
	}

	public abstract void ApplyScale();
}
