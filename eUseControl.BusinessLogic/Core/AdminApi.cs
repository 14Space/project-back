using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.BusinessLogic.Core
{
    public class AdminApi
    {
        // --- ТОВАРЫ ---
        public List<Product> GetAllProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 1000, Description = "Gaming laptop" }
            };
        }

        public void AddProduct(Product product) { /* Логика добавления */ }
        public void UpdateProduct(int id, Product product) { /* Логика обновления */ }
        public void DeleteProduct(int id) { /* Логика удаления */ }

        // --- КАТЕГОРИИ ---
        public List<Category> GetAllCategories()
        {
            return new List<Category>
            {
                new Category { Id = 1, Name = "Electronics", Description = "Gagdets" }
            };
        }

        public void AddCategory(Category category) { /* Логика добавления */ }
        public void UpdateCategory(int id, Category category) { /* Логика обновления */ }
        public void DeleteCategory(int id) { /* Логика удаления */ }

        // --- ПОЛЬЗОВАТЕЛИ ---
        public void UpdateUserRole(int id, string role) { /* Логика роли */ }
        public void DeleteUser(int id) { /* Логика удаления (бан) */ }
    }
}
