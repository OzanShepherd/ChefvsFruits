using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FruitEnemy))]
public class AppleBoss : MonoBehaviour
{
    [Header("Health & Phase Settings")]
    public float phaseTwoThreshold = 6f;

    [Header("Slice Minions (Assign in order in Inspector)")]
    public List<GameObject> sliceMinions;

    private FruitEnemy fruitEnemy;
    private FollowPlayer followPlayer;
    private int sliceIndexToSpawn = 0;
    private bool waitingForMinionsToDie = false;
    private bool inPhaseTwo = false;

    void Start()
    {
        fruitEnemy = GetComponent<FruitEnemy>();
        followPlayer = GetComponent<FollowPlayer>();
    }

    void Update()
    {
        // Mid-phase: Waiting for minions to be defeated
        if (waitingForMinionsToDie && AllSliceMinionsAreDead())
        {
            BeginPhaseTwo();
        }
    }

    public void NotifyDamageTaken(float currentHealth)
    {
        // Phase 1: Spawn slice-minions after each hit above threshold
        if (currentHealth > phaseTwoThreshold)
        {
            ActivateNextSliceMinion();
        }
        // Phase trigger: stop moving and wait for slice fight
        else if (!inPhaseTwo && !waitingForMinionsToDie)
        {
            EnterSliceSummonPhase();
        }
    }

    void ActivateNextSliceMinion()
    {
        if (sliceIndexToSpawn < sliceMinions.Count)
        {
            GameObject minion = sliceMinions[sliceIndexToSpawn];
            if (minion != null)
            {
                minion.SetActive(true);
            }
            sliceIndexToSpawn++;
        }
    }

    void EnterSliceSummonPhase()
    {
        waitingForMinionsToDie = true;
        if (followPlayer != null)
            followPlayer.enabled = false;

        Debug.Log("Apple Boss has entered slice phase — minions active!");
    }

    bool AllSliceMinionsAreDead()
    {
        foreach (GameObject minion in sliceMinions)
        {
            if (minion != null && minion.activeSelf)
                return false;
        }
        return true;
    }

    void BeginPhaseTwo()
    {
        inPhaseTwo = true;
        waitingForMinionsToDie = false;

        Debug.Log("Apple Boss begins Phase 2 — reabsorbing slices!");

        if (followPlayer != null)
            followPlayer.enabled = true;

        // Optional: buff speed, unlock new abilities here
    }
}
