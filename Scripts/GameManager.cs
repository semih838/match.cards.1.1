using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clipCardUp;
    public AudioClip clipCardDown;
    public AudioClip clipCardMatch;

    public GameObject[] allCards;
    public List<Vector3> allPositions = new List<Vector3>();

    private MemoryCard firstSelectedCard;
    private MemoryCard secondSelectedCard;

    private bool canClick = true;

    private int successfulMatches = 0;

    public int currentLevelIndex = 0;

    private void Awake()
    {
        // Get all card positions and save in list
        foreach(GameObject card in allCards)
        {
            allPositions.Add(card.transform.position);
        }

        // Randomize positions 
        System.Random randomNumber = new System.Random();
        allPositions = allPositions.OrderBy(position => randomNumber.Next()).ToList();

        // Assign new positions
        for (int i = 0; i < allCards.Length; i++)
        {
            allCards[i].transform.position = allPositions[i];
        }
    }

    public void CardClicked(MemoryCard card)
    {
        if (canClick == false || card == firstSelectedCard  ) return;

        // Always rotate card up to show its image
        card.targetRotation = 90;
        card.targetHeight = 0.05f;
        audioSource.PlayOneShot(clipCardUp);


        if (firstSelectedCard == null)
        {
            firstSelectedCard = card;
        }
        else
        {
            // Second card clicked
            secondSelectedCard = card;
            canClick = false;

            Invoke("CheckMatch", 1);
        }
    }

    public void CheckMatch()
    {
        // Result
        if (firstSelectedCard.identifier == secondSelectedCard.identifier)
        {
            Destroy(firstSelectedCard.gameObject);
            Destroy(secondSelectedCard.gameObject);

            audioSource.PlayOneShot(clipCardMatch);

            successfulMatches++; // Increment the count of successful matches

            if (successfulMatches == 8)
            {
                // Code to go to the next scene when 8 successful matches are reached
                // Example: SceneManager.LoadScene("NextScene");
                LoadNextLevel();
            }
        }
        else
        {
            firstSelectedCard.targetRotation = -90;
            secondSelectedCard.targetRotation = -90;

            firstSelectedCard.targetHeight = 0.01f;
            secondSelectedCard.targetHeight = 0.01f;

            audioSource.PlayOneShot(clipCardDown);

        }

        // Reset
        firstSelectedCard = null;
        secondSelectedCard = null;

        canClick = true;
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        SceneManager.LoadScene(currentLevelIndex);
    }
}
