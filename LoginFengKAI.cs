using System.Collections;
using UnityEngine;

public class LoginFengKAI : MonoBehaviour
{
	private string ChangeGuildURL = "http://aotskins.com/version/guild.php";

	private string ChangePasswordURL = "http://fenglee.com/game/aog/change_password.php";

	private string CheckUserURL = "http://aotskins.com/version/login.php";

	private string ForgetPasswordURL = "http://fenglee.com/game/aog/forget_password.php";

	public string formText = string.Empty;

	private string GetInfoURL = "http://aotskins.com/version/getinfo.php";

	public PanelLoginGroupManager loginGroup;

	public GameObject output;

	public GameObject output2;

	public GameObject panelChangeGUILDNAME;

	public GameObject panelChangePassword;

	public GameObject panelForget;

	public GameObject panelLogin;

	public GameObject panelRegister;

	public GameObject panelStatus;

	public static PlayerInfoPHOTON player;

	private static string playerGUILDName = string.Empty;

	private static string playerName = string.Empty;

	private static string playerPassword = string.Empty;

	private string RegisterURL = "http://fenglee.com/game/aog/signup_check.php";

	public void cGuild(string name)
	{
		if (LoginFengKAI.playerName == string.Empty)
		{
			this.logout();
			NGUITools.SetActive(this.panelChangeGUILDNAME, state: false);
			NGUITools.SetActive(this.panelLogin, state: true);
			this.output.GetComponent<UILabel>().text = "Please sign in.";
		}
		else
		{
			base.StartCoroutine(this.changeGuild(name));
		}
	}

