using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour {

    public float ExplodeTimer = 2.5f;
    public GameObject Explosion;
	// Use this for initialization
	void Start () {
        StartCoroutine("Explode");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(ExplodeTimer);
        Instantiate(Explosion, this.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
