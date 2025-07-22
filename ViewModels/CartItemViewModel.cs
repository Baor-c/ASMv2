namespace FastFoodApp.ViewModels
{
    public class CartItemViewModel
    {
        public int ItemId { get; set; }
        public string TenSanPham { get; set; }
        public string HinhAnh { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien => DonGia * SoLuong;

        public bool IsCombo { get; set; }
        public string TenMonAn { get; internal set; }
        public int MaMonAn { get; internal set; }
    }
}
