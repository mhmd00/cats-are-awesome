using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;

public class CardManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] LevleUIManager levelUIManager;
    [SerializeField] GameManager gameManager;

    private List<CardBehaviour> flippedCards = new List<CardBehaviour>();
    private bool isCardInteractionLocked = false;

    public bool IsCardInteractionLocked => isCardInteractionLocked;

    public void LockCardInteraction()
    {
        isCardInteractionLocked = true;
    }

    public void UnlockCardInteraction()
    {
        isCardInteractionLocked = false;
    }

    public void AssignImagesToCards(LevelData levelData, GameObject[] cards)
    {
        Sprite[] shuffledImages = ShuffleImages(levelData);

        for (int i = 0; i < cards.Length; i++)
        {
            CardBehaviour cardBehaviour = cards[i].GetComponent<CardBehaviour>();
            Button cardButton = cards[i].GetComponent<Button>();

            if (cardBehaviour != null && shuffledImages.Length > i)
            {
                cardBehaviour.frontImage.GetComponent<Image>().sprite = shuffledImages[i];

                cardButton.onClick.RemoveAllListeners();
                cardButton.onClick.AddListener(() => OnCardFlipped(cardBehaviour));
            }
        }
    }

    private Sprite[] ShuffleImages(LevelData levelData)
    {
        int rows = levelData.rows;
        int columns = levelData.columns;

        Sprite[] originalImages = levelData.cardImages;
        Sprite[] images = new Sprite[rows * columns];

        int totalPairs = (rows * columns) / 2;

        for (int i = 0; i < totalPairs; i++)
        {
            images[2 * i] = originalImages[i % originalImages.Length];
            images[2 * i + 1] = originalImages[i % originalImages.Length];
        }

        for (int i = images.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Sprite temp = images[i];
            images[i] = images[randomIndex];
            images[randomIndex] = temp;
        }

        return images;
    }

    public void AdjustCardSize(int rows, int columns, GridLayoutGroup gridLayout, Transform gridParent)
    {
        RectTransform gridRect = gridParent.GetComponent<RectTransform>();
        float gridWidth = gridRect.rect.width;
        float gridHeight = gridRect.rect.height;

        float cardWidth = gridWidth / columns - gridLayout.spacing.x;
        float cardHeight = gridHeight / rows - gridLayout.spacing.y;

        float cardSize = Mathf.Min(cardWidth, cardHeight);
        gridLayout.cellSize = new Vector2(cardSize, cardSize);
    }

    public void OnCardFlipped(CardBehaviour flippedCard)
    {
        if (isCardInteractionLocked || levelUIManager.IsCountdownActive()) return;

        if (flippedCard.IsFlipped)
        {
            return;
        }

        flippedCards.Add(flippedCard);
        flippedCard.FlipCard();

        if (flippedCards.Count == 2)
        {
            CheckForMatch();
        }
    }



    private void CheckForMatch()
    {
        LockCardInteraction();

        CardBehaviour card1 = flippedCards[0];
        CardBehaviour card2 = flippedCards[1];

        if (card1.frontImage.GetComponent<Image>().sprite == card2.frontImage.GetComponent<Image>().sprite)
        {
            levelUIManager.UpdateMatches();
            SoundManager.Instance.PlaySound("RightMatch");
            HideMatchedCards(card1, card2);
            gameManager.CheckUserVictory();
        }
        else
        {
            SoundManager.Instance.PlaySound("WrongMatch");
            gameManager.CheckTrialStatus(false);
            ResetCards(card1, card2);
        }

        levelUIManager.UpdateTurns();
    }

    private void HideMatchedCards(CardBehaviour card1, CardBehaviour card2)
    {
        Sequence hideSequence = DOTween.Sequence();
        hideSequence.Append(card1.transform.DOScale(0, 0.25f).SetEase(Ease.OutQuad))
                    .Join(card2.transform.DOScale(0, 0.25f).SetEase(Ease.OutQuad))
                    .OnComplete(() =>
                    {
                        card1.HideCard();
                        card2.HideCard();
                        flippedCards.Clear();
                        UnlockCardInteraction();
                    });
    }

    private void ResetCards(CardBehaviour card1, CardBehaviour card2)
    {
        StartCoroutine(ResetCardsAfterDelay(card1, card2));
    }

    private IEnumerator ResetCardsAfterDelay(CardBehaviour card1, CardBehaviour card2)
    {
        yield return new WaitForSeconds(1f);

        card1.ResetCard();
        card2.ResetCard();

        flippedCards.Clear();
        UnlockCardInteraction();
    }
}
