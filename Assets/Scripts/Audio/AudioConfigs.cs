using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public enum AudioEnum
    {
        None = 0,
        Telephone_Receiving = 1,
        Telephone_Calling = 2,
        Telephone_Hangup = 3,
        Telephone_Connected = 4,
        Telephone_Static = 5,
        PP_On = 6,
        PP_Off = 7,
        Button_On = 8,
        Button_Off = 9,
        Telephone_HangupOther = 10,
        Door_Unlock = 11,

        GameTheme = 12,
        MainMenuTHeme = 13,
        Success = 14,
        Fail = 15
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