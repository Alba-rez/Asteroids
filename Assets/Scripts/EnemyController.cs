using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed;
    const float DESTROY_HEIGTH = -6;
    [SerializeField] GameObject Explosion;

    [SerializeField] GameObject shoot;
    [SerializeField] float shootDelay;
    [SerializeField] float shootProb;


    private void Start()
    {
        StartCoroutine("Shoot");
    }
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < DESTROY_HEIGTH)
        {
            Destroy(gameObject);
        }

    }
    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootDelay);

            GameObject player = GameObject.FindWithTag("Player");

            if (player != null && (transform.position.x)>(player.transform.position.x -0.25f) 
                && (transform.position.x<player.transform.position.x +0.25f))
            {
                Instantiate(shoot, transform.position, Quaternion.identity);
            }

        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        DestroyEnemy();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        DestroyEnemy();  
    }

    void DestroyEnemy()
    {
        GameManager.GetInstance().AddScore(gameObject.tag);

        Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
