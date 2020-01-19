using System.Collections;
using UnityEngine;

public class Boundry : MonoBehaviour
{
    [SerializeField] private WallGenerator wallGenerator;
    [SerializeField] private PlayingSceneManager playingSceneManager;
    [SerializeField] private PlayingSceneCanves playingSceneCanves;
    private AudioManager audioManager;

    private void OnTriggerExit(Collider collision)
    {
        //print(collision);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))//if wall get out the boundries
        {
            GameObject wall = collision.gameObject;
            wall.SetActive(false);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayLoseSound();
            //collision.gameObject.SetActive(false);//if it was ball hide it
            collision.transform.position = new Vector3(0, 0, 100);//hide the ball from the playing scene
            collision.GetComponent<Rigidbody>().velocity = Vector3.zero;
            playingSceneCanves.ShowAdMenu();
            PlayingSceneManager.playerDied = true;
            playingSceneManager.ShowLoseFlash();//show flash light
            playingSceneManager.PauseTheGame(1.5f);//pause the game after 1 sec
        }
    }

    public IEnumerator HideWall(GameObject wall)
    {
        yield return new WaitForSeconds(3);//hide after 5 seconds
        wall.SetActive(false);
    }

    public void PlayLoseSound()//play wall hit sound
    {
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        audioManager.PlayLoseAudio();
    }
}
