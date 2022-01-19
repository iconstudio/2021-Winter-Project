using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SignInButtonClick : MonoBehaviour
{
	public Button Login_button;
	public UnityAction Login_button_click;

	public void Awake()
	{
		Login_button = GetComponent<Button>();
	}
	void Start()
	{
	}
	void Update()
	{

	}
}
