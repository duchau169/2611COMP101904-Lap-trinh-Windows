using System.Text;
using StudentReportDemo.Models;

namespace StudentReportDemo.Forms;

public partial class FrmBaoCaoSinhVien : Form
{
    private List<SinhVien> danhSachSinhVien = new List<SinhVien>();

    public FrmBaoCaoSinhVien()
    {
        InitializeComponent();
    }

    private void FrmBaoCaoSinhVien_Load(object? sender, EventArgs e)
    {
        TaoDuLieuMau();
        LoadComboBox();
        XemBaoCao(false);

        // Gắn sự kiện sau khi nạp xong để tránh gọi sự kiện nhiều lần lúc khởi động
        cboLop.SelectedIndexChanged += (s, ev) => XemBaoCao(false);
        cboKetQua.SelectedIndexChanged += (s, ev) => XemBaoCao(false);
        txtTuKhoa.TextChanged += (s, ev) => XemBaoCao(false);
    }

    private void TaoDuLieuMau()
    {
        danhSachSinhVien = new List<SinhVien>
        {
            new SinhVien { MaSV = "SV001", HoTen = "Nguyễn Văn An",    MaLop = "K51A", TenLop = "CNTTA K51", Diem = 8.2 },
            new SinhVien { MaSV = "SV002", HoTen = "Trần Thị Bình",    MaLop = "K51A", TenLop = "CNTTA K51", Diem = 6.5 },
            new SinhVien { MaSV = "SV003", HoTen = "Lê Minh Cường",    MaLop = "K51A", TenLop = "CNTTA K51", Diem = 4.8 },
            new SinhVien { MaSV = "SV004", HoTen = "Phạm Thanh Dung",  MaLop = "K51A", TenLop = "CNTTA K51", Diem = 9.1 },
            new SinhVien { MaSV = "SV005", HoTen = "Hoàng Quốc Huy",   MaLop = "K51B", TenLop = "CNTTB K51", Diem = 7.4 },
            new SinhVien { MaSV = "SV006", HoTen = "Võ Gia Khang",     MaLop = "K51B", TenLop = "CNTTB K51", Diem = 3.9 },
            new SinhVien { MaSV = "SV007", HoTen = "Đặng Minh Khôi",   MaLop = "K51B", TenLop = "CNTTB K51", Diem = 5.5 },
            new SinhVien { MaSV = "SV008", HoTen = "Bùi Thảo Linh",    MaLop = "K51B", TenLop = "CNTTB K51", Diem = 8.0 },
            new SinhVien { MaSV = "SV009", HoTen = "Ngô Tuấn Minh",    MaLop = "K52A", TenLop = "CNTTA K52", Diem = 6.0 },
            new SinhVien { MaSV = "SV010", HoTen = "Đỗ Bảo Ngọc",      MaLop = "K52A", TenLop = "CNTTA K52", Diem = 9.3 },
            new SinhVien { MaSV = "SV011", HoTen = "Huỳnh Gia Phúc",   MaLop = "K52A", TenLop = "CNTTA K52", Diem = 4.2 },
            new SinhVien { MaSV = "SV012", HoTen = "Mai Anh Thư",      MaLop = "K52A", TenLop = "CNTTA K52", Diem = 7.8 }
        };
    }

    private void LoadComboBox()
    {
        // Gom nhóm theo MaLop và sắp xếp theo tên lớp để danh sách hiển thị chuẩn xác, không bị lệch
        var dsLop = danhSachSinhVien
            .GroupBy(sv => sv.MaLop)
            .Select(g => new { MaLop = g.Key, TenLop = g.First().TenLop })
            .OrderBy(lop => lop.TenLop)
            .ToList();

        // Thêm tùy chọn "Tất cả các lớp" vào đầu danh sách
        dsLop.Insert(0, new { MaLop = "ALL", TenLop = "-- Tất cả các lớp --" });

        cboLop.DisplayMember = "TenLop";
        cboLop.ValueMember = "MaLop";
        cboLop.DataSource = dsLop;

        cboKetQua.Items.Clear();
        cboKetQua.Items.Add("Tất cả");
        cboKetQua.Items.Add("Đạt");
        cboKetQua.Items.Add("Không đạt");
        cboKetQua.SelectedIndex = 0;
    }

