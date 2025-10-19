using System;
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
    }
}