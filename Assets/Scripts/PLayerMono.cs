using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PLayerMono : MonoBehaviour
{
    public int x;
    public int y;
    public Animator animator;
    public NavMeshAgent agent;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    public void Update()
    {
        animator.SetFloat("Velocity", agent.velocity.magnitude);
    }
}
