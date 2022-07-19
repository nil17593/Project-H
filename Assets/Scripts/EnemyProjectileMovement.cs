using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyProjectileMovement : MonoBehaviour
{
    public float speed;

    private void OnEnable()
    {
        transform.SetParent(null);
    }

    void Update()
    {
        transform.position += (transform.forward) * (speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision co)
    {
        if (co.gameObject.CompareTag("Bullet") || (co.gameObject.CompareTag("EnemyBullet")) || (co.gameObject.CompareTag("EnemyHitEffect") || (co.gameObject.CompareTag("PlayerHitEffect"))))
        {
            return;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
