using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.Localization;
using Kingmaker.Pathfinding;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.Utility;
using Kingmaker.View;
using Owlcat.Runtime.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBased.Controllers;
using UnityEngine;

namespace DragonLibrary.NewComponents
{
    [AllowedOn(typeof(BlueprintAbility), false)]
    [TypeId("F6CABA0C-7B39-44D7-BC74-D9CBF0753E6C")]
    [Serializable]
    public class ChargingThrow : AbilityCustomLogic, IAbilityTargetRestriction, IAbilityMinRangeProvider
    {
        static readonly LocalizedString FailReasonWeapon = new() { m_Key = "Chargin Hurler Fail Wrong Weapon" };
        static readonly LocalizedString FailReasonMount = new() { m_Key = "Chargin Hurler Fail Must Run" };
        public override void Cleanup(AbilityExecutionContext context)
        {
            context.Caster.View.AgentASP.IsCharging = false;
            context.Caster.View.AgentASP.MaxSpeedOverride = null;
            context.Caster.Descriptor.State.IsCharging = false;
        }

        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper targetWrapper)
        {
            UnitEntityData target = targetWrapper.Unit;
            if (target == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                yield break;
            }
            UnitEntityData caster = context.Caster;
            var position = caster.Position;
            var endPoint = target.Position;
            caster.View.StopMoving();
            caster.View.AgentASP.IsCharging = true;
            caster.View.AgentASP.ForcePath(new ForcedPath(new List<Vector3>
            {
                position,
                endPoint
            }), true);
            caster.Descriptor.AddBuff(BlueprintRoot.Instance.SystemMechanics.ChargeBuff, context, new TimeSpan?(1.Rounds().Seconds));
            caster.Descriptor.State.IsCharging = true;
            UnitAttack attack = new UnitAttack(target, null);
            attack.Init(caster);

            IEnumerator routine = CombatController.IsInTurnBasedCombat() ? TurnBasesRoutine(caster, target, attack) : RuntimeRoutine(caster, target, attack, endPoint);
            while (routine.MoveNext())
                yield return null;
        }
        private static IEnumerator TurnBasesRoutine(UnitEntityData caster, UnitEntityData target, UnitAttack attack)
        {            
            UnitMovementAgent agentASP = caster.View.AgentASP;
            float timeSinceStart = 0f;
            float passedDistance = 0f;
            var distance = (caster.Position - target.Position).magnitude;
            while (distance > 30f || (attack.ShouldUnitApproach && passedDistance < 5))
            {
                passedDistance += (caster.Position - caster.PreviousPosition).magnitude;
                if (Game.Instance.TurnBasedCombatController.WaitingForUI)
                {
                    yield return null;
                }
                else
                {
                    timeSinceStart += Game.Instance.TimeController.GameDeltaTime;
                    if (timeSinceStart > 6f)
                    {
                        break;
                    }
                    if (!caster.Descriptor.State.CanMove)
                    {
                        break;
                    }
                    if (!agentASP)
                    {
                        break;
                    }
                    if (!agentASP.IsReallyMoving)
                    {
                        agentASP.ForcePath(new ForcedPath(new List<Vector3>
                        {
                            caster.Position,
                            target.Position
                        }), true);
                        if (!agentASP.IsReallyMoving)
                        {
                            break;
                        }
                    }
                    agentASP.MaxSpeedOverride = new float?(Math.Max(agentASP.MaxSpeedOverride.GetValueOrDefault(), caster.CombatSpeedMps * 2f));
                    yield return null;
                }
            }
            caster.View.StopMoving();
            if (!attack.ShouldUnitApproach)
            {
                attack.IgnoreCooldown(null);
                attack.IsCharge = true;
            }
            caster.Commands.AddToQueueFirst(attack);
            yield break;
        }
        private static IEnumerator RuntimeRoutine(UnitEntityData caster, UnitEntityData target, UnitAttack attack, Vector3 endPoint)
        {
            float maxDistance = GetMaxRangeMeters(caster)-30f;
            float passedDistance = 0f;
            while (caster.View.MovementAgent.IsReallyMoving)
            {
                caster.View.MovementAgent.MaxSpeedOverride = new float?(Math.Max(caster.View.MovementAgent.MaxSpeedOverride.GetValueOrDefault(), caster.CombatSpeedMps * 2f));
                passedDistance += (caster.Position - caster.PreviousPosition).magnitude;
                if (passedDistance > maxDistance || (passedDistance > 5 && !attack.ShouldUnitApproach))
                {
                    break;
                }
                Vector3 position = target.Position;
                if (ObstacleAnalyzer.TraceAlongNavmesh(caster.Position, position) != position)
                {
                    break;
                }
                if (position != endPoint)
                {
                    endPoint = position;
                    caster.View.AgentASP.ForcePath(new ForcedPath(new List<Vector3>
                    {
                        caster.Position,
                        endPoint
                    }), true);
                }
                yield return null;
            }    
            if (!attack.ShouldUnitApproach)
            {
                attack.IgnoreCooldown(null);
                attack.IsCharge = true;
            }
            caster.Commands.AddToQueueFirst(attack);
            yield break;
        }
        public string GetAbilityTargetRestrictionUIText(UnitEntityData caster, TargetWrapper target)
        {
            CheckTargetRestriction(caster, target, out var localizedString);
            return localizedString ?? string.Empty;
        }

        public float GetMinRangeMeters(UnitEntityData caster)
        {
            return AbilityCustomCharge.GetMinRangeMeters(caster, null);
        }
        public static float GetMinRangeMeters(UnitEntityData caster, [CanBeNull] UnitEntityData target)
        {
            float num = target?.View.Corpulence ?? 0.5f;
            if (Game.Instance.Player.IsTurnBasedModeOn())
                return TurnController.MetersOfFiveFootStep *2 + GameConsts.MinWeaponRange.Meters + caster.View.Corpulence + num;            
            else
                return 15.Feet().Meters + caster.View.Corpulence + num;
        }
        public bool IsTargetRestrictionPassed(UnitEntityData caster, TargetWrapper targetWrapper)
        {
            return CheckTargetRestriction(caster, targetWrapper, out var localizedString);
        }
        private bool CheckTargetRestriction(UnitEntityData caster, TargetWrapper targetWrapper, [CanBeNull] out LocalizedString failReason)
        {
            UnitEntityData unitEntityData = targetWrapper?.Unit;
            if (unitEntityData == null)
            {
                failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.TargetIsInvalid;
                return false;
            }
            if (!(caster.Body.PrimaryHand.MaybeWeapon?.Blueprint.Type.FighterGroup.Contains(WeaponFighterGroup.Thrown)?? false))
            {
                failReason = FailReasonWeapon;
                return false;
            }
            if (caster.GetSaddledUnit() != null)
            {
                failReason = FailReasonMount;
                return false;
            }
            float magnitude = (unitEntityData.Position - caster.Position).magnitude;
            if (magnitude > GetMaxRangeMeters(caster))
            {
                failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.TargetIsTooFar;
                return false;
            }
            if (magnitude < GetMinRangeMeters(caster, unitEntityData))
            {
                failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.TargetIsTooClose;
                return false;
            }
            if (ObstacleAnalyzer.TraceAlongNavmesh(caster.Position, unitEntityData.Position) != unitEntityData.Position)
            {
                failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.ObstacleBetweenCasterAndTarget;
                return false;
            }
            failReason = null;
            return true;
        }
        public static float GetMaxRangeMeters(UnitEntityData caster)
        {
            return caster.CombatSpeedMps * 6f + 30f;
        }

    }
}
