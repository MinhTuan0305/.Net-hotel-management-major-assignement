# Thư mục Core – lớp dùng chung

Mọi form đều gọi vào các lớp ở đây. **Không sửa file trong Core mà không báo nhóm.**
Muốn thêm hàm thì chỉ thêm hàm mới, không đổi tên hoặc tham số của hàm cũ.
Code viết cho .NET Framework 4.8 / C# 7.3.

| File | Để làm gì | Hay dùng nhất |
| --- | --- | --- |
| `Constants.cs` | Giá trị cứng khớp CHECK trong DB: trạng thái, loại tiền, phương thức, vai trò, mã lỗi 50001–50007, mốc nghiệp vụ, thông tin khách sạn. Có `ToVietnamese()` và `RoomStatus.ToColor()` | `RoomStatus.Available`, `StayStatus.ToVietnamese(s)` |
| `DbHelper.cs` | Cửa duy nhất gọi SQL Server: procedure, view, function, tham số OUTPUT, tham số bảng, transaction | `Query`, `QueryText`, `Exec`, `P`, `Out`, `Value<T>` |
| `ErrorHandler.cs` | Đổi lỗi SQL thành thông báo tiếng Việt, ghi log, bắt lỗi toàn cục | `ErrorHandler.Show(ex, this)`, `ErrorHandler.Try(...)` |
| `Session.cs` | Nhân viên đang đăng nhập; kiểm tra đăng nhập, đổi mật khẩu; phân quyền | `Session.EmployeeParam()`, `Session.IsManager` |
| `PasswordHasher.cs` | Băm và kiểm tra mật khẩu bằng BCrypt | `Hash`, `Verify` |
| `Formatters.cs` | Định dạng tiền, ngày giờ, %, số phút; đọc số tiền gõ vào; đọc tiền thành chữ | `Money`, `TryParseMoney`, `MoneyToWords` |
| `Lookups.cs` | Nạp ComboBox (từ DB hoặc danh sách cố định), đọc giá trị đang chọn | `LoadServices`, `SelectedInt`, `SelectValue` |
| `UiHelper.cs` | Kiểu lưới chung, gán dữ liệu, đặt tiêu đề cột, dịch trạng thái, tô màu, hộp thông báo, kiểm tra nhập, phân quyền giao diện, xuất CSV | `StyleGrid`, `Bind`, `ShowColumns`, `Confirm` |
| `FormManager.cs` | Mở form / hộp thoại theo một cách chung, báo "dữ liệu đã đổi" cho màn hình khác | `Open<T>()`, `ShowDialog(...)`, `NotifyDataChanged` |

**Hàm bổ sung ngày 26/10/2026** (chỉ thêm mới, không đổi hàm cũ):

| Hàm | Để làm gì |
| --- | --- |
| `UiHelper.RequireRole(form, roles...)` | Gọi đầu `Load`: không đủ quyền thì báo và tự đóng form (ẩn menu thôi là chưa đủ) |
| `UiHelper.InputText(owner, title, prompt, out value, ...)` | Hộp nhập nhanh một dòng: lý do void, ghi chú bảo trì, báo hỏng |
| `UiHelper.IsValidEmail / IsValidPhone / IsValidIdNumber` | Kiểm tra định dạng email, SĐT (8–15 số), CCCD/hộ chiếu (6–20 chữ/số); chuỗi rỗng coi là hợp lệ |
| `DbHelper.LikeContains(text)` | Chuỗi tìm kiếm cho LIKE: thoát `[ % _` rồi bọc `%...%`; ô trống trả `null` |
| `DbHelper.ScalarTx<T>(conn, tx, sql, ...)` | Lấy một giá trị bên trong `InTransaction` (vd. id vừa INSERT) |

Ngoài thư mục Core còn có 4 file ở gốc project:

| File | Để làm gì | Lên Git? |
| --- | --- | --- |
| `App.config` | Cấu hình chung, trỏ chuỗi kết nối sang file riêng | Có |
| `connectionStrings.example.config` | Mẫu chuỗi kết nối | Có |
| `connectionStrings.config` | Chuỗi kết nối thật của máy bạn (chép từ file mẫu) | **Không** |
| `Program.cs` | Khởi động: bắt lỗi toàn cục → thử kết nối → đăng nhập → frmMain | Có |

## Cài đặt một lần (trưởng nhóm làm, rồi đẩy lên Git)

