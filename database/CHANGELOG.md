# CHANGELOG – database

Nhật ký thay đổi các script trong `database/`. Người giữ DB: **Lê Minh Tuấn**.

## Quy ước

- Mỗi lần sửa `01`, `02` hoặc `03` thì thêm một mục **lên đầu** danh sách bên dưới (mới nhất ở trên).
- Mỗi mục ghi: ngày, file đã sửa, sửa gì, và **có phải chạy lại DB không**.
- Giữ nguyên tên bảng, cột, procedure và tham số cũ. Tham số mới phải có giá trị mặc định, để code của người khác không bị hỏng.
- Có ghi "Cần chạy lại DB: **Có**" thì báo cả nhóm chạy `database\reset_db.bat <server>`.
  Lệnh này xóa và tạo lại DB, dữ liệu tự nhập trên máy sẽ mất.
- Sau tuần 3 (đóng băng schema) chỉ sửa lỗi, không thêm bảng hoặc cột mới.

Mẫu một mục:

```
## YYYY-MM-DD – tóm tắt ngắn
- File: 01 (và 02/03 nếu có)
- Thay đổi: ...
- Ảnh hưởng tới form: ... (form nào, của ai)
- Cần chạy lại DB: Có / Không
```

---

## 2026-10-15 – Thêm `reset_db.bat` và CHANGELOG

- File: `reset_db.bat` (mới), `CHANGELOG.md` (mới). Không sửa `01`, `02`, `03`.
- Thay đổi: một lệnh để chạy 01 → 03 → 02. Dừng khi có lỗi SQL hoặc có ca kiểm thử FAIL.
  Chạy `sqlcmd` có `-I` (bật QUOTED_IDENTIFIER). Thiếu `-I` thì file 02 lỗi 1934 do file 01 có filtered index.
- Ảnh hưởng tới form: không.
- Cần chạy lại DB: Không.

## 2026-10-14 – Bản đầu tiên (mốc chuẩn)

- File: `01_HotelManagement_TaoDB.sql`, `02_HotelManagement_DuLieuMau.sql`, `03_HotelManagement_KiemThu.sql`.
- Nội dung: 14 bảng, kiểu bảng `dbo.BookingRoomInput`, 6 function, 14 view, 27 procedure, dữ liệu khởi tạo (vai trò, tài khoản `admin`, dịch vụ hệ thống "Phụ thu trả phòng trễ").
  Danh sách đầy đủ xem mục 6 của `CLAUDE.md`.
- Đã chạy thử trên SQL Server 2022: file 03 PASS hết.
- Cần chạy lại DB: **Có**. Mọi thành viên chạy lần đầu bằng `database\reset_db.bat <server>`.
