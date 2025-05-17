using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionMonitor : MonoBehaviour
{
    private static ConnectionMonitor _instance;

    private void Awake()
    {
        // Singleton persistente entre cenas
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Registra eventos de desconexão

        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnClientDisconnected(ulong clientId)
    {
        // Apenas age se for o client local
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            Debug.LogWarning("[Netcode] Conexão perdida. Retornando ao lobby...");
            SceneManager.LoadScene("LobbyScene");
        }
    }
}
