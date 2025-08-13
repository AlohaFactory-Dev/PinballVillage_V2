using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using Stage.Building;
using System.Collections;
using UnityEngine;
using Zenject;

namespace _Scripts.Unit
{
    public class VillagerMoveSystem : MonoBehaviour
    {
        // ===== [DI] =====
        [Inject] private BuildingManager _buildingManager;
        [Inject] private SpawnerGridManager _spawnerGridManager;

        // ===== [Serialized Fields] =====
        [SerializeField] private Transform modelTransform;

        [BoxGroup("Howitzer Settings"), SerializeField]
        private AnimationCurve howitzerCurve;

        [BoxGroup("Howitzer Settings"), SerializeField]
        private float howitzerHeight;

        [SerializeField] private float jumpToCastleDuration = 0.5f;
        [SerializeField] private float afterJumpDelay = 1f;

        [Header("ex) 15도면 반사각 -15 ~ 15도 범위로 랜덤 회전")]
        [SerializeField]
        private float randomDirectionRange = 15f;

        [Header("Bounce Boost Settings")]
        [SerializeField]
        private float bounceBoostSpeed = 2f;

        [SerializeField] private float bounceBoostDuration = 0.5f;

        // ===== [Private Fields] =====
        private float _minSpeed = 1f;
        private float _constantSpeed;
        private float _currentSpeed;
        private Rigidbody2D _rb2D;
        private Collider2D _collider2D;
        private Vector2 _lastDirection;
        private OwnerType _ownerType;
        private bool _isJumping;
        private SkeletonMecanim _skeletonMecanim;
        private IEnumerator _boostCoroutine;
        private IEnumerator _bounceBoostCoroutine;
        private Villager _villager;

        // ===== [Public Methods] =====
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
            _minSpeed = _constantSpeed;
            RandomizeDirection();
        }

        public void BoostSpeedToDirection(float additionalSpeed, float duration, Direction direction)
        {
            _lastDirection = direction switch
            {
                Direction.Left => Vector2.left,
                Direction.Right => Vector2.right,
                Direction.Up => Vector2.up,
                Direction.Down => Vector2.down,
                _ => _lastDirection
            };
            if (_boostCoroutine != null)
                StopCoroutine(_boostCoroutine);

            _boostCoroutine = SpeedBoostCoroutine(additionalSpeed, duration);
            StartCoroutine(_boostCoroutine);
        }

        public void AdjustSpeed(float speedDelta)
        {
            float oldConstantSpeed = _constantSpeed;
            _constantSpeed = Mathf.Max(_minSpeed, _constantSpeed + speedDelta);

            if (_boostCoroutine != null)
            {
                float actualDelta = _constantSpeed - oldConstantSpeed;
                _currentSpeed = Mathf.Max(_minSpeed, _currentSpeed + actualDelta);
            }
            else
            {
                _currentSpeed = _constantSpeed;
            }

            if (_rb2D.velocity.magnitude > 0.1f)
                _rb2D.velocity = _lastDirection * _currentSpeed;
        }

        public void JumpNearTheCastle()
        {
            _isJumping = true;
            var sequence = DOTween.Sequence();
            _collider2D.enabled = false;
            _rb2D.velocity = Vector2.zero;
            _rb2D.isKinematic = true;
            if (_boostCoroutine != null)
                StopCoroutine(_boostCoroutine);

            var currentSpawner = _buildingManager.GetCastle(_ownerType);
            var emptySpawner = _spawnerGridManager.GetNearestEmptySpawner(currentSpawner, _ownerType);
            sequence.Append(transform.DOMove(emptySpawner.transform.position, jumpToCastleDuration));
            sequence.Join(modelTransform.DOLocalMoveY(howitzerHeight, jumpToCastleDuration).SetEase(howitzerCurve));
            sequence.AppendInterval(afterJumpDelay);
            sequence.OnComplete(() =>
            {
                _isJumping = false;
                _rb2D.isKinematic = false;
                _collider2D.enabled = true;
                _rb2D.velocity = Vector2.zero;
                RandomizeDirection();
            });
        }

        // ===== [Unity Events] =====
        private void FixedUpdate()
        {
            if (_isJumping) return;
            if (_rb2D.velocity.magnitude > 0.1f)
            {
                _lastDirection = _rb2D.velocity.normalized;
                _rb2D.velocity = _lastDirection * _currentSpeed;
                UpdateRotation(_lastDirection);
            }
            else
            {
                RandomizeDirection();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _lastDirection = CalculateBounceDirection(collision);
            _rb2D.velocity = _lastDirection * _currentSpeed;
            UpdateRotation(_lastDirection);

            var boostable = collision.transform.GetComponent<PushVillager>();
            if (boostable != null)
            {
                boostable.PerformAction(new ActionContext(_villager));
            }
            else
            {
                if (_boostCoroutine != null)
                    StopCoroutine(_boostCoroutine);

                _boostCoroutine = BounceBoostCoroutine();
                StartCoroutine(_boostCoroutine);
            }
        }

        // ===== [Private Methods] =====
        private void RandomizeDirection()
        {
            Vector2 randomDirection = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized;
            StartMovement(randomDirection);
        }

        private void StartMovement(Vector2 initialDirection)
        {
            _lastDirection = initialDirection.normalized;
            _rb2D.velocity = _lastDirection * _currentSpeed;
            UpdateRotation(_lastDirection);
        }

        private void UpdateRotation(Vector2 direction)
        {
            if (direction == Vector2.zero) return;
            float yRotation = direction.x >= 0 ? 0f : 180f;
            modelTransform.localEulerAngles = new Vector3(
                modelTransform.localEulerAngles.x,
                yRotation,
                modelTransform.localEulerAngles.z
            );
        }

        private Vector2 CalculateBounceDirection(Collision2D collision)
        {
            Vector2 normal = collision.contacts[0].normal;
            Vector2 incomingDirection = _lastDirection.normalized;
            Vector2 reflectedDirection = Vector2.Reflect(incomingDirection, normal);

            float randomAngle = Random.Range(-randomDirectionRange, randomDirectionRange);
            Quaternion rot = Quaternion.AngleAxis(randomAngle, Vector3.forward);
            Vector2 finalDirection = rot * reflectedDirection;

            float dot = Mathf.Abs(Vector2.Dot(finalDirection, normal));
            if (dot > 0.9f)
            {
                Vector2 perpendicular = new Vector2(-normal.y, normal.x);
                finalDirection = (finalDirection + perpendicular * 0.3f).normalized;
            }

            return finalDirection.normalized;
        }

        private IEnumerator SpeedBoostCoroutine(float additionalSpeed, float duration)
        {
            _currentSpeed = _constantSpeed + additionalSpeed;
            _rb2D.velocity = _lastDirection * _currentSpeed;
            yield return new WaitForSeconds(duration);
            _currentSpeed = _constantSpeed;
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
    }
}