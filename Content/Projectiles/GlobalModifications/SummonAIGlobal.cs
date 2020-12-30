﻿using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Projectiles.GlobalModifications
{
    public class SummonAIGlobal : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override bool CloneNewInstances => true;

        public float savedRotation;

        public static Dictionary<int, float> SummonRotationAdjustments = new Dictionary<int, float>()
        {
            { ModContent.ProjectileType<IceClasperMinion>(), 0.15f },
            { ModContent.ProjectileType<PowerfulRaven>(), 0.25f },
            { ModContent.ProjectileType<HerringMinion>(), 0.25f }
        };

        public override bool PreAI(Projectile projectile)
        {
            if (SummonRotationAdjustments.ContainsKey(projectile.type) || projectile.type == ModContent.ProjectileType<CinderBlossom>())
                savedRotation = projectile.rotation;

            return base.PreAI(projectile);
        }

        public override void PostAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];

            if (SummonRotationAdjustments.ContainsKey(projectile.type))
            {
                projectile.rotation = savedRotation;

                if (projectile.type == ModContent.ProjectileType<PowerfulRaven>())
                {
                    projectile.spriteDirection = 1;
                    LerpToRotation(projectile, SummonRotationAdjustments[projectile.type], 0);
                }
                else
                    LerpToRotation(projectile, SummonRotationAdjustments[projectile.type]);
            }

            if (projectile.type == ModContent.ProjectileType<CinderBlossom>())
                projectile.rotation = savedRotation + (MathHelper.ToRadians(1.5f) + (player.velocity.X / player.maxRunSpeed / 5f * player.direction)) * player.direction;
        }

        public static void LerpToRotation(Projectile projectile, float amount, float adjustment = MathHelper.PiOver2) => projectile.rotation = projectile.rotation.AngleTowards(projectile.velocity.ToRotation() - adjustment, amount);
    }
}