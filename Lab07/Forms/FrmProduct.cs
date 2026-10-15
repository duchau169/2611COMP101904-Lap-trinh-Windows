using System.ComponentModel;

namespace ShopManagerMini
{
    /// <summary>Quản lý sản phẩm: CRUD trong bộ nhớ.</summary>
    public class FrmProduct : Form
    {
        private TextBox txtId, txtName, txtPrice, txtQty, txtSearch;
        private ComboBox cboCategory;
        private Button btnAdd, btnEdit, btnDelete, btnRefresh, btnSearch;
        private DataGridView dgvProducts;

        public FrmProduct()
        {
            Text = "Quản lý sản phẩm";
            ClientSize = new Size(760, 480);

            var pnl = new Panel { Dock = DockStyle.Top, Height = 170 };
            txtId = UiHelper.TextBox("txtId", 110, 15);
            txtName = UiHelper.TextBox("txtName", 110, 50, 250);
            cboCategory = UiHelper.Combo("cboCategory", 110, 85, 160, "Đồ uống", "Thực phẩm", "Gia dụng", "Khác");
            txtPrice = UiHelper.TextBox("txtPrice", 480, 15);
            txtQty = UiHelper.TextBox("txtQty", 480, 50);

            btnAdd = UiHelper.Button("btnAdd", "Thêm", 15, 125);
            btnEdit = UiHelper.Button("btnEdit", "Sửa", 105, 125);
            btnDelete = UiHelper.Button("btnDelete", "Xóa", 195, 125);
            btnRefresh = UiHelper.Button("btnRefresh", "Làm mới", 285, 125);
            txtSearch = UiHelper.TextBox("txtSearch", 400, 128, 200);
            btnSearch = UiHelper.Button("btnSearch", "Tìm kiếm", 610, 125, 90);

            pnl.Controls.AddRange(new Control[]
            {
                UiHelper.Label("Mã sản phẩm:", 15, 15), txtId,
                UiHelper.Label("Tên sản phẩm:", 15, 50), txtName,
                UiHelper.Label("Loại:", 15, 85), cboCategory,
                UiHelper.Label("Đơn giá:", 400, 15), txtPrice,
                UiHelper.Label("Số lượng:", 400, 50), txtQty,
                btnAdd, btnEdit, btnDelete, btnRefresh, txtSearch, btnSearch
            });

            dgvProducts = UiHelper.Grid("dgvProducts");
            Controls.Add(dgvProducts); // Fill trước, rồi mới thêm panel Top
            Controls.Add(pnl);

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += (s, e) => { txtSearch.Clear(); LoadGrid(DataStore.Products); };
            btnSearch.Click += BtnSearch_Click;
            dgvProducts.SelectionChanged += DgvProducts_SelectionChanged;

            LoadGrid(DataStore.Products);
        }

        /// <summary>Hiển thị danh sách lên DataGridView và đặt tiêu đề cột.</summary>
        private void LoadGrid(IEnumerable<Product> data)
        {
            dgvProducts.DataSource = new BindingList<Product>(data.ToList());
            dgvProducts.Columns["Id"].HeaderText = "Mã SP";
            dgvProducts.Columns["Name"].HeaderText = "Tên sản phẩm";
            dgvProducts.Columns["Category"].HeaderText = "Loại";
            dgvProducts.Columns["Price"].HeaderText = "Đơn giá";
            dgvProducts.Columns["Price"].DefaultCellStyle.Format = "N0";
            dgvProducts.Columns["Quantity"].HeaderText = "Số lượng";
            dgvProducts.ClearSelection();
            ClearInput();
        }

        private void ClearInput()
        {
            txtId.Clear(); txtName.Clear(); txtPrice.Clear(); txtQty.Clear();
            cboCategory.SelectedIndex = -1;
            txtId.ReadOnly = false;
            txtId.Focus();
        }

        private void DgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0) return;
            var p = dgvProducts.SelectedRows[0].DataBoundItem as Product;
            if (p == null) return;

            txtId.Text = p.Id;
            txtName.Text = p.Name;
            cboCategory.SelectedItem = p.Category;
            txtPrice.Text = p.Price.ToString("0.##");
            txtQty.Text = p.Quantity.ToString();
            txtId.ReadOnly = true; // không cho đổi mã khi đang sửa
        }

        /// <summary>Kiểm tra dữ liệu nhập và tạo đối tượng Product.</summary>
        private bool TryReadInput(out Product p)
        {
            p = null;
            string id = txtId.Text.Trim(), name = txtName.Text.Trim();

            if (id == "" || name == "")
            {
                Warn("Mã và tên sản phẩm không được để trống!");
                return false;
            }
            if (cboCategory.SelectedIndex < 0)
            {
                Warn("Vui lòng chọn loại sản phẩm!");
                return false;
            }
            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                Warn("Đơn giá phải là số không âm!");
                return false;
            }
            if (!int.TryParse(txtQty.Text, out int qty) || qty < 0)
            {
                Warn("Số lượng phải là số nguyên không âm!");
                return false;
            }

            p = new Product { Id = id, Name = name, Category = cboCategory.Text, Price = price, Quantity = qty };
            return true;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (txtId.ReadOnly)
            {
                Warn("Đang chọn một sản phẩm. Hãy bấm 'Làm mới' rồi nhập sản phẩm mới.");
                return;
            }
            if (!TryReadInput(out Product p)) return;

            if (DataStore.Products.Any(x => x.Id.Equals(p.Id, StringComparison.OrdinalIgnoreCase)))
            {
                Warn("Mã sản phẩm đã tồn tại!");
                return;
            }

            DataStore.Products.Add(p);
            LoadGrid(DataStore.Products);
            MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (!txtId.ReadOnly)
            {
                Warn("Vui lòng chọn sản phẩm cần sửa trong danh sách!");
                return;
            }
            if (!TryReadInput(out Product p)) return;

            var old = DataStore.Products.First(x => x.Id == p.Id);
            old.Name = p.Name; old.Category = p.Category; old.Price = p.Price; old.Quantity = p.Quantity;

            txtSearch.Clear();
            LoadGrid(DataStore.Products);
            MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                Warn("Vui lòng chọn sản phẩm cần xóa!");
                return;
            }
            var p = (Product)dgvProducts.SelectedRows[0].DataBoundItem;

            if (MessageBox.Show($"Bạn có chắc muốn xóa sản phẩm '{p.Name}'?", "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataStore.Products.Remove(DataStore.Products.First(x => x.Id == p.Id));
                txtSearch.Clear();
                LoadGrid(DataStore.Products);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string key = txtSearch.Text.Trim().ToLower();
            var result = DataStore.Products.Where(p =>
                p.Id.ToLower().Contains(key) || p.Name.ToLower().Contains(key) || p.Category.ToLower().Contains(key));
            LoadGrid(result);
            if (dgvProducts.Rows.Count == 0)
                MessageBox.Show("Không tìm thấy sản phẩm phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void Warn(string msg) =>
            MessageBox.Show(msg, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
