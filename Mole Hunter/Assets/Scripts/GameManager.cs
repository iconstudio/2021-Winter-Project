using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon;
using ExPhoton = ExitGames.Client.Photon;

public class GameManager : PunBehaviour
{
	public static readonly string GAME_VERSION = "0.0.1";

	public static ExPhoton.Hashtable player_attributes = new();
	public delegate bool PlyerPredicate(PhotonPlayer player);
	public enum META_PLAYER : uint
	{
		NICKNAME = 0,

		PHOTON_ID = 10,
		PW,

		NUM_OF_GAMES = 20,
		WIN, LOSE, DRAW,

		SCORE_RANKING = 30
	}

	public static PhotonPlayer SearchPlayer(PlyerPredicate predicate)
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
	public static bool AnyOfPlayer(PlyerPredicate predicate)
	{
		return (SearchPlayer(predicate) is not null);
	}
	public static bool PlayerExists(int ID)
	{
		var check = PhotonPlayer.Find(ID);
		return !(check is null);
	}
	public static bool PlayerExists(string Name)
	{
		return AnyOfPlayer((PhotonPlayer player) =>
		{
			return (player.NickName == Name);
		});
	}
	private static void PrintError(DisconnectCause error)
	{
		Debug.LogError(error);
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
		PrintError(cause);
	}
	public override void OnConnectionFail(DisconnectCause cause)
	{
		PrintError(cause);
	}
}
