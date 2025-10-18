using System;
using System.Collections.Generic;
using System.Threading;
using Adrenak.UniMic;
using Adrenak.UniVoice;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Object;
using Scripts;
using UnityEngine;

namespace Scripts
{
    public class TapeSystem : NetworkBehaviour
    {
        public GameObject TapePrefab;

        public int RemainingTapes = 1;
        public float TapeDurationInSeconds = 5;

        public void Start()
        {
            Mic.Init();
        }

        public async UniTask<List<AudioFrame>> RecordFrames(CancellationToken ct)
        {
            var frames = new List<AudioFrame>();
            var dev = Mic.AvailableDevices[0];

            dev.OnFrameCollected += (int frequency, int channels, float[] samples) =>
            {
                var frame = new AudioFrame
                {
                    timestamp = 0,
                    frequency = frequency,
                    channelCount = channels,
                    samples = Utils.Bytes.FloatsToBytes(samples)
                };
                frames.Add(frame);
            };
            dev.StartRecording(100);
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(TapeDurationInSeconds), cancellationToken: ct);
            }
            catch (OperationCanceledException ex)
            {
                // let user cancel before
            }

            dev.StopRecording();
            return frames;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnTape(Vector3 position, List<AudioFrame> frames)
        {
            var instance = Instantiate(TapePrefab);
            instance.transform.position = position;
            var tape = instance.GetComponent<AudioTape>();
            tape.SetAudioFrames(frames);
            InstanceFinder.ServerManager.Spawn(instance);
        }
    }
}