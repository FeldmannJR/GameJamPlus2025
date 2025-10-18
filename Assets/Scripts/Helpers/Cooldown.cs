using System;
using UnityEngine;

namespace Scripts.Helpers
{
    /// <summary>
    /// Little tool to control cooldowns in isolation
    /// </summary>
    public class Cooldown
    {
        private float _trigger;

        private float _delay;

        public TimeSpan Delay => TimeSpan.FromSeconds(_delay);

        public Cooldown(TimeSpan delay)
        {
            _delay = (float)delay.TotalSeconds;
        }

        public bool CheckTrigger()
        {
            if (IsCooldown()) return false;
            Trigger();
            return true;
        }

        public void Trigger() => _trigger = Time.time;

        public bool IsCooldown() => Time.time < _trigger + _delay;

        public void Reset() => _trigger = Time.time;
    }
}