1. Tạo project **Windows Forms App (.NET Framework)**, Framework 4.8, tên `HotelManagement`, Location là thư mục `src`.
2. Chép thư mục `Core/` và 4 file ở gốc vào project. Trong Solution Explorer bật **Show All Files**, chuột phải từng file → **Include In Project**.
3. **References** → Add Reference → Assemblies: tích `System.Configuration` (các reference `System.Data`, `System.Drawing`, `System.Windows.Forms` đã có sẵn).
4. **NuGet**: Tools → Options → NuGet Package Manager → Default package management format = **PackageReference**. Sau đó cài gói `BCrypt.Net-Next`.
5. Chọn file `connectionStrings.example.config` → Properties → *Copy to Output Directory* = **Do not copy**.
6. Mở file `HotelManagement.csproj` bằng Notepad, dán đoạn sau ngay trước dòng `</Project>`. Đoạn này tự tạo `connectionStrings.config` từ file mẫu nếu máy chưa có, rồi chép nó vào thư mục chạy:

   ```xml
   <Target Name="EnsureConnectionConfig" BeforeTargets="BeforeBuild"
           Condition="!Exists('$(ProjectDir)connectionStrings.config')">
     <Copy SourceFiles="$(ProjectDir)connectionStrings.example.config"
           DestinationFiles="$(ProjectDir)connectionStrings.config" />
   </Target>
   <Target Name="CopyConnectionConfig" AfterTargets="Build">
     <Copy SourceFiles="$(ProjectDir)connectionStrings.config" DestinationFolder="$(OutDir)" />
   </Target>
   ```

   **Không** Include `connectionStrings.config` vào project. Hai Target ở trên đã lo phần đó.
7. Thêm vào `.gitignore`: `connectionStrings.config` và `Logs/`.
8. Tạo file `.gitattributes` ở gốc repo với dòng `*.csproj merge=union`.
9. `Program.cs` tham chiếu `frmLogin`, `frmMain` (và thuộc tính `frmMain.LoggedOut`). Nếu chưa có hai form này thì tạm giữ `Program.cs` mặc định của VS, chỉ thêm `ErrorHandler.Install();` trước `Application.Run`.

## Mỗi thành viên sau khi clone

1. Build một lần: file `connectionStrings.config` tự được tạo từ file mẫu.
2. Mở `connectionStrings.config`, sửa `Server=` cho đúng máy mình (`.\SQLEXPRESS`, `localhost`...).
3. Chạy `database\reset_db.bat` (hoặc chạy 01 → 03 → 02 trong SSMS), rồi F5.

## Mẫu code trong một form

```csharp
using HotelManagement.Core;

public partial class dlgCheckIn : Form
{
    private readonly int _bookingRoomId;

    public dlgCheckIn(int bookingRoomId)
    {
        InitializeComponent();
        _bookingRoomId = bookingRoomId;
        UiHelper.SetupDialog(this, btnOk, btnCancel);
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            DbHelper.Exec("dbo.sp_CheckInRoom",
                DbHelper.P("@BookingRoomId", _bookingRoomId),
                DbHelper.P("@GuestName", txtGuestName.Text),
                DbHelper.P("@GuestIdNumber", txtGuestId.Text),
                Session.EmployeeParam());

            FormManager.NotifyDataChanged(DataArea.Stays);
            FormManager.NotifyDataChanged(DataArea.Rooms);
            DialogResult = DialogResult.OK;   // đóng hộp thoại, form gọi sẽ nạp lại
        }
        catch (Exception ex) when (ErrorHandler.GetSqlNumber(ex) == SqlErrorCodes.Money)
        {
            // Chưa đủ cọc: hỏi mở hộp thoại thu tiền
            if (UiHelper.Confirm(ex.Message + "\n\nThu cọc ngay?", false, this))
            {
                // FormManager.ShowDialog(new dlgPayment(PaymentMode.Deposit, bookingId), this);
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.Show(ex, this);
        }
    }
}
```

Một số lệnh hay dùng khác:

```csharp
// Lưới danh sách (trong Load gọi StyleGrid/TranslateColumn MỘT lần)
UiHelper.StyleGrid(dgvRooms);
UiHelper.TranslateColumn(dgvRooms, "RoomStatus", RoomStatus.ToVietnamese);
UiHelper.ColorRows(dgvRooms, "RoomStatus", RoomStatus.ToColor);
UiHelper.Bind(dgvRooms, DbHelper.QueryText("SELECT * FROM dbo.vw_RoomBoard ORDER BY RoomNumber"), "RoomId");
UiHelper.ShowColumns(dgvRooms, "RoomNumber", "Phòng", "TypeName", "Loại", "RoomStatus", "Trạng thái");

// Procedure có OUTPUT
SqlParameter invoiceId = DbHelper.Out("@InvoiceId", SqlDbType.Int);
DbHelper.Exec("dbo.sp_CheckOutRoom", /* ... các tham số khác ... */ Session.EmployeeParam(), invoiceId);
int id = DbHelper.OutValue<int>(invoiceId);

// Procedure trả nhiều bảng
DataSet ds = DbHelper.QueryMulti("dbo.sp_RevenueReport",
    DbHelper.P("@FromDate", dtpFrom.Value.Date), DbHelper.P("@ToDate", dtpTo.Value.Date),
    DbHelper.P("@GroupBy", "DAY"), Session.EmployeeParam());
UiHelper.Bind(dgvCash, ds.Tables[0]);
UiHelper.Bind(dgvInvoice, ds.Tables[1]);
```
