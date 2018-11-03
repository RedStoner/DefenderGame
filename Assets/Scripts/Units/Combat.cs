using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Combat : NetworkBehaviour {
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    private float attackCooldown = 0.0f;
    private Attributes attributes;
    private float attackSpeed;
    private float attackRange;
    private GameObject target;
    // Use this for initialization
    void Start ()
    {
        attributes = gameObject.GetComponent<Attributes>();
        attackSpeed = attributes.attackSpeed;
        attackRange = attributes.attackRange;
    }
	
	// Update is called once per frame
	void Update () {
        attackCooldown += Time.deltaTime;
        if (attackCooldown >= attackSpeed)
        {
            attackCooldown -= attackSpeed;
            if (target == null)
            {
                Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, attackRange);
                if (hitColliders.Length < 0)
                {
                    print("No Target");
                    return;
                }
                int i = 0;
                while (i < hitColliders.Length)
                {
                    if (IsHostile(hitColliders[i].gameObject))
                    {
                        //check if its closest and has line of sight.
                        target = hitColliders[i].gameObject;
                    }
                    i++;
                }
            }
            if (target != null)
            {
                //check if moving
                if (gameObject.GetComponent<HeroControllerAgent>().playerAgent.hasPath)
                {
                    print("Cant Shoot, Still Moving.");
                } else {
                    CmdShootTarget();
                    print("Shooting");
                }
            } else
            {
                print("No Target");
            }

        }
	}
    private bool IsHostile(GameObject obj)
    {
        if (obj.GetComponent<Attributes>())
        {
            string type = obj.GetComponent<Attributes>().unitType;
            if (type == "Hostile" || type == "Boss")
            {
                return true;
            } else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }
    [Command]
    void CmdShootTarget()
    {
        gameObject.transform.LookAt(target.transform);
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.transform.LookAt(target.transform);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 20;
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 1.0f);
    }
}
