using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor.GUI
{
	public enum EditorIcon
	{
		OpenFile,
		Save,
		Undo,
		Redo,

		MenuFile,
		MenuFighter,
		MenuMove,
		MenuAnimation,

		EditorMove,
		EditorAnimation,
		EditorHitbox,
		EditorDynamics,

		EditorAI,
		EditorProjectile,
		EditorEntity, // Unused

		HitboxTypeNormal,
		HitboxTypeGrab,
		HitboxTypeShield,
		HitboxTypeWind,
		HitboxTypeTrigger,
		HitboxTypeProjectile,

		HurtboxTypeVulnerable,
		HurtboxTypeInvulnerable,
		HurtboxTypeIntangible,

		PhysicsEJumpCount,
		PhysicsJumpForce,
		PhysicsEJumpForce,
		PhysicsAirResistance,
		PhysicsTraction,
		PhysicsGravity,
		PhysicsAirSpeed,
		PhysicsGroundSpeed,
		PhysicsFallSpeed,
		PhysicsFastFallSpeed,
		PhysicsAirAcceleration,
		PhysicsGroundAcceleration,

		FighterWeight,

		NA1,
		NA2,
		NA3,
		NA4,
		NA5,
		NA6,
		NA7,
		NA8,
		NA9,
		NA10,
		NA11,

		HitboxPosition,
		HitboxRadius,
		HitboxKeyframe,

		HitboxDamage,
		HitboxKnockback,
		HitboxKnockbackGrowth,
		HitboxLaunchAngle,

		MoveDuration,
		HitboxAutoswipe,
		HitboxEnabled,

		HitboxGrounded,
		HitboxAerial,
		HitboxPart,
		HitboxPriority,

		TagIcon,
		TagMaleSign,
		TagFemaleSign,
		TagPalette,
		TagHairColor,
		TagSkinTone,
		TagClothesColor,
		TagPaintbrush,
		TagGroup,
		TagOther,

		PreviewPlay,
		PreviewPause,
		PreviewStepForth,
		PreviewStepBack,
		PreviewGoBack,
		PreviewBuffer,
		PreviewCameraNormal,
		PreviewCameraPlayer,
		PreviewCameraLocked,
		PreviewResetCamera,
		PreviewSlow,
		PreviewFast,

		PreviewShowHurtbox,
		PreviewShowHitbox,
		PreviewShowSprite,
		PreviewShowGrounded,
		PreviewShowAerial,
		PreviewShowFlashes,

		CostumeIcon,
		CostumeCustomizable,
		CostumeSprite,
		CostumeIdol,
		CostumeBeach,
		CostumeChallenge,

		AnimationIcon,
		AnimationFrame,
		AnimationSoundEffect,
		AnimationFlash,

		NA12,
		NA13,

		Add,
		Delete,
		Clone,
		MoveBack,
		MoveForth,

		NA14,

		DynamicsTrigger,
		DynamicsScript,
		DynamicsVariable,

		DynamicsTypeBool,
		DynamicsTypeInt,
		DynamicsTypeString,
		DynamicsTypeVector,
		DynamicsTypeFighter,
		DynamicsTypeFloat,
		DynamicsTypeEntity,

		NA15,
		NA16,
	}
}
