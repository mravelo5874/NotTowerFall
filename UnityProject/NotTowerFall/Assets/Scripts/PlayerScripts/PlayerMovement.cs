using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PhysicsObject
{
    public float speed = 1f;
    void Update()
    {
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);
        bool jump = Input.GetKey(KeyCode.Space);

        if (jump)
        {
            print ("jump bitch");
            JUMP();
        }

        targetVelocity= Vector2.zero;

        if (left && right)
            return;

        if (left)
        {
            targetVelocity = Vector2.left * speed;
        }
        else if (right)
        {
            targetVelocity = Vector2.right * speed;
        }
    }
}
