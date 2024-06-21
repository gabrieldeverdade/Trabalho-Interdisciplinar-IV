
using System.Linq;
using UnityEngine;

public class HelpMenu : MonoBehaviour
{
   public GameObject helpMenu;
   public bool isPaused;

   void Start(){
      helpMenu.SetActive(false);
   }

	void Update(){
      if (Input.GetKeyDown(KeyCode.H)){
         if(isPaused){
            ResumeGame();
         }
         else{
            PauseGame();
         }
      }
   }

   public void PauseGame(){
      helpMenu.SetActive(true);
      Time.timeScale = 0f;
      isPaused = true;
   }
   public void ResumeGame(){
      helpMenu.SetActive(false);
      Time.timeScale = 1f;
      isPaused = false;
   }
}
