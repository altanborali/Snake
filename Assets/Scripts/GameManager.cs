using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

namespace SA
{

    public class GameManager : MonoBehaviour
    {

        public Button continueBtn;

        Vector3 fp;
        Vector3 lp;
        float dragDistance;

        public int maxHeight = 15;
        public int maxWidth = 17;

        public Color color1;
        public Color color2;
        public Color playerColor = Color.black;
        public Color appleColor = Color.red;
        Sprite playerSprite;

        public Transform cameraHolder;
        public Camera MainCamera;

        GameObject playerObj;
        GameObject appleObj;
        GameObject tailParent;

        Node playerNode;
        Node prevPlayerNode;
        Node appleNode;

        GameObject mapObject;
        SpriteRenderer mapRenderer;

        Node[,] grid;
        List<Node> availableNodes = new List<Node>();
        List<SpecialNode> tail = new List<SpecialNode>();

        bool up, left, right, down, tap;

        int currentScore;
        int highScore;

        public int gameOvercount;

        public bool isGameOver;
        public bool isFirstInput;

        float timer;

        Direciton targetDirection;
        Direciton curDirection;

        public Text currrentScoreText;
        public Text highScoreText;


        public enum Direciton
        {
            up, left, right, down
        }

        public UnityEvent onStart;
        public UnityEvent onGameOver;
        public UnityEvent firstInput;
        public UnityEvent onScore;

        public GameObject GameOverBanner;

        //GET INPUT TELEFON VE PC ARASI GEÇİŞ SAĞLAR  !!!!!!!!!!!!!!!!!!!!!!!!!

        #region Init

        void Start()
        {
            onStart.Invoke();
            MainCamOrthogroSize();

            if (MenuCtrl.instance.moveRate == 0.20f)
            {
                highScore = PlayerPrefs.GetInt("easy");
                Debug.Log("oldScore: " + highScore);
            }

            if (MenuCtrl.instance.moveRate == 0.12f)
            {
                highScore = PlayerPrefs.GetInt("normal");
            }

            if (MenuCtrl.instance.moveRate == 0.08f)
            {
                highScore = PlayerPrefs.GetInt("hard");
            }

            highScoreText.text = highScore.ToString() ;

        }

        void MainCamOrthogroSize()
        {
            //float oran = Screen.height / Screen.width;
            float w = Screen.height;
            float h = Screen.width;
            //Debug.Log("width: " + Screen.width);
            //Debug.Log("height: " + Screen.height);

            //Debug.Log("ratio: " + h/w);

            MainCamera.orthographicSize = 15 / (1.77f * ((h/w)));
            //Debug.Log("orto: " + MainCamera.orthographicSize);

        }

        public void StartNewGame()
        {

            ClearReferences();
            CreateMap();
            PlacePlayer();
            PlaceCamera();
            CreateApple();
            targetDirection = Direciton.right;  // bunu yazmazsan random başlıyor
            isGameOver = false;
            currentScore = 0;
            UpdateScore();
            AdsManager();
            StartCoroutine(TakeSs());
        }

        public void ClearReferences()
        {
            if(mapObject != null)
            Destroy(mapObject);

            if (playerObj != null)
                Destroy(playerObj);

            if (appleObj != null)
                Destroy(appleObj);

            foreach (var t in tail)
            {
                if(t.obj != null)
                Destroy(t.obj);
            }
            tail.Clear();
            availableNodes.Clear();
            grid = null;
        }

        void CreateMap()
        {
            mapObject = new GameObject("Map");
            mapRenderer = mapObject.AddComponent<SpriteRenderer>();
            //mapObject.transform.localScale = Vector3.one * 0.5f;

            grid = new Node[maxWidth, maxHeight];

            Texture2D txt = new Texture2D(maxWidth, maxHeight);


            for (int x = 0; x < maxWidth; x++)
            {

                for (int y = 0; y < maxHeight; y++)
                {
                    Vector3 tp = Vector3.zero;   //Camera holder pozisyonu için. Ama player ve apple ona göre değil.    // Node içinde new Vector3 tanımlıcaktı ama new yazmak performansı etkiliyormuş  //
                    tp.x = x;   //tp target position
                    tp.y = y;

                    Node n = new Node() // Player'ın hareketini etkiliyor
                    {
                        x = x,
                        y = y,
                        worldPosition = tp
                    };

                    grid[x, y] = n;

                    availableNodes.Add(n);

                    //Debug.Log("hoop" + availableNodes.Count);

                    #region Visual
                    if (x % 2 != 0)
                    {
                        if (y % 2 != 0)
                        {
                            txt.SetPixel(x, y, color1);
                        }
                        else
                        {
                            txt.SetPixel(x, y, color2);
                        }
                    }
                    else
                    {
                        if (y % 2 != 0)
                        {
                            txt.SetPixel(x, y, color2);
                        }
                        else
                        {
                            txt.SetPixel(x, y, color1);
                        }

                    }
                    #endregion Visual
                }
            }

            txt.filterMode = FilterMode.Point;

            txt.Apply();
            Rect rect = new Rect(0, 0, maxWidth, maxHeight);
            Sprite sprite = Sprite.Create(txt, rect, Vector2.zero, 1, 0, SpriteMeshType.FullRect);
            mapRenderer.sprite = sprite;
        }

