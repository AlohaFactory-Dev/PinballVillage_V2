using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aloha.Coconut;
using FactorySystem;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Stage.Building;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using UnityEditor; // 추가

#if UNITY_EDITOR

[InfoBox("G : 10000골드 추가" +
    "\nH : 골드 획등량 세팅" +
    "\nL : 건물 레벨업" +
    "\nI : 적과의 충돌 무시 토글" +
    "\nB : 건물 생성 시작" +
    "\nC : 카메라 줌 인" +
    "\nU : CPIUI OnOff" +
    "\nUpArrow : Time Scale + 1" +
    "\nDownArrow : Time Scale - 1" +
    "\nRightArrow, LeftArrow : Time Scale = 1")]
[InfoBox("입력 기록/재생:\n" +
    "- onRecord: 입력 기록 시작\n" +
    "- onReplay: 다음 런타임 시 자동 재생\n" +
    "- 두개 모두 true면 onRecord 무시")]
public class TestManager : MonoBehaviour
{
    [Serializable]
    public struct SpawnBuildingList
    {
        public string buildingId;
        public Vector2Int gridPosition;
    }

    [Serializable]
    public struct LevelUpBuildingList
    {
        public bool allBuildingLevelUp;
        public List<string> buildingIds;
    }

    [Serializable]
    public struct SpawnList
    {
        public List<SpawnBuildingList> spawnBuildingList;
    }

    [Inject] private GoldManager _goldManager;
    [Inject] private StageGlobalClock _stageGlobalClock;
    [Inject] StageUI stageUI;
    [Inject] private CPIUI cpiui;

    [Space]
    [InfoBox("건물 레벨업 설정")]
    [SerializeField]
    private List<LevelUpBuildingList> levelUpBuildingLists = new();

    private int _levelUpBuildingIndex = 0;

    [Space]
    [InfoBox("적과의 충돌 무시 설정")]
    [SerializeField]
    private bool onEnemyCollisionIgnore = false;

    private static bool _onEnemyCollsionIgnore = false;

    [Space]
    [InfoBox("건물 생성 설정")]
    [SerializeField]
    private int spawnBuildingIndex;

    [SerializeField] private SpawnList[] spawnList;
    [SerializeField] private float spawnInterval = 0.1f;

    [Space]
    [InfoBox("건물 카드 생성 모드")]
    [SerializeField]
    private bool onSettingBuildingCardMode = false;

    private static bool _onSettingBuildingCardMode = false;
    [SerializeField] private List<string> settingBuildingCardIds;
    public static List<string> SettingBuildingCardIds;

    [Space]
    [InfoBox("CPIUI On/Off")]
    [SerializeField]
    private bool cpiuiOnOff = false;

    [Space]
    [InfoBox("골드 획득량 설정")]
    [SerializeField]
    private List<int> settingGoldHistory = new();

    private int _goldHistoryIndex = 0;

    [Space]
    [InfoBox("중립 스킵 설정")]
    [SerializeField]
    private bool onNaturalSkip = false;

    [Space]
    [InfoBox("카메라 컨트롤러")]
    [SerializeField]
    float[] cameraNomalizedPathPositions = { 0.5f, 0.75f, 1f };

    [SerializeField] float cameraSpeed = 0.5f;

    [SerializeField] AnimationCurve cameraMoveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // 추가

    [Space]
    [InfoBox("입력 기록/재생 기능")]
    [SerializeField]
    private string inputRecordFilePath = "Assets/CPIAction/Aiden";

    [SerializeField] private string fileName = "input_record";

    [SerializeField] private bool onRecord = false;

    private bool replayInput = false;
    [SerializeField] private bool onReplay = true;

    // 입력 기록/재생 관련 변수
    private List<InputEvent> inputEvents = new List<InputEvent>();
    private float recordStartTime;
    private int replayIndex = 0;
    private string inputRecordPath => Path.Combine(inputRecordFilePath, fileName + ".json");
    private bool isReplaying = false;

    // 정적 프로퍼티
    public static bool OnEnemyCollsionIgnore => _onEnemyCollsionIgnore;
    public static bool OnSettingBuildingCardMode => _onSettingBuildingCardMode;
    public static bool OnNaturalSkip { get; private set; }

