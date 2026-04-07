using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement; // Necessário para trocar de cena

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Status do Jogo")]
    public int pontos = 0;
    public int vidas = 3;
    public int metaVitoria = 200; // Meta para vencer

    [Header("UI Textos")]
    public TextMeshProUGUI textoPontos;
    public TextMeshProUGUI textoVidas;
    public TextMeshProUGUI textoPontosFinal; // Texto que mostra a pontuação na tela de derrota

    [Header("Telas (Popups)")]
    public GameObject telaVitoria;
    public GameObject telaDerrota;

    [Header("Configurações Player")]
    public GameObject player;
    public Vector2 respawnPos = new Vector2(-4.08f, 0f);

    private int proximaMetaBonus = 100;
    private bool jogoFinalizado = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Garante que as telas comecem escondidas
        if (telaVitoria) telaVitoria.SetActive(false);
        if (telaDerrota) telaDerrota.SetActive(false);
        
        Time.timeScale = 1f; // Reseta a velocidade do tempo
        AtualizarUI();
    }

    public void AdicionarPontos(int valor)
    {
        if (jogoFinalizado) return;

        pontos += valor;
        AtualizarUI();

        // Lógica de Vitória
        if (pontos >= metaVitoria)
        {
            VencerJogo();
        }
        // Lógica de Bônus (Asteroides Lentos)
        else if (pontos >= proximaMetaBonus)
        {
            StartCoroutine(AsteroidesLentos());
            proximaMetaBonus += 100;
        }
    }

    public void PerderVida()
    {
        if (jogoFinalizado) return;

        vidas--;
        AtualizarUI();

        if (vidas <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    void VencerJogo()
    {
        jogoFinalizado = true;
        telaVitoria.SetActive(true);
        PausarJogo();
    }

    void GameOver()
    {
        jogoFinalizado = true;
        if (textoPontosFinal != null)
            textoPontosFinal.text = "Pontuação Final: " + pontos;
            
        telaDerrota.SetActive(true);
        PausarJogo();
    }

    void PausarJogo()
    {
        Time.timeScale = 0f; // Para o movimento do jogo
        // Opcional: Desativar o script de tiro do player aqui
    }

    // Método para o Botão "Voltar para Home" ou "Menu"
    public void IrParaHome(string nomeDaCenaMenu)
    {
        SceneManager.LoadScene(nomeDaCenaMenu);
    }

    // --- Métodos Auxiliares Mantidos ---

    IEnumerator RespawnPlayer()
    {
        player.transform.position = respawnPos;
        PlayerInvencivel inv = player.GetComponent<PlayerInvencivel>();
        if (inv != null) inv.AtivarInvencibilidade();
        yield return null;
    }

    IEnumerator AsteroidesLentos()
    {
        Asteroide.velocidadeGlobal *= 0.5f;
        yield return new WaitForSeconds(5f);
        Asteroide.velocidadeGlobal *= 2f;
    }

    void AtualizarUI()
    {
        if (textoPontos != null) textoPontos.text = "Pontos: " + pontos;
        if (textoVidas != null) textoVidas.text = "Vidas: " + vidas;
    }
}