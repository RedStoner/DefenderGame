using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Combat : NetworkBehaviour {
    public float effectTime = .15f;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private float attackCooldown = 0.0f;
    private Attributes attributes;
    private float attackSpeed;
    private float attackRange;
    private GameObject target;
    private float lineTimer;
    LineRenderer gunLine;
    // Use this for initialization
    void Start ()
    {
        attributes = gameObject.GetComponent<Attributes>();
        attackSpeed = attributes.attackSpeed;
        attackRange = attributes.attackRange;
        gunLine = GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        EffectsCheck();
        attackCooldown += Time.deltaTime;
        if (attackCooldown >= attackSpeed)
        {
            attackCooldown -= attackSpeed;
            if (target == null || !TargetInRange(target))
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
                        if (HasLineOfSight(hitColliders[i].gameObject))
                        {
                            target = hitColliders[i].gameObject;
                        }
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
                    CmdShootTargetHitScan();
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
    void CmdShootTargetBullet()
    {
        gameObject.transform.LookAt(target.transform);
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.transform.LookAt(target.transform);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 20;
        bullet.GetComponent<ProjectileBullet>().damageAmount = attributes.attackDamage;
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 1.0f);
    }
    [Command]
    void CmdShootTargetHitScan()
    {
        gameObject.transform.LookAt(target.transform);
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);
        gunLine.SetPosition(1, target.transform.position);
        target.GetComponent<Health>().TakeDamage(attributes.attackDamage);
    }
    private bool TargetInRange(GameObject checkTarget)
    {
        if (checkTarget == null)
        {
            return false;
        }
        double distance = Vector3.Distance(gameObject.transform.position, checkTarget.transform.position);
        if (distance > attackRange)
        {
            return false;
        }
        return HasLineOfSight(checkTarget);
        
    }
    private bool HasLineOfSight(GameObject checkTarget)
    {
        bulletSpawn.transform.LookAt(checkTarget.transform);
        RaycastHit hit;
        if (Physics.Raycast(bulletSpawn.transform.position, bulletSpawn.transform.forward, out hit, attackRange ))
        {
            if (hit.transform.gameObject == checkTarget)
            {
                return true;
            }
        } 
        return false;

    }
    private void EffectsCheck()
    {

        lineTimer += Time.deltaTime;
        if (lineTimer >= effectTime)
        {
            gunLine.enabled = false;
        }
    }
}
