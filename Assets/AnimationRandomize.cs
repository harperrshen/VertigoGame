using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRandomize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        anim.Play(0, -1, UnityEngine.Random.Range(0f, 1f)); // Play at random normalized time (0 to 1)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
