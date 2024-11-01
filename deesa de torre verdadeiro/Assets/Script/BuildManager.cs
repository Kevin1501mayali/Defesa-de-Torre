using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define a classe BuildManager que herda de MonoBehaviour
public class BuildManager : MonoBehaviour
{
    // Instância estática da classe BuildManager, usada para acesso global
    public static BuildManager Instance;

    // Cabeçalho para organizar referências no inspetor
    [Header("References")]

    // Array de torres disponíveis para construção
    [SerializeField] private Tower[] towers;
    // Índice da torre atualmente selecionada
    private int selectedTower = 0;

    // Método chamado quando o script é carregado
    private void Awake()
    {
        // Inicializa a instância estática com a instância atual do BuildManager
        Instance = this;
    }

    // Método para obter a torre atualmente selecionada
    public Tower GetselectedTower()
    {
        // Retorna a torre correspondente ao índice selecionado
        return towers[selectedTower];
    }

    // Método para definir a torre selecionada
    public void SetSelectedTower(int _selectedtower)
    {
        // Atualiza o índice da torre selecionada
        selectedTower = _selectedtower;
    }
}