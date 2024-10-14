using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

[Serializable]
public class PassiveGCInfo : MonoBehaviour
{

    UdpClient gcInfoClient;
    IPAddress targetIpAddress = IPAddress.Any;
    public TMP_Text passiveGCInfoText;
    public GameObject errorMessage;
    public GameObject infoCanvas;
    int gcInfoPort = 3838;
    private bool receivePackets = true;

    private bool updateReady = false;
    private bool errorToDisplay = false;
    private string errorString;

    private CancellationTokenSource cancellationTokenSource;

    public GameControlData gameControlData = new GameControlData();

    LogoController firstTeamLogoControllerField;
    LogoController secondTeamLogoControllerField;
    CombinedLogoController combinedLogoController;

    // Start is called before the first frame update
    void Start()
    {
        StartTasks();
    }

    void OnEnable()
    {
        StartTasks();
    }
    void StartTasks()
    {
        gcInfoClient = new UdpClient(gcInfoPort);
        gcInfoClient.Client.ReceiveTimeout = 5000;
        // Handle RGTr asynchronously
        cancellationTokenSource = new CancellationTokenSource();
        Task.Run(() => HandlePassiveGCInfo(cancellationTokenSource.Token));
        errorMessage.SetActive(true);
        infoCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (updateReady)
        {
            if (passiveGCInfoText != null)
            {
                Debug.Log("passiveGCInfoText is not null!");
                try
                {
                    passiveGCInfoText.text = gameControlData.ToString();

                }
                catch
                {
                    Debug.LogError("could not set passiveGCInfoText");
                }
            }

            try
            {
                Debug.Log("Getting here!");
                if (firstTeamLogoControllerField == null)
                {
                    try
                    {
                        firstTeamLogoControllerField = GameObject.Find("FirstTeamLogo").GetComponent<LogoController>();
                        secondTeamLogoControllerField = GameObject.Find("SecondTeamLogo").GetComponent<LogoController>();
                    }
                    catch
                    {
                        Debug.Log("GCINFO:LogoControllers not present");
                    }
                }

                if (firstTeamLogoControllerField != null){
                    firstTeamLogoControllerField.SetTeamNumber(gameControlData.team[0].teamNumber);
                    secondTeamLogoControllerField.SetTeamNumber(gameControlData.team[1].teamNumber);
                }



                if (combinedLogoController == null){
                    try{
                        combinedLogoController = GameObject.Find("Error Message Canvas").GetComponent<CombinedLogoController>();
                    }
                    catch {
                        Debug.Log("GCINFO:CombinedLogoController not present");
                    }
                }

                combinedLogoController.SetTeam0Number(gameControlData.team[0].teamNumber);
                combinedLogoController.SetTeam1Number(gameControlData.team[1].teamNumber);

            }
            catch (Exception err)
            {
                Debug.LogError("could not set Logos");
                Debug.LogError(err.ToString());
            }
            updateReady = false;
        }

        errorMessage.GetComponent<TMP_Text>().text = errorString;

        if (errorToDisplay){
            if (combinedLogoController != null){
                        combinedLogoController.SetTeam0Number(0);
                        combinedLogoController.SetTeam1Number(0);
            }
            else {
                try{
                    combinedLogoController = GameObject.Find("Error Message Canvas").GetComponent<CombinedLogoController>();
                    combinedLogoController.SetTeam0Number(0);
                    combinedLogoController.SetTeam1Number(0);
                }
                catch {
                    Debug.Log("GCINFO:CombinedLogoController not present");
                }
            }
        }
    }

    void OnDestroy()
    {
        StopAndDestroy();
    }

    void OnDisable()
    {
        StopAndDestroy();
    }

    void StopAndDestroy()
    {
        receivePackets = false;

        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel(); // Signal cancellation to the running task
            cancellationTokenSource.Dispose(); // Dispose the CancellationTokenSource
            Debug.Log("Stopped Tasks");
        }

        if (gcInfoClient != null)
        {
            gcInfoClient.Close();
            Debug.Log("Closed UDP Client");
        }
        errorString = "Welcome to GameSight\nLooking for a GameController";
        errorMessage.SetActive(false);
        infoCanvas.SetActive(false);
    }

    static GameControlData ReceiveMessages(UdpClient udpClient, IPAddress targetIpAddress, int targetPort, string headerMagic, string messageType)
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(targetIpAddress, targetPort);
        try
        {
            byte[] receivedData = udpClient.Receive(ref remoteEndPoint);
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
                return null;
            }
        }
        catch (System.Net.Sockets.SocketException err)
        {
            // Handle the exception, which occurs when the timeout is reached
            if (err.SocketErrorCode == System.Net.Sockets.SocketError.TimedOut)
            {
                // Handle timeout
                Debug.LogError("UDP Receive timed out.");
            }
            else
            {
                // Handle other socket exceptions
                Debug.LogError("UDP Receive error: " +err.Message);
            }
            return null;
        }
        catch (Exception err)
        {
            Debug.LogError("PASSIVEGCINFO:Caught Exception during ReceiveMessages");
            Debug.LogError(err.ToString());
            return null;
        }
    }

    async Task HandlePassiveGCInfo(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (gcInfoClient != null)
            {
                GameControlData data = await Task.Run(() => ReceiveMessages(gcInfoClient, targetIpAddress, gcInfoPort, "RGTr", "Passive GC"));

                if (data != null)
                {
                    Debug.Log("PASSIVEGCINFO: GC Data Recieved");
                    Debug.Log(data);
                    Debug.Log(data != null);
                    // Handle the data accordingly
                    gameControlData = data;
                    Debug.Log($"Received Pasive GC Data: {gameControlData.ToString()}");
                    updateReady = true;
                    errorToDisplay = false;
                    errorString = "Found a GameController!\n\nUse the left trigger to place the field\n\nThen press Y to start receving GCInfo";
                }
                else {
                    errorToDisplay = true;
                    errorString = "Something went wrong getting GCinfo\n\nIs there a GameController Online?";
                    
                }
            }
        }
    }

}
