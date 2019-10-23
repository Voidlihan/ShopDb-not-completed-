using Shop.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Shop.DataAcess.Abstract;

namespace Shop.DataAcess.Abstract
{
    public interface ICategoryRepository
    {
        void Add(Category category);
        void Delete(Guid categoryID);
        void Update(Category category);
        ICollection<Category> GetAll();
    }
}
