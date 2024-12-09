using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float temp;
    [SerializeField] GameObject hitExplosion;


    // Update is called once per frame
    void Update()
    {
        // actualizamos temporizador 
        temp -= Time.deltaTime;
        if (temp < 0)
        {
            Destroy(gameObject);
        }

        //actualizaremos la posición
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // instanciamos la animación 
        Instantiate(hitExplosion, transform.position, Quaternion.identity);

        // destruimos el disparo
        Destroy(gameObject);
        
    }
}
