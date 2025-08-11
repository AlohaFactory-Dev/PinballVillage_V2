using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Stage.Building;
using UnityEngine;
using Zenject;

namespace FactorySystem
{
    public class StageManager : MonoBehaviour
    {
        [Inject] private SpawnerGridManager _spawnerGridManager;


        private void Start()
        {
            Time.timeScale = 1f;
        }

        public void StageResult()
        {
            StageResult(_spawnerGridManager.Winner);
        }

        public void StageResult(OwnerType winner)
        {
            Time.timeScale = 0f; // 게임 일시 정지
            StageContainer.Get<StageUI>().OnStageResult(winner);
        }
    }
}