using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class LeaveQueryMsg : MonoBehaviour
{
	public Image FadeBackground;
	public RectTransform PanelTransform;

	public enum PHASE
	{
		OPENING, IDLE, CLOSING
	}
	public PHASE Phase = PHASE.OPENING;
	public float Life = 0f;
	public float Duration = 0.3f;

	void Awake()
	{
		FadeBackground = GetComponentInChildren<Image>();
		PanelTransform = GameObject.Find("Panel").GetComponentInChildren<RectTransform>();
		PanelTransform.localScale = new Vector3(0f, 0f, 0f);
	}
	void OnEnable()
	{
		Phase = PHASE.OPENING;
	}
	void OnDisable()
	{
		Life = 0f;
	}
	void Update()
	{
		var ratio = Life / Duration;
		FadeBackground.color = new Color(1f, 1f, 1f, ratio);
		var scale = ratio * ratio;
		PanelTransform.localScale = new Vector3(scale, scale, scale);

		switch (Phase)
		{
			case PHASE.OPENING:
			{
				Life = Mathf.Clamp(Life + Time.deltaTime, 0f, Duration);

				if (Duration == Life)
				{
					Phase = PHASE.IDLE;

					gameObject.SetActive(true);
				}
			}
			break;

			case PHASE.IDLE:
			{

			}
			break;

			case PHASE.CLOSING:
			{
				Life = Mathf.Clamp(Life - Time.deltaTime, 0f, Duration);

				if (Life == 0f)
				{
					Phase = PHASE.IDLE;

					gameObject.SetActive(false);
				}
			}
			break;
		}
	}
	public void OnClickYesButton()
	{
		Phase = PHASE.CLOSING;
	}
	public void OnClickNoButton()
	{
		Phase = PHASE.CLOSING;
	}
}
