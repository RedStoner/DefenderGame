using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
    public bool destroyOnDeath;
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth;
    public RectTransform healthBar;
    private Attributes attributes;
    // Use this for initialization
    private void Start()
    {
        attributes = gameObject.GetComponent<Attributes>();
        currentHealth = attributes.maxHealth;
    }
    public void TakeDamage (int amt)
    {
        if (!isServer) { return; }
        if (amt - attributes.armor < 0)
        {
            amt = 0;
        } else
        {
            amt -= attributes.armor;
        }
        currentHealth -= amt;
        if (currentHealth <= 0)
        {
            currentHealth = attributes.maxHealth;
            print("A " + attributes.unitType + " has died!");
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                RpcRespawn();
            }
        }
    }
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            transform.position = new Vector3(10f, 1f, 10f);
        }
    }
    void OnChangeHealth (int currentHealth)
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }
}
