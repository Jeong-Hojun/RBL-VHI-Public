using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Reads a biofeedback score from TCPManager every 0.25 seconds
/// and smoothly updates the score bar and text UI elements.
/// Score is expected in the range [0, 1] from the TCP stream.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image scoreBar;
    [SerializeField] private Text scoreText;

    private float maxScore = 1;
    private float nowScore;
    private float _time;

    void Start()
    {
        _time = 0;
        nowScore = 0;
        scoreBar.fillAmount = nowScore / maxScore;
        scoreText.text = (nowScore / maxScore * 100).ToString();
    }

    void Update()
    {
        _time += Time.deltaTime;

        // Poll TCP score at 4 Hz to reduce CPU load
        if (_time >= 0.25f)
        {
            nowScore = (float)Math.Round(this.gameObject.GetComponent<TCPManager>().test, 2);
            _time = 0;
        }

        HandleScore();
    }

    private void HandleScore()
    {
        // Smoothly animate the fill bar toward the target score
        scoreBar.fillAmount = Mathf.Lerp(scoreBar.fillAmount, nowScore / maxScore, Time.deltaTime * 7);
        scoreText.text = (nowScore / maxScore * 100).ToString();
    }
}
