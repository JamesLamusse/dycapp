using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLoading : MonoBehaviour
{
    public float ThresholdMax = 0.3f;
    public float FadeSpeed = 1f;
    public float t =0.3f;

    public bool switchFade = true;

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Text>().text == "▆▆▆▆▆▆▆▆")
        {
            if (!switchFade)
            {

                t += Time.deltaTime;
                if (t >= ThresholdMax)
                {
                    switchFade = true;
                }

                GetComponent<Text>().color = new Color((GetComponent<Text>().color.r), GetComponent<Text>().color.g, GetComponent<Text>().color.b, GetComponent<Text>().color.a + FadeSpeed * Time.deltaTime);
            }
            else
            {
                t -= Time.deltaTime;

                if (t <= 0)
                {
                    switchFade = false;
                }

                GetComponent<Text>().color = new Color((GetComponent<Text>().color.r), GetComponent<Text>().color.g, GetComponent<Text>().color.b, GetComponent<Text>().color.a - FadeSpeed * Time.deltaTime);
            }
        }
    }
}
