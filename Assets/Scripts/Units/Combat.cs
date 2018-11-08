using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Combat : NetworkBehaviour {
    public float effectTime = .15f;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Transform rotator;

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
            //check if current target is in range if it has a target.
            if (HasTarget())
            {
                //target has left attack range, clear target
                if (!TargetInRange(target))
                {
                    target = null;
                    Log("Prior Target In Range: false");
                } else
                {
                    Log("Prior Target In Range: true");
                }
            }
            //If there is no target, get a new one.
            if (!HasTarget())
            {
                Log("Aquiring New Target.");
                Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, attackRange);
                Log("Number of targets in range: " + hitColliders.Length);
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
                        if (HasLineOfSight(hitColliders[i].gameObject))
                        {
                            Log("Found valid target in scan.");
                            target = hitColliders[i].gameObject;
                            break;
                        } else
                        {
                        }
                    } else
                    {
                        Log("Range Test - Not a valid hostile");
                    }
                    i++;
                }
            }
            //Attack the target.
            if (HasTarget())
            {
                //check if moving
                if (gameObject.GetComponent<HeroControllerAgent>().playerAgent.hasPath)
                {
                    Log("Can't Shoot, Still Moving.");
                } else {
                    CmdShootTargetHitScan();
                    //Log("Shooting");
                }
            } else
            {
                Log("No Target At End");
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
        LookAt(transform, target.transform);
        bulletSpawn.LookAt(target.transform);
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
        LookAt(transform, target.transform);
        bulletSpawn.LookAt(target.transform);
        gunLine.enabled = true;
        gunLine.SetPosition(0, bulletSpawn.position);
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
            //Log("Target not in range.");
            return false;
        }
        return HasLineOfSight(checkTarget);
        
    }
    private bool HasLineOfSight(GameObject checkTarget)
    {
        LookAt(rotator, checkTarget.transform);
        bulletSpawn.transform.LookAt(checkTarget.transform);
        RaycastHit hit;
        bool isValid = false;
        if (Physics.Raycast(bulletSpawn.transform.position, bulletSpawn.transform.forward, out hit, attackRange ))
        {
            if (hit.transform.gameObject == checkTarget)
            {
                //Log("Has Line of sight.");
                isValid = true;
            }
        }
        rotator.rotation = transform.rotation;
        return isValid;

    }
    private void EffectsCheck()
    {

        lineTimer += Time.deltaTime;
        if (lineTimer >= effectTime)
        {
            gunLine.enabled = false;
        }
    }
    public bool HasTarget()
    {
        if (target != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void Log(string str)
    {
        print("[" + Time.fixedTime + "] " + str);
    }
    void LookAt(Transform lookie, Transform lookTarget)
    {
        lookie.LookAt(new Vector3(lookTarget.position.x, lookie.position.y, lookTarget.position.z));
    }
}
