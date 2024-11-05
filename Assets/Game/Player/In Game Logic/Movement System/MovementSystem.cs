using Confront.Stage;
using System;
using UnityEngine;

namespace Confront.Physics
{
    public class MovementSystem
    {
        private CharacterController _controller;
        private MovementSettings _settings;
        private GroundSensor _groundSensor;

        private GroundSensorResult _groundSensorResult;

        private Vector3 _velocity;

        private float _jumpTimeout = 0f;
        private bool IsJumping => _jumpTimeout > 0f;
        private bool _isAscendingDueToJump = false;

        private GroundState _previousGroundState;
        private GroundState _groundState;

        public Vector3 Velocity => _velocity;
        public float MaxSpeed => _settings._maxSpeed;
        public GroundState GroundState => _groundState;

        public MovementSystem(CharacterController controller, MovementSettings settings, GroundSensor groundSensor)
        {
            _controller = controller;
            _settings = settings;
            _groundSensor = groundSensor;
        }

        public void Jump(float jumpForce)
        {
            _velocity.y = jumpForce;
            _jumpTimeout = _settings._jumpTimeoutDelta;
            _isAscendingDueToJump = true;
        }

        private float _inputX;

        public void Update(float inputX)
        {
            _inputX = inputX;
            _jumpTimeout -= Time.deltaTime;
            _groundSensorResult = _groundSensor.CheckGround(_controller.transform.position, Vector2.down, _controller.slopeLimit, _isAscendingDueToJump);
            _previousGroundState = _groundState;
            _groundState = _groundSensorResult.GroundState;

            if (_velocity.y < 0f) _isAscendingDueToJump = false;

            UpdateVelocity();
            ApplyVelocity();
        }

        private void UpdateVelocity()
        {
            switch (_groundState)
            {
                case GroundState.Grounded:
                    UpdateGroundedVelocity();
                    break;
                case GroundState.Abyss:
                    UpdateAbyssVelocity();
                    break;
                case GroundState.SteepSlope:
                    UpdateSteepSlopeVelocity();
                    break;
                case GroundState.InAir:
                    UpdateInAirVelocity();
                    break;
            }
        }

        private float CalculateDirection(float direction)
        {
            if (Mathf.Abs(direction) < 0.01f) return 0;
            return Mathf.Sign(direction);
        }

        private void UpdateGroundedVelocity()
        {
            if (_previousGroundState == GroundState.InAir)
            {
                _velocity.y = 0f;
            }

            // 入力に応じてx速度を更新する。
            var acceleration = _settings._acceleration;
            var deceleration = _settings._deceleration;
            var inputDirection = CalculateDirection(_inputX);

            var isInputZero = inputDirection == 0f;
            var isTurning = IsTurning(inputDirection);
            var velocitySign = Mathf.Sign(_velocity.x);

            float velocity = _velocity.magnitude * velocitySign;

            if (isInputZero)
            {
                velocity = Mathf.Lerp(velocity, 0f, deceleration * Time.deltaTime);
            }
            else if (isTurning)
            {
                velocity = Mathf.Lerp(velocity, 0f, _settings._turnDeceleration * Time.deltaTime);
            }
            else
            {
                velocity = Mathf.Lerp(velocity, _settings._maxSpeed * inputDirection, acceleration * Time.deltaTime);
            }

            if (!IsJumping) // ジャンプしていない場合は、地面の法線に合わせて横移動する。ジャンプ中に下記の処理を行うと、y軸方向の速度が上書きされるのでジャンプ中は処理をスキップする。
            {
                var groundNormal = _groundSensorResult.GroundNormal;
                _velocity = Vector3.ProjectOnPlane(new Vector3(velocity, 0f), groundNormal).normalized * Mathf.Abs(velocity);
            }
        }

        private bool IsTurning(float inputDirection)
        {
            return (inputDirection > 0.1f && _velocity.x < -0.1f) || (inputDirection < -0.1f && _velocity.x > 0.1f);
        }

        private void UpdateAbyssVelocity()
        {
            // ななめ移動（崖から滑落する。）
            // もし、x軸方向の速度が滑落側と反対方向の場合、x軸方向の速度をゼロにする。
            var fallDirection = Mathf.Sign(_groundSensorResult.GroundNormal.x);
            var velocityDirection = Mathf.Sign(_velocity.x);
            var acceleration = _settings._abyssGravity;

            var velocity = _velocity.magnitude;
            if (Mathf.Abs(_groundSensorResult.GroundNormal.x) >= 0.001f ||
                Mathf.Abs(_velocity.x) >= 0.001f)
            {
                if (fallDirection != velocityDirection)
                {
                    _velocity.x = 0f;
                }
            }

            _velocity += new Vector3(fallDirection, -1f) * acceleration * Time.deltaTime;
        }

        private void UpdateSteepSlopeVelocity()
        {
            // 斜面の角度に合わせて滑落する。
            // もし、前フレームで落下していた場合、落下速度を維持する。
            var groundNormal = _groundSensorResult.GroundNormal;
            var downhillDirection = Vector3.Cross(Vector3.Cross(Vector3.up, groundNormal), groundNormal).normalized;
            var acceleration = _settings._slopeAcceleration;

            if (_previousGroundState != GroundState.SteepSlope)
            {
                if (_velocity.y < 2f)
                {
                    var _fallSpeed = _velocity.y;
                    _velocity = downhillDirection * _fallSpeed;
                }
            }

            if (!IsJumping)
            {
                var velocity = _velocity.magnitude;
                velocity += acceleration * Time.deltaTime;
                _velocity = downhillDirection * velocity;
            }
        }

        private void UpdateInAirVelocity()
        {
            // 空中での移動。
            // 重力を適用する。
            // 入力に応じて横移動を行う。
            var acceleration = _settings._inAirAcceleration;
            var deceleration = _settings._inAirDeceleration;
            var maxSpeed = _settings._inAirMaxSpeed;
            var gravity = _settings._gravity;
            var isInputZero = Mathf.Abs(_inputX) < 0.01f;
            var isTurning = IsTurning(CalculateDirection(_inputX));

            if (isInputZero)
            {
                _velocity.x = Mathf.MoveTowards(_velocity.x, 0f, deceleration * Time.deltaTime);
            }
            else if (isTurning)
            {
                _velocity.x = 0f;
            }
            else
            {
                var inputDirection = Mathf.Sign(_inputX);
                _velocity.x = Mathf.MoveTowards(_velocity.x, maxSpeed * inputDirection, acceleration * Time.deltaTime);
            }

            _velocity.y -= gravity * Time.deltaTime;
        }

        private void ApplyVelocity()
        {
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}