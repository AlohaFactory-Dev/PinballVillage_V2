using UnityEngine;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using Stage.Building;
using Zenject;

namespace _Scripts.Unit
{
    public class VillagerMoveSystem : MonoBehaviour
    {
        [Inject] private BuildingManager _buildingManager;
        [Inject] private SpawnerGridManager _spawnerGridManager;
        [SerializeField] Transform modelTransform; // 모델 트랜스폼

        [BoxGroup("Howitzer Settings")]
        [SerializeField]
        private AnimationCurve howitzerCurve;

        [BoxGroup("Howitzer Settings")]
        [SerializeField]
        private float howitzerHeight;

        [SerializeField] private float jumpToCastleDuration = 0.5f; // 점프 속도
        [SerializeField] private float afterJumpDelay = 1f; // 점프 후 딜레이

        [Header("ex) 15도면 반사각 -15 ~ 15도 범위로 랜덤 회전")]
        [SerializeField]
        private float randomDirectionRange = 15f; // 초기 방향 랜덤 범위

        [Header("Bounce Boost Settings")]
        [SerializeField]
        private float bounceBoostSpeed = 2f; // 충돌 시 추가되는 속도

        [SerializeField] private float bounceBoostDuration = 0.5f; // 추가 속도에서 원래 속도로 복귀하는 시간

        private float _minSpeed = 1f; // 최소 속도 제한

        private float _constantSpeed;
        private Rigidbody2D _rb2D;

        private Vector2 _lastDirection; // 충돌 직전의 속도 저장

        // 스피드 부스트 관련 변수
        private float _currentSpeed;
        private IEnumerator _boostCoroutine;
        private Collider2D _collider2D;
        private OwnerType _ownerType;
        private bool _isJumping;
        private SkeletonMecanim _skeletonMecanim;

        private IEnumerator _bounceBoostCoroutine;
        private Villager _villager;

        public void Init(Villager villager, float moveSpeed)
        {
            _villager = villager;
            _ownerType = _villager.OwnerType;
            _rb2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            _rb2D.gravityScale = 0f;
            _rb2D.drag = 0f;
            _rb2D.angularDrag = 0f;
            _constantSpeed = moveSpeed / 10f;
            _currentSpeed = _constantSpeed;
            RandomizeDirection();
            _minSpeed = _constantSpeed;
        }

        private void FixedUpdate()
        {
            if (_isJumping) return; // 점프 또는 부스트 중이면 FixedUpdate에서 velocity를 덮어쓰지 않음
            // 속도가 있는 경우 일정한 속도로 유지
            if (_rb2D.velocity.magnitude > 0.1f) // 매우 작은 속도는 정지로 간주
            {
                // 현재 속도 방향을 _lastDirection으로 업데이트
                _lastDirection = _rb2D.velocity.normalized;

                // 일정한 속도로 설정
                _rb2D.velocity = _lastDirection * _currentSpeed;
                UpdateRotation(_lastDirection);
            }
            else
            {
                RandomizeDirection();
            }
        }

        public void BoostSpeedToDirection(float additionalSpeed, float duration, Direction direction)
        {
            _lastDirection = direction switch
            {
                Direction.Left => Vector2.left,
                Direction.Right => Vector2.right,
                Direction.Up => Vector2.up,
                Direction.Down => Vector2.down,
                _ => _lastDirection // 기본적으로 이전 방향 유지
            };
            if (_boostCoroutine != null)
            {
                StopCoroutine(_boostCoroutine);
            }

            _boostCoroutine = SpeedBoostCoroutine(additionalSpeed, duration);
            StartCoroutine(_boostCoroutine);
        }

        private IEnumerator SpeedBoostCoroutine(float additionalSpeed, float duration)
        {
            _currentSpeed = _constantSpeed + additionalSpeed;
            _rb2D.velocity = _lastDirection * _currentSpeed;
            yield return new WaitForSeconds(duration);
            _currentSpeed = _constantSpeed;
        }

        private void RandomizeDirection()
        {
            // 초기 방향을 랜덤하게 설정
            Vector2 randomDirection = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized;

            StartMovement(randomDirection);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _lastDirection = CalculateBounceDirection(collision);

            // velocity를 즉시 설정하여 새로운 방향으로 이동
            _rb2D.velocity = _lastDirection * _currentSpeed;

            UpdateRotation(_lastDirection);

            var boostable = collision.transform.GetComponent<PushVillager>();
            if (boostable != null)
            {
                boostable.PerformAction(_villager, 0, Building.CalculateType.Add);
            }
            else
            {
                if (_boostCoroutine != null)
                {
                    StopCoroutine(_boostCoroutine);
                }

                _boostCoroutine = BounceBoostCoroutine();
                StartCoroutine(_boostCoroutine);
            }
        }

