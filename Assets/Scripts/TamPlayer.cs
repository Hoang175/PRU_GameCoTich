//using UnityEngine;
//using SV = HE181245_DaoHuyHoang_UntoldTalesFairyTales;
//public class Tam : MonoBehaviour
//{
//    [Header("Giao diện Mobile")]
//    public Joystick joystick; // Kéo thả Fixed Joystick vào đây

//    [Header("Cài đặt Di chuyển")]
//    public float moveSpeed = 5f;

//    [Header("Cài đặt Lội Nước & Bước Chân")]
//    public float waterSpeed = 2.5f;
//    public AudioClip waterSound;       
//    public AudioClip landStepSound;    
//    public float stepInterval = 0.5f;

//    [Header("Âm thanh (Audio)")]
//    public AudioSource audioSource;    
//    public AudioSource footstepSource; 

//    private float originalSpeed;
//    private bool isInWater = false;
//    private float stepTimer = 0f;

//    private Rigidbody2D rb;
//    private SpriteRenderer spriteRenderer;
//    private Animator animator;

//    private Vector2 movement;
//    private bool isPicking;

//    private FacingDirection facing = FacingDirection.Down;
//    private Vector3 baseSceneScale;
//    private Vector2 baseSpriteSize;

//    private enum FacingDirection { Left, Right, Up, Down }

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        spriteRenderer = GetComponent<SpriteRenderer>();
//        animator = GetComponent<Animator>();

//        if (audioSource == null) audioSource = GetComponent<AudioSource>();

//        originalSpeed = moveSpeed;
//        baseSceneScale = transform.localScale;

//        if (spriteRenderer.sprite != null)
//            baseSpriteSize = spriteRenderer.sprite.bounds.size;
//    }

//    void Update()
//    {
//        if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
//        {
//            movement = Vector2.zero;
//            animator.SetBool("moveSide", false); animator.SetBool("moveUp", false); animator.SetBool("moveDown", false);

//            if (Input.GetKeyDown(KeyCode.Space)) DialogueManager.instance.DisplayNextSentence();

//            if (footstepSource != null && footstepSource.isPlaying) footstepSource.Stop();
//            return;
//        }

//        if (isPicking)
//        {
//            movement = Vector2.zero;
//            animator.SetBool("moveSide", false); animator.SetBool("moveUp", false); animator.SetBool("moveDown", false);
//            return;
//        }

//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            StartPick();
//            return;
//        }

//        //float moveX = Input.GetAxisRaw("Horizontal");
//        //float moveY = Input.GetAxisRaw("Vertical");

//        //if (moveX != 0) moveY = 0;

//        float moveX = Input.GetAxisRaw("Horizontal");
//        float moveY = Input.GetAxisRaw("Vertical");

//        // Nếu đang dùng Joystick trên điện thoại thì lấy giá trị của Joystick
//        if (joystick != null)
//        {
//            if (Mathf.Abs(joystick.Horizontal) >= 0.2f) moveX = joystick.Horizontal;
//            if (Mathf.Abs(joystick.Vertical) >= 0.2f) moveY = joystick.Vertical;
//        }

//        // Ưu tiên di chuyển theo 1 hướng (ngang hoặc dọc) giống logic cũ của bạn
//        if (Mathf.Abs(moveX) > Mathf.Abs(moveY)) moveY = 0;
//        else if (Mathf.Abs(moveY) > Mathf.Abs(moveX)) moveX = 0;
//        // end mobile

//        movement = new Vector2(moveX, moveY);

//        animator.SetBool("moveSide", false); animator.SetBool("moveUp", false); animator.SetBool("moveDown", false);

//        if (movement != Vector2.zero)
//        {
//            if (moveY > 0) { animator.SetBool("moveUp", true); facing = FacingDirection.Up; }
//            else if (moveY < 0) { animator.SetBool("moveDown", true); facing = FacingDirection.Down; }
//            else { animator.SetBool("moveSide", true); facing = moveX > 0 ? FacingDirection.Right : FacingDirection.Left; }

//            stepTimer -= Time.deltaTime;
//            if (stepTimer <= 0)
//            {
//                if (footstepSource != null)
//                {
//                    if (isInWater && waterSound != null)
//                    {
//                        footstepSource.clip = waterSound;
//                        footstepSource.Play();
//                    }
//                    else if (!isInWater && landStepSound != null)
//                    {
//                        footstepSource.clip = landStepSound;
//                        footstepSource.Play();
//                    }
//                }
//                stepTimer = stepInterval;
//            }
//        }
//        else
//        {
//            stepTimer = 0f;
//            if (footstepSource != null && footstepSource.isPlaying)
//            {
//                footstepSource.Stop(); 
//            }
//        }

