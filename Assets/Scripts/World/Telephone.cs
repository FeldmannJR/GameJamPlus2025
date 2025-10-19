using System;
using Adrenak.UniVoice.Samples;
using Audio;
using DefaultNamespace;
using FishNet.Object;
using Scripts.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class Telephone : InteractableObject
    {
        public enum States
        {
            Idle,
            MakingCall,
            ReceivingCall,
            OnCall
        }

        public UniVoice _uniVoice;
        public Telephone Other;
        private States _state = States.Idle;


        private int _callsAmount = 0;
        public UnityEvent OnTakeFirstCall;
        public UnityEvent OnLocalInteractedUnityEvent;
        public UnityEvent OnReceivedCallUnityEvent;

        private PlayerControls _playerInteracted;
        public BoxCollider2D _turnOffCollider;
        public GameAudioBehaviour[] Audios;
        public GameAudioBehaviour StaticAudio;
        private Cooldown _interactionCooldown = new Cooldown(TimeSpan.FromSeconds(0.5f));

        public void Play(AudioEnum audio, bool repeating = false)
        {
            foreach (var audioBehaviour in Audios)
            {
                if (audioBehaviour.Value == audio)
                {
                    if (repeating)
                    {
                        audioBehaviour.PlayRepeating();
                    }
                    else
                    {
                        audioBehaviour.PlayOneShot();
                    }
                }
                else
                {
                    audioBehaviour.Stop();
                }
            }
        }

        public bool IsMyPhone()
        {
            return PositionConverterSystem.Instance.ArePositionsOnSameHouse(this.transform.position,
                PlayerControls.LocalPlayer.transform.position);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            this.OnLocalInteracted += OnOnLocalInteracted;
        }

        void OnOnLocalInteracted()
        {
            OnLocalInteractedUnityEvent?.Invoke();
            _playerInteracted = PlayerControls.LocalPlayer;
            Debug.Log("CELULAR: OnOnLocalInteracted");

            if (!_interactionCooldown.CheckTrigger()) return;
            if (_state == States.Idle)
            {
                MakeCall();
            }

            else if (_state == States.MakingCall || _state == States.OnCall)
            {
                Disconnect();
            }

            else if (_state == States.ReceivingCall)
            {
                AcceptCall();
            }
        }

        public void OnReceivedCall()
        {
            Debug.Log("CELULAR: OnReceivedCall");

            if (_state == States.MakingCall)
            {
                ConnectCall();
                return;
            }

            OnReceivedCallUnityEvent?.Invoke();
            _state = States.ReceivingCall;
            MusicController.Instance.PauseMusic();
            Play(AudioEnum.Telephone_Receiving, true);
        }


        private void Update()
        {
            if (_state == States.MakingCall || _state == States.OnCall)
            {
                if (_playerInteracted == null) return;

                {
                    if (!_turnOffCollider.bounds.Contains(_playerInteracted.FootTransform.position))
                    {
                        Disconnect();
                    }
                }
            }
        }

        public void MakeCall()
        {
            Debug.Log("CELULAR: MakeCall");
            _state = States.MakingCall;
            MusicController.Instance.PauseMusic();
            Play(AudioEnum.Telephone_Calling, true);
            Other.RpcCall(true);
        }

        public void Disconnect()
        {
            Debug.Log("CELULAR: Disconnect");
            _uniVoice.StopRecording();
            _state = States.Idle;
            StaticAudio.Stop();
            Play(AudioEnum.Telephone_Hangup);
            Other.RpcCall(false);
            MusicController.Instance.ContinueMusic();
        }

        private void OnOtherDisconnected()
        {
            if (_state == States.ReceivingCall)
            {
                _state = States.Idle;
                Play(AudioEnum.None);
                MusicController.Instance.ContinueMusic();
                return;
            }

            if (_state != States.OnCall) return;
            Debug.Log("CELULAR: OnOtherDisconnected");
            _uniVoice.StopRecording();
            StaticAudio.Stop();
            _state = States.Idle;
            Play(AudioEnum.Telephone_HangupOther);
            MusicController.Instance.ContinueMusic();
        }


        public void AcceptCall()
        {
            Debug.Log("CELULAR: AcceptCall");

            Other.RpcCall(true);
            ConnectCall();
        }

        public void ConnectCall()
        {
            Debug.Log("CELULAR: ConnectCall");

            Play(AudioEnum.Telephone_Connected);
            StaticAudio.PlayRepeating();
            _state = States.OnCall;
            _uniVoice.StartRecording();
            if (_callsAmount == 0)
                OnTakeFirstCall?.Invoke();
            _callsAmount++;
        }


        [ServerRpc(RequireOwnership = false)]
        public void RpcCall(bool connect)
        {
            Debug.Log("CELULAR RpcCall: " + connect);
            RpcOnCalled(connect);
        }


        [ObserversRpc]
        public void RpcOnCalled(bool connect)
        {
            if (!IsMyPhone()) return;
            ;
            Debug.Log("CELULAR RpcOnCalled " + connect);
            if (connect)
            {
                OnReceivedCall();
            }
            else
            {
                OnOtherDisconnected();
            }
        }
    }
}