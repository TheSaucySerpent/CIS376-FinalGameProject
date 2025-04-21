using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieSpawnController : MonoBehaviour
{
    public int initialZombiesPerWave = 5; // initial zombies per wave
    public int currentZombiesPerWave; // the current zombies per wave (increases each round)

    public float spawnDelay = 0.5f; // delay between spawning each zombie in the wave

    public int currentWave = 1; // the current wave we are on (start with 1 for ui)
    public float waveCooldown = 10.0f; // time in seconds between waves

    public bool inCooldown;
    public float cooldownCounter = 0; // only used for testing and the UI

    // a list of all the currently alive zombies (to know when wave is over and how many zombies are left)
    public List<Enemy> currentZombiesAlive;

    // reference to the Zombie prefab
    public GameObject zombiePrefab;


    // reference to the UI text meshes
    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI cooldownCounterUI;
    public TextMeshProUGUI currentWaveUI;

  private void Start()
  {
    currentZombiesPerWave = initialZombiesPerWave; // set the initial zombies per wave
    currentWaveUI.text = currentWave.ToString(); // set the current wave to 1 for the UI
    StartNextWave(); // start the first wave
  }

  private void StartNextWave()
  {
    currentZombiesAlive.Clear(); // clear the list of zombies (since we are starting a new wave)
    currentWave++; // increase the current wave
    currentWaveUI.text = currentWave.ToString(); // update the UI text
    StartCoroutine(SpawnWave()); // start spawning the zombies
  }

  private IEnumerator SpawnWave()
  {
    for (int i=0; i<currentZombiesPerWave; i++)
    {
      // generate a random offset from the spawner
      Vector3 spawnOffset = new Vector3(Random.Range(-1.0f, 1.0f), 0f, Random.Range(-1.0f, 1.0f));
      Vector3 spawnPosition = transform.position + spawnOffset; // apply the offset to the spawner's position

      // instantiate the zombie prefab at the spawn position
      var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

      // get the Enemy script
      Enemy enemyScript = zombie.GetComponent<Enemy>();

      // track this zombie by adding it to the list of zombies
      currentZombiesAlive.Add(enemyScript);

      yield return new WaitForSeconds(spawnDelay); // wait for the spawn delay before spawning the next zombie
    }
  }

  private void Update()
  {
    // get all the dead zombies
    List<Enemy> zombiesToRemove = new List<Enemy>();
    foreach (Enemy zombie in currentZombiesAlive)
    {
        if (zombie.isDead)
        {
            // cannot change a list while iterating over it, so we do this instead
            zombiesToRemove.Add(zombie);
        }
    }

    // remove all the dead zombies
    foreach (Enemy zombie in zombiesToRemove)
    {
        currentZombiesAlive.Remove(zombie);
    }

    // clear the list of zombies to remove
    zombiesToRemove.Clear();

    // Start cooldown if all zombies are dead
    if (currentZombiesAlive.Count == 0 && inCooldown == false)
    {
        // cooldown for the next wave
        StartCoroutine(WaveCooldownRoutine());
    }

    // run the cooldown counter
    if (inCooldown)
    {
        cooldownCounter -= Time.deltaTime;
    }
    else
    {
        // reset the cooldown counter
        cooldownCounter = waveCooldown;
    }
    cooldownCounterUI.text = cooldownCounter.ToString("F0"); // F0 = round to nearest whole number
  }

  private IEnumerator WaveCooldownRoutine()
  {
    // we are in cooldown
    inCooldown = true;
    waveOverUI.gameObject.SetActive(true); // show the wave over UI
    cooldownCounterUI.gameObject.SetActive(true);

    // create the delay
    yield return new WaitForSeconds(waveCooldown);

    // we are no longer in cooldown
    inCooldown = false;
    waveOverUI.gameObject.SetActive(false); // disable show the wave over UI
    cooldownCounterUI.gameObject.SetActive(false);

    currentZombiesPerWave *= 2; // increase the zombies per wave (double each wave)

    StartNextWave(); // start the next wave
  }
}