    private void XemBaoCao(bool thongBao)
    {
        if (cboLop.SelectedValue == null) return;

        string maLop = cboLop.SelectedValue.ToString() ?? "ALL";
        string ketQua = cboKetQua.SelectedItem?.ToString() ?? "Tất cả";
        string tuKhoa = txtTuKhoa.Text.Trim().ToLower();

        // Lọc chính xác theo MaLop hoặc lấy toàn bộ nếu chọn "ALL"
        var query = (maLop == "ALL")
            ? danhSachSinhVien.AsEnumerable()
            : danhSachSinhVien.Where(sv => sv.MaLop == maLop);

        if (ketQua == "Đạt")
            query = query.Where(sv => sv.Diem >= 5);
        else if (ketQua == "Không đạt")
            query = query.Where(sv => sv.Diem < 5);

        if (!string.IsNullOrWhiteSpace(tuKhoa))
            query = query.Where(sv => sv.MaSV.ToLower().Contains(tuKhoa) || sv.HoTen.ToLower().Contains(tuKhoa));

        var baoCao = query
            .OrderByDescending(sv => sv.Diem)
            .ThenBy(sv => sv.HoTen)
            .Select(sv => new { sv.MaSV, sv.HoTen, sv.TenLop, sv.Diem, sv.KetQua })
            .ToList();

        dgvBaoCao.DataSource = baoCao;
        CapNhatThongKe(baoCao.Count);

        if (baoCao.Count == 0 && thongBao)
        {
            MessageBox.Show("Không có dữ liệu phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void CapNhatThongKe(int soDongHienThi)
    {
        string maLop = cboLop.SelectedValue?.ToString() ?? "ALL";

        var dsLop = (maLop == "ALL")
            ? danhSachSinhVien
            : danhSachSinhVien.Where(sv => sv.MaLop == maLop).ToList();

        int tongSo = dsLop.Count;
        int soDat = dsLop.Count(sv => sv.Diem >= 5);
        int soKhongDat = dsLop.Count(sv => sv.Diem < 5);
        double diemTB = tongSo > 0 ? dsLop.Average(sv => sv.Diem) : 0;

        string tenLopHienThi = maLop == "ALL" ? "Tất cả các lớp" : (cboLop.Text ?? maLop);
        lblThongKe.Text = $"Đang hiển thị: {soDongHienThi} | Tổng ({tenLopHienThi}): {tongSo} | Đạt: {soDat} | " +
                          $"Không đạt: {soKhongDat} | ĐTB: {diemTB:0.00}";
    }

    private void btnXemBaoCao_Click(object? sender, EventArgs e) => XemBaoCao(true);

    private void btnThongKe_Click(object? sender, EventArgs e)
    {
        var thongKe = danhSachSinhVien
            .GroupBy(sv => new { sv.MaLop, sv.TenLop })
            .Select(g => new
            {
                g.Key.MaLop,
                g.Key.TenLop,
                SoLuong = g.Count(),
                SoDat = g.Count(sv => sv.Diem >= 5),
                SoKhongDat = g.Count(sv => sv.Diem < 5),
                DiemTrungBinh = Math.Round(g.Average(sv => sv.Diem), 2)
            })
            .OrderBy(x => x.TenLop)
            .ToList();

        dgvBaoCao.DataSource = thongKe;
        lblThongKe.Text = $"Thống kê theo lớp: {thongKe.Count} lớp, {thongKe.Sum(x => x.SoLuong)} sinh viên";
    }

    private void btnXuatFile_Click(object? sender, EventArgs e) => XuatFile();

    private void XuatFile()
    {
        if (dgvBaoCao.Rows.Count == 0)
        {
            MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "CSV file (*.csv)|*.csv|Text file (*.txt)|*.txt";
        sfd.FileName = "BaoCaoSinhVien.csv";
        if (sfd.ShowDialog() != DialogResult.OK) return;

        try
        {
            char sep = Path.GetExtension(sfd.FileName).Equals(".txt", StringComparison.OrdinalIgnoreCase) ? '\t' : ',';
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Join(sep, dgvBaoCao.Columns.Cast<DataGridViewColumn>()
                .Select(c => DocGiaTri(c.HeaderText, sep))));

            foreach (DataGridViewRow row in dgvBaoCao.Rows)
            {
                if (row.IsNewRow) continue;
                sb.AppendLine(string.Join(sep, row.Cells.Cast<DataGridViewCell>()
                    .Select(c => DocGiaTri(c.Value?.ToString() ?? string.Empty, sep))));
            }

            File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("Xuất file thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static string DocGiaTri(string value, char sep)
    {
        if (value.IndexOfAny(new[] { sep, '"', '\n', '\r' }) < 0) return value;
        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }
}