using System.Collections;
using UnityEngine;

public class characterHareket : MonoBehaviour
{
    public float speed = 3f;
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Maria'n�n pozisyonunu sahne ge�i�i i�in ayarla
        if (SceneTransitionManager.Instance != null)
        {
            transform.position = SceneTransitionManager.Instance.GetSpawnPosition();
        }

        Debug.Log("Maria'n�n hareket scripti ba�lat�ld�.");
    }

    void Update()
    {
        // Y�n bilgisi al
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Animasyonlar� kontrol et
        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        // Maria'n�n fiziksel hareketini yap
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    // Sahne ge�i�i �ncesinde pozisyonu kaydet
    private void OnDestroy()
    {
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.SetSpawnPosition(transform.position);
        }
    }
}
