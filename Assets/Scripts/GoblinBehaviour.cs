using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GoblinBehaviour : MonoBehaviour, Damageable
{
    private NavMeshAgent agent;
    private PlayerBehaviour target;
    private Animator animator;
    [SerializeField] private float health;
    [SerializeField] private GameObject healthContainer;
    private float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerBehaviour>();
        animator = GetComponent<Animator>();
        maxHealth = health;
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
    public void AttackByGoblin()
    {
        Collider[] colliderHits = Physics.OverlapSphere(transform.GetChild(0).position, 0.5f);

        foreach (Collider item in colliderHits)
        {
            if (item.TryGetComponent(out PlayerBehaviour player))
            {
                if (player.TryGetComponent(out Damageable damageable))
                {
                    damageable.DamageTarget(20f);
                }
            }
        }
    }

    //Se ejecuta desde Animacion - Bite
    private void AttackEnd()
    {
        agent.isStopped = false;
        animator.SetBool("IsAttacking", false);
    }

    public void DamageTarget(float damage)
    {
        health -= damage;

        healthContainer.transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = health / maxHealth;
        transform.parent.GetComponent<AudioSource>().Play();

        if (health <= 0)
        {
            FindFirstObjectByType<GoblinSpawner>().NumberOfEnemiesKilled += 1; 
            Destroy(this.gameObject);
       
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.GetChild(0).position, 0.5f);
    }
}
