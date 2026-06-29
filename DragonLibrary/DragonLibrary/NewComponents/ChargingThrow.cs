using System;
using System.Collections;
using System.Collections.Generic;
using DragonLibrary;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers.Clicks.Handlers;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.Localization;
using Kingmaker.Pathfinding;
using Kingmaker.TurnBasedMode;
using Kingmaker.TurnBasedMode.Controllers;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.Utility;
using Kingmaker.View;
using Owlcat.Runtime.Core.Utils;
using TurnBased.Controllers;
using UnityEngine;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Kingmaker.UnitLogic.Commands.Base;

[Serializable]
[AllowedOn(typeof(BlueprintAbility), false)]
[TypeId("F6CABA0C-7B39-44D7-BC74-D9CBF0753E6C")]
public class ChargingThrow : AbilityCustomCharge, IAbilityTargetRestriction, IAbilityMinRangeProvider
{
    private static readonly LocalizedString FailReasonWeapon = new LocalizedString
    {
        m_Key = "Chargin Hurler Fail Wrong Weapon"
    };

    private static readonly LocalizedString FailReasonMount = new LocalizedString
    {
        m_Key = "Chargin Hurler Fail Must Run"
    };

    public override void Cleanup(AbilityExecutionContext context)
    {
        context.Caster.View.AgentASP.IsCharging = false;
        context.Caster.View.AgentASP.MaxSpeedOverride = null;
        context.Caster.Descriptor.State.IsCharging = false;
    }

    public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper targetWrapper)
    {
        UnitEntityData unit = targetWrapper.Unit;
        if (unit == null)
        {
            PFLog.Default.Error("Target unit is missing");
            return Enumerable.Empty<AbilityDeliveryTarget>().GetEnumerator();
        }
        UnitEntityData caster = context.Caster;
        Vector3 position = caster.Position;
        Vector3 position2 = unit.Position;
        caster.View.StopMoving();
        caster.View.AgentASP.IsCharging = true;
        //caster.View.AgentASP.ForcePath(new ForcedPath(new List<Vector3> { position, position2 }), disableApproachRadius: true);
        caster.Descriptor.AddBuff(BlueprintRoot.Instance.SystemMechanics.ChargeBuff, context, 1.Rounds().Seconds);
        caster.Descriptor.State.IsCharging = true;
        UnitAttack unitAttack = new(unit) { IsCharge = true, MovementType = UnitAnimationActionLocoMotion.WalkSpeedType.Charge};
        unitAttack.Init(caster);
        var routine = (CombatController.IsInTurnBasedCombat() ? TurnBasedRoutine(caster, unit, unitAttack) : RuntimeRoutine(caster, unit, unitAttack, position2));
        return routine;
    }

    private static IEnumerator<AbilityDeliveryTarget> TurnBasedRoutine(UnitEntityData caster, UnitEntityData _, UnitAttack attack)
    {
        caster.View.AgentASP.MaxSpeedOverride = Math.Max(caster.View.AgentASP.MaxSpeedOverride.GetValueOrDefault(), caster.CombatSpeedMps * 2f);
        var currenTurn = Game.Instance.TurnBasedCombatController.CurrentTurn;
        currenTurn.CurrentMovementLimit = caster.IsSurprising() ? TurnController.MovementLimit.OneAction : TurnController.MovementLimit.TwoActions;
        var colldown = currenTurn.GetCooldown(caster);
        colldown.MoveAction = 0;
        colldown.StandardAction = 0;
        attack.Type = UnitCommand.CommandType.Free;
        caster.Commands.Run(attack);
        while ((attack.Result is not UnitCommand.ResultType.Success and not UnitCommand.ResultType.Interrupt))
            yield return null;
        colldown.MoveAction = 6;
        colldown.StandardAction = 6;
        caster.Descriptor.State.IsCharging = false;
    }

    private static new IEnumerator<AbilityDeliveryTarget> RuntimeRoutine(UnitEntityData caster, UnitEntityData target, UnitAttack attack, Vector3 endPoint)
    {
        float maxDistance = GetMaxRangeMeters(caster) - 30f;
        float passedDistance = 0f;
        while (caster.View.MovementAgent.IsReallyMoving)
        {
            caster.View.MovementAgent.MaxSpeedOverride = Math.Max(caster.View.MovementAgent.MaxSpeedOverride.GetValueOrDefault(), caster.CombatSpeedMps * 2f);
            passedDistance += (caster.Position - caster.PreviousPosition).magnitude;
            if (passedDistance > maxDistance || (passedDistance > 5f && !attack.ShouldUnitApproach))
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
                caster.View.AgentASP.ForcePath(new ForcedPath(new List<Vector3> { caster.Position, endPoint }), disableApproachRadius: true);
            }
            yield return null;
        }
        if (!attack.ShouldUnitApproach)
        {
            attack.IgnoreCooldown();
            attack.IsCharge = true;
        }
        caster.Commands.AddToQueueFirst(attack);
    }

    public new string GetAbilityTargetRestrictionUIText(UnitEntityData caster, TargetWrapper target)
    {
        CheckTargetRestriction(caster, target, out var failReason);
        LocalizedString localizedString = failReason;
        if (localizedString == null)
        {
            return string.Empty;
        }
        return localizedString;
    }

    public new float GetMinRangeMeters(UnitEntityData caster)
    {
        return AbilityCustomCharge.GetMinRangeMeters(caster, null);
    }

    public static new float GetMinRangeMeters(UnitEntityData caster, [CanBeNull] UnitEntityData target)
    {
        float num = target?.View.Corpulence ?? 0.5f;
        if (Game.Instance.Player.IsTurnBasedModeOn())
        {
            return TurnController.MetersOfFiveFootStep * 2f + GameConsts.MinWeaponRange.Meters + caster.View.Corpulence + num;
        }
        return 15.Feet().Meters + caster.View.Corpulence + num;
    }

    public new bool IsTargetRestrictionPassed(UnitEntityData caster, TargetWrapper targetWrapper)
    {
        return CheckTargetRestriction(caster, targetWrapper, out _);
    }

    private new bool CheckTargetRestriction(UnitEntityData caster, TargetWrapper targetWrapper, [CanBeNull] out LocalizedString failReason)
    {
        UnitEntityData unitEntityData = targetWrapper?.Unit;
        if (unitEntityData == null)
        {
            failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.TargetIsInvalid;
            return false;
        }
        ItemEntityWeapon maybeWeapon = caster.Body.PrimaryHand.MaybeWeapon;
        if (maybeWeapon == null || !maybeWeapon.Blueprint.Type.FighterGroup.Contains(WeaponFighterGroup.Thrown))
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

    public static new float GetMaxRangeMeters(UnitEntityData caster)
    {
        var movementLimit = caster.IsSurprising() ? 1f : 2f;
        return caster.CombatSpeedMps * 3f * movementLimit + Convert.ToSingle(caster.GetFirstWeapon().AttackRange.Meters);
    }
}

