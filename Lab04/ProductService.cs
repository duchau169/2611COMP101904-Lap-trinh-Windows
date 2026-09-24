using System;
using System.Collections.Generic;

namespace Lab04_QuanLySanPham
{
    // Xu ly nghiep vu san pham: kiem tra du lieu, goi Repository, phat event
    public class ProductService
    {
        private readonly Repository<Product> repository;

        // Event duoc phat khi them / xoa san pham thanh cong
        public event Action<Product> ProductAdded;
        public event Action<Product> ProductRemoved;

        public ProductService(Repository<Product> repository)
        {
            this.repository = repository;
        }

        // Them san pham - nem DuplicateProductException neu ma da ton tai
        public void AddProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.MaSP))
            {
                throw new ArgumentException("Ma san pham khong duoc de trong.");
            }

            if (repository.FindById(product.MaSP) != null)
            {
                throw new DuplicateProductException(product.MaSP);
            }

            repository.Add(product);

            // Phat event bao thanh cong
            ProductAdded?.Invoke(product);
        }

        // Xoa san pham - nem ProductNotFoundException neu khong ton tai
        public void RemoveProduct(string maSP)
        {
            Product sp = repository.FindById(maSP);
            if (sp == null)
            {
                throw new ProductNotFoundException(maSP);
            }

            repository.Remove(sp);

            // Phat event bao thanh cong
            ProductRemoved?.Invoke(sp);
        }

        // Tim theo ma
        public Product TimTheoMa(string maSP)
        {
            return repository.FindById(maSP);
        }

        // Tim theo ten (dung Func<Product,bool>)
        public List<Product> Search(string tuKhoa)
        {
            Func<Product, bool> dieuKien = sp =>
                sp.TenSP.IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0;
            return repository.Find(dieuKien);
        }

        // Loc theo khoang gia (dung Func<Product,bool>)
        public List<Product> Filter(decimal giaMin, decimal giaMax)
        {
            Func<Product, bool> dieuKien = sp => sp.Price >= giaMin && sp.Price <= giaMax;
            return repository.Find(dieuKien);
        }

        // Lay toan bo danh sach
        public List<Product> GetAll()
        {
            return repository.GetAll();
        }

        // Tinh tong gia tri kho = tong (don gia * so luong) cua tat ca san pham
        public decimal TinhTongGiaTriKho()
        {
            decimal tong = 0;
            foreach (var sp in repository.GetAll())
            {
                tong += sp.ThanhTien();
            }
            return tong;
        }
    }
}
