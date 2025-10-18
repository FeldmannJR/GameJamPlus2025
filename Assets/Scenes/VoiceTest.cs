using System;
using System.Collections.Generic;
using Adrenak.UniMic;
using Adrenak.UniVoice.Networks;
using Adrenak.UniVoice.Outputs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Adrenak.UniVoice.Samples
{
    public class VoiceTest : MonoBehaviour
    {
        public List<AudioFrame> _frames = new();
        private FishNetClient _client;


        private void Start()
        {
            TestRecord().Forget();
        }

        public async UniTaskVoid TestRecord()
        {
            Mic.Init();
            _frames = new();
            var mic = Mic.AvailableDevices[0];
            mic.OnFrameCollected += OnFrameCollected;
            mic.StartRecording(150);
            await UniTask.Delay(8000);
            mic.StopRecording();

            await UniTask.Delay(5000);
            PlayBack();
        }


        private void OnFrameCollected(int frequency, int channels, float[] samples)
        {
            var frame = new AudioFrame
            {
                timestamp = 0,
                frequency = frequency,
                channelCount = channels,
                samples = Utils.Bytes.FloatsToBytes(samples)
            };
            _frames.Add(frame);
        }

        public async UniTask PlayBack()
        {
            var s = StreamedAudioSourceOutput.New();
            foreach (var audioFrame in _frames)
            {
                s.Feed(audioFrame);
                await UniTask.Delay(100);
            }
        }
    }
}