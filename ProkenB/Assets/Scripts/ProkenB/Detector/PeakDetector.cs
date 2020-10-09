using System;
using UnityEngine;

namespace ProkenB.Detector
{
    /// <summary>
    /// 振幅の入力に対して、ピークを持続的に検出する。
    /// エンベローパーというか、コンデンサみたいなやつ。
    /// </summary>
    public class PeakDetector
    {
        // しきい値
        public float Threshold = 0.5f; // しきい値
        public float DetectorThreshold = 0.8f; // 検出器のしきい値

        // アタックレート・リリースレート
        public float AttackRate
        {
            get => _attackSec;
            set
            {
                _attackSec = value;
                CalculateCoefficients();
            }
        }
        public float ReleaseRate
        {
            get => _releaseSec;
            set
            {
                _releaseSec = value;
                CalculateCoefficients();
            }
        }

        // アタック・リリースの内部表現
        private float _attackSec = 0.1f;
        private float _releaseSec = 0.8f;

        // アタック・リリースに基づいた計数
        private float _attackCoef;
        private float _releaseCoef;

        // 現在のエンベロープの状態
        private float _currentValue = 0f;

        public PeakDetector()
        {
            // 初期値について係数を計算する。
            CalculateCoefficients();
        }

        void CalculateCoefficients()
        {
            // アタック・リリースの値に基づいて係数を計算する

            _attackCoef = -1.0f / _attackSec;
            _releaseCoef = -1.0f / _releaseSec;
        }

        public void UpdateValue(float value, float deltaTime)
        {
            if (value < Threshold)
            {
                // しきい値以下の場合、値を減算処理
                _currentValue *= Mathf.Exp(_releaseCoef * deltaTime);
            }
            else
            {
                _currentValue = 1.0f - (1.0f - _currentValue) * Mathf.Exp(_attackCoef * deltaTime);
            }
        }

        public bool Current()
        {
            return _currentValue >= DetectorThreshold;
        }
    }
}
