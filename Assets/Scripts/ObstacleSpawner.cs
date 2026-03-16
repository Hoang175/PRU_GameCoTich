using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Kho đồ nghề (Kéo Prefab vào đây)")]
    public GameObject[] mangChungNgaiVat;

    [Header("Cài đặt Độ khó")]
    public float thoiGianSpawnMin = 1.5f; // Nhả nhanh nhất
    public float thoiGianSpawnMax = 3.5f; // Nhả chậm nhất
    public float tocDoChungNgaiVat = 5f;  // Lưu ý: Gõ số này BẰNG VỚI TỐC ĐỘ MAP

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        // 1. Chờ đến khi Tấm leo lên Xích Thố xong mới bắt đầu nhả đồ
        yield return new WaitUntil(() => Level4Director.isPlaying == true);

        // 2. Vòng lặp đẻ vô tận
        while (true)
        {
            // Nghỉ một chút theo thời gian Random
            float thoiGianCho = Random.Range(thoiGianSpawnMin, thoiGianSpawnMax);
            yield return new WaitForSeconds(thoiGianCho);

            // Bốc đại 1 món trong kho (Đá, Hàng rào, Gỗ...)
            int index = Random.Range(0, mangChungNgaiVat.Length);

            // Đẻ nó ra ngay tại vị trí của Spawner
            GameObject quai = Instantiate(mangChungNgaiVat[index], transform.position, Quaternion.identity);

            // TỰ ĐỘNG gắn script bắt nó chạy sang trái
            ObstacleMove moveScript = quai.AddComponent<ObstacleMove>();
            moveScript.speed = tocDoChungNgaiVat;
        }
    }
}