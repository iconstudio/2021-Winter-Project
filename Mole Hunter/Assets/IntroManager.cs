using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon;
using ExPhoton = ExitGames.Client.Photon;

public class IntroManager : PunBehaviour
{
    public float Period = 3f;

    IEnumerator GotoNextRoom()
    {
        yield return new WaitForSecondsRealtime(Period);

        SceneManager.LoadScene("SceneMain");
    }
    
    void Start()
    {
        StartCoroutine(GotoNextRoom());
    }
}
