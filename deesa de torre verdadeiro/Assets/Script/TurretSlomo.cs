using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// Define a classe TurretSlomo que herda de Turret
public class TurretSlomo : Turret
{
    // Define a taxa de disparo (a��es por segundo)
    [SerializeField] private float aps = 4f;
    // Define o tempo de congelamento dos inimigos
    [SerializeField] private float FreezeTime = 5f;

    // M�todo chamado a cada frame
    private void Update()
    {
        // Rotaciona a torre em dire��o ao alvo
        RotateTowardsTarget();
        // Acumula o tempo at� o pr�ximo disparo
        timeUntilFire += Time.deltaTime;

        // Verifica se � hora de disparar
        if (timeUntilFire >= 1f / aps)
        {
            // Congela os inimigos
            FreezeEnemies();
            // Reseta o contador de tempo at� o pr�ximo disparo
            timeUntilFire = 0f;
        }
    }

    // M�todo para congelar inimigos dentro do alcance
    private void FreezeEnemies()
    {
        // Realiza um CircleCast para detectar inimigos na �rea de alcance
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingrange, (Vector2)transform.position, 0f, enemyMask);

        // Verifica se h� inimigos detectados
        if (hits.Length > 0)
        {
            // Itera por todos os inimigos atingidos
            for (int i = 0; i < hits.Length; i++)
            {
                // Obt�m o hit atual
                RaycastHit2D hit = hits[i];
                // Obt�m o componente EnemyMovement do inimigo
                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                // Obt�m o componente Health do inimigo
                Health enemyHealth = hit.transform.GetComponent<Health>();

                // Se o inimigo tem um componente Health, aplica dano
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(0.1f);
                }

                // Atualiza a velocidade do inimigo para metade
                em.UpdateSpeed(0.5f);
                // Inicia uma coroutine para resetar a velocidade do inimigo ap�s o tempo de congelamento
                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    // Coroutine para restaurar a velocidade do inimigo ap�s o congelamento
    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        // Espera pelo tempo de congelamento
        yield return new WaitForSeconds(FreezeTime);
        // Restaura a velocidade original do inimigo
        em.ResetSpeed();
    }

    // M�todo para rotacionar a torre em dire��o ao alvo
    private void RotateTowardsTarget()
    {
        // Verifica se h� um alvo
        if (target == null)
        {
            return; // Se n�o houver, sai do m�todo
        }

        // Calcula o �ngulo entre a torre e o alvo
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - turretRotationPoint.position.x) * Mathf.Rad2Deg - 90;

        // Cria uma rota��o de destino com base no �ngulo calculado
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        // Rotaciona suavemente a torre em dire��o � rota��o de destino
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationspeed * Time.deltaTime);
    }
}