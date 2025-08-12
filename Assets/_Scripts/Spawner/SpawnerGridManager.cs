using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Stage.Building
{
    public class SpawnerGridManager : MonoBehaviour
    {
        [System.Serializable]
        public struct TilemapSettings
        {
            public OwnerType ownerType;
            public Sprite[] tileSprites; // 타일 스프라이트 배열
        }

        [SerializeField] TilemapSettings[] tilemapSettings; // 타일맵 설정 배열

        [Header("Grid Settings")]
        [SerializeField]
        private Tilemap tilemap;

        private int _minGridX, _maxGridX, _minGridY, _maxGridY;
        [SerializeField] private int gridWidth;

        [SerializeField] private int gridHeight;
        private int _stageClearPercent;
        private Spawner[,] _spawnerGrid;
        private readonly List<Spawner> _allSpawners = new List<Spawner>();
        private List<Spawner> _enemySpawners = new List<Spawner>();
        private List<Spawner> _playerSpawners = new List<Spawner>();
        private ReactiveProperty<int> _playerPercent = new ReactiveProperty<int>();
        public IReadOnlyReactiveProperty<int> PlayerPercent => _playerPercent;
        public OwnerType Winner => PlayerPercent.Value >= _stageClearPercent ? OwnerType.Player : OwnerType.Enemy;


        public int GetPlayerSpawnerPercentage()
        {
            if (_allSpawners.Count == 0) return 0;

            int playerSpawnerCount = _allSpawners.FindAll(s => s.CurrentOwner == OwnerType.Player).Count;
            return Mathf.CeilToInt((float)playerSpawnerCount / _allSpawners.Count * 100);
        }

        public void AddPercentage(Spawner spawner, OwnerType ownerType)
        {
            if (OwnerType.Player == ownerType && !_playerSpawners.Contains(spawner))
            {
                _playerSpawners.Add(spawner);
            }
            else if (OwnerType.Enemy == ownerType && !_enemySpawners.Contains(spawner))
            {
                _enemySpawners.Add(spawner);
            }
            else if (OwnerType.Neutral == ownerType)
            {
                if (_playerSpawners.Contains(spawner))
                {
                    _playerSpawners.Remove(spawner);
                }

                if (_enemySpawners.Contains(spawner))
                {
                    _enemySpawners.Remove(spawner);
                }
            }

            _playerPercent.Value = Mathf.CeilToInt((float)_playerSpawners.Count / (_playerSpawners.Count + _enemySpawners.Count) * 100);
        }

        private readonly Vector2Int[] _directions =
        {
            new Vector2Int(0, 1), // 위
            new Vector2Int(0, -1), // 아래
            new Vector2Int(-1, 0), // 왼쪽
            new Vector2Int(1, 0) // 오른쪽
        };

        public void Init()
        {
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            _stageClearPercent = Mathf.CeilToInt(TableListContainer.Get<EtcTableList>().GetEtcTable("stageClearCondition").values[1] * 100);
            _allSpawners.Clear();
            Spawner[] existingSpawners = tilemap.GetComponentsInChildren<Spawner>();

            _spawnerGrid = new Spawner[gridWidth, gridHeight];

            for (int i = 0; i < existingSpawners.Length; i++)
            {
                RegisterSpawner(existingSpawners[i], existingSpawners[i].GridPosition);
            }

            for (int i = 0; i < existingSpawners.Length; i++)
            {
                existingSpawners[i].SpawnDefaultBuilding();
            }
        }

        private void CalculateGridBounds(List<Vector2Int> gridPositions)
        {
            if (gridPositions.Count == 0) return;

            _minGridX = _maxGridX = gridPositions[0].x;
            _minGridY = _maxGridY = gridPositions[0].y;

            foreach (Vector2Int pos in gridPositions)
            {
                _minGridX = Mathf.Min(_minGridX, pos.x);
                _maxGridX = Mathf.Max(_maxGridX, pos.x);
                _minGridY = Mathf.Min(_minGridY, pos.y);
                _maxGridY = Mathf.Max(_maxGridY, pos.y);
            }

            gridWidth = _maxGridX - _minGridX + 1;
            gridHeight = _maxGridY - _minGridY + 1;
        }

        public Vector2Int WorldToGridPosition(Vector2 worldPosition)
        {
            if (tilemap)
            {
                Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
                return new Vector2Int(cellPosition.x, cellPosition.y);
            }

            return new Vector2Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.y));
        }

        private Vector2Int GridToArrayIndex(Vector2Int gridPos)
        {
            return new Vector2Int(gridPos.x - _minGridX, _maxGridY - gridPos.y);
        }

        private bool IsValidArrayIndex(int arrayX, int arrayY)
        {
            return arrayX >= 0 && arrayX < gridWidth && arrayY >= 0 && arrayY < gridHeight;
        }

        private void RegisterSpawner(Spawner spawner, Vector2Int gridPos)
        {
            spawner.Init(gridPos.x, gridPos.y);
            _spawnerGrid[gridPos.x, gridPos.y] = spawner;
            _allSpawners.Add(spawner);
        }

        private Spawner GetSpawner(int arrayX, int arrayY)
        {
            return IsValidArrayIndex(arrayX, arrayY) ? _spawnerGrid[arrayX, arrayY] : null;
        }

        public List<Spawner> GetNeighbors(Spawner spawner, int range)
        {
            if (!spawner || range <= 0) return new List<Spawner>();
            return GetNeighbors(spawner.GridPosition.x, spawner.GridPosition.y, range);
        }

        private List<Spawner> GetNeighbors(int arrayX, int arrayY, int range)
        {
            var neighbors = new List<Spawner>();
            if (range <= 0) return neighbors;

            // 다이아몬드 형태로 탐색 (맨하탄 거리 기준)
            for (int dx = -range; dx <= range; dx++)
            {
                for (int dy = -range; dy <= range; dy++)
                {
                    // 맨하탄 거리가 range 이하인 경우만 포함
                    if (Mathf.Abs(dx) + Mathf.Abs(dy) <= range && (dx != 0 || dy != 0))
                    {
                        int nextX = arrayX + dx;
                        int nextY = arrayY + dy;

                        if (IsValidArrayIndex(nextX, nextY))
                        {
                            Spawner neighbor = GetSpawner(nextX, nextY);
                            if (neighbor) neighbors.Add(neighbor);
                        }
                    }
                }
            }

            return neighbors;
        }

        public Spawner GetRandomSpawner()
        {
            if (_allSpawners.Count == 0) return null;

            var validSpawners = _allSpawners.FindAll(s => s.IsEmpty && s.CurrentOwner == OwnerType.Player);
            return validSpawners.Count > 0 ? validSpawners[Random.Range(0, validSpawners.Count)] : null;
        }

        public Spawner GetNearestEmptySpawner(Spawner currentSpawner, OwnerType ownerType)
        {
            return GetNearestEmptySpawner(currentSpawner.GridPosition.x, currentSpawner.GridPosition.y, ownerType);
        }

        private Spawner GetNearestEmptySpawner(int arrayX, int arrayY, OwnerType ownerType)
        {
            if (!IsValidArrayIndex(arrayX, arrayY)) return null;

            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            bool[][] visited = new bool[gridWidth][];
            for (int index = 0; index < gridWidth; index++)
            {
                visited[index] = new bool[gridHeight];
            }

            visited[arrayX][arrayY] = true;

            foreach (Vector2Int dir in _directions)
            {
                int nextX = arrayX + dir.x;
                int nextY = arrayY + dir.y;

                if (IsValidArrayIndex(nextX, nextY) && !visited[nextX][nextY])
                {
                    visited[nextX][nextY] = true;
                    queue.Enqueue(new Vector2Int(nextX, nextY));
                }
            }

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                Spawner currentSpawner = GetSpawner(current.x, current.y);

                // OwnerType이 일치하고 비어있는 경우만 반환
                if (currentSpawner && currentSpawner.IsEmpty && currentSpawner.CurrentOwner == ownerType)
                    return currentSpawner;

                foreach (Vector2Int dir in _directions)
                {
                    int nextX = current.x + dir.x;
                    int nextY = current.y + dir.y;

                    if (IsValidArrayIndex(nextX, nextY) && !visited[nextX][nextY])
                    {
                        visited[nextX][nextY] = true;
                        queue.Enqueue(new Vector2Int(nextX, nextY));
                    }
                }
            }

            return null;
        }

        public List<Spawner> GetAllSpawners() => _allSpawners;
