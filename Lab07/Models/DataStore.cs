using System.ComponentModel;

namespace ShopManagerMini
{
    /// <summary>Kho dữ liệu dùng chung trong bộ nhớ (chưa dùng SQL Server).</summary>
    public static class DataStore
    {
        public static List<User> Users { get; } = new List<User>
        {
            new User { Username = "admin", Password = "123", FullName = "Quản trị viên", Role = "Admin" },
            new User { Username = "user",  Password = "123", FullName = "Nhân viên bán hàng", Role = "User" }
        };

        public static BindingList<Product> Products { get; } = new BindingList<Product>
        {
            new Product { Id = "SP001", Name = "Nước suối 500ml", Category = "Đồ uống",   Price = 5000,  Quantity = 100 },
            new Product { Id = "SP002", Name = "Mì gói",          Category = "Thực phẩm", Price = 4500,  Quantity = 200 },
            new Product { Id = "SP003", Name = "Nước rửa chén",   Category = "Gia dụng",  Price = 28000, Quantity = 40 }
        };

        public static BindingList<Customer> Customers { get; } = new BindingList<Customer>
        {
            new Customer { Id = "KH001", FullName = "Nguyễn Văn An", Phone = "0901234567", CustomerType = "Thường" },
            new Customer { Id = "KH002", FullName = "Trần Thị Bình", Phone = "0912345678", CustomerType = "VIP" }
        };
    }
}
