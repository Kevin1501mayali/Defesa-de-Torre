using System.Collections;
using System.Collections.Generic;
using UnityEngine; // Importa o namespace do Unity

// Define a classe LevelManager que herda de MonoBehaviour
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; // Instância estática para o padrão Singleton

    public Transform startPoint; // Ponto de início do nível
    public Transform[] path; // Array que contém o caminho que os inimigos seguirão
    public int currency; // Quantidade de moeda disponível para o jogador

    // Método chamado quando o script é carregado
    private void Awake()
    {
        instance = this; // Define a instância estática para permitir acesso global
    }

    // Método chamado no início do jogo
    private void Start()
    {
        currency = 500; // Inicializa a moeda do jogador em 500
    }

    // Método para aumentar a quantidade de moeda
    public void IncreaseCurrency(int amount)
    {
        amount = 50; // Valor fixo que será adicionado à moeda (deveria ser o parâmetro)
        currency += amount; // Aumenta a quantidade de moeda
    }

    // Método para gastar moeda
    public bool SpendCurrency(int amount)
    {
        // Verifica se o jogador tem moeda suficiente
        if (amount <= currency)
        {
            currency -= amount; // Deduz o valor gasto da moeda
            return true; // Retorna verdadeiro se a transação for bem-sucedida
        }
        else
        {
            Debug.Log("You do not have enough to purchase this item"); // Loga uma mensagem de erro
            return false; // Retorna falso se não houver moeda suficiente
        }
    }
}