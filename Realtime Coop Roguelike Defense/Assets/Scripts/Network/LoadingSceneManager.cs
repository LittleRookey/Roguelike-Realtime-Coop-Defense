using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName : byte
{
    BootStrap,
    Menu,
    Main,
    //.
    //.
    // Add more scenes states if needed
};

public class LoadingSceneManager : SingletonPersistent<LoadingSceneManager>
{
    public SceneName SceneActive => m_sceneActive;

    private SceneName m_sceneActive;


    // After running the menu scene, which initiates this manager, we subscribe to these events
    // due to the fact that when a network session ends it cannot longer listen to them.
    public void Init()
    {
        NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnLoadComplete;
        NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;
    }

    public virtual IEnumerator Start()
    {
        // -- To test with latency on development builds --
        // To set the latency, jitter and packet-loss percentage values for develop builds we need
        // the following code to execute before NetworkManager attempts to connect (changing the
        // values of the parameters as desired).
        //
        // If you'd like to test without the simulated latency, just set all parameters below to zero(0).
        //
        // More information here:
        // https://docs-multiplayer.unity3d.com/docs/tutorials/testing/testing_with_artificial_conditions#debug-builds
#if DEVELOPMENT_BUILD && !UNITY_EDITOR
        NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().
            SetDebugSimulatorParameters(
                packetDelay: 50,
                packetJitter: 5,
                dropRate: 3);
#endif

        //ClearAllCharacterData();

        // Wait for the network Scene Manager to start
        yield return new WaitUntil(() => NetworkManager.Singleton.SceneManager != null);

        // Set the events on the loading manager
        // Doing this because every time the network session ends the loading manager stops
        // detecting the events
        Init();
    }

    //private void OnDestroy()
    //{
    //    NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnLoadComplete;
    //    //SceneManager.sceneLoaded -= OnSceneLoaded;
        
    //}

    //private void OnEnable()
    //{
    //    NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;
    //}

    //private void OnDisable()
    //{
    //    NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnLoadComplete;
    //}

    public void LoadScene(SceneName sceneToLoad, bool isNetworkSessionActive = true)
    {
        StartCoroutine(Loading(sceneToLoad, isNetworkSessionActive));
    }

    // Coroutine for the loading effect. It use an alpha in out effect
    private IEnumerator Loading(SceneName sceneToLoad, bool isNetworkSessionActive)
    {
        LoadingFadeEffect.Instance.FadeIn();

        // Here the player still sees the black screen
        yield return new WaitUntil(() => LoadingFadeEffect.s_canLoad);

        if (isNetworkSessionActive)
        {
            if (NetworkManager.Singleton.IsServer)
                LoadSceneNetwork(sceneToLoad);
        }
        else
        {
            LoadSceneLocal(sceneToLoad);
        }

        // Because the scenes are not heavy we can just wait a second and continue with the fade.
        // In case the scene is heavy instead we should use additive loading to wait for the
        // scene to load before we continue
        yield return new WaitForSeconds(1f);

        LoadingFadeEffect.Instance.FadeOut();
    }

    // Load the scene using the regular SceneManager, use this if there's no active network session
    private void LoadSceneLocal(SceneName sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad.ToString());
        switch (sceneToLoad)
        {
            case SceneName.Menu:
                //if (AudioManager.Instance != null)
                //    AudioManager.Instance.PlayMusic(AudioManager.MusicName.intro);
                break;
        }
    }

    // Load the scene using the SceneManager from NetworkManager. Use this when there is an active
    // network session
    private void LoadSceneNetwork(SceneName sceneToLoad)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(
            sceneToLoad.ToString(),
            LoadSceneMode.Single);
    }

    // This callback function gets triggered when a scene is finished loading
    // Here we set up what to do for each scene, like changing the music
    private void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        // We only care the host/server is loading because every manager handles
        // their information and behavior on the server runtime
        if (!NetworkManager.Singleton.IsServer)
            return;

        Enum.TryParse(sceneName, out m_sceneActive);
        Debug.Log(m_sceneActive.ToString());
        if (!ClientConnection.Instance.CanClientConnect(clientId))
            return;

        // What to initially do on every scene when it finishes loading
        switch (m_sceneActive)
        {
            // When a client/host connects tell the manager
            case SceneName.Menu:
                Debug.Log("Moved to Menu");
                //CharacterSelectionManager.Instance.ServerSceneInit(clientId);
                break;

            // When a client/host connects tell the manager to create the ship and change the music
            case SceneName.Main:
                Debug.Log("Moved to Main");
                //GameplayManager.Instance.ServerSceneInit(clientId);
                break;

            // When a client/host connects tell the manager to create the player score ships and
            //// play the right SFX
            //case SceneName.Victory:
            //case SceneName.Defeat:
            //    EndGameManager.Instance.ServerSceneInit(clientId);
            //    break;
        }
    }
}