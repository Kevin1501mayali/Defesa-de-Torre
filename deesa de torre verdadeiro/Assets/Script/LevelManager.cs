using System.Collections;
using System.Collections.Generic;
using UnityEngine; // Importa o namespace do Unity

// Define a classe LevelManager que herda de MonoBehaviour
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; // Inst�ncia est�tica para o padr�o Singleton

    public Transform startPoint; // Ponto de in�cio do n�vel
    public Transform[] path; // Array que cont�m o caminho que os inimigos seguir�o
    public int currency; // Quantidade de moeda dispon�vel para o jogador

    // M�todo chamado quando o script � carregado
    private void Awake()
    {
        instance = this; // Define a inst�ncia est�tica para permitir acesso global
    }

    // M�todo chamado no in�cio do jogo
    private void Start()
    {
        currency = 500; // Inicializa a moeda do jogador em 500
    }

    // M�todo para aumentar a quantidade de moeda
    public void IncreaseCurrency(int amount)
    {
        amount = 50; // Valor fixo que ser� adicionado � moeda (deveria ser o par�metro)
        currency += amount; // Aumenta a quantidade de moeda
    }

    // M�todo para gastar moeda
    public bool SpendCurrency(int amount)
    {
        // Verifica se o jogador tem moeda suficiente
        if (amount <= currency)
        {
            currency -= amount; // Deduz o valor gasto da moeda
            return true; // Retorna verdadeiro se a transa��o for bem-sucedida
        }
        else
        {
            Debug.Log("You do not have enough to purchase this item"); // Loga uma mensagem de erro
            return false; // Retorna falso se n�o houver moeda suficiente
        }
    }
}