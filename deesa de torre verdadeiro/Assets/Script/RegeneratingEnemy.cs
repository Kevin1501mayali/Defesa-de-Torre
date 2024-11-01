using System.Collections; // Importa o namespace para uso de cole��es
using UnityEngine; // Importa o namespace do Unity

// Define a classe RegeneratingEnemy que herda de Health
public class RegeneratingEnemy : Health
{
    [SerializeField] private float regenerationRate = 1f; // Taxa de regenera��o de sa�de por segundo
    [SerializeField] private float maxHitPoints = 5f; // M�ximo de pontos de vida que o inimigo pode ter

    // M�todo chamado no in�cio do jogo
    private void Start()
    {
        hitPoints = maxHitPoints; // Inicializa os pontos de vida com o m�ximo
        StartCoroutine(RegenerateHealth()); // Inicia a coroutine para regenera��o de sa�de
    }

    // Coroutine para regenerar sa�de
    private IEnumerator RegenerateHealth()
    {
        // Loop que continua enquanto o inimigo n�o estiver destru�do
        while (!isDestroyed)
        {
            // Verifica se os pontos de vida est�o abaixo do m�ximo
            if (hitPoints < maxHitPoints)
            {
                // Regenera sa�de
                hitPoints += regenerationRate * Time.deltaTime;
                // Garante que os pontos de vida n�o ultrapassem o m�ximo
                hitPoints = Mathf.Min(hitPoints, maxHitPoints);
            }
            yield return null; // Espera um quadro antes de continuar o loop
        }
    }

    // Sobrescreve o m�todo TakeDamage da classe base
    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg); // Chama o m�todo da classe base para processar o dano
    }
}