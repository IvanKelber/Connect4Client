using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitPopup : MonoBehaviour
{

    [SerializeField]
    private Timer timer;

    [SerializeField]
    private TMP_Text waitDescription;

    [SerializeField]
    private Client client;

    private string opponent;

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
        this.opponent = opponent;
    }

    public void Reject() {
        Debug.Log("Request was rejected");
        Hide();
    }

    public void Cancel() {
        Debug.Log("Cancelling Request");
        client.CancelChallengeProposal(opponent);
        Hide();
    }

    private void Hide() {
        poppedUp = false;
        gameObject.SetActive(false);
        timer.Stop();
    }
}
