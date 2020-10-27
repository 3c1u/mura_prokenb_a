using System;
using System.Linq;
using ProkenB.Game;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private Image image = null;
    private Texture2D m_targetTexture = null;

    void Awake()
    {
        // 描画先のテクスチャを取得
        m_targetTexture = image.sprite.texture;
    }

    void Start()
    {
        
    }

    void Update()
    {
        // キャンバスサイズの取得
        var width = m_targetTexture.width;
        var height = m_targetTexture.height;
         
        var fft = GameManager.Instance.Detector.FftResultBuffer;
        
        // テクスチャを空にする
        var pixels = new Color[width * height];

        for (var x = 0; x < width; x++)
        {
            var nyquistFreq = 22050.0f;
            var freq = Mathf.Pow(2, (x / (float) width) * Mathf.Log(nyquistFreq, 2));
            var fftNormalized = fft[(int) (freq * fft.Length / nyquistFreq)];
            var fftDecibel = 120.0f + 10.0f * Mathf.Log(fftNormalized, 2);
            var threshold = fftDecibel / 120.0f * height;
            
            for (var y = 0; y < height; y++)
            {
                if (y < threshold)
                {
                    pixels[y * width + x] = Color.green;;
                }
                else
                {
                    pixels[y * width + x] = Color.black;
                }
            }
        }
        
        m_targetTexture.SetPixels(pixels);
        m_targetTexture.Apply();
    }
}
