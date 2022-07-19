using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Transform bulletTransform;
    public float health = 100f;
    public Image healthBar;
    public static EnemyController instance;

    private void Awake()
    {
        instance = this;
    }

    public void InstantiateAttack()
    {
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("EnemyBullet");
        bullet.transform.position = bulletTransform.position;
        bullet.transform.rotation = bulletTransform.rotation;
        bullet.SetActive(true);
        StartCoroutine(DeactivateBullets(bullet));
    }

    IEnumerator DeactivateBullets(GameObject go)
    {
        yield return new WaitForSeconds(5f);
        go.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.SetActive(false);
            GameObject playerHitVFX = ObjectPooler.SharedInstance.GetPooledObject("PlayerHitEffect");
            playerHitVFX.transform.position = collision.gameObject.transform.position;
            playerHitVFX.SetActive(true);
            StartCoroutine(deactivateHitEffect(playerHitVFX));
            if (healthBar.fillAmount <= 0 && health <= 0)
            {
                return;
            }
            else
            {
                //Debug.Log("Bullet " + collision.gameObject.name);
                health -= 20f;
                healthBar.fillAmount -= 0.2f;
            }
        }
    }

    public IEnumerator DeactivateEnemy()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        this.GetComponent<Animator>().enabled = false;

        yield return new WaitForSeconds(0.5f);
        //this.gameObject.SetActive(false);
        healthBar.transform.parent.parent.parent.gameObject.SetActive(false);
    }

    IEnumerator deactivateHitEffect(GameObject go)
    {
        yield return new WaitForSeconds(0.4f);
        go.SetActive(false);
    }
}
