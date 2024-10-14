using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Data;
using System.IO;
using System.Threading.Tasks;
using Oculus.Interaction.Locomotion;
using System.Linq.Expressions;

[Serializable]
public class GCInfo : MonoBehaviour
{
    public TMP_Text _title;
    public TMP_Text team0ScoreText;
    public TMP_Text team1ScoreText;
    public TMP_Text timerText;
    public TMP_Text scoreboardPlate;
    public TMP_Text gcToStringPlate;

    public GameObject rgrtInfoPlateP0_1;
    public GameObject rgrtInfoPlateP0_2;
    public GameObject rgrtInfoPlateP0_3;
    public GameObject rgrtInfoPlateP0_4;
    public GameObject rgrtInfoPlateP0_5;

    public GameObject rgrtInfoPlateP1_1;
    public GameObject rgrtInfoPlateP1_2;
    public GameObject rgrtInfoPlateP1_3;
    public GameObject rgrtInfoPlateP1_4;
    public GameObject rgrtInfoPlateP1_5;

    public TMP_Text player1_3Text;
    public TMP_Text player1_4Text;

    public TMP_Text rgrtInfoPlate;
    public TMP_Text rgrtInfoLastTime;

    public GameObject player0_1;
    public GameObject player0_2;
    public GameObject player0_3;
    public GameObject player0_4;
    public GameObject player0_5;
    public GameObject player0_6;
    public GameObject player0_7;
    public GameObject player0_8;
    public GameObject player0_9;
    public GameObject player0_10;
    public GameObject player0_11;
    public GameObject player0_12;
    public GameObject player0_13;
    public GameObject player0_14;
    public GameObject player0_15;
    public GameObject player0_16;
    public GameObject player0_17;
    public GameObject player0_18;
    public GameObject player0_19;
    public GameObject player0_20;
    public GameObject player1_1;
    public GameObject player1_2;
    public GameObject player1_3;
    public GameObject player1_4;
    public GameObject player1_5;
    public GameObject player1_6;
    public GameObject player1_7;
    public GameObject player1_8;
    public GameObject player1_9;
    public GameObject player1_10;
    public GameObject player1_11;
    public GameObject player1_12;
    public GameObject player1_13;
    public GameObject player1_14;
    public GameObject player1_15;
    public GameObject player1_16;
    public GameObject player1_17;
    public GameObject player1_18;
    public GameObject player1_19;
    public GameObject player1_20;

    public GameObject test_player;

    private Coroutine ballAgeCoroutine;

    public GameObject[] team0Players;
    public GameObject[] team1Players;

    public GameObject[] team0InfoPlates;
    public GameObject[] team1InfoPlates;

    public int[] team0Timestamps = { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 };
    public int[] team1Timestamps = { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 };

    [SerializeField]
    public GameControlData gameControlData = new GameControlData();
    [SerializeField]
    public GameControlReturnData gameControlReturnData = new GameControlReturnData();
    private bool updateReady = false;
    private bool returnUpdateReady = false;
    private bool receivePackets = true;

    // Use a wildcard address for the target
    IPAddress targetIpAddress = IPAddress.Any;

    // Ports for different types of messages
    int monitorPort = 3636;
    int controlPort = 3838;
    int returnDataPort = 3939;
    int forwardedStatusPort = 3940;
    // Start is called before the first frame update

    // Create UDP clients for each port
    UdpClient monitorRequestClient;
    UdpClient controlClient;
    UdpClient forwardedStatusClient;

    LogoController firstTeamLogoController;
    LogoController secondTeamLogoController;

    public Material cylinderMaterial;

    private const float MAX_BALLAGE = 5f;
    private const float BALL_RADIUS = 0.011f;
    private const float MIN_CYLINDER_RADIUS = 0.03f;

    float ballAge;

    void OnEnable(){
        Initialise();
    }


