using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform Player;

    //Have the camera constantly follow the player by getting the players coordinates every frame.
    private void Update()
    {
        transform.position = new Vector3(Player.position.x, Player.position.y, transform.position.z);
    }
}
