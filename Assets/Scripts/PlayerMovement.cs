using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    #region UI
    public Joystick joystick;
    #endregion

    #region Character
    private Rigidbody rb;
    private Animator animator;
    public float rotationSpeed;
    public float speed;
    #endregion

    #region targets
    private GameObject target;
    public GameObject[] enemies;
    public float gap;
    #endregion

    #region Booleons
    private bool run = false;
    public bool isLevel1Cleared;
    public bool isLevel2Cleared;

    public bool levelTwo = false;
    public bool levelThree = false;
    #endregion

    public float health = 100f;
    public Image healthBar;

    public Transform bulletTransform;

    public static PlayerMovement Instance;

    public List<GameObject> levelOneEnemies = new List<GameObject>();
    public List<GameObject> levelTwoEnemies = new List<GameObject>();
    public List<GameObject> levelThreeEnemies = new List<GameObject>();

    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;
    public GameObject wall4;
    void Start()
    {
        Instance = this;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (levelOneEnemies.Count <= 0)
        {
            isLevel1Cleared = true;
            levelTwo = true;
            //wall1.SetActive(false);
            wall1.transform.DOLocalMoveY(-4f, 1.5f);
        }
        if (levelTwoEnemies.Count <= 0)
        {
            //levelTwo = false;
            levelThree = true;
            wall3.transform.DOLocalMoveY(-4, 1.5f);
        }

        if ((joystick.Horizontal != 0) || (joystick.Vertical != 0))
        {
            animator.Play("Blend Tree");
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rb.velocity), rotationSpeed * Time.deltaTime);

            if (!run)
            {
                run = true;
                StopCoroutine(StopRun());
                StartCoroutine(StartRun());
            }
        }

        else
        {
            if (run)
            {
                run = false;
                rb.velocity = new Vector3(0, 0, 0);
                StopCoroutine(StartRun());
                StartCoroutine(StopRun());
            }
        }



        
            for (int i = 0; i < levelOneEnemies.Count; i++)
            {
                //Debug.Log("Level1");
                if (levelOneEnemies[i] != null)
                {
                    if (Vector3.Distance(transform.position, levelOneEnemies[i].gameObject.transform.position) <= gap)
                    {
                        target = levelOneEnemies[i];

                        target.gameObject.transform.LookAt(transform.position);
                        target.gameObject.GetComponent<Animator>().Play("attack");

                        if (!run)
                        {
                            transform.LookAt(target.gameObject.transform.position);
                            animator.Play("Attack");
                        }
                        if (target.gameObject.GetComponent<EnemyController>().health <= 0)
                        {
                            //levelOneEnemies[i].gameObject.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
                            //levelOneEnemies[i].gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                            StartCoroutine(target.gameObject.GetComponent<EnemyController>().DeactivateEnemy());
                            levelOneEnemies.RemoveAt(i);
                        }
                    }
                    else
                    {
                        levelOneEnemies[i].gameObject.GetComponent<Animator>().Play("idle");
                    }
                }
            }
        //}

        if (!levelTwo) { return; }
        else
        {
            for (int i = 0; i < levelTwoEnemies.Count; i++)
            {
                //Debug.Log("Level2");
                if (levelTwoEnemies[i] != null)
                {
                    if (Vector3.Distance(transform.position, levelTwoEnemies[i].gameObject.transform.position) <= gap)
                    {
                        target = levelTwoEnemies[i];

                        target.gameObject.transform.LookAt(transform.position);
                        target.gameObject.GetComponent<Animator>().Play("attack");

                        if (!run)
                        {
                            transform.LookAt(target.gameObject.transform.position);
                            animator.Play("Attack");
                        }
                        if (target.gameObject.GetComponent<EnemyController>().health <= 0)
                        {
                            StartCoroutine(target.gameObject.GetComponent<EnemyController>().DeactivateEnemy());
                            levelTwoEnemies.RemoveAt(i);
                        }
                    }
                    else
                    {
                        levelTwoEnemies[i].gameObject.GetComponent<Animator>().Play("idle");
                    }
                }
            }
        }

        if (!levelThree) { return; }
        else
        {
            for (int i = 0; i < levelThreeEnemies.Count; i++)
            {
                //Debug.Log("Level3");
                if (levelThreeEnemies[i] != null)
                {
                    if (Vector3.Distance(transform.position, levelThreeEnemies[i].gameObject.transform.position) <= gap)
                    {
                        target = levelThreeEnemies[i];

                        target.gameObject.transform.LookAt(transform.position);
                        target.gameObject.GetComponent<Animator>().Play("attack");

                        if (!run)
                        {
                            transform.LookAt(target.gameObject.transform.position);
                            animator.Play("Attack");
                        }
                        if (target.gameObject.GetComponent<EnemyController>().health <= 0)
                        {
                            StartCoroutine(target.gameObject.GetComponent<EnemyController>().DeactivateEnemy());
                            levelThreeEnemies.RemoveAt(i);
                        }
                    }
                    else
                    {
                        levelThreeEnemies[i].gameObject.GetComponent<Animator>().Play("idle");                       
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            collision.gameObject.SetActive(false);
            GameObject enemyHitVFX = ObjectPooler.SharedInstance.GetPooledObject("EnemyHitEffect");
            enemyHitVFX.transform.position = collision.gameObject.transform.position;
            enemyHitVFX.SetActive(true);
            StartCoroutine(DeactivateHitEffect(enemyHitVFX));
            if (healthBar.fillAmount <= 0 && health <= 0)
            {
                return;
            }
            else
            {
                health -= 10f;
                healthBar.fillAmount -= 0.1f;
            }
          
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall2Trigger"))
        {
            wall2.transform.DOLocalMoveY(-4f, 1.5f);
            Camera.main.GetComponent<CameraController>().playerEnteredLevel2 = true;
        }
        if (other.gameObject.CompareTag("Wall3Trigger"))
        {
            wall4.transform.DOLocalMoveY(-4, 1.5f);
            Camera.main.GetComponent<CameraController>().playerEnteredLevel2 = false;
        }
    }

    IEnumerator DeactivateHitEffect(GameObject go)
    {
        yield return new WaitForSeconds(0.1f);
        go.SetActive(false);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(joystick.Vertical * speed, 0, -joystick.Horizontal * speed);
    }

    public void InstantiateAttack()
    {
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Bullet");
        bullet.transform.position = bulletTransform.position;
        bullet.transform.rotation = bulletTransform.rotation;
        bullet.SetActive(true);
        //StartCoroutine(DeactivateBullet(bullet));
    }

    IEnumerator DeactivateBullet(GameObject go)
    {
        yield return new WaitForSeconds(5f);
        go.SetActive(false);
    }

    IEnumerator StartRun()
    {
        animator.SetFloat("Speed", 0.4f);
        yield return new WaitForSeconds(0.25f);
        animator.SetFloat("Speed", 1);
    }

    IEnumerator StopRun()
    {
        animator.SetFloat("Speed", 0.4f);
        yield return new WaitForSeconds(0.25f);
        animator.SetFloat("Speed", 0);
        run = false;
    }
}
