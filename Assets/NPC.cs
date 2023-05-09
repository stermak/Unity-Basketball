using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NPC : MonoBehaviour {
    public Text textUI;

    private void Start() {
        // Скрыть текстовый интерфейс при запуске игры
        textUI.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && player.IsBallInHands) {
                // Показать текстовый интерфейс игроку
                textUI.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            // Скрыть текстовый интерфейс игроку
            textUI.gameObject.SetActive(false);
        }
    }
}
