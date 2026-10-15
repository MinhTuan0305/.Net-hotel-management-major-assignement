/* =====================================================================
   HOTEL MANAGEMENT - TỰ KIỂM THỬ NGHIỆP VỤ (chạy SAU file 01, có hoặc không có file 02)
   ---------------------------------------------------------------------
   - Tự tạo dữ liệu test riêng (loại phòng "TEST Type", phòng T901-T904, nhân viên test_*,
     khách TEST0001), chạy các kịch bản, in bảng PASS/FAIL, rồi XÓA toàn bộ dữ liệu test.
   - Mọi dòng phải PASS. Dòng FAIL: gửi lại cột Detail để sửa.
   ===================================================================== */
USE HotelManagement;
GO
SET NOCOUNT ON;
SET XACT_ABORT OFF;

DECLARE @R TABLE (Seq INT IDENTITY(1,1), TestName NVARCHAR(200), Passed BIT, Detail NVARCHAR(4000));
DECLARE @Today DATE = CAST(GETDATE() AS DATE);
DECLARE @Hash VARCHAR(255) = '$2a$11$daKlmKa19waEfjVD44kjOOLeRXk.TGWT0jyumdd/kem9R5NxEY8dW';
DECLARE @Rec INT, @Mgr INT, @Hk INT, @Cust INT, @Type INT, @RoomA INT, @RoomB INT, @RoomC INT, @RoomD INT;
DECLARE @B1 INT, @B2 INT, @B3 INT, @B4 INT, @B5 INT, @StayA INT, @StayB INT, @StayC INT, @StayD INT, @StayE INT;
DECLARE @Rooms dbo.BookingRoomInput;
DECLARE @Inv INT, @Inv2 INT, @Usage INT, @Task INT, @Refund DECIMAL(14,0), @Amt DECIMAL(14,0);
DECLARE @CheckOutAt DATETIME2(0), @Total DECIMAL(14,0), @NetPaid DECIMAL(14,0), @Balance DECIMAL(14,0);
DECLARE @Status VARCHAR(20), @Ignore INT;
DECLARE @D2 DATE = DATEADD(DAY, 2, CAST(GETDATE() AS DATE)), @D3 DATE = DATEADD(DAY, 3, CAST(GETDATE() AS DATE));

/* ---------- Dọn dữ liệu test cũ (nếu lần trước dừng giữa chừng) ---------- */
DELETE p FROM dbo.Payments p JOIN dbo.BookingRooms br ON br.BookingRoomId = p.BookingRoomId JOIN dbo.Rooms r ON r.RoomId = br.RoomId WHERE r.RoomNumber LIKE 'T90_';
UPDATE i SET ReplacesInvoiceId = NULL FROM dbo.Invoices i JOIN dbo.BookingRooms br ON br.BookingRoomId = i.BookingRoomId JOIN dbo.Rooms r ON r.RoomId = br.RoomId WHERE r.RoomNumber LIKE 'T90_';
DELETE i FROM dbo.Invoices i JOIN dbo.BookingRooms br ON br.BookingRoomId = i.BookingRoomId JOIN dbo.Rooms r ON r.RoomId = br.RoomId WHERE r.RoomNumber LIKE 'T90_';
DELETE su FROM dbo.ServiceUsages su JOIN dbo.BookingRooms br ON br.BookingRoomId = su.BookingRoomId JOIN dbo.Rooms r ON r.RoomId = br.RoomId WHERE r.RoomNumber LIKE 'T90_';
DELETE t FROM dbo.HousekeepingTasks t JOIN dbo.Rooms r ON r.RoomId = t.RoomId WHERE r.RoomNumber LIKE 'T90_';
DELETE br FROM dbo.BookingRooms br JOIN dbo.Bookings b ON b.BookingId = br.BookingId JOIN dbo.Customers c ON c.CustomerId = b.CustomerId WHERE c.IdNumber = 'TEST0001';
DELETE b FROM dbo.Bookings b JOIN dbo.Customers c ON c.CustomerId = b.CustomerId WHERE c.IdNumber = 'TEST0001';
DELETE FROM dbo.Rooms WHERE RoomNumber LIKE 'T90_';
DELETE FROM dbo.RoomTypes WHERE TypeName = N'TEST Type';
DELETE FROM dbo.Customers WHERE IdNumber = 'TEST0001';
DELETE FROM dbo.Employees WHERE Username LIKE 'test[_]%';

