using System.Collections;
using UnityEngine;

public class characterHareket : MonoBehaviour
{
    public float speed = 3f;                        // Normal y�r�me h�z�
    public float sprintMultiplier = 1.7f;           // Ko�ma h�z�n� art�ran �arpan
    public Rigidbody2D rb;                           // Rigidbody2D komponent referans� (fizik i�in)
    public Animator animator;                        // Animat�r komponent referans� (animasyonlar i�in)
    Vector2 movement;                                // Hareket girdilerini tutacak vekt�r

    void Start()
    {
        // Rigidbody ve Animator componentlerini al
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // E�er SceneTransitionManager varsa, �nceki sahneden gelen pozisyonu al ve konumu ayarla
        if (SceneTransitionManager.Instance != null)
        {
            transform.position = SceneTransitionManager.Instance.GetSpawnPosition();
        }
        Debug.Log("Maria'n�n hareket scripti ba�lat�ld�.");  // Ba�lang�� mesaj�
    }

    void Update()
    {
        // Kullan�c�n�n yatay ve dikey hareket giri�lerini oku (-1, 0, 1 de�erleri)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Animat�re hareket y�n� ve h�z bilgisini g�nder
        animator.SetFloat("moveX", movement.x);                     // X ekseni hareketi
        animator.SetFloat("moveY", movement.y);                     // Y ekseni hareketi
        animator.SetFloat("speed", movement.sqrMagnitude);          // Hareket h�z�n�n karesi (performans i�in optimize)
    }

    void FixedUpdate()
    {
        // Temel hareket h�z�
        float currentSpeed = speed;

        // E�er Shift tu�una bas�l�yorsa ko�ma h�z�n� uygula
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        // Rigidbody'yi kullanarak yeni pozisyona hareket ettir
        // Time.fixedDeltaTime ile zaman ba��ml� d�zg�n hareket sa�la
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
    }

    // Bu obje yok olurken (�rne�in sahne de�i�irken) �a�r�l�r
    private void OnDestroy()
    {
        // Sahne ge�i� y�neticisi varsa Maria'n�n son pozisyonunu kaydet
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.SetSpawnPosition(transform.position);
        }
    }
}
