using DG.Tweening;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    public GameObject frontImage;
    public GameObject backImage;

    private bool isFlipped = false;

    public bool IsFlipped => isFlipped;

    public void FlipCard(System.Action onFlipComplete = null)
    {
        if (isFlipped) return;
        isFlipped = true;

        Sequence flipSequence = DOTween.Sequence();
        flipSequence.Append(transform.DOScaleX(0, 0.15f))
            .AppendCallback(() => {
                backImage.SetActive(false);
                frontImage.SetActive(true);
            })
            .Append(transform.DOScaleX(1, 0.15f))
            .OnComplete(() => onFlipComplete?.Invoke());
    }

    public void ResetCard(System.Action onResetComplete = null)
    {
        if (!isFlipped) return;
        isFlipped = false;

        Sequence flipBackSequence = DOTween.Sequence();
        flipBackSequence.Append(transform.DOScaleX(0, 0.15f))
            .AppendCallback(() => {
                frontImage.SetActive(false);
                backImage.SetActive(true);
            })
            .Append(transform.DOScaleX(1, 0.15f))
            .OnComplete(() => onResetComplete?.Invoke());
    }

    public void HideCard()
    {
        isFlipped = false;
        Sequence hideSequence = DOTween.Sequence();
        hideSequence.Append(transform.DOScale(0, 0.25f).SetEase(Ease.OutQuad))
                    .OnComplete(() =>
                    {
                        frontImage.SetActive(false);
                        backImage.SetActive(false);
                    });
    }
}
