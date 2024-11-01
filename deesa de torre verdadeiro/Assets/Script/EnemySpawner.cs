using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; // Importa o namespace do Visual Scripting, n�o necessariamente usado aqui
using UnityEngine;
using UnityEngine.Events; // Importa o namespace para eventos Unity

// Define a classe EnemySpawner que herda de MonoBehaviour
public class EnemySpawner : MonoBehaviour
{
    // Cabe�alho para organizar refer�ncias no inspetor
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs; // Array de prefabs de inimigos

    // Cabe�alho para atributos do spawner
    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8; // N�mero base de inimigos por onda
    [SerializeField] private float enemiesPerSecond = 0.5f; // Quantidade de inimigos que aparecem por segundo
    [SerializeField] private float timeBetweenWaves = 5f; // Tempo entre ondas de inimigos
    [SerializeField] private float difficultyScalingFactor = 0.75f; // Fator de escalonamento de dificuldade

    [SerializeField] private int numEnemiesPerSpawn = 3; // Quantidade de inimigos spawnados ao mesmo tempo

    // Cabe�alho para eventos
    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent(); // Evento chamado quando um inimigo � destru�do

    private int currentWave = 1; // Contador da onda atual
    private float timeSinceLastSpawn; // Tempo desde o �ltimo spawn
    private int enemiesAlive; // N�mero de inimigos atualmente vivos
    private int enemiesLeftToSpawn; // Inimigos restantes a serem spawnados
    private bool isSpawning = false; // Indica se est� spawnando inimigos

    // M�todo chamado ao iniciar o objeto
    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed); // Adiciona o m�todo EnemyDestroyed ao evento onEnemyDestroy
    }

    // M�todo chamado no in�cio do jogo
    private void Start()
    {
        StartCoroutine(StartWave()); // Inicia a primeira onda
    }

    // M�todo chamado a cada frame
    private void Update()
    {
        if (!isSpawning) return; // Sai se n�o estiver spawnando

        timeSinceLastSpawn += Time.deltaTime; // Atualiza o tempo desde o �ltimo spawn

        // Verifica se � hora de spawnar um novo grupo de inimigos
        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            // Spawna um grupo de inimigos de uma vez
            for (int i = 0; i < numEnemiesPerSpawn && enemiesLeftToSpawn > 0; i++)
            {
                SpawnEnemy(); // Chama o m�todo para spawnar um inimigo
                enemiesLeftToSpawn--; // Decrementa o n�mero de inimigos restantes a serem spawnados
                enemiesAlive++; // Incrementa o n�mero de inimigos vivos
            }
            timeSinceLastSpawn = 0f; // Reseta o tempo desde o �ltimo spawn
        }

        // Verifica se todos os inimigos foram spawnados e derrotados para encerrar a onda
        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave(); // Chama o m�todo para encerrar a onda
        }
    }

    // M�todo para encerrar a onda atual
    private void EndWave()
    {
        isSpawning = false; // Para o spawn de novos inimigos
        timeSinceLastSpawn = 0f; // Reseta o tempo desde o �ltimo spawn
        currentWave++; // Avan�a para a pr�xima onda
        StartCoroutine(StartWave()); // Inicia a pr�xima onda
    }

    // M�todo chamado quando um inimigo � destru�do
    private void EnemyDestroyed()
    {
        enemiesAlive--; // Decrementa o n�mero de inimigos vivos
    }

    // Corrotina para iniciar uma nova onda de inimigos
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves); // Espera o tempo entre ondas
        isSpawning = true; // Habilita o spawn de inimigos
        enemiesLeftToSpawn = EnemiesPerWave(); // Define o n�mero de inimigos a serem spawnados nesta onda
    }

    // M�todo para spawnar um inimigo
    private void SpawnEnemy()
    {
        // Seleciona um inimigo aleat�rio para spawnar
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[randomIndex]; // Seleciona um prefab aleat�rio
        Instantiate(prefabToSpawn, LevelManager.instance.startPoint.position, Quaternion.identity); // Cria o inimigo na posi��o inicial
    }

    // M�todo para calcular o n�mero de inimigos por onda com base na dificuldade
    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor)); // Calcula o n�mero de inimigos com escalonamento
    }
}
