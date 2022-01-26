using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TMPro;
using Photon;
using PN = PhotonNetwork;

public class SignInSystem : PunBehaviour
{
	public TextMeshProUGUI Text_id;
	public Text Text_msg;
	private Coroutine Crtine_text_modifier;
	private const string Msg_sign_in = "Please enter your ID.";

	public IEnumerator MsgOnSignInFailed(string info = "")
	{
		Text_msg.text = "Error Appeared: Cannot sign in!\n" + info;
		yield return new WaitForSeconds(3f);

		Text_msg.text = Msg_sign_in;
		yield return null;
	}

	public void OnClickSignInButton()
	{
		if (PN.connectedAndReady && !PN.insideLobby)
		{
			var My_nickname = Text_id.text.Trim();
			if (3 <= My_nickname.Length)
			{
				PlayerPrefs.SetString("NickName", My_nickname);
				PN.playerName = My_nickname;

				print("Player's nickname is " + My_nickname + ".");

				if (!PN.JoinLobby())
				{
					throw new Exception("Cannot join the lobby!");
				}
			}
			else
			{
				if (Crtine_text_modifier is not null)
					StopCoroutine(Crtine_text_modifier);

				var routine = MsgOnSignInFailed("The nickname should be longer than 3 characters.");
				Crtine_text_modifier = StartCoroutine(routine);
			}
		}
	}

	void Start()
	{
		if (!PN.connected)
		{
			GameManager.Connect();
		}
	}

	public override void OnJoinedLobby()
	{
		SceneManager.LoadScene("SceneLobby");
	}
}
