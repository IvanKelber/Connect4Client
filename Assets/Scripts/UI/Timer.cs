using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TMP_Text counter;
    
    [SerializeField]
    private int secondsToWait = 10;

    private float secondsWaited;

    private bool countingDown = false;
    private bool doneCounting = false;

    public bool IsDone {
        get {
            return doneCounting;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(countingDown) {
            counter.text = "" + Mathf.Ceil(secondsToWait - secondsWaited);
            secondsWaited += Time.deltaTime;
            if(secondsWaited >= secondsToWait) {
                countingDown = false;
                doneCounting = true;
            }
        }
    }

    public void StartCountdown() {
        secondsWaited = 0;
        doneCounting = false;
        countingDown = true;
        counter.text = "" + secondsToWait;
    }

    public void Stop() {
        doneCounting = false;
        countingDown = false;
    }
}
