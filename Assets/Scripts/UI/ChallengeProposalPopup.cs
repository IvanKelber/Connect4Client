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

    [SerializeField]
    Client client;

    private string opponent;
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
        this.opponent = opponent;
    }

    public void Reject() {
        Debug.Log("Rejecting request");
        client.RejectChallengeProposal(opponent);
        Hide();
    }

    public void Accept() {
        Debug.Log("Accepting Request");
        client.AcceptChallengeProposal(opponent);
        Hide();
    }

    public void Cancel() {
        Debug.Log("Opponent canceled the request");
        Hide();
    }

    private void Hide() {
        poppedUp = false;
        gameObject.SetActive(false);
        timer.Stop();
    }
}
