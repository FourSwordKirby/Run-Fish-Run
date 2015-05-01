using UnityEngine;
using System.Collections;

public class SewerCapScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1500f));
    }  
}