    void Initialise()
    {
        receivePackets = true;
        // Define the starting and ending points for each array
        Vector3 start1 = new Vector3(-2.7f, 0.25f, -1.74f);
        Vector3 end1 = new Vector3(-2.7f, 0.25f, 1.74f);

        Vector3 start2 = new Vector3(-2.7f, 0.25f, 1.74f);
        Vector3 end2 = new Vector3(2.7f, 0.25f, -1.74f);

        Vector3 start3 = new Vector3(2.7f, 0.25f, -1.74f);
        Vector3 end3 = new Vector3(2.7f, 0.25f, 1.74f);

        Vector3 start4 = new Vector3(2.7f, 0.25f, 1.74f);
        Vector3 end4 = new Vector3(-2.7f, 0.25f, -1.74f);

        // Start counting seconds
        StartCoroutine(IncrementTeamTimestamps());

        Debug.Log("DEBUG_DISPLAY:Start GCInfo");

        // Dynamically find and assign TMP_Text objects
        try
        {
            _title = GameObject.Find("TitleText").GetComponent<TMP_Text>();
            team0ScoreText = GameObject.Find("Team0ScoreText").GetComponent<TMP_Text>();
            team1ScoreText = GameObject.Find("Team1ScoreText").GetComponent<TMP_Text>();
            timerText = GameObject.Find("TimerText").GetComponent<TMP_Text>();
            scoreboardPlate = GameObject.Find("ScoreboardPlate").GetComponent<TMP_Text>();
            gcToStringPlate = GameObject.Find("GCToStringPlate").GetComponent<TMP_Text>();

        }
        catch
        {
            Debug.Log("GCINFO:Scoredboard not present");
        }

        try
        {

            // Dynamically find and assign player GameObjects
            player0_1 = GameObject.Find("Player0-1");
            player0_2 = GameObject.Find("Player0-2");
            player0_3 = GameObject.Find("Player0-3");
            player0_4 = GameObject.Find("Player0-4");
            player0_5 = GameObject.Find("Player0-5");
            player0_6 = GameObject.Find("Player0-6");
            player0_7 = GameObject.Find("Player0-7");
            player0_8 = GameObject.Find("Player0-8");
            player0_9 = GameObject.Find("Player0-9");
            player0_10 = GameObject.Find("Player0-10");
            player0_11 = GameObject.Find("Player0-11");
            player0_12 = GameObject.Find("Player0-12");
            player0_13 = GameObject.Find("Player0-13");
            player0_14 = GameObject.Find("Player0-14");
            player0_15 = GameObject.Find("Player0-15");
            player0_16 = GameObject.Find("Player0-16");
            player0_17 = GameObject.Find("Player0-17");
            player0_18 = GameObject.Find("Player0-18");
            player0_19 = GameObject.Find("Player0-19");
            player0_20 = GameObject.Find("Player0-20");
            player1_1 = GameObject.Find("Player1-1");
            player1_2 = GameObject.Find("Player1-2");
            player1_3 = GameObject.Find("Player1-3");
            player1_4 = GameObject.Find("Player1-4");
            player1_5 = GameObject.Find("Player1-5");
            player1_6 = GameObject.Find("Player1-6");
            player1_7 = GameObject.Find("Player1-7");
            player1_8 = GameObject.Find("Player1-8");
            player1_9 = GameObject.Find("Player1-9");
            player1_10 = GameObject.Find("Player1-10");
            player1_11 = GameObject.Find("Player1-11");
            player1_12 = GameObject.Find("Player1-12");
            player1_13 = GameObject.Find("Player1-13");
            player1_14 = GameObject.Find("Player1-14");
            player1_15 = GameObject.Find("Player1-15");
            player1_16 = GameObject.Find("Player1-16");
            player1_17 = GameObject.Find("Player1-17");
            player1_18 = GameObject.Find("Player1-18");
            player1_19 = GameObject.Find("Player1-19");
            player1_20 = GameObject.Find("Player1-20");

            team0Players = new GameObject[] { player0_1, player0_2, player0_3, player0_4, player0_5, player0_6, player0_7, player0_8, player0_9, player0_10, player0_11, player0_12, player0_13, player0_14, player0_15, player0_16, player0_17, player0_18, player0_19, player0_20 };
            team1Players = new GameObject[] { player1_1, player1_2, player1_3, player1_4, player1_5, player1_6, player1_7, player1_8, player1_9, player1_10, player1_11, player1_12, player1_13, player1_14, player1_15, player1_16, player1_17, player1_18, player1_19, player1_20 };

        }
        catch
        {
            Debug.Log("GCINFO:Players not present");
        }

        try
        {
            rgrtInfoPlateP0_1 = GameObject.Find("rgrtInfoPlateP0_1");
            rgrtInfoPlateP0_2 = GameObject.Find("rgrtInfoPlateP0_2");
            rgrtInfoPlateP0_3 = GameObject.Find("rgrtInfoPlateP0_3");
            rgrtInfoPlateP0_4 = GameObject.Find("rgrtInfoPlateP0_4");
            rgrtInfoPlateP0_5 = GameObject.Find("rgrtInfoPlateP0_5");
            rgrtInfoPlateP1_1 = GameObject.Find("rgrtInfoPlateP1_1");
            rgrtInfoPlateP1_2 = GameObject.Find("rgrtInfoPlateP1_2");
            rgrtInfoPlateP1_3 = GameObject.Find("rgrtInfoPlateP1_3");
            rgrtInfoPlateP1_4 = GameObject.Find("rgrtInfoPlateP1_4");
            rgrtInfoPlateP1_5 = GameObject.Find("rgrtInfoPlateP1_5");

            team0InfoPlates = new GameObject[] { rgrtInfoPlateP0_1, rgrtInfoPlateP0_2, rgrtInfoPlateP0_3, rgrtInfoPlateP0_4, rgrtInfoPlateP0_5 };

            team1InfoPlates = new GameObject[] { rgrtInfoPlateP1_1, rgrtInfoPlateP1_2, rgrtInfoPlateP1_3, rgrtInfoPlateP1_4, rgrtInfoPlateP1_5 };
        }
        catch
        {
            Debug.Log("GCINFO:rgrtInfoPlates not present");
        }


        controlClient = new UdpClient(controlPort);
        forwardedStatusClient = new UdpClient(forwardedStatusPort);
        IPEndPoint remoteEndPoint = new IPEndPoint(targetIpAddress, controlPort);
        byte[] receivedData = controlClient.Receive(ref remoteEndPoint);
        Debug.Log("GCINFO:" + remoteEndPoint.Address.ToString());
        monitorRequestClient = new UdpClient();
        byte[] packet = Encoding.ASCII.GetBytes("RGTr\0");
        monitorRequestClient.Send(packet, packet.Length, new IPEndPoint(remoteEndPoint.Address, monitorPort));
        if (_title != null)
        {
            _title.text = "Monitor request sent.";
        }
        else
        {
            Debug.Log("GCINFO:Monitor request sent.");
        }
        monitorRequestClient.Close();

        try
        {
            firstTeamLogoController = GameObject.Find("FirstTeamLogo").GetComponent<LogoController>();
        }
        catch
        {
            Debug.LogError("GCINFO:LogoController with the specified name not found.");
        }

        try
        {
            secondTeamLogoController = GameObject.Find("SecondTeamLogo").GetComponent<LogoController>();
        }
        catch
        {
            Debug.LogError("GCINFO:LogoController with the specified name not found.");
        }

        // Handle RGTr asynchronously
        Task.Run(() => HandleRGTrPacket("RGTr"));

        // Handle RGrt asynchronously
        Task.Run(() => HandleRGrtPacket("RGrt"));

        // Start the coroutine to randomly change the ballAge
        // ballAgeCoroutine = StartCoroutine(ChangeBallAge());
    }

