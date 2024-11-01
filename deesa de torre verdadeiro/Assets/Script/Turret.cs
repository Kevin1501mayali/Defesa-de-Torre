using System.Collections; // Importa o namespace para uso de cole��es
using System.Collections.Generic; // Importa o namespace para cole��es gen�ricas
using UnityEditor; // Importa o namespace para funcionalidades do editor (n�o necess�rio em builds)
using UnityEngine; // Importa o namespace do Unity

// Define a classe Turret que herda de MonoBehaviour e implementa a interface Iatacavel
public class Turret : MonoBehaviour, Iatacavel
{
    [SerializeField] public Transform turretRotationPoint; // Ponto de rota��o da torre

    [SerializeField] protected float targetingrange = 5f; // Alcance de ataque da torre

    [SerializeField] protected LayerMask enemyMask; // M�scara para detectar inimigos

    [SerializeField] protected GameObject bulletPrefab; // Prefab do proj�til

    [SerializeField] protected Transform firingPoint; // Ponto de onde o proj�til ser� disparado

    [SerializeField] public float rotationspeed = 10f; // Velocidade de rota��o da torre

    [SerializeField] private float bps = 1f; // Disparos por segundo

    protected Transform target; // Alvo atual da torre

    protected float timeUntilFire; // Tempo at� o pr�ximo disparo

    // M�todo da interface Iatacavel (n�o implementado aqui)
    public virtual void Atacar()
    {
    }

    // M�todo para desenhar gizmos no editor para visualizar o alcance da torre
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan; // Define a cor do gizmo
        Handles.DrawWireDisc(transform.position, transform.forward, targetingrange); // Desenha um disco para o alcance
    }

    // M�todo chamado a cada frame
    private void Update()
    {
        if (target == null) // Se n�o houver um alvo
        {
            Findtarget(); // Tenta encontrar um alvo
            return;
        }

        RotateTowardsTarget(); // Rotaciona em dire��o ao alvo

        if (!checktargetisrange()) // Se o alvo n�o estiver dentro do alcance
        {
            target = null; // Remove o alvo
        }
        else
        {
            timeUntilFire += Time.deltaTime; // Aumenta o tempo at� o pr�ximo disparo

            if (timeUntilFire >= 1f / bps) // Verifica se � hora de disparar
            {
                Shoot(); // Dispara um proj�til
                timeUntilFire = 0f; // Reseta o temporizador
            }
        }
    }

    // M�todo para rotacionar a torre em dire��o ao alvo
    private void RotateTowardsTarget()
    {
        if (target == null) // Se n�o houver alvo
        {
            Debug.LogWarning("Nenhum alvo detectado para rotacionar."); // Exibe um aviso
            return; // Sai do m�todo
        }

        // Calcula o �ngulo necess�rio para rotacionar em dire��o ao alvo
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - turretRotationPoint.position.x) * Mathf.Rad2Deg - 90;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle)); // Cria a rota��o desejada
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationspeed * Time.deltaTime); // Rotaciona suavemente
    }

    // M�todo respons�vel por disparar um proj�til
    protected virtual void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity); // Instancia o proj�til

        Bullet bulletScript = bulletObj.GetComponent<Bullet>(); // Obt�m o script do proj�til
        bulletScript.SetTarget(target); // Define o alvo do proj�til
    }

    // Verifica se o alvo est� dentro do alcance da torre
    private bool checktargetisrange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingrange; // Retorna verdadeiro se o alvo estiver no alcance
    }

    // M�todo para encontrar um alvo dentro do alcance
    private void Findtarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingrange, (Vector2)transform.position, 0f, enemyMask); // Realiza um c�rculo para detectar inimigos

        if (hits.Length > 0) // Se algum inimigo for detectado
        {
            target = hits[0].transform; // Define o primeiro inimigo encontrado como alvo
        }
    }
}