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
		private float _minDistanceFromPlayer = ShockwaveRadius;
		private int _currentNumberOfWaypoint;
		private int _numberOfWaypointBeforeMine;
		public bool CanOrientToEnemy { get { return DistanceBtwPlayers > _minDistanceFromPlayer; } }
		
		//Space ship data
		private int _spaceShipOwner;
		private int _spaceShipOwnerEnemy;
		private SpaceShipView _spaceShip;
		private SpaceShipView _spaceShipEnemy;

		//Blackboard variables, [UpdateInput]
		private float _thrust = 1; 
		public float Thrust { get { return _thrust; }} 
		private float _targetOrient; 
		public float TargetOrient { get { return _targetOrient; } set { _targetOrient = value; } }
		private bool _canShoot;
		private bool _needShoot;
		public bool NeedShoot { get { return _needShoot; } set { _needShoot = value; } } 
		private bool _dropMine; 
		private bool _canDropMine; 
		public bool CanDropMine { get { return _canDropMine; } set { _canDropMine = value; } } 
		private bool _fireShockwave; 
		public bool FireShockwave { get { return _fireShockwave; } set { _fireShockwave = value; } } 
		
		//Blackboard variables, [Helpers]
		public Vector2 NearestWaypointEnemy { get { return PositionHelper.FindNearestWaypointEnemy(_gameData, _spaceShip, _spaceShipOwnerEnemy); }}
		public Vector2 NearestWaypointNeutral { get { return PositionHelper.FindNearestWaypointNeutral(_gameData, _spaceShip); } }

		private float _targetOrientToNearestWaypointEnemy; 
		public float TargetOrientToNearestWaypointEnemy { get { return _targetOrientToNearestWaypointEnemy; } set { _targetOrientToNearestWaypointEnemy = value; } } 
		
		private float _targetOrientToNearestWaypointNeutral; 
		public float TargetOrientToNearestWaypointNeutral { get { return _targetOrientToNearestWaypointNeutral; } set { _targetOrientToNearestWaypointNeutral = value; } } 
		private float _targetOrientToEnemy; 
		public float TargetOrientToEnemy { get { return _targetOrientToEnemy; } set { _targetOrientToEnemy = value; } } 
		
		//Blackboard variables, [GameData]
		public int WaypointsBeforeMine { get { return _numberOfWaypointBeforeMine;  } }

		public float TimeLeft { get { return _gameData.timeLeft; } } 
		public bool isWinning { get { return ScoreHelper.IsWining(_gameData, _spaceShipOwner, _spaceShipOwnerEnemy); }} 
		public bool CanChockwave {get { return CanPerformActionHelper.CanShockWave(_gameData, _spaceShipOwner, _spaceShipOwnerEnemy, ShockwaveRadius); }}
		public float DistanceBtwPlayers {get { return PositionHelper.DistanceBtwPlayers(_gameData, _spaceShipOwner, _spaceShipOwnerEnemy); }}
		public bool HaveAllWaypoints { get { return WaypointHelper.HavingAllWaypoint(_gameData, _spaceShipOwner); } }
		public bool EnemyHasWaypoint { get { return WaypointHelper.EnemyHasWaypoint(_gameData, _spaceShipOwnerEnemy); } }
		public bool NeutralWaypointExist { get { return WaypointHelper.NeutralWaypointExist(_gameData, _spaceShipOwner, _spaceShipOwnerEnemy); } }
		public float MineEnergyCost { get { return _gameData.SpaceShips[_spaceShipOwner].MineEnergyCost; } }
		public float ShootEnergyCost { get { return _gameData.SpaceShips[_spaceShipOwner].ShootEnergyCost; } }
		public float ShockwaveEnergyCost { get { return _gameData.SpaceShips[_spaceShipOwner].ShockwaveEnergyCost; } }
		public float Energy { get { return _gameData.SpaceShips[_spaceShipOwner].Energy; }}
		public float HitPenaltyCountdown { get { return _gameData.SpaceShips[_spaceShipOwner].HitPenaltyCountdown; }}
		public float StunPenaltyCountdown { get { return _gameData.SpaceShips[_spaceShipOwner].StunPenaltyCountdown; }} 
		public float EnemyEnergy { get { return _gameData.SpaceShips[_spaceShipOwnerEnemy].Energy; }}
		public float EnemyHitPenaltyCountdown { get { return _gameData.SpaceShips[_spaceShipOwnerEnemy].HitPenaltyCountdown; }}
		public float EnemyStunPenaltyCountdown { get { return _gameData.SpaceShips[_spaceShipOwnerEnemy].StunPenaltyCountdown; }} 
		
		#region function
		public void OnValueChangedWaypoints()
		{
			var waypoints = _gameData.SpaceShips[_spaceShipOwner].WaypointScore;
			if (waypoints < _currentNumberOfWaypoint)
			{
				_numberOfWaypointBeforeMine++;
			}
			_currentNumberOfWaypoint = waypoints;
		}
		
		#endregion
		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
			_spaceShipOwner = spaceship.Owner;
			_spaceShipOwnerEnemy = _spaceShipOwner == 0 ? 1 : 0;
			_spaceShip = spaceship;
			_spaceShipEnemy = data.GetSpaceShipForOwner(_spaceShipOwnerEnemy);
			_gameData = data;
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			CanPerformActionHelper.UpdateMineCounter(_gameData, _spaceShipOwner, _currentNumberOfWaypoint, this);
			_targetOrientToNearestWaypointEnemy = AimingHelpers.ComputeSteeringOrient(spaceship, NearestWaypointEnemy);
			_targetOrientToNearestWaypointNeutral = AimingHelpers.ComputeSteeringOrient(spaceship, NearestWaypointNeutral);
			_targetOrientToEnemy = AimingHelpers.ComputeSteeringOrient(spaceship, data.GetSpaceShipForOwner(_spaceShipOwnerEnemy).Position);
			_canShoot = AimingHelpers.CanHit(spaceship, _spaceShipEnemy.Position, _spaceShipEnemy.Velocity, 0.15f);
			bool shootResult =_canShoot && _needShoot;
			bool mineResult = _dropMine && CanDropMine;
	
			return new InputData(_thrust, _targetOrient, shootResult, mineResult, _fireShockwave);
		}
		
		
	}
}