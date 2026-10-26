# CLAUDE.md – Hệ thống quản lý khách sạn (đồ án môn .NET)

File này mô tả dự án cho Claude Code. Đọc hết file này trước khi làm bất cứ việc gì.
Chi tiết từng form nằm trong `docs/KE_HOACH_TRIEN_KHAI.md`; nghiệp vụ đầy đủ nằm trong `docs/Tai_lieu_nghiep_vu.md`.

## 1. Bối cảnh

- Đồ án lớn môn .NET, làm nhóm 4 người. Ứng dụng desktop **Windows Forms, .NET Framework 4.8, C# 7.3**, database **SQL Server**.
- Người đang làm việc với bạn: **Lê Minh Tuấn**, sinh viên. Phụ trách:
  - giữ database (script trong `database/`);
  - 7 form của phần lưu trú và hóa đơn: `frmFrontDesk`, `dlgCheckIn`, `frmServiceUsage`, `dlgExtendStay`, `frmCheckOut`, `frmInvoices`, `frmInvoicePrint`.
- Các thành viên khác làm phần còn lại (xem mục 7). Khi Tuấn nhờ làm form của người khác thì vẫn làm, nhưng nhắc rằng form đó thuộc phần ai.
- Trả lời và viết chú thích bằng **tiếng Việt có dấu**. Giải thích ngắn gọn vì sao làm vậy, vì người dùng đang học.

## 2. Trạng thái hiện tại (cập nhật 26/10/2026)

| Hạng mục | Trạng thái |
| --- | --- |
| Database: 3 script `database/01..03` | Xong, đã chạy trên SQL Server 2022, file `03` PASS hết (14/10/2026) |
| Lớp dùng chung `Core/` (9 file) | Xong. 26/10 thêm 7 hàm mới (không đổi hàm cũ): `UiHelper.RequireRole`, `InputText`, `IsValidEmail/Phone/IdNumber`; `DbHelper.LikeContains`, `ScalarTx` – xem `Core/README.md` |
| `.csproj` đã khai báo Core, BCrypt.Net-Next 4.2.0, System.Configuration, System.Windows.Forms.DataVisualization (biểu đồ frmReports), 2 target tạo `connectionStrings.config` | Xong |
| `Program.cs` | Xong (26/10): vòng lặp frmLogin → frmMain, Đăng xuất quay lại frmLogin. Đã bỏ code tạm |
| Git | Repo GitHub `MinhTuan0305/.Net-hotel-management-major-assignement`, nhánh `main` |
| `database/reset_db.bat` | Xong (14/10/2026), đã thử nhánh hỏi server/hủy; chưa chạy thật trọn 01→03→02 |
| `database/CHANGELOG.md` | Xong (15/10/2026). Mỗi lần sửa script DB thì thêm mục mới lên đầu |
| 23 form | 26/10: đã viết code đủ 23 form + `ucSettlement` + form bổ sung `dlgReissueInvoice` (Forms/Billing). Rebuild 0 lỗi 0 cảnh báo; 36 câu SQL hằng số chạy thử trên DB đều OK; tên tham số procedure khớp DB. **Chưa thử F5** – xem kịch bản thử trong báo cáo/kế hoạch mục 7 |

Khi xong một hạng mục, cập nhật bảng này và cột "Trạng thái" ở mục 7.

## 3. Cấu trúc thư mục (thực tế trên máy)

```
Hotel_management_system/              <- thư mục gốc = thư mục project (không có src/)
  Hotel_management_system.slnx        <- solution (định dạng .slnx của VS mới)
  Hotel_management_system.csproj      <- project KIỂU CŨ (non-SDK), phải khai báo từng file
  App.config                          <- trỏ connectionStrings sang file riêng
  connectionStrings.example.config    <- mẫu (lên Git)
  connectionStrings.config            <- riêng từng máy, KHÔNG lên Git, tự tạo khi build
  Program.cs
  Core/                               <- lớp dùng chung, namespace HotelManagement.Core
  Forms/                              <- Main/ Catalog/ Booking/ Stay/ Billing/ Operations/ (đủ 23 form + ucSettlement)
  Properties/
  database/  01_HotelManagement_TaoDB.sql, 02_..._DuLieuMau.sql, 03_..._KiemThu.sql
  docs/      Tai_lieu_nghiep_vu.md (+ .docx gốc), KE_HOACH_TRIEN_KHAI.md
```

