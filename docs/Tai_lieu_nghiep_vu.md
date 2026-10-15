**TÀI LIỆU NGHIỆP VỤ  
HỆ THỐNG QUẢN LÝ KHÁCH SẠN**

*Phiên bản thống nhất nghiệp vụ – làm cơ sở thiết kế database và lập trình*

| **Thông tin**     | **Nội dung**                                                                                   |
|-------------------|------------------------------------------------------------------------------------------------|
| Công nghệ dự kiến | C# .NET WinForms + SQL Server                                                                  |
| Phạm vi           | 15 nhóm nghiệp vụ từ đăng nhập đến tìm kiếm, kiểm tra lỗi                                      |
| Nguyên tắc        | Chốt quy tắc nghiệp vụ trước; database sẽ được thiết kế/điều chỉnh sau để đáp ứng tài liệu này |
| Đối tượng sử dụng | Admin, Manager, Receptionist, Housekeeping                                                     |

# HƯỚNG DẪN ĐỌC TÀI LIỆU

- Tài liệu này thống nhất cách hệ thống phải hoạt động, không khẳng định rằng database hiện tại đã có đủ cột/bảng để triển khai.

- Tên bảng/field đang được ghi theo schema hiện tại khi phù hợp. Nếu nghiệp vụ cần thông tin chưa có, phần thiết kế database sẽ bổ sung hoặc điều chỉnh sau.

- Mọi thao tác ghi dữ liệu liên quan đến nhiều bảng phải được xử lý nguyên tử bằng transaction: hoặc lưu đầy đủ, hoặc rollback toàn bộ.

- Các quyền dưới đây là đề xuất thống nhất cho phạm vi đồ án; có thể điều chỉnh theo yêu cầu giảng viên nhưng phải sửa đồng bộ toàn tài liệu.

- Không xóa cứng dữ liệu đã tham gia giao dịch. Ưu tiên khóa/ngừng sử dụng hoặc từ chối xóa để bảo toàn lịch sử.

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>GHI CHÚ BẢN RÀ SOÁT (13/10/2026)</strong></p>
<p><strong>Cách đọc</strong></p>
<ul>
<li><p>Nội dung gốc của nhóm được giữ nguyên. Mỗi mục cần sửa có thêm phần <strong>"7. Đề xuất sửa và điều chỉnh database"</strong> ở cuối mục. Khi nhóm duyệt, nội dung trong khung đề xuất <strong>thay thế</strong> phần tương ứng phía trên.</p></li>
<li><p>Các tiểu mục được đánh lại số cho liền mạch (bản cũ thiếu mục 4): 4. Dữ liệu nghiệp vụ, 5. Checklist, 6. Quy tắc thống nhất, 7. Đề xuất sửa. Các bước quy trình được đánh số lại từ 1 trong mỗi mục.</p></li>
</ul>
<p><strong>Thay đổi lớn nhất</strong></p>
<ul>
<li><p><strong>Tính tiền và cung cấp dịch vụ theo từng phòng.</strong> Đơn vị nghiệp vụ là <strong>lượt lưu trú phòng (BookingRoom)</strong>: mỗi phòng tự check-in, ghi dịch vụ, gia hạn, check-out, có hóa đơn và các khoản thu riêng. Booking chỉ còn là phiếu đặt và giữ chỗ của người đặt.</p></li>
<li><p><strong>Tiền gắn với lượt lưu trú, không gắn với hóa đơn.</strong> Cọc, thanh toán, hoàn tiền đều ghi vào Payments theo phòng. Hóa đơn là chứng từ chốt khi trả phòng. Cách này xóa vòng lặp "phải có hóa đơn mới thu tiền, phải thu tiền mới cho trả phòng".</p></li>
<li><p><strong>Thu đủ khi trả phòng</strong>, phần dư hoàn ngay. Hóa đơn chỉ còn hai trạng thái Paid/Void.</p></li>
<li><p>Đưa lại các quyết định đã chốt nhưng bị rơi, áp dụng chính sách phụ thu trả phòng trễ mới (mục 11), bổ sung mục <strong>7A. Quản lý danh mục dịch vụ</strong> còn thiếu.</p></li>
<li><p>Tổng hợp xung đột ở <strong>mục VI</strong>; đề xuất database làm lại toàn bộ (14 bảng, view, function, stored procedure) ở <strong>mục VII</strong>.</p></li>
</ul></td>
</tr>
</tbody>
</table>

# I. QUY ƯỚC NGHIỆP VỤ DÙNG CHUNG

## 1. Vai trò và quyền

| **Vai trò**  | **Quyền chính**                                                                                          |
|--------------|----------------------------------------------------------------------------------------------------------|
| Admin        | Toàn quyền cấu hình hệ thống, vai trò, nhân viên; được xem và xử lý dữ liệu nghiệp vụ.                   |
| Manager      | Quản lý danh mục và nghiệp vụ vận hành; xem dashboard/báo cáo; không quản lý vai trò và tài khoản Admin. |
| Receptionist | Quản lý khách hàng, đặt phòng, check-in/check-out, dịch vụ, hóa đơn và thanh toán.                       |
| Housekeeping | Xem danh sách phòng cần dọn, cập nhật kết quả dọn phòng; không sửa booking, hóa đơn hay thanh toán.      |

### Đề xuất sửa – I.1 Vai trò và quyền

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>I.1 ghi Manager "không quản lý vai trò và tài khoản Admin", ngầm hiểu Manager được quản lý nhân viên khác; mục 3 lại chỉ cho Admin quản lý nhân viên.</p></li>
<li><p>Nhiều mục chỉ ghi "người có quyền" (xác nhận booking, nhận phòng sớm, sửa danh mục, void hóa đơn, điều chỉnh dịch vụ), không đủ để code và kiểm thử.</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>Chỉ Admin quản lý tài khoản nhân viên. Sửa dòng Manager thành: "Quản lý danh mục (loại phòng, phòng, tiện nghi, dịch vụ), đưa phòng vào/ra bảo trì, duyệt giảm giá trên 10%, void và lập lại hóa đơn, xem báo cáo doanh thu; không quản lý vai trò và tài khoản nhân viên."</p></li>
<li><p>Ma trận quyền dưới đây là nguồn duy nhất; các mục khác chỉ tham chiếu, không định nghĩa lại quyền.</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>Không thêm bảng Permissions. Quyền kiểm tra ở tầng nghiệp vụ C# và kiểm tra lại trong stored procedure qua tham số @EmployeeId (đọc RoleName).</p></li>
</ul></td>
</tr>
</tbody>
</table>

**Ma trận quyền** (A = Admin, M = Manager, R = Receptionist, H = Housekeeping; "xem" = chỉ xem):

| **Chức năng**                                                                     | **A** | **M** | **R** | **H**              |
|-----------------------------------------------------------------------------------|-------|-------|-------|--------------------|
| Đăng nhập, đổi mật khẩu của mình                                                  | ✓     | ✓     | ✓     | ✓                  |
| Vai trò: xem, sửa mô tả                                                           | ✓     |       |       |                    |
| Nhân viên: tạo, sửa, khóa/mở, reset mật khẩu                                      | ✓     |       |       |                    |
| Khách hàng: thêm, sửa                                                             | ✓     | ✓     | ✓     |                    |
| Khách hàng: xóa (khi chưa có lịch sử)                                             | ✓     | ✓     |       |                    |
| Danh mục loại phòng, phòng, tiện nghi, dịch vụ                                    | ✓     | ✓     | xem   |                    |
| Đưa phòng vào bảo trì, kết thúc bảo trì                                           | ✓     | ✓     |       |                    |
| Đặt, sửa, hủy phòng; đánh dấu no-show                                             | ✓     | ✓     | ✓     |                    |
| Thu cọc, thu tiền; hoàn tiền theo chính sách (hủy đủ điều kiện, dư khi trả phòng) | ✓     | ✓     | ✓     |                    |
| Hoàn tiền/điều chỉnh ngoài chính sách (bút toán ngược)                            | ✓     | ✓     |       |                    |
| Check-in, đổi phòng trước check-in, gia hạn, check-out                            | ✓     | ✓     | ✓     |                    |
| Ghi, sửa, xóa dịch vụ khi phòng đang ở                                            | ✓     | ✓     | ✓     |                    |
| Giảm giá đến 10% tiền hóa đơn trước giảm                                          | ✓     | ✓     | ✓     |                    |
| Giảm giá trên 10%                                                                 | ✓     | ✓     |       |                    |
| Void và lập lại hóa đơn; sửa dịch vụ sau khi void                                 | ✓     | ✓     |       |                    |
| Nhận và hoàn tất việc dọn phòng                                                   | ✓     | ✓     | xem   | ✓                  |
| Dashboard vận hành (phòng, khách đến/đi)                                          | ✓     | ✓     | ✓     | hàng đợi dọn phòng |
| Báo cáo doanh thu                                                                 | ✓     | ✓     |       |                    |

## 2. Trạng thái thống nhất

| **Đối tượng** | **Trạng thái** | **Quy tắc**                                                                                                                 |
|---------------|----------------|-----------------------------------------------------------------------------------------------------------------------------|
| Booking       | Pending        | Phiếu mới tạo, chờ xác nhận; chưa được check-in.                                                                            |
| Booking       | Confirmed      | Đã xác nhận, có thể check-in khi đủ điều kiện.                                                                              |
| Booking       | CheckedIn      | Khách đã nhận phòng; phòng đang lưu trú.                                                                                    |
| Booking       | CheckedOut     | Tất cả phòng trong booking đã check-out; hóa đơn cuối cùng đã được lập.                                                     |
| Booking       | Cancelled      | Đã hủy; không được check-in và không chiếm lịch phòng.                                                                      |
| Room          | Available      | Phòng sẵn sàng để được đặt/nhận khách nếu không xung đột lịch.                                                              |
| Room          | Reserved       | Phòng có booking đã xác nhận trong khoảng thời gian đặt; đây là trạng thái vận hành hiện tại, không thay thế kiểm tra lịch. |
| Room          | Occupied       | Đang có khách lưu trú.                                                                                                      |
| Room          | Cleaning       | Khách đã trả phòng; chờ Housekeeping hoàn tất.                                                                              |
| Room          | Maintenance    | Không thể đưa vào danh sách phòng khả dụng.                                                                                 |
| Invoice       | Unpaid         | Chưa ghi nhận khoản thanh toán nào cho hóa đơn.                                                                             |
| Invoice       | PartiallyPaid  | Tổng tiền đã thanh toán nhỏ hơn tổng phải trả nhưng lớn hơn 0.                                                              |
| Invoice       | Paid           | Tổng tiền đã thanh toán bằng tổng phải trả.                                                                                 |
| Invoice       | Void           | Hóa đơn bị vô hiệu hóa theo quyền và quy trình kiểm soát.                                                                   |

### Đề xuất sửa – I.2 Trạng thái thống nhất

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>Room.Reserved không có bước nào bật hoặc tắt. Nếu bật khi xác nhận booking của tháng sau, phòng bị khóa vô lý từ hôm nay; trạng thái hiện tại không thể biểu diễn lịch tương lai.</p></li>
<li><p>Trạng thái Booking chỉ đúng khi mọi phòng đi cùng nhau; khi tính theo từng phòng, mỗi phòng cần trạng thái riêng.</p></li>
<li><p>Invoice Unpaid/PartiallyPaid mâu thuẫn với quy định thu đủ khi trả phòng (mục 11).</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>Dùng bảng trạng thái dưới đây thay cho bảng ở trên.</p></li>
<li><p>Trạng thái Booking do hệ thống tự tính lại sau mỗi thao tác ở cấp phòng (thủ tục sp_RefreshBookingStatus), không cho sửa tay.</p></li>
<li><p>"Phòng có khách sắp đến" lấy từ lịch BookingRooms, không lưu vào Room.Status.</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>CHECK theo đúng danh sách giá trị bên dưới; thêm cột BookingRooms.Status; Rooms.Status bỏ "Reserved"; Invoices.Status chỉ còn Paid/Void; thêm Payments.Type và HousekeepingTasks.Status/Result.</p></li>
</ul></td>
</tr>
</tbody>
</table>

| **Đối tượng**              | **Trạng thái**              | **Quy tắc**                                                                          |
|----------------------------|-----------------------------|--------------------------------------------------------------------------------------|
| Booking (phiếu đặt)        | Pending                     | Mới tạo, chưa đủ cọc. Tự hủy sau 24 giờ nếu chưa từng được xác nhận.                 |
| Booking                    | Confirmed                   | Mọi phòng còn hiệu lực đã đủ cọc yêu cầu.                                            |
| Booking                    | CheckedIn                   | Có ít nhất một phòng đang ở, hoặc có phòng đã trả trong khi phòng khác chưa đến.     |
| Booking                    | CheckedOut                  | Mọi phòng còn hiệu lực đã trả phòng.                                                 |
| Booking                    | Cancelled                   | Mọi phòng đã hủy.                                                                    |
| BookingRoom (lượt lưu trú) | Booked                      | Đã giữ phòng theo ngày, chưa nhận.                                                   |
| BookingRoom                | CheckedIn                   | Khách đang ở; được ghi dịch vụ, thu tiền, gia hạn.                                   |
| BookingRoom                | CheckedOut                  | Đã trả phòng, đã có hóa đơn Paid.                                                    |
| BookingRoom                | Cancelled                   | Đã hủy; CancelReason ghi lý do (khách hủy, hết hạn giữ chỗ, NoShow). Không giữ lịch. |
| Room                       | Available                   | Sẵn sàng nhận khách ngay.                                                            |
| Room                       | Occupied                    | Đang có lượt lưu trú CheckedIn.                                                      |
| Room                       | Cleaning                    | Đã trả phòng hoặc vừa bảo trì xong, chờ dọn.                                         |
| Room                       | Maintenance                 | Đang bảo trì; không đặt mới, không nhận khách.                                       |
| Invoice                    | Paid                        | Hóa đơn hiệu lực của lượt lưu trú, đã thu đủ.                                        |
| Invoice                    | Void                        | Đã hủy để lập lại; giữ lịch sử, có lý do và người hủy.                               |
| Payment.Type               | Deposit / Payment / Refund  | Cọc trước check-in / thu khi ở hoặc khi trả phòng / hoàn tiền.                       |
| HousekeepingTask           | Pending / InProgress / Done | Kết quả khi Done: Clean hoặc NeedMaintenance.                                        |

## 3. Quy tắc ngày lưu trú và giá

- Khoảng lưu trú được hiểu là \[ngày nhận, ngày trả): ngày nhận được tính là ngày bắt đầu, ngày trả là mốc kết thúc; khách trả phòng trong ngày dự kiến không bị tính thêm đêm.

- Số đêm dự kiến = số ngày giữa CheckOutDate và CheckInDate. Ngày trả phải lớn hơn ngày nhận.

- Hai booking bị trùng phòng khi khoảng thời gian giao nhau: ngày nhận booking hiện có \< ngày trả booking mới VÀ ngày trả booking hiện có \> ngày nhận booking mới. Loại trừ booking Cancelled.

- Giá phòng được chốt tại thời điểm đặt và lưu riêng cho từng phòng trong booking; thay đổi giá loại phòng sau này không làm thay đổi booking đã tạo.

- Tiền dịch vụ sử dụng đơn giá tại thời điểm phát sinh; sửa giá trong danh mục không làm đổi các lượt sử dụng đã ghi nhận.

- Các khoản tiền được làm tròn đến đơn vị tiền tệ nhỏ nhất mà ứng dụng thống nhất (đề xuất VND, làm tròn đến đồng).

### Đề xuất sửa – I.3 Quy tắc ngày lưu trú và giá

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>Chưa có giờ nhận/trả chuẩn nên không xác định được nhận sớm và trả trễ.</p></li>
<li><p>Ngày lưu trú gắn ở cấp booking nên không gia hạn hay trả riêng từng phòng.</p></li>
<li><p>Quy tắc trùng lịch chưa loại phòng đã trả sớm, khiến phòng trống vẫn bị coi là bận.</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>Giờ nhận phòng chuẩn <strong>14:00</strong>, giờ trả phòng chuẩn <strong>12:00</strong> (hằng số dùng chung, xem VII.10).</p></li>
<li><p>Ngày nhận, ngày trả, số khách và giá/đêm lưu <strong>theo từng phòng</strong>. Khi đặt, nhân viên nhập một lần và áp cho mọi phòng; sau đó từng phòng sửa hoặc gia hạn riêng.</p></li>
<li><p>Số đêm tính tiền của phòng = CheckOutDate − CheckInDate của phòng đó (đã gồm gia hạn). Trả sớm không giảm số đêm.</p></li>
<li><p>Trùng lịch xét trên các lượt lưu trú có Status Booked hoặc CheckedIn: lượt hiện có CheckInDate &lt; ngày trả mới VÀ CheckOutDate &gt; ngày nhận mới. Lượt Cancelled hoặc CheckedOut không giữ lịch (phòng trả sớm được bán lại).</p></li>
<li><p>Tiền VND lưu DECIMAL(14,0); mọi khoản tính theo % làm tròn đến đồng.</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>Chuyển CheckInDate, CheckOutDate, Adults, Children từ Bookings sang BookingRooms; thêm cột tính Nights; index (RoomId, CheckInDate, CheckOutDate) có lọc theo Status.</p></li>
</ul></td>
</tr>
</tbody>
</table>

## 4. Luồng nghiệp vụ tổng thể

Cấu hình hệ thống → Quản lý khách hàng → Đặt phòng → Xác nhận booking → Check-in → Ghi nhận dịch vụ → Check-out toàn bộ phòng trong booking → Lập hóa đơn cuối cùng → Ghi nhận thanh toán/đối trừ tiền cọc → Housekeeping → Phòng Available → Dashboard/Báo cáo.

