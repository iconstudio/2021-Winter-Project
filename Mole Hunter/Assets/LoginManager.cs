using System.Collections;
using System.Collections.Generic;

using Photon;
using ExPhoton = ExitGames.Client.Photon;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class GameManager : PunBehaviour
{
	public static readonly string GAME_VERSION = "0.0.1";

	public static ExPhoton.Hashtable player_attributes = new();

	public class MolePlayer : Photon.MonoBehaviour
	{
		private uint __Photon_id;

		public string Nickname { get; set; }
		public uint ID { get => __Photon_id; }
		public uint Number_of_games { get; set; }
		public uint Wins { get; set; }
		public uint Loses { get; set; }
		public uint Draws { get; set; }
		public ulong Score { get; set; }
	}

	public enum META_PLAYER : uint
	{
		NICKNAME = 0,

		PHOTON_ID = 10,
		PW,

		NUM_OF_GAMES = 20,
		WIN, LOSE, DRAW,

		SCORE_RANKING = 30
	}

	public delegate bool PlyerPredicate(PhotonPlayer player);
	PhotonPlayer SearchPlayer(PlyerPredicate predicate)
	{
		if (PhotonNetwork.connected)
		{
			var Plist = PhotonNetwork.playerList;
			foreach (var player in Plist)
			{
				if (predicate(player))
				{
					return player;
				}
			}
		}

		return null;
	}
	bool AnyOfPlayer(PlyerPredicate predicate)
	{
		return (SearchPlayer(predicate) is not null);
	}

	public List<MolePlayer> Players_list;
	public bool PlayerExists(int ID)
	{
		var check = PhotonPlayer.Find(ID);
		return !(check is null);
	}
	public bool PlayerExists(string Name)
	{
		return AnyOfPlayer((PhotonPlayer player) =>
		{
			return (player.NickName == Name);
		});
	}

	public void Awake()
	{
		player_attributes.Add(META_PLAYER.NUM_OF_GAMES, 0);
		player_attributes.Add(META_PLAYER.WIN, 0);
		player_attributes.Add(META_PLAYER.LOSE, 0);
		player_attributes.Add(META_PLAYER.DRAW, 0);
		player_attributes.Add(META_PLAYER.SCORE_RANKING, 0);
	}
	public void Start()
	{
		PhotonNetwork.SetPlayerCustomProperties(player_attributes);
		PhotonNetwork.ConnectUsingSettings(GameManager.GAME_VERSION);

		PhotonHandler.StopFallbackSendAckThread();
	}

	public override void OnConnectedToPhoton()
	{
		print("Connection Succeed");
	}
	public override void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError(cause);
	}
}

public class LoginManager : PunBehaviour
{
	public GameManager My_manager;
	public TMP_InputField Inputfield_nickname;
	public Button Button_signup;
	public TMP_Text Text_notification;

	public void Awake()
	{
		My_manager = FindObjectOfType<GameManager>();
		if (My_manager is null)
			throw new System.Exception("The game manager does not exist!");

		var myComponent = GameObject.Find("NickNameInputField");
		Inputfield_nickname = myComponent.GetComponent<TMP_InputField>();

		myComponent = GameObject.Find("NickNameInputField");
		Button_signup = myComponent.GetComponent<Button>();

		myComponent = GameObject.Find("NickNameInputField");
		Text_notification = myComponent.GetComponent<TMP_Text>();
	}
	public void Start()
	{

	}
	public void Update()
	{
		if (Input.GetKeyDown("Submit"))
		{
			var My_nickname = Inputfield_nickname.text.Trim();
			if (0 < My_nickname.Length)
			{
				var player_check = My_manager.PlayerExists(0);

				if (player_check)
				{
					PlayerPrefs.SetString("NickName", My_nickname);
				}
			}
		}
	}

}
