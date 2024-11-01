using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define a classe Plot que herda de MonoBehaviour
public class Plot : MonoBehaviour
{
    // Refer�ncias do editor para o SpriteRenderer e a cor de destaque
    [Header("References")]
    [SerializeField] private SpriteRenderer sr; // Componente para renderizar o sprite
    [SerializeField] private Color hoverColor; // Cor a ser exibida quando o mouse passa sobre o plot

    // Vari�veis para armazenar a torre constru�da e a cor inicial
    private GameObject tower; // Refer�ncia � torre constru�da
    private Color startColor; // Armazena a cor original do sprite

    // M�todo chamado ao iniciar o script
    private void Start()
    {
        // Armazena a cor inicial do sprite
        startColor = sr.color;
    }

    // M�todo chamado quando o mouse � clicado sobre o plot
    private void OnMouseDown()
    {
        // Se j� houver uma torre, n�o faz nada
        if (tower != null) return;

        // Obt�m a torre que o jogador selecionou para construir
        Tower towertobuild = BuildManager.Instance.GetselectedTower();

        // Verifica se o custo da torre � maior que a moeda dispon�vel
        if (towertobuild.cost > LevelManager.instance.currency)
        {
            // Exibe mensagem de erro no console
            Debug.Log("you cant afford this");
            return; // Sai do m�todo
        }

        // Deduz o custo da torre da moeda do jogador
        LevelManager.instance.SpendCurrency(towertobuild.cost);
        // Instancia a torre na posi��o do plot
        tower = Instantiate(towertobuild.prefab, transform.position, Quaternion.identity);
    }

    // M�todo chamado quando o mouse entra na �rea do plot
    private void OnMouseEnter()
    {
        // Altera a cor do sprite para a cor de destaque
        sr.color = hoverColor;
    }

    // M�todo chamado quando o mouse sai da �rea do plot
    private void OnMouseExit()
    {
        // Restaura a cor original do sprite
        sr.color = startColor;
    }
}