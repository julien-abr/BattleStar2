using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using DoNotModify;
using UnityEngine.VFX;

namespace BattleStar {

	public class BattleStarController : BaseSpaceShipController
	{
		[SerializeField] private BehaviorTree behaviorTree;
		private GameData _gameData;
		
		//Other data
		private const float ShockwaveRadius = 2.2f;
		
		//Space ship data
		private int _spaceShipOwner;
		private int _spaceShipOwnerEnemy;
		private SpaceShipView _spaceShipEnemy;

		//Blackboard variables, [UpdateInput]
		private float _thrust = 1; 
		public float Thrust { get { return _thrust; }} 
		private float _targetOrient; 
		public float TargetOrient { get { return _targetOrient; } set { _targetOrient = value; } } 
		private bool _needShoot;
		private bool _dropMine; 
		public bool DropMine { get { return _dropMine; } set { _dropMine = value; } } 
		private bool _fireShockwave; 
		public bool FireShockwave { get { return _fireShockwave; } set { _fireShockwave = value; } } 
		
		//Blackboard variables, [Helpers]
		private Vector2 _nearestWaypoint; 
		public Vector2 NearestWaypoint { get { return _nearestWaypoint; } set { _nearestWaypoint = value; } } 
		private float _targetOrientToNearestWaypoint; 
		public float TargetOrientToNearestWaypoint { get { return _targetOrientToNearestWaypoint; } set { _targetOrientToNearestWaypoint = value; } } 
		
		private float _targetOrientToEnemy; 
		public float TargetOrientToEnemy { get { return _targetOrientToEnemy; } set { _targetOrientToEnemy = value; } } 
		
		//Blackboard variables, [GameData]
		public float TimeLeft { get { return _gameData.timeLeft; } } 
		public bool isWinning { get { return CheckScore.IsWining(_gameData, _spaceShipOwner, _spaceShipOwnerEnemy); }} 
		public bool CanChockwave {get { return CanShockwave.LaunchShockWave(_gameData, _spaceShipOwner, _spaceShipOwnerEnemy, ShockwaveRadius); }}
		public float DistanceBtwPlayers; 
		
		public bool HaveAllWaypoints { get { return HavingAllWaypoint.AllWaypoint(_gameData, _spaceShipOwner); } } 
		
		public float Energy { get { return _gameData.SpaceShips[_spaceShipOwner].Energy; }} 
		
		public float HitPenaltyCountdown { get { return _gameData.SpaceShips[_spaceShipOwner].HitPenaltyCountdown; }} 
		
		public float StunPenaltyCountdown { get { return _gameData.SpaceShips[_spaceShipOwner].StunPenaltyCountdown; }} 
		public float EnemyEnergy { get { return _gameData.SpaceShips[_spaceShipOwnerEnemy].Energy; }} 
		
		public float EnemyHitPenaltyCountdown { get { return _gameData.SpaceShips[_spaceShipOwnerEnemy].HitPenaltyCountdown; }} 
		
		public float EnemyStunPenaltyCountdown { get { return _gameData.SpaceShips[_spaceShipOwnerEnemy].StunPenaltyCountdown; }} 
		
		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
			_spaceShipOwner = spaceship.Owner;
			_spaceShipOwnerEnemy = _spaceShipOwner == 0 ? 1 : 0;
			_spaceShipEnemy = data.GetSpaceShipForOwner(_spaceShipOwnerEnemy);
			_gameData = data;
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			_targetOrientToNearestWaypoint = AimingHelpers.ComputeSteeringOrient(spaceship, NearestWaypoint);
			_targetOrientToEnemy = AimingHelpers.ComputeSteeringOrient(spaceship, data.GetSpaceShipForOwner(_spaceShipOwnerEnemy).Position);
			_nearestWaypoint = FindNearestWayPointHelper.actualNearestWaypoint(_gameData, spaceship, _spaceShipOwner);
			_needShoot = AimingHelpers.CanHit(spaceship, _spaceShipEnemy.Position, _spaceShipEnemy.Velocity, 0.15f);
			return new InputData(_thrust, _targetOrient, _needShoot, _dropMine, _fireShockwave);
		}
	}

}