- RootNamespace của project là `Hotel_management_system`. Form đặt trong `Forms/<Nhóm>/` có namespace `Hotel_management_system.Forms.<Nhóm>`.
- **Không đặt thư mục/namespace tên `System`** (vd. `Forms.System`): mọi `System.Windows.Forms...` trong file Designer sẽ bị hiểu nhầm và hỏng build cả project. Vì vậy frmLogin, frmMain, dlgChangePassword nằm ở `Forms/Main`.
- Form ở thư mục khác thì thêm `using` namespace của thư mục đó (vd. frmFrontDesk mở dlgPayment cần `using Hotel_management_system.Forms.Billing;`).
- Trong `.csproj`, form khai báo theo **khối `<ItemGroup>` riêng của từng người** (có comment tên). Thêm form mới thì thêm vào đúng khối của mình, để tránh xung đột Git.
- Core dùng namespace `HotelManagement.Core`. Mọi form phải có `using HotelManagement.Core;`.

## 4. Build, chạy, database

Build bằng MSBuild của Visual Studio (không dùng `dotnet build` vì đây là project .NET Framework kiểu cũ). Trong PowerShell:

```powershell
$msbuild = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -requires Microsoft.Component.MSBuild -find "MSBuild\**\Bin\MSBuild.exe" | Select-Object -First 1
& $msbuild Hotel_management_system.csproj /restore /t:Build /p:Configuration=Debug /v:minimal
```

- Máy của Tuấn dùng **Visual Studio 18 Insiders** (bản prerelease), nên `vswhere` phải có `-prerelease`, nếu không sẽ không tìm thấy MSBuild.
  Đường dẫn thực tế: `C:\Program Files\Microsoft Visual Studio\18\Insiders\MSBuild\Current\Bin\MSBuild.exe`.
- Muốn chắc chắn code được biên dịch lại thật thì dùng `/t:Rebuild` (VS hay báo "up-to-date" mà không biên dịch).

- Sau khi sửa code luôn build lại và sửa hết lỗi trước khi báo xong.
- Chạy chương trình và thử giao diện thì để **người dùng tự làm trong Visual Studio** (F5). Bạn mô tả cần thử những gì.
- Database: tên `HotelManagement`. **Máy của Tuấn dùng SQL Server 2022 bản mặc định (`MSSQLSERVER`), server là `.`**; thành viên khác có thể là `.\SQLEXPRESS` (xem `connectionStrings.config` của từng máy). Làm mới DB trên máy Tuấn:
  ```
  database\reset_db.bat .            (chạy 01 -> 03 -> 02, dừng nếu lỗi hoặc có ca FAIL; máy khác: .\SQLEXPRESS)
  ```
  Hoặc chạy tay từng file (bắt buộc có `-I` vì file 01 có filtered index; `-C` cho sqlcmd mới):
  ```
  sqlcmd -S . -E -C -b -I -f 65001 -i database\01_HotelManagement_TaoDB.sql
  sqlcmd -S . -E -C -b -I -f 65001 -i database\03_HotelManagement_KiemThu.sql   (phải ra toàn PASS)
  sqlcmd -S . -E -C -b -I -f 65001 -i database\02_HotelManagement_DuLieuMau.sql
  ```
  File 03 có ca FAIL vẫn kết thúc "thành công"; phải xem bảng kết quả (cột `Failed` = 0).
  File 01 DROP và tạo lại DB, nên dữ liệu cũ mất hết. Hỏi người dùng trước khi chạy.
  Dữ liệu mẫu tính ngày theo `GETDATE()`, nên các màn hình "hôm nay" chỉ đúng trong ngày chạy file 02.
- Tài khoản mẫu: `admin` / `Admin@123`; nhân viên mẫu `quanlm, hapt, lantt, namnh, maivt, hungdv` / `123456`; `tunghv` đã bị khóa.

## 5. Quy tắc bắt buộc khi viết code

