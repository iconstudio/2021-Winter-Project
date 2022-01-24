using System;
using System.Collections;

using Photon;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : PunBehaviour
{
	public enum PHASE
	{
		SIGNIN, LOBBY, IN_ROOM
	}
	public PHASE Phase
	{
		get => default;
		set
		{
			switch (value)
			{
				case PHASE.SIGNIN:
				{
					UI_signin?.SetActive(true);
					UI_lobby?.SetActive(false);
					UI_room?.SetActive(false);
				}
				break;

				case PHASE.LOBBY:
				{
					UI_signin?.SetActive(false);
					UI_lobby?.SetActive(true);
					UI_room?.SetActive(false);
				}
				break;

				case PHASE.IN_ROOM:
				{
					UI_signin?.SetActive(false);
					UI_lobby?.SetActive(false);
					UI_room?.SetActive(true);
				}
				break;
			}
		}
	}

	private const string MsgSignIn = "Please enter your ID.";
	public string? Error_msg = null;

	private GameObject UI_signin, UI_lobby, UI_room;
	public TMP_InputField Inputfield_nickname;
	public Button Button_signup;
	public TextMeshProUGUI Text_notification, Text_network, Text_room_list;

	public void LoginAndJoinLobbyProceed()
	{
		if (MoleHunter.Is_connected
			&& !PhotonNetwork.insideLobby
			&& Inputfield_nickname is not null)
		{
			var My_nickname = Inputfield_nickname.text.Trim();
			if (0 < My_nickname.Length)
			{
				var player_check = MoleHunter.PlayerExists(My_nickname);
				if (!player_check)
				{
					PlayerPrefs.SetString("NickName", My_nickname);
					PhotonNetwork.playerName = My_nickname;

					print("Player's nickname is " + My_nickname + ".");

					JoinLobby();
				}
				else
				{
					StartCoroutine(MsgOnSignInFailed());
				}
			}
		}
	}
	public bool JoinLobby()
	{
		var joined = PhotonNetwork.JoinLobby();
		if (joined)
		{
			print("Joined to the lobby!");
			Phase = PHASE.LOBBY;
		}
		else
		{
			StartCoroutine(MsgOnJoinFailed());
			Phase = PHASE.SIGNIN;
		}
		return joined;
	}
	public IEnumerator MsgOnSignInFailed()
	{
		Text_notification.text = "Error Appeared: Cannot sign in!";
		yield return new WaitForSeconds(3f);

		Text_notification.text = MsgSignIn;
		yield return null;
	}
	public IEnumerator MsgOnJoinFailed()
	{
		Text_notification.text = "Error Appeared: Cannot join the lobby!";
		yield return new WaitForSeconds(3f);

		Text_notification.text = MsgSignIn;
		yield return null;
	}

	public void Awake()
	{
		UI_signin = GameObject.Find("SignInUI");
		UI_lobby = GameObject.Find("LobbyUI");
		UI_room = GameObject.Find("RoomsUI");

		Phase = PHASE.SIGNIN;

		var obj = GameObject.Find("NickNameInputField");
		Inputfield_nickname = obj.GetComponent<TMP_InputField>();

		obj = GameObject.Find("SignInButton");
		Button_signup = obj.GetComponent<Button>();

		obj = GameObject.Find("NickNameNotification");
		Text_notification = obj.GetComponent<TextMeshProUGUI>();

		obj = GameObject.Find("NetworkNotification");
		Text_network = obj.GetComponent<TextMeshProUGUI>();

		obj = GameObject.Find("NetworkNotification");
		Text_room_list = obj.GetComponent<TextMeshProUGUI>();
	}
	public void Start()
	{
	}
	public void Update()
	{
		{
			switch (Phase)
			{
				case PHASE.SIGNIN:
				{
					if (MoleHunter.Is_connected)
					{
						Text_network.text = "Server is connected";

						if (Input.GetButtonDown("Submit"))
						{
							LoginAndJoinLobbyProceed();
						}
					}
					else
					{
						Text_network.text = "Server is disconnected";
					}
				}
				break;

				case PHASE.LOBBY:
				{
					if (MoleHunter.Is_connected)
					{
						Text_network.text = "Server is connected";

						if (!PhotonNetwork.insideLobby)
						{
							if (!JoinLobby())
							{
								Phase = PHASE.SIGNIN;
							}
						}
					}
					else
					{
						Text_network.text = "Server is disconnected";

						PhotonNetwork.RefreshCloudServerRating();
						if (!PhotonNetwork.Reconnect())
						{
							throw new Exception("Network is down");
						}
					}
				}
				break;

				case PHASE.IN_ROOM:
				{
					if (PhotonNetwork.inRoom)
					{
						MoleHunter.My_room = PhotonNetwork.room.Name;
					}
					else
					{
						var lastRoom = MoleHunter.My_room;

						if (lastRoom is not null)
						{
							if (!PhotonNetwork.ReJoinRoom(lastRoom))
							{
								PhotonNetwork.LeaveRoom();
								JoinLobby();
							}
						}
					}
				}
				break;
			}
		}
	}

	void OnDisconnectedFromPhoton(PhotonMessageInfo info)
	{
		print(info.ToString());
	}
}
