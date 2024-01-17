using SimpleJSON;
using UnityEngine;

internal class Test_CSharp : MonoBehaviour
{
	private string m_InGameLog = string.Empty;

	private Vector2 m_Position = Vector2.zero;

	private void OnGUI()
	{
		this.m_Position = GUILayout.BeginScrollView(this.m_Position);
		GUILayout.Label(this.m_InGameLog);
		GUILayout.EndScrollView();
	}

	private void P(string aText)
	{
		this.m_InGameLog = this.m_InGameLog + aText + "\n";
	}

	private void Start()
	{
		this.Test();
		Debug.Log("Test results:\n" + this.m_InGameLog);
	}

	private void Test()
	{
		JSONNode jSONNode = JSONNode.Parse("{\"name\":\"test\", \"array\":[1,{\"data\":\"value\"}]}");
		jSONNode["array"][1]["Foo"] = "Bar";
		this.P("'nice formatted' string representation of the JSON tree:");
		this.P(jSONNode.ToString(string.Empty));
		this.P(string.Empty);
		this.P("'normal' string representation of the JSON tree:");
		this.P(jSONNode.ToString());
		this.P(string.Empty);
		this.P("content of member 'name':");
		this.P(jSONNode["name"]);
		this.P(string.Empty);
		this.P("content of member 'array':");
		this.P(jSONNode["array"].ToString(string.Empty));
		this.P(string.Empty);
		this.P("first element of member 'array': " + jSONNode["array"][0]);
		this.P(string.Empty);
		jSONNode["array"][0].AsInt = 10;
		this.P("value of the first element set to: " + jSONNode["array"][0]);
		this.P("The value of the first element as integer: " + jSONNode["array"][0].AsInt);
		this.P(string.Empty);
		this.P("N[\"array\"][1][\"data\"] == " + jSONNode["array"][1]["data"]);
		this.P(string.Empty);
		string text = jSONNode.SaveToBase64();
		string aText = jSONNode.SaveToCompressedBase64();
		jSONNode = null;
		this.P("Serialized to Base64 string:");
		this.P(text);
		this.P("Serialized to Base64 string (compressed):");
		this.P(aText);
		this.P(string.Empty);
		jSONNode = JSONNode.LoadFromBase64(text);
		this.P("Deserialized from Base64 string:");
		this.P(jSONNode.ToString());
		this.P(string.Empty);
		JSONClass jSONClass = new JSONClass();
		jSONClass["version"].AsInt = 5;
		jSONClass["author"]["name"] = "Bunny83";
		jSONClass["author"]["phone"] = "0123456789";
		jSONClass["data"][-1] = "First item\twith tab";
		jSONClass["data"][-1] = "Second item";
		jSONClass["data"][-1]["value"] = "class item";
		jSONClass["data"].Add("Forth item");
		jSONClass["data"][1] = string.Concat(jSONClass["data"][1], " 'addition to the second item'");
		jSONClass.Add("version", "1.0");
		this.P("Second example:");
		this.P(jSONClass.ToString());
		this.P(string.Empty);
		this.P("I[\"data\"][0]            : " + jSONClass["data"][0]);
		this.P("I[\"data\"][0].ToString() : " + jSONClass["data"][0].ToString());
		this.P("I[\"data\"][0].Value      : " + jSONClass["data"][0].Value);
		this.P(jSONClass.ToString());
	}
}
