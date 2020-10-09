namespace ProkenB.Detector
{
    using static MicrophoneSoundDetector;

    public abstract class DetectorStrategy
    {
        /// <summary>
        /// FFTの結果から、音の情報を取得する。
        /// </summary>
        /// <param name="fft">FFTの結果</param>
        /// <param name="samplingRate">サンプリングレート</param>
        /// <returns></returns>
        abstract public VoiceInputState Detect(float[] fft, int samplingRate, float deltaTime);
    }
}