### Ngôn ngữ và project
- **C# 7.3**: KHÔNG dùng switch expression (`x switch { }`), `using var`, `record`, `new()` không ghi kiểu, `??=`, pattern `or/and/not`, kiểu tham chiếu nullable (`string?`), range/index (`..`, `^1`). Dùng `switch/case`, `using (...) { }`, `is T x`, `out`, tuple, interpolation thì được.
- **`.csproj` kiểu cũ**: mỗi file mới PHẢI được thêm vào `Hotel_management_system.csproj`, nếu không VS sẽ không thấy file đó. Một form gồm 3 mục:
  ```xml
  <Compile Include="Forms\Stay\frmCheckOut.cs">
    <SubType>Form</SubType>
  </Compile>
  <Compile Include="Forms\Stay\frmCheckOut.Designer.cs">
    <DependentUpon>frmCheckOut.cs</DependentUpon>
  </Compile>
  <EmbeddedResource Include="Forms\Stay\frmCheckOut.resx">
    <DependentUpon>frmCheckOut.cs</DependentUpon>
  </EmbeddedResource>
  ```
  Sửa `.csproj` xong thì nhắc người dùng: nếu VS hỏi file bị đổi thì bấm **Reload**, **không bấm Save As/Overwrite**. Lần trước bấm Save As đã làm mất khai báo file.
- **File `.Designer.cs`** phải đúng mẫu VS sinh ra: `partial class`, `private System.ComponentModel.IContainer components = null;`, `Dispose(bool)`, `InitializeComponent()` chỉ gán thuộc tính và `Controls.Add`. Không viết logic trong Designer. Có thể không tạo `.resx` nếu form không có tài nguyên; khi đó bỏ mục EmbeddedResource.
- File `.cs` lưu UTF-8 có BOM, xuống dòng CRLF (giống VS).
- Tên control dùng tiền tố: `txt cbo dgv btn dtp nud lbl chk tab grp pnl`.

### Dùng lớp Core, không tự viết lại
- Gọi DB **chỉ qua `DbHelper`**: `Query`, `QueryText`, `QueryMulti`, `Exec`, `ExecText`, `Scalar<T>`, `InTransaction`; tham số `P`, `Out`, `Tvp`; đọc giá trị `Value<T>`, `OutValue<T>`. **Không nối chuỗi SQL.**
- Mọi procedure có `@EmployeeId` thì truyền `Session.EmployeeParam()`.
- Mọi nút gọi DB: `try { ... } catch (Exception ex) { ErrorHandler.Show(ex, this); }`. Không tự viết MessageBox báo lỗi SQL.
- Chuỗi trạng thái lấy từ `Constants.cs` (`RoomStatus.Available`, `StayStatus.CheckedIn`...). Hiển thị chữ Việt bằng `ToVietnamese`.
- Tiền là `decimal`, hiển thị bằng `Formatters.Money`. Ngày giờ hiển thị bằng `Formatters.Date` / `DateTimeText`.
- Lưới: `UiHelper.StyleGrid`, `Bind`, `ShowColumns`, `TranslateColumn`, `ColorRows`, `SelectedId`. ComboBox: `Lookups.*`.
- Mở form: `FormManager.Open<T>()` cho form lớn, `FormManager.ShowDialog(new dlgX(...), this)` cho hộp thoại. Sau khi lưu thì gọi `FormManager.NotifyDataChanged(DataArea.X)`.
- Hộp thoại gọi `UiHelper.SetupDialog(this, btnOk, btnCancel)` trong constructor, và đặt `DialogResult = DialogResult.OK` khi lưu thành công.
- `Core/` dùng chung cả nhóm: **chỉ thêm hàm mới, không đổi tên hay tham số hàm cũ**. Sửa Core thì nói rõ với người dùng để họ báo nhóm.

