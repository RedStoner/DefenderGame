using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
    public const int maxHealth = 50;
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;
    public RectTransform healthBar;
	// Use this for initialization
	public void TakeDamage (int amt)
    {
        if (!isServer) { return; }
        currentHealth -= amt;
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
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
