using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitPopup : MonoBehaviour
{

    [SerializeField]
    private TMP_Text counter;
    
    [SerializeField]
    private int secondsToWait;

    [SerializeField]
    private TMP_Text waitDescription;

    private float secondsWaited;

    private bool poppedUp = false;

    // Update is called once per frame
    void Update()
    {
        if(poppedUp) {
            counter.text = "" + Mathf.Ceil(secondsToWait - secondsWaited);
            secondsWaited += Time.deltaTime;
            if(secondsWaited >= secondsToWait) {
                // The opponent never responded to the challenge proposal
                Reject();
            }

            if(Input.GetKeyDown(KeyCode.Escape)) {
                Cancel();
            }
        }

    }

    public void Popup(string opponent) {
        counter.text = "" + secondsToWait;
        UpdateWaitDescription(opponent);

        secondsWaited = 0;
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
    }
}
