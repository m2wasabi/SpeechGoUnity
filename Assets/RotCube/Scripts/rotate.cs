using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{

    public float Speed = 100.0f;

    public Vector3 RotateVector3;
	// Use this for initialization
	void Start () {
		this.RotateVector3 = new Vector3(Random.Range(-10.0f,10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f)).normalized;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(RotateVector3 * Time.deltaTime * Speed);
	}
}
