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

        // Maria'nýn pozisyonunu sahne geçiþi için ayarla
        if (SceneTransitionManager.Instance != null)
        {
            transform.position = SceneTransitionManager.Instance.GetSpawnPosition();
        }

        Debug.Log("Maria'nýn hareket scripti baþlatýldý.");
    }

    void Update()
    {
        // Yön bilgisi al
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Animasyonlarý kontrol et
        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        // Maria'nýn fiziksel hareketini yap
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    // Sahne geçiþi öncesinde pozisyonu kaydet
    private void OnDestroy()
    {
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.SetSpawnPosition(transform.position);
        }
    }
}
