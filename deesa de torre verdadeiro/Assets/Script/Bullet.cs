using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define a classe Bullet que herda de MonoBehaviour
public class Bullet : MonoBehaviour
{
    // Cabeçalho para organizar referências no inspetor
    [Header("References")]
    [SerializeField] private Rigidbody2D rb; // Componente Rigidbody2D para a física da bala

    // Cabeçalho para atributos da bala
    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f; // Velocidade da bala
    [SerializeField] private int bulletdamage = 1; // Dano que a bala causa

    // Variável para armazenar o alvo da bala
    private Transform target;

    // Método para definir o alvo da bala
    public void SetTarget(Transform _target)
    {
        target = _target; // Atualiza o alvo com o valor passado
    }

    // Método chamado a cada atualização de física
    private void FixedUpdate()
    {
        // Verifica se há um alvo definido
        if (!target) return; // Se não houver alvo, sai do método

        // Calcula a direção em que a bala deve se mover
        Vector2 direction = (target.position - transform.position).normalized;

        // Atualiza a velocidade do Rigidbody2D da bala na direção do alvo
        rb.velocity = direction * bulletSpeed;
    }

    // Método chamado quando a bala colide com outro objeto
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Aplica dano ao componente Health do objeto colidido
        other.gameObject.GetComponent<Health>().TakeDamage(bulletdamage);
        // Destrói a própria bala após a colisão
        Destroy(gameObject);
    }
}