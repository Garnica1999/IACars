/// Author: Samuel Arzt
/// Date: March 2017

#region Includes
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#endregion

/// <summary>
/// Singleton class managing the overall simulation.
/// </summary>
public class GameStateManager : MonoBehaviour
{
    #region Members
    // The camera object, to be referenced in Unity Editor.
    [SerializeField]
    private CameraMovement Camera;

    // The name of the track to be loaded
    [SerializeField]
    public string TrackName;

    /// <summary>
    /// The UIController object.
    /// </summary>
    public UIController UIController
    {
        get;
        set;
    }

    public static GameStateManager Instance
    {
        get;
        private set;
    }

    private CarController prevBest, prevSecondBest;
    [SerializeField]
    private GameObject PanelTracks;
    [SerializeField]
    private Canvas CanvasPrincipal;
    [SerializeField]
    private Sprite[] ImagesTracks;
    private int typeIA = -1;
    [SerializeField]
    private Button[] ButtonsTrack;
    private string[] NameTracksFNA = new string[] {"Ovalo","Pista1"};
    private string[] NameTracksQL = new string[] {"QOvalo", "QPista1"};
    [SerializeField]
    private Camera MainCamera;

    public enum TiposIA
    {
        FNA = 0,
        QL = 1
    }

    #endregion

    #region Constructors
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameStateManagers in the Scene.");
            return;
        }
        Instance = this;

        /*//Load gui scene
        SceneManager.LoadScene("GUI", LoadSceneMode.Additive);

        //Load track
        SceneManager.LoadScene(TrackName, LoadSceneMode.Additive);*/
    }

    void Start ()
    {
        /*TrackManager.Instance.BestCarChanged += OnBestCarChanged;
        EvolutionManager.Instance.StartEvolution();*/
	}
    #endregion

    #region Methods
    // Callback method for when the best car has changed.
    private void OnBestCarChanged(CarController bestCar)
    {
        if (bestCar == null)
            Camera.SetTarget(null);
        else
            Camera.SetTarget(bestCar.gameObject);
            
        if (UIController != null)
            UIController.SetDisplayTarget(bestCar);
    }

    private void enablePanelTracks()
    {
        Transform[] childrens = this.PanelTracks.GetComponentsInChildren<Transform>();
        foreach(Transform children in childrens)
        {
            children.gameObject.SetActive(true);
        }
    }

    private void LoadSpritesOnButtons()
    {
        for(int i = 0; i < this.ButtonsTrack.Length; i++)
        {
            Button button = this.ButtonsTrack[i];
            button.GetComponent<Image>().sprite = this.ImagesTracks[i];
            //LOAD TEXT
            if (this.typeIA == (int)GameStateManager.TiposIA.FNA)
            {
                button.GetComponentInChildren<Text>().text = this.NameTracksFNA[i];
            }
            else if(this.typeIA == (int)GameStateManager.TiposIA.QL)
            {
                button.GetComponentInChildren<Text>().text = this.NameTracksQL[i];
            }
        }
    }

    public void OnClickButtonTracks(Button button)
    {
        string pista = button.GetComponentInChildren<Text>().text;
        this.PanelTracks.gameObject.SetActive(false);
        this.CanvasPrincipal.gameObject.SetActive(false);
        SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
        SceneManager.LoadScene(pista, LoadSceneMode.Additive);
        if (this.typeIA == (int)GameStateManager.TiposIA.QL)
        {
            this.MainCamera.gameObject.SetActive(false);
            //this.MainCamera.GetComponent<FXAA>().enabled = false;
        }
        StartCoroutine(this.Sleep(1.0f));
        
    }

    IEnumerator Sleep(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (this.typeIA == (int)GameStateManager.TiposIA.FNA)
        {
            TrackManager.Instance.BestCarChanged += OnBestCarChanged;
            EvolutionManager.Instance.StartEvolution();
        }
    }

    public void OnClicButtonFNA()
    {
        this.typeIA = (int)GameStateManager.TiposIA.FNA;
        this.enablePanelTracks();
        this.LoadSpritesOnButtons();
    }
    public void OnClicButtonQL()
    {
        this.typeIA = (int)GameStateManager.TiposIA.QL;
        this.enablePanelTracks();
        this.LoadSpritesOnButtons();
    }

    #endregion
}
