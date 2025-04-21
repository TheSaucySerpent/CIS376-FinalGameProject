using System.Collections;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class Player : MonoBehaviour
{
    public int HP = 100;

    // reference to the bloody screen
    public GameObject bloodyScreen;

    // reference to the Player Health TMP
    public TextMeshProUGUI playerHealthUI;

    // reference to the Player Game Over UI
    public GameObject gameOverUI;

    public bool isDead; // flag to track if the player is dead

  private void Start()
  {
    playerHealthUI.text = $"Health: {HP}";
  }

  public void TakeDamage(int damageAmount)
  {
    // decrease the HP of the enemy by the damage amount
    HP -= damageAmount;

    // check if the player is dead
    if (HP <= 0)
    {
        print("Player Dead");
        PlayerDead();
        isDead = true; // we are now dead

        // play a death sound
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDeath);

        // have a delay before playing the game over music (Horizon from Dying Light)
        SoundManager.Instance.playerChannel.clip = SoundManager.Instance.gameOverMusic;
        // create a 2 second delay on the channel
        SoundManager.Instance.playerChannel.PlayDelayed(2f);
    }
    else
    {
        print("Player Hit");

        // display the bloody screen using a coroutine
        StartCoroutine(BloodyScreenEffect());

        // update the player health UI
        playerHealthUI.text = $"Health: {HP}"; 

        // play a hurt sound
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
    }
  }

  private void PlayerDead()
  {
    // disable player movement and mouse movement scripts
    GetComponent<MouseMovement>().enabled = false;
    GetComponent<PlayerMovement>().enabled = false;

    // play a dying animation via enabling the animator
    GetComponentInChildren<Animator>().enabled = true;

    // disable the player health UI by setting it to inactive
    playerHealthUI.gameObject.SetActive(false);

    // start the fade screen effect
    GetComponent<ScreenFader>().StartFade();

    // show the game over UI
    StartCoroutine(ShowGameOverUI());
  }

  private IEnumerator ShowGameOverUI()
  {
    // wait for 1 second, then show the game over UI
    yield return new WaitForSeconds(1f);
    gameOverUI.SetActive(true);
  }

  private IEnumerator BloodyScreenEffect()
  {
    if (!bloodyScreen.activeInHierarchy)
    {
      bloodyScreen.SetActive(true);
    }

    var image = bloodyScreen.GetComponentInChildren<Image>();
    
    // set the initial alpha value to 1 (fully visible)
    Color startColor = image.color;
    startColor.a = 1f;
    image.color = startColor;

    float duration = 2f;
    float elapsedTime = 0f;

    while (elapsedTime < duration)
    {
      // calculate the new alpha value using Lerp
      float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

      // Update the color with the new alpha value
      Color newColor = image.color;
      newColor.a = alpha;
      image.color = newColor;

      // increment the elapsed time
      elapsedTime += Time.deltaTime;

      // wait for the next frame
      yield return null;
    }

    if (bloodyScreen.activeInHierarchy)
    {
      bloodyScreen.SetActive(false);
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    // check if the ZombieHand hit us
    if (other.CompareTag("ZombieHand"))
    {
      // don't want to keep dying if we're already dead
      if (!isDead) 
      {
        // get the damage amount from the ZombieHand script
        TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
      }
    }
  }
}