/* ---------- Dữ liệu test ---------- */
INSERT INTO dbo.Employees (RoleId, FullName, Username, PasswordHash)
SELECT RoleId, N'Test lễ tân', 'test_rec', @Hash FROM dbo.Roles WHERE RoleName = N'Receptionist';
SET @Rec = SCOPE_IDENTITY();
INSERT INTO dbo.Employees (RoleId, FullName, Username, PasswordHash)
SELECT RoleId, N'Test quản lý', 'test_mgr', @Hash FROM dbo.Roles WHERE RoleName = N'Manager';
SET @Mgr = SCOPE_IDENTITY();
INSERT INTO dbo.Employees (RoleId, FullName, Username, PasswordHash)
SELECT RoleId, N'Test buồng phòng', 'test_hk', @Hash FROM dbo.Roles WHERE RoleName = N'Housekeeping';
SET @Hk = SCOPE_IDENTITY();
INSERT INTO dbo.Customers (FullName, IdNumber, Phone) VALUES (N'Khách kiểm thử', 'TEST0001', '0999999999');
SET @Cust = SCOPE_IDENTITY();
INSERT INTO dbo.RoomTypes (TypeName, BasePrice, MaxOccupancy) VALUES (N'TEST Type', 1000000, 2);
SET @Type = SCOPE_IDENTITY();
INSERT INTO dbo.Rooms (RoomTypeId, RoomNumber, Floor) VALUES (@Type, 'T901', 9); SET @RoomA = SCOPE_IDENTITY();
INSERT INTO dbo.Rooms (RoomTypeId, RoomNumber, Floor) VALUES (@Type, 'T902', 9); SET @RoomB = SCOPE_IDENTITY();
INSERT INTO dbo.Rooms (RoomTypeId, RoomNumber, Floor) VALUES (@Type, 'T903', 9); SET @RoomC = SCOPE_IDENTITY();
INSERT INTO dbo.Rooms (RoomTypeId, RoomNumber, Floor) VALUES (@Type, 'T904', 9); SET @RoomD = SCOPE_IDENTITY();
IF NOT EXISTS (SELECT 1 FROM dbo.Services WHERE ServiceName = N'TEST Nước suối')
    INSERT INTO dbo.Services (ServiceName, Unit, Price) VALUES (N'TEST Nước suối', N'Chai', 20000);
DECLARE @Svc INT = (SELECT ServiceId FROM dbo.Services WHERE ServiceName = N'TEST Nước suối');

/* ---------- 1. Phụ thu trả trễ (giá 1.000.000, mốc 12:00 ngày 10/01/2026) ---------- */
INSERT INTO @R SELECT N'Phụ thu 12:20 = 0',       CASE WHEN dbo.fn_LateCheckoutFee(1000000, '2026-01-10', '2026-01-10 12:20') = 0      THEN 1 ELSE 0 END, NULL;
INSERT INTO @R SELECT N'Phụ thu 12:45 = 50.000',  CASE WHEN dbo.fn_LateCheckoutFee(1000000, '2026-01-10', '2026-01-10 12:45') = 50000  THEN 1 ELSE 0 END, NULL;
INSERT INTO @R SELECT N'Phụ thu 13:10 = 100.000', CASE WHEN dbo.fn_LateCheckoutFee(1000000, '2026-01-10', '2026-01-10 13:10') = 100000 THEN 1 ELSE 0 END, NULL;
INSERT INTO @R SELECT N'Phụ thu 13:50 = 150.000', CASE WHEN dbo.fn_LateCheckoutFee(1000000, '2026-01-10', '2026-01-10 13:50') = 150000 THEN 1 ELSE 0 END, NULL;
INSERT INTO @R SELECT N'Phụ thu 14:30 = 300.000', CASE WHEN dbo.fn_LateCheckoutFee(1000000, '2026-01-10', '2026-01-10 14:30') = 300000 THEN 1 ELSE 0 END, NULL;
INSERT INTO @R SELECT N'Phụ thu 15:30 = 450.000', CASE WHEN dbo.fn_LateCheckoutFee(1000000, '2026-01-10', '2026-01-10 15:30') = 450000 THEN 1 ELSE 0 END, NULL;
INSERT INTO @R SELECT N'Phụ thu 17:59 = 750.000', CASE WHEN dbo.fn_LateCheckoutFee(1000000, '2026-01-10', '2026-01-10 17:59') = 750000 THEN 1 ELSE 0 END, NULL;
INSERT INTO @R SELECT N'Sau 18:00 phải gia hạn (NULL)', CASE WHEN dbo.fn_LateCheckoutFee(1000000, '2026-01-10', '2026-01-10 18:01') IS NULL THEN 1 ELSE 0 END, NULL;
INSERT INTO @R SELECT N'Trả sớm không phụ thu',   CASE WHEN dbo.fn_LateCheckoutFee(1000000, '2026-01-10', '2026-01-09 20:00') = 0      THEN 1 ELSE 0 END, NULL;

/* ---------- 2. Đặt phòng ---------- */
-- Booking 1: phòng A hôm nay -> +2, phòng B +1 -> +3, cọc 50%
DELETE FROM @Rooms;
INSERT INTO @Rooms (RoomId, CheckInDate, CheckOutDate, Adults, Children) VALUES
    (@RoomA, @Today, DATEADD(DAY, 2, @Today), 2, 0),
    (@RoomB, DATEADD(DAY, 1, @Today), DATEADD(DAY, 3, @Today), 1, 0);
BEGIN TRY
    EXEC dbo.sp_CreateBooking @CustomerId = @Cust, @DepositRate = 50, @EmployeeId = @Rec, @Rooms = @Rooms, @BookingId = @B1 OUTPUT;
    SELECT @Status = Status FROM dbo.Bookings WHERE BookingId = @B1;
    SELECT @StayA = BookingRoomId FROM dbo.BookingRooms WHERE BookingId = @B1 AND RoomId = @RoomA;
    SELECT @StayB = BookingRoomId FROM dbo.BookingRooms WHERE BookingId = @B1 AND RoomId = @RoomB;
    INSERT INTO @R SELECT N'Tạo booking 2 phòng -> Pending', CASE WHEN @Status = 'Pending' AND @StayA IS NOT NULL AND @StayB IS NOT NULL THEN 1 ELSE 0 END, @Status;
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Tạo booking 2 phòng -> Pending', 0, ERROR_MESSAGE();
END CATCH;

