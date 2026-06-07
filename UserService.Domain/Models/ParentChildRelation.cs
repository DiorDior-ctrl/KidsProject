using System;
using System.Collections.Generic;
using System.Text;
using UserService.Domain.Exceptions;
using UserService.Domain.Models;

namespace UserService.Domain.Models
{
    public class ParentChildRelation
    {
        public Guid Id { get; private set; }
        public Guid ParentId { get; private set; }
        public Guid ChildID { get; private set; }
        public DateTime LinkedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public User Parent { get; private set; } = null!;
        public User Child {  get; private set; } = null!;

        private ParentChildRelation(){}

        public static ParentChildRelation Create(Guid parentId , Guid childId)
        {
            if (parentId == childId)
                throw new BusinessException("Femija dhe prindi nuk mund te jene i njejti perdorues!");

            return new ParentChildRelation
            { 
                Id = Guid.NewGuid(),
                ParentId = parentId,    
                ChildID = childId,
                LinkedAt = DateTime.UtcNow
            };
        }
        public void SoftDelete()
        { 
           DeletedAt = DateTime.UtcNow;
        }
    }
}