    // 재생 이벤트
    private event Action<int, Vector3> OnReplayMouseDown;
    private event Action<int, Vector3> OnReplayMouseUp;
    private event Action<int, Vector3> OnReplayMouseDrag;
    private event Action<KeyCode> OnReplayKeyDown;
    private event Action<KeyCode> OnReplayKeyUp;

    [Inject] private BuildModeManager _buildModeManager; // BuildModeManager 참조 추가

    private void Awake()
    {
        InitializeSettings();
        InitializeReplaySystem();
        CPIUIOnOff();
        // onNaturalSkip 인스펙터 값으로 static 프로퍼티 할당
        OnNaturalSkip = onNaturalSkip;
    }

    private void InitializeSettings()
    {
        cpiui.Init();
        _onSettingBuildingCardMode = onSettingBuildingCardMode;
        _onEnemyCollsionIgnore = onEnemyCollisionIgnore;
        SettingBuildingCardIds = settingBuildingCardIds;
    }

    private void InitializeReplaySystem()
    {
        recordStartTime = Time.time;

        RegisterReplayEventHandlers();

        if (onReplay)
        {
            LoadInputEvents();
            onRecord = false;
            replayInput = true;
            isReplaying = true;
        }
        // recordInput이 true일 때 파일이 이미 있으면 덮어쓸지 물어봄
        else if (onRecord && File.Exists(inputRecordPath))
        {
            bool overwrite = EditorUtility.DisplayDialog(
                "입력 기록 파일 덮어쓰기",
                $"기존 입력 기록 파일이 존재합니다.\n{inputRecordPath}\n덮어쓰시겠습니까?",
                "예(덮어쓰기)", "아니오(기록 안함)"
            );
            if (overwrite)
            {
                inputEvents.Clear();
                SaveInputEvents(); // 기존 파일 덮어쓰기(초기화)
                Debug.Log("기존 입력 기록 파일을 덮어썼습니다.");
            }
            else
            {
                onRecord = false;
                Debug.Log("입력 기록을 시작하지 않습니다.");
            }
        }
    }

    private void RegisterReplayEventHandlers()
    {
        OnReplayKeyDown += HandleReplayKeyDown;
        OnReplayKeyUp += HandleReplayKeyUp;
        OnReplayMouseDown += HandleReplayMouseDown;
        OnReplayMouseUp += HandleReplayMouseUp;
        OnReplayMouseDrag += HandleReplayMouseDrag;
    }

