using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerExplosion : MonoBehaviour
{
    ParticleSystem system;

    public void Explode()
    {
        GameManager.instance.PlaySoundEffect("Explosion");
        system.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        system = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() { }
}
