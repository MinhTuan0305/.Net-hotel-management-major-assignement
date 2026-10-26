# Kế hoạch triển khai – đặc tả từng form

Bản Markdown của tài liệu kế hoạch nhóm (bản trực tuyến: "Kế hoạch triển khai đồ án Quản lý khách sạn"),
đã chỉnh theo cấu trúc thư mục thực tế. Quy tắc code chung xem `CLAUDE.md`.

Mỗi form có 3 phần: **Giao diện** (control chính), **Xử lý** (procedure/view gọi khi bấm), **Xong khi** (điều kiện để coi là xong).

---

## 0. Lộ trình (giả định 6 tuần, 4 người)

| Tuần | Việc | Cổng kiểm tra |
| --- | --- | --- |
| 1 | Môi trường, DB, Core, khung (frmLogin, frmMain, dlgChangePassword) | Core + khung chạy được trên 2 máy |
| 2 | Danh mục; bắt đầu đặt phòng, lưu trú | — |
| 3 | Đặt phòng xong; lưu trú; hóa đơn; dọn phòng, báo cáo | Đóng băng schema: file 03 PASS, sau đó chỉ sửa lỗi |
| 4 | Lưu trú, hóa đơn xong | — |
| 5 | Dọn phòng, báo cáo xong; tích hợp, kiểm thử | — |
| 6 | Sửa lỗi, nộp bài | Đóng băng code 3 ngày trước hạn |

Mỗi cuối tuần: gộp code, chạy lại DB, chạy kịch bản xuyên suốt (mục 7).

---

## 1. Khung ứng dụng (Forms/Main)

### frmLogin – tất cả
- Giao diện: tên đăng nhập, mật khẩu (`UseSystemPasswordChar`), nút Đăng nhập/Thoát, Enter để đăng nhập.
- Xử lý: `Session.TryLogin(user, pass, out error)`; sai thì `UiHelper.Warn(error)`; đúng thì `DialogResult = OK`.
- Xong khi: `admin/Admin@123` vào được; nhân viên mẫu `/123456` vào được; `tunghv` bị chặn vì đã khóa; sai mật khẩu báo chung chung.

### frmMain – tất cả
- Giao diện: MenuStrip (Hệ thống, Danh mục, Đặt phòng, Lễ tân, Hóa đơn, Dọn phòng, Báo cáo); StatusStrip (tên, vai trò, giờ); thân form là dashboard (`sp_DashboardSummary`) và sơ đồ phòng (`vw_RoomBoard`) tô màu bằng `RoomStatus.ToColor`; nút Làm mới.
- Xử lý: Load → `FormManager.Init(this)`, `DbHelper.Exec("dbo.sp_ExpirePendingBookings")`, ẩn/hiện menu theo vai trò (`UiHelper.VisibleFor`); Housekeeping chỉ thấy Dọn phòng và Đổi mật khẩu. `FormManager.Subscribe(this, area => ...)` để sơ đồ phòng tự nạp lại. Đăng xuất: `FormManager.CloseAll(); Session.Logout(); LoggedOut = true; Close();`.
- Xong khi: 4 vai trò thấy 4 bộ menu khác nhau; mọi mục menu mở được form; sơ đồ phòng hiện đúng màu.

### dlgChangePassword – tất cả
- Xử lý: `Session.ChangePassword(old, new, confirm, out error)`.

---

## 2. Danh mục (Forms/Catalog)

Bố cục chung: ô tìm, lưới trái, khung chi tiết phải, nút Thêm/Sửa/Lưu/Hủy/Ngừng dùng. Ghi bằng `DbHelper.ExecText` có tham số. Bảng có `IsActive` thì "ngừng dùng" thay cho xóa. Làm `frmAmenities` trước để làm mẫu cho các form còn lại.

### frmAmenities (A, M)
- Thêm/sửa tên, mô tả. Xóa chỉ khi chưa gán cho loại phòng nào; lỗi 547 được ErrorHandler báo sẵn.

### frmRoomTypes (A, M)
- Trường: TypeName, BasePrice > 0, MaxOccupancy 1–10, BedType, Description, IsActive.
- Tab Tiện nghi: CheckedListBox; Lưu = `DbHelper.InTransaction` (DELETE RoomTypeAmenities của loại, rồi INSERT lại).
- Ghi chú: đổi giá không ảnh hưởng booking cũ (giá đã chốt vào `BookingRooms.PricePerNight`).

