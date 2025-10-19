using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public enum AudioEnum
    {
        None,
        Telephone_Receiving,
        Telephone_Calling,
        Telephone_Hangup,
        Telephone_Connected,
        
    }

    [Serializable]
    public class AudioConfig
    {
        public AudioClip[] Clips;
    }

    [CreateAssetMenu(fileName = "AudioConfigs", menuName = "Configs/AudioConfigs")]
    public class AudioConfigs : ScriptableObject
    {
        public SerializedDictionary<AudioEnum, AudioConfig> Configs = new();


        public static AudioClip GetClipFor(AudioEnum source)
        {
            var configs = Resources.Load<AudioConfigs>("AudioConfigs");
            if (configs.Configs.TryGetValue(source, out var clip))
            {
                var index = UnityEngine.Random.Range(0, clip.Clips.Length);
                return clip.Clips[index];
            }

            throw new Exception("Audio not configured for " + source);
        }
    }
}