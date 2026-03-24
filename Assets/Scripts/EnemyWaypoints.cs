using UnityEngine;
using UnityEngine.AI;


public class EnemyWaypoints : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] waypoints;

    int m_CurrentWaypointIndex;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.SetDestination(waypoints[0].position);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(m_CurrentWaypointIndex.ToString());
        //enemy must have a nonzero stopping distance
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1);
            
            if (m_CurrentWaypointIndex < waypoints.Length-1)
            {
                agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                
                
            }
            else
            {
                agent.SetDestination(waypoints[waypoints.Length-1].position);
            }
            
        }
        
    }
}
