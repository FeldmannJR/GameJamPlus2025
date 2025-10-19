using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class OpacityDownAnimation : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }


    public void AnimateShowAndHide(float duration)
    {
        AsyncAnimateShowAndHide(duration).Forget();
    }

    private async UniTaskVoid AsyncAnimateShowAndHide(float duration)
    {
        await AnimateColor(Color.white);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        await AnimateColor(Color.clear);
    }

    public void Animate()
    {
        AnimateColor(Color.clear).Forget();
    }

    public async UniTask AnimateColor(Color color)
    {
        var spr = GetComponent<SpriteRenderer>();
        if (spr.color == color) return;
        spr.DOColor(color, 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
    }
}