using System.Collections;
using System.Collections.Generic;

using Photon;
using ExPhoton = ExitGames.Client.Photon;

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : PunBehaviour
{
	public TMP_InputField Inputfield_nickname;
	public Button Button_signup;
	public TextMeshProUGUI Text_notification;
	public int? Error_code = null;
	private bool Done = false;

	public void LoginProceed()
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
					print("Player's nickname is " + My_nickname + ".");

					SceneManager.LoadScene("SceneGame");
					print("Going to the game scene.");
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
		if (MoleHunter.Is_connected)
		{
			Text_notification.gameObject.SetActive(true);
			if (Error_code == null)
			{
				Text_notification.text = "Network Connected";
			}
			else
			{
				Text_notification.text = "Error Appeared: " + Error_code;
			}

			if (Input.GetButtonDown("Submit"))
			{
				LoginProceed();
			}
		}
		else
		{
			Text_notification.gameObject.SetActive(false);
		}
	}

}