Quy ước phạm vi để tránh mâu thuẫn: một booking có thể chứa nhiều phòng, nhưng trong phiên bản đồ án này, các phòng thuộc cùng booking được check-in và check-out theo cùng một quy trình chung. Hóa đơn cuối cùng chỉ lập khi toàn bộ phòng của booking đã check-out. Không triển khai trả từng phòng ở các thời điểm khác nhau trong phiên bản cơ bản.

### Đề xuất sửa – I.4 Luồng nghiệp vụ tổng thể

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>Quy ước "các phòng cùng booking check-in/check-out chung, hóa đơn lập khi toàn bộ phòng đã trả" trái yêu cầu tính tiền và dịch vụ theo từng phòng. Một phòng trả sớm phải chờ các phòng khác mới có hóa đơn.</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>Thay đoạn "Quy ước phạm vi…" bằng luồng: Đặt phòng (giữ chỗ từng phòng) → Thu cọc theo phòng → Booking Confirmed → <strong>với từng phòng:</strong> Check-in → Ghi dịch vụ → (Gia hạn nếu cần) → Check-out: tính phụ thu trễ, lập hóa đơn của phòng, thu đủ hoặc hoàn dư → Housekeeping → Available. Booking tự chuyển CheckedOut khi phòng cuối cùng trả.</p></li>
<li><p>Một booking 3 phòng có 3 hóa đơn. Bảng kê tổng hợp theo booking (vw_BookingStatement) chỉ để đối chiếu, không phải hóa đơn.</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>BookingRooms trở thành bảng trung tâm: ServiceUsages, Invoices, Payments, HousekeepingTasks đều tham chiếu BookingRoomId.</p></li>
</ul></td>
</tr>
</tbody>
</table>

# 1. Đăng nhập và phân quyền

## 1. Mục đích

Đảm bảo chỉ nhân viên có tài khoản hoạt động và đúng quyền mới sử dụng chức năng.

## 2. Điều kiện thực hiện

- Nhân viên phải có tài khoản trong hệ thống; tài khoản đang hoạt động mới được đăng nhập.

- Username không được trùng; mật khẩu lưu dưới dạng hash, không lưu mật khẩu thô.

- Quyền được xác định theo Role: Admin, Manager, Receptionist, Housekeeping.

- Ẩn menu không đủ để bảo vệ chức năng; mỗi thao tác nghiệp vụ phải kiểm tra quyền ở tầng xử lý.

## 3. Quy trình xử lý

1.  Người dùng nhập Username và Password.

2.  Hệ thống kiểm tra trường bắt buộc, tìm tài khoản theo Username và xác minh mật khẩu.

3.  Nếu tài khoản không tồn tại, mật khẩu sai hoặc tài khoản bị khóa, từ chối đăng nhập và thông báo chung phù hợp.

4.  Nếu hợp lệ, tạo phiên làm việc gồm EmployeeId, FullName, Role; mở màn hình chính theo quyền.

5.  Khi đăng xuất, xóa thông tin phiên và quay lại màn hình đăng nhập.

## 4. Dữ liệu nghiệp vụ liên quan

| **Bảng/đối tượng** | **Thông tin sử dụng**                              |
|--------------------|----------------------------------------------------|
| Employees/Roles    | Username, PasswordHash, IsActive, RoleId, RoleName |

## 5. Checklist tự kiểm tra

- [ ] Từ chối tài khoản không tồn tại, sai mật khẩu hoặc IsActive = 0.

- [ ] Menu và thao tác phù hợp với Role.

- [ ] Không thể mở trực tiếp chức năng ngoài quyền bằng cách gọi Form.

- [ ] Đăng xuất xóa phiên làm việc.

## 6. Quy tắc thống nhất

- Không xây dựng bảng Permissions riêng trong phạm vi cơ bản; quyền theo Role được kiểm soát thống nhất trong code.

- Không cho Admin tự khóa tài khoản đang dùng nếu việc đó khiến không còn tài khoản quản trị khả dụng.

# 2. Quản lý vai trò (Roles)

## 1. Mục đích

Quản lý danh mục vai trò được dùng để phân quyền nhân viên.

## 2. Điều kiện thực hiện

- Chỉ Admin được thêm, sửa hoặc xóa vai trò.

- RoleName bắt buộc và duy nhất.

- Không xóa vai trò đang được nhân viên sử dụng; không xóa các vai trò hệ thống mặc định.

## 3. Quy trình xử lý

1.  Tải danh sách vai trò.

2.  Khi thêm, nhập RoleName và Description; kiểm tra bắt buộc và trùng tên.

3.  Khi sửa, kiểm tra trùng tên với bản ghi khác.

4.  Khi xóa, kiểm tra nhân viên đang gắn vai trò; nếu có thì từ chối.

5.  Lưu thành công thì tải lại danh sách và thông báo kết quả.

## 4. Dữ liệu nghiệp vụ liên quan

| **Bảng/đối tượng** | **Thông tin sử dụng**         |
|--------------------|-------------------------------|
| Roles              | RoleId, RoleName, Description |
| Employees          | RoleId                        |

## 5. Checklist tự kiểm tra

- [ ] Không lưu tên vai trò trống/trùng.

- [ ] Không xóa vai trò đang được sử dụng.

- [ ] Người không phải Admin không truy cập được chức năng.

## 6. Quy tắc thống nhất

- Đổi RoleName phải đồng thời đảm bảo code phân quyền vẫn nhận diện đúng vai trò; không tùy tiện đổi tên 4 vai trò chuẩn.

## 7. Đề xuất sửa và điều chỉnh database

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>Cho Admin thêm vai trò mới, nhưng quyền được viết cố định trong code theo 4 vai trò (mục 1, quy tắc mục 2). Vai trò mới sẽ không có quyền nào, còn đổi RoleName làm hỏng kiểm tra quyền.</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>4 vai trò cố định, tạo sẵn khi cài đặt: Admin, Manager, Receptionist, Housekeeping. Không thêm, xóa hay đổi tên.</p></li>
<li><p>Màn hình vai trò chỉ để xem; Admin được sửa Description.</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>CHECK RoleName IN ('Admin', 'Manager', 'Receptionist', 'Housekeeping'); seed sẵn 4 dòng.</p></li>
</ul></td>
</tr>
</tbody>
</table>

# 3. Quản lý nhân viên (Employees)

## 1. Mục đích

Quản lý hồ sơ nhân viên và trạng thái tài khoản.

## 2. Điều kiện thực hiện

- Chỉ Admin được tạo, sửa, khóa/mở khóa và reset mật khẩu nhân viên.

- FullName, Username, mật khẩu lúc tạo và Role là bắt buộc; Username duy nhất.

- Email/SĐT nếu nhập phải đúng định dạng; Role phải tồn tại.

- Tài khoản bị khóa không đăng nhập được.

- Không xóa cứng nhân viên đã phát sinh Booking hoặc Invoice; khóa tài khoản thay thế.

## 3. Quy trình xử lý

1.  Tải danh sách nhân viên kèm tên vai trò; cho phép tìm theo tên, Username, SĐT.

2.  Khi tạo mới, nhập thông tin và mật khẩu; hệ thống kiểm tra hợp lệ, băm mật khẩu và lưu.

3.  Khi sửa, cập nhật thông tin và Role; không thay đổi các giao dịch lịch sử đã gắn với EmployeeId.

4.  Khi reset mật khẩu, băm mật khẩu mới trước khi lưu.

5.  Khi khóa/mở khóa, cập nhật trạng thái và thông báo.

6.  Khi xóa, kiểm tra lịch sử; nếu có giao dịch thì từ chối và hướng dẫn khóa tài khoản.

## 4. Dữ liệu nghiệp vụ liên quan

| **Bảng/đối tượng** | **Thông tin sử dụng**                                                                  |
|--------------------|----------------------------------------------------------------------------------------|
| Employees          | EmployeeId, RoleId, FullName, Username, PasswordHash, Phone, Email, HireDate, IsActive |
| Bookings/Invoices  | EmployeeId của nhân viên phụ trách theo thiết kế dữ liệu                               |

## 5. Checklist tự kiểm tra

- [ ] Username trùng bị từ chối.

- [ ] Mật khẩu không lưu dạng thô.

- [ ] Tài khoản khóa bị từ chối đăng nhập.

- [ ] Không xóa nhân viên có dữ liệu lịch sử.

## 6. Quy tắc thống nhất

- Nhân viên tạo booking và nhân viên lập hóa đơn được lưu để truy vết theo khả năng database cuối cùng hỗ trợ.

## 7. Đề xuất sửa và điều chỉnh database

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>Quy tắc "lưu nhân viên tạo booking và lập hóa đơn theo khả năng database" chưa chắc chắn, trong khi truy vết là cần thiết cho tiền và trạng thái phòng.</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>Giữ: chỉ Admin quản lý nhân viên (đã sửa I.1 cho khớp).</p></li>
<li><p>Bắt buộc lưu người thực hiện: tạo booking, check-in, check-out, ghi dịch vụ, thu/hoàn tiền, lập/void hóa đơn, dọn phòng.</p></li>
<li><p>Không cho khóa hoặc đổi vai trò của Admin cuối cùng đang hoạt động.</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>Các cột CreatedBy, CheckInBy, CheckOutBy, IssuedBy, VoidedBy, EmployeeId, AssignedTo tham chiếu Employees (xem VII.4).</p></li>
</ul></td>
</tr>
</tbody>
</table>

# 4. Quản lý khách hàng (Customers)

## 1. Mục đích

Lưu hồ sơ khách hàng dùng cho đặt phòng và giao dịch.

## 2. Điều kiện thực hiện

- Admin, Manager, Receptionist được thêm/sửa; xóa do Admin/Manager thực hiện.

- FullName, IdNumber (CCCD/Passport), Phone bắt buộc; IdNumber duy nhất.

- Ngày sinh nếu có không được ở tương lai; Email nếu có phải đúng định dạng.

- Không xóa cứng khách đã có Booking hoặc dữ liệu giao dịch.

## 3. Quy trình xử lý

1.  Tải danh sách và tìm theo tên, CCCD/Passport hoặc số điện thoại.

2.  Khi thêm, nhập thông tin; kiểm tra bắt buộc, định dạng và trùng IdNumber.

3.  Lưu hồ sơ; mặc định Nationality là Việt Nam nếu bỏ trống.

4.  Khi sửa, kiểm tra IdNumber trùng với khách khác nếu thay đổi.

5.  Khi xóa, kiểm tra lịch sử booking; nếu có thì từ chối.

6.  Làm mới danh sách sau thao tác.

## 4. Dữ liệu nghiệp vụ liên quan

| **Bảng/đối tượng** | **Thông tin sử dụng**                                                                              |
|--------------------|----------------------------------------------------------------------------------------------------|
| Customers          | CustomerId, FullName, Gender, DateOfBirth, IdNumber, Phone, Email, Address, Nationality, CreatedAt |
| Bookings           | CustomerId                                                                                         |

## 5. Checklist tự kiểm tra

- [ ] Không lưu thiếu trường bắt buộc hoặc IdNumber trùng.

- [ ] Khách mới tạo chọn được ở màn hình Booking.

- [ ] Tìm kiếm trả về đúng kết quả.

- [ ] Không xóa khách có lịch sử.

## 6. Quy tắc thống nhất

- Cho phép cập nhật thông tin liên hệ khách cũ; các booking vẫn gắn với CustomerId đã lưu.

## 7. Đề xuất sửa và điều chỉnh database

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>Khi tính theo phòng, mỗi phòng có thể do người khác ở hoặc đơn vị khác thanh toán; hồ sơ khách chỉ có người đặt.</p></li>
<li><p>Quyết định cũ thêm CompanyName/TaxCode vào Customers làm hóa đơn cũ đổi theo khi sửa hồ sơ khách.</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>Booking giữ người đặt. Người ở thực tế của từng phòng (nếu khác người đặt) nhập tên và CCCD/hộ chiếu khi check-in phòng đó.</p></li>
<li><p>Thông tin xuất hóa đơn cho đơn vị (tên, mã số thuế, địa chỉ) nhập trên hóa đơn của phòng (mục 12), không lưu ở Customers.</p></li>
<li><p>Ngày sinh không ở tương lai được kiểm tra cả ở DB.</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>Thêm BookingRooms.GuestName, GuestIdNumber (NULL). Customers thêm CHECK DateOfBirth &lt;= ngày hiện tại; không thêm cột công ty.</p></li>
</ul></td>
</tr>
</tbody>
</table>

# 5. Quản lý loại phòng (RoomTypes)

## 1. Mục đích

Quản lý cấu hình loại phòng, sức chứa, loại giường và giá cơ bản.

## 2. Điều kiện thực hiện

- Nhân viên được phân quyền quản lý danh mục mới được thao tác.

- TypeName bắt buộc, duy nhất; BasePrice \> 0; MaxOccupancy \> 0.

- Không xóa loại phòng đang có phòng thực tế hoặc lịch sử giao dịch.

## 3. Quy trình xử lý

1.  Tải danh sách loại phòng.

2.  Thêm loại phòng bằng tên, giá cơ bản/đêm, sức chứa, loại giường và mô tả.

3.  Kiểm tra tên trùng, giá và sức chứa.

4.  Khi sửa giá hoặc sức chứa, áp dụng cho cấu hình mới; không tự sửa giá đã chốt ở booking cũ.

5.  Khi xóa, kiểm tra Room và dữ liệu liên quan; nếu đang được sử dụng thì từ chối.

## 4. Dữ liệu nghiệp vụ liên quan

| **Bảng/đối tượng** | **Thông tin sử dụng**                                               |
|--------------------|---------------------------------------------------------------------|
| RoomTypes          | RoomTypeId, TypeName, Description, BasePrice, MaxOccupancy, BedType |
| Rooms              | RoomTypeId                                                          |
| RoomTypeAmenities  | RoomTypeId                                                          |
| BookingRooms       | PricePerNight                                                       |

## 5. Checklist tự kiểm tra

- [ ] Không lưu tên trống/trùng, giá \<= 0 hoặc sức chứa \<= 0.

- [ ] Giá booking cũ không đổi khi giá cơ bản thay đổi.

- [ ] Không xóa loại phòng đang được sử dụng.

## 6. Quy tắc thống nhất

- Sức chứa được kiểm tra khi đặt phòng: tổng Adults + Children không vượt tổng sức chứa của các phòng được chọn.

## 7. Đề xuất sửa và điều chỉnh database

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>Tài liệu yêu cầu BasePrice &gt; 0, DB cũ cho ≥ 0.</p></li>
<li><p>Không có cách ngừng dùng loại phòng đã có lịch sử (chỉ có "không xóa").</p></li>
<li><p>Sức chứa kiểm tra theo tổng các phòng: 4 khách có thể dồn vào một phòng đơn nếu đặt kèm phòng khác.</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>BasePrice &gt; 0; MaxOccupancy từ 1 đến 10.</p></li>
<li><p>Ngừng sử dụng bằng IsActive = 0: không chọn khi tạo phòng mới hoặc đặt phòng; dữ liệu cũ giữ nguyên.</p></li>
<li><p>Sức chứa kiểm tra <strong>theo từng phòng</strong>: Adults + Children của phòng ≤ MaxOccupancy loại phòng đó (thay quy tắc tổng sức chứa ở mục 6 bên trên).</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>CHECK BasePrice &gt; 0, CHECK MaxOccupancy BETWEEN 1 AND 10; thêm RoomTypes.IsActive.</p></li>
</ul></td>
</tr>
</tbody>
</table>

# 6. Quản lý phòng (Rooms)

## 1. Mục đích

Quản lý từng phòng vật lý và tình trạng vận hành.

## 2. Điều kiện thực hiện

- RoomNumber bắt buộc và duy nhất; RoomType phải tồn tại.

- Phòng mới có trạng thái Available.

- Không cho đặt phòng Maintenance; không cho sửa/xóa làm mất lịch sử giao dịch.

- Trạng thái phòng chỉ được đổi qua các nghiệp vụ có thẩm quyền, không cho người dùng chọn tùy ý.

## 3. Quy trình xử lý

1.  Tải danh sách phòng kèm loại phòng, tầng và trạng thái.

2.  Thêm phòng, kiểm tra số phòng và loại phòng; đặt trạng thái Available.

3.  Sửa số phòng, tầng, ghi chú; chỉ đổi RoomType khi phòng Available và không có xung đột lịch.

4.  Đưa phòng từ Available sang Maintenance khi cần bảo trì; phòng Maintenance không thể đặt.

5.  Khi bảo trì xong, chuyển Maintenance về Available sau khi xác nhận đã sẵn sàng.

6.  Booking/check-in/check-out/housekeeping tự cập nhật trạng thái phòng theo quy tắc chung.

## 4. Dữ liệu nghiệp vụ liên quan

| **Bảng/đối tượng** | **Thông tin sử dụng**                               |
|--------------------|-----------------------------------------------------|
| Rooms              | RoomId, RoomTypeId, RoomNumber, Floor, Status, Note |
| RoomTypes          | TypeName, BasePrice, MaxOccupancy                   |
| BookingRooms       | RoomId, BookingId                                   |

## 5. Checklist tự kiểm tra

- [ ] Không tạo trùng số phòng.

- [ ] Phòng Maintenance không được chọn khi đặt.

- [ ] Không chuyển Occupied sang Available trực tiếp.

- [ ] Không xóa phòng có lịch sử booking.

## 6. Quy tắc thống nhất

- Trạng thái hiện tại không thay thế việc kiểm tra lịch theo ngày. Một phòng đang Available hôm nay vẫn có thể có booking ở tương lai.

