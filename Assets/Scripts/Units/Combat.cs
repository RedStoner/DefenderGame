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
                CmdShootTarget();
            }

        }
	}
    private bool IsHostile(GameObject obj)
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
    [Command]
    void CmdShootTarget()
    {
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.transform.LookAt(target.transform);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 2.0f);
    }
}
