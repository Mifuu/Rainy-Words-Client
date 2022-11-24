using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Conclusion : MonoBehaviour
{
    public Image topContainer;
    public Image midContainer;
    public Image lowContainer;
    public float waitBeforeStart = 1.8f;
    public float betweenContainerPeriod = 0.8f;
    public float containerFadeTime = 1f;

    void OnEnable() {
        StartCoroutine(EnableCR());
    }

    IEnumerator EnableCR() {
        topContainer.gameObject.SetActive(false);
        midContainer.gameObject.SetActive(false);
        lowContainer.gameObject.SetActive(false);

        yield return new WaitForSeconds(waitBeforeStart);

        topContainer.gameObject.SetActive(true);
        StartCoroutine(ImageFade(topContainer));
        yield return new WaitForSeconds(betweenContainerPeriod);

        midContainer.gameObject.SetActive(true);
        StartCoroutine(ImageFade(midContainer));
        yield return new WaitForSeconds(betweenContainerPeriod);

        lowContainer.gameObject.SetActive(true);
        StartCoroutine(ImageFade(lowContainer));
        yield return new WaitForSeconds(betweenContainerPeriod);
    }

    IEnumerator ImageFade(Image i) {
        float stepTime = 1 / (containerFadeTime * 30);
        float stepValue = 1 / (containerFadeTime * 30);

        float a = 1;
        i.color = new Color(i.color.r, i.color.g, i.color.b, a);

        while (a > 0) {
            yield return new WaitForSeconds(stepTime);
            a -= stepValue;
            if (a < 0) a = 0;
            i.color = new Color(i.color.r, i.color.g, i.color.b, a);
        }
    }
}
