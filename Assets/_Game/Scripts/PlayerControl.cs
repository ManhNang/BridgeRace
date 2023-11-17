using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : Character
{
    [SerializeField] private FixedJoystick joystick;
    //private bool isMoving = false;

    //private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        LookInDirection();
    }

    private void Move()
    {
        agent.velocity = new Vector3(joystick.Horizontal * moveSpeed, transform.position.y, joystick.Vertical * moveSpeed);
    }

    private void LookInDirection()
    {
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            ChangeAnim("run");
            if (agent.velocity != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(agent.velocity);
            }
        }
    }
}
