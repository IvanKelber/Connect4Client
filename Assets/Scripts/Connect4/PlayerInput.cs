using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.TakingTurn) {
            int playerInput = GetPlayerInput();
            if(playerInput > 0) {
                player.PlacePiece(playerInput);
            }
        }
    }

    public int GetPlayerInput() {
        int input = 0;
        for(int i = 1; i <= player.Board.Dimensions.x; i++) {
            if(Input.GetKeyDown(i + "")) {
                input = i;
                break;
            }
        }
        return input;
    }
}
