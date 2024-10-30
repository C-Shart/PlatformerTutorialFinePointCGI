using Godot;
using GColl = Godot.Collections;
using System;

public partial class Enemy : Node2D
{
    private AnimatedSprite2D _sprite;
    private PlayerController _player;
    private CollisionObject2D _collisionObject;
    private bool _playerDetected;
    private bool _ableToShoot;
    private float _shootTimer = 1.1f;
    private float _shootTimerReset = 1f;
    private Marker2D _projectileSpawn;

    [Export] public PackedScene Projectile;

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _player = GetNode<PlayerController>("/root/Node2D/PlayerCeleste");
        _collisionObject = GetNode<CollisionObject2D>("Detection Radius");
        _projectileSpawn = GetNode<Marker2D>("ProjectileSpawn");
        _shootTimer = _shootTimerReset;
        _ableToShoot = false;
        _playerDetected = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        _sprite.FlipH = GlobalPosition.X - _player.GlobalPosition.X >= 0;

        HandleFiring((float)delta);
    }

    private void HandleFiring(float delta)
    {
        if(_playerDetected){
            if(_ableToShoot){
                PhysicsDirectSpaceState2D spaceState = GetWorld2D().DirectSpaceState;
                Rid _thisCollisionRid = _collisionObject.GetRid();
                PhysicsRayQueryParameters2D parameters = PhysicsRayQueryParameters2D.Create(Position, _player.Position, 6, new GColl.Array<Rid> {_thisCollisionRid});
                GColl.Dictionary result = spaceState.IntersectRay(parameters);
                if(result != null){
                    if(result.ContainsKey("collider")){
                        _projectileSpawn.LookAt(_player.Position);
                        if(result["collider"].AsGodotObject() == _player){
                            _sprite.Play("firing");
                            MSProjectile projectile = Projectile.Instantiate() as MSProjectile;
                            Owner.AddChild(projectile);
                            projectile.GlobalTransform = _projectileSpawn.GlobalTransform;
                            _ableToShoot = false;
                            _shootTimer = _shootTimerReset;
                        }
                    }
                }
            }
        }

        if(_shootTimer <= 0){
            _ableToShoot = true;
        }
        else{
            _shootTimer -= delta;
        }
    }

    private void OnDetectRadiusEnter(Node2D body)
    {
        if(body is PlayerController){
            _playerDetected = true;
            _player = body as PlayerController;

            // TOFIX: I want this to play before it starts firing
            _sprite.Play("detected");
        }
    }

    private void OnDetectRadiusExit(Node2D body)
    {
        if(body is PlayerController){
            _playerDetected = false;

            _sprite.Play("idle");
        }
    }
}
