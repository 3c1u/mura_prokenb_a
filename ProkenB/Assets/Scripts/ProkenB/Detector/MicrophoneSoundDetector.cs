using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProkenB.Detector
{
    public class MicrophoneSoundDetector : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource = null;
        [SerializeField] private float gain = 1.0f;
        private int _samplingRate = 44100;

        private const int FFT_SIZE = 1024;
        private DetectorStrategy _strategy = CreateDetectionStrategy();

        private VoiceInputState _currentState = VoiceInputState.Off;
        private float[] _fftResultBuffer = new float[FFT_SIZE];

        public bool Voice => _currentState == VoiceInputState.Tonal;

        public bool Breath => _currentState == VoiceInputState.Blow;

        private float _volumeRate = 0f;

        public enum VoiceInputState
        {
            /// <summary>
            /// 音程、風のいずれも検知できない状態。
            /// </summary>
            Off = 0,
            /// <summary>
            /// 音程。
            /// </summary>
            Tonal,
            /// <summary>
            /// 風。
            /// </summary>
            Blow,
        }

        IEnumerator Start()
        {
            // マイクの取得
            yield return ObtainMicrophone();
        }

        static DetectorStrategy CreateDetectionStrategy()
        {
            // ディテクターストラテジーの初期化が必要なら、ここでやる。　

            return new SimpleDetectionStrategy();
        }

        IEnumerator ObtainMicrophone()
        {
            int samplingRate = 44100;

            // マイクの権限取得をする。
            yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);

            if (Application.HasUserAuthorization(UserAuthorization.Microphone)
                && Microphone.devices.Length > 0)
            {
                // マイクの権限を持っているなら、マイクを取得する

                // マイクの能力を取得
                int minSamplingFreq, maxSamplingFreq;
                Microphone.GetDeviceCaps("", out minSamplingFreq, out maxSamplingFreq);

                // 特殊なサンプリングレートを要求されたときはとりあえず最低4000Hzはあるように保証したい（声検出のため）
                if (maxSamplingFreq != 0
                    && (samplingRate < minSamplingFreq || maxSamplingFreq < samplingRate))
                {
                    Debug.LogWarning("特殊なサンプリングレートの可能性があります： (Minimum: "
                                        + minSamplingFreq + " Hz, Maximum: " + maxSamplingFreq + " Hz)");
                    samplingRate = Math.Max(Math.Min(maxSamplingFreq, 4000), minSamplingFreq);
                }

                if (samplingRate < 4000)
                    Debug.LogWarning("サンプリングレートが4000Hz以下です。音声の検出に失敗するかもしれません。");

                audioSource.clip = Microphone.Start("", true, 1, samplingRate);
                audioSource.loop = true;

                // 遅延を0にする
                while (!(Microphone.GetPosition("") > 0))
                    ;

                audioSource.Play();
            }
            else
            {
                Debug.LogError("マイクの取得に失敗しました。");
            }

            _samplingRate = samplingRate;
        }

        void Update()
        {
            var state = Detect(audioSource);

            if (_currentState != state)
            {
                _currentState = state;
                // Debug.Log(state);
            }

            // TODO: ゲームコントローラと統合
            // GameController.Instance.Player.Breath.isActive = state == VoiceInputState.Blow;
            // GameController.Instance.Player.Voice.isActive = state == VoiceInputState.Tonal;

            // GameController.Instance.Player.Breath.Power = _volumeRate / 100.0f;
            // GameController.Instance.Player.Voice.Power = _volumeRate / 100.0f;
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            // TODO: 高速なFFTをするならここでNative？

            // ゲインの検出（RMS）
            float sum = 0;

            for (int i = 0; i < data.Length; ++i)
            {
                sum += Mathf.Pow(data[i], 2);
            }

            // データ数で割ったものに倍率をかけて音量とする
            _volumeRate = Mathf.Clamp01(Mathf.Sqrt(sum) * gain / (float)data.Length) * 25000;
        }

        /// <summary>
        /// オーディオソースの入力に対し、音声を判別して更新されたステートを返す。
        /// </summary>
        /// <param name="source">オーディオソース</param>
        /// <param name="frequency">周波数の出力（検出された場合）</param>
        /// <returns></returns>
        VoiceInputState Detect(AudioSource source)
        {
            // FFTの結果を取り出す。（ローレイテンシーで動かすにはNativeの力が必要...?）
            source.GetSpectrumData(_fftResultBuffer, 0, FFTWindow.Blackman);
            return _strategy.Detect(_fftResultBuffer, 44100, Time.deltaTime);
        }
    }
}
