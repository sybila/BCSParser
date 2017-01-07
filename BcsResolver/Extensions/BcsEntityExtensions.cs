using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.File;
using BcsResolver.SemanticModel;
using BcsResolver.SemanticModel.Exceptions;

namespace BcsResolver.Extensions
{
    public static class BcsEntityExtensions
    {
        public static BcsEntity ThrowIfNull(this BcsEntity entity, string expectedName)
        {
            if (entity == null)
            {
                throw new EntityNotFoundException($"Entity {expectedName} is not defined.");
            }
            return entity;
        }

        public static BcsEntity ThrowIfNotType(this BcsEntity entity, BcsSymbolType expectedType)
        {
            if ((int)entity.Type != (int)expectedType)
            {
                throw new EntityTypeException($"Entity {entity.Id} is inforrect type: {entity.Type}. Expected type: {expectedType}");
            }
            return entity;
        }

        public static BcsEntity ThrowIfIdEmpty(this BcsEntity entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                throw new InvalidOperationException($"Entity has empty Id");
            }
            return entity;
        }
    }
}
