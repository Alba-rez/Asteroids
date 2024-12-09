using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    const float SHOOT_OFFSET = 0.5f;
    [SerializeField] float force;
    [SerializeField] Vector3 endPosition;
    [SerializeField] float duration;
    [SerializeField] int blinkNum;
    [SerializeField] GameObject shoot;
    [SerializeField] GameObject Explosion;

    Rigidbody2D rb;
    bool active;
    Vector3 posicionInicial;
    GameManager game;

    void Start()
    {
        posicionInicial = transform.position;
        rb = GetComponent<Rigidbody2D>();
        game = GameManager.GetInstance();
        StartCoroutine("StartPlayer");
    }
   void Update()
    {
        if (active && !game.isGamePaused() && Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(shoot, new Vector3(transform.position.x, transform.position.y + SHOOT_OFFSET, 0),Quaternion.identity);
        }

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        string tag = other.gameObject.tag;
        if (tag== "ShootEnemy" || tag == "Enemy" || tag == "AsteroidBig"|| tag=="AsteroidSmall" )
        {
            DestroyShip();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ShootEnemy")
        {
            DestroyShip();
        }
    }

    void DestroyShip()
    {
        active = false;
        //instaciamos la explosión
        Instantiate(Explosion, transform.position, Quaternion.identity);

        // actualizamos las vidas 
        GameManager gm = GameManager.GetInstance();
        gm.LoseLive();

        // instanciamos una nueva nave
        if (!gm.isGameOver())
        Instantiate(gameObject, posicionInicial, Quaternion.identity);
        // destruimos la vieja nave
        Destroy(gameObject);

    }

    void FixedUpdate()
    {
        if (active)

            CheckMove();
    }

    private void CheckMove()
    {
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        direction.Normalize();

        // aplicamos una fuerza en esa dirección 
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    IEnumerator StartPlayer()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        Vector3 initialPosition = transform.position;
        Material mat = GetComponent<SpriteRenderer>().material;
        Color color = mat.color;
        float t = 0, t2 = 0;
        while (t < duration)
        {
            // traslación del objeto
            t += Time.deltaTime;
            Vector3 newPosition = Vector3.Lerp(initialPosition, endPosition, t / duration);
            transform.position = newPosition;

            // parpadeo
            t2 += Time.deltaTime;
            float newAlpha=blinkNum*(t2/duration);
            if (newAlpha > 1)
                t2 = 0;
            color.a = newAlpha;
            mat.color = color;

            yield return null;

        }

        color.a = 1;
        mat.color=color;

        transform.position = endPosition;

        // activamos las colisiones

        collider.enabled = true;

        // activamos flag para la nave 
        active = true;

    }

}
