using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    public Transform target;
    private Vector3 pos;

    public Vector3 offSet;

    void Update()
    {
        pos = target.position + offSet;
        transform.position = pos;
    }
}