## 7. Đề xuất sửa và điều chỉnh database

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>Bước 5 cho Maintenance → Available trực tiếp, trái mục 14 và phần II (phòng sửa xong phải qua dọn phòng).</p></li>
<li><p>Housekeeping phát hiện hỏng phải "chuyển yêu cầu cho người có quyền" nhưng không có nơi ghi nhận.</p></li>
<li><p>Đưa phòng vào bảo trì khi đã có khách đặt các ngày tới: chưa có xử lý.</p></li>
<li><p>Đổi RoomType khi phòng có lịch tương lai làm sai sức chứa so với khách đã đặt.</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>Bảng chuyển trạng thái dưới đây là quy tắc duy nhất. Mọi chuyển khác bị từ chối.</p></li>
<li><p>Khi đưa phòng vào bảo trì, hệ thống liệt kê các lượt Booked trùng thời gian để nhân viên đổi phòng (mục 8).</p></li>
<li><p>Chỉ đổi RoomType khi phòng không có lượt Booked/CheckedIn từ hôm nay trở đi.</p></li>
<li><p>Ngừng khai thác phòng bằng IsActive = 0 thay vì xóa.</p></li>
<li><p>Đổi phòng khi khách đang ở: ngoài phạm vi (mục V).</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>Rooms.Status CHECK 4 giá trị; thêm Rooms.IsActive; thủ tục sp_SetRoomMaintenance, sp_FinishMaintenance; kết quả phát hiện hỏng lưu ở HousekeepingTasks.</p></li>
</ul></td>
</tr>
</tbody>
</table>

| **Chuyển trạng thái**   | **Kích hoạt bởi**                                                                          | **Ai được làm** |
|-------------------------|--------------------------------------------------------------------------------------------|-----------------|
| Available → Occupied    | Check-in phòng                                                                             | A, M, R         |
| Occupied → Cleaning     | Check-out phòng (tự tạo việc dọn)                                                          | A, M, R         |
| Cleaning → Available    | Hoàn tất việc dọn, kết quả Clean                                                           | H, A, M         |
| Cleaning → Maintenance  | Hoàn tất việc dọn, kết quả NeedMaintenance (bắt buộc ghi chú)                              | H, A, M         |
| Available → Maintenance | Đưa phòng vào bảo trì                                                                      | A, M            |
| Maintenance → Cleaning  | Kết thúc bảo trì (tự tạo việc dọn)                                                         | A, M            |
| Không cho phép          | Occupied → Available, Maintenance → Available, Cleaning → Occupied, Occupied → Maintenance | —               |

# 7. Quản lý tiện nghi (Amenities)

## 1. Mục đích

Quản lý danh mục tiện nghi và cấu hình tiện nghi theo loại phòng.

## 2. Điều kiện thực hiện

- Tên tiện nghi bắt buộc và duy nhất.

- Mỗi loại phòng có thể có nhiều tiện nghi; một tiện nghi có thể gán cho nhiều loại phòng.

- Không tạo liên kết trùng RoomType–Amenity.

## 3. Quy trình xử lý

1.  Tải danh sách tiện nghi.

2.  Thêm/sửa tên và mô tả theo thiết kế màn hình.

3.  Chọn loại phòng và hiển thị các tiện nghi đã gán/chưa gán.

4.  Thêm liên kết cho tiện nghi được chọn; bỏ chọn thì xóa liên kết, không xóa tiện nghi khỏi danh mục.

5.  Không xóa tiện nghi đang được gán; trước tiên cần bỏ liên kết theo quyền.

## 4. Dữ liệu nghiệp vụ liên quan

| **Bảng/đối tượng** | **Thông tin sử dụng**                      |
|--------------------|--------------------------------------------|
| Amenities          | AmenityId, AmenityName, Description nếu có |
| RoomTypeAmenities  | RoomTypeId, AmenityId                      |
| RoomTypes          | RoomTypeId, TypeName                       |

## 5. Checklist tự kiểm tra

- [ ] Không lưu tên trống/trùng.

- [ ] Không tạo cặp liên kết trùng.

- [ ] Bỏ gán không làm mất bản ghi danh mục tiện nghi.

- [ ] Chi tiết loại phòng hiển thị đúng tiện nghi.

## 6. Quy tắc thống nhất

- Tiện nghi được gán ở cấp RoomType, không gán trực tiếp cho từng Room; không quản lý tồn kho số lượng tiện nghi.

## 7. Đề xuất sửa và điều chỉnh database

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>DB cũ đặt ON DELETE CASCADE trên RoomTypeAmenities: xóa tiện nghi sẽ tự xóa liên kết, trái quy định "không xóa tiện nghi đang được gán".</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>Giữ quy định của mục này; DB phải chặn được việc xóa.</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>Khóa ngoại RoomTypeAmenities → Amenities và → RoomTypes dùng NO ACTION (không cascade).</p></li>
</ul></td>
</tr>
</tbody>
</table>

# 7A. Quản lý danh mục dịch vụ (Services) – MỤC BỔ SUNG

*Mục này chưa có trong bản gốc nhưng mục 10 phụ thuộc vào nó.*

## 1. Mục đích

Quản lý danh sách dịch vụ và đơn giá dùng khi ghi nhận dịch vụ cho phòng.

## 2. Điều kiện thực hiện

- Admin, Manager thêm, sửa, ngừng dịch vụ; Receptionist chỉ xem.

- ServiceName bắt buộc và duy nhất; Unit bắt buộc; Price ≥ 0 (cho phép dịch vụ miễn phí).

- Không xóa dịch vụ đã có lượt sử dụng; ngừng bằng IsActive = 0.

- Dịch vụ hệ thống (IsSystem = 1, ví dụ "Phụ thu trả phòng trễ") không được sửa, ngừng, xóa hay chọn tay.

## 3. Quy trình xử lý

1.  Tải danh sách dịch vụ, lọc theo nhóm và trạng thái.

2.  Thêm: nhập tên, nhóm, đơn vị, giá; kiểm tra trùng tên.

3.  Sửa giá: chỉ áp dụng cho lượt sử dụng mới; lượt đã ghi giữ đơn giá cũ.

4.  Ngừng hoặc mở lại: đổi IsActive.

5.  Xóa: chỉ khi chưa có lượt sử dụng; nếu có thì từ chối và hướng dẫn ngừng.

## 4. Dữ liệu nghiệp vụ liên quan

| **Bảng/đối tượng** | **Thông tin sử dụng**                                             |
|--------------------|-------------------------------------------------------------------|
| Services           | ServiceId, ServiceName, Category, Unit, Price, IsActive, IsSystem |
| ServiceUsages      | ServiceId (kiểm tra trước khi xóa)                                |

## 5. Checklist tự kiểm tra

- [ ] Không lưu tên trống/trùng, giá âm.

- [ ] Đổi giá không làm đổi lượt sử dụng cũ.

- [ ] Dịch vụ ngừng không xuất hiện khi ghi nhận dịch vụ.

- [ ] Không sửa, ngừng, xóa được dịch vụ hệ thống.

## 6. Điều chỉnh database

- Thêm Services.IsSystem; CHECK Price \>= 0; seed dòng "Phụ thu trả phòng trễ" (IsSystem = 1, Price = 0, đơn giá thực ghi lúc check-out).

# 8. Đặt phòng (Booking)

## 1. Mục đích

Tạo, xác nhận, sửa và hủy phiếu đặt một hoặc nhiều phòng cho một khách hàng trong một khoảng ngày.

## 2. Điều kiện thực hiện

- Khách hàng phải tồn tại; booking phải có ít nhất một phòng.

- Ngày nhận/trả bắt buộc; ngày trả lớn hơn ngày nhận.

- Phòng phải tồn tại, không Maintenance và không có booking hiệu lực bị trùng khoảng ngày.

- Giá mỗi phòng lấy từ RoomTypes.BasePrice tại thời điểm tạo và lưu riêng cho booking.

- Số khách không vượt tổng sức chứa của các phòng được chọn.

- Chỉ booking Pending hoặc Confirmed mới được sửa/hủy theo điều kiện; booking đã CheckedIn/CheckedOut không được sửa như booking mới.

## 3. Quy trình xử lý

1.  Mở danh sách Booking, tìm kiếm theo mã, khách, ngày hoặc trạng thái.

2.  Chọn khách hàng; nếu chưa có thì tạo hồ sơ khách trước.

3.  Nhập ngày nhận/trả, số người lớn/trẻ em, tiền cọc dự kiến và ghi chú.

4.  Hệ thống kiểm tra ngày hợp lệ và tính số đêm dự kiến.

5.  Hệ thống tìm phòng khả dụng bằng cả trạng thái vận hành và lịch booking trong khoảng ngày; loại trừ booking Cancelled.

6.  Chọn một hoặc nhiều phòng; hiển thị loại phòng, sức chứa và giá/đêm.

7.  Kiểm tra tổng sức chứa và kiểm tra xung đột lần cuối ngay trước khi lưu.

8.  Lưu Booking và các dòng BookingRooms trong cùng một transaction; lưu PricePerNight cho từng phòng.

9.  Booking mới ở trạng thái Pending; nhân viên có quyền xác nhận để chuyển sang Confirmed.

10. Khi sửa ngày/phòng, kiểm tra lại toàn bộ lịch và cập nhật Booking cùng BookingRooms trong một transaction.

11. Khi hủy, chuyển Status = Cancelled; không xóa lịch sử và giải phóng lịch đặt. Xử lý tiền cọc theo chính sách đã công bố.

## 4. Dữ liệu nghiệp vụ liên quan

| **Bảng/đối tượng** | **Thông tin sử dụng**                                                                                            |
|--------------------|------------------------------------------------------------------------------------------------------------------|
| Bookings           | BookingId, CustomerId, EmployeeId, CheckInDate, CheckOutDate, Adults, Children, Status, Deposit, Note, CreatedAt |
| BookingRooms       | BookingId, RoomId, PricePerNight                                                                                 |
| Rooms              | RoomId, RoomTypeId, Status                                                                                       |
| RoomTypes          | BasePrice, MaxOccupancy                                                                                          |

## 5. Checklist tự kiểm tra

- [ ] Không tạo booking thiếu khách/phòng.

- [ ] Không cho ngày trả \<= ngày nhận.

- [ ] Không cho đặt trùng phòng trong khoảng thời gian giao nhau.

- [ ] Cho phép booking kế tiếp bắt đầu đúng ngày booking trước kết thúc.

- [ ] Giá từng phòng được chốt tại thời điểm đặt.

- [ ] Booking Cancelled không chiếm lịch phòng.

- [ ] Không lưu Booking nếu lưu BookingRooms thất bại.

## 6. Quy tắc thống nhất

- Quy tắc trùng lịch: booking hiện có CheckInDate \< ngày trả mới VÀ CheckOutDate \> ngày nhận mới; loại trừ Cancelled. Pending cũng giữ lịch trong khi chờ xác nhận, tránh hai khách cùng giữ một phòng.

- Tiền cọc là khoản thu gắn với booking. Trước khi chốt database, nhóm phải thiết kế cách ghi nhận cọc và đối trừ vào hóa đơn cuối; không coi Deposit là doanh thu.

- Booking nhiều phòng được check-in/check-out cùng quy trình trong phiên bản cơ bản.

## 7. Đề xuất sửa và điều chỉnh database

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>Ngày và số khách ở cấp booking: không tính, không gia hạn, không hủy riêng từng phòng.</p></li>
<li><p>Pending giữ lịch vô thời hạn; không có xử lý khách không đến (no-show).</p></li>
<li><p>"Xử lý tiền cọc theo chính sách đã công bố" nhưng chưa có chính sách; cách ghi nhận cọc chưa chốt.</p></li>
<li><p>Chưa chặn đặt ngày trong quá khứ.</p></li>
<li><p>Phòng đã đặt bị hỏng: chưa có quy trình đổi phòng; sửa booking hiện lấy lại BasePrice của phòng mới, trái quyết định "nâng hạng nhưng giữ giá".</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>Booking là phiếu đặt của một người đặt, gồm một hoặc nhiều lượt lưu trú phòng. Mỗi lượt có ngày nhận/trả, số khách, giá/đêm, trạng thái riêng.</p></li>
<li><p>Khi tạo: CheckInDate ≥ hôm nay; CheckOutDate &gt; CheckInDate; Adults + Children mỗi phòng ≤ MaxOccupancy; phòng IsActive = 1, không Maintenance, không trùng lịch.</p></li>
<li><p><strong>Cọc:</strong> chọn DepositRate 50% hoặc 100% cho cả booking. Cọc yêu cầu của mỗi phòng = ROUND(PricePerNight × Số đêm × DepositRate / 100). Tiền cọc ghi vào Payments (Type = Deposit) theo từng phòng; khách đóng một lần cho cả booking thì hệ thống chia theo phần cọc còn thiếu của từng phòng.</p></li>
<li><p>Booking tự chuyển <strong>Confirmed</strong> khi mọi phòng còn hiệu lực đủ cọc. Booking chưa từng được xác nhận mà quá <strong>24 giờ</strong> kể từ lúc tạo chưa đủ cọc thì tự hủy (lý do "Hết hạn giữ chỗ"). Khách đến đặt và nhận ngay đóng cọc tại quầy rồi check-in luôn.</p></li>
<li><p><strong>Sửa:</strong> chỉ sửa lượt đang Booked; đổi ngày/phòng phải kiểm tra lại lịch. Khách yêu cầu đổi phòng → giá theo BasePrice phòng mới. Khách sạn đổi do phòng hỏng → chọn phòng cùng hạng hoặc cao hơn, <strong>giữ PricePerNight cũ</strong>, bắt buộc ghi lý do. Cọc yêu cầu tăng mà chưa đủ → Booking quay về Pending (không tự hủy vì đã từng xác nhận).</p></li>
<li><p><strong>Hủy</strong> (từng phòng hoặc cả booking): trước 14:00 ngày nhận <strong>ít nhất 48 giờ</strong> → hoàn 100% tiền đã thu của phòng đó; <strong>dưới 48 giờ</strong> → không hoàn.</p></li>
<li><p><strong>No-show:</strong> đến 12:00 ngày sau CheckInDate mà chưa check-in → nhân viên đánh dấu No-show (Cancelled, lý do NoShow), không hoàn cọc.</p></li>
<li><p>Tiền cọc giữ lại là doanh thu thực thu tại thời điểm nhận; nếu hoàn thì ghi Refund.</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>Bookings bỏ CheckInDate, CheckOutDate, Adults, Children, Deposit, EmployeeId; thêm DepositRate, CreatedBy, ConfirmedAt, CancelledAt, CancelReason.</p></li>
<li><p>BookingRooms thêm CheckInDate, CheckOutDate, Nights (tính), Adults, Children, Status, GuestName, GuestIdNumber, CancelReason, CancelledAt, Note; bỏ UNIQUE(BookingId, RoomId).</p></li>
<li><p>Thủ tục: sp_CreateBooking, sp_UpdateBookingRoom, sp_CancelBookingRoom, sp_RecordBookingDeposit, sp_ExpirePendingBookings, sp_RefreshBookingStatus; hàm fn_AvailableRooms.</p></li>
</ul></td>
</tr>
</tbody>
</table>

# 9. Check-in

## 1. Mục đích

Xác nhận khách đến và chuyển các phòng đã đặt sang trạng thái đang lưu trú.

## 2. Điều kiện thực hiện

- Booking phải ở trạng thái Confirmed; không check-in booking Pending hoặc Cancelled.

- Ngày hiện tại phải phù hợp với ngày nhận theo chính sách check-in của khách sạn; trường hợp nhận sớm cần được người có quyền xác nhận.

- Tất cả phòng thuộc booking phải còn đủ điều kiện nhận khách, không Maintenance/Occupied và không có xung đột vận hành.

- Khách hàng và thông tin booking phải được hiển thị để nhân viên đối chiếu.

## 3. Quy trình xử lý

1.  Tìm booking theo mã, tên khách hoặc số điện thoại.

2.  Kiểm tra trạng thái Confirmed, ngày dự kiến, số người và danh sách phòng.

3.  Đối chiếu thông tin khách; cập nhật thông tin khách nếu cần trước khi xác nhận.

4.  Nhân viên xác nhận check-in; hệ thống ghi thời điểm thực tế cho từng BookingRoom.

5.  Trong cùng transaction, chuyển các phòng nhận khách sang Occupied và Booking sang CheckedIn.

6.  Hiển thị kết quả, thời gian nhận thực tế và danh sách phòng.

## 4. Dữ liệu nghiệp vụ liên quan

| **Bảng/đối tượng** | **Thông tin sử dụng**                                    |
|--------------------|----------------------------------------------------------|
| Bookings           | BookingId, CustomerId, CheckInDate, CheckOutDate, Status |
| BookingRooms       | BookingRoomId, RoomId, ActualCheckIn                     |
| Rooms              | Status                                                   |
| Customers          | Thông tin khách                                          |

## 5. Checklist tự kiểm tra

- [ ] Không check-in booking không tồn tại hoặc không ở Confirmed.

- [ ] Không nhận phòng Maintenance hoặc đang có khách.

- [ ] Check-in thành công ghi thời gian thực tế và chuyển phòng sang Occupied.

- [ ] Nếu cập nhật một phòng thất bại, rollback toàn bộ check-in booking.

## 6. Quy tắc thống nhất

- Trong phạm vi cơ bản, các phòng của một booking được check-in cùng lúc. Thời gian thực tế được lưu theo từng BookingRoom để phục vụ tính tiền và truy vết.

