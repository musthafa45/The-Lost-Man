using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerFearSystem playerFearSystem))
        {
            playerFearSystem.SetPlayerFearState(PlayerFearSystem.PlayerFearState.Healing);
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
