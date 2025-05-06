using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterHareket : MonoBehaviour
{
    public float speed = 3f; //karakter hýzý
    public Rigidbody2D rb; //karakterin fiziksel hareketi için gereklidir
    public Animator animator; //karakterin animasyonlarýný(sað,sol,yukarý,aþaðý yürüme anmsyonlarý) kontrol etmek için gerklidir

    Vector2 movement; ///karakter yönünü tutar
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //karakterin üzerindeki bileþenleri scripte tanýtýyoruz
        animator = GetComponent<Animator>();

    }


    void Update()
    {

       // Debug.Log("Input X: " + Input.GetAxisRaw("Horizontal") + " | Y: " + Input.GetAxisRaw("Vertical")); //yatay ve dikey konumu konsola yazdýrýr

        
        movement.x = Input.GetAxisRaw("Horizontal");//yatay giriþleri (a,d veya sað,sol yön tuþlarý) alýr
        movement.y = Input.GetAxisRaw("Vertical");  //dikey giriþleri (w,s veya yukarý,aþaðý yön tuþlarý) alýr

       // Animator'a verileri gönder
        animator.SetFloat("moveX", movement.x); //karakterin baktýðý yöne göre animasyonu oynatýr
        animator.SetFloat("moveY", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude); //hareket þiddeti


    }

    void FixedUpdate()
    {
        // Hareket ettir
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);//karakterin hareketi
        Debug.Log("Karakter pozisyonu: " + rb.position); 

    }
}