-- Trùng lịch
DELETE FROM @Rooms;
INSERT INTO @Rooms (RoomId, CheckInDate, CheckOutDate, Adults, Children) VALUES (@RoomA, DATEADD(DAY, 1, @Today), DATEADD(DAY, 4, @Today), 1, 0);
BEGIN TRY
    EXEC dbo.sp_CreateBooking @CustomerId = @Cust, @DepositRate = 50, @EmployeeId = @Rec, @Rooms = @Rooms, @BookingId = @Ignore OUTPUT;
    INSERT INTO @R SELECT N'Chặn đặt trùng lịch', 0, N'Không bị chặn';
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Chặn đặt trùng lịch', CASE WHEN ERROR_NUMBER() = 50005 THEN 1 ELSE 0 END, ERROR_MESSAGE();
END CATCH;

-- Booking nối tiếp đúng ngày trả của booking trước (không trùng) - tạo rồi hủy ngay
DELETE FROM @Rooms;
INSERT INTO @Rooms (RoomId, CheckInDate, CheckOutDate, Adults, Children) VALUES (@RoomA, DATEADD(DAY, 2, @Today), DATEADD(DAY, 3, @Today), 1, 0);
BEGIN TRY
    EXEC dbo.sp_CreateBooking @CustomerId = @Cust, @DepositRate = 50, @EmployeeId = @Rec, @Rooms = @Rooms, @BookingId = @B5 OUTPUT;
    EXEC dbo.sp_CancelBooking @BookingId = @B5, @Reason = N'Test nối tiếp', @EmployeeId = @Rec;
    INSERT INTO @R SELECT N'Cho đặt nối tiếp ngày trả', 1, NULL;
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Cho đặt nối tiếp ngày trả', 0, ERROR_MESSAGE();
END CATCH;

-- Vượt sức chứa (loại phòng chứa 2)
DELETE FROM @Rooms;
INSERT INTO @Rooms (RoomId, CheckInDate, CheckOutDate, Adults, Children) VALUES (@RoomC, DATEADD(DAY, 5, @Today), DATEADD(DAY, 6, @Today), 2, 1);
BEGIN TRY
    EXEC dbo.sp_CreateBooking @CustomerId = @Cust, @DepositRate = 50, @EmployeeId = @Rec, @Rooms = @Rooms, @BookingId = @Ignore OUTPUT;
    INSERT INTO @R SELECT N'Chặn vượt sức chứa từng phòng', 0, N'Không bị chặn';
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Chặn vượt sức chứa từng phòng', CASE WHEN ERROR_NUMBER() = 50004 THEN 1 ELSE 0 END, ERROR_MESSAGE();
END CATCH;

-- Ngày quá khứ
DELETE FROM @Rooms;
INSERT INTO @Rooms (RoomId, CheckInDate, CheckOutDate, Adults, Children) VALUES (@RoomC, DATEADD(DAY, -1, @Today), @Today, 1, 0);
BEGIN TRY
    EXEC dbo.sp_CreateBooking @CustomerId = @Cust, @DepositRate = 50, @EmployeeId = @Rec, @Rooms = @Rooms, @BookingId = @Ignore OUTPUT;
    INSERT INTO @R SELECT N'Chặn đặt ngày quá khứ', 0, N'Không bị chặn';
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Chặn đặt ngày quá khứ', CASE WHEN ERROR_NUMBER() = 50007 THEN 1 ELSE 0 END, ERROR_MESSAGE();
END CATCH;

/* ---------- 3. Cọc ---------- */
-- Cọc yêu cầu: A = 1.000.000 x 2 x 50% = 1.000.000; B = 1.000.000 x 2 x 50% = 1.000.000
BEGIN TRY
    EXEC dbo.sp_RecordBookingDeposit @BookingId = @B1, @Amount = 1500000, @Method = 'Cash', @EmployeeId = @Rec;
    SELECT @Status = Status FROM dbo.Bookings WHERE BookingId = @B1;
    INSERT INTO @R SELECT N'Cọc thiếu -> vẫn Pending, phòng A đủ, phòng B 500.000',
        CASE WHEN @Status = 'Pending' AND dbo.fn_StayNetPaid(@StayA) = 1000000 AND dbo.fn_StayNetPaid(@StayB) = 500000 THEN 1 ELSE 0 END,
        CONCAT(@Status, ' A=', dbo.fn_StayNetPaid(@StayA), ' B=', dbo.fn_StayNetPaid(@StayB));

    EXEC dbo.sp_RecordBookingDeposit @BookingId = @B1, @Amount = 500000, @Method = 'BankTransfer', @ReferenceNo = 'TESTFT01', @EmployeeId = @Rec;
    SELECT @Status = Status FROM dbo.Bookings WHERE BookingId = @B1;
    INSERT INTO @R SELECT N'Đủ cọc -> Confirmed', CASE WHEN @Status = 'Confirmed' THEN 1 ELSE 0 END, @Status;
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Thu cọc', 0, ERROR_MESSAGE();
END CATCH;

