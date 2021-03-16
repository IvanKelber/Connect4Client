using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChallengeProposalPopup : MonoBehaviour
{
    [SerializeField]
    private Timer timer;

    [SerializeField]
    private TMP_Text challengeDescription;

    private bool poppedUp = false;

    // Update is called once per frame
    void Update()
    {
        if(poppedUp) {
            if(timer.IsDone) {
                // Automatically reject the challenge proposal after the timer is done
                Reject();
            }
        }

    }

    public void Popup(string opponent) {
        UpdateChallengeDescription(opponent);
        timer.StartCountdown();
        poppedUp = true;
        gameObject.SetActive(true);
    }

    public void UpdateChallengeDescription(string opponent) {
        challengeDescription.text = opponent + " has challenged you to a game";
    }

    public void Reject() {
        Debug.Log("Rejecting request");
        Hide();
    }

    public void Accept() {
        Debug.Log("Accepting Request");
        Hide();
    }

    private void Hide() {
        poppedUp = false;
        gameObject.SetActive(false);
        timer.Stop();
    }
}
