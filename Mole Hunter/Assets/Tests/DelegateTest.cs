using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DelegateTest : MonoBehaviour
{
	public delegate void TestDelegate();
	public TestDelegate testDelegate;

	public void ButtonClick()
	{
		if (testDelegate != null)
			testDelegate();
	}
}