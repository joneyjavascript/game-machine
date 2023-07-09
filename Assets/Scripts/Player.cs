using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() { }

    public void SetPlaying(bool isPlaying)
    {
        animator.SetBool("isPlaying", isPlaying);
    }

    public void SetAngry(bool isAngry)
    {
        animator.SetBool("isAngry", isAngry);
    }
}
