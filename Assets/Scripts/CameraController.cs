using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    private Vector3 pos;
    public float smoothness;
    public Vector3 offSet;
    public float leftLimit;
    public float rightLimit;
    public bool playerEnteredLevel2 = false;

    void Update()
    {
       pos = Vector3.Lerp(transform.position, target.position + offSet, smoothness);
    }


    private void LateUpdate()
    {
        transform.position = pos;

        if (playerEnteredLevel2)
        {

            transform.position = new Vector3(
           transform.position.x,
           transform.position.y,
           Mathf.Clamp(transform.position.z, leftLimit - 2, rightLimit + 1.5f));
        }
        else
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                Mathf.Clamp(transform.position.z, leftLimit, rightLimit));
        }
    }
}
