using System;

namespace Lab04_QuanLySanPham
{
    // San pham - implement IEntity de dung duoc trong Repository<T>
    public class Product : IEntity
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Don gia khong duoc am.");
                }
                _price = value;
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("So luong khong duoc am.");
                }
                _quantity = value;
            }
        }

        // Id cua IEntity chinh la MaSP
        public string Id => MaSP;

        public Product()
        {
            MaSP = string.Empty;
            TenSP = string.Empty;
            _price = 0;
            _quantity = 0;
        }

        public Product(string maSP, string tenSP, decimal price, int quantity)
        {
            MaSP = maSP;
            TenSP = tenSP;
            Price = price;       // qua property de kiem tra
            Quantity = quantity; // qua property de kiem tra
        }

        // Thanh tien cua san pham nay
        public decimal ThanhTien()
        {
            return Price * Quantity;
        }

        public override string ToString()
        {
            return $"Ma: {MaSP,-8} | Ten: {TenSP,-25} | Gia: {Price,10:N0} | SL: {Quantity,-6} | Thanh tien: {ThanhTien(),12:N0}";
        }
    }
}
