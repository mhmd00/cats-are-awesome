using DG.Tweening;
using UnityEngine;

public static class UIAnimator
{
    /// <summary>
    /// Activates the UI object and animates it by scaling it up from zero and fading in.
    /// This is typically used for showing a menu or any UI element.
    /// </summary>
    /// <param name="uiObject">The GameObject (UI element) that you want to show.</param>
    /// <param name="duration">The duration of the animation (default is 0.5 seconds).</param>
    public static void ShowUI(GameObject uiObject, float duration = 0.5f)
    {
        if (uiObject == null) return; 

        uiObject.SetActive(true);

        uiObject.transform.localScale = Vector3.zero;

        CanvasGroup canvasGroup = uiObject.GetComponent<CanvasGroup>() ?? uiObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0;

        uiObject.transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);

        canvasGroup.DOFade(1, duration);
    }

    /// <summary>
    /// Hides the UI object by animating it to scale down to zero and fading out.
    /// This is typically used for closing a menu or any UI element.
    /// </summary>
    /// <param name="uiObject">The GameObject (UI element) that you want to hide.</param>
    /// <param name="duration">The duration of the animation (default is 0.5 seconds).</param>
    public static void HideUI(GameObject uiObject, float duration = 0.5f)
    {
        if (uiObject == null) return;
        Debug.Log("<color=cyan>Starting HideUI Animation</color>");
        CanvasGroup canvasGroup = uiObject.GetComponent<CanvasGroup>() ?? uiObject.AddComponent<CanvasGroup>();

        uiObject.transform.DOScale(Vector3.zero, duration).SetEase(Ease.InBack);

        canvasGroup.DOFade(0, duration)
            .OnComplete(() => {
                uiObject.SetActive(false);
            });
    }
}