    private IEnumerator ChangeBallAge()
    {
        while (true)
        {
            // Randomly generate a new ballAge between 1 and 5
            ballAge = UnityEngine.Random.Range(0, 5);

            // Wait for the specified interval before changing the ballAge again
            yield return new WaitForSeconds(2);
        }
    }
    void Update()
    {
        // float angle = ballAge;
        // Transform testPlayerTransform = test_player.transform;
        // testPlayerTransform.localPosition = new Vector3(0.5f, 0, 0.5f);
        // // Find the PlayerRotMarker child object
        // Transform testPlayerRotMarker = testPlayerTransform.Find("PlayerRotMarker");
        // Transform testBall = testPlayerTransform.Find("Ball");
        // Transform testCylinder = testPlayerTransform.Find("BallLaser");
        // LineRenderer lineRenderer = testPlayerTransform.Find("Line").GetComponent<LineRenderer>();

        // // Update the rotation (assuming pose[2] contains the rotation value in radians -> 0 to 2π)
        // float testrotationAngle = angle; // Get the rotation value

        // // Convert rotation to degrees and convert to clockwise rotation
        // float test1rotationAngleDegrees = testrotationAngle * -Mathf.Rad2Deg + 90;

        // Vector3 testrotationEuler = new Vector3(0, test1rotationAngleDegrees, 0); // Convert to degrees
        // testPlayerRotMarker.localRotation = Quaternion.Euler(testrotationEuler); // Set the rotation   

        // testBall.gameObject.SetActive(true);

        // // Convert robot's orientation angle from radians to degrees
        // float testrotationAngleDegrees = angle * Mathf.Rad2Deg;

        // // Calculate rotation matrix elements
        // float testcosTheta = Mathf.Cos(testrotationAngleDegrees * Mathf.Deg2Rad);
        // float testsinTheta = Mathf.Sin(testrotationAngleDegrees * Mathf.Deg2Rad);

        // float testballRelX = 2000 / 1000f;
        // float testballRelZ = 1000/ 1000f;

        // // Rotate the ball coordinates relative to the robot
        // float testballFieldX = testcosTheta * testballRelX - testsinTheta * testballRelZ;
        // float testballFieldZ = testsinTheta * testballRelX + testcosTheta * testballRelZ;

        // testBall.localPosition = new Vector3(testballFieldX, 0, testballFieldZ);

        // lineRenderer.SetPosition(0, new Vector3(testPlayerRotMarker.transform.position.x, testPlayerRotMarker.transform.position.y + .01f, testPlayerRotMarker.transform.position.z));
        // lineRenderer.SetPosition(1, testBall.transform.position);

        // float scale = 0.15f * (1 - ballAge / MAX_BALLAGE);
        // float alpha  = Mathf.Lerp(.75f, .2f, -(ballAge / MAX_BALLAGE));
        // Debug.Log("ballage: " + ballAge + " scale: " + scale);
        // lineRenderer.transform.localScale = new Vector3(scale, scale, scale);
        // lineRenderer.material.color = new Color(1,0,1,alpha);


        // // Draw the cylinder from the robot's rotMarker
        // Vector3 teststart = testPlayerRotMarker.transform.position + new Vector3(0, 0, 0);
        // Vector3 testend = testPlayerRotMarker.transform.position + new Vector3(testballFieldX, 0, testballFieldZ);
        // DrawCylinder(testCylinder.gameObject, teststart, testend, ballAge / MAX_BALLAGE);
        
        // Debug.Log("Update()");
        if (updateReady)
        {
            if (_title == null)
            {
                try
                {
                    // Dynamically find and assign TMP_Text objects
                    _title = GameObject.Find("TitleText").GetComponent<TMP_Text>();
                    team0ScoreText = GameObject.Find("Team0ScoreText").GetComponent<TMP_Text>();
                    team1ScoreText = GameObject.Find("Team1ScoreText").GetComponent<TMP_Text>();
                    timerText = GameObject.Find("TimerText").GetComponent<TMP_Text>();
                }
                catch
                {
                    Debug.Log("GCINFO:Scoreboard not present");
                }
            }


            if (scoreboardPlate == null)
            {
                try
                {
                    scoreboardPlate = GameObject.Find("ScoreboardPlate").GetComponent<TMP_Text>();
                }
                catch
                {
                    Debug.Log("GCINFO:ScoreboardPlate not present");
                }
            }

            if (gcToStringPlate == null)
            {
                try
                {
                    gcToStringPlate = GameObject.Find("GCToStringPlate").GetComponent<TMP_Text>();
                }
                catch
                {
                    Debug.Log("GCINFO:GCToStringPlate not present");
                }
            }

            if (firstTeamLogoController == null)
            {
                try
                {
                    firstTeamLogoController = GameObject.Find("FirstTeamLogo").GetComponent<LogoController>();
                    secondTeamLogoController = GameObject.Find("SecondTeamLogo").GetComponent<LogoController>();
                }
                catch
                {
                    Debug.Log("GCINFO:LogoControllers not present");
                }
            }


            if (_title != null)
            {
                _title.text = gameControlData.ToString();
            }

            if (scoreboardPlate != null)
            {
                scoreboardPlate.text = gameControlData.getScoreBoard();
            }

            if (gcToStringPlate != null)
            {
                gcToStringPlate.text = "<align=left><mspace=.8em>" + gameControlData.ToString();
            }

            // Update the Team Logos
            if (firstTeamLogoController != null)
            {
                firstTeamLogoController.SetTeamNumber(gameControlData.team[0].teamNumber);
                secondTeamLogoController.SetTeamNumber(gameControlData.team[1].teamNumber);
            }

            // Update the Team colours and disable penalised robots
            for (int playerNumber = 0; playerNumber < team0Players.Length; ++playerNumber)
            {
                UpdatePlayer(playerNumber, 0);
            }

            for (int playerNumber = 0; playerNumber < team1Players.Length; ++playerNumber)
            {
                UpdatePlayer(playerNumber, 1);
            }
            


            if (team0ScoreText != null)
            {
                UpdateScores();
                UpdateTimer();
            }

            updateReady = false;
        }

        if (returnUpdateReady)
        {
            Debug.Log("DEBUG_DISPLAY: returnUpdateReady");

            if (player0_1 == null)
            {
                try
                {
                    // Dynamically find and assign player GameObjects
                    player0_1 = GameObject.Find("Player0-1");
                    player0_2 = GameObject.Find("Player0-2");
                    player0_3 = GameObject.Find("Player0-3");
                    player0_4 = GameObject.Find("Player0-4");
                    player0_5 = GameObject.Find("Player0-5");
                    player0_6 = GameObject.Find("Player0-6");
                    player0_7 = GameObject.Find("Player0-7");
                    player0_8 = GameObject.Find("Player0-8");
                    player0_9 = GameObject.Find("Player0-9");
                    player0_10 = GameObject.Find("Player0-10");
                    player0_11 = GameObject.Find("Player0-11");
                    player0_12 = GameObject.Find("Player0-12");
                    player0_13 = GameObject.Find("Player0-13");
                    player0_14 = GameObject.Find("Player0-14");
                    player0_15 = GameObject.Find("Player0-15");
                    player0_16 = GameObject.Find("Player0-16");
                    player0_17 = GameObject.Find("Player0-17");
                    player0_18 = GameObject.Find("Player0-18");
                    player0_19 = GameObject.Find("Player0-19");
                    player0_20 = GameObject.Find("Player0-20");
                    player1_1 = GameObject.Find("Player1-1");
                    player1_2 = GameObject.Find("Player1-2");
                    player1_3 = GameObject.Find("Player1-3");
                    player1_4 = GameObject.Find("Player1-4");
                    player1_5 = GameObject.Find("Player1-5");
                    player1_6 = GameObject.Find("Player1-6");
                    player1_7 = GameObject.Find("Player1-7");
                    player1_8 = GameObject.Find("Player1-8");
                    player1_9 = GameObject.Find("Player1-9");
                    player1_10 = GameObject.Find("Player1-10");
                    player1_11 = GameObject.Find("Player1-11");
                    player1_12 = GameObject.Find("Player1-12");
                    player1_13 = GameObject.Find("Player1-13");
                    player1_14 = GameObject.Find("Player1-14");
                    player1_15 = GameObject.Find("Player1-15");
                    player1_16 = GameObject.Find("Player1-16");
                    player1_17 = GameObject.Find("Player1-17");
                    player1_18 = GameObject.Find("Player1-18");
                    player1_19 = GameObject.Find("Player1-19");
                    player1_20 = GameObject.Find("Player1-20");

                    team0Players = new GameObject[] { player0_1, player0_2, player0_3, player0_4, player0_5, player0_6, player0_7, player0_8, player0_9, player0_10, player0_11, player0_12, player0_13, player0_14, player0_15, player0_16, player0_17, player0_18, player0_19, player0_20 };
                    team1Players = new GameObject[] { player1_1, player1_2, player1_3, player1_4, player1_5, player1_6, player1_7, player1_8, player1_9, player1_10, player1_11, player1_12, player1_13, player1_14, player1_15, player1_16, player1_17, player1_18, player1_19, player1_20 };
                }
                catch
                {
                    Debug.Log("GCINFO:Players not present");
                }

            }
            try
            {
                Debug.Log("Trying to find InfoPlate Objects");
                rgrtInfoPlateP0_1 = GameObject.Find("rgrtInfoPlateP0_1");
                rgrtInfoPlateP0_2 = GameObject.Find("rgrtInfoPlateP0_2");
                rgrtInfoPlateP0_3 = GameObject.Find("rgrtInfoPlateP0_3");
                rgrtInfoPlateP0_4 = GameObject.Find("rgrtInfoPlateP0_4");
                rgrtInfoPlateP0_5 = GameObject.Find("rgrtInfoPlateP0_5");
                rgrtInfoPlateP1_1 = GameObject.Find("rgrtInfoPlateP1_1");
                rgrtInfoPlateP1_2 = GameObject.Find("rgrtInfoPlateP1_2");
                rgrtInfoPlateP1_3 = GameObject.Find("rgrtInfoPlateP1_3");
                rgrtInfoPlateP1_4 = GameObject.Find("rgrtInfoPlateP1_4");
                rgrtInfoPlateP1_5 = GameObject.Find("rgrtInfoPlateP1_5");

                team0InfoPlates = new GameObject[] { rgrtInfoPlateP0_1, rgrtInfoPlateP0_2, rgrtInfoPlateP0_3, rgrtInfoPlateP0_4, rgrtInfoPlateP0_5 };

                team1InfoPlates = new GameObject[] { rgrtInfoPlateP1_1, rgrtInfoPlateP1_2, rgrtInfoPlateP1_3, rgrtInfoPlateP1_4, rgrtInfoPlateP1_5 };
                Debug.Log("InfoPlates found");
            }
            catch
            {
                Debug.Log("GCINFO:rgrtInfoPlates not present");
            }


            if (player0_1 != null && gameControlReturnData != null)
            {

                Debug.Log("GCINFO:Moving players based on gameControlReturnData ");
                Debug.Log("DEBUG_DISPLAY: returnUpdateReady for player: " + gameControlReturnData.playerNum.ToString());
                int playerIndex = gameControlReturnData.playerNum - 1;
                if (gameControlReturnData.teamNumValid)
                {

                    Debug.Log("GCINFO:PLayer index:" + playerIndex);
                    GameObject currentPlayer;
                    GameObject currentPlayerPlate;

                    int poseMultiple = 1;

                    if (gameControlReturnData.teamNum == gameControlData.team[0].teamNumber)
                    {
                        // Reset time since last packet
                        team0Timestamps[playerIndex] = 0;

                        currentPlayer = team0Players[playerIndex];
                        try
                        {
                            currentPlayerPlate = team0InfoPlates[playerIndex];
                        }
                        catch
                        {
                            Debug.Log("Error setting current PlayerPlate");
                            currentPlayerPlate = null;
                        }

                    }
                    else
                    {
                        // Reset time since last packet
                        team1Timestamps[playerIndex] = 0;

                        currentPlayer = team1Players[playerIndex];
                        try
                        {
                            currentPlayerPlate = team1InfoPlates[playerIndex];
                        }
                        catch
                        {
                            Debug.Log("Error setting current PlayerPlate");
                            currentPlayerPlate = null;
                        }
                        poseMultiple = -1;
                    }

                    Debug.Log("GCINFO:" + currentPlayer);

                    if (currentPlayerPlate != null)
                    {
                        currentPlayerPlate.GetComponent<TMP_Text>().text = gameControlReturnData.ToString();
                    }

                    // Check if pose data is valid before updating the position
                    if (currentPlayer != null)
                    {
                        // Get the Transform component of the player
                        Transform playerTransform = currentPlayer.transform;

                        // Check if pose data is valid before updating the position
                        if (gameControlReturnData.poseValid)
                        {
                            // Update the position
                            float xPosition = poseMultiple * gameControlReturnData.pose[0] / 1000;
                            float zPosition = poseMultiple * gameControlReturnData.pose[1] / 1000;
                            playerTransform.localPosition = new Vector3(xPosition, 0, zPosition);
                            // Find the PlayerRotMarker child object
                            Transform playerRotMarker = playerTransform.Find("PlayerRotMarker");
                            Transform ball = playerTransform.Find("Ball");
                            Transform cylinder = playerTransform.Find("BallLaser");

                    
                            if (playerRotMarker != null)
                            {
                                // Update the rotation (assuming pose[2] contains the rotation value in radians -> 0 to 2π)
                                float rotationAngle = gameControlReturnData.pose[2]; // Get the rotation value

                                // Convert rotation to degrees and convert to clockwise rotation
                                float rotationAngleDegrees = rotationAngle * -Mathf.Rad2Deg + 90;

                                Vector3 rotationEuler = new Vector3(0, rotationAngleDegrees, 0); // Convert to degrees
                                playerRotMarker.localRotation = Quaternion.Euler(rotationEuler); // Set the rotation
                            }
                            else
                            {
                                Debug.LogError("GCINFO:PlayerRotMarker not found as child of player object");
                            }

                            if (ball != null){
                                if (gameControlReturnData.ballAge >= 0 && gameControlReturnData.ballAge < MAX_BALLAGE)
                                {
                                    ball.gameObject.SetActive(true);

                                    // Convert robot's orientation angle from radians to degrees
                                    float rotationAngleDegrees = gameControlReturnData.pose[2] * Mathf.Rad2Deg;

                                    // Calculate rotation matrix elements
                                    float cosTheta = Mathf.Cos(rotationAngleDegrees * Mathf.Deg2Rad);
                                    float sinTheta = Mathf.Sin(rotationAngleDegrees * Mathf.Deg2Rad);

                                    float ballRelX = gameControlReturnData.ball[0] / 1000f;
                                    // float ballRelX = gameControlReturnData.ball[0];
                                    float ballRelZ = gameControlReturnData.ball[1] / 1000f;
                                    // float ballRelZ = gameControlReturnData.ball[1];

                                    // Rotate the ball coordinates relative to the robot
                                    float ballFieldX = cosTheta * ballRelX - sinTheta * ballRelZ;
                                    float ballFieldZ = sinTheta * ballRelX + cosTheta * ballRelZ;

                                    ball.localPosition = new Vector3(ballFieldX, 0, ballFieldZ);
                                    Debug.Log("Ball field pos: " + ballFieldX + " , " + ballFieldZ);

                                    // Draw the cylinder from the robot's rotMarker
                                    // Vector3 start = playerRotMarker.transform.position + new Vector3(0, 0, 0);
                                    // Vector3 start = playerRotMarker.transform.localPosition;
                                    Vector3 start = new Vector3(0, .02f ,0);
                                    // Vector3 end = playerRotMarker.transform.position + new Vector3(poseMultiple * ballFieldX, 0, poseMultiple * ballFieldZ);
                                    Vector3 end = new Vector3(ballFieldX, .02f, ballFieldZ);
                                    // Draw the cylinder
                                    // DrawCylinder(cylinder.gameObject, start, end, gameControlReturnData.ballAge / MAX_BALLAGE);

                                    try {
                                        LineRenderer lineRenderer = playerTransform.Find("Line").GetComponent<LineRenderer>();
                                        
                                        lineRenderer.SetPosition(0, new Vector3(playerRotMarker.transform.position.x, playerRotMarker.transform.position.y + .01f, playerRotMarker.transform.position.z));
                                        lineRenderer.SetPosition(1, ball.transform.position);

                                        float scale = 0.15f * (1 - ballAge / MAX_BALLAGE);
                                        float alpha  = Mathf.Lerp(.75f, .2f, -(ballAge / MAX_BALLAGE));
                                        Debug.Log("ballage: " + ballAge + " scale: " + scale);
                                        lineRenderer.transform.localScale = new Vector3(scale, scale, scale);
                                        lineRenderer.material.color = new Color(1,0,1,alpha);
                                    } catch {
                                        Debug.LogError("failed to render line to ball");
                                    }

                                }
                                else
                                {
                                    cylinder.gameObject.SetActive(false);
                                    ball.gameObject.SetActive(false);
                                }
                            }
                            else
                            {
                                Debug.LogError("GCINFO:Ball not found as child of player object");
                            }
                        }
                    }

                }
            }
            else
            {
                Debug.Log("GCINFO:Not valid playernumber");
            }
            returnUpdateReady = false;
        }

        // Hide any players that haven't sent an update for 4 seconds
        for (int i = 0; i < TeamInfo.MAX_NUM_PLAYERS; i++)
        {
            if (team0Players[i] != null && team0Timestamps[i] > 4)
            {
                team0Players[i].SetActive(false);
            }
            else if (team0Players[i] != null)
            {
                if (gameControlData != null){
                    if (gameControlData.team[0].player[i].penalty != PlayerInfo.PENALTY_NONE &&
                        gameControlData.team[0].player[i].penalty != PlayerInfo.PENALTY_SPL_ILLEGAL_MOTION_IN_SET){
                            team0Players[i].SetActive(false);
                    }
                    else{
                        team0Players[i].SetActive(true);
                    } 
                }
            }



            if (team1Players[i] != null && team1Timestamps[i] > 4)
            {
                team1Players[i].SetActive(false);
            }
            else if (team1Players[i] != null)
            {
                if (gameControlData != null){
                    if (gameControlData.team[1].player[i].penalty != PlayerInfo.PENALTY_NONE &&
                        gameControlData.team[1].player[i].penalty != PlayerInfo.PENALTY_SPL_ILLEGAL_MOTION_IN_SET){
                            team1Players[i].SetActive(false);
                    }
                    else{
                        team1Players[i].SetActive(true);
                    } 
                }
            }
        }
    }

