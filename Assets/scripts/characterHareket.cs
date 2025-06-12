using System.Collections;
using UnityEngine;

public class characterHareket : MonoBehaviour
{
    public float speed = 3f;                        // Normal yürüme hýzý
    public float sprintMultiplier = 1.7f;           // Koþma hýzýný artýran çarpan
    public Rigidbody2D rb;                           // Rigidbody2D komponent referansý (fizik için)
    public Animator animator;                        // Animatör komponent referansý (animasyonlar için)
    Vector2 movement;                                // Hareket girdilerini tutacak vektör

    void Start()
    {
        // Rigidbody ve Animator componentlerini al
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Eðer SceneTransitionManager varsa, önceki sahneden gelen pozisyonu al ve konumu ayarla
        if (SceneTransitionManager.Instance != null)
        {
            transform.position = SceneTransitionManager.Instance.GetSpawnPosition();
        }
        Debug.Log("Maria'nýn hareket scripti baþlatýldý.");  // Baþlangýç mesajý
    }

    void Update()
    {
        // Kullanýcýnýn yatay ve dikey hareket giriþlerini oku (-1, 0, 1 deðerleri)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Animatöre hareket yönü ve hýz bilgisini gönder
        animator.SetFloat("moveX", movement.x);                     // X ekseni hareketi
        animator.SetFloat("moveY", movement.y);                     // Y ekseni hareketi
        animator.SetFloat("speed", movement.sqrMagnitude);          // Hareket hýzýnýn karesi (performans için optimize)
    }

    void FixedUpdate()
    {
        // Temel hareket hýzý
        float currentSpeed = speed;

        // Eðer Shift tuþuna basýlýyorsa koþma hýzýný uygula
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        // Rigidbody'yi kullanarak yeni pozisyona hareket ettir
        // Time.fixedDeltaTime ile zaman baðýmlý düzgün hareket saðla
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
    }

    // Bu obje yok olurken (örneðin sahne deðiþirken) çaðrýlýr
    private void OnDestroy()
    {
        // Sahne geçiþ yöneticisi varsa Maria'nýn son pozisyonunu kaydet
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.SetSpawnPosition(transform.position);
        }
    }
}
