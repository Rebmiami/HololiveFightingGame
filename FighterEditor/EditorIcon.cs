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

		HitboxTypeNormal,
		HitboxTypeGrab,
		HitboxTypeShield,
		HitboxTypeWind,
		HitboxTypeTrigger,
		HitboxTypeProjectile,

		HurtboxTypeVulnerable,
		HurtboxTypeInvulnerable,
		HurtboxTypeIntangible,

		HitboxGrounded,
		HitboxAerial,
		NA1,

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

		NA2,
		NA3,

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

		NA4,

		CostumeIcon,
		CostumeCustomizable,
		CostumeSprite,

		NA5,
		NA6,
		NA7,

		AnimationIcon,
		AnimationFrame,
		AnimationSoundEffect,
		AnimationFlash,

		NA8,
		NA9,

		DynamicsTrigger,
		DynamicsScript,
		DynamicsVariable,

		NA10,
		NA11,
		NA12,

		Add,
		Delete,
		Clone,
		MoveBack,
		MoveForth
	}
}
