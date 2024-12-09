using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [SerializeField] float force;
    [SerializeField] GameObject Explosion;
     [SerializeField] GameObject asteroid;

    Rigidbody2D rb;
    int hitCount=0;

    const int  HITS_TO_DESTROY= 4;
    const float DESTROY_HEIGTH = -6;

    void Start()
    {
        if (tag == "AsteroidBig")
        {
            LaunchBigAsteroid();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < DESTROY_HEIGTH)
        {
            Destroy(gameObject);
        }

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            DestroyAsteroids();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (tag == "AsteroidBig")
        {
            ++hitCount;
            if (hitCount == HITS_TO_DESTROY)
            {
                LaunchSmallAsteroid();
                DestroyAsteroids();
            }
        }
        else
        {
            DestroyAsteroids();
        }
        
        
    }
    void DestroyAsteroids()
    {
        GameManager.GetInstance().AddScore(gameObject.tag);
        Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void LaunchBigAsteroid()
    {
        Vector2 direct = new Vector2(1, -0.25f);
        direct.Normalize();
        rb=GetComponent<Rigidbody2D>();
        rb.AddForce(direct * force,ForceMode2D.Impulse);
        rb.AddTorque(-.1f, ForceMode2D.Impulse);

    }

    void LaunchSmallAsteroid()
    {
        // posición y velocidad del padre
        Vector2 linearVelocity = rb.velocity;
        float angularVelocity = rb.angularVelocity;
        Vector3 position = rb.position;

        //instanciar los asteroides pequeños
        for (int i = 0, s = 1; i < 2; i++, s *= -1)
        {
            GameObject smallAsteroid = Instantiate(asteroid, position, Quaternion.identity);
            Rigidbody2D rbSmall = smallAsteroid.GetComponent<Rigidbody2D>();
            rbSmall.velocity = new Vector2(s * linearVelocity.x, linearVelocity.y);
            rb.angularVelocity = angularVelocity;
        }

    }
}
