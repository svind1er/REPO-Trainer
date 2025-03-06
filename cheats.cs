using System;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

namespace repoMenu
{
    public static class UIHelper
    {
        private static readonly Color mainColor = new Color(0.1f, 0.1f, 0.15f, 0.95f);

        private static readonly Color accentColor = new Color(0.2f, 0.7f, 0.9f);
        private static readonly Color textColor = new Color(0.9f, 0.9f, 0.9f);
        private static readonly Color buttonColor = new Color(0.2f, 0.2f, 0.25f);
        private static readonly Color buttonHoverColor = new Color(0.25f, 0.25f, 0.3f);
        private static readonly Color sliderBgColor = new Color(0.15f, 0.15f, 0.2f);
        private static readonly Color sliderFillColor = new Color(0.8f, 0.2f, 0.2f);

        private static GUIStyle titleStyle;

        private static GUIStyle labelStyle;
        private static GUIStyle buttonStyle;
        private static GUIStyle toggleOnStyle;
        private static GUIStyle toggleOffStyle;
        private static GUIStyle sliderThumbStyle;
        private static GUIStyle sliderStyle;
        private static GUIStyle debugLabelStyle;
        private static GUIStyle selectionGridStyle;
        private static GUIStyle boxStyle;

        private static float x, y, width, height, margin, controlHeight, controlDist, nextControlY;

        private static int columns = 2;
        private static int currentColumn = 0;
        private static int currentRow = 0;

        private static float debugX, debugY, debugWidth, debugHeight, debugMargin, debugControlHeight, debugControlDist, debugNextControlY;
        private static int debugCurrentColumn = 0;
        private static int debugCurrentRow = 0;
        private static int debugColumns = 1;

        public static void InitializeStyles()
        {
            if (titleStyle == null)
            {
                titleStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 16,
                    fontStyle = FontStyle.Bold,
                    normal = { textColor = textColor },
                    alignment = TextAnchor.UpperCenter,
                    margin = new RectOffset(0, 0, 10, 10)
                };

                labelStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 12,
                    normal = { textColor = textColor },
                    alignment = TextAnchor.MiddleLeft
                };

                buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    fontSize = 12,
                    fontStyle = FontStyle.Bold,
                    normal = { background = CreateTexture(buttonColor), textColor = textColor },
                    hover = { background = CreateTexture(buttonHoverColor), textColor = textColor },
                    active = { background = CreateTexture(accentColor), textColor = textColor },
                    padding = new RectOffset(8, 8, 6, 6),
                    alignment = TextAnchor.MiddleCenter
                };

                toggleOnStyle = new GUIStyle(buttonStyle)
                {
                    normal = { background = CreateTexture(accentColor), textColor = textColor },
                    hover = { background = CreateTexture(accentColor), textColor = textColor }
                };

                toggleOffStyle = new GUIStyle(buttonStyle);

                sliderStyle = new GUIStyle(GUI.skin.horizontalSlider)
                {
                    normal = { background = CreateTexture(sliderBgColor) }
                };

                sliderThumbStyle = new GUIStyle(GUI.skin.horizontalSliderThumb)
                {
                    normal = { background = CreateTexture(sliderFillColor) },
                    fixedWidth = 16,
                    fixedHeight = 16
                };

                debugLabelStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 11,
                    normal = { textColor = textColor },
                    wordWrap = true,
                    clipping = TextClipping.Clip
                };

                selectionGridStyle = new GUIStyle(GUI.skin.button)
                {
                    fontSize = 11,
                    normal = { background = CreateTexture(buttonColor), textColor = textColor },
                    hover = { background = CreateTexture(buttonHoverColor), textColor = textColor },
                    active = { background = CreateTexture(accentColor), textColor = textColor },
                    onNormal = { background = CreateTexture(accentColor), textColor = textColor },
                    onHover = { background = CreateTexture(accentColor), textColor = textColor },
                    onActive = { background = CreateTexture(accentColor), textColor = textColor },
                    padding = new RectOffset(6, 6, 4, 4),
                    margin = new RectOffset(1, 1, 1, 1)
                };

                boxStyle = new GUIStyle(GUI.skin.box)
                {
                    normal = { background = CreateTexture(mainColor) },
                    padding = new RectOffset(10, 10, 10, 10)
                };
            }
        }

        private static Texture2D CreateTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        public static void Begin(string text, float _x, float _y, float _width, float _height, float _margin, float _controlHeight, float _controlDist)
        {
            InitializeStyles();

            x = _x;
            y = _y;
            width = _width;
            height = _height;
            margin = _margin;
            controlHeight = _controlHeight;
            controlDist = _controlDist;
            nextControlY = y + margin + 30;

            GUI.Box(new Rect(x, y, width, height), string.Empty, boxStyle);
            GUI.Label(new Rect(x, y + 15, width, 30), text, titleStyle);

            GUI.Box(new Rect(x + margin, y + 45, width - 2 * margin, 2), string.Empty, new GUIStyle
            {
                normal = { background = CreateTexture(accentColor) }
            });
        }

        public static void BeginDebugMenu(string text, float _x, float _y, float _width, float _height, float _margin, float _controlHeight, float _controlDist)
        {
            InitializeStyles();

            debugX = _x;
            debugY = _y;
            debugWidth = _width;
            debugHeight = _height;
            debugMargin = _margin;
            debugControlHeight = _controlHeight;
            debugControlDist = _controlDist;
            debugNextControlY = debugY + debugMargin + 30;

            GUI.Box(new Rect(debugX, debugY, debugWidth, debugHeight), string.Empty, boxStyle);
            GUI.Label(new Rect(debugX, debugY + 15, debugWidth, 30), text, titleStyle);

            GUI.Box(new Rect(debugX + debugMargin, debugY + 45, debugWidth - 2 * debugMargin, 2), string.Empty, new GUIStyle
            {
                normal = { background = CreateTexture(accentColor) }
            });
        }

        private static Rect NextControlRect(float? customX = null, float? customY = null)
        {
            float columnWidth = (width - (columns + 1) * margin) / columns;
            float controlX = customX ?? (x + margin + currentColumn * (columnWidth + margin));
            float controlY = customY ?? nextControlY;

            Rect rect = new Rect(controlX, controlY, columnWidth, controlHeight);

            currentColumn++;
            if (currentColumn >= columns)
            {
                currentColumn = 0;
                currentRow++;
                nextControlY += controlHeight + controlDist;
            }

            return rect;
        }

        private static Rect NextDebugControlRect()
        {
            float controlX = debugX + debugMargin;
            float controlY = debugNextControlY + debugCurrentRow * (debugControlHeight + debugControlDist);

            Rect rect = new Rect(controlX, controlY, debugWidth - debugMargin * 2, debugControlHeight);

            debugCurrentRow++;
            debugNextControlY += debugControlHeight + debugControlDist;

            return rect;
        }

        public static string MakeEnable(string text, bool state)
        {
            return text;
        }

        public static void Label(string text, float? customX = null, float? customY = null)
        {
            GUI.Label(NextControlRect(customX, customY), text, labelStyle);
        }

        public static bool Button(string text, float? customX = null, float? customY = null)
        {
            return GUI.Button(NextControlRect(customX, customY), text, buttonStyle);
        }

        public static bool ToggleButton(string text, bool state, float? customX = null, float? customY = null)
        {
            var style = state ? toggleOnStyle : toggleOffStyle;
            if (GUI.Button(NextControlRect(customX, customY), text, style))
            {
                return !state;
            }
            return state;
        }

        public static float Slider(float val, float min, float max, float? customX = null, float? customY = null)
        {
            Rect rect = NextControlRect(customX, customY);
            float newValue = GUI.HorizontalSlider(rect, val, min, max, sliderStyle, sliderThumbStyle);
            return Mathf.Round(newValue);
        }

        public static int SelectionGrid(int selected, string[] texts, int xCount, float x, float y, float width, float height)
        {
            return GUI.SelectionGrid(new Rect(x, y, width, height), selected, texts, xCount, selectionGridStyle);
        }

        public static void DebugLabel(string text)
        {
            Rect rect = NextDebugControlRect();
            float textHeight = debugLabelStyle.CalcHeight(new GUIContent(text), rect.width);
            if (textHeight > debugControlHeight)
            {
                rect.height = textHeight;
                debugNextControlY += (textHeight - debugControlHeight);
            }
            GUI.Label(rect, text, debugLabelStyle);
        }

        public static void ResetGrid()
        {
            currentColumn = 0;
            currentRow = 0;
            nextControlY = y + margin + 50;
        }

        public static void ResetDebugGrid()
        {
            debugCurrentColumn = 0;
            debugCurrentRow = 0;
            debugNextControlY = debugY + debugMargin + 50;
        }
    }

    public class cheats : MonoBehaviour
    {
        private float nextUpdateTime = 0f;
        private const float updateInterval = 5f;

        private int selectedPlayerIndex = 0;
        private List<string> playerNames = new List<string>();
        private List<object> playerList = new List<object>();
        private float oldSliderValue = 0.5f;
        private float sliderValue = 0.5f;
        public static float offsetESp = 0.5f;
        private bool showMenu = true;
        public static bool godModeEnabled = false;
        public static List<DebugLogMessage> debugLogMessages = new List<DebugLogMessage>();

        private bool showDebugMenu = false;
        private Vector2 playerScrollPosition = Vector2.zero;
        private bool playerTabActive = true;
        private bool gameplayTabActive = false;
        private bool visualsTabActive = false;
        private bool miscTabActive = false;

        private Texture2D keyBindTexture;
        private GUIStyle keyBindStyle;

        public static List<int> colors = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public static System.Random random = new System.Random();
        public static int randomIndex = random.Next(colors.Count);
        public static bool isPlayerRGB = false;
        public static bool showLight = true;
        private Coroutine colorChangeCoroutine;

        public void Start()
        {
            Cursor.visible = showMenu;
            DebugCheats.texture2 = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            DebugCheats.texture2.SetPixel(0, 0, Color.red);
            DebugCheats.texture2.SetPixel(1, 0, Color.red);
            DebugCheats.texture2.SetPixel(0, 1, Color.red);
            DebugCheats.texture2.SetPixel(1, 1, Color.red);
            DebugCheats.texture2.Apply();

            keyBindTexture = new Texture2D(1, 1);
            keyBindTexture.SetPixel(0, 0, new Color(0.8f, 0.2f, 0.2f, 0.8f));
            keyBindTexture.Apply();

            keyBindStyle = new GUIStyle();
            keyBindStyle.normal.background = keyBindTexture;
            keyBindStyle.normal.textColor = Color.white;
            keyBindStyle.alignment = TextAnchor.MiddleCenter;
            keyBindStyle.fontSize = 10;
            keyBindStyle.fontStyle = FontStyle.Bold;

            var playerHealthType = Type.GetType("PlayerHealth, Assembly-CSharp");
            if (playerHealthType != null)
            {
                Log("playerHealthType is not null");
                playerHealth.playerHealthInstance = FindObjectOfType(playerHealthType);
                if (playerHealth.playerHealthInstance != null)
                {
                    Log("playerHealthInstance is not null");
                }
                else
                {
                    Log("playerHealthInstance is null");
                }
            }
            else
            {
                Log("playerHealthType is null");
            }

            var playerMaxHealth = Type.GetType("ItemUpgradePlayerHealth, Assembly-CSharp");
            if (playerMaxHealth != null)
            {
                playerHealth.playerMaxHealthInstance = FindObjectOfType(playerMaxHealth);
                Log("playerMaxHealth is not null");
            }
            else
            {
                Log("playerMaxHealth is null");
            }
        }

        public void UnloadCheat()
        {
            Destroy(this.gameObject);
            playerHealth.playerHealthInstance = null;
            PlayerController.playerSpeedInstance = null;
            playerHealth.playerMaxHealthInstance = null;
            System.GC.Collect();
        }

        public void Update()
        {
            if (Time.time >= nextUpdateTime)
            {
                DebugCheats.UpdateEnemyList();
                nextUpdateTime = Time.time + updateInterval;
            }

            if (oldSliderValue != sliderValue)
            {
                PlayerController.RemoveSpeed(sliderValue);
                oldSliderValue = sliderValue;
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                showMenu = !showMenu;

                Cursor.visible = showMenu;
                Cursor.lockState = showMenu ? CursorLockMode.None : CursorLockMode.Locked;
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Start();
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                UnloadCheat();
            }
            if (Input.GetKeyDown(KeyCode.F8))
            {
                showDebugMenu = !showDebugMenu;
            }

            for (int i = debugLogMessages.Count - 1; i >= 0; i--)
            {
                if (Time.time - debugLogMessages[i].timestamp > 3f)
                {
                    debugLogMessages.RemoveAt(i);
                }
            }
        }

        private IEnumerator UpdateEnemyListCoroutine()
        {
            DebugCheats.UpdateEnemyList();
            yield return null;
        }

        private void UpdatePlayerList()
        {
            playerNames.Clear();
            playerList.Clear();

            var players = SemiFunc.PlayerGetList();

            foreach (var player in players)
            {
                playerList.Add(player);
                string playerName = SemiFunc.PlayerGetName(player) ?? "Unknown Player";
                playerNames.Add(playerName);
            }

            if (playerNames.Count == 0)
            {
                playerNames.Add("No player Found");
            }
        }

        private void ReviveSelectedPlayer()
        {
            if (selectedPlayerIndex < 0 || selectedPlayerIndex >= playerList.Count)
            {
                Log("Invalid player index!");
                return;
            }

            var selectedPlayer = playerList[selectedPlayerIndex];

            var playerDeathHeadField = selectedPlayer.GetType().GetField("playerDeathHead", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (playerDeathHeadField != null)
            {
                var playerDeathHeadInstance = playerDeathHeadField.GetValue(selectedPlayer);
                if (playerDeathHeadInstance != null)
                {
                    var inExtractionPointField = playerDeathHeadInstance.GetType().GetField("inExtractionPoint", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var reviveMethod = playerDeathHeadInstance.GetType().GetMethod("Revive");

                    if (inExtractionPointField != null)
                    {
                        inExtractionPointField.SetValue(playerDeathHeadInstance, true);
                    }

                    reviveMethod?.Invoke(playerDeathHeadInstance, null);
                    Log("Player revived: " + playerNames[selectedPlayerIndex]);
                }
                else
                {
                    Log("playerDeathHead instance not found.");
                }
            }
            else
            {
                Log("Field playerDeathHead not found.");
            }
        }

        private void KillSelectedPlayer()
        {
            if (selectedPlayerIndex < 0 || selectedPlayerIndex >= playerList.Count)
            {
                Log("Invalid player index!");
                return;
            }

            var selectedPlayer = playerList[selectedPlayerIndex];
            if (selectedPlayer == null)
            {
                Log("Selected player is null!");
                return;
            }

            try
            {
                Log($"Selected player type: {selectedPlayer.GetType().FullName}");

                var playerHealthField = selectedPlayer.GetType().GetField("playerHealth", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (playerHealthField != null)
                {
                    var playerHealthInstance = playerHealthField.GetValue(selectedPlayer);
                    if (playerHealthInstance != null)
                    {
                        var healthType = playerHealthInstance.GetType();

                        var hurtMethod = healthType.GetMethod("Hurt", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (hurtMethod != null)
                        {
                            if (PhotonNetwork.IsMasterClient)
                            {
                                var photonViewField = selectedPlayer.GetType().GetField("photonView", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                if (photonViewField != null && photonViewField.GetValue(selectedPlayer) is PhotonView photonView)
                                {
                                    hurtMethod.Invoke(playerHealthInstance, new object[] { 9999, true, -1 });
                                    photonView.RPC("Hurt", RpcTarget.All, 9999, true, -1);
                                    Log($"Player {playerNames[selectedPlayerIndex]} killed with 9999 damage via RPC");
                                }
                                else
                                {
                                    hurtMethod.Invoke(playerHealthInstance, new object[] { 9999, true, -1 });
                                    Log($"Player {playerNames[selectedPlayerIndex]} killed locally with 9999 damage (no PhotonView)");
                                }
                            }
                            else
                            {
                                Log("Only the Master Client can kill other players!");
                            }
                        }
                        else
                        {
                            Log("Method 'Hurt' not found in playerHealth");
                        }
                    }
                    else
                    {
                        Log("playerHealth instance is null");
                    }
                }
                else
                {
                    Log("Field 'playerHealth' not found in the selected player");
                }
            }
            catch (Exception e)
            {
                Log($"Error killing player {playerNames[selectedPlayerIndex]}: {e.Message}");
            }
        }

        private void DrawKeybindIndicator(string key, string description)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(key, keyBindStyle, GUILayout.Width(30), GUILayout.Height(20));
            GUILayout.Label(" - " + description);
            GUILayout.EndHorizontal();
        }

        private bool DrawTabButton(string text, bool isActive)
        {
            GUIStyle tabStyle = new GUIStyle(GUI.skin.button);
            tabStyle.normal.textColor = Color.white;
            tabStyle.normal.background = Texture2D.grayTexture;

            if (isActive)
            {
                tabStyle.normal.background = keyBindTexture;
                tabStyle.fontStyle = FontStyle.Bold;
            }

            return GUILayout.Button(text, tabStyle, GUILayout.Height(30));
        }

        public void OnGUI()
        {
            UIHelper.InitializeStyles();

            if (DebugCheats.drawEspBool)
            {
                StartCoroutine(DrawESP());
            }

            GUI.Box(new Rect(10, 10, 100, 25), "DEL - OPEN", keyBindStyle);

            if (showMenu)
            {
                UpdatePlayerList();

                UIHelper.ResetGrid();
                UIHelper.Begin("R.E.P.O MENU", 50, 50, 600, 700, 20, 30, 10);

                GUILayout.BeginArea(new Rect(70, 95, 560, 30));
                GUILayout.BeginHorizontal();

                if (DrawTabButton("PLAYER", playerTabActive))
                {
                    playerTabActive = true;
                    gameplayTabActive = false;
                    visualsTabActive = false;
                    miscTabActive = false;
                }

                if (DrawTabButton("GAMEPLAY", gameplayTabActive))
                {
                    playerTabActive = false;
                    gameplayTabActive = true;
                    visualsTabActive = false;
                    miscTabActive = false;
                }

                if (DrawTabButton("VISUALS", visualsTabActive))
                {
                    playerTabActive = false;
                    gameplayTabActive = false;
                    visualsTabActive = true;
                    miscTabActive = false;
                }

                if (DrawTabButton("MISC", miscTabActive))
                {
                    playerTabActive = false;
                    gameplayTabActive = false;
                    visualsTabActive = false;
                    miscTabActive = true;
                }

                GUILayout.EndHorizontal();
                GUILayout.EndArea();

                if (playerTabActive)
                {
                    UIHelper.ResetGrid();

                    godModeEnabled = UIHelper.ToggleButton("God Mode", godModeEnabled, 70, 150);
                    if (UIHelper.Button("Heal Player", 370, 150))
                    {
                        playerHealth.HealPlayer(50);
                    }

                    if (UIHelper.Button("Infinite Health", 70, 200))
                    {
                        playerHealth.MaxHealth();
                    }

                    if (UIHelper.Button("Damage Player", 370, 200))
                    {
                        playerHealth.DamagePlayer(1);
                    }

                    if (UIHelper.Button("Infinite Stamina", 70, 250))
                    {
                        PlayerController.MaxStamina();
                    }

                    if (UIHelper.Button("Flashlight", 70, 350))
                    {
                        FlashLightController.FlashlightTrigger();
                    }

                    bool newIsPlayerRGB = UIHelper.ToggleButton("RGB Color", isPlayerRGB, 70, 300);
                    if (newIsPlayerRGB != isPlayerRGB)
                    {
                        isPlayerRGB = newIsPlayerRGB;
                        if (isPlayerRGB)
                        {
                            colorChangeCoroutine = StartCoroutine(ColorChangeLoop());
                        }
                        else
                        {
                            StopCoroutine(colorChangeCoroutine);
                        }
                    }

                    UIHelper.Label("Player Speed: " + sliderValue, 70, 630);
                    oldSliderValue = sliderValue;
                    sliderValue = UIHelper.Slider(sliderValue, .5f, 30f, 70, 660);

                    GUI.Box(new Rect(130, 390, 460, 150), "Player Selection", new GUIStyle(GUI.skin.box)
                    {
                        normal = { background = Texture2D.grayTexture, textColor = Color.white },
                        fontSize = 12,
                        fontStyle = FontStyle.Bold,
                        alignment = TextAnchor.UpperCenter,
                        padding = new RectOffset(0, 0, 5, 5)
                    });

                    playerScrollPosition = GUI.BeginScrollView(
                        new Rect(140, 405, 440, 115),
                        playerScrollPosition,
                        new Rect(0, 0, 420, playerNames.Count * 25)
                    );

                    selectedPlayerIndex = UIHelper.SelectionGrid(
                        selectedPlayerIndex,
                        playerNames.ToArray(),
                        1,
                        0,
                        0,
                        420,
                        playerNames.Count * 25
                    );

                    GUI.EndScrollView();

                    if (UIHelper.Button("Revive Player", 70, 550))
                    {
                        ReviveSelectedPlayer();
                    }

                    if (UIHelper.Button("Kill Player", 370, 550))
                    {
                        KillSelectedPlayer();
                    }

                    if (UIHelper.Button("Send To Void", 70, 600))
                    {
                        PlayerController.SendFirstPlayerToVoid();
                    }
                }
                else if (gameplayTabActive)
                {
                    UIHelper.ResetGrid();

                    if (UIHelper.Button("Kill All Enemies", 70, 150))
                    {
                        DebugCheats.KillAllEnemies();
                    }

                    if (UIHelper.Button("Spawn Money", 370, 150))
                    {
                        DebugCheats.SpawnItem();
                    }
                }
                else if (visualsTabActive)
                {
                    UIHelper.ResetGrid();
                    DebugCheats.drawEspBool = UIHelper.ToggleButton("Enemy ESP", DebugCheats.drawEspBool, 70, 150);
                }
                else if (miscTabActive)
                {
                    UIHelper.ResetGrid();

                    if (UIHelper.Button("Reload Menu (F5)", 70, 150))
                    {
                        Start();
                    }

                    if (UIHelper.Button("Unload Menu (F9)", 370, 150))
                    {
                        UnloadCheat();
                    }

                    if (UIHelper.Button("Toggle Debug (F8)", 70, 200))
                    {
                        showDebugMenu = !showDebugMenu;
                    }

                    GUI.Box(new Rect(70, 250, 460, 130), "Keybinds", new GUIStyle(GUI.skin.box)
                    {
                        normal = { background = Texture2D.grayTexture, textColor = Color.white },
                        fontSize = 12,
                        fontStyle = FontStyle.Bold,
                        alignment = TextAnchor.UpperCenter,
                        padding = new RectOffset(0, 0, 5, 5)
                    });

                    GUILayout.BeginArea(new Rect(110, 275, 440, 70));
                    DrawKeybindIndicator("DEL", "Toggle Menu");
                    DrawKeybindIndicator("F5", "Reload Menu");
                    DrawKeybindIndicator("F8", "Toggle Debug Log");
                    DrawKeybindIndicator("F9", "Unload Menu");
                    GUILayout.EndArea();
                }
            }

            if (showDebugMenu)
            {
                UIHelper.ResetDebugGrid();
                UIHelper.BeginDebugMenu("Debug Log", 660, 50, 450, 500, 10, 30, 5);

                foreach (var logMessage in debugLogMessages)
                {
                    if (!string.IsNullOrEmpty(logMessage.message))
                    {
                        UIHelper.DebugLabel(logMessage.message);
                    }
                }
            }
        }

        private IEnumerator ColorChangeLoop()
        {
            while (isPlayerRGB)
            {
                playerColor.ColorRandomizer();
                yield return new WaitForSeconds(0.2f);
            }
        }

        private IEnumerator DrawESP()
        {
            while (DebugCheats.drawEspBool)
            {
                DebugCheats.DrawESP();
                yield return null;
            }
        }

        public static void Log(string message)
        {
            debugLogMessages.Add(new DebugLogMessage(message, Time.time));
        }

        public class DebugLogMessage
        {
            public string message;
            public float timestamp;

            public DebugLogMessage(string msg, float time)
            {
                message = msg;
                timestamp = time;
            }
        }
    }
}