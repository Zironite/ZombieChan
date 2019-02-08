using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterTime());   
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator DestroyAfterTime() {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