    private void UpdatePlayer(int playerNumber, int teamNumber)
    {
        Color teamColour;
        string teamColourName = TeamInfo.GetTeamColorName(gameControlData.team[teamNumber].fieldPlayerColor);
        GameObject[] playerArray;


        if (teamNumber == 0){
            playerArray = team0Players;
        }
        else {
            playerArray = team1Players;
        }

        // if (gameControlData.team[teamNumber].player[playerNumber].penalty != PlayerInfo.PENALTY_NONE &&
        //     gameControlData.team[teamNumber].player[playerNumber].penalty != PlayerInfo.PENALTY_SPL_ILLEGAL_MOTION_IN_SET){
        //     if (playerArray[playerNumber] != null){
        //         playerArray[playerNumber].SetActive(false);
        //         Debug.Log("Would have turned off player: " + playerNumber + " of team: " + teamNumber);
        //         Debug.Log(gameControlData.team[teamNumber].player[playerNumber].penalty);
        //     }
        // }
            
        // else {
        //     if (playerArray[playerNumber] != null){
        //         playerArray[playerNumber].SetActive(true);
        //         Debug.Log(gameControlData.team[teamNumber].player[playerNumber].penalty);
        //     }
        // }
        if (ColorUtility.TryParseHtmlString(teamColourName, out teamColour))
        {
            // Update the playerPosMarker
            try
            {
                Transform playerPosMarker = playerArray[playerNumber].transform.Find("PlayerPosMarker");
                if (playerPosMarker != null)
                {
                    Transform playerPosMarkerChild = playerPosMarker.Find("PlayerPosMarker");
                    if (playerPosMarkerChild != null)
                    {
                        Renderer playerPosRenderer = playerPosMarkerChild.GetComponent<Renderer>();
                        if (playerPosRenderer != null)
                        {
                            // Set the material color to the team color
                            playerPosRenderer.material.color = teamColour;
                            Debug.Log("Colour changed to: " + teamColourName);
                        }
                        else
                        {
                            Debug.LogWarning("no renderer");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("no grandhild");
                    }
                }
                else
                {
                    Debug.LogWarning("no firstchild");
                }
            }
            catch
            {
                Debug.Log("Error setting player Colour ");
            }

            // Update the playerRotMarker
            try
            {
                Transform playerRotMarker = playerArray[playerNumber].transform.Find("PlayerRotMarker");
                Debug.Log("Updating playerrotmarker team " + teamNumber);
                if (playerRotMarker != null)
                {
                    Transform playerRotMarkerChild = playerRotMarker.Find("PlayerRotMarker");
                    if (playerRotMarkerChild != null)
                    {
                        Renderer playerRotRenderer = playerRotMarkerChild.GetComponent<Renderer>();
                        if (playerRotRenderer != null)
                        {
                            // Set the material color to the team color
                            playerRotRenderer.material.color = teamColour;
                            playerRotRenderer.material.SetColor("_EmissionColor", teamColour);
                            Debug.Log("Rot team" + teamNumber + " changed to: " + teamColourName);
                        }
                        else
                        {
                            Debug.LogWarning("no renderer");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("no grandhild");
                    }
                }
                else
                {
                    Debug.LogWarning("no firstchild");
                }
            }
            catch
            {
                Debug.Log("Error setting player Colour ");
            }
        }
        else
        {
            Debug.LogError("Failed to parse team color name: " + teamColourName);
        }
    }

    async Task HandleRGTrPacket(string headerMagic)
    {
        while (receivePackets)
        {
            if (controlClient != null)
            {
                Debug.Log("Waiting for data");
                GameControlData data = await Task.Run(() => ReceiveMessages(controlClient, targetIpAddress, controlPort, headerMagic, "Regular Control"));
                if (data != null)
                {
                    Debug.Log("Data Received");
                    // Handle the data accordingly
                    gameControlData = data;
                    // Debug.Log($"Received {headerMagic}: {gameControlData.ToString()}");
                    updateReady = true;
                }
            }
        }
    }

    async Task HandleRGrtPacket(string headerMagic)
    {
        while (receivePackets)
        {
            // Debug.Log("Start HandleRGrtPacket");
            if (forwardedStatusClient != null)
            {
                // Debug.Log("====> await Task.Run");
                GameControlReturnData data = await Task.Run(() => ReceiveMessageGCReturnDataReceive(forwardedStatusClient, targetIpAddress, forwardedStatusPort, headerMagic, "GC Return Data"));
                // Debug.Log("====> Got some data");
                if (data != null)
                {
                    gameControlReturnData = data;
                    // Debug.Log($"Received {headerMagic}:\n{data.ToString()}");
                    returnUpdateReady = true;
                }
            }
        }
    }

    void OnDestroy()
    {
        EndTasks();
    }

    void OnDisable(){
        EndTasks();
    }

    void EndTasks(){
        receivePackets = false;
        // Debug.Log("OnDestroy Called!");

        if (controlClient != null)
        {
            controlClient.Close();
        }

        if (monitorRequestClient != null)
        {
            monitorRequestClient.Close();
        }

        if (forwardedStatusClient != null)
        {
            forwardedStatusClient.Close();
        }

        if (ballAgeCoroutine != null)
        {
            StopCoroutine(ballAgeCoroutine);
        }
    }

    static GameControlData ReceiveMessages(UdpClient controlClient, IPAddress targetIpAddress, int targetPort, string headerMagic, string messageType)
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(targetIpAddress, targetPort);
        try
        {
            byte[] receivedData = controlClient.Receive(ref remoteEndPoint);
            string receivedHeaderMagic = Encoding.ASCII.GetString(receivedData, 0, 4);
            string receivedMessage = Encoding.ASCII.GetString(receivedData);
            int byteArrayLength = receivedMessage.Length;
            string decodedMessage = Encoding.ASCII.GetString(receivedData, 5, byteArrayLength - 5);
            // Debug.Log($"{receivedHeaderMagic} Received: {receivedMessage} Extracted: {decodedMessage}");

            GameControlData data = new GameControlData();

            using (MemoryStream memoryStream = new MemoryStream(receivedData))
            {
                if (data.FromByteArray(memoryStream))
                {
                    if (data.isTrueData)
                    {
                        StringBuilder output = new StringBuilder();
                        output.AppendLine(data.ToString());
                        output.AppendLine("#############################################");
                        // string teamName0 = GetTeamName(data.team[0].teamNumber);
                        string teamName0 = "teamName0";
                        output.AppendLine($"Team Number: {data.team[0].teamNumber} Name: {teamName0}");
                        output.AppendLine($"Goals: {data.team[0].score}");
                        List<int> playerNums0 = new List<int> { 1, 2, 3, 4, 5 };
                        output.AppendLine(data.team[0].PlayersToSring(playerNums0));
                        output.AppendLine("#############################################");
                        // string teamName1 = GetTeamName(data.team[1].teamNumber);
                        string teamName1 = "teamName1";
                        output.AppendLine($"Team Number: {data.team[1].teamNumber} Name: {teamName1}");
                        output.AppendLine($"Goals: {data.team[1].score}");
                        List<int> playerNums1 = new List<int> { 1, 2, 3, 4, 5 };
                        output.AppendLine(data.team[1].PlayersToSring(playerNums1));
                        output.AppendLine("#############################################");
                        // Debug.Log(output.ToString());
                        return (data);
                    }
                }
                return null;
            }
        }
        catch (Exception err)
        {
            Debug.LogError("GCINFO:Caught Exception during ReceiveMessages");
            Debug.LogError(err.ToString());
            return null;
        }
    }

    static GameControlReturnData ReceiveMessageGCReturnDataReceive(UdpClient controlClient, IPAddress targetIpAddress, int targetPort, string headerMagic, string messageType)
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(targetIpAddress, targetPort);
        try
        {
            byte[] receivedData = controlClient.Receive(ref remoteEndPoint);
            string receivedHeaderMagic = Encoding.ASCII.GetString(receivedData, 0, 4);
            string receivedMessage = Encoding.ASCII.GetString(receivedData);
            int byteArrayLength = receivedMessage.Length;
            string decodedMessage = Encoding.ASCII.GetString(receivedData, 5, byteArrayLength - 5);
            // Console.WriteLine($"{receivedHeaderMagic} WOW got a RGrt: {receivedMessage} Extracted: {decodedMessage}");
            Console.WriteLine("GCINFO:====> Got some Data");
            GameControlReturnData data = new GameControlReturnData();

            using (MemoryStream memoryStream = new MemoryStream(receivedData))
            {
                if (data.FromByteArray(memoryStream))
                {
                    return data;
                }
                Debug.LogError("GCINFO:====> Failed to parse memory stream");
                return null;
            }
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
            return null;
        }
    }
    static string GetTeamName(byte teamNumber)
    {
        Console.WriteLine("GCINFO:getTeamName");

        string[] teamNames = Teams.GetNames(true);

        if (teamNumber != null)
        {
            if (teamNumber < teamNames.Length && teamNames[teamNumber] != null)
            {
                return "Team " + teamNames[teamNumber];
            }
            else
            {
                return "Unknown" + teamNumber + ")";
            }
        }
        else
        {
            return "Unknown";
        }
    }

