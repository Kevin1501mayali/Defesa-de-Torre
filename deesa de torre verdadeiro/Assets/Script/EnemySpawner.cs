using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; // Importa o namespace do Visual Scripting, não necessariamente usado aqui
using UnityEngine;
using UnityEngine.Events; // Importa o namespace para eventos Unity

// Define a classe EnemySpawner que herda de MonoBehaviour
public class EnemySpawner : MonoBehaviour
{
    // Cabeçalho para organizar referências no inspetor
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs; // Array de prefabs de inimigos

    // Cabeçalho para atributos do spawner
    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8; // Número base de inimigos por onda
    [SerializeField] private float enemiesPerSecond = 0.5f; // Quantidade de inimigos que aparecem por segundo
    [SerializeField] private float timeBetweenWaves = 5f; // Tempo entre ondas de inimigos
    [SerializeField] private float difficultyScalingFactor = 0.75f; // Fator de escalonamento de dificuldade

    [SerializeField] private int numEnemiesPerSpawn = 3; // Quantidade de inimigos spawnados ao mesmo tempo

    // Cabeçalho para eventos
    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent(); // Evento chamado quando um inimigo é destruído

    private int currentWave = 1; // Contador da onda atual
    private float timeSinceLastSpawn; // Tempo desde o último spawn
    private int enemiesAlive; // Número de inimigos atualmente vivos
    private int enemiesLeftToSpawn; // Inimigos restantes a serem spawnados
    private bool isSpawning = false; // Indica se está spawnando inimigos

    // Método chamado ao iniciar o objeto
    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed); // Adiciona o método EnemyDestroyed ao evento onEnemyDestroy
    }

    // Método chamado no início do jogo
    private void Start()
    {
        StartCoroutine(StartWave()); // Inicia a primeira onda
    }

    // Método chamado a cada frame
    private void Update()
    {
        if (!isSpawning) return; // Sai se não estiver spawnando

        timeSinceLastSpawn += Time.deltaTime; // Atualiza o tempo desde o último spawn

        // Verifica se é hora de spawnar um novo grupo de inimigos
        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            // Spawna um grupo de inimigos de uma vez
            for (int i = 0; i < numEnemiesPerSpawn && enemiesLeftToSpawn > 0; i++)
            {
                SpawnEnemy(); // Chama o método para spawnar um inimigo
                enemiesLeftToSpawn--; // Decrementa o número de inimigos restantes a serem spawnados
                enemiesAlive++; // Incrementa o número de inimigos vivos
            }
            timeSinceLastSpawn = 0f; // Reseta o tempo desde o último spawn
        }

        // Verifica se todos os inimigos foram spawnados e derrotados para encerrar a onda
        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave(); // Chama o método para encerrar a onda
        }
    }

    // Método para encerrar a onda atual
    private void EndWave()
    {
        isSpawning = false; // Para o spawn de novos inimigos
        timeSinceLastSpawn = 0f; // Reseta o tempo desde o último spawn
        currentWave++; // Avança para a próxima onda
        StartCoroutine(StartWave()); // Inicia a próxima onda
    }

    // Método chamado quando um inimigo é destruído
    private void EnemyDestroyed()
    {
        enemiesAlive--; // Decrementa o número de inimigos vivos
    }

    // Corrotina para iniciar uma nova onda de inimigos
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves); // Espera o tempo entre ondas
        isSpawning = true; // Habilita o spawn de inimigos
        enemiesLeftToSpawn = EnemiesPerWave(); // Define o número de inimigos a serem spawnados nesta onda
    }

    // Método para spawnar um inimigo
    private void SpawnEnemy()
    {
        // Seleciona um inimigo aleatório para spawnar
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[randomIndex]; // Seleciona um prefab aleatório
        Instantiate(prefabToSpawn, LevelManager.instance.startPoint.position, Quaternion.identity); // Cria o inimigo na posição inicial
    }

    // Método para calcular o número de inimigos por onda com base na dificuldade
    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor)); // Calcula o número de inimigos com escalonamento
    }
}
