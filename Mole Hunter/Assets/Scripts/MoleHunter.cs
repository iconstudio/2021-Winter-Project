using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon;
using ExPhoton = ExitGames.Client.Photon;

public class MoleHunter : PunBehaviour
{
	private static MoleHunter Instance;
	public static string My_room;

	public static readonly string GAME_VERSION = "0.0.1";
	public static readonly ExPhoton.Hashtable Player_attributes = new();
	public enum META_PLAYER : uint
	{
		NUM_OF_GAMES = 20,
		WIN, LOSE, DRAW,
		SCORE_RANKING = 30
	}
	public delegate bool PlyerPredicate(PhotonPlayer player);

	public void Awake()
	{
		if (Instance is not null)
		{
			Destroy(gameObject);
			return;
		}
		else
		{
			Instance = this;
		}

		Player_attributes.Add(META_PLAYER.NUM_OF_GAMES, 0);
		Player_attributes.Add(META_PLAYER.WIN, 0);
		Player_attributes.Add(META_PLAYER.LOSE, 0);
		Player_attributes.Add(META_PLAYER.DRAW, 0);
		Player_attributes.Add(META_PLAYER.SCORE_RANKING, 0);

		PhotonNetwork.autoCleanUpPlayerObjects = true;
		PhotonNetwork.autoJoinLobby = false;
		PhotonNetwork.SetPlayerCustomProperties(Player_attributes);

		DontDestroyOnLoad(gameObject);
	}

	public override void OnConnectedToPhoton()
	{
		print("Connection Succeed");
	}
	public override void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		PrintError(cause);
	}
	public override void OnConnectionFail(DisconnectCause cause)
	{
		PrintError(cause);
	}
	public override void OnJoinedLobby()
	{
		print("Joined to the lobby.");
	}

	public void Connect()
	{
		StartCoroutine(RetryConnections());
	}
	public void TryConnect()
	{
		PhotonNetwork.ConnectUsingSettings(GAME_VERSION);

		PhotonHandler.StopFallbackSendAckThread();
	}
	private IEnumerator RetryConnections()
	{
		print("First trying connections");
		TryConnect();
		yield return new WaitForSeconds(3f);

		if (Is_connected) yield break;
		print("Second trying connections");
		TryConnect();
		yield return new WaitForSeconds(3f);

		if (Is_connected) yield break;
		print("Third trying connections");
		TryConnect();
		yield return null;
	}

	public static void LoadScene(string scene_name)
	{
		SceneManager.LoadScene(scene_name);
		print("Going to " + scene_name + ".");
	}

	public static bool Is_connected => PhotonNetwork.connected;
	public static PhotonPlayer SearchPlayer(PlyerPredicate predicate)
	{
		if (Is_connected)
		{
			var plist = PhotonNetwork.playerList;
			foreach (var player in plist)
			{
				if (predicate(player))
				{
					return player;
				}
			}
		}

		return null;
	}
	public static bool AnyOfPlayer(PlyerPredicate predicate)
	{
		return (SearchPlayer(predicate) is not null);
	}
	public static bool PlayerExists(int id)
	{
		var check = PhotonPlayer.Find(id);
		return check is not null;
	}
	public static bool PlayerExists(string name)
	{
		return AnyOfPlayer((PhotonPlayer player) => (player.NickName == name));
	}
	private static void PrintError(DisconnectCause error)
	{
		Debug.LogError(error);
	}
}