        private IEnumerator BounceBoostCoroutine()
        {
            float boostedSpeed = bounceBoostSpeed / 10f;
            float elapsed = 0f;

            float startSpeed = _constantSpeed + boostedSpeed;

            while (elapsed < bounceBoostDuration)
            {
                yield return new WaitForFixedUpdate();
                elapsed += Time.fixedDeltaTime;
                float t = Mathf.Clamp01(elapsed / bounceBoostDuration);
                _currentSpeed = Mathf.Lerp(startSpeed, _constantSpeed, t);
            }
        }

        private Vector2 CalculateBounceDirection(Collision2D collision)
        {
            Vector2 normal = collision.contacts[0].normal;
            Vector2 incomingDirection = _lastDirection.normalized;

            // 반사 방향 계산
            Vector2 reflectedDirection = Vector2.Reflect(incomingDirection, normal);

            // 랜덤 각도 적용 (더 자연스럽게)
            float randomAngle = Random.Range(-randomDirectionRange, randomDirectionRange);
            Quaternion rot = Quaternion.AngleAxis(randomAngle, Vector3.forward);
            Vector2 finalDirection = rot * reflectedDirection;

            // 반사 방향이 법선과 너무 평행하면, 법선에 수직인 방향으로 약간 섞어줌
            float dot = Mathf.Abs(Vector2.Dot(finalDirection, normal));
            if (dot > 0.9f)
            {
                Vector2 perpendicular = new Vector2(-normal.y, normal.x);
                finalDirection = (finalDirection + perpendicular * 0.3f).normalized;
            }

            return finalDirection.normalized;
        }

        private void StartMovement(Vector2 initialDirection)
        {
            _lastDirection = initialDirection.normalized;

            // velocity를 직접 설정하여 즉시 이동 시작
            _rb2D.velocity = _lastDirection * _currentSpeed;

            // 초기 방향에 맞게 회전
            UpdateRotation(_lastDirection);
        }

        public void StopMovement()
        {
            _rb2D.velocity = Vector2.zero;
        }

        private void UpdateRotation(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                float yRotation = direction.x >= 0 ? 0f : 180f;
                modelTransform.localEulerAngles = new Vector3(
                    modelTransform.localEulerAngles.x,
                    yRotation,
                    modelTransform.localEulerAngles.z
                );
            }
        }

        public void AdjustSpeed(float speedDelta)
        {
            float oldConstantSpeed = _constantSpeed;
            _constantSpeed = Mathf.Max(_minSpeed, _constantSpeed + speedDelta);

            // 부스트 중일 때도 현재 속도를 조정
            if (_boostCoroutine != null)
            {
                // 기본 속도 변화량만큼 현재 속도도 조정
                float actualDelta = _constantSpeed - oldConstantSpeed;
                _currentSpeed = Mathf.Max(_minSpeed, _currentSpeed + actualDelta);
            }
            else
            {
                _currentSpeed = _constantSpeed;
            }

            // 현재 이동 중이면 새로운 속도로 즉시 적용
            if (_rb2D.velocity.magnitude > 0.1f)
            {
                _rb2D.velocity = _lastDirection * _currentSpeed;
            }
        }

        public void JumpNearTheCastle()
        {
            _isJumping = true;
            var sequence = DOTween.Sequence();
            _collider2D.enabled = false; // 충돌을 일시적으로 비활성화하여 점프 중 충돌 방지
            _rb2D.velocity = Vector2.zero; // 점프 전 속도 초기화
            _rb2D.isKinematic = true; // 점프 중 물리 엔진의 영향을 받지 않도록 설정
            if (_boostCoroutine != null) // 스피드 부스트가 진행 중이면 중지
            {
                StopCoroutine(_boostCoroutine);
            }


            var currentSpawner = _buildingManager.GetCastle(_ownerType);
            var emptySpawner = _spawnerGridManager.GetNearestEmptySpawner(currentSpawner, _ownerType);
            sequence.Append(transform.DOMove(emptySpawner.transform.position, jumpToCastleDuration));
            sequence.Join(modelTransform.DOLocalMoveY(howitzerHeight, jumpToCastleDuration).SetEase(howitzerCurve));
            sequence.AppendInterval(afterJumpDelay);
            sequence.OnComplete(() =>
            {
                _isJumping = false;
                _rb2D.isKinematic = false;
                _collider2D.enabled = true; // 점프 후 충돌 활성화
                _rb2D.velocity = Vector2.zero; // 점프 후 속도 초기화
                RandomizeDirection(); // 점프 후 새로운 방향으로 이동 시작
            });
        }
    }
}