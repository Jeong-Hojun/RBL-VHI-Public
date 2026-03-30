using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Leap;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image scoreBar;
    [SerializeField]
    private Text scoreText;

    private float maxScore = 100;
    private float nowScore;
    private System.Random rand = new System.Random();
    private double[] rp;

    // Start is called before the first frame update
    void Start()
    {
        rp = this.gameObject.GetComponent<s_TCP_thread>().rp;
        nowScore = 0;
        scoreBar.fillAmount = nowScore/maxScore;
        scoreText.text = (nowScore / maxScore * 100).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    nowScore = rand.Next(0, 100);
        //}
        nowScore = PPMCC();
        nowScore = (float)Math.Round(nowScore, 2);
        HandleScore();
    }

    private void HandleScore()
    {
        //scoreBar.fillAmount = Mathf.Lerp(scoreBar.fillAmount, nowScore / maxScore, Time.deltaTime * 7);
        scoreBar.fillAmount = nowScore / maxScore;
        scoreText.text = (nowScore / maxScore * 100).ToString();
    }

    public double[] standardMap = new double[6];
    private double Numerator;
    private double x_CAR_sq_sum;
    private double y_CAR_sq_sum;
    private double Denominator = 0;
    private float PPMCC()
    {
        double[] x = rp;
        double[] y = standardMap;
        int len = x.Length;
        double[] x_CAR = new double[len];
        double[] y_CAR = new double[len];

        double x_bar = x.Average();
        //Debug.Log(x_bar);
        double y_bar = y.Average();

        for (int i=0; i < len; i++) //for 문 돌리면 안될듯 0 나오는거 자체가 일단 문제임.
        {
            x_CAR[i] = x[i] - x_bar;
            y_CAR[i] = y[i] - y_bar;
        }
        Debug.Log(x_CAR[5]);
        //Debug.Log(x_CAR[0] == x_CAR[1]);
        Debug.Log(y_CAR[5]);

        Numerator = x_CAR[0] * y_CAR[0] + x_CAR[1] * y_CAR[01] + x_CAR[2] * y_CAR[2] + x_CAR[3] * y_CAR[3] + x_CAR[4] * y_CAR[4] + x_CAR[5] * y_CAR[5];
        x_CAR_sq_sum = x_CAR[0] * x_CAR[0] + x_CAR[1] * x_CAR[01] + x_CAR[2] * x_CAR[2] + x_CAR[3] * x_CAR[3] + x_CAR[4] * x_CAR[4] + x_CAR[5] * x_CAR[5];
        y_CAR_sq_sum = y_CAR[0] * y_CAR[0] + y_CAR[1] * y_CAR[01] + y_CAR[2] * y_CAR[2] + y_CAR[3] * y_CAR[3] + y_CAR[4] * y_CAR[4] + y_CAR[5] * y_CAR[5];


        Debug.Log(Numerator);

        nowScore = (float)Numerator / (float)(Math.Sqrt(x_CAR_sq_sum) * Math.Sqrt(y_CAR_sq_sum));
        //Debug.Log(nowScore);
        return nowScore;
    }
}
