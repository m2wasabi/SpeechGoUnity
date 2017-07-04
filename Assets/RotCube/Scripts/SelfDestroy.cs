using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour {

    public float EliminateTimer = 1.5f;

    // Use this for initialization
    void Start () {
        StartCoroutine("Eliminate");
    }

    // Update is called once per frame
    void Update () {
		
	}
    IEnumerator Eliminate()
    {
        yield return new WaitForSeconds(EliminateTimer);
        Destroy(gameObject);
    }
}