        void PlacePlayer()
        {
            playerObj = new GameObject("Player");
            SpriteRenderer playerRender = playerObj.AddComponent<SpriteRenderer>();
            playerSprite = CreateSprite(playerColor);
            playerRender.sprite = playerSprite;
            playerRender.sortingOrder = 1;
            playerNode = GetNode(3, 3);

            PlacePlayerObject(playerObj, playerNode.worldPosition);
            playerObj.transform.localScale = Vector3.one * 1.2f;

            tailParent = new GameObject("tailParent");
            

        }

        void PlaceCamera()
        {
            Node n = GetNode(maxWidth / 2, maxHeight / 2);
            Vector3 p = n.worldPosition;
            p += Vector3.one * 0.5f;
            cameraHolder.position = p;
        }

        void CreateApple()
        {
            appleObj = new GameObject("Apple");
            SpriteRenderer appleRenderer = appleObj.AddComponent<SpriteRenderer>();
            appleRenderer.sprite = CreateSprite(appleColor);
            appleRenderer.sortingOrder = 1;
            RandomlyPlaceApple();
        }

        void AdsManager()
        {
            gameOvercount++;

            if (gameOvercount == 3)
            {
                if (Advertisement.IsReady("video"))
                {
                    Advertisement.Show("video");
                }
                gameOvercount = 0;
            }

        }

        #endregion

        #region Update

        private void Update()
        {

            GetInput();



            if (isGameOver)
            {

                //if (Input.GetKeyDown(KeyCode.R))  // PC için
                /*if(tap)  //telefon için
                {
                    onStart.Invoke();
                    
                }*/

                return;
            }
                
            

            if (isFirstInput)
            {

                SetPlayerDirection();

                timer += Time.deltaTime;
                if (timer > MenuCtrl.instance.moveRate)
                {
                    timer = 0;
                    curDirection = targetDirection;
                    MovePlayer();
                    
                }
            }
            else
            {
                if(up || down || left || right)  //hem telefon hem PC için  tap yapmıyoruz çünkü restart veya reklam tuşunca basınca direk oynuyor
                //if(tap)  
                {
                    SetPlayerDirection();
                    isFirstInput = true;
                    firstInput.Invoke();
                    
                }
            }
        }


        IEnumerator TakeSs()
        {
            int no = 1;
            string name = "2048.2732 gameview" + no + ".png";

            while (true)
            {
                yield return new WaitForSeconds(5);
                ScreenCapture.CaptureScreenshot(name);
                no++;
                name = "2048.2732 gameview" + no + ".png";
            }
        }

