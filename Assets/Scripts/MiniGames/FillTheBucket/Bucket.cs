using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Bucket : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField] private Image TargetImage;
    [SerializeField] private Sprite FilledSprite;
    [SerializeField] private Sprite FullFilledSprite;
    [SerializeField] private Sprite EmptySprite;
    [SerializeField] private Sprite PouringSprite;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI Text;

    [Header("Setting")]
    [SerializeField] private int MaxCapacity = 3;
    [SerializeField] public bool canBeFilled;
    public bool CanBeFilled => canBeFilled;
    [SerializeField] private bool bOverflowSensitive;

    [Header("Sound")]
    [SerializeField] private AudioClip WaterSound;

    private Button Btn;
    private RectTransform Rect;
    private Vector2 InitialPosition;
    public Vector2 PickupPosition;
    public float SpriteHalfWidth;
    private Coroutine CurrentCoroutine = null;
    public int CurrentAmount = 0;
    public void EnableButton()
    {
        if (Btn != null)
        {
            Btn.enabled = true;
        }
    }
    private void Awake()
    {
        if (!TryGetComponent(out Rect))
        {
            Debug.Log("Can't Find RectTransform in Bucket");
        }
        if (!TryGetComponent(out Btn))
        {
            Debug.Log("Can't Find Button in Bucket");
        }
        InitialPosition = Rect.anchoredPosition;
        PickupPosition = InitialPosition + Vector2.up * 300f;
        SpriteHalfWidth = Rect.rect.width / 2f;
    }
    public void Reset()
    {
        CurrentAmount = 0;
        Btn.enabled = true;
        UpdateSprite(EmptySprite);
        UpdateText();
    }
    public bool CanFill(int AddAmount)
    {
        return bOverflowSensitive
            ? CurrentAmount + AddAmount <= MaxCapacity
            : CurrentAmount < MaxCapacity;
    }
    public int Fill(int AddAmount)
    {
        int Space = MaxCapacity - CurrentAmount;
        int AddedAmount = Math.Min(Space, AddAmount);

        CurrentAmount += AddedAmount;

        if (AudioManager.Instance != null && WaterSound != null)
        {
            AudioManager.Instance.PlaySFX(WaterSound);
        }
        UpdateSprite(GetBucketStateSprite());
        UpdateText();
        EnableButton();
        return AddAmount - AddedAmount;
    }
    public void PickUpBucket()
    {
        StopCurrentCoroutine();
        CurrentCoroutine = StartCoroutine(
            MoveTo(PickupPosition, 0.25f)
        );
    }
    public void PickDownBucket()
    {
        StopCurrentCoroutine();
        CurrentCoroutine = StartCoroutine(
            MoveTo(InitialPosition, 0.25f)
        );
        EnableButton();
    }
    public void DoPouring(Bucket TargetBucket)
    {
        StopCurrentCoroutine();
        Vector2 TargetPosition = TargetBucket.PickupPosition;
        TargetPosition.x += Rect.localScale.x * SpriteHalfWidth;

        CurrentCoroutine = StartCoroutine(
            DoPouringRoutine(TargetPosition, TargetBucket)
        );
    }
    private IEnumerator DoPouringRoutine(Vector2 TargetPosition, Bucket TargetBucket)
    {
        yield return MoveTo(TargetPosition, 0.25f);
        yield return Pouring(TargetBucket);
        yield return MoveTo(PickupPosition, 0.25f);
        yield return MoveTo(InitialPosition, 0.25f);
        EnableButton();
    }
    private IEnumerator Pouring(Bucket targetBucket)
    {
        UpdateSprite(PouringSprite);
        CurrentAmount = targetBucket.Fill(CurrentAmount);
        yield return new WaitForSeconds(0.3f);
        UpdateSprite(GetBucketStateSprite());
        UpdateText();
    }

    private IEnumerator MoveTo(Vector2 Target, float Duration)
    {
        Vector2 Start = Rect.anchoredPosition;
        float time = 0f;

        while (time < Duration)
        {
            time += Time.deltaTime;
            float t = time / Duration;

            Rect.anchoredPosition = Vector2.Lerp(Start, Target, t);
            yield return null;
        }
        Rect.anchoredPosition = Target;
    }
    private void StopCurrentCoroutine()
    {
        if (CurrentCoroutine != null)
        {
            StopCoroutine(CurrentCoroutine);
            CurrentCoroutine = null;
        }
    }
    private void UpdateSprite(Sprite targetSprite)
    {
        if (TargetImage != null && targetSprite != null)
        {
            TargetImage.sprite = targetSprite;
        }
    }
    private Sprite GetBucketStateSprite()
    {
        return CurrentAmount == 0
            ? EmptySprite
            : CurrentAmount == MaxCapacity
                ? FullFilledSprite
                : FilledSprite;
    }
    private void UpdateText()
    {
        Text.text = $"{CurrentAmount}L / {MaxCapacity}L";
    }
}