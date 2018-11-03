using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class EnemyPathing : NetworkBehaviour {

    public double attackRange;
    public float pathingUpdateTime;
    
    public NavMeshAgent unitAgent;
    
    private Attributes attributes;
    private GameObject[] targets;
    private GameObject target;

    private NavMeshPath path;
    private float pathingCooldown = 0.0f;

    // Use this for initialization
    void Start () {
        path = new NavMeshPath();
        attributes = gameObject.GetComponent<Attributes>();
        attackRange = attributes.attackRange;
        pathingUpdateTime = attributes.pathingUpdateTime;
        unitAgent.speed = attributes.moveSpeed;

    }
	
	// Update is called once per frame
	void Update () {
        if (isServer)
        {
            //Pathfinding
            DoPathing();
        }
	}
    private void DoPathing()
    {
        pathingCooldown += Time.deltaTime;
        if (pathingCooldown > pathingUpdateTime)
        {
            pathingCooldown -= pathingUpdateTime;

            if (target == null)
            {
                target = FindNewTarget();
            }
            
            if (target != null)
            {
                //check if target is in attack range if so attack.
                double dist = GetDistance(target.transform.position);
                //print("Distance from target" + dist);
                if (dist <= attackRange)
                {
                    //attack mechanics done in different timer in update loop.
                    //stop moving to target once in range
                    if (unitAgent.hasPath)
                    {
                        unitAgent.ResetPath();
                    }
                    return;
                }
                //if target is not in range, move closer to the target. 
                //NavMesh.CalculatePath(unitAgent.transform.position, target.transform.position, NavMesh.AllAreas, path);
                //unitAgent.path = path;
                unitAgent.SetDestination(target.transform.position);
                //unitAgent.Move(unitAgent.transform.forward);
                    //if so move to target
                    //if not find new target
                        //check tracking range for new target
                        //if no target found, move back to spawn point

            }
        }
        //Debug.Log(unitAgent.transform.position);
    }
    public GameObject FindNewTarget()
    {
        Debug.Log("Finding Target");
        //print("Aquiriring Targets");
        targets = GameObject.FindGameObjectsWithTag("Player Hero");
        
        //check if it found any targets.
        if (targets.Length > 0)
        {
            if (targets.Length == 1)
            {
                //print("Only one target.");
                return targets[0];
            }
            //print("Multiple targets found.");
            List<float> distances = new List<float>();
            List<double> weights = new List<double>();
            foreach (GameObject t in targets)
            {
                //calculate the distance to the target and save it temporarily.
                float testDistance = GetDistance(t.transform.position);
                distances.Add(testDistance);
                //calculate and save the weight value.
                weights.Add(testDistance / t.GetComponent<Attributes>().pathingWeight);
            }
            int bestIndex = 0;
            for (int i = 0; i < targets.Length; i++)
            {
                if (weights[i] < weights[bestIndex])
                {
                    bestIndex = i;
                }
                else if (weights[i] == weights[bestIndex])
                {
                    if (distances[i] < distances[bestIndex])
                    {
                        bestIndex = i;

                    }
                }
            }
            return targets[bestIndex];

        } else {
            print("ALL PLAYERS DEAD?!?!?!");
            return null;
        }
    }
    public float GetDistance(Vector3 t)
    {
        NavMesh.CalculatePath(unitAgent.transform.position, t, NavMesh.AllAreas, path);
        float dist = 0.0f;
        for (int i = 0; i < path.corners.Length - 1; i++)
            dist += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        Debug.Log(dist);
        return dist;  
    }
}
