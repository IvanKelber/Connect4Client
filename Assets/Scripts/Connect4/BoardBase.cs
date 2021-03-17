using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class BoardBase : MonoBehaviour
{
    private BoxCollider2D collider;

    private Rectangle body;

    void Awake() {
        body = GetComponent<Rectangle>();
        collider = GetComponent<BoxCollider2D>();
    }

    public void UpdateSize(float width, float height) {
        body.Width = width;
        body.Height = height;
        collider.size = new Vector2(width,height);
    }

    public void UpdatePosition(Vector3 position) {
        transform.position = position;
    }

}