BEGIN TRY
    EXEC dbo.sp_RecordBookingDeposit @BookingId = @B1, @Amount = 100000, @Method = 'BankTransfer', @EmployeeId = @Rec;
    INSERT INTO @R SELECT N'Chuyển khoản thiếu mã giao dịch bị chặn', 0, N'Không bị chặn';
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Chuyển khoản thiếu mã giao dịch bị chặn', CASE WHEN ERROR_NUMBER() = 50004 THEN 1 ELSE 0 END, ERROR_MESSAGE();
END CATCH;

/* ---------- 4. Check-in theo phòng ---------- */
BEGIN TRY
    EXEC dbo.sp_CheckInRoom @BookingRoomId = @StayB, @EmployeeId = @Rec;
    INSERT INTO @R SELECT N'Chặn check-in trước ngày nhận', 0, N'Không bị chặn';
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Chặn check-in trước ngày nhận', CASE WHEN ERROR_NUMBER() = 50007 THEN 1 ELSE 0 END, ERROR_MESSAGE();
END CATCH;

BEGIN TRY
    EXEC dbo.sp_CheckInRoom @BookingRoomId = @StayA, @GuestName = N'Người ở A', @GuestIdNumber = 'TESTGUEST', @EmployeeId = @Rec;
    SELECT @Status = Status FROM dbo.Bookings WHERE BookingId = @B1;
    INSERT INTO @R SELECT N'Check-in phòng A: lượt CheckedIn, phòng Occupied, phiếu CheckedIn',
        CASE WHEN @Status = 'CheckedIn'
              AND (SELECT Status FROM dbo.BookingRooms WHERE BookingRoomId = @StayA) = 'CheckedIn'
              AND (SELECT Status FROM dbo.Rooms WHERE RoomId = @RoomA) = 'Occupied'
              AND (SELECT Status FROM dbo.BookingRooms WHERE BookingRoomId = @StayB) = 'Booked'
             THEN 1 ELSE 0 END, @Status;
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Check-in phòng A', 0, ERROR_MESSAGE();
END CATCH;

/* ---------- 5. Dịch vụ theo phòng ---------- */
BEGIN TRY
    EXEC dbo.sp_AddServiceUsage @BookingRoomId = @StayA, @ServiceId = @Svc, @Quantity = 2, @EmployeeId = @Rec, @UsageId = @Usage OUTPUT;
    INSERT INTO @R SELECT N'Ghi dịch vụ phòng đang ở, giá chốt theo danh mục',
        CASE WHEN (SELECT Amount FROM dbo.ServiceUsages WHERE UsageId = @Usage) = 40000 THEN 1 ELSE 0 END, NULL;
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Ghi dịch vụ phòng đang ở', 0, ERROR_MESSAGE();
END CATCH;

BEGIN TRY
    EXEC dbo.sp_AddServiceUsage @BookingRoomId = @StayB, @ServiceId = @Svc, @Quantity = 1, @EmployeeId = @Rec, @UsageId = @Ignore OUTPUT;
    INSERT INTO @R SELECT N'Chặn dịch vụ cho phòng chưa nhận', 0, N'Không bị chặn';
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Chặn dịch vụ cho phòng chưa nhận', CASE WHEN ERROR_NUMBER() = 50003 THEN 1 ELSE 0 END, ERROR_MESSAGE();
END CATCH;

BEGIN TRY
    EXEC dbo.sp_AddServiceUsage @BookingRoomId = @StayA, @ServiceId = @Svc, @Quantity = 0, @EmployeeId = @Rec, @UsageId = @Ignore OUTPUT;
    INSERT INTO @R SELECT N'Chặn số lượng 0', 0, N'Không bị chặn';
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Chặn số lượng 0', CASE WHEN ERROR_NUMBER() = 50004 THEN 1 ELSE 0 END, ERROR_MESSAGE();
END CATCH;

/* ---------- 6. Check-out phòng A ---------- */
-- Script chạy rất nhanh: lùi giờ nhận phòng 1 tiếng để giờ trả luôn sau giờ nhận
UPDATE dbo.BookingRooms SET ActualCheckIn = DATEADD(HOUR, -1, ActualCheckIn) WHERE BookingRoomId = @StayA;
UPDATE dbo.ServiceUsages SET UsedAt = DATEADD(MINUTE, -30, UsedAt) WHERE BookingRoomId = @StayA;
SET @CheckOutAt = CAST(SYSDATETIME() AS DATETIME2(0));

-- Thiếu tiền: không cho check-out, dữ liệu giữ nguyên
BEGIN TRY
    EXEC dbo.sp_CheckOutRoom @BookingRoomId = @StayA, @CheckOutAt = @CheckOutAt, @PayAmount = 0, @EmployeeId = @Rec, @InvoiceId = @Ignore OUTPUT;
    INSERT INTO @R SELECT N'Chặn check-out khi chưa thu đủ', 0, N'Không bị chặn';
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Chặn check-out khi chưa thu đủ (rollback)',
        CASE WHEN ERROR_NUMBER() = 50006
              AND (SELECT Status FROM dbo.BookingRooms WHERE BookingRoomId = @StayA) = 'CheckedIn'
              AND NOT EXISTS (SELECT 1 FROM dbo.Invoices WHERE BookingRoomId = @StayA)
             THEN 1 ELSE 0 END, ERROR_MESSAGE();
