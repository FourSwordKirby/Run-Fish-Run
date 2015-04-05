using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public enum State { FREE, STUNNED, GRAPPLE, SWORD, DASH, CHARGE };

    private Animator anim;
    private bool allowKeyMovement = true;
    private bool allowActions = true;

    private GameObject bodyCollider;
    private bool invulnerable;


    //the forward speed of our character
    private float forwardMovementSpeed = 2.0f;


    // Optimization
    private Transform _transform;
    public new Transform transform
    {
        get
        {
            if (_transform == null)
                _transform = base.transform;
            return _transform;
        }
    }

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        //bodyCollider = transform.FindChild("BodyCollider").gameObject;
    }


    void FixedUpdate()
    {
        Vector2 newVelocity = GetComponent<Rigidbody2D>().velocity;
        newVelocity.x = forwardMovementSpeed;
        GetComponent<Rigidbody2D>().velocity = newVelocity;
    }
}
