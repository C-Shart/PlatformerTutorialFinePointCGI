using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{

    private Vector2 _velocity = new Vector2();
    private int _speed = 250;
    private int _gravity = 700;
    private int _jumpHeight = 400;
    private float _friction = .25f;
    private float _acceleration = 0.1f;
    private int _dashSpeed = 600;
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
    private int _direction = 0;
    private bool _isTakingDamage = false;

    [Export] public PackedScene GhostPlayerInstance;

    private AnimatedSprite2D _animationNode;

    public override void _Ready()
    {
        Velocity = Vector2.Zero;
        UpDirection = Vector2.Up;

        _animationNode = GetNode<AnimatedSprite2D>("Sprite");
        _isTakingDamage = false;
    }

    public override void _Process(double delta)
    {
        
    }

    public override void _PhysicsProcess(double delta)
    {
        if(!_isDashing && !_isWallJumping){
            _ProcessMovement((float)delta);
        }

        // Jumping
        if(IsOnFloor()){
            if(Input.IsActionPressed("jump")){
                _animationNode.Play("jump pre");
                // _velocity.X = Mathf.Lerp(_velocity.X, 0, _friction);
                _velocity.X = 0;
            }
            else if(Input.IsActionJustReleased("jump")){
                _velocity.Y = -_jumpHeight;
                _animationNode.Play("jumping");
            }
            else{
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
            GhostPlayer ghost = GhostPlayerInstance.Instantiate() as GhostPlayer;
            Owner.AddChild(ghost);
            ghost.GlobalPosition = this.GlobalPosition;
            ghost.SetHValue(_animationNode.FlipH);

            if(_dashTimer <= 0){
                _isDashing = false;
                _velocity = Vector2.Zero;
                if(!IsOnFloor()){
                    _isDashAvailable = false;
                }
            }
        }else{
            _velocity.Y += _gravity * (float)delta;         // Gravity
        }

        Velocity = _velocity;
        MoveAndSlide();
    }

    private void _ProcessMovement(float delta)
    {
        int _direction = 0;

        if(!_isTakingDamage){
            if(Input.IsActionPressed("ui_left")){
                _direction -= 1;
            }
            if(Input.IsActionPressed("ui_right")){
                _direction += 1;
            }

            if(_direction != 0){
                _velocity.X = Mathf.Lerp(_velocity.X, _direction * _speed, _acceleration);
                if(IsOnFloor()){
                    _animationNode.Play("walk");
                    _animationNode.FlipH = _velocity.X < 0;
                }
            }else{
                _velocity.X = Mathf.Lerp(_velocity.X, 0, _friction);
                _animationNode.Play("idle");
            }
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
            _animationNode.Play("jumping");
            _animationNode.FlipH = _velocity.X < 0;
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
            _animationNode.Play("dash");
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

    // Spikes test
    private void OnHitboxEntered(Node2D body)
    {
        if(body is TileMap){
            _isTakingDamage = true;
            GameController.Instance.HeartLost(1);
            _velocity.X *= -2;
            _velocity.Y = -300;
        }
    }

    private void OnHitboxExited(Node2D body)
    {
        if(body is TileMap){
            _isTakingDamage = false;
        }
    }

}