        void GetInput()
        {
            up = Input.GetButtonDown("Up");  //PC için
            //Debug.Log("Up" + Input.GetButtonDown("Up"));
            left = Input.GetButtonDown("Left");  //PC için
            right = Input.GetButtonDown("Right");  //PC için
            down = Input.GetButtonDown("Down");    //PC için 
            

            //telefon için aşağıda

            /*if (Input.touchCount == 1) // user is touching the screen with a single touch
            {
                Touch touch = Input.GetTouch(0); // get the touch
                if (touch.phase == TouchPhase.Began) //check for the first touch
                {
                    fp = touch.position;
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
                {
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
                {
                    lp = touch.position;  //last touch position. Ommitted if you use list

                    //Check if drag distance is greater than 20% of the screen height
                    if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                    {//It's a drag
                     //check if the drag is vertical or horizontal
                        if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                        {   //If the horizontal movement is greater than the vertical movement...
                            if ((lp.x > fp.x))  //If the movement was to the right)
                            {   //Right swipe
                                Debug.Log("Right Swipe");
                                right = true;
                                left = false;
                                up = false;
                                down = false;
                                tap = false;
                            }
                            else
                            {   //Left swipe
                                Debug.Log("Left Swipe");
                                right = false;
                                left = true;
                                up = false;
                                down = false;
                                tap = false;
                            }
                        }
                        else
                        {   //the vertical movement is greater than the horizontal movement
                            if (lp.y > fp.y)  //If the movement was up
                            {   //Up swipe
                                Debug.Log("Up Swipe");
                                right = false;
                                left = false;
                                up = true;
                                down = false;
                                tap = false;
                            }
                            else
                            {   //Down swipe
                                Debug.Log("Down Swipe");
                                right = false;
                                left = false;
                                up = false;
                                down = true;
                                tap = false;
                            }
                        }
                    }
                    else
                    {   //It's a tap as the drag distance is less than 20% of the screen height
                        Debug.Log("Tap");
                        right = false;
                        left = false;
                        up = false;
                        down = false;
                        tap = true;
                    }
                }
            } */  //buraya kadar

        }

        void SetPlayerDirection()
        {
            if (up)
            {
                SetDirection(Direciton.up);
            }
            else if (left)
            {
                SetDirection(Direciton.left);
            }
            else if (right)
            {
                SetDirection(Direciton.right);
            }
            else if (down)
            {
                SetDirection(Direciton.down);
            }

        }

        void SetDirection(Direciton d)
        {
            if (!isOpposite(d))
            {
                targetDirection = d;
            }
        }

        public void restartButton()
        {
            onStart.Invoke();
            SoundManager.Instance.Play("Button");
        }

        void HandleRewardedAdResult(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    //Debug.Log("Finished !!");
                    GameOverBanner.gameObject.SetActive(false);
                    isGameOver = false;
                    isFirstInput = false;
                    break;


                case ShowResult.Skipped:
                    //Debug.Log("Skipped !!");
                    GameOverBanner.gameObject.SetActive(false);
                    isGameOver = false;
                    isFirstInput = false;
                    break;


                case ShowResult.Failed:
                    //Debug.Log("Failed !!");
                    GameOverBanner.gameObject.SetActive(false);
                    isGameOver = false;
                    isFirstInput = false;
                    break;
            }
        }

