using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class DefaultButtonClick : MonoBehaviour
{
	public UnityAction Action_on_click;

	public void OnClick()
	{
		Action_on_click.Invoke();
	}
}