//        spriteRenderer.flipX = (facing == FacingDirection.Left);
//    }

//    void FixedUpdate()
//    {
//        if (!isPicking) rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
//    }

//    void StartPick()
//    {
//        isPicking = true;
//        movement = Vector2.zero;
//        GetComponent<FishCollector>()?.TryInteract();

//        switch (facing)
//        {
//            case FacingDirection.Left: case FacingDirection.Right: animator.SetTrigger("pickSide"); break;
//            case FacingDirection.Up: animator.SetTrigger("pickUp"); break;
//            case FacingDirection.Down: animator.SetTrigger("pickDown"); break;
//        }
//        Invoke("EndPick", 0.5f);
//    }

//    public void EndPick()
//    {
//        isPicking = false;
//        animator.ResetTrigger("pickSide"); animator.ResetTrigger("pickUp"); animator.ResetTrigger("pickDown");
//    }

//    void LateUpdate()
//    {
//        if (spriteRenderer.sprite == null) return;
//        Vector2 currentSize = spriteRenderer.sprite.bounds.size;
//        if (currentSize.x == 0 || currentSize.y == 0) return;
//        float scaleX = baseSpriteSize.x / currentSize.x;
//        float scaleY = baseSpriteSize.y / currentSize.y;
//        transform.localScale = new Vector3(baseSceneScale.x * scaleX, baseSceneScale.y * scaleY, baseSceneScale.z);
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Water"))
//        {
//            isInWater = true;
//            moveSpeed = waterSpeed;
//            if (audioSource != null && waterSound != null) audioSource.PlayOneShot(waterSound);
//        }
//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Water"))
//        {
//            isInWater = false;
//            moveSpeed = originalSpeed;
//            if (footstepSource != null) footstepSource.Stop();
//        }
//    }

//    // Hàm này sẽ được gọi khi bạn bấm nút Tương Tác trên màn hình cảm ứng

//    //public void OnInteractButton_Pressed()
//    //{
//    //    // 1. Nếu đang nói chuyện thì next thoại
//    //    if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
//    //    {
//    //        DialogueManager.instance.DisplayNextSentence();
//    //        if (footstepSource != null && footstepSource.isPlaying) footstepSource.Stop();
//    //        return; // Thoát ra luôn để không chạy lệnh nhặt đồ/bắt chuyện mới
//    //    }

//    //    // 2. DÙNG LỆNH MỚI CỦA UNITY 6: FindObjectsByType
//    //    HE181245_DaoHuyHoang_UntoldTalesFairyTales.NPC_Dialogue[] allNPCs = FindObjectsByType<HE181245_DaoHuyHoang_UntoldTalesFairyTales.NPC_Dialogue>(FindObjectsSortMode.None);

//    //    foreach (var npc in allNPCs)
//    //    {
//    //        npc.TriggerDialogue(); // Hàm này sẽ tự kiểm tra xem nhân vật có đứng gần NPC đó không
//    //    }

//    //    // 3. Thực hiện nhặt đồ (nếu có hàm nhặt)
//    //    if (!isPicking)
//    //    {
//    //        StartPick();
//    //    }
//    //}

//    public void OnInteractButton_Pressed()
//    {
//        // 1. Next thoại nếu đang nói chuyện
//        if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
//        {
//            DialogueManager.instance.DisplayNextSentence();
//            if (footstepSource != null && footstepSource.isPlaying) footstepSource.Stop();
//            return;
//        }

//        // Báo cho script biết là dùng Namespace nào


//        // 2. Tìm NPC ở gần
//        SV.NPC_Dialogue[] allNPCs = FindObjectsByType<SV.NPC_Dialogue>(FindObjectsSortMode.None);
//        foreach (var npc in allNPCs) { npc.TriggerDialogue(); }

//        // 3. Tìm ĐỐNG RƠM ở gần
//        SV.DongRomInteract[] domRoms = FindObjectsByType<SV.DongRomInteract>(FindObjectsSortMode.None);
//        foreach (var rom in domRoms) { rom.TriggerInteract(); }

//        // 4. Tìm CHÌA KHÓA ở gần
//        SV.KeyItem[] keys = FindObjectsByType<SV.KeyItem>(FindObjectsSortMode.None);
//        foreach (var key in keys) { key.TriggerInteract(); }

//        // 5. Tìm CỔNG KHÓA ở gần
//        SV.LockedGateInteract[] gates = FindObjectsByType<SV.LockedGateInteract>(FindObjectsSortMode.None);
//        foreach (var gate in gates) { gate.TriggerInteract(); }

