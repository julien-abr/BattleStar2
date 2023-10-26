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
		public float Thrust => _thrust;  
		private float _targetOrient; 
		public float TargetOrient { get { return _targetOrient; } set { _targetOrient = value; } }
		private bool _canShoot;
		private bool _needShoot;
		public bool NeedShoot { get { return _needShoot; } set { _needShoot = value; } } 
		private bool _dropMine;
		public bool CanDropMine { get { return _dropMine; } set { _dropMine = value; } } 
		private bool _fireShockwave; 
		public bool FireShockwave { get { return _fireShockwave; } set { _fireShockwave = value; } } 
		
		//Blackboard variables, [Helpers]
		public bool MineDetected => DetectMine.MineDetectedBox(_gameData, _spaceShipOwner, this);
		public Vector2 NearestWaypointEnemy => PositionHelper.FindNearestWaypointEnemy(_gameData, _spaceShip, _spaceShipOwnerEnemy);
		public Vector2 NearestWaypointNeutral => PositionHelper.FindNearestWaypointNeutral(_gameData, _spaceShip);

		private Vector2 _nearestMine;
		public Vector2 NearestMine => _nearestMine; 

		private float _targetOrientToNearestWaypointEnemy; 
		public float TargetOrientToNearestWaypointEnemy { get { return _targetOrientToNearestWaypointEnemy; } set { _targetOrientToNearestWaypointEnemy = value; } } 
		
		private float _targetOrientToNearestWaypointNeutral; 
		public float TargetOrientToNearestWaypointNeutral { get { return _targetOrientToNearestWaypointNeutral; } set { _targetOrientToNearestWaypointNeutral = value; } } 
		private float _targetOrientToEnemy; 
		public float TargetOrientToEnemy { get { return _targetOrientToEnemy; } set { _targetOrientToEnemy = value; } } 
		
		//Blackboard variables, [GameData]
		public int WaypointsBeforeMine => _numberOfWaypointBeforeMine;  
		public float TimeLeft => _gameData.timeLeft;  
		public bool isWinning => ScoreHelper.IsWining(_gameData, _spaceShipOwner, _spaceShipOwnerEnemy);
		public bool CanChockwave => CanPerformActionHelper.CanShockWave(_gameData, _spaceShipOwner, _spaceShipOwnerEnemy, ShockwaveRadius); 
		public float DistanceBtwPlayers => PositionHelper.DistanceBtwPlayers(_gameData, _spaceShipOwner, _spaceShipOwnerEnemy);
		public bool HaveAllWaypoints => WaypointHelper.HavingAllWaypoint(_gameData, _spaceShipOwner);
		public bool EnemyHasWaypoint => WaypointHelper.EnemyHasWaypoint(_gameData, _spaceShipOwnerEnemy);
		public bool NeutralWaypointExist => WaypointHelper.NeutralWaypointExist(_gameData, _spaceShipOwner, _spaceShipOwnerEnemy);
		public float MineEnergyCost =>_gameData.SpaceShips[_spaceShipOwner].MineEnergyCost;
		public float ShootEnergyCost => _gameData.SpaceShips[_spaceShipOwner].ShootEnergyCost;
		public float ShockwaveEnergyCost => _gameData.SpaceShips[_spaceShipOwner].ShockwaveEnergyCost;
		public float Energy => _gameData.SpaceShips[_spaceShipOwner].Energy;
		public float HitPenaltyCountdown => _gameData.SpaceShips[_spaceShipOwner].HitPenaltyCountdown; 
		public float StunPenaltyCountdown => _gameData.SpaceShips[_spaceShipOwner].StunPenaltyCountdown;
		public int Waypoints => _gameData.SpaceShips[_spaceShipOwner].WaypointScore;
		public int Score => _gameData.SpaceShips[_spaceShipOwner].Score;
		public int HitScore => _gameData.SpaceShips[_spaceShipOwner].HitScore;
		public float EnemyEnergy =>_gameData.SpaceShips[_spaceShipOwnerEnemy].Energy;
		public float EnemyHitPenaltyCountdown =>_gameData.SpaceShips[_spaceShipOwnerEnemy].HitPenaltyCountdown;
		public float EnemyStunPenaltyCountdown =>_gameData.SpaceShips[_spaceShipOwnerEnemy].StunPenaltyCountdown;
		public int EnemyWaypoints => _gameData.SpaceShips[_spaceShipOwnerEnemy].WaypointScore;
		public int EnemyScore => _gameData.SpaceShips[_spaceShipOwnerEnemy].Score;
		public int EnemyHitScore => _gameData.SpaceShips[_spaceShipOwnerEnemy].HitScore;

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

		public void SetNearestMine(Vector2 minePos) { _nearestMine = minePos; }
		
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
			bool shootResult =_canShoot && _needShoot || NeedShoot && MineDetected;

			return new InputData(_thrust, _targetOrient, shootResult, _dropMine, _fireShockwave);
		}
		
		
	}
}