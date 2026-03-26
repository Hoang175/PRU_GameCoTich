# 🌟 Untold Tales: Fairy Tales (Tấm Cám - Chuyện Chưa Kể)

![Unity Version](https://img.shields.io/badge/Unity-6.3%20LTS-black?logo=unity)
![Platform](https://img.shields.io/badge/Platform-Android%20%7C%20Windows-brightgreen?logo=android)
![Genre](https://img.shields.io/badge/Genre-Multi--Genre%20(Stealth%2C%20Runner%2C%20Puzzle)-blue)
![Status](https://img.shields.io/badge/Status-Completed-success)

Chào mừng bạn đến với **Untold Tales: Fairy Tales** - một tựa game 2D đưa người chơi bước vào thế giới cổ tích Việt Nam huyền diệu, nhưng với một góc nhìn và lối chơi hoàn toàn mới lạ! 

Đây không chỉ là một tựa game kể lể cốt truyện thông thường. Dự án này là sự kết hợp đầy tham vọng của **nhiều thể loại gameplay khác nhau** (Multi-genre) trong cùng một tựa game, đòi hỏi người chơi phải liên tục thích nghi và vận dụng sự khéo léo để giúp Tấm giành lấy hạnh phúc.

---

## 📖 Cốt truyện: Hành trình lật ngược thế cờ
Hóa thân thành Tấm - một cô gái nông thôn hiền lành nhưng liên tục bị chèn ép bởi người Dì Ghẻ độc ác và cô em Cám ranh ma. Thay vì chỉ ngồi khóc chờ Bụt hiện lên, **bạn** sẽ là người trực tiếp hành động! 

Người chơi sẽ đồng hành cùng Tấm lội suối bắt cá, lén lút né tránh những ánh mắt rình rập khắt khe của Dì Ghẻ, giải mã các thử thách phép thuật và cuối cùng là phi ngựa bạt mạng xuyên màn đêm đến Hoàng cung để ướm thử chiếc hài vàng định mệnh. Khởi đầu là một cô bé nhem nhuốc, kết thúc là một Hoàng Hậu uy nghi. Liệu bạn có đủ khéo léo để viết lại cái kết cho câu chuyện này?

---

## 🎮 Khám phá 5 Màn Chơi - 5 Thể Loại Khác Biệt
Điểm nhấn lớn nhất của dự án là thiết kế mỗi Level mang một cơ chế (Mechanic) hoàn toàn riêng biệt, giữ cho nhịp game luôn tươi mới:

* **🐟 Level 1: Fishing Task (Thu thập & Khám phá)** * *Thể loại:* Top-down RPG. 
  * *Nhiệm vụ:* Điều khiển Tấm di chuyển linh hoạt trên bản đồ, lội nước để bắt đủ số lượng cá bống/tép theo yêu cầu.

* **🥷 Level 2: Stealthy Entry (Hành động lén lút)** * *Thể loại:* Stealth / Maze. 
  * *Nhiệm vụ:* Gameplay cốt lõi xoay quanh hệ thống **Tầm nhìn (Vision Cone)** và **Trí tuệ nhân tạo (Enemy Patrol AI)**. Tấm phải lợi dụng điểm mù, nấp sau bụi cây, tìm chìa khóa trong đống rơm và thậm chí là *ném đá giấu tay* để đánh lạc hướng Dì Ghẻ, qua đó đột nhập thành công vào nhà.

* **✨ Level 3: Bird's Magic (Giải đố / Tìm đồ)** * *Thể loại:* Point & Click / Hidden Object. 
  * *Nhiệm vụ:* Tương tác với môi trường và sử dụng sự trợ giúp của Bụt cùng đàn chim sẻ thần kỳ để tìm kiếm các vật phẩm bị giấu kín (Áo tứ thân, Đôi hài).

* **🐎 Level 4: Festival Gallop (Đua tốc độ)** * *Thể loại:* 2D Auto-Runner. 
  * *Nhiệm vụ:* Nhịp độ game được đẩy lên mức cao trào. Tấm cưỡi Xích Thố tự động phi nước đại. Hệ thống **Random Spawner** sẽ liên tục tạo ra chướng ngại vật (đá tảng, khúc gỗ), đòi hỏi người chơi phải phản xạ cực nhanh để nhảy (Jump) và né tránh.

* **👑 Level 5: The Royal Slipper (Mê cung Hoàng cung)** * *Thể loại:* Advanced Stealth. 
  * *Nhiệm vụ:* Trở lại với lối chơi lén lút nhưng ở không gian rộng lớn và phức tạp hơn. Luồn lách qua các cột đình chạm trổ và chậu cây hoàng gia để tiếp cận Nhà Vua mà không bị mẹ con Cám phát hiện.

---

## 🛠 Điểm nhấn Kỹ thuật (Technical Highlights)
Dự án được phát triển với tư duy code sạch, dễ bảo trì và tối ưu cho thiết bị di động (Mobile Optimization):

1. **Kiến trúc Code (Clean Code & Namespace):** Toàn bộ Script được quản lý độc lập, bọc trong hệ thống `Namespace` riêng biệt để đảm bảo an toàn, tránh xung đột biến toàn cục.
2. **Hệ thống Quick-Edit (Thân thiện với Game Designer):** Khai thác triệt để `[SerializeField]` và `ScriptableObject`. Tốc độ chạy, lực nhảy, số lượng quái, thời gian spawn... tất cả đều có thể được tinh chỉnh mượt mà ngay trên Inspector của Unity mà không cần can thiệp vào mã nguồn.
3. **Tối ưu Giao diện (Responsive UI):** Sử dụng hệ thống *Canvas Scaler (Match Width/Height)*, đảm bảo giao diện game hiển thị hoàn hảo, không bị méo hay cắt xén trên mọi tỷ lệ màn hình điện thoại "tràn viền" hiện nay (19:9, 20:9...).
4. **Quản lý Scene & Bất đồng bộ:** Sử dụng hệ thống Canvas Panel để bật/tắt UI tức thì ở Main Menu thay vì load Scene mới, mang lại trải nghiệm mượt mà (Fast Performance) cho người chơi.

---

## 🚀 Cài đặt & Chạy Game

**1. Trải nghiệm trên điện thoại Android:**
* Vào phần [Releases](../../releases) của repository này.
* Tải file `UntoldTales.apk` về thiết bị Android (Yêu cầu Android 13+) và cài đặt.

**2. Dành cho Nhà phát triển (Mở project trong Unity):**
* Clone repository này về máy của bạn:
  ```bash
  git clone [https://github.com/Hoang175/PRU_GameCoTich.git](https://github.com/Hoang175/PRU_GameCoTich.git)