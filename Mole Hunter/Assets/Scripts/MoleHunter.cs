using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon;
using ExPhoton = ExitGames.Client.Photon;

public class MoleHunter : PunBehaviour
{
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
		Player_attributes.Add(META_PLAYER.NUM_OF_GAMES, 0);
		Player_attributes.Add(META_PLAYER.WIN, 0);
		Player_attributes.Add(META_PLAYER.LOSE, 0);
		Player_attributes.Add(META_PLAYER.DRAW, 0);
		Player_attributes.Add(META_PLAYER.SCORE_RANKING, 0);
		
		PhotonNetwork.SetPlayerCustomProperties(Player_attributes);
		
		DontDestroyOnLoad(gameObject);
	}
	public void Start()
	{
		StartCoroutine(RetryConnections());
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

	public void Connect()
	{
		PhotonNetwork.ConnectUsingSettings(GAME_VERSION);

		PhotonHandler.StopFallbackSendAckThread();
	}
	private IEnumerator RetryConnections()
	{
		print("First trying connections");
		Connect();
		yield return new WaitForSeconds(3f);

		if (Is_connected) yield break;
		print("Second trying connections");
		Connect();
		yield return new WaitForSeconds(3f);

		if (Is_connected) yield break;
		print("Third trying connections");
		Connect();
		yield return null;
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
