using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon;

public class MoleHole : PunBehaviour
{
	public enum PHASE
	{
		/// <summary>
		/// 평소 상태
		/// </summary>
		NONE,
		/// <summary>
		/// 나올 준비를 하는 상태
		/// </summary>
		READY,
		/// <summary>
		/// 다시 들어가는 상태
		/// </summary>
		FALLBACK,
		/// <summary>
		/// 완전히 나온 상태
		/// </summary>
		OUT
	}


}