END CATCH;

-- Receptionist giảm giá trên 10%
BEGIN TRY
    EXEC dbo.sp_CheckOutRoom @BookingRoomId = @StayA, @CheckOutAt = @CheckOutAt, @Discount = 500000, @DiscountReason = N'Test',
         @PayAmount = 0, @EmployeeId = @Rec, @InvoiceId = @Ignore OUTPUT;
    INSERT INTO @R SELECT N'Chặn lễ tân giảm giá trên 10%', 0, N'Không bị chặn';
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Chặn lễ tân giảm giá trên 10%', CASE WHEN ERROR_NUMBER() = 50001 THEN 1 ELSE 0 END, ERROR_MESSAGE();
END CATCH;

-- Mã số thuế thiếu tên đơn vị
BEGIN TRY
    EXEC dbo.sp_CheckOutRoom @BookingRoomId = @StayA, @CheckOutAt = @CheckOutAt, @BuyerTaxCode = '0312345678',
         @PayAmount = 0, @EmployeeId = @Rec, @InvoiceId = @Ignore OUTPUT;
    INSERT INTO @R SELECT N'Chặn hóa đơn có MST thiếu tên đơn vị', 0, N'Không bị chặn';
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Chặn hóa đơn có MST thiếu tên đơn vị', CASE WHEN ERROR_NUMBER() = 50004 THEN 1 ELSE 0 END, ERROR_MESSAGE();
END CATCH;

-- Check-out đúng: tiền phòng 2 đêm (trả sớm vẫn tính đủ) + dịch vụ 40.000, VAT 10%
-- Tổng = ROUND((2.000.000 + 40.000) x 1.1) = 2.244.000; đã cọc 1.000.000 -> thu thêm 1.244.000
BEGIN TRY
    SELECT @Total = TotalAmount, @Balance = Balance FROM dbo.fn_CheckOutQuote(@StayA, @CheckOutAt, 0, 10);
    EXEC dbo.sp_CheckOutRoom @BookingRoomId = @StayA, @CheckOutAt = @CheckOutAt, @PayAmount = @Balance, @PayMethod = 'Cash',
         @EmployeeId = @Rec, @InvoiceId = @Inv OUTPUT;
    SELECT @Status = Status FROM dbo.Bookings WHERE BookingId = @B1;
    INSERT INTO @R SELECT N'Check-out A: hóa đơn 2.244.000 Paid, thu đủ, phòng Cleaning, có việc dọn, phiếu vẫn CheckedIn',
        CASE WHEN @Total = 2244000 AND @Balance = 1244000
              AND (SELECT TotalAmount FROM dbo.Invoices WHERE InvoiceId = @Inv) = 2244000
              AND (SELECT Status FROM dbo.Invoices WHERE InvoiceId = @Inv) = 'Paid'
              AND dbo.fn_StayNetPaid(@StayA) = 2244000
              AND NOT EXISTS (SELECT 1 FROM dbo.Payments WHERE BookingRoomId = @StayA AND ISNULL(InvoiceId, 0) <> @Inv)
              AND (SELECT Status FROM dbo.Rooms WHERE RoomId = @RoomA) = 'Cleaning'
              AND EXISTS (SELECT 1 FROM dbo.HousekeepingTasks WHERE RoomId = @RoomA AND Status = 'Pending')
              AND @Status = 'CheckedIn'
             THEN 1 ELSE 0 END,
        CONCAT('Total=', @Total, ' Balance=', @Balance, ' Booking=', @Status);
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Check-out phòng A', 0, ERROR_MESSAGE();
END CATCH;

/* ---------- 7. Hóa đơn: khóa dịch vụ, void, lập lại ---------- */
BEGIN TRY
    EXEC dbo.sp_AddServiceUsage @BookingRoomId = @StayA, @ServiceId = @Svc, @Quantity = 1, @EmployeeId = @Mgr, @UsageId = @Ignore OUTPUT;
    INSERT INTO @R SELECT N'Khóa dịch vụ khi đã có hóa đơn', 0, N'Không bị chặn';
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Khóa dịch vụ khi đã có hóa đơn', CASE WHEN ERROR_NUMBER() = 50003 THEN 1 ELSE 0 END, ERROR_MESSAGE();
END CATCH;

BEGIN TRY
    EXEC dbo.sp_VoidInvoice @InvoiceId = @Inv, @Reason = N'Test', @EmployeeId = @Rec;
    INSERT INTO @R SELECT N'Lễ tân không được void', 0, N'Không bị chặn';
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Lễ tân không được void', CASE WHEN ERROR_NUMBER() = 50001 THEN 1 ELSE 0 END, ERROR_MESSAGE();
END CATCH;

