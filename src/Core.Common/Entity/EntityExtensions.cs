using Core.Common.Security.Claims;
using System;
using System.Security.Principal;

namespace Core.Common.Entity
{
    /// <summary>
    /// 实体接口扩展方法
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// 检测并执行<see cref="ICreatedTime"/>接口的逻辑
        /// </summary>
        public static TEntity CheckICreatedTime<TEntity, TKey>(this TEntity entity)
            where TEntity : IEntity<TKey>
        {
            if (!(entity is ICreatedTime))
            {
                return entity;
            }
            ICreatedTime entity1 = (ICreatedTime)entity;
            entity1.CreatedTime = DateTime.Now;
            return (TEntity)entity1;
        }

        /// <summary>
        /// 检测并执行<see cref="ICreationAudited{TUserKey}"/>接口的处理
        /// </summary>
        public static TEntity CheckICreationAudited<TEntity, TKey, TUserKey>(this TEntity entity, IPrincipal user)
            where TEntity : IEntity<TKey>
            where TUserKey : struct
        {
            if (!(entity is ICreationAudited<TUserKey>))
            {
                return entity;
            }

            ICreationAudited<TUserKey> entity1 = (ICreationAudited<TUserKey>)entity;
            entity1.CreatorId = user.Identity.GetUserId<TUserKey>();
            entity1.CreatedTime = DateTime.Now;
            return (TEntity)entity1;
        }

        /// <summary>
        /// 检测并执行<see cref="IUpdateAudited{TUserKey}"/>接口的处理
        /// </summary>
        public static TEntity CheckIUpdateAudited<TEntity, TKey, TUserKey>(this TEntity entity, IPrincipal user)
            where TEntity : IEntity<TKey>
            where TUserKey : struct
        {
            if (!(entity is IUpdateAudited<TUserKey>))
            {
                return entity;
            }

            IUpdateAudited<TUserKey> entity1 = (IUpdateAudited<TUserKey>)entity;
            entity1.LastUpdaterId = user.Identity.GetUserId<TUserKey>();
            entity1.LastUpdatedTime = DateTime.Now;
            return (TEntity)entity1;
        }


        /// <summary>
        /// 检测并执行<see cref="IDeleteAudited{TUserKey}"/>接口的处理
        /// </summary>
        public static TEntity CheckIDeleteAudited<TEntity, TKey, TUserKey>(this TEntity entity, IPrincipal user)
            where TEntity : IEntity<TKey>
            where TUserKey : struct
        {
            if (!(entity is IUpdateAudited<TUserKey>))
            {
                return entity;
            }

            IDeleteAudited<TUserKey> entity1 = (IDeleteAudited<TUserKey>)entity;
            entity1.LastDeleterId = user.Identity.GetUserId<TUserKey>();
            entity1.LastDeletedTime = DateTime.Now;
            return (TEntity)entity1;
        }
    }
}
