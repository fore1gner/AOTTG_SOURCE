using UnityEngine;
using Utility;

namespace ApplicationManagers;

internal class DebugTesting : MonoBehaviour
{
	private static DebugTesting _instance;

	public static void Init()
	{
		DebugTesting._instance = SingletonFactory.CreateSingleton(DebugTesting._instance);
	}

	public static void RunTests()
	{
		_ = ApplicationConfig.DevelopmentMode;
	}

	public static void Log(object message)
	{
		Debug.Log(message);
	}

	private void Update()
	{
	}

	public static void RunDebugCommand(string command)
	{
		if (!ApplicationConfig.DevelopmentMode)
		{
			Debug.Log("Debug commands are not available in release mode.");
			return;
		}
		string[] array = command.Split(' ');
		if (array[0] == "spawnasset")
		{
			string text = array[1];
			string[] array2 = array[2].Split(',');
			Vector3 position = new Vector3(float.Parse(array2[0]), float.Parse(array2[1]), float.Parse(array2[2]));
			string[] array3 = array[3].Split(',');
			Object.Instantiate(rotation: new Quaternion(float.Parse(array3[0]), float.Parse(array3[1]), float.Parse(array3[2]), float.Parse(array3[3])), original: FengGameManagerMKII.RCassets.Load(text), position: position);
		}
		else
		{
			Debug.Log("Invalid debug command.");
		}
	}
}
