using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AirWall : MonoBehaviour
{
    public bool Display;
    public float FadeInTime=0.2f;
    public float DisplayTime=2f;
    public float Staytime=-1f;
    void Start()
    {
        Display = false;
        Staytime = -1f;
    }

    void Update()
    {
        if (Staytime < DisplayTime && Staytime>=0f) {

            Staytime += Time.deltaTime;
        }
        else if(Staytime >=0f)
        {
            StartCoroutine(FadeOut(0, FadeInTime));
            Staytime = -1f;
        }

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        // ������Ķ����Ƿ������
        if (other.gameObject.CompareTag("Player"))
        {
            Staytime = 0;
            if (!Display)
            {
                
                Display = true;
                StartCoroutine(FadeIn(0, FadeInTime));
            }
        }
    }

    
    //private void OnCollisionExit2D(Collision2D other)
    //{
    //    // ����뿪�Ķ����Ƿ������
    //    if (other.gameObject.CompareTag("Player"))
    //    {
           

    //    }
    //}
    public IEnumerator FadeIn(float time,float FadeInTime)
    {
        Debug.Log("FadeIn");
        Display=true;
        while (time < FadeInTime)
        {
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, time / FadeInTime);
            time += Time.deltaTime;
            yield return null;
        }

    }

    public IEnumerator FadeOut(float time, float FadeOutTime)
    {
        while (time < FadeOutTime)
        {
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, (FadeOutTime - time) / FadeOutTime);// (1-time) / FadeOutTime);
            time += Time.deltaTime;
            yield return null;
        }
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0f);
        Display = false;

    }
}
