using Assets.Scripts.Models.Bloons.Behaviors;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Simulation.SMath;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;

namespace VillageParagon
{
    public class VillageParagon
    {
        public class MonkeyVillageParagon : ModVanillaParagon
        {
            public override string BaseTower => "MonkeyVillage-140";
            public override string Name => "MonkeyVillage";
        }

        static public bool addSentriesInShop = false;

        public class MonkeyVillageParagonUpgrade : ModParagonUpgrade<MonkeyVillageParagon>
        {
            public override int Cost => 650000;
            public override string Description => "Throws specialized Bots on the field. Tier 5s sacrifices enchance the paragon's tower of the same type but will consume everything that isn't a village. Also gives mega buffs.";
            public override string DisplayName => "Monkey Industry";

            public override string Icon => "VillageParagon-Icon";
            public override string Portrait => "VillageParagon-Portrait";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.targetTypes = Game.instance.model.GetTower(TowerType.DartMonkey).targetTypes;
                towerModel.TargetTypes = Game.instance.model.GetTower(TowerType.DartMonkey).TargetTypes;

                // ballista gaming.
                towerModel.AddBehavior(Game.instance.model.GetTower("MonkeyVillage", 5, 0, 0).GetAttackModel(0).Duplicate());
                var attackmodel1 = towerModel.GetAttackMode(0);
                attackmodel1.range = 2000.0f;
                attackmodel1.weapons[0].Rate = 5.0f;
                attackmodel1.weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 250, 0.0f, 360.0f, null, false, false);
                attackmodel1.weapons[0].projectile.pierce = 50.0f;
                attackmodel1.weapons[0].projectile.GetDamageModel().damage = 100.0f;
                attackmodel1.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan = 100.0f;
                attackmodel1.weapons[0].projectile.AddBehavior(new ExpireProjectileAtScreenEdgeModel("ExpireProjectileAtScreenEdgeModel_"));
                attackmodel1.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("TackShooter-050").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].Duplicate());

                // Mega discount
                towerModel.AddBehavior(Game.instance.model.GetTower("MonkeyVillage", 0, 0, 2).GetBehavior<DiscountZoneModel>());
                towerModel.GetBehavior<DiscountZoneModel>().discountMultiplier = 0.30f;
                towerModel.GetBehavior<DiscountZoneModel>().tierCap = 5;
                towerModel.GetBehavior<DiscountZoneModel>().buffLocsName = "MonkeyBusinessBuff";
                towerModel.GetBehavior<DiscountZoneModel>().stackName = "MonkeyIndustryDiscount";
                towerModel.GetBehavior<DiscountZoneModel>().name = "MonkeyIndustryDiscount";
                towerModel.GetBehavior<DiscountZoneModel>().buffIconName = "BuffIconVillagexx1";

                // Extra range
                towerModel.AddBehavior(Game.instance.model.GetTower("MonkeyVillage", 5, 0, 0).GetBehavior<RangeSupportModel>());
                towerModel.GetBehavior<RangeSupportModel>().multiplier = 0.5f;

                // Boosted Drums
                towerModel.AddBehavior(Game.instance.model.GetTower("MonkeyVillage", 5, 0, 0).GetBehavior<RateSupportModel>());
                towerModel.GetBehavior<RateSupportModel>().multiplier = 1.7f;
            }
        }

        public class VillageParagonDisplay : ModTowerDisplay<MonkeyVillageParagon>
        {
            public override string BaseDisplay => GetDisplay(TowerType.MonkeyVillage, 1, 4, 0);

            public override bool UseForTower(int[] tiers)
            {
                return IsParagon(tiers);
            }

            public override int ParagonDisplayIndex => 0;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture("VillageParagonDisplay");
                    //node.SaveMeshTexture();
                }

                base.ModifyDisplayNode(node);
            }
        }
    }
}
