using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TMP_InputField roomAddressInput;
    public Button createRoomButton;
    public Button joinRoomButton;
    public TMP_Text statusText;

    void Start()
    {
        createRoomButton.onClick.AddListener(CreateRoom);
        joinRoomButton.onClick.AddListener(JoinRoom);
        roomAddressInput.text = "127.0.0.1";
        NetworkManager.Singleton.ConnectionApprovalCallback += ConnectionApproval;
    }

    private void ConnectionApproval(
        NetworkManager.ConnectionApprovalRequest request,
        NetworkManager.ConnectionApprovalResponse response
    )
    {
        // Limite: 2 jogadores no total (Host + 1 Cliente)
        if (NetworkManager.Singleton.ConnectedClientsList.Count >= 2)
        {
            Debug.Log("[Netcode] Nova conexão recusada: limite de 2 jogadores atingido.");
            response.Approved = false;
            response.CreatePlayerObject = false;
            return;
        }

        Debug.Log("[Netcode] Conexão aprovada.");
        response.Approved = true;
        response.CreatePlayerObject = true;
        response.PlayerPrefabHash = null;
        response.Position = null;
        response.Rotation = null;
    }

    bool SetPlayerName()
    {
        string name = playerNameInput.text;

        Debug.Log($"Nome do jogador: {name}");
        if (string.IsNullOrEmpty(name) || name.Length < 3)
        {
            statusText.text = "Nome inválido. O nome deve ter pelo menos 3 caracteres.";
            return false;
        }

        PlayerPrefs.SetString("PlayerName", name);

        // Salva o nome do jogador nas preferências do jogador
        PlayerPrefs.Save();
        Debug.Log($"Nome do jogador salvo: {name}");
        return true;
    }

    void OnDestroy()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback -= ConnectionApproval;
    }

    void CreateRoom()
    {
        if (!SetPlayerName())
            return;

        statusText.text = "Iniciando host...";

        // Faz NetworkManager persistir entre cenas
        DontDestroyOnLoad(NetworkManager.Singleton.gameObject);

        // Inicia como host
        bool success = NetworkManager.Singleton.StartHost();
        if (success)
        {
            statusText.text = "Host iniciado!";
            SceneManager.LoadScene("Main");
        }
        else
        {
            statusText.text = "Falha ao iniciar como host.";
        }
    }

    void JoinRoom()
    {
        if (!SetPlayerName())
            return;

        string roomAddress = roomAddressInput.text;
        if (string.IsNullOrEmpty(roomAddress))
        {
            statusText.text = "Endereço da sala inválido.";
            return;
        }

        statusText.text = $"Conectando para {roomAddress}...";

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData(roomAddress, 7777); // IP e porta

        DontDestroyOnLoad(NetworkManager.Singleton.gameObject);

        if (NetworkManager.Singleton.StartClient())
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            statusText.text = "Falha ao iniciar como cliente.";
        }
    }
}
