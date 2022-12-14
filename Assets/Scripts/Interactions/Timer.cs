using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    public float currentTime;
    public bool countDown;

    [Header("Limit Settings")]
    public bool hasLimit;
    public float timerLimit;

    public bool startCountDown;
    public bool startTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startCountDown)
        {
            timerText.rectTransform.anchoredPosition = Vector3.zero;
            timerText.text = currentTime.ToString("0");
            currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;

            if (hasLimit && ((countDown && currentTime <= timerLimit) || (!countDown && currentTime >= timerLimit)))
            {
                currentTime = timerLimit;

                hasLimit = false;
                countDown = false;
                startCountDown = false;
                startTimer = true;
            }
        }

        if (startTimer)
        {
            timerText.rectTransform.anchoredPosition = new Vector3(0, 175, 0);
            timerText.text = currentTime.ToString("0.00");
            currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;
        }
    }

    public void ResetTimer()
    {
        currentTime = 3;
        startTimer = false;
        hasLimit = true;
        countDown = true;
    }
}
