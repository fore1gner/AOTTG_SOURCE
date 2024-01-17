using UnityEngine;
using Utility;

namespace Replay;

internal class ReplayManager : MonoBehaviour
{
	private static ReplayManager _instance;

	public static void Init()
	{
		ReplayManager._instance = SingletonFactory.CreateSingleton(ReplayManager._instance);
	}
}
