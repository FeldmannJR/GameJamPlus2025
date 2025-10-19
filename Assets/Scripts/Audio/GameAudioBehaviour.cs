using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class GameAudioBehaviour : MonoBehaviour
    {
        public AudioEnum Value;
        private AudioSource _audioSource;


        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayOneShot()
        {
            var clip = AudioConfigs.GetClipFor(Value);
            _audioSource.loop = false;
            _audioSource.PlayOneShot(clip);
        }

        public void PlayRepeating()
        {
            var clip = AudioConfigs.GetClipFor(Value);
            _audioSource.loop = true;
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }

        private CancellationTokenSource _cc;

        public void Pause()
        {
            if (_cc != null)
            {
                _cc.Cancel();
                _cc = null;
            }

            _audioSource.Pause();
        }

        public void Unpause()
        {
            _cc = new CancellationTokenSource();
            UniTask.Delay(4000, cancellationToken: _cc.Token).ContinueWith(() =>
            {
                _audioSource.UnPause();
                _cc = null;
            });
        }
    }
}