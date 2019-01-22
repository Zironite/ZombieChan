using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private float velocity;
    private float maxDistance;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        velocity = 100f;
        maxDistance = 500f;
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((initialPosition - transform.position).magnitude <= maxDistance)
            transform.position += transform.forward*velocity*Time.fixedDeltaTime;
        else
            Destroy(this.gameObject);
    }
}
