using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace M2MqttUnity.Examples      //blev tvungen att innehålla den filen i namespace M2Mq... 
                                    //för att importera värden från en annan fil som också är i samma namespace
{
public class ui_scripting : MonoBehaviour
{
    public GameObject radar;      

    public void center()            //Den funktionen körs när värdena på distanser ändras.
    {
        StartCoroutine(comparing());      /* IEnumerator för att kunna använda WaitForSeconds*/
        
    }

    IEnumerator comparing()
    {
        M2MqttUnityTest M2MqttUnityTest = this.GetComponent<M2MqttUnityTest>(); /*importar scripten "M2MqttUnityTest.cs"
                                                                                    med alla värden i.*/
        yield return new WaitForSeconds(0.01F);             /*Vi behöver lägga den här för att det tar några millisekunder
                                                         att få värden från sensorna till appen.*/
        string objectname;
        int distance_middle = M2MqttUnityTest.distance1_int;           // Importerar distance värdet från sensorn i mitten 'här;0;0;'
        int distance_left = M2MqttUnityTest.distance3_int;              //0;0;här;
        int distance_right = M2MqttUnityTest.distance2_int;             //0;här;0;

        if (distance_middle<= distance_left && distance_middle<= distance_right) /* Vi jämför mellan distanserna och om värdet 
                                                                                från middle sensron är mindre än de andra då 
                                                                                kör vi följande funktionen som påverkar 
                                                                                både höger och vänster radars.*/
        {
            objectname="left_radar";
            radar_animation(distance_middle,objectname);
            objectname="right_radar";
            radar_animation(distance_middle,objectname);
        }
        else 
        {
            objectname="left_radar";
            radar_animation(distance_left,objectname);
            objectname="right_radar";
            radar_animation(distance_right,objectname);
        }
    }

    private void radar_animation(int distance, string objectname)  // funktionen ändrar animation på högra radarn.  
    {                                               
            radar = GameObject.Find(objectname);            //GameObject.Find(string) hittar objekt by its name.
            if (distance>=150)                         // om avståndet mellan hinder och sensorn är större än 150 cm
                                                       // då 'lyser' alla färgerna på radarn.
            {
                radar.GetComponent<Animator>().Play("full");
            }

            else if (150>distance && distance>=75)
            {
                radar.GetComponent<Animator>().Play("orange");
            }

            else if (75>distance && distance>=30)
            {
                radar.GetComponent<Animator>().Play("dark_orange");
            }

            else if (distance<30)
            {
                radar.GetComponent<Animator>().Play("red");
            }
     }
        



}

}
