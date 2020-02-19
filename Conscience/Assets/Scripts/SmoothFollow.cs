using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public GameObject targetFollow;
    public float smoothing;

    private void Update()
    {
        Vector3 posLerp = Vector3.Lerp(transform.position, targetFollow.transform.position, smoothing);
        transform.position = new Vector3(posLerp.x, posLerp.y, -10);
    }
}
