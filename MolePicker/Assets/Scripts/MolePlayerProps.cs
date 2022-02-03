using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon;
using PN = PhotonNetwork;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MolePlayerProps : UnityEngine.MonoBehaviour
{
	public const string PropColor = "colour";
}

public static class PlayerColorExtension
{
	public static void SetColor(this PhotonPlayer player, Color newColor)
	{
		Hashtable data = new Hashtable();
		data[MolePlayerProps.PropColor] = newColor;

		player.SetCustomProperties(data);
	}

	public static Color GetColor(this PhotonPlayer player)
	{
		object color;
		if (player.CustomProperties.TryGetValue(MolePlayerProps.PropColor, out color))
		{
			return (Color)color;
		}

		return Color.gray;
	}
}
