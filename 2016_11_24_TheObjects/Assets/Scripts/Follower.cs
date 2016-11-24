using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {
    Vector3 delta;
    GameObject player;
    public float inertia = 0.03f;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        delta = player.transform.position - transform.position;
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = Vector3.Lerp(transform.position, player.transform.position - delta, inertia);
	}
}
