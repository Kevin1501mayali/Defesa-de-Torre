using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// Define a classe TurretSlomo que herda de Turret
public class TurretSlomo : Turret
{
    // Define a taxa de disparo (ações por segundo)
    [SerializeField] private float aps = 4f;
    // Define o tempo de congelamento dos inimigos
    [SerializeField] private float FreezeTime = 5f;

    // Método chamado a cada frame
    private void Update()
    {
        // Rotaciona a torre em direção ao alvo
        RotateTowardsTarget();
        // Acumula o tempo até o próximo disparo
        timeUntilFire += Time.deltaTime;

        // Verifica se é hora de disparar
        if (timeUntilFire >= 1f / aps)
        {
            // Congela os inimigos
            FreezeEnemies();
            // Reseta o contador de tempo até o próximo disparo
            timeUntilFire = 0f;
        }
    }

    // Método para congelar inimigos dentro do alcance
    private void FreezeEnemies()
    {
        // Realiza um CircleCast para detectar inimigos na área de alcance
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingrange, (Vector2)transform.position, 0f, enemyMask);

        // Verifica se há inimigos detectados
        if (hits.Length > 0)
        {
            // Itera por todos os inimigos atingidos
            for (int i = 0; i < hits.Length; i++)
            {
                // Obtém o hit atual
                RaycastHit2D hit = hits[i];
                // Obtém o componente EnemyMovement do inimigo
                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                // Obtém o componente Health do inimigo
                Health enemyHealth = hit.transform.GetComponent<Health>();

                // Se o inimigo tem um componente Health, aplica dano
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(0.1f);
                }

                // Atualiza a velocidade do inimigo para metade
                em.UpdateSpeed(0.5f);
                // Inicia uma coroutine para resetar a velocidade do inimigo após o tempo de congelamento
                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    // Coroutine para restaurar a velocidade do inimigo após o congelamento
    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        // Espera pelo tempo de congelamento
        yield return new WaitForSeconds(FreezeTime);
        // Restaura a velocidade original do inimigo
        em.ResetSpeed();
    }

    // Método para rotacionar a torre em direção ao alvo
    private void RotateTowardsTarget()
    {
        // Verifica se há um alvo
        if (target == null)
        {
            return; // Se não houver, sai do método
        }

        // Calcula o ângulo entre a torre e o alvo
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - turretRotationPoint.position.x) * Mathf.Rad2Deg - 90;

        // Cria uma rotação de destino com base no ângulo calculado
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        // Rotaciona suavemente a torre em direção à rotação de destino
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationspeed * Time.deltaTime);
    }
}