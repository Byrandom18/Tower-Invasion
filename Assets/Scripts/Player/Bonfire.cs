using UnityEngine;

public class Bonfire : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Убедитесь, что у игрока есть тег "Player"
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.RestAtBonfire();
                Debug.Log("Игрок отдохнул у костра!");
            }
        }
    }
}