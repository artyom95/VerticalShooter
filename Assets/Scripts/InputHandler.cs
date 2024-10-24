using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public event Action<Vector2> Move;
    private bool _shouldListenInputButton;

    public void Initialize()
    {
        _shouldListenInputButton = true;
    }

    private void FixedUpdate()
    {
        if (!_shouldListenInputButton)
        {
            return;
        }
        ShouldMovePlayer();
        
    }
    
    private void ShouldMovePlayer()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Move?.Invoke(Vector2.up);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Move?.Invoke(Vector2.down);
        }

        if (Input.GetKey(KeyCode.A))
        {
            Move?.Invoke(Vector2.left);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Move?.Invoke(Vector2.right);
        }
    }
}