using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
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
        currentHealth -= amt - attributes.armor;
        if (currentHealth <= 0)
        {
            currentHealth = attributes.maxHealth;
            print("A hero has died!");
            RpcRespawn();
        }
    }
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            transform.position = new Vector3(0f, 1f, 0f);
        }
    }
    void OnChangeHealth (int currentHealth)
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }
}
