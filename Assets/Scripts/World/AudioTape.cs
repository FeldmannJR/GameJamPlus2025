using System.Collections.Generic;
using Adrenak.UniVoice;
using Adrenak.UniVoice.Outputs;
using Cysharp.Threading.Tasks;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Scripts
{
    public class AudioTape : InteractableObject
    {
        private readonly SyncList<AudioFrame> AudioFrames = new SyncList<AudioFrame>();
        private bool _playing = false;

        public override void OnStartClient()
        {
            OnLocalInteracted += OnTapeInteracted;
        }


        public void SetAudioFrames(List<AudioFrame> audioFrames)
        {
            AudioFrames.Clear();
            AudioFrames.AddRange(audioFrames);
        }

        public async UniTask Play()
        {
            _playing = true;
            var s = StreamedAudioSourceOutput.New();
            foreach (var audioFrame in AudioFrames)
            {
                s.Feed(audioFrame);
                await UniTask.Delay(100);
            }

            _playing = false;
        }

        private void OnTapeInteracted()
        {
            if (_playing) return;
            Play().Forget();
        }
    }
}