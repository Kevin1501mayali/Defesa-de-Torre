using System.Collections; // Importa o namespace para uso de coleções
using UnityEngine; // Importa o namespace do Unity

// Define a classe RegeneratingEnemy que herda de Health
public class RegeneratingEnemy : Health
{
    [SerializeField] private float regenerationRate = 1f; // Taxa de regeneração de saúde por segundo
    [SerializeField] private float maxHitPoints = 5f; // Máximo de pontos de vida que o inimigo pode ter

    // Método chamado no início do jogo
    private void Start()
    {
        hitPoints = maxHitPoints; // Inicializa os pontos de vida com o máximo
        StartCoroutine(RegenerateHealth()); // Inicia a coroutine para regeneração de saúde
    }

    // Coroutine para regenerar saúde
    private IEnumerator RegenerateHealth()
    {
        // Loop que continua enquanto o inimigo não estiver destruído
        while (!isDestroyed)
        {
            // Verifica se os pontos de vida estão abaixo do máximo
            if (hitPoints < maxHitPoints)
            {
                // Regenera saúde
                hitPoints += regenerationRate * Time.deltaTime;
                // Garante que os pontos de vida não ultrapassem o máximo
                hitPoints = Mathf.Min(hitPoints, maxHitPoints);
            }
            yield return null; // Espera um quadro antes de continuar o loop
        }
    }

    // Sobrescreve o método TakeDamage da classe base
    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg); // Chama o método da classe base para processar o dano
    }
}