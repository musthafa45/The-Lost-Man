using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerWallet 
{
    private static int currentWalletAmount = 0;

    public static int GetCurrentCashAmount() => currentWalletAmount;

    public static void AddCashToWallet(int amount) => currentWalletAmount += amount;

    public static bool TryTakeCashFromWallet(int amount)
    {
        if (currentWalletAmount >= amount) // player has Valid Amount
        {
            currentWalletAmount -= amount; // Detuct Cash
            return true;
        }
        else
        {
            return false;
        }
    }
}
