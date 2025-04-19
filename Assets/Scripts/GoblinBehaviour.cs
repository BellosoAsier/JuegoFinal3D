using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private PlayerBehaviour target;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerBehaviour>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.transform.position);
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            directionToTarget.y = 0f;
            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
            transform.rotation = rotationToTarget;
            agent.isStopped = true;
            animator.SetBool("IsAttacking", true);
        }
    }

    //Se ejecuta desde Animacion - Bite
    private void AttackEnd()
    {
        agent.isStopped = false;
        animator.SetBool("IsAttacking", false);
    }
}
