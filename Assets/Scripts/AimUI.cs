using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimUI : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update(){
        Cursor.lockState = CursorLockMode.Locked;
    }
}
