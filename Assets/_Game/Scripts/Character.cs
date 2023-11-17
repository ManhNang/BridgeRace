using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected SkinnedMeshRenderer skinRenderer;
    [SerializeField] private Material brickColor;
    [SerializeField] LayerMask layerMask;

    [SerializeField] protected GameObject planeStart;
    [SerializeField] private GameObject brickPoint;

    [SerializeField] protected float moveSpeed;
    [SerializeField] private float indexNextBrick;

    [SerializeField] protected Animator anim;
    protected string currentAnim;

    protected NavMeshAgent agent;

    [SerializeField] protected NavMeshSurface navMeshSurface;

    private void Update()
    {
        brickPoint.transform.rotation = transform.rotation;
    }

     void OnTriggerEnter(Collider other)
     {
        if (other.gameObject.CompareTag("Brick") && brickPoint.transform.childCount <= 15)
        {
            brickColor = other.gameObject.GetComponent<MeshRenderer>().material;
            if (skinRenderer.material.color == brickColor.color)
            {
                Vector3 posRespawn = new Vector3();
                posRespawn = other.gameObject.transform.position;
                StartCoroutine(DelayReSpawn(posRespawn));
                AddBrickToPoint(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("Stair"))
        {
            if (brickPoint.transform.childCount != 0 && other.gameObject.GetComponent<MeshRenderer>().material.color != skinRenderer.material.color)
            {
                other.gameObject.GetComponent<MeshRenderer>().material.color = skinRenderer.material.color;

                RemoveBrickToPoint(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("Plane2"))
        {
            planeStart = BrickSpawn.instance.ListPlane[1];
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Stair"))
        {
            if (other.gameObject.GetComponent<MeshRenderer>().material.color == skinRenderer.material.color || agent.velocity.z < 0)
            {
                other.transform.parent.GetChild(1).GetComponent<NavMeshObstacle>().enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stair"))
        {
            other.transform.parent.GetChild(1).GetComponent<NavMeshObstacle>().enabled = true;
        }
    }

    protected void AddBrickToPoint(GameObject other)
    {
        other.transform.SetParent(brickPoint.transform);
        Vector3 posNewBrick = new Vector3(brickPoint.transform.position.x, brickPoint.transform.position.y + indexNextBrick, brickPoint.transform.position.z);
        other.transform.position = posNewBrick;
        other.transform.rotation = brickPoint.transform.rotation;
        indexNextBrick += 0.3f;
    }

    protected void RemoveBrickToPoint(GameObject other)
    {
        int lastChildIndex = brickPoint.transform.childCount - 1;
        Material tag = other.GetComponent<MeshRenderer>().material;
        Queue<GameObject> brickPool = PoolBrick.instance.poolDictionary[tag.color];

        PoolBrick.instance.AddToEnqueue(brickPoint.transform.GetChild(lastChildIndex).gameObject, brickPool);
        indexNextBrick -= 0.3f;
    }

    IEnumerator DelayReSpawn(Vector3 posRespawn)
    {
        yield return new WaitForSeconds(2f);
        BrickSpawn.instance.Spawn(posRespawn, planeStart);
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(animName);

            currentAnim = animName;

            anim.SetTrigger(currentAnim);
        }
    }

    //private void CheckStair()
    //{
    //    if (brickPoint.transform.childCount != 0)
    //    {
    //        RaycastHit hit;
    //        if (Physics.SphereCast(transform.GetChild(0).position, 2f, Vector3.forward, out hit, 2f, layerMask))
    //        {
    //            //hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = true;
    //            hit.collider.gameObject.GetComponent<MeshRenderer>().material.color = skinRenderer.material.color;
    //            //hit.transform.parent.GetChild(1).GetComponent<MeshRenderer>().material.color = skinRenderer.material.color;
    //            //navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);

    //            hit.transform.parent.GetChild(1).GetComponent<NavMeshObstacle>().enabled = false;

    //            hit.collider.gameObject.GetComponent<BoxCollider>().enabled = false;

    //            RemoveBrickToPoint(hit.collider.gameObject);

    //            hit.collider.transform.parent.GetChild(1).GetComponent<BoxCollider>().enabled = false;
    //        }
    //    }
    //}
}