### Nghiệp vụ nằm trong database
- Quy tắc nghiệp vụ (quyền, trùng lịch, cọc, phụ thu, giảm giá, chặn trạng thái) **đã có trong procedure**. Form chỉ kiểm tra nhập liệu cơ bản (ô trống, số âm) rồi gọi procedure. Lỗi nghiệp vụ hiện ra bằng `ErrorHandler`. Không viết lại logic này bằng C#.
- **Không INSERT/UPDATE trực tiếp** vào `Bookings`, `BookingRooms`, `ServiceUsages`, `Invoices`, `Payments`, `HousekeepingTasks` hay `Rooms.Status`. Luôn đi qua procedure. Bảng danh mục (Customers, RoomTypes, Rooms trừ Status, Amenities, RoomTypeAmenities, Services, Employees) được ghi bằng `ExecText` có tham số.
- Cần đổi schema: sửa `database/01` (và 02, 03 nếu liên quan), giữ nguyên tên và tham số cũ (tham số mới phải có giá trị mặc định), ghi `database/CHANGELOG.md`, rồi nhắc người dùng báo nhóm chạy lại DB.
- Tài liệu nghiệp vụ và DB khác nhau ở đâu thì **file `database/01` là chuẩn**.

### An toàn
- Không đụng `connectionStrings.config`, `bin/`, `obj/`, `.vs/`.
- Không chạy script DB, không `git commit`/`push` khi người dùng chưa đồng ý.

## 6. Tóm tắt database (`database/01_HotelManagement_TaoDB.sql`)

**Mô hình:** lượt lưu trú từng phòng (`BookingRooms`, khóa `BookingRoomId`) là đơn vị trung tâm. Check-in, dịch vụ, gia hạn, check-out, hóa đơn, thanh toán đều theo **từng phòng**, không gộp theo phiếu. Phiếu (`Bookings`) chỉ gom các phòng của một khách đặt; trạng thái phiếu do DB tự tính.

**14 bảng:** Roles, Employees, Customers, RoomTypes, Rooms, Amenities, RoomTypeAmenities, Bookings, BookingRooms, Services, ServiceUsages, Invoices, Payments, HousekeepingTasks.

**Quy tắc chính:**
- Giờ nhận phòng 14:00, giờ trả 12:00.
- Trả trễ: ≤ 30 phút miễn phụ thu; 30–120 phút tính 5% giá đêm cho mỗi 30 phút; trên 120 phút tính 15% + 15% cho mỗi giờ; sau 18:00 phải gia hạn thêm 1 đêm. Phụ thu ghi thành dịch vụ hệ thống "Phụ thu trả phòng trễ".
- Cọc 50% hoặc 100%, tính bằng `ROUND(giá × số đêm × tỷ lệ)`. Phiếu Pending quá 24 giờ thì tự hủy. Hủy trước giờ nhận ≥ 48 giờ được hoàn cọc. No-show đánh dấu sau 12:00 ngày hôm sau.
- Check-in chỉ khi phiếu Confirmed và phòng đủ cọc. Check-out phải thu đủ 100%; khách trả dư thì phải chọn cách hoàn.
- Bảng tính check-out hết hạn sau 10 phút. Phải truyền đúng `CheckOutAt` lấy từ bước xem trước.
- Lễ tân giảm giá tối đa 10%, quá mức này phải là Manager/Admin; giảm giá phải ghi lý do. VAT 10%.
- Hóa đơn chỉ có 2 trạng thái Paid/Void, mỗi phòng tối đa 1 hóa đơn Paid. Void rồi lập lại được (`ReplacesInvoiceId`). Sau khi void, chỉ Admin/Manager được sửa dịch vụ của phòng đó.
- Trạng thái phòng: Available → Occupied → Cleaning → Available; Cleaning hoặc Available → Maintenance; Maintenance → Cleaning. Không có Reserved.
- 4 vai trò: Admin, Manager, Receptionist, Housekeeping.

**Mã lỗi THROW:** 50001 quyền, 50002 không tìm thấy, 50003 sai trạng thái, 50004 dữ liệu không hợp lệ, 50005 trùng lịch, 50006 tiền, 50007 ngày giờ (hằng số trong `SqlErrorCodes`).

**Type:** `dbo.BookingRoomInput` (RoomId, CheckInDate, CheckOutDate, Adults, Children, GuestName, GuestIdNumber). Dùng `DbHelper.CreateBookingRoomInput()` / `AddBookingRoom()`.

**Procedure và function** (gọi bằng tên đầy đủ có `dbo.`):

