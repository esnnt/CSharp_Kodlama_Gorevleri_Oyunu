using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterHareket : MonoBehaviour
{
    public float speed = 3f; //karakter h�z�
    public Rigidbody2D rb; //karakterin fiziksel hareketi i�in gereklidir
    public Animator animator; //karakterin animasyonlar�n�(sa�,sol,yukar�,a�a�� y�r�me anmsyonlar�) kontrol etmek i�in gerklidir

    Vector2 movement; ///karakter y�n�n� tutar
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //karakterin �zerindeki bile�enleri scripte tan�t�yoruz
        animator = GetComponent<Animator>();

    }


    void Update()
    {

       // Debug.Log("Input X: " + Input.GetAxisRaw("Horizontal") + " | Y: " + Input.GetAxisRaw("Vertical")); //yatay ve dikey konumu konsola yazd�r�r

        
        movement.x = Input.GetAxisRaw("Horizontal");//yatay giri�leri (a,d veya sa�,sol y�n tu�lar�) al�r
        movement.y = Input.GetAxisRaw("Vertical");  //dikey giri�leri (w,s veya yukar�,a�a�� y�n tu�lar�) al�r

       // Animator'a verileri g�nder
        animator.SetFloat("moveX", movement.x); //karakterin bakt��� y�ne g�re animasyonu oynat�r
        animator.SetFloat("moveY", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude); //hareket �iddeti


    }

    void FixedUpdate()
    {
        // Hareket ettir
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);//karakterin hareketi
        Debug.Log("Karakter pozisyonu: " + rb.position); 

    }
}