### frmRooms (A, M; R chỉ xem)
- Lọc theo tầng/loại/trạng thái (`Lookups.LoadFloors`, `LoadRoomTypes`, `LoadRoomStatuses`).
- Thêm/sửa RoomNumber (duy nhất), Floor, RoomTypeId (chỉ loại đang dùng), Note, IsActive. **Không sửa trực tiếp `Status`.**
- Nút Bảo trì → `sp_SetRoomMaintenance` (bắt buộc ghi chú). Nút Kết thúc bảo trì → `sp_FinishMaintenance` (phòng sang Cleaning, sinh việc dọn).
- Trước khi ngừng khai thác: kiểm tra phòng không còn lượt Booked/CheckedIn.

### frmServices (A, M; R chỉ xem)
- Trường: ServiceName (duy nhất), Category, Unit, Price ≥ 0, IsActive. Dòng `IsSystem = 1` chỉ xem.

### frmCustomers (A, M, R; xóa chỉ A, M)
- Tìm theo tên/SĐT/CCCD, tìm không dấu bằng `COLLATE Vietnamese_CI_AI`.
- Bắt buộc: FullName, IdNumber (duy nhất), Phone. Gender lấy từ `Lookups.LoadGenders`. Ngày sinh không ở tương lai. Nationality mặc định "Việt Nam".
- Tab Lịch sử: các phiếu của khách từ `vw_BookingSummary`.
- `pickMode`: hiện nút Chọn, nhấp đúp để chọn, thêm khách mới xong thì chọn luôn; `SelectedCustomerId`; `DialogResult = OK`.

### frmEmployees (Admin)
- Trường: FullName, Username (duy nhất), Role (`Lookups.LoadRoles`), Phone, Email, HireDate. Thêm mới thì nhập mật khẩu và băm bằng `PasswordHasher.Hash`.
- Nút Khóa/Mở khóa (`IsActive`) và Đặt lại mật khẩu (về `PasswordHasher.DefaultResetPassword`). Không xóa cứng.
- Chặn tự khóa chính mình và chặn khóa Admin cuối cùng.
- Tab Vai trò: chỉ sửa Description.

Xong khi: thêm/sửa/tìm/ngừng được; nhập trùng hoặc xóa bản ghi đang dùng thì có thông báo tiếng Việt, không văng; đăng nhập bằng lễ tân thì nút sửa bị ẩn đúng quyền.

---

## 3. Đặt phòng và thu tiền (Forms/Booking, Forms/Billing)

Phiếu chỉ sang Confirmed khi đã thu đủ cọc; phòng chỉ check-in được khi phiếu đã Confirmed.

### frmBookings (A, M, R)
- Lưới phiếu từ `vw_BookingSummary`, lọc theo trạng thái, khoảng ngày nhận, tên/SĐT. Lưới dưới là các phòng của phiếu (`vw_RoomStays WHERE BookingId = @Id`).
- Nút: Tạo, Sửa (`frmBookingEdit`), Thu cọc (`dlgPayment(PaymentMode.Deposit, bookingId)`), Hủy phòng / Hủy phiếu / No-show (`dlgCancel`).
- Phiếu Pending tô màu và hiện hạn còn lại (24 giờ tính từ CreatedAt).

### frmBookingEdit (A, M, R)
- Phần phiếu: khách (nút Chọn → `frmCustomers(true)`), tỷ lệ cọc (`Lookups.LoadDepositRates`), ghi chú.
- Tìm phòng: chọn ngày nhận/trả, loại phòng → `SELECT * FROM dbo.fn_AvailableRooms(@In, @Out, NULL)`; thêm vào giỏ kèm số người lớn, trẻ em, người ở.
- Tạo: `DbHelper.CreateBookingRoomInput()` + `AddBookingRoom(...)` → `sp_CreateBooking` với `Tvp("@Rooms", "dbo.BookingRoomInput", rooms)`, lấy `@BookingId` qua `Out`. Xong thì hỏi có thu cọc ngay không.
- Sửa (chỉ phòng còn Booked): `sp_UpdateBookingRoom`. Tìm phòng thay thế thì truyền `@ExcludeBookingRoomId`. "Giữ giá cũ" bắt buộc có lý do.
- Tạm tính trên form: số đêm × giá, tổng, cọc = ROUND(tổng × tỷ lệ). Số chính thức lấy từ DB.

