using Unity.Netcode;
using UnityEngine;

public class CustomPlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject swordPrefab,
        sniperPrefab,
        pistolPrefab;

    private void Start()
    {
        if (!NetworkManager.Singleton.IsServer)
            return;

        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
    }

    private void HandleClientConnected(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer)
            return;

        // Só o host spawna jogadores
        string classId = PlayerPrefs.GetString("SelectedClass", "sword");
        GameObject prefabToSpawn = classId switch
        {
            "sword" => swordPrefab,
            "sniper" => sniperPrefab,
            "pistol" => pistolPrefab,
            _ => swordPrefab,
        };

        GameObject instance = Instantiate(
            prefabToSpawn,
            GetSpawnPosition(clientId),
            Quaternion.identity
        );
        instance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }

    private Vector3 GetSpawnPosition(ulong clientId)
    {
        // Pode usar uma lógica mais avançada depois
        return new Vector3(clientId * 2f, 0f, 0f);
    }
}
