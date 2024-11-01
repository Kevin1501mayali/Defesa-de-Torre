using System; // Importa o namespace para funcionalidades básicas do C#
using UnityEngine; // Importa o namespace do Unity

// Define a classe Tower como serializável, permitindo que suas instâncias sejam exibidas e editadas no editor do Unity
[Serializable]
public class Tower
{
    public string name; // Nome da torre
    public int cost; // Custo da torre em moeda
    public GameObject prefab; // Prefab que representa a torre no jogo

    // Construtor da classe Tower
    public Tower(string _name, int _cost, GameObject _prefab)
    {
        name = _name; // Inicializa o nome da torre
        cost = _cost; // Inicializa o custo da torre
        prefab = _prefab; // Inicializa o prefab da torre
    }
}