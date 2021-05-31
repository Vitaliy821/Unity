using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsController : MonoBehaviour
{
    [SerializeField] private Button[] buttons;

    public MovementDirection Control { get; private set; }

    public void OnPointerUp()
    {
        Control = MovementDirection.Idle;
    }
    public void OnPointerDown(int direction)
    {
        Control = (MovementDirection)direction;
    }
}
