using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        // start black instantly
        fadeGroup.alpha = 1f;
        fadeGroup.blocksRaycasts = true;

        StartCoroutine(FadeIn());
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeOutThenLoad(sceneName));
    }

    private IEnumerator FadeOutThenLoad(string sceneName)
    {
        yield return Fade(1f);   // fade to black
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeIn()
    {
        yield return Fade(0f);   // fade to transparent
        fadeGroup.blocksRaycasts = false;
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeGroup.alpha;
        float t = 0f;

        fadeGroup.blocksRaycasts = true;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;

            float normalized = t / fadeDuration;

            // smoother curve (important)
            normalized = Mathf.SmoothStep(0f, 1f, normalized);

            fadeGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, normalized);

            yield return null;
        }

        fadeGroup.alpha = targetAlpha;
    }
}