	private IEnumerator changeGuild(string name)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("name", LoginFengKAI.playerName);
		wWWForm.AddField("guildname", name);
		WWW wWW = new WWW(this.ChangeGuildURL, wWWForm);
		yield return wWW;
		if (wWW.error == null)
		{
			this.output.GetComponent<UILabel>().text = wWW.text;
			if (wWW.text.Contains("Guild name set."))
			{
				NGUITools.SetActive(this.panelChangeGUILDNAME, state: false);
				NGUITools.SetActive(this.panelStatus, state: true);
				this.StartCoroutine(this.getInfo());
			}
			wWW.Dispose();
		}
		else
		{
			MonoBehaviour.print(wWW.error);
		}
	}

	private IEnumerator changePassword(string oldpassword, string password, string password2)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("userid", LoginFengKAI.playerName);
		wWWForm.AddField("old_password", oldpassword);
		wWWForm.AddField("password", password);
		wWWForm.AddField("password2", password2);
		WWW wWW = new WWW(this.ChangePasswordURL, wWWForm);
		yield return wWW;
		if (wWW.error == null)
		{
			this.output.GetComponent<UILabel>().text = wWW.text;
			if (wWW.text.Contains("Thanks, Your password changed successfully"))
			{
				NGUITools.SetActive(this.panelChangePassword, state: false);
				NGUITools.SetActive(this.panelLogin, state: true);
			}
			wWW.Dispose();
		}
		else
		{
			MonoBehaviour.print(wWW.error);
		}
	}

	private void clearCOOKIE()
	{
		LoginFengKAI.playerName = string.Empty;
		LoginFengKAI.playerPassword = string.Empty;
	}

	public void cpassword(string oldpassword, string password, string password2)
	{
		if (LoginFengKAI.playerName == string.Empty)
		{
			this.logout();
			NGUITools.SetActive(this.panelChangePassword, state: false);
			NGUITools.SetActive(this.panelLogin, state: true);
			this.output.GetComponent<UILabel>().text = "Please sign in.";
		}
		else
		{
			base.StartCoroutine(this.changePassword(oldpassword, password, password2));
		}
	}

	private IEnumerator ForgetPassword(string email)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("email", email);
		WWW wWW = new WWW(this.ForgetPasswordURL, wWWForm);
		yield return wWW;
		if (wWW.error == null)
		{
			this.output.GetComponent<UILabel>().text = wWW.text;
			wWW.Dispose();
			NGUITools.SetActive(this.panelForget, state: false);
			NGUITools.SetActive(this.panelLogin, state: true);
		}
		else
		{
			MonoBehaviour.print(wWW.error);
		}
		this.clearCOOKIE();
	}

	private IEnumerator getInfo()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("userid", LoginFengKAI.playerName);
		wWWForm.AddField("password", LoginFengKAI.playerPassword);
		WWW wWW = new WWW(this.GetInfoURL, wWWForm);
		yield return wWW;
		if (wWW.error == null)
		{
			if (wWW.text.Contains("Error,please sign in again."))
			{
				NGUITools.SetActive(this.panelLogin, state: true);
				NGUITools.SetActive(this.panelStatus, state: false);
				this.output.GetComponent<UILabel>().text = wWW.text;
				LoginFengKAI.playerName = string.Empty;
				LoginFengKAI.playerPassword = string.Empty;
			}
			else
			{
				char[] separator = new char[1] { '|' };
				string[] array = wWW.text.Split(separator);
				LoginFengKAI.playerGUILDName = array[0];
				this.output2.GetComponent<UILabel>().text = array[1];
				LoginFengKAI.player.name = LoginFengKAI.playerName;
				LoginFengKAI.player.guildname = LoginFengKAI.playerGUILDName;
			}
			wWW.Dispose();
		}
		else
		{
			MonoBehaviour.print(wWW.error);
		}
	}

	public void login(string name, string password)
	{
		base.StartCoroutine(this.Login(name, password));
	}

	private IEnumerator Login(string name, string password)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("userid", name);
		wWWForm.AddField("password", password);
		wWWForm.AddField("version", UIMainReferences.Version);
		WWW wWW = new WWW(this.CheckUserURL, wWWForm);
		yield return wWW;
		this.clearCOOKIE();
		if (wWW.error == null)
		{
			this.output.GetComponent<UILabel>().text = wWW.text;
			this.formText = wWW.text;
			wWW.Dispose();
			if (this.formText.Contains("Welcome back") && this.formText.Contains("(^o^)/~"))
			{
				NGUITools.SetActive(this.panelLogin, state: false);
				NGUITools.SetActive(this.panelStatus, state: true);
				LoginFengKAI.playerName = name;
				LoginFengKAI.playerPassword = password;
				this.StartCoroutine(this.getInfo());
			}
		}
		else
		{
			MonoBehaviour.print(wWW.error);
		}
	}

	public void logout()
	{
		this.clearCOOKIE();
		LoginFengKAI.player = new PlayerInfoPHOTON();
		LoginFengKAI.player.initAsGuest();
		this.output.GetComponent<UILabel>().text = "Welcome," + LoginFengKAI.player.name;
	}

	private IEnumerator Register(string name, string password, string password2, string email)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("userid", name);
		wWWForm.AddField("password", password);
		wWWForm.AddField("password2", password2);
		wWWForm.AddField("email", email);
		WWW wWW = new WWW(this.RegisterURL, wWWForm);
		yield return wWW;
		if (wWW.error == null)
		{
			this.output.GetComponent<UILabel>().text = wWW.text;
			if (wWW.text.Contains("Final step,to activate your account, please click the link in the activation email"))
			{
				NGUITools.SetActive(this.panelRegister, state: false);
				NGUITools.SetActive(this.panelLogin, state: true);
			}
			wWW.Dispose();
		}
		else
		{
			MonoBehaviour.print(wWW.error);
		}
		this.clearCOOKIE();
	}

	public void resetPassword(string email)
	{
		base.StartCoroutine(this.ForgetPassword(email));
	}

	public void signup(string name, string password, string password2, string email)
	{
		base.StartCoroutine(this.Register(name, password, password2, email));
	}

	private void Start()
	{
		if (LoginFengKAI.player == null)
		{
			LoginFengKAI.player = new PlayerInfoPHOTON();
			LoginFengKAI.player.initAsGuest();
		}
		if (LoginFengKAI.playerName != string.Empty)
		{
			NGUITools.SetActive(this.panelLogin, state: false);
			NGUITools.SetActive(this.panelStatus, state: true);
			base.StartCoroutine(this.getInfo());
		}
		else
		{
			this.output.GetComponent<UILabel>().text = "Welcome," + LoginFengKAI.player.name;
		}
	}
}
