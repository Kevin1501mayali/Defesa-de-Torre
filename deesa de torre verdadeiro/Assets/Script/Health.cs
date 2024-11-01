using System.Collections;
using System.Collections.Generic;
using UnityEngine; // Importa o namespace do Unity

// Define a classe Health que herda de MonoBehaviour
public class Health : MonoBehaviour
{
    [SerializeField] protected float hitPoints = 2; // Pontos de vida do objeto, protegidos para subclasses
    [SerializeField] private int currencyWorth = 50; // Valor em moeda que o objeto gera ao ser destruído

    protected bool isDestroyed = false; // Indica se o objeto já foi destruído

    // Método virtual para receber dano
    public virtual void TakeDamage(float dmg)
    {
        hitPoints -= dmg; // Decrementa os pontos de vida pelo valor de dano recebido

        // Verifica se os pontos de vida caíram para zero ou menos e se o objeto não foi destruído
        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke(); // Chama o evento para indicar que um inimigo foi destruído
            LevelManager.instance.IncreaseCurrency(1); // Aumenta a moeda do jogador em 1
            isDestroyed = true; // Marca o objeto como destruído
            Destroy(gameObject); // Destroi o objeto
        }
    }
}