#if UNITY_EDITOR
        [ContextMenu("Refresh Grid")]
        public void RefreshGrid()
        {
            _allSpawners.Clear();
            Spawner[] existingSpawners = tilemap.GetComponentsInChildren<Spawner>();
            // 기존 그리드 재계산 및 등록
            List<Vector2Int> gridPositions = new List<Vector2Int>();
            foreach (Spawner spawner in existingSpawners)
            {
                gridPositions.Add(spawner.GridPosition);
            }

            CalculateGridBounds(gridPositions);
            _spawnerGrid = new Spawner[gridWidth, gridHeight];
            for (int i = 0; i < existingSpawners.Length; i++)
            {
                Vector2Int arrayIndex = GridToArrayIndex(gridPositions[i]);
                RegisterSpawnerEdit(existingSpawners[i], arrayIndex);
            }

            // Row 오름차순, Col 오름차순 정렬 (예: (0,0),(0,1),(0,2),(1,0),(1,1),(1,2)...)
            var sorted = new List<Spawner>(existingSpawners);
            sorted.Sort((a, b) =>
            {
                int rowCompare = a.GridPosition.y.CompareTo(b.GridPosition.y);
                if (rowCompare != 0) return rowCompare;
                // Col 오름차순
                return a.GridPosition.x.CompareTo(b.GridPosition.x);
            });
            for (int i = 0; i < sorted.Count; i++)
            {
                sorted[i].transform.SetSiblingIndex(i);
                sorted[i].SetName();
            }
        }

        private void RegisterSpawnerEdit(Spawner spawner, Vector2Int gridPos)
        {
            spawner.InitEdit(gridPos.x, gridPos.y);
            _spawnerGrid[gridPos.x, gridPos.y] = spawner;
            _allSpawners.Add(spawner);
        }

        [ContextMenu("Set Tilemap Sprites")]
        public void SetTilemapSprites()
        {
            Spawner[] existingSpawners = tilemap.GetComponentsInChildren<Spawner>();
            foreach (var spawner in existingSpawners)
            {
                OwnerType ownerType = spawner.CurrentOwner;
                int col = spawner.GridPosition.x;
                int row = spawner.GridPosition.y;


                // (col + row) % 2 패턴으로 Sprite 선택
                int spriteIdx = (col + row) % tilemapSettings[0].tileSprites.Length;
                Dictionary<OwnerType, Sprite> sprites = new Dictionary<OwnerType, Sprite>();
                for (int i = 0; i < tilemapSettings.Length; i++)
                {
                    sprites.Add(tilemapSettings[i].ownerType, tilemapSettings[i].tileSprites[spriteIdx]);
                }


                spawner.SetSprite(sprites);
            }
        }
#endif
    }
}