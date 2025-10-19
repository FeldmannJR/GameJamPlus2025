using System;
using Adrenak.UniVoice.Samples;
using Audio;
using DefaultNamespace;
using FishNet.Object;
using Scripts.Helpers;
using UnityEngine;

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

        private PlayerControls _playerInteracted;

        public GameAudioBehaviour[] Audios;

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

            _state = States.ReceivingCall;
            Play(AudioEnum.Telephone_Receiving, true);
        }


        private void Update()
        {
            if (_state == States.MakingCall || _state == States.ReceivingCall)
            {
                if (Time.frameCount % 30 == 0)
                {
                    if (Vector2.Distance(this.transform.position, _playerInteracted.transform.position) > 8f)
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
            Play(AudioEnum.Telephone_Calling, true);
            Other.RpcCall(true);
        }

        public void Disconnect()
        {
            Debug.Log("CELULAR: Disconnect");
            _uniVoice.StopRecording();
            _state = States.Idle;
            Play(AudioEnum.Telephone_Hangup);
            Other.RpcCall(false);
        }

        private void OnOtherDisconnected()
        {
            Debug.Log("CELULAR: OnOtherDisconnected");
            _uniVoice.StopRecording();
            _state = States.Idle;
            Play(AudioEnum.Telephone_Hangup);
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
            _state = States.OnCall;
            _uniVoice.StartRecording();
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