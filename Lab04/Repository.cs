using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab04_QuanLySanPham
{
    // Generic repository quan ly danh sach doi tuong T, T phai co Id (IEntity)
    public class Repository<T> where T : IEntity
    {
        private readonly List<T> danhSach;

        public Repository()
        {
            danhSach = new List<T>();
        }

        // Them 1 phan tu vao repository
        public void Add(T item)
        {
            danhSach.Add(item);
        }

        // Xoa 1 phan tu khoi repository, tra ve true neu xoa thanh cong
        public bool Remove(T item)
        {
            return danhSach.Remove(item);
        }

        // Tim theo Id (dung LINQ)
        public T FindById(string id)
        {
            return danhSach.FirstOrDefault(x =>
                x.Id != null && x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        // Tim/loc theo dieu kien tuy y bang Func<T,bool>
        public List<T> Find(Func<T, bool> dieuKien)
        {
            return danhSach.Where(dieuKien).ToList();
        }

        // Lay toan bo danh sach
        public List<T> GetAll()
        {
            return danhSach;
        }
    }
}