    void UpdateScores()
    {
        // Check if gameControlData is not null before accessing its members
        if (gameControlData != null)
        {
            // Assuming team[0] and team[1] represent the two teams
            if (gameControlData.team != null && gameControlData.team.Length >= 2)
            {
                // Debug.Log("Team0 Score!:" + gameControlData.team[0].score.ToString());
                team0ScoreText.text = gameControlData.team[0].score.ToString();
                team1ScoreText.text = gameControlData.team[1].score.ToString();
            }
            else
            {
                Debug.LogError("GCINFO:gameControlData.team is null or has insufficient length.");
            }
        }
        else
        {
            Debug.LogError("GCINFO:gameControlData is null.");
        }
    }

    void UpdateTimer()
    {
        // Check if gameControlData is not null before accessing its members
        if (gameControlData != null)
        {
            // Assuming secsRemaining represents the time in seconds
            // Debug.Log("Timer!:" + gameControlData.secsRemaining);
            int minutes = gameControlData.secsRemaining / 60;
            int seconds = Math.Abs(gameControlData.secsRemaining % 60);

            timerText.text = $"{minutes:D2}:{seconds:D2}";
        }
        else
        {
            Debug.LogError("GCINFO:gameControlData is null.");
        }
    }

    // Coroutine to increment timmestamps every second
    private IEnumerator IncrementTeamTimestamps()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second

