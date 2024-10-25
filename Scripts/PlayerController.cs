using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{

    private Vector2 _velocity = new Vector2();
    private int _speed = 350;
    private int _gravity = 700;
    private int _jumpHeight = 400;
    private float _friction = .25f;
    private float _acceleration = 0.1f;
    private int _dashSpeed = 750;
    private float _dashTimer = .2f;
    private float _dashTimerReset = .2f;
    private bool _isDashing = false;
    private bool _isDashAvailable = true;
    private bool _isWallJumping = false;
    private float _wallJumpTimer = .5f;
    private float _wallJumpTimerReset = .5f;
    private bool _canClimb = true;
    private int _climbSpeed = 100;
    private float _climbTimer = 5f;
    private float _climbReset = 5f;
    private bool _isClimbing = false;


    public override void _Ready()
    {
        Velocity = Vector2.Zero;
        UpDirection = Vector2.Up;
    }

    public override void _Process(double delta)
    {
        
    }

    public override void _PhysicsProcess(double delta)
    {

        _isDashAvailable = true;

        if(!IsOnFloor()){
            // _velocity.Y += _gravity * (float)delta;         // Gravity
            _isDashAvailable = false;
        }

        if(!_isDashing && !_isWallJumping){
            _ProcessMovement((float)delta);
        }

        // Jumping
        if(IsOnFloor()){
            if(Input.IsActionJustPressed("jump")){
                _velocity.Y = -_jumpHeight;
            }else{
                _velocity.Y = 0;
            }
            _isDashAvailable = true;
            _canClimb = true;
        }

        if(_canClimb){
            _ProcessClimb((float)delta);
        }

        _ProcessWallJump((float)delta);

        if(_isDashAvailable){
            _ProcessDash((float)delta);
        }

        if(_isDashing){
            _dashTimer -= (float)delta;
            if(_dashTimer <= 0){
                _isDashing = false;
                _velocity = Vector2.Zero;
                _isDashAvailable = true;
            }
        }else{
            _velocity.Y += _gravity * (float)delta;         // Gravity
        }

        Velocity = _velocity;
        MoveAndSlide();
    }

    private void _ProcessMovement(float delta)
    {
        int direction = 0;
        if(Input.IsActionPressed("ui_left")){
            direction -= 1;
        }
        if(Input.IsActionPressed("ui_right")){
            direction += 1;
        }

        if(direction != 0){
            _velocity.X = Mathf.Lerp(_velocity.X, direction * _speed, _acceleration);
        }else{
            _velocity.X = Mathf.Lerp(_velocity.X, 0, _friction);
        }
    }

    private void _ProcessWallJump(float delta)
    {
        if(!IsOnFloor() && Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RayCastLeft").IsColliding()){
            _velocity.Y = -_jumpHeight;
            _velocity.X = _jumpHeight;
            _isWallJumping = true;
        }
        if(!IsOnFloor() && Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RayCastRight").IsColliding()){
            _velocity.Y = -_jumpHeight;
            _velocity.X = -_jumpHeight;
            _isWallJumping = true;
        }

        if(_isWallJumping){
            _wallJumpTimer -= delta;
            if(_wallJumpTimer <= 0){
                _isWallJumping = false;
                _wallJumpTimer = _wallJumpTimerReset;
            }
        }
    }

    private void _ProcessDash(float delta)
    {
        if(Input.IsActionJustPressed("dash")){
            if(Input.IsActionPressed("ui_left")){
                _velocity.X = -_dashSpeed;
                _velocity.Y = 0;
            }
            if(Input.IsActionPressed("ui_right")){
                _velocity.X = _dashSpeed;
                _velocity.Y = 0;
            }
            if(Input.IsActionPressed("ui_up")){
                _velocity.X = 0;
                _velocity.Y = -_dashSpeed;
            }
            if(Input.IsActionPressed("ui_right") && Input.IsActionPressed("ui_up")){
                _velocity.X = _dashSpeed;
                _velocity.Y = -_dashSpeed;
            }
            if(Input.IsActionPressed("ui_left") && Input.IsActionPressed("ui_up")){
                _velocity.X = -_dashSpeed;
                _velocity.Y = -_dashSpeed;
            }
            _dashTimer = _dashTimerReset;
            _isDashing = true;
            _isDashAvailable = false;
        }
    }

    private void _ProcessClimb(float delta)
    {
        if(Input.IsActionPressed("climb") && (GetNode<RayCast2D>("RayCastRightClimb").IsColliding() || GetNode<RayCast2D>("RayCastLeftClimb").IsColliding())){
            if(_canClimb){
                if(Input.IsActionPressed("ui_up")){
                    _velocity.Y = -_climbSpeed;
                    _isClimbing = true;
                }
                else if(Input.IsActionPressed("ui_down")){
                    _velocity.Y = _climbSpeed;
                    _isClimbing = true;
                }
                else {
                    _velocity = Vector2.Zero;
                }
            }
        }
        if(_isClimbing){
            _climbTimer -= delta;
            if(_climbTimer <= 0){
                _canClimb = false;
                _climbTimer = _climbReset;
            }
        }
    }

}
