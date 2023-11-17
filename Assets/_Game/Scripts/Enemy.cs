using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : Character
{
    [SerializeField] private GameObject finish;
    [SerializeField] private List<GameObject> targets;

    [SerializeField] private bool isMoveToBridge = false;
    [SerializeField] private bool isSearchBrick = false;
    [SerializeField] private bool isHaveTarget = false;
    [SerializeField] private bool isMoveToTarget = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        foreach (GameObject target in targets)
        {
            Debug.Log(gameObject.name + " " + transform.position + " target" + target.transform.position);
            if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
            {
                Debug.Log(gameObject.name + " ss");
                isMoveToTarget = true;
            }
        }
        

        IState();
    }

    private Vector3 SearchBrick()
    {
        if (planeStart.transform.childCount != 0)
        {
            Vector3 brickTarget = transform.position;
            for (int i = 0; i < planeStart.transform.childCount; i++)
            {
                if (planeStart.transform.GetChild(i).GetComponent<MeshRenderer>().material.color == skinRenderer.material.color)
                {
                    if (brickTarget == transform.position)
                    {
                        brickTarget = planeStart.transform.GetChild(i).position;
                        continue;
                    }

                    float distance = Vector3.Distance(transform.position, brickTarget);
                    
                    if (distance > Vector3.Distance(transform.position, planeStart.transform.GetChild(i).position))
                    {
                        brickTarget = planeStart.transform.GetChild(i).position;
                    }
                }
            }
            return brickTarget;
        }
        return transform.position;
    }

    private Vector3 MoveToBridge()
    {
        Vector3 targetNear = targets[0].transform.position;

        float checkNear = Vector3.Distance(transform.position, targetNear);

        for (int i = 1; i < targets.Count; i++)
        {
            if (checkNear > Vector3.Distance(transform.position, targets[i].transform.position))
            {
                checkNear = Vector3.Distance(transform.position, targets[i].transform.position);
                targetNear = targets[i].transform.position;
            }
        }
        isHaveTarget = true;

        return targetNear;
    }

    private void IState()
    {
        ChangeAnim("run");
        int numberBrick = Random.Range(9, 10);
        if (transform.GetChild(1).childCount < numberBrick && !isMoveToBridge)
        {
            isSearchBrick = true;
            agent.destination = SearchBrick();
        }
        else
        {
            isSearchBrick = false;
        }

        if (!isSearchBrick && transform.GetChild(1).childCount > 0)
        {
            isMoveToBridge = true;

            if (!isMoveToTarget && !isHaveTarget)
            {
                agent.destination = MoveToBridge();
            }
            else
            {
                agent.destination = finish.transform.position;
            }
        }
        else
        {
            isMoveToBridge = false;
        }
    }
}
