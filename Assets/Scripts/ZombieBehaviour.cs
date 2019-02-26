using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

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
    private Transform[] points;
    private Transform[] shuffledPoints;
    private int nextDest;
    private int lastPlayerHealth;
    public AudioClip zombieGrowl;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        zombieAnimator = GetComponent<Animator>();
        zombieNavAgent = GetComponent<NavMeshAgent>();
        zombieNavAgent.enabled = true;
        zombieRenderer = GetComponent<Renderer>();
        points = GameObject.FindGameObjectsWithTag("PatrolPoint").Select(p => p.transform).ToArray();
        shuffledPoints = points.OrderBy(p => Random.Range(0,2) == 0).ToArray();
        nextDest = 0;
        lastPlayerHealth = 100;
        zombieAnimator.SetBool("Attack", false);
        zombieAnimator.SetBool("Walk", true);
        zombieNavAgent.enabled = true;
        GotToNextDestination();
        InvokeRepeating("Growl", 0, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive == true)
        {
            if (player.GetComponent<SciFiSoldierController>().health > 0 && 
                Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
            {
                zombieAnimator.SetBool("Attack", true);
                zombieAnimator.SetBool("Walk", false);
                zombieNavAgent.enabled = false;
            }
            else if (player.GetComponent<SciFiSoldierController>().health > 0 && 
                Vector3.Distance(transform.position, player.transform.position) <= chaseDistance)
            {
                zombieAnimator.SetBool("Attack", false);
                zombieAnimator.SetBool("Walk", true);
                zombieNavAgent.enabled = true;
                zombieNavAgent.SetDestination(player.transform.position);
            }
            else if ((player.GetComponent<SciFiSoldierController>().health <= 0 && lastPlayerHealth > 0) || 
                (!zombieNavAgent.pathPending && Vector3.Distance(transform.position, zombieNavAgent.destination) <= 1))
            {
                zombieAnimator.SetBool("Attack", false);
                zombieAnimator.SetBool("Walk", true);
                zombieNavAgent.enabled = true;
                GotToNextDestination();
            }
            lastPlayerHealth = player.GetComponent<SciFiSoldierController>().health;
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
            GetComponent<CapsuleCollider>().enabled = false;
            GameObject.Destroy(gameObject, 4);
        }
    }

    private void GotToNextDestination() {
        zombieNavAgent.SetDestination(shuffledPoints[nextDest].position);
        if(nextDest == shuffledPoints.Length - 1) nextDest = 0;
        else nextDest++;
    }

    private void Growl() {
        if(Random.Range(0,1.0f) >= Random.Range(0,0.4f)) {
            AudioSource.PlayClipAtPoint(zombieGrowl, transform.position, 0.2f);
        }
    }
}