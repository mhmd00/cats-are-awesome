using DG.Tweening;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    public GameObject frontImage;
    public GameObject backImage;

    private bool isFlipped = false;
    private Sequence currentSequence;

    public bool IsFlipped => isFlipped;

    private void OnDestroy()
    {
        if (transform != null)
        {
            DOTween.Kill(transform, complete: false);
        }
        currentSequence?.Kill(complete: false);
    }

    public void FlipCard(System.Action onFlipComplete = null)
    {
        if (isFlipped || frontImage == null || backImage == null || transform == null) return;

        isFlipped = true;

        KillCurrentSequence();

        currentSequence = DOTween.Sequence();
        currentSequence.Append(transform.DOScaleX(0, 0.15f))
            .AppendCallback(() => {
                if (backImage != null) backImage.SetActive(false);
                if (frontImage != null) frontImage.SetActive(true);
            })
            .Append(transform.DOScaleX(1, 0.15f))
            .OnComplete(() => onFlipComplete?.Invoke());
    }

    public void ResetCard(System.Action onResetComplete = null)
    {
        if (!isFlipped || frontImage == null || backImage == null || transform == null) return;

        isFlipped = false;

        KillCurrentSequence();

        currentSequence = DOTween.Sequence();
        currentSequence.Append(transform.DOScaleX(0, 0.15f))
            .AppendCallback(() => {
                if (frontImage != null) frontImage.SetActive(false);
                if (backImage != null) backImage.SetActive(true);
            })
            .Append(transform.DOScaleX(1, 0.15f))
            .OnComplete(() => onResetComplete?.Invoke());
    }

    public void HideCard()
    {
        if (frontImage == null || backImage == null || transform == null) return;

        isFlipped = false;

        KillCurrentSequence();

        currentSequence = DOTween.Sequence();
        currentSequence.Append(transform.DOScale(0, 0.25f).SetEase(Ease.OutQuad))
            .OnComplete(() =>
            {
                if (frontImage != null) frontImage.SetActive(false);
                if (backImage != null) backImage.SetActive(false);
            });
    }

    private void KillCurrentSequence()
    {
        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill(complete: false);
        }
    }
}
