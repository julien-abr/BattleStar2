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
		private int _spaceShipOwner;
		private int _spaceShipOwnerEnemy;
		private SpaceShipView _spaceShipEnemy;

		//Blackboard variables, [UpdateInput]
		private float _thrust; 
		public float Thrust { get { return _thrust; } set { _thrust = value; } } 
		private float _targetOrient; 
		public float TargetOrient { get { return _thrust; } set { _thrust = value; } } 
		private bool _needShoot; 
		public bool NeedShoot { get { return _needShoot; } set { _needShoot = value; } } 
		private bool _dropMine; 
		public bool DropMine { get { return _dropMine; } set { _dropMine = value; } } 
		private bool _fireShockwave; 
		public bool FireShockwave { get { return _fireShockwave; } set { _fireShockwave = value; } } 
		
		//Blackboard variables, [Helpers]
		private Vector2 _nearestWaypoint; 
		public Vector2 NearestWaypoint { get { return _nearestWaypoint; } set { _nearestWaypoint = value; } } 
		private float _targetOrientToNearestWaypoint; 
		public float TargetOrientToNearestWaypoint { get { return _targetOrientToNearestWaypoint; } set { _targetOrientToNearestWaypoint = value; } } 
		
		//Blackboard variables, [GameData]
		public float TimeLeft { get { return _gameData.timeLeft; } } 
		public int Score { get { return _gameData.SpaceShips[_spaceShipOwner].Score; }} 
		
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
			_gameData = data;
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			_spaceShipEnemy = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			_targetOrientToNearestWaypoint = AimingHelpers.ComputeSteeringOrient(spaceship, NearestWaypoint);
			//float thrust = 1.0f;
			//float targetOrient = spaceship.Orientation + 90.0f;
			_needShoot = AimingHelpers.CanHit(spaceship, _spaceShipEnemy.Position, _spaceShipEnemy.Velocity, 0.15f);
			return new InputData(_thrust, _targetOrient, _needShoot, _dropMine, _fireShockwave);
		}
	}

}
