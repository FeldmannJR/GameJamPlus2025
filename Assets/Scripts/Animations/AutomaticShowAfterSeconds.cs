using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class AutomaticShowAfterSeconds : OpacityDownAnimation
{
    [SerializeField] private float _showAfterSeconds;
    private CancellationTokenSource _cc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
   
        LevelSystem.Instance.OnGameStart += GameStarted;
    }

    private void GameStarted()
    {
        _cc = new CancellationTokenSource();
        AsyncStart().Forget();
        return;
    }

    private async UniTaskVoid AsyncStart()
    {
        Debug.Log("Appear Component");
        var spr = GetComponent<SpriteRenderer>();
        spr.color = Color.clear;
        await UniTask.Delay(TimeSpan.FromSeconds(_showAfterSeconds), cancellationToken: _cc.Token);
        AnimateColor(Color.white).Forget();
    }

    public void HideAndCancel()
    {
        if (_cc != null)
        {
            _cc.Cancel();
            _cc = null;
        }

        AnimateColor(Color.clear).Forget();
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