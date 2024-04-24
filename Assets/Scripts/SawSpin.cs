using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawSpin : MonoBehaviour
{
    private float sawSpeed = 2f;

    //rotate the saw image. Substitute for animating the saw.
    void Update()
    {
        transform.Rotate(0, 0, 360 * sawSpeed * Time.deltaTime);
    }
}
