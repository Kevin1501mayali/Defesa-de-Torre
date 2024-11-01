using System.Collections; // Importa o namespace para uso de coleções
using System.Collections.Generic; // Importa o namespace para coleções genéricas
using UnityEditor; // Importa o namespace para funcionalidades do editor (não necessário em builds)
using UnityEngine; // Importa o namespace do Unity

// Define a classe Turret que herda de MonoBehaviour e implementa a interface Iatacavel
public class Turret : MonoBehaviour, Iatacavel
{
    [SerializeField] public Transform turretRotationPoint; // Ponto de rotação da torre

    [SerializeField] protected float targetingrange = 5f; // Alcance de ataque da torre

    [SerializeField] protected LayerMask enemyMask; // Máscara para detectar inimigos

    [SerializeField] protected GameObject bulletPrefab; // Prefab do projétil

    [SerializeField] protected Transform firingPoint; // Ponto de onde o projétil será disparado

    [SerializeField] public float rotationspeed = 10f; // Velocidade de rotação da torre

    [SerializeField] private float bps = 1f; // Disparos por segundo

    protected Transform target; // Alvo atual da torre

    protected float timeUntilFire; // Tempo até o próximo disparo

    // Método da interface Iatacavel (não implementado aqui)
    public virtual void Atacar()
    {
    }

    // Método para desenhar gizmos no editor para visualizar o alcance da torre
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan; // Define a cor do gizmo
        Handles.DrawWireDisc(transform.position, transform.forward, targetingrange); // Desenha um disco para o alcance
    }

    // Método chamado a cada frame
    private void Update()
    {
        if (target == null) // Se não houver um alvo
        {
            Findtarget(); // Tenta encontrar um alvo
            return;
        }

        RotateTowardsTarget(); // Rotaciona em direção ao alvo

        if (!checktargetisrange()) // Se o alvo não estiver dentro do alcance
        {
            target = null; // Remove o alvo
        }
        else
        {
            timeUntilFire += Time.deltaTime; // Aumenta o tempo até o próximo disparo

            if (timeUntilFire >= 1f / bps) // Verifica se é hora de disparar
            {
                Shoot(); // Dispara um projétil
                timeUntilFire = 0f; // Reseta o temporizador
            }
        }
    }

    // Método para rotacionar a torre em direção ao alvo
    private void RotateTowardsTarget()
    {
        if (target == null) // Se não houver alvo
        {
            Debug.LogWarning("Nenhum alvo detectado para rotacionar."); // Exibe um aviso
            return; // Sai do método
        }

        // Calcula o ângulo necessário para rotacionar em direção ao alvo
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - turretRotationPoint.position.x) * Mathf.Rad2Deg - 90;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle)); // Cria a rotação desejada
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationspeed * Time.deltaTime); // Rotaciona suavemente
    }

    // Método responsável por disparar um projétil
    protected virtual void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity); // Instancia o projétil

        Bullet bulletScript = bulletObj.GetComponent<Bullet>(); // Obtém o script do projétil
        bulletScript.SetTarget(target); // Define o alvo do projétil
    }

    // Verifica se o alvo está dentro do alcance da torre
    private bool checktargetisrange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingrange; // Retorna verdadeiro se o alvo estiver no alcance
    }

    // Método para encontrar um alvo dentro do alcance
    private void Findtarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingrange, (Vector2)transform.position, 0f, enemyMask); // Realiza um círculo para detectar inimigos

        if (hits.Length > 0) // Se algum inimigo for detectado
        {
            target = hits[0].transform; // Define o primeiro inimigo encontrado como alvo
        }
    }
}