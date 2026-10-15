using System.ComponentModel;

namespace ShopManagerMini
{
    /// <summary>Quản lý khách hàng: thêm, sửa, xóa, tìm kiếm trong bộ nhớ.</summary>
    public class FrmCustomer : Form
    {
        private TextBox txtId, txtName, txtPhone, txtSearch;
        private ComboBox cboType;
        private Button btnAdd, btnEdit, btnDelete, btnRefresh, btnSearch;
        private DataGridView dgvCustomers;

        public FrmCustomer()
        {
            Text = "Quản lý khách hàng";
            ClientSize = new Size(760, 460);

            var pnl = new Panel { Dock = DockStyle.Top, Height = 150 };
            txtId = UiHelper.TextBox("txtId", 120, 15);
            txtName = UiHelper.TextBox("txtName", 120, 50, 250);
            txtPhone = UiHelper.TextBox("txtPhone", 490, 15);
            cboType = UiHelper.Combo("cboType", 490, 50, 160, "Thường", "Thân thiết", "VIP");

            btnAdd = UiHelper.Button("btnAdd", "Thêm", 15, 105);
            btnEdit = UiHelper.Button("btnEdit", "Sửa", 105, 105);
            btnDelete = UiHelper.Button("btnDelete", "Xóa", 195, 105);
            btnRefresh = UiHelper.Button("btnRefresh", "Làm mới", 285, 105);
            txtSearch = UiHelper.TextBox("txtSearch", 400, 108, 200);
            btnSearch = UiHelper.Button("btnSearch", "Tìm kiếm", 610, 105, 90);

            pnl.Controls.AddRange(new Control[]
            {
                UiHelper.Label("Mã khách hàng:", 15, 15), txtId,
                UiHelper.Label("Họ tên:", 15, 50), txtName,
                UiHelper.Label("Số điện thoại:", 400, 15), txtPhone,
                UiHelper.Label("Loại KH:", 400, 50), cboType,
                btnAdd, btnEdit, btnDelete, btnRefresh, txtSearch, btnSearch
            });

            dgvCustomers = UiHelper.Grid("dgvCustomers");
            Controls.Add(dgvCustomers);
            Controls.Add(pnl);

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += (s, e) => { txtSearch.Clear(); LoadGrid(DataStore.Customers); };
            btnSearch.Click += BtnSearch_Click;
            dgvCustomers.SelectionChanged += DgvCustomers_SelectionChanged;

            LoadGrid(DataStore.Customers);
        }

        private void LoadGrid(IEnumerable<Customer> data)
        {
            dgvCustomers.DataSource = new BindingList<Customer>(data.ToList());
            dgvCustomers.Columns["Id"].HeaderText = "Mã KH";
            dgvCustomers.Columns["FullName"].HeaderText = "Họ tên";
            dgvCustomers.Columns["Phone"].HeaderText = "Số điện thoại";
            dgvCustomers.Columns["CustomerType"].HeaderText = "Loại khách hàng";
            dgvCustomers.ClearSelection();
            ClearInput();
        }

        private void ClearInput()
        {
            txtId.Clear(); txtName.Clear(); txtPhone.Clear();
            cboType.SelectedIndex = -1;
            txtId.ReadOnly = false;
            txtId.Focus();
        }

        private void DgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0) return;
            var c = dgvCustomers.SelectedRows[0].DataBoundItem as Customer;
            if (c == null) return;

            txtId.Text = c.Id;
            txtName.Text = c.FullName;
            txtPhone.Text = c.Phone;
            cboType.SelectedItem = c.CustomerType;
            txtId.ReadOnly = true;
        }

        private bool TryReadInput(out Customer c)
        {
            c = null;
            string id = txtId.Text.Trim(), name = txtName.Text.Trim(), phone = txtPhone.Text.Trim();

            if (id == "" || name == "" || phone == "")
            {
                Warn("Vui lòng nhập đủ mã, họ tên và số điện thoại!");
                return false;
            }
            if (!phone.All(char.IsDigit) || phone.Length < 9 || phone.Length > 11)
            {
                Warn("Số điện thoại phải gồm 9-11 chữ số!");
                return false;
            }
            if (cboType.SelectedIndex < 0)
            {
                Warn("Vui lòng chọn loại khách hàng!");
                return false;
            }

            c = new Customer { Id = id, FullName = name, Phone = phone, CustomerType = cboType.Text };
            return true;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (txtId.ReadOnly)
            {
                Warn("Đang chọn một khách hàng. Hãy bấm 'Làm mới' rồi nhập khách hàng mới.");
                return;
            }
            if (!TryReadInput(out Customer c)) return;

            if (DataStore.Customers.Any(x => x.Id.Equals(c.Id, StringComparison.OrdinalIgnoreCase)))
            {
                Warn("Mã khách hàng đã tồn tại!");
                return;
            }

            DataStore.Customers.Add(c);
            LoadGrid(DataStore.Customers);
            MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (!txtId.ReadOnly)
            {
                Warn("Vui lòng chọn khách hàng cần sửa trong danh sách!");
                return;
            }
            if (!TryReadInput(out Customer c)) return;

            var old = DataStore.Customers.First(x => x.Id == c.Id);
            old.FullName = c.FullName; old.Phone = c.Phone; old.CustomerType = c.CustomerType;

            txtSearch.Clear();
            LoadGrid(DataStore.Customers);
            MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                Warn("Vui lòng chọn khách hàng cần xóa!");
                return;
            }
            var c = (Customer)dgvCustomers.SelectedRows[0].DataBoundItem;

            if (MessageBox.Show($"Bạn có chắc muốn xóa khách hàng '{c.FullName}'?", "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataStore.Customers.Remove(DataStore.Customers.First(x => x.Id == c.Id));
                txtSearch.Clear();
                LoadGrid(DataStore.Customers);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string key = txtSearch.Text.Trim().ToLower();
            var result = DataStore.Customers.Where(c =>
                c.Id.ToLower().Contains(key) || c.FullName.ToLower().Contains(key) || c.Phone.Contains(key));
            LoadGrid(result);
            if (dgvCustomers.Rows.Count == 0)
                MessageBox.Show("Không tìm thấy khách hàng phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void Warn(string msg) =>
            MessageBox.Show(msg, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
