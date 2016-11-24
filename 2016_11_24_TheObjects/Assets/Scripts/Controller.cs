using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
    public float speed = 1, rotateSpeed = 2f;
    float lastMouseXPos;
    Rigidbody rb;
	// Use this for initialization
	void Start () {
        lastMouseXPos = Input.mousePosition.x;
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float delta = Input.mousePosition.x - lastMouseXPos;
        lastMouseXPos = Input.mousePosition.x;
        Quaternion newRotation = rb.rotation * Quaternion.Euler(0, delta * rotateSpeed * Time.fixedDeltaTime,0);
        rb.MoveRotation(newRotation);

        if (Input.GetKey(KeyCode.W))
            rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
        if (Input.GetKey(KeyCode.S))
            rb.MovePosition(rb.position - transform.forward * speed * Time.fixedDeltaTime);
        if (Input.GetKey(KeyCode.D))
            rb.MovePosition(rb.position + transform.right * speed * Time.fixedDeltaTime);
        if (Input.GetKey(KeyCode.A))
            rb.MovePosition(rb.position - transform.right * speed * Time.fixedDeltaTime);
        
    }
    
}