[UsedImplicitly]
[HarmonyLib.HarmonyPatch]
static class ChargingThrowPatches
{

    [UsedImplicitly]
    [HarmonyLib.HarmonyPatch(typeof(PathVisualizer), nameof(PathVisualizer.UpdateChargePath))]
    [HarmonyLib.HarmonyPrefix]
    static bool PathVisualizerPreviewPostfix(ref bool __result, PathVisualizer __instance)
    {
        //if (__result)
        //    return true;

        AbilityData abilityData = Game.Instance.SelectedAbilityHandler?.SelectedAbility;
        if (abilityData == null)
        {
            return true;
        }
        if (abilityData.Blueprint.GetComponent<ChargingThrow>() is null)
        {
            return true;
        }

        __result = false;
        GameObject gameObject = Game.Instance.DefaultPointerController.PointerOn;
        if (!gameObject)
        {
            return false;
        }
        var view = gameObject.GetComponent<UnitEntityView>();
        if (!view)
        {
            return false;
        }

        var targetUnitData = view.Data;
        var caster = abilityData.Caster.Unit;
        var startingPosition = caster.Position;
        var direction = (targetUnitData.Position - startingPosition).normalized;
        var targetPosition = targetUnitData.Position - (direction * (UnitAttack.GetApproachRadius(caster.GetFirstWeapon(), caster, targetUnitData) - 0.1f));
        TargetWrapper targetWrapper = new (targetPosition, null, targetUnitData);
        if (!abilityData.CanTarget(targetWrapper))
        {
            return false;
        }


        if (__instance.m_CurrentPath != null)
        {
            if (__instance.m_CurrentPath is ForcedPath { UserTag: "ChargePathTag" } && __instance.m_CurrentPath.vectorPath.Count == 2 && (__instance.m_CurrentPath.vectorPath[0] - startingPosition).sqrMagnitude < 0.1f && (__instance.m_CurrentPath.vectorPath[1] - targetPosition).sqrMagnitude < 0.1f)
            {
                return false;
            }
        }
        var forcedPath2 = new ForcedPath([startingPosition, targetPosition])
        {
            UserTag = "ChargePathTag"
        };
        __instance.m_RequestedPath = null;
        __instance.m_CurrentPath = forcedPath2;
        __instance.m_CurrentPath.Claim(__instance);
        var MaxDistance = caster.CombatSpeedMps * 6f;
        PathVisualizer.VisualPathSettings visualPathSettings = new PathVisualizer.VisualPathSettings
        {
            Unit = caster,
            UnitCanGetUpOnCommand = false,
            MoveActionCooldown = 0f,
            MovePredictionState = CombatAction.ActionState.Available,
            StandardPredictionState = CombatAction.ActionState.Available,
            RemainingMovementRange = MaxDistance,
            RemainingMovementRangeTotal = MaxDistance,
            EnabledSingleActionMove = true,
            EnabledFiveFeetStep = false
        };
        __instance.UpdateVisualPath(targetPosition, 0.3f, true, -1f, visualPathSettings);
        __result = true;
        return false;

    }

    [UsedImplicitly]
    [HarmonyLib.HarmonyPatch(typeof(UnitEntityData), nameof(UnitEntityData.IsMoveActionRestricted))]
    [HarmonyLib.HarmonyPostfix]
    static bool UnitEntityDataMoveActionRstrictedPostfix(bool __result, UnitEntityData __instance)
        => __result && !__instance.Descriptor.State.IsCharging;
}
