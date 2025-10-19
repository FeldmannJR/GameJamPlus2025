using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Audio;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class ShowLocalTip : MonoBehaviour
    {
        private CancellationTokenSource _cc;

        public GameObject go;
        public float ShowTime = 5;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (TryGetPlayer(other, out var pl))
            {
                if (pl.IsOwner)
                {
                    if (_cc != null)
                    {
                        _cc.Cancel();
                    }

                    _cc = new CancellationTokenSource();
                    Run(_cc.Token).Forget();
                }
            }
        }

        public async UniTaskVoid Run(CancellationToken tok)
        {
            go.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(ShowTime), cancellationToken: tok);
            go.SetActive(false);
            _cc = null;
        }

        private bool TryGetPlayer(Collider2D collider2D, out PlayerControls retu)
        {
            if (collider2D.TryGetComponent<PlayerControls>(out var pl))
            {
                retu = pl;
                return true;
            }

            if (collider2D.transform.parent.TryGetComponent<PlayerControls>(out var pl2))
            {
                retu = pl2;
                return true;
            }

            retu = null;
            return false;
        }
    }
}