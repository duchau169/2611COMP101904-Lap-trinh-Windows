namespace ProductManager;

/// <summary>Lưu thông tin một sản phẩm.</summary>
public class Product
{
    public string MaSP { get; set; } = string.Empty;
    public string TenSP { get; set; } = string.Empty;
    public string LoaiSP { get; set; } = string.Empty;
    public decimal DonGia { get; set; }
    public int SoLuong { get; set; }
    public DateTime NgayNhap { get; set; } = DateTime.Today;
    public bool ConKinhDoanh { get; set; } = true;
    public string DuongDanAnh { get; set; } = string.Empty;
}
