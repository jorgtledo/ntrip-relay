Public Class Form1
    Public GPS As New NTRIPRelay.GPS

    Public NTRIPShouldBeConnected As Boolean = False
    Public Shared NTRIPManualLat As Decimal = 41
    Public Shared NTRIPManualLon As Decimal = -91
    Public Shared NTRIPCaster As String = ""
    Public Shared NTRIPPort As Integer = 2101
    Public Shared NTRIPUsername As String = ""
    Public Shared NTRIPPassword As String = ""
    Public Shared NTRIPMountPoint As String = ""
    Public PreferredMountPoint As String = ""
    Public NTRIPThread As Threading.Thread
    Public NTRIPIsConnected As Boolean = False
    Public NTRIPConnectionAttempt As Integer = 1
    Public Shared NTRIPStreamRequiresGGA As Boolean = False
    Dim NTRIPByteCount As Integer = 0
    Public NTRIPStreamArray(1, -1) As String
    Public Shared MostRecentGGA As String = ""
    Public StartNTRIPThreadIn As Integer = 0

    Dim ClientStartTime As Date
    Dim ClientIsStreaming As Boolean = False
    Dim ServerStartTime As Date
    Dim ServerIsStreaming As Boolean = False

    Dim upDays As Integer
    Dim upHors As Integer
    Dim upMins As Integer
    Dim upSecs As Integer


    Public ServerShouldBeConnected As Boolean = False
    Public StartServerThreadIn As Integer = 0
    Public Shared ServerCaster As String = "127.0.0.1"
    Public Shared ServerPort As Integer = 5000
    Public Shared ServerMountPoint As String = ""
    Public Shared ServerPassword As String = ""
    Public ServerThread As Threading.Thread
    Public ServerIsConnected As Boolean = False
    Public ServerConnectionAttempt As Integer = 1
    Dim ServerByteCount As Integer = 0
    Public Shared DataQueue(9999) As Byte
    Public Shared DataQueueSize As Integer = -1


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ver As String = "Version: " & My.Application.Info.Version.Major & "." & Format(My.Application.Info.Version.Minor, "00") & "." & Format(My.Application.Info.Version.Build, "00")
        If My.Application.Info.Version.Revision <> 0 Then ver += " (Rev " & My.Application.Info.Version.Revision & ")"
        LogEvent("NTRIP Relay, " & ver)

        lblClientUptime.Text = ""
        lblServerUptime.Text = ""

        LoadSettingsFile()
        LoadNTRIPSettings()
        RefreshCasterNames()

        If NTRIPShouldBeConnected Then
            StartNTRIPThreadIn = 1
        End If

        Timer1.Start()
    End Sub
    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        LogEvent("Application Closing")
        End
    End Sub
    Dim TimerTickCounter As Integer = 0
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If StartNTRIPThreadIn > 0 Then
            StartNTRIPThreadIn -= 1
            If StartNTRIPThreadIn = 0 Then StartNTRIP()
        End If
        If StartServerThreadIn > 0 Then
            StartServerThreadIn -= 1
            If StartServerThreadIn = 0 Then StartServer()
        End If

        TimerTickCounter += 1
        If TimerTickCounter = 10 Then
            TimerTickCounter = 0
            If ClientIsStreaming Then
                upSecs = DateDiff(DateInterval.Second, ClientStartTime, Now)
                upDays = Int(upSecs / 86400)
                upHors = Int(upSecs / 3600) - (upDays * 24)
                upMins = (Int(upSecs / 60)) - (((upDays * 24) + upHors) * 60)
                upSecs = Int(upSecs Mod 60)
                lblClientUptime.Text = "Uptime: " & upDays & "d, " & upHors & ":" & Format(upMins, "00") & ":" & Format(upSecs, "00")
            End If
            If ServerIsStreaming Then
                upSecs = DateDiff(DateInterval.Second, ServerStartTime, Now)
                upDays = Int(upSecs / 86400)
                upHors = Int(upSecs / 3600) - (upDays * 24)
                upMins = (Int(upSecs / 60)) - (((upDays * 24) + upHors) * 60)
                upSecs = Int(upSecs Mod 60)
                lblServerUptime.Text = "Uptime: " & upDays & "d, " & upHors & ":" & Format(upMins, "00") & ":" & Format(upSecs, "00")
            End If
        End If
        
    End Sub

    Public Sub RefreshCasterNames()
        lblClientName.Text = NTRIPCaster & ":" & NTRIPPort
        lblServerName.Text = ServerCaster & ":" & ServerPort & " /" & ServerMountPoint
    End Sub

    Public Sub LogEvent(ByVal Message As String)
        If rtbEvents.TextLength > 5000 Then
            Dim NewText As String = Mid(rtbEvents.Text, 1000) 'Drop first 1000 characters
            NewText = NewText.Remove(0, NewText.IndexOf(ChrW(10)) + 1) 'Drop up to the next new line
            rtbEvents.Text = NewText
        End If

        rtbEvents.AppendText(vbCrLf & TimeOfDay() & " - " & Message)
        rtbEvents.SelectionStart = rtbEvents.TextLength
        rtbEvents.ScrollToCaret()

        Dim logfolder As String = Application.StartupPath & "\Logs"
        If Not My.Computer.FileSystem.DirectoryExists(logfolder) Then
            Try
                My.Computer.FileSystem.CreateDirectory(logfolder)
            Catch ex As Exception
            End Try
        End If
        Dim logfile As String = logfolder & "\" & Year(Now) & Format(Month(Now), "00") & Format(DatePart(DateInterval.Day, Now), "00") & ".txt"
        For i = 0 To 10
            Try
                My.Computer.FileSystem.WriteAllText(logfile, Now() & " - " & Message & vbCrLf, True)
                Exit For 'This worked, don't try it again
                Threading.Thread.Sleep(20)
                Application.DoEvents()
            Catch ex As Exception
            End Try
        Next
    End Sub
    Private Sub LoadSettingsFile()
        'Check to make sure directory exists, if not, throw a WTF message.
        If Not My.Computer.FileSystem.DirectoryExists(Application.StartupPath) Then
            MsgBox("Error: The Application's folder doesn't exist. Settings file not loaded.")
            Exit Sub
        End If

        If Not My.Computer.FileSystem.FileExists(Application.StartupPath & "\Settings.txt") Then 'File doesn't exist. Create it.
            Dim fn As New IO.StreamWriter(IO.File.Open(Application.StartupPath & "\Settings.txt", IO.FileMode.Create))
            fn.WriteLine("# This is the GPS Data Path Pointer file. You need to use the format ""Key=Value"" for all settings.")
            fn.WriteLine("# Any line that starts with a # symbol will be ignored.")
            fn.WriteLine("# The only setting in this file should be the Data Path Location.")
            fn.WriteLine("")
            fn.Close()
        End If

        'Open and read file
        Dim SettingsArray(1, 0) As String
        Dim keyvalpair(1) As String
        Dim key As String
        Dim value As String
        Dim lCtr As Integer = 0

        Try
            Dim oRead As System.IO.StreamReader = System.IO.File.OpenText(Application.StartupPath & "\Settings.txt")
            Dim linein

            While oRead.Peek <> -1
                linein = Trim(oRead.ReadLine)
                If Len(linein) < 3 Then 'Line is too short
                ElseIf Asc(linein) = 35 Then 'Line starts with a #
                ElseIf InStr(linein, "=") < 2 Then 'There is no equal sign in the string
                Else
                    keyvalpair = Split(linein, "=", 2)
                    key = Trim(keyvalpair(0))
                    value = Trim(keyvalpair(1))
                    If Len(key) > 0 And Len(value) > 0 Then
                        'Looks good, add it to the array
                        ReDim Preserve SettingsArray(1, lCtr)
                        SettingsArray(0, lCtr) = LCase(key)
                        SettingsArray(1, lCtr) = value
                        lCtr = lCtr + 1
                    End If
                End If
            End While
            oRead.Close()
        Catch ex As Exception
        End Try

        If lCtr > 0 Then
            For i = 0 To UBound(SettingsArray, 2)
                value = SettingsArray(1, i)
                Select Case SettingsArray(0, i)
                    Case "ntrip should be connected"
                        If LCase(value) = "yes" Then NTRIPShouldBeConnected = True
                    Case "ntrip manual latitude"
                        If IsNumeric(value) Then
                            Dim inlat As Decimal = CDec(value)
                            If inlat > -90 And inlat < 90 Then
                                NTRIPManualLat = inlat
                            Else
                                LogEvent("Specified NTRIP Manual Latitude should be between -90 and 90.")
                            End If
                        Else
                            LogEvent("Specified NTRIP Manual Latitude should be numeric.")
                        End If
                    Case "ntrip manual longitude"
                        If IsNumeric(value) Then
                            Dim inlon As Decimal = CDec(value)
                            If inlon > -180 And inlon < 180 Then
                                NTRIPManualLon = inlon
                            Else
                                LogEvent("Specified NTRIP Manual Longitude should be between -180 and 180.")
                            End If
                        Else
                            LogEvent("Specified NTRIP Manual Longitude should be numeric.")
                        End If



                    Case "server should be connected"
                        If LCase(value) = "yes" Then ServerShouldBeConnected = True
                    Case "destination caster address"
                        ServerCaster = value
                    Case "destination caster port"
                        If IsNumeric(value) Then
                            ServerPort = CInt(value)
                        End If
                    Case "destination caster mountpoint"
                        ServerMountPoint = value
                    Case "destination caster password"
                        ServerPassword = value


                    Case Else
                        'Key not found
                        If Not SettingsArray(0, i) = "" Then
                            'This will be blank if no settings were loaded
                            LogEvent("Just FYI, the """ & SettingsArray(0, i) & """ key in the data path pointer file isn't valid, so it was skipped.")
                        End If
                End Select
            Next
        End If

    End Sub
    Public Sub SaveSetting(ByVal key1 As String, ByVal value1 As String, Optional ByVal key2 As String = "", Optional ByVal value2 As String = "", Optional ByVal key3 As String = "", Optional ByVal value3 As String = "")
        If Not My.Computer.FileSystem.FileExists(Application.StartupPath & "\Settings.txt") Then 'File doesn't exist. Create it.
            Dim fn As New IO.StreamWriter(IO.File.Open(Application.StartupPath & "\Settings.txt", IO.FileMode.Create))
            fn.WriteLine("# This is the NTRIP Client settings file. You need to use the format ""Key=Value"" for all settings.")
            fn.WriteLine("# Any line that starts with a # symbol will be ignored.")
            fn.WriteLine("")
            fn.Close()
        End If


        Dim keyvalpair(1) As String
        Dim oRead As System.IO.StreamReader = System.IO.File.OpenText(Application.StartupPath & "\Settings.txt")
        Dim linein As String
        Dim newfile As String = ""
        Dim foundkey1 As Boolean = False
        Dim foundkey2 As Boolean = False
        Dim foundkey3 As Boolean = False

        While oRead.Peek <> -1
            linein = Trim(oRead.ReadLine)
            If Len(linein) < 3 Then 'Line is too short
                newfile += linein
            ElseIf Asc(linein) = 35 Then 'Line starts with a #
                newfile += linein
            ElseIf InStr(linein, "=") < 2 Then 'There is no equal sign in the string
                newfile += linein
            Else
                keyvalpair = Split(linein, "=", 2)
                If LCase(Trim(keyvalpair(0))) = LCase(key1) Then 'Found the right key, update it.
                    newfile += keyvalpair(0) & "=" & value1
                    foundkey1 = True
                ElseIf key2.Length > 0 And LCase(Trim(keyvalpair(0))) = LCase(key2) Then
                    newfile += keyvalpair(0) & "=" & value2
                    foundkey2 = True
                ElseIf key3.Length > 0 And LCase(Trim(keyvalpair(0))) = LCase(key3) Then
                    newfile += keyvalpair(0) & "=" & value3
                    foundkey3 = True
                Else
                    newfile += linein
                End If
            End If
            newfile += vbCrLf
        End While
        oRead.Close()

        If Not foundkey1 Then
            newfile += key1 & "=" & value1 & vbCrLf
        End If
        If key2.Length > 0 And Not foundkey2 Then
            newfile += key2 & "=" & value2 & vbCrLf
        End If
        If key3.Length > 0 And Not foundkey3 Then
            newfile += key3 & "=" & value3 & vbCrLf
        End If


        Try
            Dim sWriter As IO.StreamWriter = New IO.StreamWriter(Application.StartupPath & "\Settings.txt")
            sWriter.Write(newfile)
            sWriter.Flush()
            sWriter.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub





    Public Sub LoadNTRIPSettings()
        boxMountpoint.Items.Add("Download Source Table")
        boxMountpoint.SelectedIndex = 0

        'Load NTRIP settings file
        Dim ntripconfigfile As String = Application.StartupPath & "\ntripconfig.txt"
        If My.Computer.FileSystem.FileExists(ntripconfigfile) Then
            Dim SettingsArray(1, 0) As String
            Dim keyvalpair(1) As String
            Dim key As String
            Dim value As String
            Dim lCtr As Integer = 0

            Try
                Dim oRead As System.IO.StreamReader = System.IO.File.OpenText(ntripconfigfile)
                Dim linein

                While oRead.Peek <> -1
                    linein = Trim(oRead.ReadLine)
                    If Len(linein) < 3 Then
                        'Line is too short
                    ElseIf Asc(linein) = 35 Then
                        'Line starts with a #
                    ElseIf InStr(linein, "=") < 2 Then
                        'There is no equal sign in the string
                    Else
                        keyvalpair = Split(linein, "=", 2)
                        key = Trim(keyvalpair(0))
                        value = Trim(keyvalpair(1))
                        If Len(key) > 0 And Len(value) > 0 Then
                            'Looks good, add it to the array
                            ReDim Preserve SettingsArray(1, lCtr)
                            SettingsArray(0, lCtr) = LCase(key)
                            SettingsArray(1, lCtr) = value
                            lCtr = lCtr + 1
                        End If
                    End If
                End While
                oRead.Close()
            Catch ex As Exception
            End Try

            If lCtr > 0 Then
                For i = 0 To UBound(SettingsArray, 2)
                    value = SettingsArray(1, i)
                    Select Case SettingsArray(0, i)
                        Case "ntrip caster"
                            NTRIPCaster = value
                        Case "ntrip caster port"
                            If IsNumeric(value) Then
                                NTRIPPort = CInt(value)
                            End If
                        Case "ntrip username"
                            NTRIPUsername = value
                        Case "ntrip password"
                            NTRIPPassword = value
                        Case "ntrip mountpoint"
                            PreferredMountPoint = value
                    End Select
                Next
            End If
        End If

        'Load sourcetable file into drop down list
        Dim sourcetablefile As String = Application.StartupPath & "\sourcetable.dat"
        If My.Computer.FileSystem.FileExists(sourcetablefile) Then
            'File exists. Open and parse
            Try
                Dim sourcefile As String = ""
                Dim linein As String
                Dim oRead As System.IO.StreamReader = System.IO.File.OpenText(sourcetablefile)
                While oRead.Peek <> -1
                    linein = Trim(oRead.ReadLine)
                    sourcefile += linein & vbCrLf
                End While
                oRead.Close()

                If sourcefile.Length > 10 Then
                    ParseSourceTable(sourcefile)
                End If
            Catch ex As Exception
            End Try
        End If

    End Sub
    Public Sub ParseSourceTable(ByVal table As String)
        ReDim NTRIPStreamArray(1, -1)
        Dim StreamCount As Integer = -1 'zero based array
        boxMountpoint.Items.Clear()
        boxMountpoint.Items.Add("Download Source Table")
        boxMountpoint.SelectedIndex = 0

        Dim lines() As String = Split(table, vbCrLf) 'Chr(13)
        For i = 0 To UBound(lines)
            Dim fields() As String = Split(lines(i), ";")
            If UBound(fields) > 4 Then
                If LCase(fields(0)) = "str" Then
                    'We found a STReam
                    boxMountpoint.Items.Add(fields(1))
                    StreamCount += 1
                    ReDim Preserve NTRIPStreamArray(1, StreamCount)
                    NTRIPStreamArray(0, StreamCount) = fields(1)
                    NTRIPStreamArray(1, StreamCount) = fields(11)
                End If
            End If
        Next

        Dim k As Integer = 0
        Dim selectedmnt As Integer = 0
        For Each item In boxMountpoint.Items
            If item = PreferredMountPoint Then
                selectedmnt = k
            End If
            k += 1
        Next

        If boxMountpoint.Items.Count = 1 Then
            boxMountpoint.SelectedIndex = 0
        Else
            boxMountpoint.SelectedIndex = selectedmnt
        End If
    End Sub
    Public Sub SaveNTRIPSettings()
        Dim ntripsettings As String = "NTRIP Caster=" & NTRIPCaster & vbCrLf
        ntripsettings += "NTRIP Caster Port=" & NTRIPPort.ToString & vbCrLf
        ntripsettings += "NTRIP Username=" & NTRIPUsername & vbCrLf
        ntripsettings += "NTRIP Password=" & NTRIPPassword & vbCrLf
        ntripsettings += "NTRIP MountPoint=" & PreferredMountPoint & vbCrLf
        Dim targetfile As String = Application.StartupPath & "\ntripconfig.txt"
        My.Computer.FileSystem.WriteAllText(targetfile, ntripsettings, False)
    End Sub

    Private Sub btnNTRIPEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNTRIPEdit.Click
        ClientDialog.tbAddress.Text = NTRIPCaster
        ClientDialog.tbPort.Text = NTRIPPort
        ClientDialog.tbUsername.Text = NTRIPUsername
        ClientDialog.tbPassword.Text = NTRIPPassword
        ClientDialog.tbLatitude.Text = NTRIPManualLat
        ClientDialog.tbLongitude.Text = NTRIPManualLon
        ClientDialog.tbAddress.Focus()


        Dim DialogResult As Integer = ClientDialog.ShowDialog()
        Dim result As Integer = Convert.ToInt32(DialogResult)

        If result = 1 Then
            NTRIPCaster = ClientDialog.tbAddress.Text

            If IsNumeric(ClientDialog.tbPort.Text) Then
                Dim newport As Integer = CInt(ClientDialog.tbPort.Text)
                If newport > 0 And newport < 65536 Then
                    NTRIPPort = newport
                Else
                    LogEvent("The NTRIP Caster Port needs to be in the range of 1-65535.")
                End If
            Else
                LogEvent("The NTRIP Caster Port needs to be numeric.")
            End If

            NTRIPUsername = ClientDialog.tbUsername.Text
            NTRIPPassword = ClientDialog.tbPassword.Text
            SaveNTRIPSettings()

            If IsNumeric(ClientDialog.tbLatitude.Text) Then
                Dim newlat As Decimal = CDec(ClientDialog.tbLatitude.Text)
                If newlat > -90 And newlat < 90 Then
                    NTRIPManualLat = newlat
                Else
                    LogEvent("The Manual Latitude needs to be in the range of -90 to 90.")
                End If
            Else
                LogEvent("The Manual Latitude needs to be numeric.")
            End If

            If IsNumeric(ClientDialog.tbLongitude.Text) Then
                Dim newlon As Decimal = CDec(ClientDialog.tbLongitude.Text)
                If newlon > -180 And newlon < 180 Then
                    NTRIPManualLon = newlon
                Else
                    LogEvent("The Manual Longitude needs to be in the range of -180 to 180.")
                End If
            Else
                LogEvent("The Manual Longitude needs to be numeric.")
            End If

            SaveSetting("NTRIP Manual Latitude", NTRIPManualLat, "NTRIP Manual Longitude", NTRIPManualLon)

            LogEvent("NTRIP Client Settings Saved")
            RefreshCasterNames()
        ElseIf result = 2 Then
            'Don't do anything
        End If
    End Sub
    Private Sub boxMountpoint_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles boxMountpoint.SelectionChangeCommitted
        PreferredMountPoint = boxMountpoint.SelectedItem
        SaveNTRIPSettings()
    End Sub
    Private Sub btnNTRIPConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNTRIPConnect.Click
        If btnNTRIPConnect.Text = "Connect" Then
            NTRIPConnectionAttempt = 1
            NTRIPShouldBeConnected = True
            SaveSetting("NTRIP Should be Connected", "Yes")
            StartNTRIPThreadIn = 1
        Else
            NTRIPShouldBeConnected = False
            SaveSetting("NTRIP Should be Connected", "No")
            StopNTRIP()
        End If
    End Sub

    Public Sub StartNTRIP()
        'Check the status to see if it is already connected
        If Not NTRIPThread Is Nothing Then
            If NTRIPThread.IsAlive Then
                btnNTRIPConnect.Text = "Disconnect"
                LogEvent("NTRIP thread is already running. Please disconnect first before trying to connect again.")
                StopNTRIP()
                Exit Sub
            End If
        End If

        If NTRIPCaster = "" Then
            lblNTRIPStatus.Text = "No NTRIP Caster Specified"
            Exit Sub
        End If
        If NTRIPPort = 0 Then
            lblNTRIPStatus.Text = "No NTRIP Caster Port Specified"
            Exit Sub
        End If
        If NTRIPPort < 1 Or NTRIPPort > 65535 Then
            lblNTRIPStatus.Text = "Invalid Port Number"
            Exit Sub
        End If


        NTRIPMountPoint = boxMountpoint.SelectedItem
        If NTRIPMountPoint = "Download Source Table" Then
            NTRIPMountPoint = ""
        Else
            PreferredMountPoint = NTRIPMountPoint
        End If

        NTRIPStreamRequiresGGA = False
        For i = 0 To UBound(NTRIPStreamArray, 2)
            If NTRIPStreamArray(0, i) = NTRIPMountPoint Then
                If NTRIPStreamArray(1, i) = "1" Then
                    NTRIPStreamRequiresGGA = True
                End If
            End If
        Next

        boxMountpoint.Enabled = False
        btnNTRIPConnect.Text = "Disconnect"
        btnNTRIPEdit.Visible = False

        lblNTRIPStatus.Text = "Starting NTRIP Thread"
        Application.DoEvents()

        NTRIPIsConnected = True
        NTRIPThread = New Threading.Thread(AddressOf NTRIPLoop)
        NTRIPThread.Priority = Threading.ThreadPriority.AboveNormal
        NTRIPThread.Start()
    End Sub
    Public Sub StopNTRIP()
        ClientIsStreaming = False
        lblClientUptime.Text = ""

        'This gets called from the MyBase.FormClosed event as the app closes
        'Attempt to disconnect the nice way
        StartNTRIPThreadIn = 0
        NTRIPIsConnected = False
        lblNTRIPStatus.Text = "Disconnecting..."

        'Wait for the thread to notice the change and stop.
        Threading.Thread.Sleep(100)
        Application.DoEvents()
        Threading.Thread.Sleep(100)
        Application.DoEvents()

        'Ok, kill the thread if it is still running.
        If Not NTRIPThread Is Nothing Then
            If NTRIPThread.IsAlive Then
                NTRIPThread.Abort() 'Need to add the ability to truly kill a connection that is unresponsive. .Abort() doesn't seem to actually kill the thread
                Threading.Thread.Sleep(100)
                Application.DoEvents()
                Threading.Thread.Sleep(100)
                Application.DoEvents()
            End If
        End If

        pbNTRIP.Visible = False
        NTRIPConnectionAttempt += 1
        lblNTRIPStatus.Text = "Disconnected"

        If NTRIPConnectionAttempt > 10000 Then
            btnNTRIPConnect.Visible = True
            NTRIPShouldBeConnected = False
            SaveSetting("NTRIP Should be Connected", "No")
            LogEvent("NTRIP Client is Disconnected, 10000 Failed Connection Attempts.")
        End If

        If NTRIPShouldBeConnected Then
            StartNTRIPThreadIn = 10
        Else
            btnNTRIPConnect.Text = "Connect"
            btnNTRIPConnect.Visible = True
            boxMountpoint.Enabled = True
            btnNTRIPEdit.Visible = True
        End If
    End Sub
    Public Sub NTRIPLoop()
        'Pause for a bit in case we just disconnected and are now reconnecting.
        Threading.Thread.Sleep(1000)

        Dim NeedsToSendGGA As Boolean = NTRIPStreamRequiresGGA 'This is a thread-local option that can get set to false later if only need to send GGA once.

        'This sub gets called on a new thread, it send/receives data, waits 100ms, then loops.
        Dim sckt As Net.Sockets.Socket
        Dim lcount As Integer = 97
        NTRIPUpdateUIThread(0, "", Nothing) 'Connecting


        'Connect to server
        sckt = New Net.Sockets.Socket(Net.Sockets.AddressFamily.InterNetwork, Net.Sockets.SocketType.Stream, Net.Sockets.ProtocolType.Tcp)
        Try
            'sckt.Connect(New Net.IPEndPoint(NTRIPCaster, NTRIPPort))
            sckt.Connect(NTRIPCaster, NTRIPPort)
        Catch ex As Exception
            NTRIPUpdateUIThread(100, "Server did not respond.", Nothing)
            Exit Sub
        End Try


        NTRIPUpdateUIThread(1, "", Nothing) 'Connected

        'Build request message
        Dim msg As String = "GET /" & NTRIPMountPoint & " HTTP/1.0" & vbCr & vbLf
        msg += "User-Agent: NTRIP NTRIPRelay/20110331" & vbCr & vbLf
        msg += "Accept: */*" & vbCr & vbLf & "Connection: close" & vbCr & vbLf
        If NTRIPUsername.Length > 0 Then
            Dim auth As String = ToBase64(NTRIPUsername & ":" & NTRIPPassword)
            msg += "Authorization: Basic " & auth & vbCr & vbLf 'This line can be removed if no authorization is needed
        End If
        msg += vbCr & vbLf

        'Send request
        Dim data As Byte() = System.Text.Encoding.ASCII.GetBytes(msg)
        sckt.Send(data)
        Threading.Thread.Sleep(100)

        'Wait for response
        'Dim returndata As Byte() = New Byte(255) {}
        Dim responseData As String = ""
        Try
            For i = 0 To 300 'Wait 30 seconds for a response
                Threading.Thread.Sleep(100)
                Dim DataLength As Integer = sckt.Available
                If DataLength > 0 Then
                    Dim InBytes(DataLength - 1) As Byte
                    sckt.Receive(InBytes, DataLength, Net.Sockets.SocketFlags.None)
                    responseData = System.Text.Encoding.ASCII.GetString(InBytes, 0, InBytes.Length)
                End If
                If responseData.Length > 0 Then Exit For
            Next
        Catch ex As Exception
            NTRIPUpdateUIThread(100, "Unknown Response.", Nothing)
            NTRIPThread.Abort()
        End Try


        If responseData.Contains("SOURCETABLE 200 OK") Then
            'Start of source table was downloaded. Check for more data.
            For i = 0 To 100 'Wait another 10 seconds for source table
                Threading.Thread.Sleep(100)
                Dim DataLength As Integer = sckt.Available
                If DataLength > 0 Then
                    Dim InBytes(DataLength - 1) As Byte
                    sckt.Receive(InBytes, DataLength, Net.Sockets.SocketFlags.None)
                    responseData += System.Text.Encoding.ASCII.GetString(InBytes, 0, InBytes.Length)
                End If
                If responseData.Contains("ENDSOURCETABLE") Then Exit For
            Next

            Dim targetfile As String = Application.StartupPath & "\sourcetable.dat"
            My.Computer.FileSystem.WriteAllText(targetfile, responseData, False)
            NTRIPUpdateUIThread(101, responseData, Nothing) 'Send on sourcetable for parsing

            sckt.Disconnect(False)
            NTRIPUpdateUIThread(100, "Downloaded Source Table", Nothing)
            NTRIPThread.Abort()
        ElseIf responseData.Contains("401 Unauthorized") Then
            'Login failed
            sckt.Disconnect(False)
            NTRIPUpdateUIThread(100, "Invalid Username or Password.", Nothing)
            NTRIPThread.Abort()
        ElseIf responseData.Contains("ICY 200 OK") Then
            NTRIPUpdateUIThread(2, "", Nothing) 'ICY 200 OK, Waiting for data
            Dim DataNotReceivedFor As Integer = 0
            Try
                Do While True
                    Dim DataLength As Integer = sckt.Available
                    If DataLength = 0 Then
                        DataNotReceivedFor += 1
                        If DataNotReceivedFor > 300 Then
                            'Data not received for 30 seconds. Terminate the connection.
                            NTRIPUpdateUIThread(100, "Connection Timed Out.", Nothing)
                            NTRIPThread.Abort()
                        End If
                    Else
                        DataNotReceivedFor = 0
                        Dim InBytes(DataLength - 1) As Byte
                        sckt.Receive(InBytes, DataLength, Net.Sockets.SocketFlags.None)
                        NTRIPUpdateUIThread(3, Nothing, InBytes)
                    End If

                    lcount += 1
                    If lcount = 100 Then
                        If NeedsToSendGGA Then
                            Dim TheGGA As String
                            TheGGA = GPS.GenerateGPGGAcode() 'This function runs in the NTRIP thread.
                            NeedsToSendGGA = False 'Only needs to be once when using a manual GGA
                            Dim nmeadata As Byte() = System.Text.Encoding.ASCII.GetBytes(TheGGA & vbCrLf)
                            Try
                                sckt.Send(nmeadata)
                            Catch ex As Exception
                                NTRIPUpdateUIThread(100, "Error: " & ex.Message, Nothing)
                            End Try
                        End If
                        lcount = 0
                    End If

                    If Not NTRIPIsConnected Then 'Flag changed, kill the thread
                        sckt.Disconnect(False)
                        NTRIPUpdateUIThread(100, "", Nothing)
                        NTRIPThread.Abort()
                    End If
                    Threading.Thread.Sleep(100)
                Loop
            Catch ex As Threading.ThreadAbortException
                Throw 'Dejar que el abort normal se propague
            Catch ex As Exception
                'La conexion se corto inesperadamente (red caida, socket cerrado, etc.)
                NTRIPUpdateUIThread(100, "Connection Lost.", Nothing)
                NTRIPThread.Abort()
            End Try
        Else
            sckt.Disconnect(False)
            If responseData.Length = 0 Then
                NTRIPUpdateUIThread(100, "No Response.", Nothing)
            Else
                NTRIPUpdateUIThread(100, "Unknown Response.", Nothing)
            End If
            NTRIPThread.Abort()
        End If
    End Sub
    Private Sub NTRIPUpdateUIThread(ByVal Item As Integer, ByVal Value As String, ByVal myBytes() As Byte)
        Try
            Dim uidel As New NTRIPUpdateUIThreadDelegate(AddressOf NTRIPCallBacktoUIThread)
            Dim o(2) As Object
            o(0) = Item
            o(1) = Value
            o(2) = myBytes
            Invoke(uidel, o)
        Catch ex As Exception
        End Try
    End Sub
    Delegate Sub NTRIPUpdateUIThreadDelegate(ByVal Item As Integer, ByVal Value As String, ByVal myBytes() As Byte)
    Private Sub NTRIPCallBacktoUIThread(ByVal Item As Integer, ByVal Value As String, ByVal myBytes() As Byte)
        Select Case Item
            Case -1
                lblNTRIPStatus.Text = "Waiting for NMEA GGA data..."
            Case 0
                lblNTRIPStatus.Text = "Connecting..."
                If NTRIPConnectionAttempt > 1 Then
                    lblNTRIPStatus.Text += " Attempt " & NTRIPConnectionAttempt
                    LogEvent("NTRIP Client is attempting to reconnect, Attempt " & NTRIPConnectionAttempt)
                Else
                    LogEvent("NTRIP Client is attempting to connect.")
                    If NTRIPStreamRequiresGGA Then
                        LogEvent("NTRIP Client is using a simulated location of " & NTRIPManualLat & ", " & NTRIPManualLon)
                    End If
                End If
            Case 1
                lblNTRIPStatus.Text = "Connected, Requesting Data..."
                NTRIPByteCount = 0

            Case 2
                lblNTRIPStatus.Text = "Connected, Waiting for Data..."
                pbNTRIP.Value = 0
                pbNTRIP.Visible = True
                LogEvent("NTRIP Client is Connected, Waiting for Data.")

            Case 3
                If DataQueueSize + myBytes.Length > 9999 Then
                    DataQueueSize = -1
                End If
                If myBytes.Length > 9999 Then
                    ReDim Preserve myBytes(9999)
                End If
                myBytes.CopyTo(DataQueue, DataQueueSize + 1)
                DataQueueSize += myBytes.Length

                Try
                    'If COMPort.IsOpen Then
                    '    COMPort.Write(myBytes, 0, myBytes.Length)
                    'End If
                Catch ex As Exception
                End Try
                If NTRIPByteCount = 0 Then
                    LogEvent("NTRIP Client is receiving data.")
                    ClientStartTime = Now
                    ClientIsStreaming = True
                End If
                NTRIPByteCount += myBytes.Length
                lblNTRIPStatus.Text = "Connected, " & Format(NTRIPByteCount, "###,###,###,##0") & " bytes received."

                Dim remainder As Integer = CInt(NTRIPByteCount) Mod 5000
                remainder = CInt(remainder / 50)
                pbNTRIP.Value = remainder

            Case 100 'Thread commited suicide for some reason.
                If Value = "Invalid Username or Password." Then
                    NTRIPShouldBeConnected = False
                    SaveSetting("NTRIP Should be Connected", "No")
                End If

                If Value = "" Then
                    'lblNTRIPStatus.Text = "Disconnected"
                    LogEvent("NTRIP Client is Disconnected.")
                Else
                    'lblNTRIPStatus.Text = "Disconnected, " & Value
                    LogEvent("NTRIP Client is Disconnected, " & Value)
                End If

                StopNTRIP()

            Case 101 'Got Source Table, parse it
                ParseSourceTable(Value)
                btnNTRIPConnect.Visible = True
                LogEvent("NTRIP Client downloaded the Source Table.")
                NTRIPShouldBeConnected = False
                SaveSetting("NTRIP Should be Connected", "No")
                StopNTRIP()

        End Select
    End Sub
    Private Function ToBase64(ByVal str As String) As String
        Dim asciiEncoding As System.Text.Encoding = System.Text.Encoding.ASCII
        Dim byteArray As Byte() = New Byte(asciiEncoding.GetByteCount(str) - 1) {}
        byteArray = asciiEncoding.GetBytes(str)
        Return Convert.ToBase64String(byteArray, 0, byteArray.Length)
    End Function

    
    

    Private Sub btnServerEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnServerEdit.Click
        ServerDialog.tbAddress.Text = ServerCaster
        ServerDialog.tbPort.Text = ServerPort
        ServerDialog.tbMountpoint.Text = ServerMountPoint
        ServerDialog.tbPassword.Text = ServerPassword
        ServerDialog.tbAddress.Focus()


        Dim DialogResult As Integer = ServerDialog.ShowDialog()
        Dim result As Integer = Convert.ToInt32(DialogResult)

        If result = 1 Then
            ServerCaster = ServerDialog.tbAddress.Text

            If IsNumeric(ServerDialog.tbPort.Text) Then
                Dim newport As Integer = CInt(ServerDialog.tbPort.Text)
                If newport > 0 And newport < 65536 Then
                    ServerPort = newport
                Else
                    LogEvent("The Server Caster Port needs to be in the range of 1-65535.")
                End If
            Else
                LogEvent("The Server Caster Port needs to be numeric.")
            End If

            ServerMountPoint = ServerDialog.tbMountpoint.Text
            ServerPassword = ServerDialog.tbPassword.Text

            SaveSetting("Destination Caster Address", ServerCaster, "Destination Caster Port", ServerPort)
            SaveSetting("Destination Caster Mountpoint", ServerMountPoint, "Destination Caster Password", ServerPassword)

            LogEvent("NTRIP Server Settings Saved")
            RefreshCasterNames()
        ElseIf result = 2 Then
            'Don't do anything
        End If
    End Sub
    Private Sub btnServerConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnServerConnect.Click
        If btnServerConnect.Text = "Connect" Then
            ServerConnectionAttempt = 1
            ServerShouldBeConnected = True
            SaveSetting("Server Should be Connected", "Yes")
            StartServerThreadIn = 1
        Else
            ServerShouldBeConnected = False
            SaveSetting("Server Should be Connected", "No")
            StopServer()
        End If
    End Sub

    Public Sub StartServer()
        'Check the status to see if it is already connected
        If Not ServerThread Is Nothing Then
            If ServerThread.IsAlive Then
                btnServerConnect.Text = "Disconnect"
                LogEvent("Server thread is already running. Please disconnect first before trying to connect again.")
                StopServer()
                Exit Sub
            End If
        End If

        If ServerCaster = "" Then
            lblServerStatus.Text = "No Server Caster Specified"
            Exit Sub
        End If
        If ServerPort = 0 Then
            lblServerStatus.Text = "No Server Caster Port Specified"
            Exit Sub
        End If
        If ServerPort < 1 Or ServerPort > 65535 Then
            lblServerStatus.Text = "Invalid Port Number"
            Exit Sub
        End If

        btnServerConnect.Text = "Disconnect"
        btnServerEdit.Visible = False

        lblServerStatus.Text = "Starting Server Thread"
        Application.DoEvents()

        ServerIsConnected = True
        ServerThread = New Threading.Thread(AddressOf ServerLoop)
        ServerThread.Priority = Threading.ThreadPriority.AboveNormal
        ServerThread.Start()
    End Sub
    Public Sub StopServer()
        ServerIsStreaming = False
        lblServerUptime.Text = ""

        'This gets called from the MyBase.FormClosed event as the app closes
        'Attempt to disconnect the nice way
        StartServerThreadIn = 0
        ServerIsConnected = False
        lblServerStatus.Text = "Disconnecting..."

        'Wait for the thread to notice the change and stop.
        Threading.Thread.Sleep(100)
        Application.DoEvents()
        Threading.Thread.Sleep(100)
        Application.DoEvents()

        'Ok, kill the thread if it is still running.
        If Not ServerThread Is Nothing Then
            If ServerThread.IsAlive Then
                ServerThread.Abort() 'Need to add the ability to truly kill a connection that is unresponsive. .Abort() doesn't seem to actually kill the thread
                Threading.Thread.Sleep(100)
                Application.DoEvents()
                Threading.Thread.Sleep(100)
                Application.DoEvents()
            End If
        End If

        pbServer.Visible = False
        ServerConnectionAttempt += 1
        lblServerStatus.Text = "Disconnected"

        If ServerConnectionAttempt > 10000 Then
            ServerShouldBeConnected = False
            SaveSetting("Server Should be Connected", "No")
            LogEvent("NTRIP Server is Disconnected, 10000 Failed Connection Attempts.")
        End If

        If ServerShouldBeConnected Then
            StartServerThreadIn = 10
        Else
            btnServerConnect.Text = "Connect"
            btnServerEdit.Visible = True
        End If
    End Sub
    Public Sub ServerLoop()
        'Pause for a bit in case we just disconnected and are now reconnecting.
        Threading.Thread.Sleep(1000)

        'This sub gets called on a new thread, it send/receives data, waits 100ms, then loops.
        Dim sckt As Net.Sockets.Socket
        Dim lcount As Integer = 97
        ServerUpdateUIThread(0, "") 'Connecting


        'Connect to server
        sckt = New Net.Sockets.Socket(Net.Sockets.AddressFamily.InterNetwork, Net.Sockets.SocketType.Stream, Net.Sockets.ProtocolType.Tcp)
        Try
            sckt.Connect(ServerCaster, ServerPort)
        Catch ex As Exception
            ServerUpdateUIThread(100, "Server did not respond.")
            ServerThread.Abort()
        End Try


        ServerUpdateUIThread(1, "") 'Connected

        'Build request message
        Dim msg As String = "SOURCE " & ServerPassword & " /" & ServerMountPoint & vbCr & vbLf
        msg += "Source-Agent: NTRIP NTRIPRelay/20090824" & vbCr & vbLf
        msg += "STR: " & vbCr & vbLf & vbCr & vbLf

        'Send request
        Dim data As Byte() = System.Text.Encoding.ASCII.GetBytes(msg)
        sckt.Send(data)
        Threading.Thread.Sleep(100)

        'Wait for response
        'Dim returndata As Byte() = New Byte(255) {}
        Dim responseData As String = ""
        Try
            For i = 0 To 300 'Wait 30 seconds for a response
                Threading.Thread.Sleep(100)
                Dim DataLength As Integer = sckt.Available
                If DataLength > 0 Then
                    Dim InBytes(DataLength - 1) As Byte
                    sckt.Receive(InBytes, DataLength, Net.Sockets.SocketFlags.None)
                    responseData = System.Text.Encoding.ASCII.GetString(InBytes, 0, InBytes.Length)
                End If
                If responseData.Length > 0 Then Exit For
            Next
        Catch ex As Exception
            ServerUpdateUIThread(100, "Unknown Response.")
            Exit Sub
            'ServerThread.Abort()
        End Try

        If responseData.Contains("ICY 200 OK") Then
            ServerUpdateUIThread(2, "") 'ICY 200 OK, Waiting for data
            DataQueueSize = -1
            Dim DataNotSentFor As Integer = 0
            Do While True
                If DataQueueSize > -1 Then 'There is data to send
                    Try
                        Dim bytes(DataQueueSize) As Byte
                        Array.Copy(DataQueue, bytes, DataQueueSize + 1) 'Copy to another buffer
                        sckt.Send(bytes, bytes.Length, Net.Sockets.SocketFlags.None)
                        ServerUpdateUIThread(3, DataQueueSize + 1)
                        DataQueueSize = -1 'clear old data out
                        DataNotSentFor = 0
                    Catch ex As Exception
                        ServerUpdateUIThread(100, "Send failed.")
                        Exit Sub
                        'ServerThread.Abort()
                    End Try
                Else
                    DataNotSentFor += 1
                    If DataNotSentFor > 300 Then
                        ServerUpdateUIThread(100, "Connection Timed Out.")
                        Exit Sub
                        'ServerThread.Abort()
                    End If
                End If

                If Not ServerIsConnected Then 'Flag changed, kill the thread
                    sckt.Disconnect(False)
                    ServerUpdateUIThread(100, "")
                    Exit Sub
                    'ServerThread.Abort()
                End If
                Threading.Thread.Sleep(100)
            Loop
        ElseIf responseData.Contains("ERROR - Bad Password") Then
            'Login failed
            sckt.Disconnect(False)
            ServerUpdateUIThread(100, "Invalid Password.")
            Exit Sub
            'ServerThread.Abort()
        Else
            sckt.Disconnect(False)
            If responseData.Length = 0 Then
                ServerUpdateUIThread(100, "No Response.")
            Else
                ServerUpdateUIThread(100, "Unknown Response.")
            End If
            Exit Sub
            'ServerThread.Abort()
        End If
    End Sub
    Private Sub ServerUpdateUIThread(ByVal Item As Integer, ByVal Value As String)
        Try
            Dim uidel As New ServerUpdateUIThreadDelegate(AddressOf ServerCallBacktoUIThread)
            Dim o(1) As Object
            o(0) = Item
            o(1) = Value
            Invoke(uidel, o)
        Catch ex As Exception
        End Try
    End Sub
    Delegate Sub ServerUpdateUIThreadDelegate(ByVal Item As Integer, ByVal Value As String)
    Private Sub ServerCallBacktoUIThread(ByVal Item As Integer, ByVal Value As String)
        Select Case Item
            Case 0
                lblServerStatus.Text = "Connecting..."
                If ServerConnectionAttempt > 1 Then
                    lblServerStatus.Text += " Attempt " & ServerConnectionAttempt
                    LogEvent("NTRIP Server is attempting to reconnect, Attempt " & ServerConnectionAttempt)
                Else
                    LogEvent("NTRIP Server is attempting to connect.")
                End If
            Case 1
                lblServerStatus.Text = "Connected, Requesting Data..."
                ServerByteCount = 0

            Case 2
                lblServerStatus.Text = "Connected, Waiting for Data from Source Caster..."
                pbServer.Value = 0
                pbServer.Visible = True
                LogEvent("NTRIP Server is Connected, Waiting for Data.")

            Case 3
                If ServerByteCount = 0 Then
                    LogEvent("NTRIP Server is receiving data.")
                    ServerStartTime = Now
                    ServerIsStreaming = True
                End If
                ServerByteCount += CInt(Value)
                lblServerStatus.Text = "Connected, " & Format(ServerByteCount, "###,###,###,##0") & " bytes sent."

                Dim remainder As Integer = CInt(ServerByteCount) Mod 5000
                remainder = CInt(remainder / 50)
                pbServer.Value = remainder

            Case 100 'Thread commited suicide for some reason.
                If Value = "Invalid Password." Then
                    ServerShouldBeConnected = False
                    SaveSetting("Server Should be Connected", "No")
                End If

                If Value = "" Then
                    'lblNTRIPStatus.Text = "Disconnected"
                    LogEvent("NTRIP Server is Disconnected.")
                Else
                    'lblNTRIPStatus.Text = "Disconnected, " & Value
                    LogEvent("NTRIP Server is Disconnected, " & Value)
                End If

                StopServer()
        End Select
    End Sub




End Class
