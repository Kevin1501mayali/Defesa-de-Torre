using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define a classe BuildManager que herda de MonoBehaviour
public class BuildManager : MonoBehaviour
{
    // Inst�ncia est�tica da classe BuildManager, usada para acesso global
    public static BuildManager Instance;

    // Cabe�alho para organizar refer�ncias no inspetor
    [Header("References")]

    // Array de torres dispon�veis para constru��o
    [SerializeField] private Tower[] towers;
    // �ndice da torre atualmente selecionada
    private int selectedTower = 0;

    // M�todo chamado quando o script � carregado
    private void Awake()
    {
        // Inicializa a inst�ncia est�tica com a inst�ncia atual do BuildManager
        Instance = this;
    }

    // M�todo para obter a torre atualmente selecionada
    public Tower GetselectedTower()
    {
        // Retorna a torre correspondente ao �ndice selecionado
        return towers[selectedTower];
    }

    // M�todo para definir a torre selecionada
    public void SetSelectedTower(int _selectedtower)
    {
        // Atualiza o �ndice da torre selecionada
        selectedTower = _selectedtower;
    }
}