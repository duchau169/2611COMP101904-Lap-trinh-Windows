namespace ShopManagerMini
{
    /// <summary>Hàm hỗ trợ tạo control nhanh, tránh lặp code.</summary>
    internal static class UiHelper
    {
        public static Label Label(string text, int x, int y) =>
            new Label { Text = text, Left = x, Top = y + 3, AutoSize = true };

        public static TextBox TextBox(string name, int x, int y, int w = 160) =>
            new TextBox { Name = name, Left = x, Top = y, Width = w };

        public static Button Button(string name, string text, int x, int y, int w = 80) =>
            new Button { Name = name, Text = text, Left = x, Top = y, Width = w, Height = 30 };

        public static ComboBox Combo(string name, int x, int y, int w, params string[] items)
        {
            var cbo = new ComboBox { Name = name, Left = x, Top = y, Width = w, DropDownStyle = ComboBoxStyle.DropDownList };
            cbo.Items.AddRange(items);
            return cbo;
        }

        public static DataGridView Grid(string name)
        {
            return new DataGridView
            {
                Name = name,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.Window
            };
        }
    }
}
