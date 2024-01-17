using System.Collections;
using UnityEngine;

namespace Utility;

internal class CoroutineWithData
{
	public object Result;

	private IEnumerator _target;

	public bool Done;

	public Coroutine Coroutine { get; private set; }

	public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
	{
		this._target = target;
		this.Coroutine = owner.StartCoroutine(this.Run());
	}

	private IEnumerator Run()
	{
		while (this._target.MoveNext())
		{
			this.Result = this._target.Current;
			yield return this.Result;
		}
		this.Done = true;
	}
}
