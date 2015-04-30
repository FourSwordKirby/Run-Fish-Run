using UnityEngine;
using System.Collections;

public class PlayerCore : MonoBehaviour
{
    public enum State { FREE, STUNNED, GRAPPLE, SWORD, DASH, CHARGE };

    //the forward speed of our character
    private const float DEFAULT_MOVESPEED = 2.0f;

    private bool allowKeyMovement = true;
    private bool allowActions = true;
    private float invulnerable = 0.0f;

    // Physics related buffers
    private bool allowExternalInstantVelocity;
    private Vector2 instantVelocity; // For occurances where the velocity must be overriden
    private Vector2 impulseBuffer;   // For "single frame" impacts
    private Vector2 forceBuffer;     // For forces over time

    private Animator anim;
    private new Rigidbody2D rigidbody2D;
    private BoxCollider2D collider;

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

    void OnMouseDown()
    {
        // this object was clicked - do something
        Destroy(this.gameObject);
    }  

    private void Initialize()
    {
        allowExternalInstantVelocity = false;
        ClearPhysicsBuffers();
        anim.SetTrigger("Running");
        collider.enabled = true;
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        Initialize();
    }


    void Update()
    {

    }

    void FixedUpdate()
    {
        /*
         * Velocity update
         */

        // Instant velocity commands overrides everything else.
        if (allowExternalInstantVelocity && instantVelocity != Vector2.zero)
            rigidbody2D.velocity = instantVelocity;
        else
        {
            Vector2 newVelocity = rigidbody2D.velocity;
            if (impulseBuffer == Vector2.zero && forceBuffer == Vector2.zero)
            {
                // Gradual return to default Movespeed
                newVelocity.x = Mathf.Lerp(rigidbody2D.velocity.x, DEFAULT_MOVESPEED, 0.5f);
            }
            else
            {
                // Add impulses (change in momentum)
                newVelocity += impulseBuffer / rigidbody2D.mass;

                // Add forces (F = ma  ==>  a = F /m  ==> deltaV = a * dt)
                newVelocity += (forceBuffer / rigidbody2D.mass) * Time.deltaTime;
            }
            rigidbody2D.velocity = newVelocity;
        }

        // Reset buffers
        ClearPhysicsBuffers();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // If incoming obstacle is harmful
        if(coll.gameObject.GetComponent<HarmfulPart>() != null) {
            Die();
        }
    }

    private void ClearPhysicsBuffers()
    {
        instantVelocity = Vector2.zero;
        impulseBuffer = Vector2.zero;
        forceBuffer = Vector2.zero;
    }

    private void Die()
    {
        collider.enabled = false;
        anim.SetTrigger("Die");
    }

    public void SetInstantVelocity(Vector2 v)
    {
        instantVelocity = v;
    }

    public void ApplyImpulse(Vector2 p)
    {
        impulseBuffer += p;
    }

    public void ApplyForce(Vector2 f)
    {
        forceBuffer += f;
    }
}
