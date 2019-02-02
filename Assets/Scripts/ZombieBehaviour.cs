using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehaviour : MonoBehaviour
{
    // Variables
    private float attackDistance = 0.7f;
    private float chaseDistance = 10f;
    private bool isAlive= true;

    // Components
    private Animator zombieAnimator;
    private NavMeshAgent zombieNavAgent;
    private GameObject player;
    private Renderer zombieRenderer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        zombieAnimator = GetComponent<Animator>();
        zombieNavAgent = GetComponent<NavMeshAgent>();
        zombieNavAgent.enabled = false;
        zombieRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive == true)
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            isAlive = false;
            GameObject.Destroy(collision.gameObject);
            GameObject.Destroy(gameObject, 4);
        }
    }
}