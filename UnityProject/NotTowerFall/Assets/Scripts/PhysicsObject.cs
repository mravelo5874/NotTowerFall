using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    // -------- PUBLIC VARIABLES -------- //
    public float gravityModifier = 1f;
    public float minGroundNormalY = 0.65f;
    public float jumpForce = 3f;


    // -------- PRIVATE VARIABLES -------- //
    private Vector2 velocity;

    // Components:
    private Rigidbody2D rb2d;

    // Collision Detection:
    private bool grounded;
    private Vector2 groundNormal;
    private ContactFilter2D contactFilter;
    private RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    private List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);


    // -------- PROTECTED VARIABLES -------- //

    // Horizontal Movement:
    protected Vector2 targetVelocity;
    

    // -------- CONSTANT VARIABLES -------- //
    private const float minMoveDistance = 0.001f;
    private const float shellRadius = 0.01f;

    void OnEnable() 
    {
        rb2d = GetComponent<Rigidbody2D>();    
    }

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); // use object settings for checking collisions
        contactFilter.useLayerMask = true;

    }

    void FixedUpdate() 
    {
        // apply gravity to object
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 deltaPos = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2 (groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPos.x;

        Movement(move, false); // horizontal movement

        move = Vector2.up * deltaPos.y;

        Movement(move, true); // vertical movement
    }

    protected void JUMP()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;
        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius); // get number of collisions with other objects
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
                hitBufferList.Add(hitBuffer[i]);

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currNormal = hitBufferList[i].normal;
                if (currNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currNormal;
                        currNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currNormal); // get difference to determine if we need to subtract from object velocity
                if (projection < 0)
                    velocity = velocity - projection * currNormal;

                float modifiedDist = hitBufferList[i].distance - shellRadius;
                distance = modifiedDist < distance ? modifiedDist : distance;
            }
        }
        rb2d.position = rb2d.position + move.normalized * distance;
    }
}