        public void adButton()
        {
            isGameOver = false;
            isFirstInput = false;
            SoundManager.Instance.Play("Button");

            gameOvercount = 0;
            if (Advertisement.IsReady("rewardedVideo"))
            {
                Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleRewardedAdResult });
            }

        }


        public void MovePlayer()
        {

            int x = 0;
            int y = 0;

            switch (curDirection)
            {
                case Direciton.up:
                    y = 1;
                    break;
                case Direciton.left:
                    x = -1;
                    break;
                case Direciton.right:
                    x = 1;
                    break;
                case Direciton.down:
                    y = -1;
                    break;
            }

            Node targetNode = GetNode(playerNode.x + x, playerNode.y + y);
            if (targetNode == null)
            {
                //Game Over
                onGameOver.Invoke();
                SoundManager.Instance.Play("GameOver");

            }
            else
            {
                if (isTailNode(targetNode))
                {
                    //Game Over
                    onGameOver.Invoke();
                    SoundManager.Instance.Play("GameOver");
                }

                else
                {
                    bool isScore = false;

                    if (targetNode == appleNode)
                    {
                        isScore = true;
                        SoundManager.Instance.Play("Point");
                    }

                    Node previousNode = playerNode;
                    availableNodes.Add(previousNode);


                    if (isScore)
                    {
                        tail.Add(CreateTailNode(previousNode.x, previousNode.y));
                        availableNodes.Remove(previousNode);
                    }

                    MoveTail();

                    PlacePlayerObject(playerObj, targetNode.worldPosition);
                    playerNode = targetNode;
                    availableNodes.Remove(playerNode);

                    if (isScore)
                    {
                        
                        currentScore += 10;
                        if(currentScore >= highScore)
                        {
                            highScore = currentScore;
                        }

                        onScore.Invoke();

                        if (availableNodes.Count > 0)
                        {
                            RandomlyPlaceApple();
                        }
                        else
                        {
                            //You win
                        }
                    }
                }
            }

        }

        void MoveTail()
        {
            Node prevNode = null;

            for (int i = 0; i < tail.Count; i++)
            {
                SpecialNode p = tail[i];
                availableNodes.Add(p.node);

                if (i == 0)
                {
                    prevNode = p.node;
                    p.node = playerNode;
                }
                else
                {
                    Node prev = p.node;
                    p.node = prevNode;
                    prevNode = prev;
                }


                availableNodes.Remove(p.node);
                PlacePlayerObject(p.obj, p.node.worldPosition);
            }
        }

        #endregion


        #region Utilities

        public void loadScene(string sceneName)
        {
            SoundManager.Instance.Play("Button");
            if (sceneName.Equals("MenuScene"))
            {
                //SoundManager.Instance.MusicSource.Stop();
                SceneManager.LoadScene(sceneName);
            }

        }

        public void GameOver()
        {
            isGameOver = true;
            isFirstInput = false;
            int oldScore ;
            //StartCoroutine(TakeSs());

            if (MenuCtrl.instance.moveRate == 0.20f)
            {
                oldScore = PlayerPrefs.GetInt("easy");
                Debug.Log("oldScore: " + oldScore);

                if (highScore > oldScore || oldScore == 0)
                {
                    PlayerPrefs.SetInt("easy", highScore);
                }
            }

            if (MenuCtrl.instance.moveRate == 0.12f)
            {
                oldScore = PlayerPrefs.GetInt("normal");

                if (highScore > oldScore || oldScore == 0)
                {
                    PlayerPrefs.SetInt("normal", highScore);
                }
            }

            if (MenuCtrl.instance.moveRate == 0.08f)
            {
                oldScore = PlayerPrefs.GetInt("hard");

                if (highScore > oldScore || oldScore == 0)
                {
                    PlayerPrefs.SetInt("hard", highScore);
                }
            }
        }

        public void UpdateScore()
        {

            currrentScoreText.text = currentScore.ToString();
            highScoreText.text = highScore.ToString();
        }

        bool isOpposite(Direciton d)
        {
            switch (d)
            {
                default:
                case Direciton.up:
                    if (curDirection == Direciton.down)
                        return true;
                    else
                        return false;
                case Direciton.left:
                    if (curDirection == Direciton.right)
                        return true;
                    else
                        return false;
                case Direciton.right:
                    if (curDirection == Direciton.left)
                        return true;
                    else
                        return false;
                case Direciton.down:
                    if (curDirection == Direciton.up)
                        return true;
                    else
                        return false;
            }
        }

        bool isTailNode(Node n)
        {
            for (int i = 0; i < tail.Count; i++)
            {
                if (tail[i].node == n)
                {
                    return true;
                }
            }

            return false;
        }

        void PlacePlayerObject(GameObject obj, Vector3 pos)
        {
            pos += Vector3.one * 0.5f;
            obj.transform.position = pos;
        }

        void RandomlyPlaceApple()
        {
            int ran = Random.Range(0, availableNodes.Count);
            //Debug.Log("availablenodes count: " + availableNodes.Count);
            Node n = availableNodes[ran];
            PlacePlayerObject(appleObj, n.worldPosition);
            appleNode = n;
        }

        Node GetNode(int x, int y)
        {
            if (x < 0 || x > maxWidth - 1 || y < 0 || y > maxHeight - 1)
                return null;

            return grid[x, y];
        }

        SpecialNode CreateTailNode(int x, int y)
        {
            SpecialNode s = new SpecialNode();
            s.node = GetNode(x, y);
            s.obj = new GameObject();
            s.obj.transform.parent = tailParent.transform;
            s.obj.transform.position = s.node.worldPosition;
            s.obj.transform.localScale = Vector3.one * 0.95f;
            SpriteRenderer r = s.obj.AddComponent<SpriteRenderer>();
            r.sprite = playerSprite;
            r.sortingOrder = 1;

            return s;
        }

        Sprite CreateSprite(Color targetColor)
        {
            Texture2D txt = new Texture2D(1, 1);
            txt.SetPixel(0, 0, targetColor);
            txt.Apply();
            txt.filterMode = FilterMode.Point;
            Rect rect = new Rect(0, 0, 1, 1);
            return Sprite.Create(txt, rect, Vector2.one * 0.5f, 1, 0, SpriteMeshType.FullRect);   //burdaki vector2.one*0.5f i Vector2.zero yapınca da olabiliyo bide PlacePlayerPbject deki 0.5 i 1 yapınca da oluyor. !!
        }
        #endregion
    }
}
