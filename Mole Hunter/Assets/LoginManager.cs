using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
	public GameObject inputfieldNickName;
	public GameObject buttonSignUp;
	public GameObject textNotification;

	public void Awake()
	{
		var myComponent = GameObject.Find("NickNameInputField");
		//inputfieldNickName = (InputField)myComponent;
		buttonSignUp = GameObject.Find("SignInButton");
		textNotification = GameObject.Find("NickNameNotification");
	}

	void Start()
	{

	}

	void Update()
	{

	}
}
