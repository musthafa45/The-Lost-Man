using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearSystemPoint : MonoBehaviour
{
    [SerializeField] private PlayerFearSystem.FearLevel fearIntensity = PlayerFearSystem.FearLevel.Medium;

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out PlayerFearSystem playerFearSystem))
        {
            if (playerFearSystem.GetPlayerFearState() == PlayerFearSystem.PlayerFearState.Healing) return;
            
            playerFearSystem.SetPlayerFearState(PlayerFearSystem.PlayerFearState.OnFear,fearIntensity);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerFearSystem playerFearSystem))
        {
            playerFearSystem.SetPlayerFearState(PlayerFearSystem.PlayerFearState.Idle);
        }
    }
}
