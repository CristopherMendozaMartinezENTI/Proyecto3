using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeType
{
    FadeIn,
    FadeOut
}

public class ImageFader : MonoBehaviour
{
    [SerializeField] 
    private FadeType fadeType;
    [SerializeField]
    private RawImage img;
    public float fadeSpeed = 5.0f;

    public void OnEnable()
    {
        if (fadeType == FadeType.FadeIn)
        {
            StartCoroutine(FadeIn());
        }

        else if (fadeType == FadeType.FadeOut)
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime * (1.0f / fadeSpeed))
        {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime * (1.0f / fadeSpeed))
        {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }
}
