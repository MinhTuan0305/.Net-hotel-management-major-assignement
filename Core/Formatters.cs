// =============================================================================
// Formatters.cs
// -----------------------------------------------------------------------------
// FILE NÀY ĐỂ LÀM GÌ
//   Định dạng số tiền, ngày giờ, phần trăm theo kiểu Việt Nam để mọi form hiển
//   thị giống nhau, và đọc ngược số tiền người dùng gõ vào.
//     Money(1250000)            -> "1.250.000 đ"
//     TryParseMoney("1.250.000") -> 1250000
//     Date(d) -> "14/10/2026"     DateTimeText(d) -> "14/10/2026 14:30"
//     Duration(150)             -> "2 giờ 30 phút"   (số phút trả phòng trễ)
//     MoneyToWords(1250000)     -> "Một triệu hai trăm năm mươi nghìn đồng" (in hóa đơn)
//   Các hàm nhận object chấp nhận luôn DBNull (ô NULL trong DataTable) và trả "".
//
// LƯU Ý
//   Tiền trong DB là DECIMAL(14,0) – số nguyên đồng, không có phần lẻ.
// =============================================================================
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace HotelManagement.Core
{
    public static class Formatters
    {
        /// <summary>Văn hóa vi-VN: dấu chấm ngăn nghìn.</summary>
        public static readonly CultureInfo Vi = CultureInfo.GetCultureInfo("vi-VN");

        /// <summary>Chuỗi định dạng tiền cho cột DataGridView / ToString.</summary>
        public const string MoneyFormat = "#,##0";
        public const string DateFormat = "dd/MM/yyyy";
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm";
        public const string TimeFormat = "HH:mm";

        // ------------------------------------------------------------------
        // Tiền
        // ------------------------------------------------------------------

        public static string Money(decimal amount)
        {
            return amount.ToString(MoneyFormat, Vi) + " đ";
        }

        /// <summary>Định dạng giá trị đọc từ DataRow; NULL trả "".</summary>
        public static string Money(object value)
        {
            if (value == null || value == DBNull.Value) return "";
            return Money(Convert.ToDecimal(value, CultureInfo.InvariantCulture));
        }

        /// <summary>Chỉ số, không kèm "đ" (dùng trong ô nhập).</summary>
        public static string Number(decimal amount)
        {
            return amount.ToString(MoneyFormat, Vi);
        }

        /// <summary>
        /// Đọc số tiền người dùng gõ: chấp nhận "1.250.000", "1,250,000", "1250000",
        /// "1.250.000 đ". Bỏ mọi ký tự không phải số; dấu "-" ở đầu là số âm.
        /// </summary>
        public static bool TryParseMoney(string text, out decimal amount)
        {
            amount = 0;
            if (string.IsNullOrWhiteSpace(text)) return false;

            string trimmed = text.Trim();
            bool negative = trimmed.StartsWith("-", StringComparison.Ordinal);
            var digits = new StringBuilder();
            foreach (char c in trimmed)
            {
                if (c >= '0' && c <= '9') digits.Append(c);
            }
            if (digits.Length == 0 || digits.Length > 14) return false;   // DECIMAL(14,0)

            amount = decimal.Parse(digits.ToString(), CultureInfo.InvariantCulture);
            if (negative) amount = -amount;
            return true;
        }

        // ------------------------------------------------------------------
        // Ngày giờ
        // ------------------------------------------------------------------

        public static string Date(DateTime value)
        {
            return value.ToString(DateFormat, CultureInfo.InvariantCulture);
        }

        public static string Date(object value)
        {
            if (value == null || value == DBNull.Value) return "";
            return Date(Convert.ToDateTime(value, CultureInfo.InvariantCulture));
        }

        public static string DateTimeText(DateTime value)
        {
            return value.ToString(DateTimeFormat, CultureInfo.InvariantCulture);
        }

        public static string DateTimeText(object value)
        {
            if (value == null || value == DBNull.Value) return "";
            return DateTimeText(Convert.ToDateTime(value, CultureInfo.InvariantCulture));
        }

        public static string Time(DateTime value)
        {
            return value.ToString(TimeFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>Số phút -> "2 giờ 30 phút", "45 phút", "1 giờ".</summary>
        public static string Duration(int minutes)
        {
            if (minutes <= 0) return "0 phút";
            int h = minutes / 60;
            int m = minutes % 60;
            if (h == 0) return m + " phút";
            if (m == 0) return h + " giờ";
            return h + " giờ " + m + " phút";
        }

        /// <summary>Số đêm -> "3 đêm".</summary>
        public static string Nights(int nights)
        {
            return nights + " đêm";
        }

        // ------------------------------------------------------------------
        // Phần trăm
        // ------------------------------------------------------------------

        /// <summary>10 -> "10%", 12.5 -> "12,5%".</summary>
        public static string Percent(decimal value)
        {
            return value.ToString("0.##", Vi) + "%";
        }

        // ------------------------------------------------------------------
        // Đọc số tiền thành chữ (in trên hóa đơn)
        // ------------------------------------------------------------------

        private static readonly string[] Digits =
            { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };

        private static readonly string[] Units =
            { "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ" };

        /// <summary>1250000 -> "Một triệu hai trăm năm mươi nghìn đồng".</summary>
        public static string MoneyToWords(decimal amount)
        {
            decimal rounded = Math.Round(amount, 0, MidpointRounding.AwayFromZero);
            bool negative = rounded < 0;
            if (negative) rounded = -rounded;
            if (rounded >= 1000000000000000000m) return Money(amount);   // quá lớn, không đọc

            long n = (long)rounded;
            if (n == 0) return "Không đồng";

            var groups = new List<int>();          // nhóm 3 chữ số, từ hàng đơn vị lên
            while (n > 0)
            {
                groups.Add((int)(n % 1000));
                n /= 1000;
            }

            var parts = new List<string>();
            for (int i = groups.Count - 1; i >= 0; i--)
            {
                int g = groups[i];
                if (g == 0) continue;
                bool full = i < groups.Count - 1;  // nhóm không đứng đầu thì đọc đủ "không trăm", "lẻ"
                parts.Add(ReadTriple(g, full) + Units[i]);
            }

            string text = (negative ? "âm " : "") + string.Join(" ", parts) + " đồng";
            return char.ToUpper(text[0], Vi) + text.Substring(1);
        }

        private static string ReadTriple(int n, bool full)
        {
            int hundreds = n / 100;
            int tens = (n % 100) / 10;
            int ones = n % 10;
            var words = new List<string>();

            if (full || hundreds > 0)
            {
                words.Add(Digits[hundreds]);
                words.Add("trăm");
            }

            if (tens == 0)
            {
                if (ones != 0)
                {
                    if (full || hundreds > 0) words.Add("lẻ");
                    words.Add(Digits[ones]);
                }
            }
            else if (tens == 1)
            {
                words.Add("mười");
                if (ones == 5) words.Add("lăm");
                else if (ones != 0) words.Add(Digits[ones]);
            }
            else
            {
                words.Add(Digits[tens]);
                words.Add("mươi");
                if (ones == 1) words.Add("mốt");
                else if (ones == 4) words.Add("tư");
                else if (ones == 5) words.Add("lăm");
                else if (ones != 0) words.Add(Digits[ones]);
            }

            return string.Join(" ", words);
        }
    }
}
