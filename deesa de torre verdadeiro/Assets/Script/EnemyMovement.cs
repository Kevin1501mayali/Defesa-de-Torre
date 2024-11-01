using System.Collections;
using System.Collections.Generic;
using UnityEditor; // Importa o namespace do editor Unity, geralmente n�o necess�rio em scripts de runtime
using UnityEngine;

// Define a classe EnemyMovement que herda de MonoBehaviour
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb; // Componente Rigidbody2D para a f�sica do inimigo

    [SerializeField] private float moveSpeed = 2f; // Velocidade de movimento do inimigo

    private Transform target; // Alvo que o inimigo deve seguir

    private int pathIndex = 0; // �ndice do caminho que o inimigo est� seguindo

    private float baseSpeed; // Velocidade base do inimigo

    // M�todo chamado ao iniciar o objeto
    private void Start()
    {
        baseSpeed = moveSpeed; // Armazena a velocidade inicial como base
        target = LevelManager.instance.path[pathIndex]; // Define o primeiro alvo como o primeiro ponto do caminho
    }

    // M�todo chamado a cada frame
    private void Update()
    {
        // Verifica se o inimigo chegou perto do alvo
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++; // Avan�a para o pr�ximo ponto do caminho

            // Verifica se chegou ao final do caminho
            if (pathIndex == LevelManager.instance.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke(); // Invoca evento para indicar que o inimigo foi destru�do
                Destroy(gameObject); // Destroi o objeto inimigo
                return; // Sai do m�todo
            }
            else
            {
                target = LevelManager.instance.path[pathIndex]; // Atualiza o alvo para o pr�ximo ponto do caminho
            }
        }
    }

    // M�todo chamado em intervalos fixos para atualizar a f�sica
    private void FixedUpdate()
    {
        // Calcula a dire��o do movimento em rela��o ao alvo
        Vector2 direction = (target.position - transform.position).normalized;

        // Atualiza a velocidade do Rigidbody2D na dire��o do alvo
        rb.velocity = direction * moveSpeed;
    }

    // M�todo para atualizar a velocidade do inimigo
    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed; // Define a nova velocidade
    }

    // M�todo para resetar a velocidade do inimigo � base
    public void ResetSpeed()
    {
        moveSpeed = baseSpeed; // Restaura a velocidade base
    }
}