- `dbo.fn_HasRole` (@EmployeeId INT, @Roles NVARCHAR(200))
- `dbo.fn_LateCheckoutFee` (@PricePerNight DECIMAL(14,0), @CheckOutDate DATE, @ActualCheckOut DATETIME2(0))
- `dbo.fn_StayNetPaid` (@BookingRoomId INT) – số tiền thực đã thu của phòng
- `dbo.fn_RequiredDeposit` (@BookingRoomId INT) – số cọc yêu cầu của phòng
- `dbo.fn_AvailableRooms` (@CheckIn DATE, @CheckOut DATE, @ExcludeBookingRoomId INT) – bảng; `SELECT * FROM dbo.fn_AvailableRooms(@a, @b, NULL)`
- `dbo.fn_CheckOutQuote` (@BookingRoomId INT, @CheckOutAt DATETIME2(0), @Discount DECIMAL(14,0), @TaxRate DECIMAL(5,2)) – bảng
- `dbo.sp_GetEmployeeForLogin` @Username VARCHAR(50)
- `dbo.sp_CreateBooking` @CustomerId INT, @DepositRate TINYINT, @Note NVARCHAR(500) = NULL, @EmployeeId INT, @Rooms dbo.BookingRoomInput READONLY, @BookingId INT OUTPUT
- `dbo.sp_UpdateBookingRoom` @BookingRoomId INT, @NewRoomId INT, @CheckInDate DATE, @CheckOutDate DATE, @Adults TINYINT, @Children TINYINT = 0, @KeepPrice BIT = 0, @Reason NVARCHAR(200) = NULL, @EmployeeId INT
- `dbo.sp_CancelBookingRoom` @BookingRoomId INT, @Reason NVARCHAR(200) = NULL, @IsNoShow BIT = 0, @RefundMethod VARCHAR(20) = NULL, @RefundReference VARCHAR(50) = NULL, @EmployeeId INT, @RefundAmount DECIMAL(14,0) = NULL OUTPUT
- `dbo.sp_CancelBooking` @BookingId INT, @Reason NVARCHAR(200), @RefundMethod VARCHAR(20) = NULL, @RefundReference VARCHAR(50) = NULL, @EmployeeId INT, @TotalRefund DECIMAL(14,0) = NULL OUTPUT
- `dbo.sp_ExpirePendingBookings` (không tham số – gọi khi mở frmMain)
- `dbo.sp_RecordBookingDeposit` @BookingId INT, @Amount DECIMAL(14,0), @Method VARCHAR(20), @ReferenceNo VARCHAR(50) = NULL, @EmployeeId INT
- `dbo.sp_RecordPayment` @BookingRoomId INT, @Type VARCHAR(10), @Amount DECIMAL(14,0), @Method VARCHAR(20), @ReferenceNo VARCHAR(50) = NULL, @Note NVARCHAR(200) = NULL, @EmployeeId INT, @PaymentId INT = NULL OUTPUT
- `dbo.sp_CheckInRoom` @BookingRoomId INT, @GuestName NVARCHAR(100) = NULL, @GuestIdNumber VARCHAR(20) = NULL, @EmployeeId INT
- `dbo.sp_AddServiceUsage` @BookingRoomId INT, @ServiceId INT, @Quantity INT, @UsedAt DATETIME2(0) = NULL, @Note NVARCHAR(200) = NULL, @EmployeeId INT, @UsageId INT = NULL OUTPUT
- `dbo.sp_UpdateServiceUsage` @UsageId INT, @Quantity INT, @UsedAt DATETIME2(0), @Note NVARCHAR(200) = NULL, @EmployeeId INT
- `dbo.sp_DeleteServiceUsage` @UsageId INT, @EmployeeId INT
- `dbo.sp_ExtendStay` @BookingRoomId INT, @NewCheckOutDate DATE, @Reason NVARCHAR(200) = NULL, @EmployeeId INT
- `dbo.sp_PreviewCheckOut` @BookingRoomId INT, @CheckOutAt DATETIME2(0) = NULL, @Discount DECIMAL(14,0) = 0, @TaxRate DECIMAL(5,2) = 10 – trả 1 dòng, cột đầu là `CheckOutAt` phải giữ lại
- `dbo.sp_CheckOutRoom` @BookingRoomId INT, @CheckOutAt DATETIME2(0), @Discount DECIMAL(14,0) = 0, @DiscountReason NVARCHAR(200) = NULL, @TaxRate DECIMAL(5,2) = 10, @BuyerName NVARCHAR(150) = NULL, @BuyerCompanyName NVARCHAR(200) = NULL, @BuyerTaxCode VARCHAR(20) = NULL, @BuyerAddress NVARCHAR(255) = NULL, @PayAmount DECIMAL(14,0) = 0, @PayMethod VARCHAR(20) = NULL, @PayReference VARCHAR(50) = NULL, @RefundMethod VARCHAR(20) = NULL, @RefundReference VARCHAR(50) = NULL, @EmployeeId INT, @InvoiceId INT = NULL OUTPUT
- `dbo.sp_VoidInvoice` @InvoiceId INT, @Reason NVARCHAR(200), @EmployeeId INT
- `dbo.sp_ReissueInvoice` – giống sp_CheckOutRoom nhưng bỏ @CheckOutAt; chỉ cho phòng đã trả và có hóa đơn Void
- `dbo.sp_SetRoomMaintenance` @RoomId INT, @Note NVARCHAR(200), @EmployeeId INT
- `dbo.sp_FinishMaintenance` @RoomId INT, @Note NVARCHAR(200) = NULL, @EmployeeId INT
- `dbo.sp_StartHousekeepingTask` @TaskId INT, @EmployeeId INT
- `dbo.sp_CompleteHousekeepingTask` @TaskId INT, @Result VARCHAR(20), @Note NVARCHAR(500) = NULL, @EmployeeId INT
- `dbo.sp_DashboardSummary` @Date DATE = NULL, @EmployeeId INT
- `dbo.sp_RevenueReport` @FromDate DATE, @ToDate DATE, @GroupBy VARCHAR(10) = 'DAY', @EmployeeId INT – trả 2 bảng (thực thu; doanh số theo hóa đơn), dùng `QueryMulti`