    private void HandleReplayKeyDown(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.G:
                _goldManager.AddGold(10000);
                break;
            case KeyCode.L:
                LevelUpBuilding();
                break;
            case KeyCode.I:
                onEnemyCollisionIgnore = !onEnemyCollisionIgnore;
                _onEnemyCollsionIgnore = onEnemyCollisionIgnore;
                Debug.Log($"재생: On Enemy Collision Ignore: {onEnemyCollisionIgnore}");
                break;
            case KeyCode.B:
                StartCoroutine(SpawnBuilding());
                break;
            case KeyCode.C:
                if (StageContainer.Get<CameraController>().ZoomIn())
                {
                    cpiuiOnOff = true;
                    CPIUIOnOff();
                }
                else
                {
                    cpiuiOnOff = false;
                    CPIUIOnOff();
                }

                break;
            case KeyCode.U:
                cpiuiOnOff = !cpiuiOnOff;
                CPIUIOnOff();
                break;
            case KeyCode.H:
                if (_goldHistoryIndex >= settingGoldHistory.Count)
                {
                    Debug.Log("모든 골드 획득량 설정 완료");
                    return;
                }

                _goldManager.SetGoldHistory(settingGoldHistory[_goldHistoryIndex]);
                _goldHistoryIndex++;
                break;
            case KeyCode.UpArrow:
                Time.timeScale += 1f;
                SystemUI.ShowToastMessage($"재생: Time Scale: {Time.timeScale}");
                break;
            case KeyCode.DownArrow:
                Time.timeScale -= 1f;
                SystemUI.ShowToastMessage($"재생: Time Scale: {Time.timeScale}");
                break;
            case KeyCode.RightArrow:
            case KeyCode.LeftArrow:
                Time.timeScale = 1f;
                SystemUI.ShowToastMessage($"재생: Time Scale: {Time.timeScale}");
                break;
        }
    }

    private void HandleReplayKeyUp(KeyCode keyCode)
    {
        Debug.Log($"재생: 키 업 {keyCode}");
        // 필요한 KeyUp 처리 로직 추가 가능
    }

    private void HandleReplayMouseDown(int button, Vector3 position)
    {
        Debug.Log($"재생: 마우스 다운 - 버튼: {button}, 위치: {position}");
        // 마우스 다운 시 BuildModeManager의 Update 호출
#if UNITY_EDITOR
        _buildModeManager?.Update(InputEventType.MouseDown, position);
#endif
    }

    private void HandleReplayMouseUp(int button, Vector3 position)
    {
        Debug.Log($"재생: 마우스 업 - 버튼: {button}, 위치: {position}");
        // 마우스 업 시 BuildModeManager의 Update 호출
#if UNITY_EDITOR
        _buildModeManager?.Update(InputEventType.MouseUp, position);
#endif
    }

    private void HandleReplayMouseDrag(int button, Vector3 position)
    {
        Debug.Log($"재생: 마우스 드래그 - 버튼: {button}, 위치: {position}");
        // 마우스 드래그 시 BuildModeManager의 Update 호출
#if UNITY_EDITOR
        _buildModeManager?.Update(InputEventType.MouseDrag, position);
#endif
    }


    private void Update()
    {
        if (onRecord)
            RecordInputs();

        if (replayInput && inputEvents.Count > 0)
            ReplayInputs();

        if (!isReplaying)
            HandleRealTimeInput();
    }

    private void HandleRealTimeInput()
    {
        if (Input.GetKeyDown(KeyCode.G))
            _goldManager.AddGold(10000);
        if (Input.GetKeyDown(KeyCode.L))
            LevelUpBuilding();
        if (Input.GetKeyDown(KeyCode.I))
        {
            onEnemyCollisionIgnore = !onEnemyCollisionIgnore;
            _onEnemyCollsionIgnore = onEnemyCollisionIgnore;
            Debug.Log($"On Enemy Collision Ignore: {onEnemyCollisionIgnore}");
        }

        if (Input.GetKeyDown(KeyCode.B))
            StartCoroutine(SpawnBuilding());
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (StageContainer.Get<CameraController>().ZoomIn())
            {
                cpiuiOnOff = true;
                CPIUIOnOff();
            }
            else
            {
                cpiuiOnOff = false;
                CPIUIOnOff();
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            cpiuiOnOff = !cpiuiOnOff;
            CPIUIOnOff();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (_goldHistoryIndex >= settingGoldHistory.Count)
            {
                Debug.Log("모든 골드 획득량 설정 완료");
                return;
            }

            _goldManager.SetGoldHistory(settingGoldHistory[_goldHistoryIndex]);
            _goldHistoryIndex++;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Time.timeScale += 1f;
            SystemUI.ShowToastMessage($"Time Scale: {Time.timeScale}");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Time.timeScale -= 1f;
            SystemUI.ShowToastMessage($"Time Scale: {Time.timeScale}");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Time.timeScale = 1f;
            SystemUI.ShowToastMessage($"Time Scale: {Time.timeScale}");
        }

        for (int i = 0; i < cameraNomalizedPathPositions.Length && i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Keypad1 + i))
            {
                var cameraController = StageContainer.Get<CameraController>();
                if (cameraController != null)
                {
                    cameraController.MoveToPathPosition(cameraNomalizedPathPositions[i], cameraSpeed, cameraMoveCurve);

                    // 카메라 이동 액션 기록
                    if (onRecord)
                    {
                        inputEvents.Add(new InputEvent
                        {
                            type = InputEventType.CameraMove,
                            cameraIndex = i,
                            cameraPathPosition = cameraNomalizedPathPositions[i],
                            cameraSpeed = cameraSpeed,
                            time = Time.time - recordStartTime
                            // AnimationCurve는 직렬화가 제한적이므로 필요시 별도 처리
                        });
                    }
                }
            }
        }
    }

    private void RecordInputs()
    {
        // 키 입력 기록
        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                inputEvents.Add(new InputEvent
                {
                    type = InputEventType.KeyDown,
                    keyCode = key,
                    time = Time.time - recordStartTime
                });
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            inputEvents.Add(new InputEvent
            {
                type = InputEventType.MouseDown,
                mouseButton = 0,
                MousePosition = Input.mousePosition,
                time = Time.time - recordStartTime
            });
        }

        if (Input.GetMouseButtonUp(0))
        {
            inputEvents.Add(new InputEvent
            {
                type = InputEventType.MouseUp,
                mouseButton = 0,
                MousePosition = Input.mousePosition,
                time = Time.time - recordStartTime
            });
        }

        // 마우스 드래그 기록
        if (Input.GetMouseButton(0))
        {
            inputEvents.Add(new InputEvent
            {
                type = InputEventType.MouseDrag,
                mouseButton = 0,
                MousePosition = Input.mousePosition,
                time = Time.time - recordStartTime
            });
        }
    }

    private void ReplayInputs()
    {
        if (replayIndex >= inputEvents.Count)
        {
            if (isReplaying)
            {
                isReplaying = false;
                Debug.Log("입력 재생 완료");
            }

            return;
        }

        float elapsed = Time.time - recordStartTime;
        while (replayIndex < inputEvents.Count && inputEvents[replayIndex].time <= elapsed)
        {
            var e = inputEvents[replayIndex];
            switch (e.type)
            {
                case InputEventType.MouseDown:
                    OnReplayMouseDown?.Invoke(e.mouseButton, new Vector3(e.mouseX, e.mouseY, e.mouseZ));
                    Debug.Log($"재생: 마우스 버튼 {e.mouseButton} 다운 ({e.MousePosition})");
                    break;
                case InputEventType.MouseUp:
                    OnReplayMouseUp?.Invoke(e.mouseButton, new Vector3(e.mouseX, e.mouseY, e.mouseZ));
                    Debug.Log($"재생: 마우스 버튼 {e.mouseButton} 업 ({e.MousePosition})");
                    break;
                case InputEventType.MouseDrag:
                    OnReplayMouseDrag?.Invoke(e.mouseButton, new Vector3(e.mouseX, e.mouseY, e.mouseZ));
                    break;
                case InputEventType.KeyDown:
                    OnReplayKeyDown?.Invoke(e.keyCode);
                    Debug.Log($"재생: 키 다운 {e.keyCode}");
                    break;
                case InputEventType.KeyUp:
                    OnReplayKeyUp?.Invoke(e.keyCode);
                    Debug.Log($"재생: 키 업 {e.keyCode}");
                    break;
                case InputEventType.CameraMove:
                    var cameraController = StageContainer.Get<CameraController>();
                    if (cameraController != null)
                    {
                        cameraController.MoveToPathPosition(e.cameraPathPosition, e.cameraSpeed, cameraMoveCurve);
                        Debug.Log($"재생: 카메라 이동 인덱스 {e.cameraIndex}, 위치 {e.cameraPathPosition}, 속도 {e.cameraSpeed}");
                    }

                    break;
            }

            replayIndex++;
        }
    }

    private void LoadInputEvents()
    {
        if (File.Exists(inputRecordPath))
        {
            try
            {
                var json = File.ReadAllText(inputRecordPath, Encoding.UTF8);
                inputEvents = JsonConvert.DeserializeObject<List<InputEvent>>(json) ?? new List<InputEvent>();
                recordStartTime = Time.time;
                replayIndex = 0;
                Debug.Log($"Input 기록 로드됨: {inputRecordPath} (총 {inputEvents.Count}개 이벤트)");
            }
            catch (Exception e)
            {
                Debug.LogError($"입력 기록 로드 실패: {e.Message}");
                inputEvents = new List<InputEvent>();
            }
        }
    }

    private void SaveInputEvents()
    {
        try
        {
            var json = JsonConvert.SerializeObject(inputEvents, Formatting.Indented);
            File.WriteAllText(inputRecordPath, json, Encoding.UTF8);
            Debug.Log($"Input 기록 저장됨: {inputRecordPath} (총 {inputEvents.Count}개 이벤트)");
        }
        catch (Exception e)
        {
            Debug.LogError($"입력 기록 저장 실패: {e.Message}");
        }
    }


    #region Spawn Building Mode

    private IEnumerator SpawnBuilding()
    {
        var buildingManager = StageContainer.Get<BuildingManager>();
        var spawnerGridManager = StageContainer.Get<SpawnerGridManager>();
        if (spawnBuildingIndex >= spawnList.Length)
        {
            Debug.Log("모든 건물 생성 완료");
            yield break;
        }

        var spawnData = spawnList[spawnBuildingIndex];
        foreach (var spawnBuilding in spawnData.spawnBuildingList)
        {
            var spawner = spawnerGridManager.GetSpawner(spawnBuilding.gridPosition);
            if (!spawner.IsEmpty)
            {
                buildingManager.RemoveBuilding(spawner.Building);
            }

            if (spawnBuilding.buildingId.Contains("Enemy"))
                spawner.ResetOwner(OwnerType.Enemy);
            else
                spawner.ResetOwner(OwnerType.Player);
            buildingManager.SpawnBuilding(spawnBuilding.buildingId, spawner);
            yield return new WaitForSeconds(spawnInterval);
        }

        spawnBuildingIndex++;
    }

    #endregion

    #region Setting Building Card Mode

    public static BuildingTable GetSettingBuildingTable()
    {
        string buildingId = SettingBuildingCardIds[0];
        SettingBuildingCardIds.RemoveAt(0);
        if (SettingBuildingCardIds.Count == 0)
        {
            _onSettingBuildingCardMode = false;
        }

        return TableListContainer.Get<BuildingTableList>().GetBuildingTable(buildingId);
    }

    #endregion

    #region Level Up Building Mode

    private void LevelUpBuilding()
    {
        StartCoroutine(LevelUpBuildingCoroutine());
    }

    private IEnumerator LevelUpBuildingCoroutine()
    {
        if (_levelUpBuildingIndex >= levelUpBuildingLists.Count)
        {
            Debug.Log("모든 건물 레벨업 완료");
            yield break;
        }

        var levelUpList = levelUpBuildingLists[_levelUpBuildingIndex];
        if (levelUpList.allBuildingLevelUp)
        {
            StageContainer.Get<BuildingManager>().PlayerAllBuildingLevelUp();
        }
        else
        {
            foreach (var buildingId in levelUpList.buildingIds)
            {
                var buildings = StageContainer.Get<BuildingManager>().GetBuildings(buildingId);
                foreach (var building in buildings)
                {
                    StageContainer.Get<BuildingManager>().LevelUpBuilding(building.Spawner);
                    yield return new WaitForSeconds(spawnInterval); // 레벨업 간격 조정
                }
            }
        }

        _levelUpBuildingIndex++;
    }

    #endregion

    #region CPIUI On/Off

    private void CPIUIOnOff()
    {
        if (cpiuiOnOff)
        {
            cpiui.On();
            stageUI.Off();
        }
        else
        {
            cpiui.Off();
            stageUI.On();
        }
    }

    #endregion

    private void OnApplicationQuit()
    {
        if (onRecord && inputEvents.Count > 0)
        {
            SaveInputEvents();
            // 파일이 즉시 디스크에 기록되도록 Flush 처리
            try
            {
                using (var fs = new FileStream(inputRecordPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    fs.Flush(true);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"파일 즉시 기록 실패: {e.Message}");
            }

            Debug.Log("애플리케이션 종료 시 입력 기록 자동 저장 및 즉시 파일 업데이트됨");
        }
    }

    private void OnDestroy()
    {
        OnReplayKeyDown -= HandleReplayKeyDown;
        OnReplayKeyUp -= HandleReplayKeyUp;
        OnReplayMouseDown -= HandleReplayMouseDown;
        OnReplayMouseUp -= HandleReplayMouseUp;
        OnReplayMouseDrag -= HandleReplayMouseDrag;
    }
}
#endif

public enum InputEventType
{
    KeyDown,
    KeyUp,
    MouseDown,
    MouseUp,
    MouseDrag,
    CameraMove // 카메라 이동 이벤트 타입 추가
}

[Serializable]
public class InputEvent
{
    public InputEventType type;
    public KeyCode keyCode;
    public int mouseButton;
    public float mouseX;
    public float mouseY;
    public float mouseZ;
    public float time;

    // CameraMove 관련 데이터
    public int cameraIndex;
    public float cameraPathPosition;
    public float cameraSpeed;

    [JsonIgnore]
    public Vector3 MousePosition
    {
        get => new Vector3(mouseX, mouseY, mouseZ);
        set
        {
            mouseX = value.x;
            mouseY = value.y;
            mouseZ = value.z;
        }
    }
}