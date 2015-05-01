using UnityEngine;
using System.Collections;

public class PipeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnMouseDown()
    {
        Debug.Log("living");
        transform.FindChild("Interaction-Water").GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 6000));
    }  
}
