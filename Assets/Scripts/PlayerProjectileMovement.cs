using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileMovement : MonoBehaviour
{
	public float speed;

	private void OnEnable()
	{
		transform.SetParent(null);
	}

	IEnumerator DestroyBullet()
	{
		yield return new WaitForSeconds(5f);
		this.gameObject.SetActive(false);
	}

	void Update()
	{
		transform.position += (transform.forward) * (speed * Time.deltaTime);
	}

	private void OnCollisionEnter(Collision co)
	{
		if (co.gameObject.CompareTag("Player") || (co.gameObject.CompareTag("EnemyBullet")) || (co.gameObject.CompareTag("EnemyHitEffect") || (co.gameObject.CompareTag("PlayerHitEffect"))))
		{
			return;
		}
		else
		{
			this.gameObject.SetActive(false);
		}
	}
}
