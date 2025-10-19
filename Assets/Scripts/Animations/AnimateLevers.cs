using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class AnimateLevers : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
    }

    private Dictionary<Transform, Vector3> _scales = new();

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            _scales[child] = child.localScale;
            child.localScale = Vector3.zero;
        }
    }

    public void Animate()
    {
        
        AnimateAsync().Forget();
    }

    private async UniTaskVoid AnimateAsync()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var scale = _scales[child];
            if (scale == Vector3.zero)
            {
                scale = Vector3.one;
            }
            child.DOScale(scale, 0.4f).SetEase(Ease.OutBack);
            await UniTask.Delay(200);
        }
    }
}