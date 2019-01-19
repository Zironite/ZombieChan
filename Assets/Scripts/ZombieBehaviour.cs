using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehaviour : MonoBehaviour
{
    // Variables
    private float attackDistance = 0.7f;
    private float chaseDistance = 10f;
    private float life = 100f;

    // Components
    private Animator zombieAnimator;
    private NavMeshAgent zombieNavAgent;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        zombieAnimator = GetComponent<Animator>();
        zombieNavAgent = GetComponent<NavMeshAgent>();
        zombieNavAgent.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (life > 0)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
            {
                zombieAnimator.SetBool("Attack", true);
                zombieAnimator.SetBool("Walk", false);
                zombieNavAgent.enabled = false;
            }
            else if (Vector3.Distance(transform.position, player.transform.position) <= chaseDistance)
            {
                zombieAnimator.SetBool("Attack", false);
                zombieAnimator.SetBool("Walk", true);
                zombieNavAgent.enabled = true;
                zombieNavAgent.SetDestination(player.transform.position);
            }
            else
            {
                zombieAnimator.SetBool("Attack", false);
                zombieAnimator.SetBool("Walk", false);
                zombieNavAgent.enabled = false;
            }
        }
        else
        {
            zombieAnimator.SetBool("Attack", false);
            zombieAnimator.SetBool("Walk", false);
            zombieAnimator.SetBool("Alive", false);
            zombieNavAgent.enabled = false;
        }
    }
}