//        // 6. Hành động nhặt (bắt cá) gốc
//        if (!isPicking)
//        {
//            StartPick();
//        }
//    }
//}

using UnityEngine;
using SV = HE181245_DaoHuyHoang_UntoldTalesFairyTales;

namespace HE181245_DaoHuyHoang_UntoldTalesFairyTales
{
    public class Tam : MonoBehaviour
    {
        [Header("Giao diện Mobile")]
        public Joystick joystick; // Kéo thả Fixed Joystick vào đây

        [Header("Cài đặt Di chuyển")]
        public float moveSpeed = 5f;

        [Header("Cài đặt Lội Nước & Bước Chân")]
        public float waterSpeed = 2.5f;
        public AudioClip waterSound;
        public AudioClip landStepSound;
        public float stepInterval = 0.5f;

        [Header("Âm thanh (Audio)")]
        public AudioSource audioSource;
        public AudioSource footstepSource;

        private float originalSpeed;
        private bool isInWater = false;
        private float stepTimer = 0f;

        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        private Vector2 movement;
        private bool isPicking;

        private FacingDirection facing = FacingDirection.Down;
        private Vector3 baseSceneScale;
        private Vector2 baseSpriteSize;

        private enum FacingDirection { Left, Right, Up, Down }

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            if (audioSource == null) audioSource = GetComponent<AudioSource>();

            originalSpeed = moveSpeed;
            baseSceneScale = transform.localScale;

            if (spriteRenderer.sprite != null)
                baseSpriteSize = spriteRenderer.sprite.bounds.size;
        }

        void Update()
        {
            if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
            {
                movement = Vector2.zero;
                animator.SetBool("moveSide", false); animator.SetBool("moveUp", false); animator.SetBool("moveDown", false);

                if (Input.GetKeyDown(KeyCode.Space)) DialogueManager.instance.DisplayNextSentence();

                if (footstepSource != null && footstepSource.isPlaying) footstepSource.Stop();
                return;
            }

            if (isPicking)
            {
                movement = Vector2.zero;
                animator.SetBool("moveSide", false); animator.SetBool("moveUp", false); animator.SetBool("moveDown", false);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartPick();
                return;
            }

            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            // Nếu đang dùng Joystick trên điện thoại thì lấy giá trị của Joystick
            if (joystick != null)
            {
                if (Mathf.Abs(joystick.Horizontal) >= 0.2f) moveX = joystick.Horizontal;
                if (Mathf.Abs(joystick.Vertical) >= 0.2f) moveY = joystick.Vertical;
            }

            // Ưu tiên di chuyển theo 1 hướng (ngang hoặc dọc) giống logic cũ của bạn
            if (Mathf.Abs(moveX) > Mathf.Abs(moveY)) moveY = 0;
            else if (Mathf.Abs(moveY) > Mathf.Abs(moveX)) moveX = 0;
            // end mobile

            movement = new Vector2(moveX, moveY);

            animator.SetBool("moveSide", false); animator.SetBool("moveUp", false); animator.SetBool("moveDown", false);

            if (movement != Vector2.zero)
            {
                if (moveY > 0) { animator.SetBool("moveUp", true); facing = FacingDirection.Up; }
                else if (moveY < 0) { animator.SetBool("moveDown", true); facing = FacingDirection.Down; }
                else { animator.SetBool("moveSide", true); facing = moveX > 0 ? FacingDirection.Right : FacingDirection.Left; }

                stepTimer -= Time.deltaTime;
                if (stepTimer <= 0)
                {
                    if (footstepSource != null)
                    {
                        if (isInWater && waterSound != null)
                        {
                            footstepSource.clip = waterSound;
                            footstepSource.Play();
                        }
                        else if (!isInWater && landStepSound != null)
                        {
                            footstepSource.clip = landStepSound;
                            footstepSource.Play();
                        }
                    }
                    stepTimer = stepInterval;
                }
            }
            else
            {
                stepTimer = 0f;
                if (footstepSource != null && footstepSource.isPlaying)
                {
                    footstepSource.Stop();
                }
            }

            spriteRenderer.flipX = (facing == FacingDirection.Left);
        }

        void FixedUpdate()
        {
            if (!isPicking) rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }

        void StartPick()
        {
            isPicking = true;
            movement = Vector2.zero;
            GetComponent<FishCollector>()?.TryInteract();

            switch (facing)
            {
                case FacingDirection.Left:
                case FacingDirection.Right:
                    animator.SetTrigger("pickSide");
                    break;
                case FacingDirection.Up:
                    animator.SetTrigger("pickUp");
                    break;
                case FacingDirection.Down:
                    animator.SetTrigger("pickDown");
                    break;
            }
            Invoke("EndPick", 0.5f);
        }

