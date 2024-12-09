using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
   
    [SerializeField]AudioClip fxExplosion;
    const float DELAY = 0.25f;
    void Start()
    {
        AudioSource.PlayClipAtPoint(fxExplosion, Camera.main.transform.position);
        Destroy(gameObject, DELAY);
    }

    
    void Update()
    {
       
    }
}
