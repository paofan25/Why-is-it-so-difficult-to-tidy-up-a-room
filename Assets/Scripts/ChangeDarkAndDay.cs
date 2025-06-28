using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDarkAndDay : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToDark(){
        animator.Play("ye");
    }

    public void ChangeToLight(){
        animator.Play("bai");
    }
}