## 7. Đề xuất sửa và điều chỉnh database

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>Check-in toàn bộ booking cùng lúc trái yêu cầu theo từng phòng.</p></li>
<li><p>"Nhận sớm cần người có quyền xác nhận" chưa rõ, và khác quyết định đã chốt "được nhận sớm, không phụ thu". Nhận sớm từ ngày trước CheckInDate nghĩa là ở thêm một đêm không trả tiền.</p></li>
<li><p>Chưa kiểm tra cọc; phòng đã đặt bị hỏng chưa có cách xử lý.</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>Check-in <strong>theo từng phòng</strong>: lượt lưu trú Status = Booked; Booking ở Confirmed hoặc CheckedIn (đã có phòng khác nhận).</p></li>
<li><p>Tiền đã thu của phòng ≥ cọc yêu cầu.</p></li>
<li><p>Được check-in từ 00:00 ngày CheckInDate. Trước 14:00 là nhận sớm trong ngày: miễn phí, không cần duyệt, chỉ cần phòng Available. Muốn nhận trước ngày CheckInDate → sửa booking thêm đêm (có tính tiền) trước khi check-in.</p></li>
<li><p>Khách đến muộn hơn CheckInDate (trước mốc no-show) vẫn check-in được, không giảm tiền các đêm đã giữ.</p></li>
<li><p>Phòng phải Available. Cleaning → chờ dọn xong. Maintenance → đổi phòng theo mục 8 (nâng hạng, giữ giá) rồi check-in.</p></li>
<li><p>Xác thực: đối chiếu CCCD/hộ chiếu người đặt, hoặc nhập tên và CCCD người ở thực tế của phòng.</p></li>
<li><p>Một transaction: ghi ActualCheckIn, CheckInBy; lượt lưu trú → CheckedIn; phòng → Occupied; tính lại trạng thái Booking.</p></li>
<li><p>Checklist bổ sung: check-in phòng A khi phòng B cùng booking chưa đến; chặn khi thiếu cọc; chặn check-in trước ngày CheckInDate.</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>BookingRooms.Status, ActualCheckIn, CheckInBy, GuestName, GuestIdNumber; thủ tục sp_CheckInRoom.</p></li>
</ul></td>
</tr>
</tbody>
</table>

# 10. Sử dụng dịch vụ (Service Usage)

## 1. Mục đích

Ghi nhận dịch vụ khách sử dụng trong thời gian lưu trú để tính vào hóa đơn.

## 2. Điều kiện thực hiện

- Chỉ nhân viên có quyền nghiệp vụ lưu trú mới được ghi nhận dịch vụ.

- Booking phải ở trạng thái CheckedIn và phòng thuộc booking đang lưu trú.

- Dịch vụ phải đang hoạt động; số lượng phải lớn hơn 0.

- Đơn giá được chốt tại thời điểm ghi nhận; chỉ ghi dịch vụ trong khoảng lưu trú, trừ trường hợp quản lý cho phép điều chỉnh có lý do.

## 3. Quy trình xử lý

1.  Chọn booking đang lưu trú và phòng cụ thể trong booking.

2.  Chọn dịch vụ đang hoạt động; hiển thị đơn vị và đơn giá hiện tại.

3.  Nhập số lượng, thời điểm sử dụng và ghi chú nếu có.

4.  Hệ thống kiểm tra số lượng, thời điểm và trạng thái phòng/booking.

5.  Lưu ServiceUsage cùng đơn giá tại thời điểm phát sinh; thành tiền = Quantity × UnitPrice.

6.  Cho phép sửa/hủy lượt dùng trước khi chốt hóa đơn; sau khi hóa đơn phát hành, chỉ người có quyền được điều chỉnh bằng quy trình có lưu vết.

## 4. Dữ liệu nghiệp vụ liên quan

| **Bảng/đối tượng** | **Thông tin sử dụng**                                          |
|--------------------|----------------------------------------------------------------|
| Services           | ServiceId, ServiceName, Category, Unit, Price, IsActive        |
| ServiceUsages      | UsageId, BookingRoomId, ServiceId, Quantity, UnitPrice, UsedAt |
| BookingRooms       | BookingRoomId, ActualCheckIn, ActualCheckOut                   |

## 5. Checklist tự kiểm tra

- [ ] Không ghi dịch vụ cho booking chưa check-in/đã check-out.

- [ ] Không chọn dịch vụ ngừng hoạt động cho lượt sử dụng mới.

- [ ] Không lưu số lượng \<= 0.

- [ ] Thành tiền và tổng dịch vụ được tính đúng.

- [ ] Giá danh mục thay đổi không làm thay đổi lượt dùng cũ.

## 6. Quy tắc thống nhất

- Dịch vụ được gắn với BookingRoom để biết phòng nào sử dụng. Khi chốt hóa đơn, tổng dịch vụ là tổng Quantity × UnitPrice của các lượt hợp lệ thuộc booking.

## 7. Đề xuất sửa và điều chỉnh database

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>Điều kiện "Booking ở CheckedIn" sai khi tính theo phòng: phòng đã trả vẫn được thêm dịch vụ vì booking còn CheckedIn; phòng đã nhận lại bị chặn nếu booking chưa đổi trạng thái.</p></li>
<li><p>Bước 6 cho chỉnh dịch vụ sau khi phát hành hóa đơn, trái quyết định "khóa cứng" đã chốt và làm hóa đơn đã thu tiền bị sai.</p></li>
<li><p>Mất quyết định cảnh báo khi số lượng lớn; phụ thu trả trễ chưa có chỗ ghi.</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>Điều kiện ở <strong>cấp phòng</strong>: lượt lưu trú Status = CheckedIn. UsedAt nằm trong [ActualCheckIn, thời điểm hiện tại].</p></li>
<li><p>Chỉ chọn dịch vụ IsActive = 1 và IsSystem = 0.</p></li>
<li><p>Quantity &gt; 20 → hiện hộp xác nhận, không chặn.</p></li>
<li><p>Sửa/xóa lượt dùng: được khi phòng còn CheckedIn (A, M, R). Sau khi phòng trả và có hóa đơn Paid → <strong>khóa cứng</strong>. Muốn sửa phải void hóa đơn (A, M, có lý do), sửa, rồi lập lại hóa đơn (mục 12).</p></li>
<li><p>Phụ thu trả trễ do hệ thống tự ghi thành một lượt dùng dịch vụ hệ thống lúc check-out (mục 11).</p></li>
<li><p>Mỗi lượt dùng lưu người ghi và thời điểm ghi.</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>ServiceUsages thêm Amount (cột tính Quantity × UnitPrice), Note, CreatedBy, CreatedAt; Services.IsSystem; thủ tục sp_AddServiceUsage, sp_UpdateServiceUsage, sp_DeleteServiceUsage kiểm tra khóa.</p></li>
</ul></td>
</tr>
</tbody>
</table>

# 11. Check-out

## 1. Mục đích

Kết thúc lưu trú, chốt số đêm, các khoản dịch vụ và chuyển phòng sang chờ dọn.

## 2. Điều kiện thực hiện

- Booking phải ở trạng thái CheckedIn.

- Tất cả phòng thuộc booking phải được xác nhận trả trong cùng quy trình cơ bản.

- Không còn lượt sử dụng dịch vụ đang nhập dở; các khoản điều chỉnh phải được xác nhận trước khi chốt.

- Không lập hóa đơn cuối cùng trùng cho cùng một booking.

## 3. Quy trình xử lý

1.  Tìm booking CheckedIn và hiển thị khách, các phòng, ngày dự kiến, thời gian nhận thực tế và dịch vụ phát sinh.

2.  Đối chiếu dịch vụ, số lượng và đơn giá; xử lý điều chỉnh hợp lệ trước khi chốt.

3.  Nhập/xác nhận thời điểm trả thực tế cho từng BookingRoom.

4.  Tính số đêm tính tiền theo chính sách ngày lưu trú đã thống nhất; mặc định số đêm dự kiến là ngày trả trừ ngày nhận. Nếu trả muộn, áp dụng phụ thu chỉ khi chính sách phụ thu đã được công bố và cấu hình.

5.  Tính tiền phòng = tổng từng phòng (PricePerNight × số đêm tính tiền của phòng đó).

6.  Tính tiền dịch vụ = tổng (Quantity × UnitPrice) của các lượt dịch vụ thuộc booking.

7.  Xác nhận giảm giá, thuế và các phụ thu hợp lệ; hiển thị bảng tính trước khi chốt.

8.  Khi người dùng xác nhận, trong transaction ghi ActualCheckOut, chuyển phòng Occupied → Cleaning, chuyển Booking → CheckedOut và tạo dữ liệu hóa đơn cuối cùng.

9.  Nếu bất kỳ bước ghi nào thất bại, rollback và giữ booking ở trạng thái trước đó; thông báo lỗi.

## 4. Dữ liệu nghiệp vụ liên quan

| **Bảng/đối tượng** | **Thông tin sử dụng**                                             |
|--------------------|-------------------------------------------------------------------|
| Bookings           | BookingId, CheckInDate, CheckOutDate, Status                      |
| BookingRooms       | BookingRoomId, PricePerNight, ActualCheckIn, ActualCheckOut       |
| ServiceUsages      | Quantity, UnitPrice, UsedAt                                       |
| Rooms              | Status                                                            |
| Invoices           | RoomCharge, ServiceCharge, Discount, TaxRate, TotalAmount, Status |

## 5. Checklist tự kiểm tra

- [ ] Không checkout booking chưa CheckedIn.

- [ ] Tiền phòng và tiền dịch vụ có thể đối chiếu từ chi tiết.

- [ ] Không tạo hóa đơn cuối trùng.

- [ ] Checkout thành công thì booking CheckedOut và phòng Cleaning.

- [ ] Thao tác checkout có tính nguyên tử.

## 6. Quy tắc thống nhất

- Phiên bản cơ bản không hỗ trợ trả từng phòng riêng lẻ trong cùng booking. Tất cả phòng được checkout cùng lúc để tránh hóa đơn cuối thiếu tiền phòng/dịch vụ.

- Quy tắc phụ thu trả muộn chưa tự động áp dụng nếu chưa có chính sách cụ thể. Không tự ý thêm một công thức phụ thu khác nhau giữa các màn hình.

## 7. Đề xuất sửa và điều chỉnh database

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>Check-out gộp cả booking, trái yêu cầu tính theo từng phòng.</p></li>
<li><p>Phụ thu trả trễ bị gỡ dù nhóm đã chốt chính sách.</p></li>
<li><p>Không chặn khách rời đi khi chưa thanh toán, khác quyết định "thu đủ 100% mới cho trả phòng".</p></li>
<li><p>Chưa có gia hạn khi đang ở (mục 8 chỉ cho sửa lượt Booked); trả sớm chưa ghi rõ; chưa tạo việc dọn phòng.</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>Check-out <strong>theo từng phòng</strong> (Status = CheckedIn); phòng khác cùng booking không bị ảnh hưởng.</p></li>
<li><p>Thời điểm trả thực tế lấy lúc xem trước hóa đơn và giữ cố định khi xác nhận, để phụ thu không đổi giữa lúc xem và lúc bấm. Quá 10 phút phải xem lại.</p></li>
<li><p>Số đêm = Nights của phòng (đã gồm gia hạn). <strong>Trả sớm: không hoàn</strong>, tính đủ số đêm.</p></li>
<li><p><strong>Phụ thu trả trễ</strong> tính trên giá một đêm của phòng (PricePerNight), mốc 12:00 ngày CheckOutDate, theo bảng bên dưới. <strong>Sau 18:00</strong> không tính theo giờ mà phải gia hạn thêm 1 đêm trước khi check-out.</p></li>
<li><p>Phụ thu ghi thành một lượt dịch vụ hệ thống "Phụ thu trả phòng trễ" (Quantity = 1, UnitPrice = số tiền phụ thu, ghi chú số phút trễ).</p></li>
<li><p><strong>Gia hạn</strong> khi đang ở: dời CheckOutDate của phòng, kiểm tra trùng lịch các đêm thêm, giữ PricePerNight, không đòi thêm cọc.</p></li>
<li><p><strong>Thanh toán:</strong> hóa đơn phải thu đủ trong cùng thao tác check-out. Còn thiếu → nhập khoản thu; thu không đủ → không cho check-out. Đã thu dư → hệ thống ghi Refund phần dư.</p></li>
<li><p>Một transaction: ghi phụ thu, ActualCheckOut/CheckOutBy, lập hóa đơn (mục 12), ghi khoản thu/hoàn, gắn các khoản thu của phòng vào hóa đơn, lượt lưu trú → CheckedOut, phòng → Cleaning, tạo việc dọn phòng, tính lại trạng thái Booking.</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>BookingRooms.ActualCheckOut, CheckOutBy; hàm fn_LateCheckoutFee; thủ tục sp_PreviewCheckOut, sp_CheckOutRoom, sp_ExtendStay.</p></li>
</ul></td>
</tr>
</tbody>
</table>

**Chính sách phụ thu trả phòng trễ** (L = số phút trễ so với 12:00 ngày CheckOutDate):

- L ≤ 30 phút: không phụ thu.

- 30 \< L ≤ 120: mỗi 30 phút (tính từ phút thứ 31, chưa đủ 30 phút tính tròn 30 phút) phụ thu 5% → CEILING((L − 30) / 30) × 5%.

- L \> 120: 15% cho 2 giờ đầu, cộng mỗi giờ tiếp theo (chưa đủ giờ tính tròn giờ) 15% → 15% + CEILING((L − 120) / 60) × 15%.

- Sau 18:00 (L \> 360): bắt buộc gia hạn 1 đêm, không tính phụ thu theo giờ.

| **Giờ trả thực tế** | **Trễ**       | **Phụ thu**   | **Ví dụ phòng 1.000.000 đ/đêm** |
|---------------------|---------------|---------------|---------------------------------|
| Đến 12:30           | ≤ 30 phút     | 0%            | 0 đ                             |
| 12:31 – 13:00       | 31 – 60 phút  | 5%            | 50.000 đ                        |
| 13:01 – 13:30       | 61 – 90 phút  | 10%           | 100.000 đ                       |
| 13:31 – 14:00       | 91 – 120 phút | 15%           | 150.000 đ                       |
| 14:01 – 15:00       | 2 – 3 giờ     | 30%           | 300.000 đ                       |
| 15:01 – 16:00       | 3 – 4 giờ     | 45%           | 450.000 đ                       |
| 16:01 – 17:00       | 4 – 5 giờ     | 60%           | 600.000 đ                       |
| 17:01 – 18:00       | 5 – 6 giờ     | 75%           | 750.000 đ                       |
| Sau 18:00           | \> 6 giờ      | Gia hạn 1 đêm | 1.000.000 đ (tiền đêm)          |

# 12. Hóa đơn (Invoices)

## 1. Mục đích

Tổng hợp chi phí lưu trú và dịch vụ thành một hóa đơn cuối cho booking.

## 2. Điều kiện thực hiện

- Hóa đơn cuối chỉ được lập khi booking đã hoàn tất check-out.

- Mỗi booking chỉ có một hóa đơn cuối trong phạm vi cơ bản.

- RoomCharge, ServiceCharge, Discount và TaxRate phải hợp lệ; Discount không âm và không vượt tổng trước giảm giá.

- Hóa đơn phải lưu được trạng thái và nhân viên lập hóa đơn.

## 3. Quy trình xử lý

1.  Nhận dữ liệu tiền phòng từ quy trình checkout và tiền dịch vụ từ ServiceUsages.

2.  Hiển thị RoomCharge, ServiceCharge, Discount và TaxRate.

3.  Tính trước giảm giá = RoomCharge + ServiceCharge.

4.  Tiền sau giảm giá = RoomCharge + ServiceCharge − Discount.

5.  Thuế = tiền sau giảm giá × TaxRate / 100.

6.  TotalAmount = tiền sau giảm giá + thuế.

7.  Kiểm tra tổng tiền không âm; hiển thị khách hàng, booking, ngày lập và chi tiết.

8.  Xác nhận phát hành hóa đơn; mặc định Unpaid nếu chưa có khoản thanh toán được ghi nhận, PartiallyPaid nếu đã thanh toán một phần, Paid nếu đã đủ.

9.  Chỉ người có quyền mới được hủy/void hóa đơn; phải kiểm tra thanh toán đã có và lưu lý do.

## 4. Dữ liệu nghiệp vụ liên quan

| **Bảng/đối tượng** | **Thông tin sử dụng**                                                                                         |
|--------------------|---------------------------------------------------------------------------------------------------------------|
| Invoices           | InvoiceId, BookingId, EmployeeId, RoomCharge, ServiceCharge, Discount, TaxRate, TotalAmount, Status, IssuedAt |
| Bookings           | BookingId, CustomerId, Status                                                                                 |
| Payments           | InvoiceId/Booking liên quan, Amount, PaidAt                                                                   |

## 5. Checklist tự kiểm tra

- [ ] Hóa đơn cuối không lập trước khi checkout hoàn tất.

- [ ] TotalAmount khớp công thức.

- [ ] Discount không vượt tổng tiền trước giảm.

- [ ] Không tạo hai hóa đơn cuối cho một booking.

- [ ] Trạng thái hóa đơn phản ánh đúng tổng đã thanh toán.

## 6. Quy tắc thống nhất

- Tiền cọc đã thu trước checkout phải được đối trừ vào nghĩa vụ thanh toán của booking khi hóa đơn cuối được lập. Database sẽ được thiết kế sau để liên kết khoản cọc với booking và/hoặc hóa đơn; không tính cọc thành doanh thu lần thứ hai.

- Void không đồng nghĩa tự động hoàn tiền. Nếu có hoàn tiền, phải ghi nhận thành nghiệp vụ riêng và cập nhật số tiền thực thu; phạm vi cơ bản chỉ cho void theo quyền khi không gây sai lệch khoản tiền đã thu.

