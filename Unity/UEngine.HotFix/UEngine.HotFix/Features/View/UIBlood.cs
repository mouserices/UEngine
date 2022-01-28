using System;
using DG.Tweening;
// using TMPro;
using UnityEngine;
// using UnityEngine.UI;

public class UIBlood : MonoBehaviour
{
    private void Start()
    {
        // var textMeshPro = GetComponent<TextMeshProUGUI>();
        // // Grab a free Sequence to use
        // Sequence mySequence = DOTween.Sequence();
        //
        // mySequence.Append(textMeshPro.DOFade(1, 0.2f).From(0));
        // mySequence.Join(transform.DOScale(Vector3.one * 2f, 0.5f));
        // mySequence.Append(transform.DOScale(Vector3.one * 1f, 0.5f));
        // mySequence.Join(transform.DOLocalMoveY(this.transform.localPosition.y + 150f, 0.8f));
        // mySequence.Append(textMeshPro.DOFade(0, 0.2f).From(1));
        // mySequence.AppendCallback(this.OnAnimateComplete);
    }

    private void OnAnimateComplete()
    {
        Destroy(this.gameObject);
    }
}