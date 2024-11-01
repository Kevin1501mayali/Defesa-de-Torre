using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define a classe Bullet que herda de MonoBehaviour
public class Bullet : MonoBehaviour
{
    // Cabe�alho para organizar refer�ncias no inspetor
    [Header("References")]
    [SerializeField] private Rigidbody2D rb; // Componente Rigidbody2D para a f�sica da bala

    // Cabe�alho para atributos da bala
    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f; // Velocidade da bala
    [SerializeField] private int bulletdamage = 1; // Dano que a bala causa

    // Vari�vel para armazenar o alvo da bala
    private Transform target;

    // M�todo para definir o alvo da bala
    public void SetTarget(Transform _target)
    {
        target = _target; // Atualiza o alvo com o valor passado
    }

    // M�todo chamado a cada atualiza��o de f�sica
    private void FixedUpdate()
    {
        // Verifica se h� um alvo definido
        if (!target) return; // Se n�o houver alvo, sai do m�todo

        // Calcula a dire��o em que a bala deve se mover
        Vector2 direction = (target.position - transform.position).normalized;

        // Atualiza a velocidade do Rigidbody2D da bala na dire��o do alvo
        rb.velocity = direction * bulletSpeed;
    }

    // M�todo chamado quando a bala colide com outro objeto
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Aplica dano ao componente Health do objeto colidido
        other.gameObject.GetComponent<Health>().TakeDamage(bulletdamage);
        // Destr�i a pr�pria bala ap�s a colis�o
        Destroy(gameObject);
    }
}