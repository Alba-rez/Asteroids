using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] GameObject asteroid;
    [SerializeField] float minInterval;
    [SerializeField] float maxInterval;
    [SerializeField] float delay;

    Vector3 initialPosition;
    void Start()
    {
        initialPosition = transform.position;
        StartCoroutine("AsteroidSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator AsteroidSpawn()

    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
           
            //instanciamos los asteroides
            Instantiate(asteroid, initialPosition, Quaternion.identity);

            // detenemos la corrutina por un tiempo aleatorio
            yield return new WaitForSeconds(Random.Range(minInterval,maxInterval));
        }
    }
}
