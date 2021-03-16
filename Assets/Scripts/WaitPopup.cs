﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitPopup : MonoBehaviour
{

    [SerializeField]
    private Timer timer;

    [SerializeField]
    private TMP_Text waitDescription;

    private bool poppedUp = false;

    // Update is called once per frame
    void Update()
    {
        if(poppedUp) {
            if(timer.IsDone) {
                // The opponent never responded to the challenge proposal
                Reject();
            }

            if(Input.GetKeyDown(KeyCode.Escape)) {
                Cancel();
            }
        }

    }

    public void Popup(string opponent) {
        UpdateWaitDescription(opponent);
        timer.StartCountdown();
        poppedUp = true;
        gameObject.SetActive(true);
    }

    public void UpdateWaitDescription(string opponent) {
        waitDescription.text = "Waiting for " + opponent + " to respond...";
    }

    public void Reject() {
        Debug.Log("Request was rejected");
        Hide();
    }

    public void Cancel() {
        Debug.Log("Cancelling Request");
        Hide();
    }

    private void Hide() {
        poppedUp = false;
        gameObject.SetActive(false);
        timer.Stop();
    }
}
