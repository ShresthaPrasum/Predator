using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    [SerializeField] private string playerTag = "BombTrigger";
    [SerializeField] private float nextActivationDelay = 15f;
    [SerializeField] private bool activateOnStart = true;

    private readonly List<BoosterChild> boosters = new List<BoosterChild>();
    private readonly List<int> remainingIndices = new List<int>();
    private BoosterChild activeBooster;
    private Coroutine nextActivationRoutine;
    private bool isWaitingForNext;

    private void Start()
    {
        CacheChildren();
        DeactivateAll();

        if (activateOnStart)
        {
            ActivateRandomChild();
        }
    }

    private void CacheChildren()
    {
        boosters.Clear();
        remainingIndices.Clear();

        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            BoosterChild booster = child.GetComponent<BoosterChild>();
            if (booster == null)
            {
                booster = child.gameObject.AddComponent<BoosterChild>();
            }

            booster.Initialize(this);
            boosters.Add(booster);
            remainingIndices.Add(i);
        }
    }

    private void DeactivateAll()
    {
        for (int i = 0; i < boosters.Count; i++)
        {
            boosters[i].SetActive(false);
        }

        activeBooster = null;
        isWaitingForNext = false;
    }

    private void ActivateRandomChild()
    {
        if (remainingIndices.Count == 0)
        {
            return;
        }

        if (activeBooster != null && activeBooster.IsActive)
        {
            return;
        }

        activeBooster = null;

        int pick = Random.Range(0, remainingIndices.Count);
        int index = remainingIndices[pick];
        remainingIndices.RemoveAt(pick);

        activeBooster = boosters[index];
        activeBooster.SetActive(true);
        isWaitingForNext = false;
    }

    private IEnumerator ActivateNextAfterDelay()
    {
        yield return new WaitForSeconds(nextActivationDelay);
        ActivateRandomChild();
    }

    internal void HandleBoosterTouched(BoosterChild booster)
    {
        if (booster == null || booster != activeBooster || isWaitingForNext)
        {
            return;
        }

        booster.MarkConsumed();
        activeBooster = null;

        if (remainingIndices.Count == 0)
        {
            return;
        }

        isWaitingForNext = true;
        if (nextActivationRoutine != null)
        {
            StopCoroutine(nextActivationRoutine);
        }

        nextActivationRoutine = StartCoroutine(ActivateNextAfterDelay());
    }

    internal bool IsPlayer(Collider2D other)
    {
        return other != null && other.CompareTag(playerTag);
    }

    internal bool IsPlayer(Collision2D other)
    {
        return other != null && other.collider != null && other.collider.CompareTag(playerTag);
    }
}

public class BoosterChild : MonoBehaviour
{
    private BoosterManager manager;
    private bool isInitialized;
    private bool isActive;

    public bool IsActive => isActive;

    public void Initialize(BoosterManager owner)
    {
        manager = owner;
        isInitialized = true;
    }

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
        gameObject.SetActive(isActive);
    }

    public void MarkConsumed()
    {
        isActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isInitialized || manager == null)
        {
            return;
        }

        if (manager.IsPlayer(other))
        {
            manager.HandleBoosterTouched(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isInitialized || manager == null)
        {
            return;
        }

        if (manager.IsPlayer(other))
        {
            manager.HandleBoosterTouched(this);
        }
    }
}
