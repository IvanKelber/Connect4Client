using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class Tile : MonoBehaviour
{

    [SerializeField]
    private Rectangle outerBorder;
    [SerializeField]
    private Rectangle middleBorder;
    [SerializeField]
    private Rectangle innerBorder;

    private bool isOccupied = false;
    public bool IsOccupied {
        get {
            return isOccupied;
        }
    }
    public Color tileColor;

    private void Start() {
        outerBorder.Color = middleBorder.Color = innerBorder.Color = tileColor;
    }

    public void UpdateSideLength(float sideLength) {
        float currentSideLength = outerBorder.Width;
        float percentIncrease = sideLength/currentSideLength;

        outerBorder.Width = outerBorder.Height = sideLength;
        middleBorder.Width = middleBorder.Height = sideLength;
        innerBorder.Width = innerBorder.Height = sideLength;

        outerBorder.Thickness *= percentIncrease;
        middleBorder.Thickness *= percentIncrease;
        innerBorder.Thickness *= percentIncrease;
    }

    public void SetOccupied() {
        isOccupied = true;
    }
}
