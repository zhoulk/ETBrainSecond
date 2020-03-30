
namespace ETBrain
{
    public class AIUtility
    {
        public static void PerformCollision(TargetableObject entity, Entity other)
        {
            if (entity == null || other == null)
            {
                return;
            }

            Bullet bullet = other as Bullet;
            if (bullet != null)
            {
                //ImpactData entityImpactData = entity.GetImpactData();
                //ImpactData bulletImpactData = bullet.GetImpactData();
                //if (GetRelation(entityImpactData.Camp, bulletImpactData.Camp) == RelationType.Friendly)
                //{
                //    return;
                //}

                //int entityDamageHP = CalcDamageHP(bulletImpactData.Attack, entityImpactData.Defense);

                int entityDamageHP = bullet.BulletData.Attack;

                entity.ApplyDamage(bullet, entityDamageHP);
                GameEntry.Entity.HideEntity(bullet);
                return;
            }

            TargetableObject target = other as TargetableObject;
            if (target != null)
            {
                //ImpactData entityImpactData = entity.GetImpactData();
                //ImpactData targetImpactData = target.GetImpactData();
                //if (GetRelation(entityImpactData.Camp, targetImpactData.Camp) == RelationType.Friendly)
                //{
                //    return;
                //}

                //int entityDamageHP = CalcDamageHP(targetImpactData.Attack, entityImpactData.Defense);
                //int targetDamageHP = CalcDamageHP(entityImpactData.Attack, targetImpactData.Defense);

                //int delta = Mathf.Min(entityImpactData.HP - entityDamageHP, targetImpactData.HP - targetDamageHP);
                //if (delta > 0)
                //{
                //    entityDamageHP += delta;
                //    targetDamageHP += delta;
                //}

                int entityDamageHP = target.TargetableObjectData.Attack;
                int targetDamageHP = entity.TargetableObjectData.Attack;

                entity.ApplyDamage(target, entityDamageHP);
                target.ApplyDamage(entity, targetDamageHP);
                return;
            }
        }
    }
}

