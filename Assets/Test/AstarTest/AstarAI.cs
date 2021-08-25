using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Seeker))]
public class AstarAI : MonoBehaviour
{
    public Transform targetPosition;
    public float moveSpeed = 2;
    public float nextWaypointDistance = 3;

    private CharacterController m_CharacterController;
    private Seeker m_Seeker;
    private Path m_Path;
    private int m_CurWayPoint = 0;
    private bool reachedEndPoint = false;

    // Start is called before the first frame update
    void Start()
    {
        m_CharacterController = this.GetComponent<CharacterController>();
        m_Seeker = this.GetComponent<Seeker>();

        if (targetPosition != null)
        {
            m_Seeker.StartPath(this.transform.position, targetPosition.position, this.OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            m_Path = p;
            m_CurWayPoint = 0;
            reachedEndPoint = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Path == null)
        {
            return;
        }

        Vector3 curPos = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        var distance = Vector3.Distance(curPos, m_Path.vectorPath[m_CurWayPoint]);
        if (distance < nextWaypointDistance)
        {
            if (m_CurWayPoint + 1 < m_Path.vectorPath.Count)
            {
                m_CurWayPoint++;
            }
            else
            {
                reachedEndPoint = true;
            }
        }

        float speedFactor = reachedEndPoint ? Mathf.Sqrt(distance / nextWaypointDistance) : 1f;

        Vector3 moveDir = m_Path.vectorPath[m_CurWayPoint] - curPos;
        moveDir = moveDir.normalized;

        m_CharacterController.SimpleMove(moveDir * moveSpeed * speedFactor);
    }
}