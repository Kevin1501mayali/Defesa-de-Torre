using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define a classe Plot que herda de MonoBehaviour
public class Plot : MonoBehaviour
{
    // Referências do editor para o SpriteRenderer e a cor de destaque
    [Header("References")]
    [SerializeField] private SpriteRenderer sr; // Componente para renderizar o sprite
    [SerializeField] private Color hoverColor; // Cor a ser exibida quando o mouse passa sobre o plot

    // Variáveis para armazenar a torre construída e a cor inicial
    private GameObject tower; // Referência à torre construída
    private Color startColor; // Armazena a cor original do sprite

    // Método chamado ao iniciar o script
    private void Start()
    {
        // Armazena a cor inicial do sprite
        startColor = sr.color;
    }

    // Método chamado quando o mouse é clicado sobre o plot
    private void OnMouseDown()
    {
        // Se já houver uma torre, não faz nada
        if (tower != null) return;

        // Obtém a torre que o jogador selecionou para construir
        Tower towertobuild = BuildManager.Instance.GetselectedTower();

        // Verifica se o custo da torre é maior que a moeda disponível
        if (towertobuild.cost > LevelManager.instance.currency)
        {
            // Exibe mensagem de erro no console
            Debug.Log("you cant afford this");
            return; // Sai do método
        }

        // Deduz o custo da torre da moeda do jogador
        LevelManager.instance.SpendCurrency(towertobuild.cost);
        // Instancia a torre na posição do plot
        tower = Instantiate(towertobuild.prefab, transform.position, Quaternion.identity);
    }

    // Método chamado quando o mouse entra na área do plot
    private void OnMouseEnter()
    {
        // Altera a cor do sprite para a cor de destaque
        sr.color = hoverColor;
    }

    // Método chamado quando o mouse sai da área do plot
    private void OnMouseExit()
    {
        // Restaura a cor original do sprite
        sr.color = startColor;
    }
}