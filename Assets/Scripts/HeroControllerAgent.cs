
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class HeroControllerAgent : NetworkBehaviour {

    public Camera cam;
    public LayerMask groundLayer;
    public NavMeshAgent playerAgent;
    //private float elapsed = 0.0f;

    private void Awake()
    {
        cam = Camera.main;
    }
    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer) { return; }
		if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, groundLayer))
            {
                //Move agent
                playerAgent.SetDestination(hit.point);
                //elapsed = 0.0f;

            }
        }
        if (playerAgent.hasPath)
        {
            for (int i = 0; i < playerAgent.path.corners.Length - 1; i++)
                Debug.DrawLine(playerAgent.path.corners[i], playerAgent.path.corners[i + 1], Color.red);
        }
	}
    private GameObject GetClosestTarget()
    {
        //get all targets in the area. 
        return this.gameObject;
    }
    
}