-- Manager void, thêm dịch vụ bỏ sót, lập lại với giảm giá 5% -> thu thêm hoặc hoàn chênh lệch
BEGIN TRY
    EXEC dbo.sp_VoidInvoice @InvoiceId = @Inv, @Reason = N'Bỏ sót dịch vụ', @EmployeeId = @Mgr;
    EXEC dbo.sp_AddServiceUsage @BookingRoomId = @StayA, @ServiceId = @Svc, @Quantity = 5, @EmployeeId = @Mgr, @UsageId = @Ignore OUTPUT;
    -- tiền phòng 2.000.000 + dịch vụ 140.000 = 2.140.000; giảm 5% = 107.000; tổng = ROUND(2.033.000 x 1.1) = 2.236.300
    SET @Total = 2236300;
    SET @Balance = @Total - dbo.fn_StayNetPaid(@StayA);
    SET @Amt = CASE WHEN @Balance > 0 THEN @Balance ELSE 0 END;
    EXEC dbo.sp_ReissueInvoice @BookingRoomId = @StayA, @Discount = 107000, @DiscountReason = N'Xin lỗi vì sai sót',
         @PayAmount = @Amt, @PayMethod = 'Cash', @RefundMethod = 'Cash',
         @EmployeeId = @Mgr, @InvoiceId = @Inv2 OUTPUT;
    INSERT INTO @R SELECT N'Void + lập lại: tổng 2.236.300, hoàn 7.700, hóa đơn cũ Void, mới Paid và tham chiếu hóa đơn cũ',
        CASE WHEN (SELECT TotalAmount FROM dbo.Invoices WHERE InvoiceId = @Inv2) = @Total
              AND (SELECT Status FROM dbo.Invoices WHERE InvoiceId = @Inv) = 'Void'
              AND (SELECT ReplacesInvoiceId FROM dbo.Invoices WHERE InvoiceId = @Inv2) = @Inv
              AND dbo.fn_StayNetPaid(@StayA) = @Total
              AND EXISTS (SELECT 1 FROM dbo.Payments WHERE BookingRoomId = @StayA AND Type = 'Refund' AND Amount = 7700)
             THEN 1 ELSE 0 END,
        CONCAT('Balance=', @Balance, ' NetPaid=', dbo.fn_StayNetPaid(@StayA));
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Void + lập lại hóa đơn', 0, ERROR_MESSAGE();
END CATCH;

/* ---------- 8. Dọn phòng ---------- */
BEGIN TRY
    SELECT @Task = TaskId FROM dbo.HousekeepingTasks WHERE RoomId = @RoomA AND Status <> 'Done';
    EXEC dbo.sp_StartHousekeepingTask @TaskId = @Task, @EmployeeId = @Hk;
    EXEC dbo.sp_CompleteHousekeepingTask @TaskId = @Task, @Result = 'Clean', @EmployeeId = @Hk;
    INSERT INTO @R SELECT N'Dọn xong -> phòng Available',
        CASE WHEN (SELECT Status FROM dbo.Rooms WHERE RoomId = @RoomA) = 'Available' THEN 1 ELSE 0 END, NULL;
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Dọn phòng', 0, ERROR_MESSAGE();
END CATCH;

/* ---------- 9. Hủy phòng B dưới 48 giờ: không hoàn cọc ---------- */
BEGIN TRY
    SET @Refund = NULL;
    EXEC dbo.sp_CancelBookingRoom @BookingRoomId = @StayB, @Reason = N'Khách đổi kế hoạch', @RefundMethod = 'Cash',
         @EmployeeId = @Rec, @RefundAmount = @Refund OUTPUT;
    SELECT @Status = Status FROM dbo.Bookings WHERE BookingId = @B1;
    INSERT INTO @R SELECT N'Hủy dưới 48 giờ: không hoàn, phiếu thành CheckedOut',
        CASE WHEN @Refund = 0 AND dbo.fn_StayNetPaid(@StayB) = 1000000 AND @Status = 'CheckedOut' THEN 1 ELSE 0 END,
        CONCAT('Refund=', @Refund, ' Booking=', @Status);
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Hủy phòng B', 0, ERROR_MESSAGE();
END CATCH;

/* ---------- 10. Hủy trước 48 giờ: hoàn 100% ---------- */
DELETE FROM @Rooms;
INSERT INTO @Rooms (RoomId, CheckInDate, CheckOutDate, Adults, Children) VALUES (@RoomC, DATEADD(DAY, 6, @Today), DATEADD(DAY, 7, @Today), 1, 0);
BEGIN TRY
    EXEC dbo.sp_CreateBooking @CustomerId = @Cust, @DepositRate = 100, @EmployeeId = @Rec, @Rooms = @Rooms, @BookingId = @B2 OUTPUT;
    EXEC dbo.sp_RecordBookingDeposit @BookingId = @B2, @Amount = 1000000, @Method = 'Cash', @EmployeeId = @Rec;
    SELECT @StayC = BookingRoomId FROM dbo.BookingRooms WHERE BookingId = @B2;
    SET @Refund = NULL;
    EXEC dbo.sp_CancelBooking @BookingId = @B2, @Reason = N'Khách hủy sớm', @RefundMethod = 'Cash', @EmployeeId = @Rec, @TotalRefund = @Refund OUTPUT;
    INSERT INTO @R SELECT N'Hủy trước 48 giờ: hoàn 1.000.000, phiếu Cancelled',
        CASE WHEN @Refund = 1000000 AND dbo.fn_StayNetPaid(@StayC) = 0
              AND (SELECT Status FROM dbo.Bookings WHERE BookingId = @B2) = 'Cancelled' THEN 1 ELSE 0 END,
        CONCAT('Refund=', @Refund);
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Hủy trước 48 giờ', 0, ERROR_MESSAGE();
END CATCH;

