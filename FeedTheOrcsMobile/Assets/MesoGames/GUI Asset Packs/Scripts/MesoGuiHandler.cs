/// <summary>
/// This class is a basic example of how to handle screen transitions 
/// and dynamically assign OnClick listeners to our button elements in the scene.
/// 
/// All preset screen panels (or those that are pre-created with this package)
/// are handled by this class. The following are the functionalities that is processed here:
/// - setting of screen reference sources based on screen orientation
///     (which is checked at the beginning and depends on a check flag that is set in Editor)
/// - screen transitioning based on the selected Button element
///     (also handles attachment of listener function for each button based on a name prefix)
/// </summary>


using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MesoGuiHandler : MonoBehaviour
{
    protected void Awake()
    {
        Button[] buttonComponents = Resources.FindObjectsOfTypeAll< Button >();
        if( buttonComponents.Length > 0 )
        {
            for( int i = 0; i < buttonComponents.Length; i++ )
            {
                if( buttonComponents[ i ].name.Contains( "Btn" ) )
                {
                    buttonComponents[ i ].onClick.RemoveAllListeners();
                    buttonComponents[ i ].onClick.AddListener( OnClickButton );
                }
            }
        }

        if( m_screens == null )
        {
            m_screens = new UIScreens();
        }
        m_screens = m_isPortrait ? m_portraitScreens : m_landscapeScreens;

        SetScreen( m_screens.title );
    }


    /// <summary>
    /// Listener callback function for when a Button is pressed.
    /// The response is based and checked using the Button's gameObject name.
    /// </summary>
    private void OnClickButton()
    {
        if( ( EventSystem.current.currentSelectedGameObject != null ) )
        {
            string buttonName = EventSystem.current.currentSelectedGameObject.name;

            GameObject refScreen = null;
            bool isRefScreenOverlay = false;
            switch( buttonName )
            {
                case"BtnPlay":
                case "BtnPlayNext":
                case "BtnReplayLevel":
                    {
                        if( m_screens.narratorPopup.activeSelf )
                        {
                            m_screens.narratorPopup.SetActive( false ) ;  
                        }
                        refScreen = m_screens.missionsPopup;
                    }
                break;

                case "BtnStartLevel":
                    {
                        if( m_screens.hudOptions.activeSelf )
                        { 
                            m_screens.hudOptions.SetActive( false );
                        }

                        refScreen = m_screens.hud;                        
                    }
                break;

                case "BtnSelectLevel":
                case "BtnCloseShop":
                    {
                        if( m_screenPrev == m_screens.hud )
                        {
                            refScreen = m_screens.hud;
                        }
                        else if( m_screenPrev == m_screens.title )
                        {
                            refScreen = m_screens.title;
                        }
                    }
                break;

                case "BtnResume":
                case "BtnReplay":
                case "BtnHudPause":
                    {
                        isRefScreenOverlay = true;
                        refScreen = m_screens.hudPause;
                    }
                break;

                case "BtnDummyWin":
                    {
                        refScreen = m_screens.resultsCleared;
                    }
                break;

                case "BtnDummyLose":
                    {
                        refScreen = m_screens.resultsFailed;
                    }
                break;
                    
                case "BtnDummyLogin":
                case "BtnCancelLogin":
                case "BtnContinueLogin":
                    {
                        //TODO add your login code here -- ideally for internal systems or in-house server host
                        isRefScreenOverlay = true;
                        refScreen = m_screens.login;
                    }
                break;

                case "BtnAchievements":
                case "BtnCloseAchievements":
                    {
                        isRefScreenOverlay = true;
                        refScreen = m_screens.achievements;
                    }
                break;

                case "BtnCloseCredits":
                case "BtnCredits":
                    {
                        isRefScreenOverlay = true;
                        refScreen = m_screens.credits;
                    }
                break;

                case "BtnInfo":
                    {
                        isRefScreenOverlay = true;
                        refScreen = m_screens.narratorPopup;
                    }
                break;

                case "BtnShop":
                case "BtnGetMoreCoins":
                case "BtnGetMoreLife":
                    {
                        refScreen = m_screens.shop;
                    }
                break;

                case "BtnHUDOptions":
                    {
                        isRefScreenOverlay = true;
                        refScreen = m_screens.hudOptions;
                    }
                break;

                case "BtnBackToTitle":
                case "BtnLeaveGame":
                case "BtnCancelLeaveGame":
                    {
                        isRefScreenOverlay = true;
                        refScreen = m_screens.warningLeaveGame;

                        if( m_screens.hudOptions.activeSelf )
                        {
                            m_screens.hudOptions.SetActive( false );
                        }
                    }
                break;

                case "BtnContinueLeaveGame":
                    {
                        if( m_screens.hudPause.activeSelf )
                        {
                            m_screens.hudPause.SetActive( false );
                        }

                        if( m_screens.warningLeaveGame )
                        {
                            m_screens.warningLeaveGame.SetActive( false );
                        }

                        refScreen = m_screens.title;
                    }
                break;
            }

            if( refScreen != null )
            {
                SetScreen( refScreen, isRefScreenOverlay );
            }

            EventSystem.current.SetSelectedGameObject( null );
        }
    }

    /// <summary>
    /// Handles setting (activate/deactivating) the screen gameObject parameter.
    /// Screen gameObject treatment if it's a main screen or a popup is determined
    /// by the p_isOverlay boolean flag
    /// </summary>
    private void SetScreen( GameObject p_screenGo, bool p_isOverlay = false )
    {
        if( !p_isOverlay )
        {
            if( ( m_screenCurr != null ) && ( m_screenCurr.name != p_screenGo.name ) )
            {
                m_screenPrev = m_screenCurr;
                m_screenPrev.SetActive( false );
            }
            m_screenCurr = p_screenGo;
            m_screenCurr.SetActive( true );
        }
        else
        {
            bool isActive = p_screenGo.activeSelf;
            p_screenGo.SetActive( !isActive );
        }
    }

    private GameObject m_screenPrev = null;
    private GameObject m_screenCurr = null;
    private UIScreens m_screens = null;

    [SerializeField] private bool m_isPortrait = true;
    [SerializeField] private UIScreens m_portraitScreens = null;
    [SerializeField] private UIScreens m_landscapeScreens = null;

    [System.Serializable]
    protected class UIScreens
    {
        public GameObject defaultScreen = null;

        public GameObject title = null;
        public GameObject login = null;
        public GameObject hud = null;
        public GameObject hudOptions = null;
        public GameObject hudPause = null;
        public GameObject shop = null;
        public GameObject credits = null;
        public GameObject resultsCleared = null;
        public GameObject resultsFailed = null;
        public GameObject achievements = null;

        public GameObject warningLeaveGame = null;

        public GameObject missionsPopup = null;
        public GameObject narratorPopup = null;
    }
}
