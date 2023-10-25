﻿using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using DoNotModify;

namespace BattleStar {

	public class BattleStarController : BaseSpaceShipController
	{
		[SerializeField] private BehaviorTree behaviorTree;
		public float Thrust;
		public float Thrust2;
		
		[SerializeField] private float m_Float;
		public float Float { get { return m_Float; } set { m_Float = value; } } 
		public override void Initialize(SpaceShipView spaceship, GameData data)
		{ 
			var thrust = (SharedFloat)behaviorTree.GetVariable("Test2");
			Thrust2 = thrust.Value;
			//var thrust2 = (SharedFloat)behaviorTree.SetVariable("Thrust", );
			//GameManager.Instance.GetGameData().WayPoints[0].Position;
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			float thrust = 1.0f;
			float targetOrient = spaceship.Orientation + 90.0f;
			bool needShoot = AimingHelpers.CanHit(spaceship, otherSpaceship.Position, otherSpaceship.Velocity, 0.15f);
			return new InputData(thrust, targetOrient, needShoot, false, false);
		}
	}

}