        public void EndPick()
        {
            isPicking = false;
            animator.ResetTrigger("pickSide"); animator.ResetTrigger("pickUp"); animator.ResetTrigger("pickDown");
        }

        void LateUpdate()
        {
            if (spriteRenderer.sprite == null) return;
            Vector2 currentSize = spriteRenderer.sprite.bounds.size;
            if (currentSize.x == 0 || currentSize.y == 0) return;
            float scaleX = baseSpriteSize.x / currentSize.x;
            float scaleY = baseSpriteSize.y / currentSize.y;
            transform.localScale = new Vector3(baseSceneScale.x * scaleX, baseSceneScale.y * scaleY, baseSceneScale.z);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Water"))
            {
                isInWater = true;
                moveSpeed = waterSpeed;
                if (audioSource != null && waterSound != null) audioSource.PlayOneShot(waterSound);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Water"))
            {
                isInWater = false;
                moveSpeed = originalSpeed;
                if (footstepSource != null) footstepSource.Stop();
            }
        }

        //public void OnInteractButton_Pressed()
        //{
        //    // 1. Next thoại nếu đang nói chuyện
        //    if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
        //    {
        //        DialogueManager.instance.DisplayNextSentence();
        //        if (footstepSource != null && footstepSource.isPlaying) footstepSource.Stop();
        //        return;
        //    }

        //    // 2. Tìm NPC ở gần
        //    SV.NPC_Dialogue[] allNPCs = FindObjectsByType<SV.NPC_Dialogue>(FindObjectsSortMode.None);
        //    foreach (var npc in allNPCs) { npc.TriggerDialogue(); }

        //    // 3. Tìm ĐỐNG RƠM ở gần
        //    SV.DongRomInteract[] domRoms = FindObjectsByType<SV.DongRomInteract>(FindObjectsSortMode.None);
        //    foreach (var rom in domRoms) { rom.TriggerInteract(); }

        //    // 4. Tìm CHÌA KHÓA ở gần
        //    SV.KeyItem[] keys = FindObjectsByType<SV.KeyItem>(FindObjectsSortMode.None);
        //    foreach (var key in keys) { key.TriggerInteract(); }

        //    // 5. Tìm CỔNG KHÓA ở gần
        //    SV.LockedGateInteract[] gates = FindObjectsByType<SV.LockedGateInteract>(FindObjectsSortMode.None);
        //    foreach (var gate in gates) { gate.TriggerInteract(); }

        //    // 6. Hành động nhặt (bắt cá) gốc
        //    if (!isPicking)
        //    {
        //        StartPick();
        //    }
        //}

        public void OnInteractButton_Pressed()
        {
            // 1. Next thoại nếu đang nói chuyện
            if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
            {
                DialogueManager.instance.DisplayNextSentence();
                if (footstepSource != null && footstepSource.isPlaying) footstepSource.Stop();
                return;
            }

            // 2. TÌM VÀ KÍCH HOẠT CÁC OBJECT XUNG QUANH
            SV.NPC_Dialogue[] allNPCs = FindObjectsByType<SV.NPC_Dialogue>(FindObjectsSortMode.None);
            foreach (var npc in allNPCs) { npc.TriggerDialogue(); }

            SV.DongRomInteract[] domRoms = FindObjectsByType<SV.DongRomInteract>(FindObjectsSortMode.None);
            foreach (var rom in domRoms) { rom.TriggerInteract(); }

            SV.KeyItem[] keys = FindObjectsByType<SV.KeyItem>(FindObjectsSortMode.None);
            foreach (var key in keys) { key.TriggerInteract(); }

            SV.LockedGateInteract[] gates = FindObjectsByType<SV.LockedGateInteract>(FindObjectsSortMode.None);
            foreach (var gate in gates) { gate.TriggerInteract(); }

            // Thêm 3 món đồ mới ở màn này
            SV.RockItem[] rocks = FindObjectsByType<SV.RockItem>(FindObjectsSortMode.None);
            foreach (var rock in rocks) { rock.TriggerInteract(); }

            SV.DistractTarget[] distracts = FindObjectsByType<SV.DistractTarget>(FindObjectsSortMode.None);
            foreach (var dist in distracts) { dist.TriggerInteract(); }

            SV.WellInteract[] wells = FindObjectsByType<SV.WellInteract>(FindObjectsSortMode.None);
            foreach (var well in wells) { well.TriggerInteract(); }

            // 3. Hành động nhặt (bắt cá) gốc
            if (!isPicking)
            {
                StartPick();
            }
        }
    }
}