**View:**
- `vw_RoomStays` – mọi lượt lưu trú kèm phòng, khách, trạng thái `StayStatus`.
- `vw_RoomStayFolio` – sổ tiền của một phòng.
- `vw_BookingSummary` – danh sách phiếu: cọc yêu cầu, đã cọc, còn thiếu.
- `vw_BookingStatement` – sao kê một phiếu.
- `vw_InvoiceDetail`, `vw_InvoiceLines` – đầu hóa đơn và các dòng hóa đơn.
- `vw_RoomBoard` – sơ đồ phòng: cột `RoomStatus`, khách đang ở, khách sắp đến.
- `vw_TodayArrivals`, `vw_InHouseStays`, `vw_TodayDepartures` – 3 tab của frmFrontDesk.
- `vw_PaymentLedger` – sổ thu.
- `vw_ServiceUsageStats` – thống kê dịch vụ.
- `vw_HousekeepingQueue` – hàng đợi dọn phòng.
- `vw_RoomStatusSummary` – số phòng theo trạng thái.

Tên cột chính xác của từng view: đọc phần `CREATE VIEW` trong file 01 (`grep -n "CREATE VIEW" database/01_HotelManagement_TaoDB.sql`).

## 7. Danh sách 23 form

Thư mục và namespace theo nhóm. "A/M/R/H" = Admin/Manager/Receptionist/Housekeeping. Người làm A, B, C là tên tạm; nhóm có thể đã chia khác.