/* ---------- 11. Gia hạn ---------- */
-- Phòng D: lượt E hôm nay -> +1 (đang ở); lượt khác +2 -> +3 cùng phòng
DELETE FROM @Rooms;
INSERT INTO @Rooms (RoomId, CheckInDate, CheckOutDate, Adults, Children) VALUES (@RoomD, @Today, DATEADD(DAY, 1, @Today), 1, 0);
BEGIN TRY
    EXEC dbo.sp_CreateBooking @CustomerId = @Cust, @DepositRate = 50, @EmployeeId = @Rec, @Rooms = @Rooms, @BookingId = @B3 OUTPUT;
    EXEC dbo.sp_RecordBookingDeposit @BookingId = @B3, @Amount = 500000, @Method = 'Cash', @EmployeeId = @Rec;
    SELECT @StayE = BookingRoomId FROM dbo.BookingRooms WHERE BookingId = @B3;
    EXEC dbo.sp_CheckInRoom @BookingRoomId = @StayE, @EmployeeId = @Rec;

    DELETE FROM @Rooms;
    INSERT INTO @Rooms (RoomId, CheckInDate, CheckOutDate, Adults, Children) VALUES (@RoomD, DATEADD(DAY, 2, @Today), DATEADD(DAY, 3, @Today), 1, 0);
    EXEC dbo.sp_CreateBooking @CustomerId = @Cust, @DepositRate = 50, @EmployeeId = @Rec, @Rooms = @Rooms, @BookingId = @B4 OUTPUT;

    EXEC dbo.sp_ExtendStay @BookingRoomId = @StayE, @NewCheckOutDate = @Today, @EmployeeId = @Rec;
    INSERT INTO @R SELECT N'Chặn gia hạn về ngày sớm hơn', 0, N'Không bị chặn';
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Chặn gia hạn về ngày sớm hơn', CASE WHEN ERROR_NUMBER() = 50007 THEN 1 ELSE 0 END, ERROR_MESSAGE();
END CATCH;

BEGIN TRY
    EXEC dbo.sp_ExtendStay @BookingRoomId = @StayE, @NewCheckOutDate = @D2, @Reason = N'Ở thêm 1 đêm', @EmployeeId = @Rec;
    INSERT INTO @R SELECT N'Gia hạn 1 đêm (không trùng) -> 2 đêm',
        CASE WHEN (SELECT Nights FROM dbo.BookingRooms WHERE BookingRoomId = @StayE) = 2 THEN 1 ELSE 0 END, NULL;
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Gia hạn 1 đêm', 0, ERROR_MESSAGE();
END CATCH;

BEGIN TRY
    EXEC dbo.sp_ExtendStay @BookingRoomId = @StayE, @NewCheckOutDate = @D3, @EmployeeId = @Rec;
    INSERT INTO @R SELECT N'Chặn gia hạn trùng lịch khách khác', 0, N'Không bị chặn';
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Chặn gia hạn trùng lịch khách khác', CASE WHEN ERROR_NUMBER() = 50005 THEN 1 ELSE 0 END, ERROR_MESSAGE();
END CATCH;

/* ---------- 12. Bảo trì ---------- */
BEGIN TRY
    EXEC dbo.sp_SetRoomMaintenance @RoomId = @RoomB, @Note = N'Test', @EmployeeId = @Rec;
    INSERT INTO @R SELECT N'Lễ tân không được đưa phòng vào bảo trì', 0, N'Không bị chặn';
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Lễ tân không được đưa phòng vào bảo trì', CASE WHEN ERROR_NUMBER() = 50001 THEN 1 ELSE 0 END, ERROR_MESSAGE();
END CATCH;

BEGIN TRY
    EXEC dbo.sp_SetRoomMaintenance @RoomId = @RoomB, @Note = N'Hỏng điều hòa', @EmployeeId = @Mgr;
    EXEC dbo.sp_FinishMaintenance @RoomId = @RoomB, @EmployeeId = @Mgr;
    SELECT @Task = TaskId FROM dbo.HousekeepingTasks WHERE RoomId = @RoomB AND Status <> 'Done';
    EXEC dbo.sp_CompleteHousekeepingTask @TaskId = @Task, @Result = 'Clean', @EmployeeId = @Hk;
    INSERT INTO @R SELECT N'Bảo trì -> Cleaning (có việc dọn) -> Available',
        CASE WHEN (SELECT Status FROM dbo.Rooms WHERE RoomId = @RoomB) = 'Available'
              AND (SELECT Source FROM dbo.HousekeepingTasks WHERE TaskId = @Task) = 'AfterMaintenance' THEN 1 ELSE 0 END, NULL;
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Luồng bảo trì', 0, ERROR_MESSAGE();
END CATCH;

