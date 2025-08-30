namespace PetalsOfHope.Contracts
{
    public enum VFXType
    {
        None = 0,

        // Player Actions
        PlayerRunDust,
        PlayerJumpDust,
        PlayerLandImpact,
        PlayerDoubleJump,
        PlayerWallSlideDust,
        PlayerWallJump,
        PlayerDashStart,
        PlayerDashTrail,
        PlayerDashEnd,
        PlayerTakeDamage,
        PlayerDeath,

        // Enemy Interactions
        EnemyTakeDamage,
        EnemyDeathPuff,
        EnemyDeathExplosion,
        EnemySpawn,
        ProjectileImpact,

        // Item and Environment Interactions
        TalismanCollect,
        HealthPickup,
        CheckpointActivate,
        HazardImpact,
        SecretWallBreak,

        // Screen Effects (to be handled by a different system, but listed for completeness)
        CameraShakeLight,
        CameraShakeMedium,
        CameraShakeHeavy,
        ScreenFlash
    }
}