## 7. Đề xuất sửa và điều chỉnh database

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>Hóa đơn gộp theo booking, trái yêu cầu tính theo phòng.</p></li>
<li><p>Bước 9 "xác nhận phát hành" tách khỏi check-out trong khi không có trạng thái nháp; mục 11 lại tạo hóa đơn ngay trong transaction check-out.</p></li>
<li><p>Unpaid/PartiallyPaid mâu thuẫn với thu đủ khi trả phòng.</p></li>
<li><p>Void xong không lập lại được vì "mỗi booking chỉ một hóa đơn" (UNIQUE): booking đã trả phòng mất hóa đơn hợp lệ.</p></li>
<li><p>Mất các quyết định: ai được giảm giá, lưu lý do và người sửa; xuất hóa đơn VAT cho đơn vị.</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>Mỗi lượt lưu trú có tối đa <strong>một hóa đơn hiệu lực</strong> (Paid). Hóa đơn được lập và phát hành ngay trong transaction check-out; không có hóa đơn nháp.</p></li>
<li><p>Công thức theo phòng: RoomCharge = PricePerNight × Nights; ServiceCharge = tổng Amount các lượt dịch vụ của phòng (gồm phụ thu trễ); 0 ≤ Discount ≤ RoomCharge + ServiceCharge; TotalAmount = ROUND((RoomCharge + ServiceCharge − Discount) × (1 + TaxRate / 100), 0); TaxRate mặc định 10.</p></li>
<li><p><strong>Giảm giá:</strong> bắt buộc lý do; Receptionist tối đa 10% của (RoomCharge + ServiceCharge); trên 10% phải là Manager/Admin. Người lập hóa đơn được lưu.</p></li>
<li><p><strong>Hóa đơn cho đơn vị:</strong> nhập tên đơn vị, mã số thuế, địa chỉ người mua trên hóa đơn; có mã số thuế thì bắt buộc tên đơn vị.</p></li>
<li><p>Trạng thái: <strong>Paid</strong> hoặc <strong>Void</strong>.</p></li>
<li><p><strong>Void:</strong> A, M; lý do bắt buộc. Void không xóa và không tự hoàn tiền, vì tiền thuộc lượt lưu trú. Sau khi void được sửa dịch vụ/giảm giá rồi <strong>lập lại hóa đơn mới</strong> (tham chiếu hóa đơn bị thay); chênh lệch thu thêm hoặc hoàn ngay khi lập lại.</p></li>
<li><p>Bảng kê theo booking (tổng các hóa đơn phòng) chỉ để đối chiếu.</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>Invoices: BookingRoomId thay BookingId; thêm InvoiceNo, Nights, DiscountReason, BuyerName, BuyerCompanyName, BuyerTaxCode, BuyerAddress, IssuedBy, VoidedAt, VoidedBy, VoidReason, ReplacesInvoiceId; Status Paid/Void; TotalAmount là cột tính có ROUND.</p></li>
<li><p>UNIQUE có lọc: (BookingRoomId) WHERE Status = 'Paid' thay cho UNIQUE(BookingId).</p></li>
<li><p>Thủ tục sp_VoidInvoice, sp_ReissueInvoice; view vw_InvoiceDetail, vw_InvoiceLines, vw_BookingStatement.</p></li>
</ul></td>
</tr>
</tbody>
</table>

# 13. Thanh toán (Payments)

## 1. Mục đích

Ghi nhận các khoản tiền khách thực trả, hỗ trợ thanh toán nhiều lần và đối trừ tiền cọc.

## 2. Điều kiện thực hiện

- Thanh toán hóa đơn chỉ thực hiện khi hóa đơn tồn tại và chưa Void.

- Số tiền thanh toán mới phải lớn hơn 0 và không vượt số tiền còn phải thu.

- Phương thức: Cash, Card, BankTransfer, EWallet.

- Mọi khoản thu phải có thời điểm ghi nhận và được gắn với booking/hóa đơn để truy vết.

## 3. Quy trình xử lý

1.  Mở hóa đơn và xem tổng phải trả, các khoản đã thu, tiền cọc đã nhận và số dư còn phải thu.

2.  Chọn phương thức thanh toán, nhập số tiền và thông tin tham chiếu nếu có.

3.  Hệ thống tính số dư còn phải thu sau khi tính các khoản thanh toán hợp lệ và tiền cọc được đối trừ.

4.  Nếu số tiền nhập lớn hơn số dư, từ chối và hiển thị số tiền tối đa được phép thu.

5.  Lưu khoản thanh toán; cập nhật trạng thái hóa đơn: Unpaid nếu chưa thu khoản nào, PartiallyPaid nếu còn dư, Paid nếu đã đủ.

6.  Cho phép thanh toán nhiều lần đến khi đủ; mỗi lần thanh toán có bản ghi riêng.

7.  Trước khi có hóa đơn cuối, khoản đặt cọc vẫn phải được ghi nhận gắn với booking; sau checkout, đối trừ khoản cọc vào số phải thu. Không tạo khoản thu cọc lần thứ hai.

8.  Hiển thị biên nhận/thông tin giao dịch sau khi lưu.

## 4. Dữ liệu nghiệp vụ liên quan

| **Bảng/đối tượng** | **Thông tin sử dụng**                                                                              |
|--------------------|----------------------------------------------------------------------------------------------------|
| Payments           | PaymentId, InvoiceId hoặc liên kết booking theo thiết kế cuối, Amount, Method, ReferenceNo, PaidAt |
| Invoices           | InvoiceId, TotalAmount, Status                                                                     |
| Bookings           | BookingId, Deposit                                                                                 |

## 5. Checklist tự kiểm tra

- [ ] Không cho thanh toán hóa đơn Void.

- [ ] Không cho số tiền \<= 0 hoặc vượt số dư.

- [ ] Thanh toán nhiều lần cập nhật trạng thái đúng.

- [ ] Khoản cọc chỉ được tính một lần.

- [ ] Doanh thu ghi nhận dựa trên tiền thực thu, không dựa đơn thuần vào TotalAmount của hóa đơn.

## 6. Quy tắc thống nhất

- Định hướng nghiệp vụ: tiền cọc là khoản thu trước hóa đơn; khi phát hành hóa đơn cuối, đối trừ cọc vào tổng phải trả. Cấu trúc database sẽ được thiết kế sau để hỗ trợ liên kết khoản thu trước hóa đơn và khoản thu sau hóa đơn.

- Trong phạm vi cơ bản không tự động xử lý hoàn tiền; giao dịch đã ghi không được xóa tùy tiện.

## 7. Đề xuất sửa và điều chỉnh database

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>"Thanh toán chỉ khi hóa đơn tồn tại" trong khi cọc thu trước hóa đơn, và hóa đơn chỉ lập khi đã thu đủ → vòng lặp không giải được.</p></li>
<li><p>"Không vượt số dư" và "không hoàn tiền" không xử lý được khách cọc 100% rồi được giảm giá.</p></li>
<li><p>Doanh thu theo Payment chưa trừ tiền hoàn.</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>Mọi khoản tiền gắn với <strong>lượt lưu trú</strong> (BookingRoomId). InvoiceId được điền khi khoản đó được chốt vào hóa đơn lúc check-out.</p></li>
<li><p>Loại khoản: <strong>Deposit</strong> (phòng đang Booked), <strong>Payment</strong> (khi đang ở hoặc lúc check-out), <strong>Refund</strong> (hủy đủ điều kiện, dư khi check-out hoặc khi lập lại hóa đơn, điều chỉnh của A/M).</p></li>
<li><p>Amount luôn &gt; 0, dấu xác định bởi Type. <strong>Đã thu = Σ(Deposit + Payment) − Σ Refund.</strong></p></li>
<li><p>Phương thức: Cash, Card, BankTransfer, EWallet; ReferenceNo bắt buộc với BankTransfer và EWallet.</p></li>
<li><p>Không sửa, không xóa khoản đã ghi. Sai thì ghi bút toán ngược (Refund hoặc Payment) kèm ghi chú, do A/M thực hiện.</p></li>
<li><p>Không thu tiền cho lượt Cancelled; không thu thêm cho phòng đã CheckedOut trừ khi đang lập lại hóa đơn.</p></li>
<li><p>Doanh thu thực thu = Σ khoản thu có dấu theo PaidAt. Cọc được tính một lần lúc nhận, không chuyển đổi sang khoản khác nên không bị đếm hai lần.</p></li>
<li><p>Màn hình thanh toán mở theo phòng: hiển thị cọc yêu cầu, đã thu, ước tính còn phải trả.</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>Payments: BookingRoomId NOT NULL, InvoiceId NULL, Type, EmployeeId, Note, SignedAmount (cột tính); bỏ Bookings.Deposit (thay bằng tổng Deposit theo phòng).</p></li>
<li><p>Thủ tục sp_RecordPayment, sp_RecordBookingDeposit; view vw_RoomStayFolio, vw_PaymentLedger.</p></li>
</ul></td>
</tr>
</tbody>
</table>

# 14. Housekeeping (Dọn phòng)

## 1. Mục đích

Theo dõi và cập nhật trạng thái sẵn sàng của phòng sau khi khách trả.

## 2. Điều kiện thực hiện

- Housekeeping chỉ được xem danh sách phòng cần dọn và cập nhật kết quả dọn.

- Chỉ phòng Cleaning mới được chuyển sang Available sau khi hoàn tất kiểm tra.

- Không chuyển phòng Occupied hoặc Maintenance sang Available bằng chức năng dọn phòng.

## 3. Quy trình xử lý

1.  Nhân viên mở danh sách phòng trạng thái Cleaning.

2.  Chọn phòng và kiểm tra trạng thái hiện tại.

3.  Thực hiện dọn phòng ngoài hệ thống; sau khi hoàn tất, xác nhận trên màn hình.

4.  Hệ thống kiểm tra phòng vẫn ở Cleaning, sau đó chuyển sang Available.

5.  Nếu phát hiện hư hỏng cần bảo trì, chuyển yêu cầu cho người có quyền; phòng phải ở Maintenance thay vì Available.

6.  Làm mới danh sách và hiển thị kết quả.

## 4. Dữ liệu nghiệp vụ liên quan

| **Bảng/đối tượng**    | **Thông tin sử dụng**              |
|-----------------------|------------------------------------|
| Rooms                 | RoomId, RoomNumber, Status, Note   |
| Bookings/BookingRooms | Lịch sử lưu trú để tra cứu khi cần |

## 5. Checklist tự kiểm tra

- [ ] Chỉ phòng Cleaning được hoàn tất dọn.

- [ ] Phòng sau khi dọn chuyển Available.

- [ ] Phòng cần bảo trì không được chuyển Available.

- [ ] Housekeeping không truy cập được chức năng thanh toán/nhân viên.

## 6. Quy tắc thống nhất

- Phiên bản cơ bản chỉ quản lý trạng thái dọn phòng, không lưu lịch sử nhiệm vụ Housekeeping riêng. Nếu nhóm muốn lưu người dọn, thời gian bắt đầu/kết thúc hoặc ghi chú theo từng lần dọn thì sẽ xem xét bổ sung bảng sau khi chốt database.

## 7. Đề xuất sửa và điều chỉnh database

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>Không lưu ai dọn, dọn lúc nào; khi phát hiện hỏng không có nơi ghi nhận.</p></li>
<li><p>Phòng bảo trì xong không có việc dọn để Housekeeping xác nhận.</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>Mỗi lần phòng chuyển sang Cleaning (check-out hoặc kết thúc bảo trì), hệ thống tạo <strong>một việc dọn</strong> (Pending). Mỗi phòng tối đa một việc chưa xong.</p></li>
<li><p>Housekeeping nhận việc → InProgress (ghi người nhận, giờ bắt đầu) → hoàn tất với kết quả: <strong>Clean</strong> → phòng Available; <strong>NeedMaintenance</strong> (bắt buộc ghi chú) → phòng Maintenance.</p></li>
<li><p>Manager/Admin kết thúc bảo trì → phòng Cleaning và tạo việc dọn mới (nguồn AfterMaintenance).</p></li>
<li><p>Receptionist chỉ xem; không xóa lịch sử việc dọn.</p></li>
<li><p>Thay quy tắc "không lưu lịch sử nhiệm vụ Housekeeping" của bản gốc.</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>Bảng mới HousekeepingTasks (bảng thứ 14, vẫn trong giới hạn 10–14 bảng); thủ tục sp_StartHousekeepingTask, sp_CompleteHousekeepingTask, sp_FinishMaintenance; view vw_HousekeepingQueue.</p></li>
</ul></td>
</tr>
</tbody>
</table>

# 15. Dashboard, báo cáo, tìm kiếm và xử lý lỗi

## 1. Mục đích

Cung cấp số liệu vận hành và bảo đảm thao tác tìm kiếm, kiểm tra dữ liệu và thông báo lỗi nhất quán.

## 2. Điều kiện thực hiện

- Dashboard/báo cáo chỉ hiển thị dữ liệu mà người dùng có quyền xem.

- Khoảng lọc thời gian phải hợp lệ; ngày bắt đầu không lớn hơn ngày kết thúc.

- Doanh thu phải dựa trên khoản tiền thực thu trong kỳ theo PaidAt; không cộng toàn bộ hóa đơn chưa thanh toán.

- Tìm kiếm và lọc dùng truy vấn có tham số; không ghép chuỗi đầu vào trực tiếp vào SQL.

## 3. Quy trình xử lý

1.  Dashboard hiển thị tổng số phòng, số phòng theo trạng thái, booking theo trạng thái, lượt check-in/check-out và doanh thu thực thu theo khoảng thời gian.

2.  Cho phép lọc báo cáo theo ngày/tháng/năm hoặc khoảng ngày; mặc định khoảng thời gian dễ hiểu và hiển thị rõ.

3.  Báo cáo doanh thu cộng các khoản Payment có thời điểm PaidAt nằm trong kỳ; tiền cọc chỉ tính khi thực thu và không tính lại khi đối trừ vào hóa đơn.

4.  Ở từng màn hình, hỗ trợ tìm kiếm theo trường phù hợp, lọc theo trạng thái/ngày và sắp xếp theo cột nếu có.

5.  Trước khi lưu, kiểm tra trường bắt buộc, định dạng, giới hạn số/ngày và các quan hệ liên quan.

6.  Khi lỗi nghiệp vụ, hiển thị thông báo dễ hiểu; khi lỗi hệ thống/SQL, ghi nhận chi tiết kỹ thuật cho phát triển nhưng không lộ thông tin nhạy cảm cho người dùng.

7.  Khi thao tác nhiều bảng, dùng transaction; nếu thất bại thì rollback, không để dữ liệu ở trạng thái nửa chừng.

## 4. Dữ liệu nghiệp vụ liên quan

| **Bảng/đối tượng**           | **Thông tin sử dụng**                        |
|------------------------------|----------------------------------------------|
| Bookings                     | Status, CheckInDate, CheckOutDate, CreatedAt |
| Rooms                        | Status                                       |
| Invoices                     | TotalAmount, Status, IssuedAt                |
| Payments                     | Amount, PaidAt, Method                       |
| Customers/Employees/Services | Các trường tìm kiếm tương ứng                |

## 5. Checklist tự kiểm tra

- [ ] Lọc ngày hợp lệ.

- [ ] Doanh thu theo tiền thực thu, không phải tổng hóa đơn phát hành.

- [ ] Tìm kiếm không lỗi khi có dấu nháy hoặc ký tự đặc biệt.

- [ ] Thông báo lỗi rõ ràng, không làm mất dữ liệu đã nhập khi có thể giữ lại.

- [ ] Lỗi ghi dữ liệu nhiều bảng không để lại bản ghi dở dang.

## 6. Quy tắc thống nhất

- Chỉ số phòng phải phân biệt số phòng vật lý với số phòng đang có booking; không cộng booking như thể đó là số phòng.

- Không hiển thị mật khẩu hoặc PasswordHash trên bất kỳ màn hình/báo cáo nào.

