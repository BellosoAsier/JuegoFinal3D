using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GoblinSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> listSpawnPositions;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;

    [Header("Configuration")]
    [SerializeField] private int numberOfWaves;
    [SerializeField] private int numberOfEnemiesPerWave;
    private int totalNumberOfEnemies;
    [SerializeField] private float secondsBetweenWaves;
    [SerializeField] private float secondsBetweenEnemies;

    [Header("Weapon")]
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject hand;

    private GameObject obj;

    private bool HasCombatEnded = false;
    private Coroutine coroutineEnemy;
    private PlayerBehaviour player;
    // Start is called before the first frame update
    private void OnEnable()
    {
        player = FindFirstObjectByType<PlayerBehaviour>();
        player.IsZombieMinigame = true;
        totalNumberOfEnemies = numberOfWaves * numberOfEnemiesPerWave;
        coroutineEnemy = StartCoroutine(EnemiesWave(secondsBetweenWaves, secondsBetweenEnemies));
        AddWeapon();
    }

    private void OnDisable()
    {
        if (coroutineEnemy != null)
        {
            StopAllCoroutines();
        }

        foreach (Transform t in enemyContainer.transform)
        {
            Destroy(t.gameObject);
        }

        player.IsZombieMinigame = false;

        RemoveWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddWeapon()
    {
        obj = Instantiate(weaponPrefab, hand.transform.position, hand.transform.rotation);
        obj.name = "WeaponPlayer";
        obj.transform.SetParent(hand.transform);
        obj.transform.localScale = new Vector3(30f, 30f, 30f);
        obj.transform.localPosition = new Vector3(0.015f, 0f, -0.125f);
        obj.transform.localRotation = Quaternion.Euler(-108f, 0f, 90f);
    }

    private void RemoveWeapon()
    {
        Destroy(obj);
    }

    IEnumerator EnemiesWave(float secondsWave, float secondsEnemy)
    {
        while (!HasCombatEnded)
        {
            for (int i = 0; i < numberOfWaves; i++)
            {
                for (int j = 0; j < numberOfEnemiesPerWave; j++)
                {
                    Transform spawnPosition = listSpawnPositions[Random.Range(0, listSpawnPositions.Count)];
                    GameObject enemy = Instantiate(enemyPrefab, spawnPosition.position, Quaternion.identity);
                    enemy.transform.SetParent(enemyContainer.transform);
                    yield return new WaitForSeconds(secondsEnemy);
                }
                yield return new WaitForSeconds(secondsWave);
            }
        }
    }
}