### dlgCancel (A, M, R)
- Hiện số đã cọc và kết quả dự kiến: hủy trước giờ nhận ≥ 48 giờ thì được hoàn, ngược lại mất cọc.
- Lý do bắt buộc; được hoàn thì chọn phương thức và nhập mã giao dịch. Ô No-show chỉ bật sau 12:00 ngày hôm sau ngày nhận.
- `sp_CancelBookingRoom` (1 phòng) hoặc `sp_CancelBooking` (cả phiếu); báo số tiền hoàn lấy từ OUTPUT.

### dlgPayment (A, M, R; Refund chỉ A, M)
- Nhập chung: số tiền (`UiHelper.SetupMoneyInput`), phương thức (`Lookups.LoadPaymentMethods`), mã giao dịch (bắt buộc khi `PayMethod.NeedsReference`), ghi chú.

| Mode | Hiển thị | Gọi |
| --- | --- | --- |
| Deposit (id = BookingId) | Cọc yêu cầu, đã cọc, còn thiếu (`vw_BookingSummary`); gợi ý số còn thiếu | `sp_RecordBookingDeposit` |
| StayPayment (id = BookingRoomId) | Sổ tiền của phòng (`vw_RoomStayFolio`) | `sp_RecordPayment` @Type = 'Payment' |
| Refund (id = BookingRoomId) | Số đã thu của phòng | `sp_RecordPayment` @Type = 'Refund', ghi chú bắt buộc |

Xong khi: tạo phiếu 2 phòng → thu cọc 50% → phiếu Confirmed; đặt trùng phòng trùng ngày bị chặn (50005); hủy trước và sau mốc 48 giờ ra số hoàn khác nhau; chuyển khoản thiếu mã giao dịch bị chặn.

---

## 4. Lưu trú (Forms/Stay)

Mọi thao tác theo `BookingRoomId`, bắt đầu từ `frmFrontDesk`. Có thể làm ngay bằng dữ liệu mẫu (đã có phòng đến hôm nay, đang ở, trả hôm nay, đoàn 3 phòng).

### frmFrontDesk (A, M, R)

| Tab | Nguồn | Nút bật ở tab này |
| --- | --- | --- |
| Khách đến hôm nay | `vw_TodayArrivals` | Check-in, Đổi phòng (frmBookingEdit), Thu cọc, Hủy/No-show |
| Đang ở | `vw_InHouseStays` | Dịch vụ, Thu tiền, Gia hạn, Check-out |
| Dự kiến trả hôm nay | `vw_TodayDepartures` | Check-out, Gia hạn; tô đỏ phòng đã quá 12:00 |

- Ô tìm theo số phòng, tên khách, mã phiếu cho tab đang mở; nút Làm mới.
- Mọi hộp thoại trả OK thì nạp lại cả 3 tab, vì một phòng có thể chuyển từ tab này sang tab khác. Đăng ký thêm `FormManager.Subscribe`.

### dlgCheckIn
- Hiện: phiếu, khách đặt, phòng, ngày nhận–trả, cọc yêu cầu (`fn_RequiredDeposit`) và đã thu (`fn_StayNetPaid`).
- Nhập: tên + CCCD/hộ chiếu người ở thực tế (điền sẵn từ booking nếu có; có CCCD thì phải có tên).
- `sp_CheckInRoom`. Xử lý riêng các lỗi:
  - 50006 hoặc 50003 "chưa đủ cọc" → hỏi mở `dlgPayment(Deposit, bookingId)`;
  - 50007 chưa đến ngày nhận → nhắc sửa booking;
  - 50003 phòng chưa sẵn sàng → gợi ý đổi phòng.
- Thành công: `NotifyDataChanged(Stays)` và `NotifyDataChanged(Rooms)`.

### frmServiceUsage
- Đầu form: phòng, khách, thời gian ở. Lưới: `ServiceUsages` join `Services` theo `BookingRoomId`, dòng tổng ở cuối.
- Thêm: `Lookups.LoadServices(cbo)` (không có dịch vụ hệ thống), số lượng, thời điểm dùng (mặc định bây giờ), ghi chú. Số lượng > `BusinessRules.ServiceQuantityWarning` thì hỏi xác nhận → `sp_AddServiceUsage`.
- Sửa/xóa: `sp_UpdateServiceUsage`, `sp_DeleteServiceUsage`. Dòng của dịch vụ `IsSystem` (phụ thu trễ) không cho sửa.
- Phòng đã có hóa đơn Paid thì form chỉ đọc. Sau khi void, chỉ A/M được sửa (DB tự chặn).