## 7. Đề xuất sửa và điều chỉnh database

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Vấn đề / xung đột</strong></p>
<ul>
<li><p>Chỉ số "booking theo trạng thái" không còn đủ khi tính theo phòng.</p></li>
<li><p>Doanh thu chưa trừ tiền hoàn; công suất có thể chia cho 0; lọc "&lt;= ToDate" trên cột thời gian làm mất ngày cuối.</p></li>
<li><p>Tìm tên tiếng Việt không dấu không ra kết quả với collation mặc định; quyền xem báo cáo chưa chốt.</p></li>
</ul>
<p><strong>Quyết định đề xuất</strong></p>
<ul>
<li><p>Chỉ số trong ngày: phòng theo trạng thái; khách đến hôm nay (lượt Booked có CheckInDate = hôm nay); đang ở; dự kiến trả hôm nay; đã trả hôm nay (theo ActualCheckOut); booking mới tạo hôm nay; doanh thu thực thu hôm nay.</p></li>
<li><p>Công suất = số phòng đang ở / (số phòng IsActive − số phòng Maintenance) × 100; mẫu số bằng 0 thì hiển thị 0.</p></li>
<li><p>Báo cáo doanh thu: <strong>thực thu</strong> = Σ khoản thu có dấu theo PaidAt, chia theo phương thức và loại; <strong>doanh số theo hóa đơn</strong> (tiền phòng, dịch vụ, giảm giá, thuế) theo IssuedAt của hóa đơn Paid. Hai số ghi nhãn riêng, không cần bằng nhau.</p></li>
<li><p>Lọc khoảng ngày: PaidAt &gt;= FromDate AND PaidAt &lt; ToDate + 1 ngày.</p></li>
<li><p>Quyền: theo ma trận I.1 (Receptionist không xem doanh thu; Housekeeping chỉ xem hàng đợi dọn phòng).</p></li>
<li><p>Tìm kiếm tiếng Việt: so sánh với COLLATE Vietnamese_CI_AI, tham số kiểu NVARCHAR, thoát ký tự % _ [ trong LIKE.</p></li>
</ul>
<p><strong>Điều chỉnh database</strong></p>
<ul>
<li><p>View vw_RoomBoard, vw_TodayArrivals, vw_InHouseStays, vw_TodayDepartures, vw_PaymentLedger, vw_ServiceUsageStats; thủ tục sp_DashboardSummary, sp_RevenueReport; index Payments(PaidAt).</p></li>
</ul></td>
</tr>
</tbody>
</table>

# II. CÁC QUY TẮC LIÊN NGHIỆP VỤ BẮT BUỘC

| **Cặp nghiệp vụ**     | **Quy tắc thống nhất**                                                                                                                                               |
|-----------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Booking và Room       | Booking kiểm tra cả Room.Status lẫn các booking đang giữ lịch. Room.Status không đủ để kết luận phòng trống trong tương lai.                                         |
| Booking và giá        | Giá mỗi phòng được chốt vào BookingRooms.PricePerNight; không tính lại theo BasePrice hiện tại khi checkout.                                                         |
| Check-in và dịch vụ   | Chỉ ghi dịch vụ cho phòng thuộc booking đang lưu trú; dịch vụ gắn với đúng BookingRoom.                                                                              |
| Check-out và hóa đơn  | Checkout xác nhận tiền phòng/dịch vụ và chuyển phòng sang Cleaning; hóa đơn cuối được tạo một lần cho booking sau khi toàn bộ phòng đã checkout.                     |
| Hóa đơn và thanh toán | Invoice.TotalAmount là số phải trả sau giảm giá và thuế; Payments ghi tiền thực thu. Trạng thái hóa đơn được xác định từ tổng thanh toán hợp lệ sau khi đối trừ cọc. |
| Tiền cọc              | Deposit/tiền cọc là khoản thu trước hóa đơn, không phải doanh thu mới khi đối trừ vào hóa đơn. Phải tránh ghi nhận hai lần.                                          |
| Housekeeping và phòng | Chỉ Housekeeping hoàn tất dọn mới chuyển Cleaning → Available. Phòng cần sửa chữa phải ở Maintenance.                                                                |
| Nhân viên và lịch sử  | Không xóa cứng nhân viên/khách/phòng/dịch vụ đã tham gia giao dịch lịch sử; khóa hoặc ngừng sử dụng nếu cần.                                                         |
| Transaction           | Tạo/sửa booking cùng các phòng; check-in; checkout cùng cập nhật phòng và hóa đơn; thanh toán cùng cập nhật trạng thái hóa đơn phải có transaction phù hợp.          |
| Phân quyền            | Kiểm tra quyền ở tầng xử lý nghiệp vụ, không chỉ ẩn nút hoặc menu.                                                                                                   |

## Đề xuất sửa – II. Quy tắc liên nghiệp vụ

Thay bảng phía trên bằng bảng sau (các dòng đổi theo mô hình tính theo phòng):

| **Cặp nghiệp vụ**     | **Quy tắc (bản sửa)**                                                                                                                                     |
|-----------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------|
| Booking và Room       | Lịch phòng lấy từ BookingRooms (Booked, CheckedIn). Room.Status chỉ cho biết hiện tại; phòng Maintenance hoặc IsActive = 0 không đặt mới.                 |
| Booking và giá        | PricePerNight chốt theo từng phòng lúc đặt. Đổi phòng do khách sạn: giữ giá cũ. Không tính lại theo BasePrice khi check-out.                              |
| Booking và cọc        | Cọc yêu cầu theo phòng; Booking Confirmed khi mọi phòng đủ cọc; Pending chưa từng xác nhận hết hạn sau 24 giờ.                                            |
| Check-in và dịch vụ   | Dịch vụ chỉ ghi cho lượt lưu trú đang CheckedIn; khóa sau khi phòng có hóa đơn Paid.                                                                      |
| Check-out và hóa đơn  | Mỗi phòng check-out lập một hóa đơn của phòng đó, thu đủ hoặc hoàn dư trong cùng transaction.                                                             |
| Hóa đơn và thanh toán | Tiền gắn lượt lưu trú; hóa đơn chỉ Paid/Void; void không làm thay đổi tiền đã thu.                                                                        |
| Tiền cọc              | Ghi một lần vào Payments (Deposit); không chuyển đổi, không đếm hai lần trong doanh thu.                                                                  |
| Housekeeping và phòng | Chỉ hoàn tất việc dọn mới chuyển Cleaning → Available; bảo trì xong đi Maintenance → Cleaning → Available.                                                |
| Transaction           | Bắt buộc với: tạo/sửa/hủy booking, thu cọc chia theo phòng, check-in, gia hạn, check-out, void và lập lại hóa đơn, hoàn tất việc dọn, đưa vào/ra bảo trì. |
| Phân quyền            | Theo ma trận I.1, kiểm tra ở tầng nghiệp vụ và trong stored procedure.                                                                                    |

# III. NHỮNG ĐIỂM SẼ THIẾT KẾ DATABASE SAU

Đây là các yêu cầu nghiệp vụ đã chốt để nhóm dùng khi thiết kế lại database; chưa phải kết luận rằng schema hiện tại đã đáp ứng:

- Cách ghi nhận tiền cọc trước khi có hóa đơn cuối và liên kết khoản cọc với booking/hóa đơn để đối trừ đúng một lần.

- Lưu dấu vết nhân viên thực hiện các thao tác quan trọng nếu nhóm cần truy vết: check-in, check-out, ghi nhận dịch vụ, thanh toán, hoàn tất dọn phòng.

- Cơ chế lưu lý do và người thực hiện khi void hóa đơn hoặc điều chỉnh giao dịch.

- Ràng buộc dữ liệu và chỉ mục hỗ trợ kiểm tra trùng lịch booking.

- Nếu muốn lưu lịch sử dọn phòng, cân nhắc bảng nhiệm vụ/lịch sử Housekeeping; bản nghiệp vụ cơ bản hiện chưa bắt buộc.

- Bảo đảm cách lưu tiền tệ, tỷ lệ thuế, giảm giá, thời gian thực tế và trạng thái nhất quán giữa các bảng.

## Đề xuất sửa – III. Những điểm thiết kế database

Các điểm "sẽ thiết kế sau" đã có lời giải trong mục VII:

- Ghi nhận cọc và đối trừ một lần → Payments theo BookingRoomId, Type = Deposit (VII.4).

- Lưu người thực hiện → các cột CreatedBy, CheckInBy, CheckOutBy, IssuedBy, VoidedBy, EmployeeId, AssignedTo.

- Lý do và người void → Invoices.VoidedAt, VoidedBy, VoidReason, ReplacesInvoiceId.

- Chống trùng lịch → fn_AvailableRooms, index lọc trên BookingRooms, khóa UPDLOCK/HOLDLOCK trong sp_CreateBooking và sp_UpdateBookingRoom.

- Lịch sử dọn phòng → bảng HousekeepingTasks.

- Tiền tệ, thuế, giảm giá, trạng thái nhất quán → DECIMAL(14,0), cột tính có ROUND, CHECK theo bảng trạng thái I.2.

# IV. KỊCH BẢN KIỂM THỬ XUYÊN SUỐT

- [ ] Đăng nhập lần lượt bằng từng Role và xác nhận menu/chức năng đúng quyền.

- [ ] Tạo khách hàng, loại phòng, phòng và tiện nghi; kiểm tra dữ liệu trùng và dữ liệu không hợp lệ.

- [ ] Tạo booking một phòng; xác nhận booking; thử tạo booking khác trùng lịch và xác nhận hệ thống từ chối.

- [ ] Tạo booking nhiều phòng; kiểm tra sức chứa và giá được chốt cho từng phòng.

- [ ] Check-in booking Confirmed; xác nhận Booking = CheckedIn và các phòng = Occupied.

- [ ] Ghi nhận một hoặc nhiều dịch vụ; kiểm tra số lượng × đơn giá và lịch sử sử dụng.

- [ ] Checkout; kiểm tra số đêm, tiền phòng, tiền dịch vụ, giảm giá, thuế, hóa đơn và trạng thái phòng Cleaning.

- [ ] Ghi nhận thanh toán một lần và nhiều lần; kiểm tra số dư, trạng thái Unpaid/PartiallyPaid/Paid và không cho thu vượt.

- [ ] Đối trừ tiền cọc đã nhận trước đó; bảo đảm không ghi nhận cọc hai lần.

- [ ] Housekeeping hoàn tất dọn; phòng chuyển Cleaning → Available.

- [ ] Lọc dashboard theo thời gian; đối chiếu doanh thu theo Payments.PaidAt.

- [ ] Thử lỗi ở từng bước ghi nhiều bảng; bảo đảm rollback và không tạo dữ liệu nửa chừng.

## Đề xuất sửa – IV. Bổ sung kịch bản kiểm thử

- [ ] Booking 2 phòng, đóng cọc một lần: kiểm tra cọc được chia theo phòng và Booking chuyển Confirmed.

- [ ] Booking Pending quá 24 giờ chưa đủ cọc tự hủy; Booking đã từng Confirmed thì không tự hủy.

- [ ] Check-in phòng A, phòng B đến ngày hôm sau; ghi dịch vụ riêng từng phòng, không lẫn.

- [ ] Phòng đã đặt bị Maintenance: đổi sang phòng hạng cao hơn, PricePerNight giữ nguyên.

- [ ] Gia hạn phòng A thêm 1 đêm; bị chặn khi đêm thêm trùng lịch khách khác.

- [ ] Check-out trễ 25 phút (0%), 45 phút (5%), 2 giờ 10 phút (30%), sau 18:00 (bắt buộc gia hạn).

- [ ] Check-out thiếu tiền bị chặn; cọc 100% rồi giảm giá → hệ thống ghi Refund phần dư.

- [ ] Phòng A trả và có hóa đơn riêng trong khi phòng B vẫn đang ở; Booking chỉ CheckedOut khi phòng B trả.

- [ ] Receptionist giảm giá trên 10% bị chặn; hóa đơn có mã số thuế mà thiếu tên đơn vị bị chặn.

- [ ] Void hóa đơn → sửa dịch vụ → lập lại; chênh lệch được thu hoặc hoàn; doanh thu chỉ đổi đúng phần chênh lệch.

- [ ] Hủy trước 48 giờ hoàn cọc; hủy dưới 48 giờ và no-show giữ cọc.

- [ ] Housekeeping báo hỏng → Maintenance → kết thúc bảo trì → việc dọn mới → Available.

- [ ] Doanh thu ngày = Σ thu − Σ hoàn theo PaidAt; lọc đến ToDate vẫn tính đủ ngày cuối.

# V. PHẠM VI CHƯA TRIỂN KHAI TRONG BẢN CƠ BẢN

- Khách tự đặt phòng trực tuyến, cổng thanh toán trực tuyến và tích hợp bên thứ ba.

- Tách nhiều khách lưu trú trong cùng booking bằng bảng khách đi kèm riêng.

- Check-in/check-out từng phòng ở các thời điểm khác nhau trong cùng booking.

- Tự động tính phụ thu trả muộn theo nhiều mức nếu chưa có chính sách thống nhất.

- Quản lý hoàn tiền phức tạp, nhiều loại giao dịch điều chỉnh và kiểm toán kế toán chuyên sâu.

- Quản lý kho/định lượng vật tư tiện nghi hoặc lịch sử bảo trì chi tiết.

- Phân quyền chi tiết theo từng nút/thao tác bằng bảng Permissions riêng.

## Đề xuất sửa – V. Phạm vi

<table>
<colgroup>
<col style="width: 100%" />
</colgroup>
<tbody>
<tr class="odd">
<td><p><strong>ĐỀ XUẤT SỬA</strong></p>
<p><strong>Bỏ khỏi danh sách "chưa triển khai"</strong></p>
<ul>
<li><p>"Check-in/check-out từng phòng ở các thời điểm khác nhau" — nay là nghiệp vụ chính.</p></li>
<li><p>"Tự động tính phụ thu trả muộn" — đã có chính sách (mục 11).</p></li>
</ul>
<p><strong>Thêm vào danh sách "chưa triển khai"</strong></p>
<ul>
<li><p>Công nợ: cho khách rời khách sạn khi chưa thanh toán đủ.</p></li>
<li><p>Đổi phòng khi khách đang ở.</p></li>
<li><p>Hóa đơn gộp nhiều phòng; hóa đơn điện tử kết nối cơ quan thuế.</p></li>
<li><p>Cấu hình chính sách (giờ chuẩn, tỷ lệ cọc, phụ thu) trên giao diện; hiện dùng hằng số (VII.10).</p></li>
<li><p>Tác vụ chạy định kỳ bằng SQL Agent (SQL Server Express không có); việc hết hạn Pending chạy khi mở ứng dụng.</p></li>
</ul></td>
</tr>
</tbody>
</table>

# VI. TỔNG HỢP RÀ SOÁT

## 1. Xung đột đã phát hiện và cách xử lý

| **\#** | **Mâu thuẫn**                                                                                           | **Mục**       | **Cách xử lý**                                                           |
|--------|---------------------------------------------------------------------------------------------------------|---------------|--------------------------------------------------------------------------|
| 1      | Manager có được quản lý nhân viên không                                                                 | I.1, 3        | Chỉ Admin; sửa câu mô tả Manager.                                        |
| 2      | Admin được thêm vai trò mới nhưng quyền cố định trong code theo 4 vai trò                               | 1, 2          | 4 vai trò cố định, không thêm/xóa/đổi tên.                               |
| 3      | Maintenance → Available trực tiếp, trái quy định phải qua dọn phòng                                     | 6, 14, II     | Maintenance → Cleaning → Available.                                      |
| 4      | Room.Reserved không có quy trình bật/tắt, không biểu diễn được lịch tương lai                           | I.2, 6        | Bỏ Reserved; lịch lấy từ BookingRooms.                                   |
| 5      | Pending giữ lịch vô thời hạn; không có no-show                                                          | 8             | Hết hạn 24 giờ; no-show sau 12:00 ngày hôm sau.                          |
| 6      | "Chính sách cọc đã công bố" nhưng chưa có chính sách                                                    | 8             | Hủy ≥ 48 giờ hoàn 100%, dưới 48 giờ không hoàn.                          |
| 7      | Nhận sớm cần duyệt (mục 9) khác quyết định cũ; nhận trước ngày CheckInDate là ở thêm đêm không trả tiền | 9, I.3        | Nhận sớm miễn phí chỉ trong ngày CheckInDate; trước đó phải sửa booking. |
| 8      | Không gia hạn được khi khách đang ở (chỉ sửa lượt Booked)                                               | 8, 11         | Thêm nghiệp vụ gia hạn theo phòng.                                       |
| 9      | Cho chỉnh dịch vụ sau khi phát hành hóa đơn, trái quyết định khóa cứng                                  | 10, 12        | Khóa cứng; sửa qua void và lập lại.                                      |
| 10     | Hóa đơn tạo trong check-out nhưng lại có bước "xác nhận phát hành" riêng, không có trạng thái nháp      | 11, 12        | Lập và phát hành ngay trong check-out.                                   |
| 11     | Cho void nhưng "mỗi booking một hóa đơn" nên không lập lại được                                         | 12            | Lập lại hóa đơn; UNIQUE có lọc theo hóa đơn Paid.                        |
| 12     | "Thanh toán khi hóa đơn tồn tại" trong khi cọc thu trước hóa đơn và hóa đơn cần thu đủ                  | 8, 11, 13     | Tiền gắn lượt lưu trú; hóa đơn là chứng từ.                              |
| 13     | Cho check-out khi chưa thu đủ, khác quyết định "thu đủ 100% mới cho trả phòng"                          | 11, 12        | Thu đủ trong transaction check-out; hóa đơn Paid/Void.                   |
| 14     | "Không vượt số dư" + "không hoàn tiền" không xử lý được cọc 100% rồi giảm giá                           | 13            | Hoàn phần dư khi check-out.                                              |
| 15     | Check-in/out, dịch vụ, hóa đơn gộp theo booking, trái yêu cầu tính theo từng phòng                      | I.4, 9–12, II | Toàn bộ chuyển về cấp lượt lưu trú phòng.                                |
| 16     | Sức chứa kiểm tra theo tổng các phòng                                                                   | 5, 8          | Kiểm tra theo từng phòng.                                                |
| 17     | BasePrice \> 0 trong tài liệu, DB cũ cho ≥ 0                                                            | 5             | CHECK BasePrice \> 0.                                                    |
| 18     | Cấm xóa tiện nghi đang gán, DB cũ ON DELETE CASCADE                                                     | 7             | Bỏ cascade.                                                              |
| 19     | Không có mục quản lý danh mục dịch vụ, trong khi mục 10 dựa vào nó                                      | —             | Thêm mục 7A.                                                             |
| 20     | Thiếu tiểu mục số 4; số bước đánh liên tục qua các mục                                                  | Toàn tài liệu | Đánh lại số tiểu mục và số bước.                                         |

## 2. Quyết định đã chốt được đưa lại

| **Quyết định**                                  | **Áp dụng ở** | **Ghi chú**                                                                     |
|-------------------------------------------------|---------------|---------------------------------------------------------------------------------|
| Phụ thu trả phòng trễ                           | 11            | Chính sách mới: sau 30 phút, mỗi 30 phút 5%; từ 2 giờ, mỗi giờ 15% giá một đêm. |
| Cọc 50% hoặc 100%, thiếu cọc không cho check-in | 8, 9, 13      | Cọc tính và ghi theo từng phòng.                                                |
| Phòng đặt bị hỏng: nâng hạng nhưng giữ giá cũ   | 8, 9          | Chỉ khi khách sạn đổi phòng; ghi lý do.                                         |
| Xuất hóa đơn VAT cho đơn vị                     | 12            | Thông tin người mua lưu trên hóa đơn thay vì Customers.                         |
| Giảm giá: người sửa và lý do                    | 12            | Thêm hạn mức 10% cho Receptionist.                                              |
| Cảnh báo khi số lượng dịch vụ \> 20             | 10            | Hộp xác nhận, không chặn.                                                       |
| Khóa cứng dịch vụ sau khi có hóa đơn            | 10            | Sửa qua void và lập lại.                                                        |
| Trả phòng sớm không hoàn tiền                   | 11            |                                                                                 |
| Nhận phòng sớm không phụ thu                    | 9             | Giới hạn trong ngày CheckInDate.                                                |
| Thu đủ 100% mới cho trả phòng                   | 11            | Phần dư hoàn ngay.                                                              |

## 3. Giả định cần nhóm xác nhận

- Cách hiểu phụ thu: không tính 30 phút đầu; từ phút 31 đến hết phút 120 tính mỗi 30 phút 5%; từ phút 121 mỗi giờ 15%; khoảng lẻ làm tròn lên.

- Sau 18:00 bắt buộc gia hạn thêm 1 đêm thay vì tính phụ thu theo giờ (để phụ thu không vượt tiền một đêm).

- Hủy ≥ 48 giờ hoàn 100% cọc; Pending hết hạn sau 24 giờ; mốc no-show 12:00 ngày sau CheckInDate.

- Receptionist giảm giá tối đa 10%; VAT mặc định 10%; 4 phương thức thanh toán (Cash, Card, BankTransfer, EWallet).

# VII. ĐỀ XUẤT DATABASE (LÀM LẠI TOÀN BỘ)

## 1. Nguyên tắc thiết kế

- Đơn vị nghiệp vụ là **lượt lưu trú phòng (BookingRooms)**: dịch vụ, hóa đơn, khoản thu, việc dọn phòng đều tham chiếu BookingRoomId.

- Đúng **14 bảng** (giới hạn đề bài 10–14). Không thêm bảng Settings: các hằng số chính sách đặt trong stored procedure/function và lớp Constants.cs (VII.10).

- Tiền VND: DECIMAL(14,0). Giá trị suy ra (số đêm, thành tiền, tổng hóa đơn, số tiền có dấu) là cột tính PERSISTED để không lệch.

- Nghiệp vụ nhiều bước nằm trong stored procedure có transaction (SET XACT_ABORT ON, TRY/CATCH, THROW mã lỗi 50xxx); CRUD danh mục đơn giản viết SQL có tham số ở tầng DAL.

- Không xóa cứng dữ liệu đã giao dịch: dùng IsActive, Status, Void, bút toán ngược.

- Không dùng trigger: mọi thay đổi trạng thái đi qua procedure để dễ đọc và dễ kiểm thử.

## 2. Danh sách bảng

| **\#** | **Bảng**          | **Vai trò**                           | **So với DB cũ**                                            |
|--------|-------------------|---------------------------------------|-------------------------------------------------------------|
| 1      | Roles             | 4 vai trò cố định                     | Thêm CHECK tên vai trò                                      |
| 2      | Employees         | Nhân viên, tài khoản đăng nhập        | Thêm CreatedAt                                              |
| 3      | Customers         | Người đặt phòng                       | Thêm CHECK ngày sinh                                        |
| 4      | RoomTypes         | Loại phòng, giá cơ bản, sức chứa      | Thêm IsActive; BasePrice \> 0                               |
| 5      | Rooms             | Phòng vật lý, trạng thái hiện tại     | Bỏ Reserved; thêm IsActive                                  |
| 6      | Amenities         | Tiện nghi                             | Giữ nguyên                                                  |
| 7      | RoomTypeAmenities | Tiện nghi theo loại phòng (N-N)       | Bỏ ON DELETE CASCADE                                        |
| 8      | Bookings          | Phiếu đặt của người đặt               | Bỏ ngày, số khách, Deposit; thêm DepositRate, mốc thời gian |
| 9      | BookingRooms      | Lượt lưu trú phòng (trung tâm)        | Thêm ngày, số khách, Status, người ở, người check-in/out    |
| 10     | Services          | Danh mục dịch vụ                      | Thêm IsSystem                                               |
| 11     | ServiceUsages     | Dịch vụ dùng theo phòng               | Thêm Amount, Note, CreatedBy                                |
| 12     | Invoices          | Hóa đơn của từng lượt lưu trú         | Theo phòng; Paid/Void; người mua; void và lập lại           |
| 13     | Payments          | Cọc, thanh toán, hoàn tiền theo phòng | Gắn BookingRoomId; thêm Type, EmployeeId                    |
| 14     | HousekeepingTasks | Việc dọn phòng                        | Bảng mới                                                    |

## 3. Quan hệ chính

- Roles 1–N Employees; Customers 1–N Bookings; Bookings 1–N BookingRooms; Rooms 1–N BookingRooms; RoomTypes 1–N Rooms.

- RoomTypes N–N Amenities qua RoomTypeAmenities.

- BookingRooms 1–N ServiceUsages (N–1 Services); BookingRooms 1–N Invoices (tối đa 1 Paid); BookingRooms 1–N Payments; Invoices 1–N Payments (tùy chọn).

- Rooms 1–N HousekeepingTasks; BookingRooms 1–N HousekeepingTasks (tùy chọn).

- Employees được tham chiếu bởi: Bookings.CreatedBy, BookingRooms.CheckInBy/CheckOutBy, ServiceUsages.CreatedBy, Invoices.IssuedBy/VoidedBy, Payments.EmployeeId, HousekeepingTasks.AssignedTo.

## 4. Chi tiết từng bảng

### 4.1 Roles

| **Cột**     | **Kiểu**      | **Ràng buộc**                                                           | **Ý nghĩa**           |
|-------------|---------------|-------------------------------------------------------------------------|-----------------------|
| RoleId      | INT IDENTITY  | PK                                                                      |                       |
| RoleName    | NVARCHAR(50)  | NOT NULL, UNIQUE, CHECK IN (Admin, Manager, Receptionist, Housekeeping) | Tên vai trò cố định   |
| Description | NVARCHAR(200) | NULL                                                                    | Mô tả, Admin được sửa |

### 4.2 Employees

| **Cột**      | **Kiểu**                  | **Ràng buộc**                   | **Ý nghĩa**        |
|--------------|---------------------------|---------------------------------|--------------------|
| EmployeeId   | INT IDENTITY              | PK                              |                    |
| RoleId       | INT                       | NOT NULL, FK → Roles            |                    |
| FullName     | NVARCHAR(100)             | NOT NULL                        |                    |
| Username     | VARCHAR(50)               | NOT NULL, UNIQUE                |                    |
| PasswordHash | VARCHAR(255)              | NOT NULL                        | BCrypt, băm ở C#   |
| Phone, Email | VARCHAR(20), VARCHAR(100) | NULL                            |                    |
| HireDate     | DATE                      | NOT NULL, DEFAULT hôm nay       |                    |
| IsActive     | BIT                       | NOT NULL, DEFAULT 1             | Khóa tài khoản = 0 |
| CreatedAt    | DATETIME2                 | NOT NULL, DEFAULT SYSDATETIME() |                    |

### 4.3 Customers

| **Cột**     | **Kiểu**      | **Ràng buộc**                  | **Ý nghĩa**   |
|-------------|---------------|--------------------------------|---------------|
| CustomerId  | INT IDENTITY  | PK                             |               |
| FullName    | NVARCHAR(100) | NOT NULL                       |               |
| Gender      | NVARCHAR(10)  | NULL, CHECK IN (Nam, Nữ, Khác) |               |
| DateOfBirth | DATE          | NULL, CHECK \<= hôm nay        |               |
| IdNumber    | VARCHAR(20)   | NOT NULL, UNIQUE               | CCCD/hộ chiếu |
| Phone       | VARCHAR(20)   | NOT NULL                       |               |
| Email       | VARCHAR(100)  | NULL                           |               |
| Address     | NVARCHAR(255) | NULL                           |               |
| Nationality | NVARCHAR(50)  | NOT NULL, DEFAULT Việt Nam     |               |
| CreatedAt   | DATETIME2     | NOT NULL, DEFAULT              |               |

### 4.4 RoomTypes

| **Cột**      | **Kiểu**      | **Ràng buộc**        | **Ý nghĩa**        |
|--------------|---------------|----------------------|--------------------|
| RoomTypeId   | INT IDENTITY  | PK                   |                    |
| TypeName     | NVARCHAR(50)  | NOT NULL, UNIQUE     |                    |
| Description  | NVARCHAR(500) | NULL                 |                    |
| BasePrice    | DECIMAL(14,0) | NOT NULL, CHECK \> 0 | Giá/đêm hiện hành  |
| MaxOccupancy | TINYINT       | NOT NULL, CHECK 1–10 | Sức chứa một phòng |
| BedType      | NVARCHAR(50)  | NULL                 |                    |
| IsActive     | BIT           | NOT NULL, DEFAULT 1  | Ngừng sử dụng = 0  |

### 4.5 Rooms

| **Cột**    | **Kiểu**      | **Ràng buộc**                                                                      | **Ý nghĩa**         |
|------------|---------------|------------------------------------------------------------------------------------|---------------------|
| RoomId     | INT IDENTITY  | PK                                                                                 |                     |
| RoomTypeId | INT           | NOT NULL, FK → RoomTypes                                                           |                     |
| RoomNumber | VARCHAR(10)   | NOT NULL, UNIQUE                                                                   |                     |
| Floor      | SMALLINT      | NOT NULL                                                                           |                     |
| Status     | VARCHAR(20)   | NOT NULL, DEFAULT Available, CHECK IN (Available, Occupied, Cleaning, Maintenance) | Trạng thái hiện tại |
| Note       | NVARCHAR(200) | NULL                                                                               | Ghi chú tình trạng  |
| IsActive   | BIT           | NOT NULL, DEFAULT 1                                                                | Ngừng khai thác = 0 |

### 4.6 Amenities và 4.7 RoomTypeAmenities

| **Cột**                      | **Kiểu**      | **Ràng buộc**                           | **Ý nghĩa**   |
|------------------------------|---------------|-----------------------------------------|---------------|
| Amenities.AmenityId          | INT IDENTITY  | PK                                      |               |
| Amenities.AmenityName        | NVARCHAR(100) | NOT NULL, UNIQUE                        |               |
| Amenities.Description        | NVARCHAR(200) | NULL                                    |               |
| RoomTypeAmenities.RoomTypeId | INT           | PK(RoomTypeId, AmenityId), FK NO ACTION |               |
| RoomTypeAmenities.AmenityId  | INT           | FK → Amenities, NO ACTION               | Không cascade |

### 4.8 Bookings

| **Cột**                   | **Kiểu**                 | **Ràng buộc**                                                                              | **Ý nghĩa**             |
|---------------------------|--------------------------|--------------------------------------------------------------------------------------------|-------------------------|
| BookingId                 | INT IDENTITY             | PK                                                                                         | Mã phiếu đặt            |
| CustomerId                | INT                      | NOT NULL, FK → Customers                                                                   | Người đặt               |
| DepositRate               | TINYINT                  | NOT NULL, CHECK IN (50, 100)                                                               | Tỷ lệ cọc               |
| Status                    | VARCHAR(20)              | NOT NULL, DEFAULT Pending, CHECK IN (Pending, Confirmed, CheckedIn, CheckedOut, Cancelled) | Do hệ thống tính        |
| Note                      | NVARCHAR(500)            | NULL                                                                                       |                         |
| CreatedBy                 | INT                      | NOT NULL, FK → Employees                                                                   |                         |
| CreatedAt                 | DATETIME2                | NOT NULL, DEFAULT                                                                          | Mốc tính hết hạn 24 giờ |
| ConfirmedAt               | DATETIME2                | NULL                                                                                       | Lần đầu đủ cọc          |
| CancelledAt, CancelReason | DATETIME2, NVARCHAR(200) | NULL                                                                                       |                         |

### 4.9 BookingRooms (lượt lưu trú phòng)

| **Cột**                       | **Kiểu**                   | **Ràng buộc**                                                                 | **Ý nghĩa**                        |
|-------------------------------|----------------------------|-------------------------------------------------------------------------------|------------------------------------|
| BookingRoomId                 | INT IDENTITY               | PK                                                                            |                                    |
| BookingId                     | INT                        | NOT NULL, FK → Bookings                                                       |                                    |
| RoomId                        | INT                        | NOT NULL, FK → Rooms                                                          |                                    |
| CheckInDate, CheckOutDate     | DATE                       | NOT NULL, CHECK CheckOutDate \> CheckInDate                                   | Ngày theo kế hoạch (gồm gia hạn)   |
| Nights                        | cột tính                   | DATEDIFF(DAY, CheckInDate, CheckOutDate) PERSISTED                            | Số đêm tính tiền                   |
| Adults, Children              | TINYINT                    | Adults ≥ 1, Children ≥ 0                                                      | Sức chứa kiểm tra trong procedure  |
| PricePerNight                 | DECIMAL(14,0)              | NOT NULL, CHECK \> 0                                                          | Giá chốt lúc đặt                   |
| Status                        | VARCHAR(20)                | NOT NULL, DEFAULT Booked, CHECK IN (Booked, CheckedIn, CheckedOut, Cancelled) |                                    |
| GuestName, GuestIdNumber      | NVARCHAR(100), VARCHAR(20) | NULL                                                                          | Người ở thực tế nếu khác người đặt |
| ActualCheckIn, ActualCheckOut | DATETIME2                  | NULL; CHECK nhất quán với Status; ActualCheckOut \> ActualCheckIn             |                                    |
| CheckInBy, CheckOutBy         | INT                        | NULL, FK → Employees                                                          |                                    |
| CancelledAt, CancelReason     | DATETIME2, NVARCHAR(200)   | NULL                                                                          | Gồm lý do NoShow                   |
| Note                          | NVARCHAR(500)              | NULL                                                                          | Ghi đổi phòng, gia hạn             |

### 4.10 Services

| **Cột**        | **Kiểu**                   | **Ràng buộc**       | **Ý nghĩa**                           |
|----------------|----------------------------|---------------------|---------------------------------------|
| ServiceId      | INT IDENTITY               | PK                  |                                       |
| ServiceName    | NVARCHAR(100)              | NOT NULL, UNIQUE    |                                       |
| Category, Unit | NVARCHAR(50), NVARCHAR(20) | Unit NOT NULL       |                                       |
| Price          | DECIMAL(14,0)              | NOT NULL, CHECK ≥ 0 |                                       |
| IsActive       | BIT                        | NOT NULL, DEFAULT 1 |                                       |
| IsSystem       | BIT                        | NOT NULL, DEFAULT 0 | Dịch vụ do hệ thống ghi (phụ thu trễ) |

### 4.11 ServiceUsages

| **Cột**              | **Kiểu**       | **Ràng buộc**                  | **Ý nghĩa**            |
|----------------------|----------------|--------------------------------|------------------------|
| UsageId              | INT IDENTITY   | PK                             |                        |
| BookingRoomId        | INT            | NOT NULL, FK → BookingRooms    |                        |
| ServiceId            | INT            | NOT NULL, FK → Services        |                        |
| Quantity             | INT            | NOT NULL, CHECK \> 0           |                        |
| UnitPrice            | DECIMAL(14,0)  | NOT NULL, CHECK ≥ 0            | Giá tại thời điểm dùng |
| Amount               | cột tính       | Quantity × UnitPrice PERSISTED |                        |
| UsedAt               | DATETIME2      | NOT NULL                       |                        |
| Note                 | NVARCHAR(200)  | NULL                           | Ví dụ: số phút trễ     |
| CreatedBy, CreatedAt | INT, DATETIME2 | NOT NULL; FK → Employees       |                        |

### 4.12 Invoices

| **Cột**                                                 | **Kiểu**                      | **Ràng buộc**                                                                   | **Ý nghĩa**                                |
|---------------------------------------------------------|-------------------------------|---------------------------------------------------------------------------------|--------------------------------------------|
| InvoiceId                                               | INT IDENTITY                  | PK                                                                              |                                            |
| InvoiceNo                                               | VARCHAR(20)                   | NOT NULL, UNIQUE                                                                | Ví dụ HD-2026-000123, sinh trong procedure |
| BookingRoomId                                           | INT                           | NOT NULL, FK → BookingRooms                                                     |                                            |
| Nights                                                  | SMALLINT                      | NOT NULL                                                                        | Chốt tại thời điểm lập                     |
| RoomCharge, ServiceCharge                               | DECIMAL(14,0)                 | NOT NULL, ≥ 0                                                                   |                                            |
| Discount                                                | DECIMAL(14,0)                 | NOT NULL, DEFAULT 0, CHECK 0 ≤ Discount ≤ RoomCharge + ServiceCharge            |                                            |
| DiscountReason                                          | NVARCHAR(200)                 | CHECK Discount = 0 OR DiscountReason IS NOT NULL                                |                                            |
| TaxRate                                                 | DECIMAL(5,2)                  | NOT NULL, DEFAULT 10, CHECK 0–100                                               |                                            |
| TotalAmount                                             | cột tính                      | ROUND((RoomCharge + ServiceCharge − Discount) × (1 + TaxRate/100), 0) PERSISTED |                                            |
| BuyerName, BuyerCompanyName, BuyerTaxCode, BuyerAddress | NVARCHAR / VARCHAR            | NULL; CHECK BuyerTaxCode IS NULL OR BuyerCompanyName IS NOT NULL                | Thông tin người mua (VAT đơn vị)           |
| Status                                                  | VARCHAR(10)                   | NOT NULL, DEFAULT Paid, CHECK IN (Paid, Void)                                   |                                            |
| IssuedAt, IssuedBy                                      | DATETIME2, INT                | NOT NULL; FK → Employees                                                        |                                            |
| VoidedAt, VoidedBy, VoidReason                          | DATETIME2, INT, NVARCHAR(200) | Bắt buộc cả 3 khi Status = Void                                                 |                                            |
| ReplacesInvoiceId                                       | INT                           | NULL, FK → Invoices                                                             | Hóa đơn bị thay khi lập lại                |

### 4.13 Payments

| **Cột**       | **Kiểu**      | **Ràng buộc**                                                | **Ý nghĩa**                      |
|---------------|---------------|--------------------------------------------------------------|----------------------------------|
| PaymentId     | INT IDENTITY  | PK                                                           |                                  |
| BookingRoomId | INT           | NOT NULL, FK → BookingRooms                                  | Tiền thuộc lượt lưu trú          |
| InvoiceId     | INT           | NULL, FK → Invoices                                          | Điền khi chốt vào hóa đơn        |
| Type          | VARCHAR(10)   | NOT NULL, CHECK IN (Deposit, Payment, Refund)                |                                  |
| Amount        | DECIMAL(14,0) | NOT NULL, CHECK \> 0                                         |                                  |
| SignedAmount  | cột tính      | CASE Type WHEN Refund THEN −Amount ELSE Amount END PERSISTED | Cộng thẳng để ra doanh thu       |
| Method        | VARCHAR(20)   | NOT NULL, CHECK IN (Cash, Card, BankTransfer, EWallet)       |                                  |
| ReferenceNo   | VARCHAR(50)   | Bắt buộc với BankTransfer, EWallet (CHECK)                   |                                  |
| PaidAt        | DATETIME2     | NOT NULL, DEFAULT                                            |                                  |
| EmployeeId    | INT           | NOT NULL, FK → Employees                                     |                                  |
| Note          | NVARCHAR(200) | NULL                                                         | Bắt buộc với bút toán điều chỉnh |

### 4.14 HousekeepingTasks

| **Cột**                           | **Kiểu**      | **Ràng buộc**                                                   | **Ý nghĩa**           |
|-----------------------------------|---------------|-----------------------------------------------------------------|-----------------------|
| TaskId                            | INT IDENTITY  | PK                                                              |                       |
| RoomId                            | INT           | NOT NULL, FK → Rooms                                            |                       |
| BookingRoomId                     | INT           | NULL, FK → BookingRooms                                         | Lượt vừa trả (nếu có) |
| Source                            | VARCHAR(20)   | NOT NULL, CHECK IN (CheckOut, AfterMaintenance)                 |                       |
| Status                            | VARCHAR(20)   | NOT NULL, DEFAULT Pending, CHECK IN (Pending, InProgress, Done) |                       |
| Result                            | VARCHAR(20)   | NULL, CHECK IN (Clean, NeedMaintenance); bắt buộc khi Done      |                       |
| AssignedTo                        | INT           | NULL, FK → Employees                                            | Người nhận việc       |
| CreatedAt, StartedAt, CompletedAt | DATETIME2     | CreatedAt NOT NULL                                              |                       |
| Note                              | NVARCHAR(500) | Bắt buộc khi Result = NeedMaintenance                           |                       |

## 5. Index và ràng buộc đặc biệt

| **Tên**                       | **Định nghĩa**                                                                      | **Mục đích**                                 |
|-------------------------------|-------------------------------------------------------------------------------------|----------------------------------------------|
| IX_BookingRooms_Room_Dates    | BookingRooms(RoomId, CheckInDate, CheckOutDate) WHERE Status IN (Booked, CheckedIn) | Kiểm tra trùng lịch nhanh                    |
| IX_BookingRooms_Booking       | BookingRooms(BookingId)                                                             | Tải phòng theo phiếu                         |
| UX_Invoices_ActivePerStay     | UNIQUE Invoices(BookingRoomId) WHERE Status = Paid                                  | Tối đa một hóa đơn hiệu lực mỗi lượt lưu trú |
| UX_Housekeeping_OpenTask      | UNIQUE HousekeepingTasks(RoomId) WHERE Status \<\> Done                             | Mỗi phòng tối đa một việc dọn chưa xong      |
| IX_Payments_BookingRoom       | Payments(BookingRoomId) INCLUDE(SignedAmount, Type)                                 | Tính đã thu của phòng                        |
| IX_Payments_PaidAt            | Payments(PaidAt) INCLUDE(SignedAmount, Method, Type)                                | Báo cáo doanh thu                            |
| IX_ServiceUsages_BookingRoom  | ServiceUsages(BookingRoomId)                                                        | Tổng dịch vụ của phòng                       |
| IX_Invoices_IssuedAt          | Invoices(IssuedAt) WHERE Status = Paid                                              | Doanh số theo hóa đơn                        |
| IX_Bookings_Status_CreatedAt  | Bookings(Status, CreatedAt)                                                         | Quét Pending hết hạn                         |
| IX_Customers_Phone / FullName | Customers(Phone), Customers(FullName)                                               | Tìm khách                                    |

*Lưu ý: index có lọc yêu cầu các tùy chọn SET mặc định của ADO.NET (ANSI_NULLS, QUOTED_IDENTIFIER ON); không tắt chúng trong kết nối.*

## 6. Kiểu bảng (user-defined table type)

| **Tên**              | **Cột**                                                                       | **Dùng cho**                                                                                                              |
|----------------------|-------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| dbo.BookingRoomInput | RoomId, CheckInDate, CheckOutDate, Adults, Children, GuestName, GuestIdNumber | Truyền danh sách phòng vào sp_CreateBooking trong một lần gọi (SqlParameter kiểu Structured). Không tính là bảng dữ liệu. |

## 7. Function

| **Tên**                        | **Tham số**                                       | **Trả về / mục đích**                                                                   |
|--------------------------------|---------------------------------------------------|-----------------------------------------------------------------------------------------|
| fn_LateCheckoutFee             | @PricePerNight, @CheckOutDate, @ActualCheckOut    | Số tiền phụ thu theo chính sách mục 11; trả NULL nếu sau 18:00 (phải gia hạn).          |
| fn_AvailableRooms (inline TVF) | @CheckIn, @CheckOut, @ExcludeBookingRoomId = NULL | Phòng IsActive, không Maintenance, không trùng lượt Booked/CheckedIn trong khoảng ngày. |
| fn_StayNetPaid                 | @BookingRoomId                                    | Σ SignedAmount của lượt lưu trú.                                                        |
| fn_RequiredDeposit             | @BookingRoomId                                    | ROUND(PricePerNight × Nights × DepositRate / 100, 0).                                   |

## 8. View

| **Tên**                                                 | **Nội dung**                                                                                                                           | **Dùng cho**                            |
|---------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------|-----------------------------------------|
| vw_RoomStays                                            | Lượt lưu trú kèm phiếu, người đặt, người ở, phòng, loại, ngày, số đêm, giá, trạng thái, người check-in/out                             | Màn hình booking, check-in, check-out   |
| vw_RoomStayFolio                                        | Theo lượt lưu trú: tiền phòng dự kiến, tổng dịch vụ, cọc yêu cầu, cọc đã thu, đã thu khác, đã hoàn, đã thu ròng, còn phải trả ước tính | Màn hình thanh toán, điều kiện check-in |
| vw_BookingSummary                                       | Theo phiếu: số phòng theo trạng thái, ngày nhận sớm nhất/trả muộn nhất, cọc yêu cầu và đã thu, đủ điều kiện xác nhận                   | Danh sách booking, xác nhận             |
| vw_InvoiceDetail                                        | Hóa đơn kèm khách, phòng, loại, nhân viên, người mua, đã thu                                                                           | Danh sách và chi tiết hóa đơn           |
| vw_InvoiceLines                                         | Dòng tiền phòng + từng dòng dịch vụ của hóa đơn (UNION ALL)                                                                            | In hóa đơn                              |
| vw_BookingStatement                                     | Tổng các hóa đơn Paid của một phiếu đặt                                                                                                | Bảng kê đối chiếu theo booking          |
| vw_RoomBoard                                            | Mỗi phòng: trạng thái hiện tại, khách đang ở, ngày trả dự kiến, khách đến hôm nay                                                      | Sơ đồ phòng                             |
| vw_TodayArrivals / vw_InHouseStays / vw_TodayDepartures | Lượt Booked nhận hôm nay / đang ở / dự kiến trả hôm nay                                                                                | Dashboard lễ tân                        |
| vw_PaymentLedger                                        | Sổ thu: khoản thu có dấu, loại, phương thức, phòng, phiếu, nhân viên                                                                   | Báo cáo doanh thu thực thu              |
| vw_ServiceUsageStats                                    | Theo dịch vụ: số lượt, số lượng, doanh số                                                                                              | Báo cáo dịch vụ                         |
| vw_HousekeepingQueue                                    | Việc dọn chưa xong kèm phòng, tầng, người nhận, thời gian chờ                                                                          | Màn hình Housekeeping                   |
| vw_RoomStatusSummary                                    | Đếm phòng IsActive theo trạng thái                                                                                                     | Dashboard                               |

## 9. Stored procedure

**Đặt phòng và cọc**

| **Procedure**            | **Tham số chính**                                                                                             | **Xử lý**                                                                                                                       |
|--------------------------|---------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------|
| sp_CreateBooking         | @CustomerId, @DepositRate, @Note, @EmployeeId, @Rooms (BookingRoomInput), @BookingId OUT                      | Kiểm tra ngày, sức chứa từng phòng, trùng lịch (UPDLOCK, HOLDLOCK); tạo Bookings và BookingRooms với PricePerNight = BasePrice. |
| sp_UpdateBookingRoom     | @BookingRoomId, @NewRoomId, @CheckInDate, @CheckOutDate, @Adults, @Children, @KeepPrice, @Reason, @EmployeeId | Chỉ lượt Booked; kiểm tra lại lịch; @KeepPrice = 1 chỉ khi đổi do phòng hỏng và hạng mới ≥ hạng cũ; tính lại trạng thái phiếu.  |
| sp_CancelBookingRoom     | @BookingRoomId, @Reason, @IsNoShow, @RefundMethod, @EmployeeId                                                | Áp chính sách 48 giờ/no-show; tự ghi Refund khi đủ điều kiện; Status = Cancelled.                                               |
| sp_CancelBooking         | @BookingId, @Reason, @RefundMethod, @EmployeeId                                                               | Gọi hủy từng phòng còn Booked.                                                                                                  |
| sp_RecordBookingDeposit  | @BookingId, @Amount, @Method, @ReferenceNo, @EmployeeId                                                       | Chia số tiền theo phần cọc còn thiếu của từng phòng, ghi nhiều dòng Deposit cùng ReferenceNo.                                   |
| sp_RecordPayment         | @BookingRoomId, @Type, @Amount, @Method, @ReferenceNo, @Note, @EmployeeId                                     | Kiểm tra loại khoản hợp với trạng thái phòng và quyền; ghi Payments; tính lại trạng thái phiếu.                                 |
| sp_RefreshBookingStatus  | @BookingId                                                                                                    | Tính trạng thái phiếu từ trạng thái các phòng và cọc (I.2).                                                                     |
| sp_ExpirePendingBookings | (không)                                                                                                       | Hủy phiếu Pending chưa từng xác nhận, quá 24 giờ, chưa đủ cọc. Gọi khi mở ứng dụng/màn hình booking.                            |

**Lưu trú và trả phòng**

| **Procedure**                                                      | **Tham số chính**                                                                                                                                             | **Xử lý**                                                                                                                                                                                              |
|--------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| sp_CheckInRoom                                                     | @BookingRoomId, @GuestName, @GuestIdNumber, @EmployeeId                                                                                                       | Kiểm tra trạng thái, ngày, cọc, phòng Available; ghi ActualCheckIn; phòng Occupied; tính lại trạng thái phiếu.                                                                                         |
| sp_AddServiceUsage / sp_UpdateServiceUsage / sp_DeleteServiceUsage | @BookingRoomId hoặc @UsageId, @ServiceId, @Quantity, @UsedAt, @Note, @EmployeeId                                                                              | Chỉ khi phòng CheckedIn (hoặc A/M sau khi void); dịch vụ IsActive, không IsSystem; UsedAt trong khoảng lưu trú; UnitPrice lấy từ Services.                                                             |
| sp_ExtendStay                                                      | @BookingRoomId, @NewCheckOutDate, @EmployeeId                                                                                                                 | Lượt CheckedIn; kiểm tra trùng lịch các đêm thêm; dời CheckOutDate; giữ giá.                                                                                                                           |
| sp_PreviewCheckOut                                                 | @BookingRoomId, @CheckOutAt, @Discount, @TaxRate                                                                                                              | Trả về số đêm, tiền phòng, dịch vụ, số phút trễ, phụ thu, tổng, đã thu, còn phải thu/hoàn; không ghi dữ liệu.                                                                                          |
| sp_CheckOutRoom                                                    | @BookingRoomId, @CheckOutAt, @Discount, @DiscountReason, @TaxRate, @Buyer…, @PayAmount, @PayMethod, @PayReference, @RefundMethod, @EmployeeId, @InvoiceId OUT | Một transaction: phụ thu trễ, ActualCheckOut, hóa đơn Paid, thu thêm hoặc hoàn dư, gắn InvoiceId cho khoản thu, phòng Cleaning, tạo việc dọn, tính lại trạng thái phiếu. Thiếu tiền → THROW, rollback. |

**Hóa đơn, phòng, dọn phòng, báo cáo**

| **Procedure**               | **Tham số chính**                                                                                                                 | **Xử lý**                                                                                                                               |
|-----------------------------|-----------------------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------|
| sp_VoidInvoice              | @InvoiceId, @Reason, @EmployeeId                                                                                                  | A/M; Status = Void, ghi người và lý do; không đụng tới Payments.                                                                        |
| sp_ReissueInvoice           | @BookingRoomId, @Discount, @DiscountReason, @TaxRate, @Buyer…, @PayAmount, @PayMethod, @RefundMethod, @EmployeeId, @InvoiceId OUT | Yêu cầu hóa đơn trước đã Void; tính lại từ dữ liệu hiện tại (không tính lại phụ thu); thu thêm hoặc hoàn chênh lệch; ReplacesInvoiceId. |
| sp_SetRoomMaintenance       | @RoomId, @Note, @EmployeeId                                                                                                       | Chỉ từ Available; trả về danh sách lượt Booked bị ảnh hưởng để đổi phòng.                                                               |
| sp_FinishMaintenance        | @RoomId, @EmployeeId                                                                                                              | Maintenance → Cleaning; tạo việc dọn (AfterMaintenance).                                                                                |
| sp_StartHousekeepingTask    | @TaskId, @EmployeeId                                                                                                              | Pending → InProgress; ghi người nhận, giờ bắt đầu.                                                                                      |
| sp_CompleteHousekeepingTask | @TaskId, @Result, @Note, @EmployeeId                                                                                              | Done; Clean → phòng Available; NeedMaintenance → phòng Maintenance.                                                                     |
| sp_DashboardSummary         | @Date                                                                                                                             | Các chỉ số trong ngày (mục 15).                                                                                                         |
| sp_RevenueReport            | @FromDate, @ToDate, @GroupBy                                                                                                      | Thực thu theo ngày/phương thức/loại và doanh số theo hóa đơn.                                                                           |
| sp_GetEmployeeForLogin      | @Username                                                                                                                         | Trả EmployeeId, FullName, RoleName, PasswordHash, IsActive; xác minh mật khẩu ở C#.                                                     |

## 10. Hằng số chính sách

| **Hằng số**                       | **Giá trị**                  | **Dùng ở**                         |
|-----------------------------------|------------------------------|------------------------------------|
| Giờ nhận phòng chuẩn              | 14:00                        | Check-in, hủy 48 giờ               |
| Giờ trả phòng chuẩn               | 12:00                        | Phụ thu trễ, no-show               |
| Miễn phụ thu                      | 30 phút                      | fn_LateCheckoutFee                 |
| Phụ thu đến 2 giờ                 | 5% giá một đêm / 30 phút     | fn_LateCheckoutFee                 |
| Phụ thu từ giờ thứ 3              | 15% giá một đêm / giờ        | fn_LateCheckoutFee                 |
| Mốc bắt buộc gia hạn              | 18:00                        | sp_CheckOutRoom                    |
| Tỷ lệ cọc                         | 50% hoặc 100%                | Bookings.DepositRate               |
| Hạn giữ chỗ Pending               | 24 giờ                       | sp_ExpirePendingBookings           |
| Hạn hủy được hoàn cọc             | 48 giờ trước 14:00 ngày nhận | sp_CancelBookingRoom               |
| Mốc no-show                       | 12:00 ngày sau CheckInDate   | sp_CancelBookingRoom               |
| Thuế VAT mặc định                 | 10%                          | Invoices.TaxRate                   |
| Hạn mức giảm giá của Receptionist | 10%                          | sp_CheckOutRoom, sp_ReissueInvoice |
| Cảnh báo số lượng dịch vụ         | \> 20                        | Giao diện ghi dịch vụ              |
| Hiệu lực xem trước check-out      | 10 phút                      | sp_CheckOutRoom                    |

*Mỗi hằng số khai báo ở một chỗ trong SQL và trùng giá trị với lớp Constants.cs; đổi chính sách thì sửa cả hai.*

## 11. Dữ liệu khởi tạo

- 4 vai trò cố định; 1 tài khoản Admin (đổi mật khẩu lần đầu).

- Dịch vụ hệ thống "Phụ thu trả phòng trễ" (IsSystem = 1, Price = 0).

- Dữ liệu mẫu (loại phòng, phòng, khách, booking ở mọi trạng thái, hóa đơn, khoản thu, việc dọn) tạo bằng chính các procedure trên để dữ liệu đúng quy tắc.

## 12. Ghi chú triển khai

- Mỗi procedure nghiệp vụ: SET XACT_ABORT ON; BEGIN TRY … BEGIN TRAN … COMMIT; BEGIN CATCH ROLLBACK; THROW. Ứng dụng đổi mã lỗi 50xxx thành thông báo tiếng Việt, không hiện ex.Message thô.

- Chống đặt trùng khi hai lễ tân thao tác cùng lúc: đọc lịch với WITH (UPDLOCK, HOLDLOCK) trong transaction của sp_CreateBooking, sp_UpdateBookingRoom, sp_ExtendStay.

- Procedure nhận @EmployeeId và tự kiểm tra vai trò, ngoài kiểm tra ở giao diện.

- SQL Server Express không có SQL Agent: gọi sp_ExpirePendingBookings khi mở ứng dụng và khi mở màn hình booking.

- Tìm kiếm tiếng Việt không dấu: so sánh với COLLATE Vietnamese_CI_AI; tham số kiểu NVARCHAR.

- Sau khi nhóm duyệt, viết lại script tạo DB theo thứ tự: bảng → index → type → function → view → procedure → dữ liệu khởi tạo, rồi cập nhật bộ test case theo mục IV.

**KẾT THÚC TÀI LIỆU**