| # | Form | Thư mục | Người làm | Quyền | Trạng thái |
| --- | --- | --- | --- | --- | --- |
| 1 | frmLogin | Forms/Main | A | tất cả | code xong 26/10, chưa F5 |
| 2 | frmMain | Forms/Main | A | tất cả | code xong 26/10, chưa F5 |
| 3 | dlgChangePassword | Forms/Main | A | tất cả | code xong 26/10, chưa F5 |
| 4 | frmEmployees | Forms/Catalog | A | A | code xong 26/10, chưa F5 |
| 5 | frmCustomers | Forms/Catalog | B | A,M,R | code xong 26/10, chưa F5 |
| 6 | frmRoomTypes | Forms/Catalog | B | A,M | code xong 26/10, chưa F5 |
| 7 | frmAmenities | Forms/Catalog | B | A,M | code xong 26/10, chưa F5 |
| 8 | frmRooms | Forms/Catalog | B | A,M (R xem) | code xong 26/10, chưa F5 |
| 9 | frmServices | Forms/Catalog | B | A,M (R xem) | code xong 26/10, chưa F5 |
| 10 | frmBookings | Forms/Booking | C | A,M,R | code xong 26/10, chưa F5 |
| 11 | frmBookingEdit | Forms/Booking | C | A,M,R | code xong 26/10, chưa F5 |
| 12 | dlgCancel | Forms/Booking | C | A,M,R | code xong 26/10, chưa F5 |
| 13 | dlgPayment | Forms/Billing | C | A,M,R (hoàn: A,M) | code xong 26/10, chưa F5 |
| 14 | **frmFrontDesk** | Forms/Stay | **Tuấn** | A,M,R | code xong 26/10, chưa F5 |
| 15 | **dlgCheckIn** | Forms/Stay | **Tuấn** | A,M,R | code xong 26/10, chưa F5 |
| 16 | **frmServiceUsage** | Forms/Stay | **Tuấn** | A,M,R | code xong 26/10, chưa F5 |
| 17 | **dlgExtendStay** | Forms/Stay | **Tuấn** | A,M,R | code xong 26/10, chưa F5 |
| 18 | **frmCheckOut** | Forms/Stay | **Tuấn** | A,M,R | code xong 26/10, chưa F5 |
| 19 | **frmInvoices** | Forms/Billing | **Tuấn** | A,M,R (void/lập lại: A,M) | code xong 26/10, chưa F5 |
| 20 | **frmInvoicePrint** | Forms/Billing | **Tuấn** | A,M,R | code xong 26/10, chưa F5 |
| 21 | frmPaymentLedger | Forms/Billing | C | A,M | code xong 26/10, chưa F5 |
| 22 | frmHousekeeping | Forms/Operations | C | H,A,M (R xem) | code xong 26/10, chưa F5 |
| 23 | frmReports | Forms/Operations | A | A,M | code xong 26/10, chưa F5 |
| 24 | dlgReissueInvoice (bổ sung) | Forms/Billing | **Tuấn** | A,M | code xong 26/10, chưa F5 – lập lại hóa đơn sau void, dùng `ucSettlement` |

**Constructor đã chốt** (form của người này mở form của người kia, nên không được đổi):

| Form | Constructor |
| --- | --- |
| frmCustomers | `()` và `(bool pickMode)` (tách 2 constructor vì `FormManager.Open<T>()` cần constructor không tham số), chọn xong có thuộc tính `SelectedCustomerId` |
| frmBookingEdit | `(int? bookingId)` – null là tạo mới |
| dlgCancel | `(int bookingId, int? bookingRoomId)` – null là hủy cả phiếu |
| dlgPayment | `(PaymentMode mode, int id)` – Deposit nhận BookingId; StayPayment/Refund nhận BookingRoomId |
| dlgCheckIn, frmServiceUsage, dlgExtendStay, frmCheckOut | `(int bookingRoomId)` |
| frmInvoicePrint | `(int invoiceId)` |
| dlgReissueInvoice | `(int bookingRoomId, int voidInvoiceId)` |
| frmMain | có `public bool LoggedOut { get; private set; }` (Program.cs dùng) |

Đặc tả chi tiết từng form (giao diện, procedure gọi, điều kiện "xong khi", ca kiểm thử): `docs/KE_HOACH_TRIEN_KHAI.md`.

## 8. Thứ tự làm việc gợi ý

1. Build project lần đầu, sửa lỗi Core nếu có. Tạo `database/reset_db.bat`.
2. Tạo `frmFrontDesk` (3 tab đọc view) để có chỗ mở các form khác.
3. `dlgCheckIn` → `frmServiceUsage` → `dlgExtendStay`.
4. `frmInvoicePrint` (frmCheckOut cần nó) → `frmCheckOut`.
5. `frmInvoices` (void và lập lại; tách phần nhập giảm giá/người mua/thu-hoàn thành UserControl dùng chung với frmCheckOut).
6. Trong lúc chưa có `frmMain` và `frmLogin`: tạm sửa `Program.cs` cho `Session.Login(...)` bằng tài khoản `admin` rồi mở thẳng `frmFrontDesk` để thử. Ghi rõ đây là code tạm.
