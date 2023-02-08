using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class check_connection : MonoBehaviour
{

    public GameObject offlinepanel;

    IEnumerator check() //Kollar network connection varje sekund.
    {
        
        yield return new WaitForSeconds(1);
        
        if (Application.internetReachability == NetworkReachability.NotReachable) //Kollar network connection
                {
                    offlinepanel.SetActive(true); // Detta Gör en panel "Offline_Panel" visible om det finns ingen connection.
	
                }
    }

    // Den funktionen körs när man trycker på knappen "Retry".
    public void restart()
		{
		    SceneManager.LoadScene("M2MqttUnity_Test"); // Laddar om appen.
        }

    // Update is called once per frame
    void Update()
        {
            StartCoroutine(check());
            
        }
}