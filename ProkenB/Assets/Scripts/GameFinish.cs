using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ProkenB.Model;
using ProkenB.Game;
using UniRx;

public class GameFinish : MonoBehaviour
{
    public Text finishTime;
    public GameObject finishUI;
    public Text rankText;

    [SerializeField] private Button retryButton = null;

    private bool m_gameFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        finishTime.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        var model = GameManager.Instance?.Model;
        var localPlayer = model.LocalPlayer;

        if (model == null || localPlayer == null)
        {
            return;
        }

        if (!m_gameFinished && (model.LocalPlayer.HasReachedGoal || model.Lifecycle == GameModel.GameLifecycle.Finish))
        {
            ShowMessage();
        }
    }

    void ShowMessage()
    {
        if (!finishTime || !rankText || !retryButton)
        {
            return;
        }

        var model = GameManager.Instance.Model;

        if (model.Lifecycle == GameModel.GameLifecycle.Finish)
        {
            retryButton.enabled = true;
        }
        else
        {
            // ゲームが完全に終わってから抜けられるようにする
            retryButton.enabled = false;

            model.LifecycleAsObservable
                .Where(l => l == GameModel.GameLifecycle.Finish)
                .First()
                .Subscribe(_ => retryButton.enabled = true);
        }

        finishUI.SetActive(true);
        finishTime.text = $"Finish Time = {GameManager.Instance.Timer}";

        var rankview = FindObjectOfType<RankView>();
        rankText.text = $"Rnking = {rankview.rank}";

        m_gameFinished = true;
    }

    public void OnRetry()
    {
        SceneManager.LoadScene("Title");
    }
}
