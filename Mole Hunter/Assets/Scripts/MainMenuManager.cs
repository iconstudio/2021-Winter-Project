using System;

using Photon;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : PunBehaviour
{
	public TMP_InputField Inputfield_nickname;
	public Button Button_signup;
	public TextMeshProUGUI Text_notification;
	public int? Error_code = null;
	private bool Done = false;

	public void LoginAndJoinLobbyProceed()
	{
		if (MoleHunter.Is_connected && !Done && Inputfield_nickname is not null)
		{
			var My_nickname = Inputfield_nickname.text.Trim();
			if (0 < My_nickname.Length)
			{
				var player_check = MoleHunter.PlayerExists(My_nickname);
				if (!player_check)
				{
					Done = true;

					PlayerPrefs.SetString("NickName", My_nickname);
					PhotonNetwork.playerName = My_nickname;

					print("Player's nickname is " + My_nickname + ".");

					var joined = PhotonNetwork.JoinLobby();
					if (joined)
					{
						print("Joined to the lobby!");
						//MoleHunter.LoadScene("SceneGame");
					}
					else
					{
						throw new Exception("Cannot join the lobby!");
					}
				}
				else
				{
					Error_code = ErrorCode.GameIdAlreadyExists;
				}
			}
		}
	}

	public void Awake()
	{
		var myComponent = GameObject.Find("NickNameInputField");
		Inputfield_nickname = myComponent.GetComponent<TMP_InputField>();

		myComponent = GameObject.Find("SignInButton");
		Button_signup = myComponent.GetComponent<Button>();

		myComponent = GameObject.Find("NickNameNotification");
		Text_notification = myComponent.GetComponent<TextMeshProUGUI>();
	}
	public void Start()
	{
	}
	public void Update()
	{
		if (Error_code is not null)
		{
			Text_notification.text = "Error Appeared: " + Error_code;
		}
		else if (MoleHunter.Is_connected)
		{
			Text_notification.gameObject.SetActive(true);
			if (Error_code is null)
			{
				Text_notification.text = "Network Connected";
			}

			if (Input.GetButtonDown("Submit"))
			{
				LoginAndJoinLobbyProceed();
			}
		}
		else
		{
			Text_notification.gameObject.SetActive(false);
		}
	}

}