### dlgExtendStay
- Hiện ngày trả hiện tại; chọn ngày trả mới (phải sau ngày cũ), lý do; tạm tính số đêm thêm × giá đêm đã chốt.
- `sp_ExtendStay`; lỗi 50005 thì báo chọn ngày khác hoặc đổi phòng.

### frmCheckOut
1. Load → `DbHelper.Query("dbo.sp_PreviewCheckOut", P("@BookingRoomId", id))`. Giữ `CheckOutAt` của dòng kết quả. Hiện bảng tính: số đêm, tiền phòng, dịch vụ, phụ thu trễ (số phút trễ, `Formatters.Duration`), giảm giá, VAT, tổng, đã thu, còn phải thu hoặc phải hoàn. (Tên cột: xem `fn_CheckOutQuote` trong file 01.)
2. Nhập giảm giá + lý do → nút Tính lại (gọi lại preview với `@Discount`).
3. Người mua (tùy chọn): tên, đơn vị, MST, địa chỉ; có MST thì phải có tên đơn vị.
4. Tiền: còn phải thu → số thu (mặc định đúng số còn thiếu), phương thức, mã giao dịch; trả dư → phương thức hoàn, mã.
5. Xác nhận → `sp_CheckOutRoom` với đúng `CheckOutAt` ở bước 1, `@InvoiceId` OUTPUT. Lỗi 50007 "quá 10 phút" → tự xem trước lại. Thành công → `FormManager.ShowDialog(new frmInvoicePrint(invoiceId))`, rồi `NotifyDataChanged` cho Stays, Rooms, Invoices, Payments, Housekeeping.
6. Sau 18:00 ngày trả, DB chặn (50007) → hiện nút mở `dlgExtendStay`.
- Nên tách phần nhập ở bước 2–4 thành UserControl `ucSettlement` để `frmInvoices` dùng lại khi lập lại hóa đơn.

**Ca kiểm thử bắt buộc:**

| Ca | Kỳ vọng |
| --- | --- |
| Trả trước 12:30 | Không phụ thu |
| Trả 12:45 (trễ 45 phút) | Phụ thu 5% giá đêm |
| Trả 14:30 (trễ 150 phút) | Phụ thu 15% + 15% = 30% giá đêm |
| Trả sau 18:00 | Bị chặn, phải gia hạn 1 đêm |
| Lễ tân giảm 15% | Bị chặn (50001); Manager thì được |
| Thu thiếu | Bị chặn, báo số còn thiếu |
| Đã thu nhiều hơn tổng (cọc 100% rồi được giảm giá) | Phải chọn cách hoàn, sinh khoản Refund |
| Đoàn 3 phòng trả 2 phòng | 2 hóa đơn riêng; phiếu chưa CheckedOut; phòng còn lại vẫn ghi dịch vụ được |
| Sau check-out | Phòng sang Cleaning, có việc dọn mới trong hàng đợi |

---

## 5. Hóa đơn và sổ thu (Forms/Billing)

### frmInvoicePrint (A, M, R) – Tuấn
- Dữ liệu: `vw_InvoiceDetail` (đầu hóa đơn) và `vw_InvoiceLines` (các dòng) theo `InvoiceId`.
- Mẫu in: `HotelInfo` (tên, địa chỉ, MST khách sạn), số `HDxxxxxx`, ngày, người mua/đơn vị/MST, phòng, ngày nhận–trả; bảng dòng (tiền phòng = số đêm × giá, từng dịch vụ, phụ thu trễ); tạm tính, giảm giá, VAT, tổng, `Formatters.MoneyToWords(tổng)`, đã cọc/đã thu/đã hoàn; chữ "ĐÃ HỦY" nếu Void.
- Vẽ bằng `PrintDocument` + `PrintPreviewDialog`; nút In, Đóng.

