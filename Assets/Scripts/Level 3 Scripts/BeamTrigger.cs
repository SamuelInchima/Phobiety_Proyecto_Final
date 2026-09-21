using UnityEngine;

public class BeamTrigger : MonoBehaviour
{
    [Header("Probabilidad de perder balance")]
    [Range(0f, 1f)]
    public float dropProbability = 0.5f; // 0.5 significa 50% de probabilidad de activarse

    private bool miniGameActivated = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Tiramos los dados: si el valor aleatorio es menor a la probabilidad, se activa
            if (Random.value <= dropProbability)
            {
                BalanceSystem balance = other.GetComponent<BalanceSystem>();
                if (balance != null)
                {
                    balance.StartBalanceMiniGame();
                    miniGameActivated = true;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Al terminar de cruzar el tubo, desactivamos el minijuego (si estaba activo)
        if (other.CompareTag("Player") && miniGameActivated)
        {
            BalanceSystem balance = other.GetComponent<BalanceSystem>();
            if (balance != null)
            {
                balance.StopBalanceMiniGame();
                miniGameActivated = false;
            }
        }
    }
}