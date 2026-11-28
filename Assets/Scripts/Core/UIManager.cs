using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.EventSystems.EventSystem eventSystem;
    [SerializeField] private GameObject MainMenuPannel;
    [SerializeField] private Selectable MainMenuPannelToSelect;
    [SerializeField] private GameObject SettingsMenuPannel;
    [SerializeField] private Selectable SettingsMenuPannelToSelect;
    [SerializeField] private GameObject MatchMakingMenuPannel;
    [SerializeField] private Selectable MatchMakingMenuPannelToSelect;
    [SerializeField] private GameObject ScoreScreenPannel;
    [SerializeField] private Selectable ScoreScreenPannelToSelect;
    [SerializeField] private GameObject InGameHUDPannel;
    [SerializeField] private GameObject PauseMenuPannel;
    [SerializeField] private Selectable PauseMenuPannelToSelect;

    private void Awake() {
        MainMenuPannel.SetActive(true);
        SettingsMenuPannel.SetActive(false);
        MatchMakingMenuPannel.SetActive(false);
        ScoreScreenPannel.SetActive(false);
        InGameHUDPannel.SetActive(false);
        PauseMenuPannel.SetActive(false);
    }

    public void ShowMainMenu() {
        MainMenuPannel.SetActive(true);
        eventSystem.SetSelectedGameObject(MainMenuPannelToSelect.gameObject);
        SettingsMenuPannel.SetActive(false);
        MatchMakingMenuPannel.SetActive(false);
        ScoreScreenPannel.SetActive(false);
        InGameHUDPannel.SetActive(false);
        PauseMenuPannel.SetActive(false);
    }

    public void ShowSettingsMenu() {
        MainMenuPannel.SetActive(false);
        SettingsMenuPannel.SetActive(true);
        eventSystem.SetSelectedGameObject(SettingsMenuPannelToSelect.gameObject);
        MatchMakingMenuPannel.SetActive(false);
        ScoreScreenPannel.SetActive(false);
        InGameHUDPannel.SetActive(false);
        PauseMenuPannel.SetActive(false);
    }

    public void ShowMatchMakingMenu() {
        MainMenuPannel.SetActive(false);
        SettingsMenuPannel.SetActive(false);
        MatchMakingMenuPannel.SetActive(true);
        eventSystem.SetSelectedGameObject(MatchMakingMenuPannelToSelect.gameObject);
        ScoreScreenPannel.SetActive(false);
        InGameHUDPannel.SetActive(false);
        PauseMenuPannel.SetActive(false);
    }

    public void ShowScoreScreen() {
        MainMenuPannel.SetActive(false);
        SettingsMenuPannel.SetActive(false);
        MatchMakingMenuPannel.SetActive(false);
        ScoreScreenPannel.SetActive(true);
        eventSystem.SetSelectedGameObject(ScoreScreenPannelToSelect.gameObject);
        InGameHUDPannel.SetActive(false);
        PauseMenuPannel.SetActive(false);
    }

    public void ShowInGameHUD() {
        MainMenuPannel.SetActive(false);
        SettingsMenuPannel.SetActive(false);
        MatchMakingMenuPannel.SetActive(false);
        ScoreScreenPannel.SetActive(false);
        InGameHUDPannel.SetActive(true);
        PauseMenuPannel.SetActive(false);
    }

    public void ShowPauseMenu() {
        MainMenuPannel.SetActive(false);
        SettingsMenuPannel.SetActive(false);
        MatchMakingMenuPannel.SetActive(false);
        ScoreScreenPannel.SetActive(false);
        InGameHUDPannel.SetActive(false);
        eventSystem.SetSelectedGameObject(PauseMenuPannelToSelect.gameObject);
        PauseMenuPannel.SetActive(true);
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}
