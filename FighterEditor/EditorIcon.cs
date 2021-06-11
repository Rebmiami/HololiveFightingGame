using System;
using System.Collections.Generic;
using System.Text;

namespace HololiveFightingGame.FighterEditor
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
		EditorEntity,

		HitboxTypeNormal,
		HitboxTypeGrab,
		HitboxTypeShield,
		HitboxTypeWind,
		HitboxTypeTrigger,
		HitboxTypeProjectile,

		HurtboxTypeVulnerable,
		HurtboxTypeInvulnerable,
		HurtboxTypeIntangible,

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

		NA1,

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

		NA2,
		NA3,

		Add,
		Delete,
		Clone,
		MoveBack,
		MoveForth,

		NA4,

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

		NA5,
		NA6,
	}
}
