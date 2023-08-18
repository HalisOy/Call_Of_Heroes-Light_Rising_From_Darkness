using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform StartPoint;
    private GameObject Character;
    public GameObject Story;
    void Awake()
    {
        if (StartPoint != null)
        {
            Character = Instantiate(CurrentCharacter.Character.CharacterPrefab, StartPoint.position, Quaternion.identity);
            CinemachineVirtualCamera virtualCamera = GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CinemachineVirtualCamera>();
            virtualCamera.Follow = Character.transform;
            if (Story == null)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                if (StartPoint != null)
                {
                    Character.GetComponent<PlayerMovement>().InputEnable = false;
                    Character.GetComponent<PlayerAttack>().InputEnable = false;
                }
            }
        }
    }

    static public void NextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    static public void HomeScene()
    {
        SceneManager.LoadScene("HomePage");
    }

    public void CursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        Character.GetComponent<PlayerMovement>().InputEnable = true;
        Character.GetComponent<PlayerAttack>().InputEnable = true;
    }


}
