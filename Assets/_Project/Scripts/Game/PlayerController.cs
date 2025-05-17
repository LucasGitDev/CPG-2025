using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody2D rb;

    public float moveSpeed = 3f;

    void Start()
    {
        if (!IsOwner)
            return; // só o dono pode controlar esse jogador

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!IsOwner)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(h, v) * moveSpeed;
    }
}
