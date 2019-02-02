using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private float velocity;
    private float maxDistance;
    private Vector3 initialPosition;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        velocity = 100f;
        maxDistance = 500f;
        initialPosition = transform.position;
        GetComponent<Rigidbody>().AddForce(transform.forward*velocity,ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if ((initialPosition - transform.position).magnitude > maxDistance)
            Destroy(this.gameObject);
    }

    public void OnCollisionEnter(Collision collision) {
        Destroy(this.gameObject);
        Instantiate(explosion, transform.position, transform.rotation);
    }
}