/* ---------- 13. Hết hạn Pending ---------- */
BEGIN TRY
    INSERT INTO dbo.Bookings (CustomerId, DepositRate, CreatedBy, CreatedAt) VALUES (@Cust, 50, @Rec, DATEADD(HOUR, -30, SYSDATETIME()));
    SET @B5 = SCOPE_IDENTITY();
    INSERT INTO dbo.BookingRooms (BookingId, RoomId, CheckInDate, CheckOutDate, Adults, PricePerNight)
    VALUES (@B5, @RoomC, DATEADD(DAY, 8, @Today), DATEADD(DAY, 9, @Today), 1, 1000000);
    EXEC dbo.sp_ExpirePendingBookings;
    INSERT INTO @R SELECT N'Pending quá 24 giờ bị hủy',
        CASE WHEN (SELECT Status FROM dbo.Bookings WHERE BookingId = @B5) = 'Cancelled'
              AND (SELECT Status FROM dbo.BookingRooms WHERE BookingId = @B5) = 'Cancelled' THEN 1 ELSE 0 END, NULL;
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Hết hạn Pending', 0, ERROR_MESSAGE();
END CATCH;

/* ---------- 14. Báo cáo chạy được ---------- */
BEGIN TRY
    DECLARE @Dash TABLE (ReportDate DATE, TotalRooms INT, AvailableRooms INT, OccupiedRooms INT, CleaningRooms INT, MaintenanceRooms INT,
                         ArrivalsToday INT, InHouseRooms INT, DeparturesDue INT, CheckedInToday INT, CheckedOutToday INT,
                         NewBookingsToday INT, OccupancyRate DECIMAL(5,1), RevenueToday DECIMAL(14,0));
    INSERT INTO @Dash EXEC dbo.sp_DashboardSummary @EmployeeId = @Mgr;
    INSERT INTO @R SELECT N'Dashboard chạy và có doanh thu hôm nay',
        CASE WHEN EXISTS (SELECT 1 FROM @Dash WHERE RevenueToday IS NOT NULL) THEN 1 ELSE 0 END, NULL;
    DELETE FROM @Dash;
    INSERT INTO @Dash EXEC dbo.sp_DashboardSummary @EmployeeId = @Rec;
    INSERT INTO @R SELECT N'Lễ tân xem dashboard không thấy doanh thu',
        CASE WHEN EXISTS (SELECT 1 FROM @Dash WHERE RevenueToday IS NULL) THEN 1 ELSE 0 END, NULL;
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Dashboard', 0, ERROR_MESSAGE();
END CATCH;

BEGIN TRY
    EXEC dbo.sp_RevenueReport @FromDate = @Today, @ToDate = @Today, @EmployeeId = @Rec;
    INSERT INTO @R SELECT N'Lễ tân không xem báo cáo doanh thu', 0, N'Không bị chặn';
END TRY
BEGIN CATCH
    INSERT INTO @R SELECT N'Lễ tân không xem báo cáo doanh thu', CASE WHEN ERROR_NUMBER() = 50001 THEN 1 ELSE 0 END, ERROR_MESSAGE();
END CATCH;

/* ---------- Kết quả ---------- */
SELECT Seq, CASE WHEN Passed = 1 THEN 'PASS' ELSE 'FAIL' END AS Result, TestName, Detail FROM @R ORDER BY Seq;
SELECT COUNT(*) AS Total, SUM(CASE WHEN Passed = 1 THEN 1 ELSE 0 END) AS Passed,
       SUM(CASE WHEN Passed = 0 THEN 1 ELSE 0 END) AS Failed
FROM @R;

/* ---------- Dọn dữ liệu test ---------- */
DELETE p FROM dbo.Payments p JOIN dbo.BookingRooms br ON br.BookingRoomId = p.BookingRoomId JOIN dbo.Rooms r ON r.RoomId = br.RoomId WHERE r.RoomNumber LIKE 'T90_';
UPDATE i SET ReplacesInvoiceId = NULL FROM dbo.Invoices i JOIN dbo.BookingRooms br ON br.BookingRoomId = i.BookingRoomId JOIN dbo.Rooms r ON r.RoomId = br.RoomId WHERE r.RoomNumber LIKE 'T90_';
DELETE i FROM dbo.Invoices i JOIN dbo.BookingRooms br ON br.BookingRoomId = i.BookingRoomId JOIN dbo.Rooms r ON r.RoomId = br.RoomId WHERE r.RoomNumber LIKE 'T90_';
DELETE su FROM dbo.ServiceUsages su JOIN dbo.BookingRooms br ON br.BookingRoomId = su.BookingRoomId JOIN dbo.Rooms r ON r.RoomId = br.RoomId WHERE r.RoomNumber LIKE 'T90_';
DELETE t FROM dbo.HousekeepingTasks t JOIN dbo.Rooms r ON r.RoomId = t.RoomId WHERE r.RoomNumber LIKE 'T90_';
DELETE br FROM dbo.BookingRooms br JOIN dbo.Bookings b ON b.BookingId = br.BookingId JOIN dbo.Customers c ON c.CustomerId = b.CustomerId WHERE c.IdNumber = 'TEST0001';
DELETE b FROM dbo.Bookings b JOIN dbo.Customers c ON c.CustomerId = b.CustomerId WHERE c.IdNumber = 'TEST0001';
DELETE FROM dbo.Rooms WHERE RoomNumber LIKE 'T90_';
DELETE FROM dbo.RoomTypes WHERE TypeName = N'TEST Type';
DELETE FROM dbo.Customers WHERE IdNumber = 'TEST0001';
DELETE FROM dbo.Services WHERE ServiceName = N'TEST Nước suối';
DELETE FROM dbo.Employees WHERE Username LIKE 'test[_]%';
PRINT N'Đã dọn dữ liệu test.';
GO
