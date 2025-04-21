using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class GoblinSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> listSpawnPositions;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private GameObject mainDoor;

    [Header("Configuration")]
    [SerializeField] private int numberOfWaves;
    [SerializeField] private int numberOfEnemiesPerWave;
    private int totalNumberOfEnemies;
    [SerializeField] private float secondsBetweenWaves;
    [SerializeField] private float secondsBetweenEnemies;

    [Header("Weapon")]
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject hand;

    [Header("Canvas")]
    [SerializeField] private GameObject zombieCanvas;
    [SerializeField] private TMP_Text waveIndicatorText;


    private GameObject obj;

    private bool HasMinigameEnded = false;
    private Coroutine coroutineEnemy;
    private PlayerBehaviour player;
    private int numberOfEnemiesKilled = 0;

    public int NumberOfEnemiesKilled { get => numberOfEnemiesKilled; set => numberOfEnemiesKilled = value; }

    // Start is called before the first frame update
    private void OnEnable()
    {
        CloseDoor();
        player = FindFirstObjectByType<PlayerBehaviour>();
        player.IsZombieMinigame = true;
        totalNumberOfEnemies = numberOfWaves * numberOfEnemiesPerWave;
        zombieCanvas.SetActive(true);
        waveIndicatorText.text = "Enemy count:\n" + numberOfEnemiesKilled + "/" + totalNumberOfEnemies;
        coroutineEnemy = StartCoroutine(EnemiesWave(secondsBetweenWaves, secondsBetweenEnemies));
        AddWeapon();
    }

    private void OnDisable()
    {
        numberOfEnemiesKilled = 0;
        if (zombieCanvas != null)
        {
            zombieCanvas.SetActive(false);
        }
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
        if (numberOfEnemiesKilled.Equals(totalNumberOfEnemies))
        {
            FindAnyObjectByType<GameManager>().Minigame1_Flag = true;
            waveIndicatorText.text = "Enemy count:\n" + numberOfEnemiesKilled + "/" + totalNumberOfEnemies;
            numberOfEnemiesKilled = 0;
            if (!HasMinigameEnded)
            {
                GetComponent<AudioSource>().Play();
                HasMinigameEnded = true;
            }
            OpenDoor();
        }
        else 
        { 
            waveIndicatorText.text = "Enemy count:\n" + numberOfEnemiesKilled + "/" + totalNumberOfEnemies; 
        }
        
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

    private void OpenDoor()
    {
        mainDoor.transform.GetChild(2).gameObject.SetActive(false);
        mainDoor.transform.GetChild(3).gameObject.SetActive(false);
    }
    private void CloseDoor()
    {
        mainDoor.transform.GetChild(2).gameObject.SetActive(true);
        mainDoor.transform.GetChild(3).gameObject.SetActive(true);
    }
}
