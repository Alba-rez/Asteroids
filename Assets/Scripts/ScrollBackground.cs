using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    [SerializeField] float speed;

    float heigth;
    void Start()
    {
        heigth = GetComponent<SpriteRenderer>().bounds.size.y;


    }

    // Update is called once per frame
    void Update()
    {


        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // reposicionar imágenes

        if (transform.position.y < -heigth)
        {
            transform.Translate(Vector3.up * 2 * heigth);
        }
    }
}
