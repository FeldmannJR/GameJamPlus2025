using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Scripts;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace UI
{
    public class GameplayUI : GameUI
    {
        [SerializeField] private TapeSystem _tapeSystem;


        [Q] private Button _recordButton;
        [Q] private Label _tapesLeft;
        [Q] private Label _recordingLabel;

        private CancellationTokenSource _recordCancelationToken;
        private bool _recording = false;

        private float _startedRecording;

        public override void OnOpen(VisualElement root)
        {
            _recordButton.clicked += OnRecordClick;
            _recordingLabel.style.display = DisplayStyle.None;
            _tapesLeft.text = "Tapes Left: " + _tapeSystem.RemainingTapes;
        }

        private void OnRecordClick()
        {
            if (_tapeSystem.RemainingTapes <= 0) return;
            if (_recording)
            {
                _recordCancelationToken.Cancel();
                return;
            }

            Record().Forget();
        }

        private void Update()
        {
            if (_recording)
            {
                var finishesAt = _startedRecording + _tapeSystem.TapeDurationInSeconds;
                var ts = TimeSpan.FromSeconds(finishesAt - Time.time);
                var seconds = ts.Seconds >= 10 ? ts.Seconds.ToString() : "0" + ts.Seconds;
                _recordingLabel.text = "00:" + seconds;
            }
        }

        private async UniTask Record()
        {
            if (_recording) return;
            _recordingLabel.style.display = DisplayStyle.Flex;
            _startedRecording = Time.time;
            _recordCancelationToken = new CancellationTokenSource();
            _recording = true;
            _recordButton.text = "STOP";
            var frames = await _tapeSystem.RecordFrames(_recordCancelationToken.Token);
            PositionConverterSystem.Instance.TryConvert(PlayerControls.LocalPlayer.transform.position,
                out var otherSidePos);
            _tapeSystem.SpawnTape(otherSidePos, frames);
            _tapeSystem.RemainingTapes--;

            _recordingLabel.style.display = DisplayStyle.None;
            _recording = false;
            _tapesLeft.text = "Tapes Left: " + _tapeSystem.RemainingTapes;
            if (_tapeSystem.RemainingTapes > 0)
            {
                _recordButton.text = "RECORD";
                return;
            }

            _recordButton.text = "NO TAPE!";
        }
    }
}