            // Increment each element in team0Timestamps
            for (int i = 0; i < team0Timestamps.Length; i++)
            {
                team0Timestamps[i]++;
                team1Timestamps[i]++;
            }
        }
    }


    private void DrawCylinder(GameObject cylinder, Vector3 start, Vector3 end, float relativeAge)
    {
        float distance = Vector3.Distance(start, end);
        Vector3 scale = new Vector3(MIN_CYLINDER_RADIUS * (1 - relativeAge), distance / 2, MIN_CYLINDER_RADIUS * (1 - relativeAge));

        cylinder.transform.localPosition = (start + end) / 2;
        cylinder.transform.up = (end - start).normalized;
        cylinder.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        Debug.Log("Current y angle: " + cylinder.transform.eulerAngles.y);
        // cylinder.transform.rotation = Quaternion.Euler(-90f, cylinder.transform.rotation.eulerAngles.y, cylinder.transform.rotation.eulerAngles.z);
        // cylinder.transform.rotation(new Vector3(0,90,0));
        cylinder.transform.rotation = Quaternion.Euler(cylinder.transform.rotation.eulerAngles.x, cylinder.transform.rotation.eulerAngles.y + 90, cylinder.transform.rotation.eulerAngles.z);
        Debug.Log("New y angle: " + cylinder.transform.eulerAngles.y);
        cylinder.SetActive(true);

        // Adjust transparency based on relative age
        Renderer renderer = cylinder.GetComponent<Renderer>();
        Material material = renderer.material;
        Color color = material.color;
        color.a = Mathf.Lerp(.5f, .2f, -relativeAge);
        material.color = color;
    }

}