### frmInvoices (A, M, R; void/lập lại chỉ A, M) – Tuấn
- Lọc khoảng ngày, trạng thái (`Lookups.LoadInvoiceStatuses`), tìm theo số hóa đơn/số phòng/tên khách. Lưới trên từ `vw_InvoiceDetail`, lưới dưới từ `vw_InvoiceLines`.
- Xem/In → `frmInvoicePrint`.
- Void: lý do bắt buộc, `UiHelper.Confirm(..., defaultNo: true)` → `sp_VoidInvoice`.
- Lập lại: chỉ bật khi phòng đã trả và hóa đơn mới nhất là Void → `ucSettlement` → `sp_ReissueInvoice` → in hóa đơn mới.
  Đã làm thành hộp thoại riêng `dlgReissueInvoice(int bookingRoomId, int voidInvoiceId)` trong `Forms/Billing`
  (bảng tính tạm + `ucSettlement`), kèm nút "Sửa dịch vụ phòng" để sửa dịch vụ sau khi void.
- Cột "Thay cho" hiện số hóa đơn cũ (`ReplacesInvoiceId`).

### frmPaymentLedger (A, M) – C
- Lọc khoảng ngày, loại, phương thức, nhân viên; lưới từ `vw_PaymentLedger`.
- Dòng tổng: tổng thu, tổng hoàn, thực thu; tổng theo từng phương thức.
- Nút "Ghi hoàn thủ công" → `dlgPayment(PaymentMode.Refund, bookingRoomId)`.

Xong khi: check-out xong in được hóa đơn đúng số; void → sửa dịch vụ → lập lại ra hóa đơn mới, hóa đơn cũ vẫn còn ở trạng thái Void; lễ tân bấm Void bị chặn.

---

## 6. Dọn phòng và báo cáo (Forms/Operations)

### frmHousekeeping (H, A, M; R chỉ xem)
- Lưới từ `vw_HousekeepingQueue`, lọc Pending/InProgress/Done hôm nay; Housekeeping mặc định lọc "việc của tôi" và việc chưa ai nhận.
- Nhận việc → `sp_StartHousekeepingTask`. Hoàn tất → chọn Sạch / Cần bảo trì (ghi chú bắt buộc) → `sp_CompleteHousekeepingTask`.
- `Timer` tự làm mới mỗi 60 giây.

### frmReports (A, M)

| Tab | Nguồn | Hiển thị |
| --- | --- | --- |
| Doanh thu | `DbHelper.QueryMulti("dbo.sp_RevenueReport", ...)` | Bảng 0: thực thu theo kỳ (cọc, thu, hoàn, theo phương thức); bảng 1: doanh số theo hóa đơn. Biểu đồ cột thực thu (Chart có sẵn) |
| Dịch vụ | `vw_ServiceUsageStats` lọc theo ngày | Top dịch vụ theo doanh thu/số lượt |
| Phòng | `vw_RoomStatusSummary` | Số phòng theo trạng thái |

- Nút Xuất CSV (`UiHelper.ExportCsv`). Ghi chú trên form: thực thu tính theo ngày nhận tiền (gồm cả cọc), doanh số tính theo ngày lập hóa đơn.

---

## 7. Kịch bản xuyên suốt (chạy trên DB vừa làm mới)

1. Lễ tân đăng nhập, thêm khách mới.
2. Đặt 2 phòng nhận hôm nay, cọc 50% → thu cọc → phiếu Confirmed.
3. Check-in cả 2 phòng; ghi dịch vụ riêng từng phòng; gia hạn phòng 2 thêm 1 đêm.
4. Check-out phòng 1, thu đủ, in hóa đơn. Kiểm tra phòng 2 vẫn ghi dịch vụ được và phiếu chưa CheckedOut.
5. Housekeeping đăng nhập, nhận và hoàn tất việc dọn phòng 1.
6. Manager void hóa đơn phòng 1, sửa dịch vụ, lập lại hóa đơn.
7. Manager xem báo cáo và sổ thu hôm nay, đối chiếu với các khoản vừa thu.

## 8. Rủi ro đã biết

- Script mới kiểm tra cú pháp và tham chiếu, chưa từng chạy đủ trên SQL Server thật → chạy file 03, FAIL thì sửa trước.
- Phụ thu trễ phụ thuộc giờ thật của máy (`SYSDATETIME()`) → demo sau 12:30 hoặc chỉnh đồng hồ máy; công thức đã có test riêng trong file 03.
- Thiếu thời gian thì gộp còn 16 form: dlgCheckIn, dlgExtendStay, dlgCancel thành ô nhập trên form cha; frmAmenities thành tab của frmRoomTypes; frmPaymentLedger thành tab của frmReports.
