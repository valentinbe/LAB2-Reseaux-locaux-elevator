Imports System.Text
Imports RLI___TP2___Elevator.AsyncSocket.AsynchronousSocket
Imports RLI___TP2___Elevator.AsyncSocket.ClientSocket
Imports RLI___TP2___Elevator.AsyncSocket.ServerSocket

Public Class Elevator
    Public Shared ServerName As String = "localhost"
    Private serverIsRunning As Boolean = False
    Private clientIsRunning As Boolean = False
    Private direction As Integer = 0
    'Private last_sensor_checked As Integer = 0
    'Private called_floor As Integer = 10
    'Private floor_memory As Integer() = {10, 10, 10, 10, 10, 10, 10}
    'Private index_current_floor As Integer
    'Private index_last_saved_floor As Integer
    Private level As Byte
    Private elevatorMoving As Boolean
    Private datagram(12) As Byte
    Private transactionID As Short = 0
    Private dataReceived As Boolean = False

#Region "not to be touched"
    Private Sub ConnectToServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConnectToServer.Click
        If Not clientIsRunning Then
            Me.clientIsRunning = True
            Dim serverNameForm As AsyncSocket.ServerNameForm = New AsyncSocket.ServerNameForm
            serverNameForm.ShowDialog()
            Me.ConnectToServer.ForeColor = System.Drawing.Color.Green
            Me.ConnectToServer.Text = "Disconnect From the Server"

            Try
                Me._socket = New AsynchronousClient()
                _socket.AttachReceiveCallBack(AddressOf ReceivedDataFromServer)
                TryCast(_socket, AsynchronousClient).ConnectToServer(ServerName)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                Me.clientIsRunning = False
                Me.ConnectToServer.ForeColor = System.Drawing.Color.Red
                Me.ConnectToServer.Text = "Connect To the Server"
            End Try
        Else
            If _socket IsNot Nothing Then
                _socket.Close()
            End If
            Me.clientIsRunning = False
            Me.ConnectToServer.ForeColor = System.Drawing.Color.Red
            Me.ConnectToServer.Text = "Connect To the Server"
        End If
    End Sub

    Private Sub LauchServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LauchServer.Click
        If Not serverIsRunning Then
            Me._socket = New AsynchronousServer()
            Me._socket.AttachReceiveCallBack(AddressOf ReceivedDataFromClient)
            TryCast(_socket, AsynchronousServer).RunServer()

            Me.serverIsRunning = True
            Me.LauchServer.ForeColor = System.Drawing.Color.Green
            Me.LauchServer.Text = "Stop the Server"
        Else
            If _socket IsNot Nothing Then
                _socket.Close()
            End If
            Me.serverIsRunning = False
            Me.LauchServer.ForeColor = System.Drawing.Color.Red
            Me.LauchServer.Text = "Launch the Server"
        End If
    End Sub

    Public Sub New()
        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()
        Timer1.Enabled = True
        Timer1.Start()
        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
    End Sub

    Private Sub Ascenseur_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        If _socket IsNot Nothing Then
            _socket.Close()
        End If
    End Sub

    Public Sub SendMessageToClient(ByVal msg As Byte())
        If _socket IsNot Nothing Then
            If TryCast(_socket, AsynchronousServer) IsNot Nothing Then
                Me._socket.SendMessage(msg)
            End If
        End If
    End Sub

    Public Sub SendMessageToServer(ByVal msg As Byte())
        If _socket IsNot Nothing Then
            If TryCast(_socket, AsynchronousClient) IsNot Nothing Then
                Me._socket.SendMessage(msg)
                transactionID = transactionID + 1
            End If
        End If
    End Sub

    ' This delegate enables asynchronous calls for setting
    ' the property on a Checkbox control.
    Delegate Sub SetCoilUPCallback(ByVal [val] As Boolean)
    Public Sub SetCoilUP(ByVal [val] As Boolean)
        ' InvokeRequired required compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If Me.CoilUP.InvokeRequired Then
            Dim d As New SetCoilUPCallback(AddressOf SetCoilUP)
            Me.Invoke(d, New Object() {[val]})
        Else
            Me.CoilUP.Checked = [val]
        End If
    End Sub

    ' This delegate enables asynchronous calls for setting
    ' the property on a Checkbox control.
    Delegate Sub SetCoilDownCallback(ByVal [val] As Boolean)
    Public Sub SetCoilDown(ByVal [val] As Boolean)
        ' InvokeRequired required compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If Me.CoilDown.InvokeRequired Then
            Dim d As New SetCoilDownCallback(AddressOf SetCoilDown)
            Me.Invoke(d, New Object() {[val]})
        Else
            Me.CoilDown.Checked = [val]
        End If
    End Sub


#End Region



    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' YOUR JOB START HERE. You don't have to modify another file!
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    Private Sub ReceivedDataFromServer(ByVal sender As Object, ByVal e As AsyncEventArgs) ' si le maitre recoit des données
        'Add some stuff to interpret messages (and remove the next line!)
        'Bytes are in e.ReceivedBytes and you can encore the bytes to string using Encoding.ASCII.GetString(e.ReceivedBytes)
        'MessageBox.Show("Server says :" + Encoding.ASCII.GetString(e.ReceivedBytes), "I am Client")

        Dim LedStatusBoolean(4) As Boolean
        dataReceived = True

        Select Case e.ReceivedBytes(7)

            Case &H1
                'On récupère l'état des bobines
                CoilUP.Checked = System.Convert.ToBoolean(e.ReceivedBytes(9) And &H1)
                CoilDown.Checked = System.Convert.ToBoolean((e.ReceivedBytes(9) >> 1) And &H1)
            Case &H2
                LedStatusBoolean(0) = System.Convert.ToBoolean(e.ReceivedBytes(9) And &H1)
                LedStatusBoolean(1) = System.Convert.ToBoolean((e.ReceivedBytes(9) >> 1) And &H1)
                LedStatusBoolean(2) = System.Convert.ToBoolean((e.ReceivedBytes(9) >> 2) And &H1)
                LedStatusBoolean(3) = System.Convert.ToBoolean((e.ReceivedBytes(9) >> 3) And &H1)
                LedStatusBoolean(4) = System.Convert.ToBoolean((e.ReceivedBytes(9) >> 4) And &H1)

                'Pas certain que ce test serve à grand chose.
                If Not e.ReceivedBytes(9) = 0 Then
                    elevatorMoving = False

                    If LedStatusBoolean(0) Then
                        LedSensor0.BackColor = System.Drawing.Color.Red
                        level = 0
                    ElseIf Not LedStatusBoolean(0) Then
                        LedSensor0.BackColor = System.Drawing.Color.WhiteSmoke

                    ElseIf LedStatusBoolean(1) Then
                        LedSensor0.BackColor = System.Drawing.Color.Red
                        level = 1
                    ElseIf Not LedStatusBoolean(1) Then
                        LedSensor0.BackColor = System.Drawing.Color.WhiteSmoke

                        'En l'état actuel des choses, le capteur 2 n'est jamais actif
                    ElseIf LedStatusBoolean(2) Then
                        LedSensor0.BackColor = System.Drawing.Color.Red
                    ElseIf Not LedStatusBoolean(2) Then
                        LedSensor0.BackColor = System.Drawing.Color.WhiteSmoke

                    ElseIf LedStatusBoolean(3) Then
                        LedSensor0.BackColor = System.Drawing.Color.Red
                        level = 2
                    ElseIf Not LedStatusBoolean(3) Then
                        LedSensor0.BackColor = System.Drawing.Color.WhiteSmoke

                    ElseIf LedStatusBoolean(4) Then
                        LedSensor0.BackColor = System.Drawing.Color.Red
                        level = 3
                    ElseIf Not LedStatusBoolean(4) Then
                        LedSensor0.BackColor = System.Drawing.Color.WhiteSmoke
                    End If
                ElseIf e.ReceivedBytes(9) = 0 Then
                    elevatorMoving = True
                End If

            Case &H5
                'Pour le moment peu utile
            Case &HF
                'Pour le moment peu utile
        End Select
    End Sub

    Private Sub ReceivedDataFromClient(ByVal sender As Object, ByVal e As AsyncEventArgs) ' si lesclave recoit des données
        'Add some stuff to interpret messages (and remove the next line!)
        'Bytes are in e.ReceivedBytes and you can encore the bytes to string using Encoding.ASCII.GetString(e.ReceivedBytes)
        'MessageBox.Show("Client says :" + Encoding.ASCII.GetString(e.ReceivedBytes), "I am Server")
        dataReceived = True
        transactionID = e.ReceivedBytes(0) 'Peu utile pour le moment

        Select Case (e.ReceivedBytes(7))
            Case &H1
                ReadMultipleCoilsSlave(e.ReceivedBytes)
            Case &H2
                InquireSensorsSlave(e.ReceivedBytes)
            Case &H5
                WriteSingleCoilSlave(e.ReceivedBytes)
            Case &HF
                WriteMultipleCoilsSlave(e.ReceivedBytes)
        End Select

    End Sub

    'quand on appuie sur coil up
    Private Sub CoilUP_CheckedChanged(sender As Object, e As EventArgs) Handles CoilUP.CheckedChanged
        If serverIsRunning Then
            If Me.CoilUP.Checked Then
                direction = 1
            ElseIf Not Me.CoilUP.Checked Then
                direction = 0
            End If
        End If
    End Sub
    'quand on appuie sur coil down
    Private Sub CoilDown_CheckedChanged(sender As Object, e As EventArgs) Handles CoilDown.CheckedChanged
        If serverIsRunning Then ' si on est l'esclave
            If Me.CoilDown.Checked Then
                direction = -1
            ElseIf Not Me.CoilDown.Checked Then
                direction = 0
            End If
        End If
    End Sub

    ''fonction qui modifie les coilup et coil down en fonction de la direction demandée
    'Private Sub mouvement(direction As Integer)
    '    If direction = -1 Then
    '        WriteMultipleCoilsMaster(0, 2, New Byte() {2})
    '    ElseIf direction = 1 Then
    '        WriteMultipleCoilsMaster(0, 2, New Byte() {1})
    '    Else
    '        WriteMultipleCoilsMaster(0, 2, New Byte() {0})
    '    End If
    'End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If serverIsRunning Then ' si on est l'esclave
            ' on bouge en fonction de letat des coils
            If direction = -1 Then
                Me.ElevatorPhys.Location = New Point(Me.ElevatorPhys.Location.X, Me.ElevatorPhys.Location.Y + 10)
            ElseIf direction = 1 Then
                Me.ElevatorPhys.Location = New Point(Me.ElevatorPhys.Location.X, Me.ElevatorPhys.Location.Y - 10)
            Else
                ' on ne bouge pas
            End If

            'gestion allumage LEDS
            If Me.PositionSensor0.Location.Y > (Me.ElevatorPhys.Location.Y) And Me.PositionSensor0.Location.Y < (Me.ElevatorPhys.Location.Y + Me.ElevatorPhys.Size.Height) Then
                Me.LedSensor0.BackColor = System.Drawing.Color.Red

                Me.LedSensor1.BackColor = System.Drawing.Color.WhiteSmoke

            ElseIf Me.PositionSensor2.Location.Y < (Me.ElevatorPhys.Location.Y) And Me.PositionSensor1.Location.Y > (Me.ElevatorPhys.Location.Y + Me.ElevatorPhys.Size.Height) Then
                Me.LedSensor1.BackColor = System.Drawing.Color.Red

                Me.LedSensor0.BackColor = System.Drawing.Color.WhiteSmoke
                Me.LedSensor2.BackColor = System.Drawing.Color.WhiteSmoke
                Me.LedSensor3.BackColor = System.Drawing.Color.WhiteSmoke
                Me.LedSensor4.BackColor = System.Drawing.Color.WhiteSmoke
            ElseIf Me.PositionSensor3.Location.Y < (Me.ElevatorPhys.Location.Y) And Me.PositionSensor2.Location.Y > (Me.ElevatorPhys.Location.Y + Me.ElevatorPhys.Size.Height) Then
                Me.LedSensor3.BackColor = System.Drawing.Color.Red

                Me.LedSensor0.BackColor = System.Drawing.Color.WhiteSmoke
                Me.LedSensor1.BackColor = System.Drawing.Color.WhiteSmoke
                Me.LedSensor2.BackColor = System.Drawing.Color.WhiteSmoke
                Me.LedSensor4.BackColor = System.Drawing.Color.WhiteSmoke

                'ElseIf Me.PositionSensor1.Location.Y > (Me.ElevatorPhys.Location.Y) And Me.PositionSensor1.Location.Y < (Me.ElevatorPhys.Location.Y + Me.ElevatorPhys.Size.Height) Then
                '    Me.LedSensor1.BackColor = System.Drawing.Color.Red

                '    Me.LedSensor0.BackColor = System.Drawing.Color.WhiteSmoke
                '    Me.LedSensor2.BackColor = System.Drawing.Color.WhiteSmoke

                'ElseIf Me.PositionSensor2.Location.Y > (Me.ElevatorPhys.Location.Y) And Me.PositionSensor2.Location.Y < (Me.ElevatorPhys.Location.Y + Me.ElevatorPhys.Size.Height) Then
                '    Me.LedSensor2.BackColor = System.Drawing.Color.Red

                '    Me.LedSensor1.BackColor = System.Drawing.Color.WhiteSmoke
                '    Me.LedSensor3.BackColor = System.Drawing.Color.WhiteSmoke

                'ElseIf Me.PositionSensor3.Location.Y > (Me.ElevatorPhys.Location.Y) And Me.PositionSensor3.Location.Y < (Me.ElevatorPhys.Location.Y + Me.ElevatorPhys.Size.Height) Then
                '    Me.LedSensor3.BackColor = System.Drawing.Color.Red

                '    Me.LedSensor2.BackColor = System.Drawing.Color.WhiteSmoke
                '    Me.LedSensor4.BackColor = System.Drawing.Color.WhiteSmoke

            ElseIf (Me.PositionSensor4.Location.Y) > (Me.ElevatorPhys.Location.Y) And Me.PositionSensor4.Location.Y < (Me.ElevatorPhys.Location.Y + Me.ElevatorPhys.Size.Height) Then
                Me.LedSensor4.BackColor = System.Drawing.Color.Red

                Me.LedSensor3.BackColor = System.Drawing.Color.WhiteSmoke
            Else
                Me.LedSensor0.BackColor = System.Drawing.Color.WhiteSmoke
                Me.LedSensor1.BackColor = System.Drawing.Color.WhiteSmoke
                Me.LedSensor2.BackColor = System.Drawing.Color.WhiteSmoke
                Me.LedSensor3.BackColor = System.Drawing.Color.WhiteSmoke
                Me.LedSensor4.BackColor = System.Drawing.Color.WhiteSmoke
            End If

        ElseIf clientIsRunning Then ' si on est le maitre
            ' on envoi les requetes etat des sensor
            InquireSensorsMaster()

        End If
    End Sub

    'Private Function index_inc(index As Integer) As Integer
    '    If index = 6 Then
    '        index = 0
    '    Else
    '        index = index + 1
    '    End If
    '    Return index
    'End Function

    Private Sub ButtonCallFloor0_Click(sender As Object, e As EventArgs) Handles ButtonCallFloor0.Click
        'Si on est côté client, que l'ascenseur côté serveur est immobile et n'est pas à l'étage 0, alors:
        If Not serverIsRunning And elevatorMoving = False And level <> 0 Then

            WriteMultipleCoilsMaster(0, 2, New Byte() {2})

            Do Until level = 0
                Console.WriteLine("Ascenseur en mouvement\n")
            Loop

            'Dès que l'ascenseur a atteint 0, on l'arrête
            WriteMultipleCoilsMaster(0, 2, New Byte() {0})
        End If
    End Sub

    Private Sub ButtonCallFloor1_Click(sender As Object, e As EventArgs) Handles ButtonCallFloor1.Click
        If Not serverIsRunning And elevatorMoving = False Then

        End If
    End Sub

    Private Sub ButtonCallFloor2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCallFloor2.Click
        If Not serverIsRunning And elevatorMoving = False Then
            'ReadMultipleCoilsMaster(0, 2)

            ''On attend la réponse'
            'Do Until dataReceived
            '    Application.DoEvents()
            'Loop
            'dataReceived = False

            'If CoilDown.Checked = False And CoilUP.Checked = False Then

            'ElseIf CoilDown.Checked = False And CoilUP.Checked = True Then

            'ElseIf CoilDown.Checked = True And CoilUP.Checked = False Then

            'End If
        End If
    End Sub

    Private Sub ButtonCallFloor3_Click(sender As Object, e As EventArgs) Handles ButtonCallFloor3.Click
        If Not serverIsRunning And elevatorMoving = False Then
            'Si on est côté client, que l'ascenseur côté serveur est immobile et n'est pas à l'étage 0, alors:
            If Not serverIsRunning And elevatorMoving = False And level <> 3 Then

                WriteMultipleCoilsMaster(0, 2, New Byte() {1})

                Do Until level = 3
                    Application.DoEvents()
                Loop

                'Dès que l'ascenseur a atteint 0, on l'arrête
                WriteMultipleCoilsMaster(0, 2, New Byte() {0})
            End If
        End If
    End Sub

#Region "MODBUS Server_to_Client"

    ''' <summary>
    ''' Permet au serveur d'envoyer au client l'état des bobines.
    ''' </summary>
    ''' <param name="ReceivedDatagram">Tableau d'octets contenant les instructions du client MODBUS.</param>
    ''' <remarks></remarks>
    Private Sub ReadMultipleCoilsSlave(ReceivedDatagram As Byte())
        Dim temp1 As Byte
        Dim temp2 As Byte
        'Est-ce que l'adresse de la bobine est supérieure ou égal à 2? (On a seulement deux bobines)
        If Not (ReceivedDatagram(8) <> 0 And ReceivedDatagram(9) >= 2) Then
            'Est-ce que le nombre de bobines est supérieure à 2?
            If Not (ReceivedDatagram(10) <> 0 And ReceivedDatagram(11) > 2) Then
                'Si non dans les deux cas, alors:
                For i = 0 To 7
                    datagram(i) = ReceivedDatagram(i)
                Next
                'Le nombre d'octet contenant l'état des bobines vaudra toujours 1 (Seulement deux bobines => 0x00, 0x01... 0x03)
                datagram(8) = 1

                temp1 = System.Convert.ToByte(CoilUP.Checked) And &H1
                temp2 = System.Convert.ToByte(CoilUP.Checked) And &H1
                temp2 = temp2 << 1
                datagram(9) = temp1 + temp2

                'Réponse vers le client
                SendMessageToClient(datagram)
            End If
        End If
    End Sub
    ''' <summary>
    ''' Permet au serveur d'envoyer au client l'état des capteurs.
    ''' </summary>
    ''' <param name="ReceivedDatagram">Tableau d'octets contenant les instructions du client MODBUS.</param>
    ''' <remarks></remarks>
    Private Sub InquireSensorsSlave(ReceivedDatagram As Byte())
        Dim SensorStatus(4) As Byte
        'Est-ce que l'adresse des capteur est supérieure ou égal à 2? (On a seulement deux bobines)
        If Not (ReceivedDatagram(8) <> 0 And ReceivedDatagram(9) >= 6) Then
            'Est-ce que le nombre de capteurs est supérieur à 6?
            If Not (ReceivedDatagram(10) <> 0 And ReceivedDatagram(11) > 6) Then
                'Si non dans les deux cas, alors:
                If LedSensor0.BackColor.Equals(System.Drawing.Color.WhiteSmoke) Then
                    SensorStatus(0) = 0 And &H1
                ElseIf LedSensor0.BackColor.Equals(System.Drawing.Color.Red) Then
                    SensorStatus(0) = 1 And &H1
                End If

                If LedSensor1.BackColor.Equals(System.Drawing.Color.WhiteSmoke) Then
                    SensorStatus(1) = 0 And &H1
                    SensorStatus(1) = SensorStatus(1) << 1
                ElseIf LedSensor1.BackColor.Equals(System.Drawing.Color.Red) Then
                    SensorStatus(1) = 1 And &H1
                    SensorStatus(1) = SensorStatus(1) << 1
                End If

                If LedSensor2.BackColor.Equals(System.Drawing.Color.WhiteSmoke) Then
                    SensorStatus(2) = 0 And &H1
                    SensorStatus(2) = SensorStatus(1) << 2
                ElseIf LedSensor2.BackColor.Equals(System.Drawing.Color.Red) Then
                    SensorStatus(2) = 1 And &H1
                    SensorStatus(2) = SensorStatus(1) << 2
                End If

                If LedSensor3.BackColor.Equals(System.Drawing.Color.WhiteSmoke) Then
                    SensorStatus(3) = 0 And &H1
                    SensorStatus(3) = SensorStatus(3) << 3
                ElseIf LedSensor3.BackColor.Equals(System.Drawing.Color.Red) Then
                    SensorStatus(3) = 1 And &H1
                    SensorStatus(3) = SensorStatus(3) << 3
                End If

                If LedSensor4.BackColor.Equals(System.Drawing.Color.WhiteSmoke) Then
                    SensorStatus(4) = 0 And &H1
                    SensorStatus(4) = SensorStatus(4) << 4
                ElseIf LedSensor4.BackColor.Equals(System.Drawing.Color.Red) Then
                    SensorStatus(4) = 1 And &H1
                    SensorStatus(4) = SensorStatus(4) << 4
                End If

                For i = 0 To 7
                    datagram(i) = ReceivedDatagram(i)
                Next
                'Le nombre d'octet contenant l'état des capteurs vaudra toujours 1 (Seulement cinq capteurs => 0x00, 0x01... 0x1F)
                datagram(8) = 1
                datagram(9) = SensorStatus(0) + SensorStatus(1) + SensorStatus(2) + SensorStatus(3) + SensorStatus(4)

                SendMessageToClient(datagram)
            End If
        End If
    End Sub
    ''' <summary>
    ''' Permet au serveur de modifier l'état de la bobines choisie en fonction des données reçues et envoie une réponse au client.
    ''' </summary>
    ''' <param name="ReceivedDatagram">Tableau d'octets contenant les instructions du client MODBUS.</param>
    ''' <remarks></remarks>
    Private Sub WriteSingleCoilSlave(ReceivedDatagram As Byte())
        'Est-ce que l'adresse de la bobine est supérieure ou égal à 2? (On a seulement deux bobines)
        If Not (ReceivedDatagram(8) <> 0 And ReceivedDatagram(9) >= 2) Then
            'Est-ce que le nombre de bobines est supérieure à 2?
            If Not (ReceivedDatagram(10) <> 0 And ReceivedDatagram(11) > 2) Then
                'Si non dans les deux cas, alors:
                Select Case ReceivedDatagram(9)
                    Case 0
                        SetCoilUP(System.Convert.ToBoolean(ReceivedDatagram(10)))
                    Case 1
                        SetCoilDown(System.Convert.ToBoolean(ReceivedDatagram(10)))
                End Select
            End If

            For i = 0 To 11
                datagram(i) = ReceivedDatagram(i)
            Next
            'Réponse vers le client
            SendMessageToClient(datagram)

        End If
    End Sub

    ''' <summary>
    ''' Permet au serveur de modifier l'état des bobines en fonction des données reçues et envoie une réponse au client.
    ''' </summary>
    ''' <param name="ReceivedDatagram">Tableau d'octets contenant les instructions du client MODBUS.</param>
    ''' <remarks></remarks>
    Private Sub WriteMultipleCoilsSlave(ReceivedDatagram As Byte())
        Dim i As Integer = 0
        'Est-ce que l'adresse de départ est supérieure ou égal à 2? (On a seulement deux bobines)
        If Not (ReceivedDatagram(8) <> 0 And ReceivedDatagram(9) >= 2) Then
            'Est-ce que le nombre de bobines est supérieure à 2?
            If Not (ReceivedDatagram(10) <> 0 And ReceivedDatagram(11) > 2) Then
                'Si non dans les deux cas, alors:
                Select Case ReceivedDatagram(13)
                    Case 0
                        SetCoilDown(False)
                        SetCoilUP(False)
                    Case 1
                        SetCoilDown(False)
                        SetCoilUP(True)
                    Case 2
                        SetCoilDown(True)
                        SetCoilUP(False)
                End Select

                For i = 0 To 11
                    datagram(i) = ReceivedDatagram(i)
                Next
                'Réponse vers le client
                SendMessageToClient(datagram)
            End If
        End If
    End Sub
#End Region

#Region "MODBUS_Client_to_Server"
    ''' <summary>
    ''' Permet au client de lire la valeur de plusieurs bobines (UP/ DOWN) en même temps
    ''' </summary>
    ''' <param name="StartAddress">Adresse à partir de laquelle on sélectionne les bobines. Par défaut 0</param>
    ''' <param name="BitCount">Nombre de bobines: ici 2</param>
    ''' <remarks></remarks>
    Private Sub ReadMultipleCoilsMaster(StartAddress As Integer, BitCount As Integer)
        Dim tempData As Integer
        tempData = BitCount
        StartAddress = StartAddress << 16
        tempData = tempData + StartAddress

        FillDatagram(1, tempData)

        SendMessageToServer(datagram)
    End Sub

    ''' <summary>
    ''' Permet au client de modifier plusieurs bobines (UP/ DOWN) en même temps
    ''' </summary>
    ''' <param name="StartAddress">Adresse à partir de laquelle on sélectionne les bobines. Par défaut 0</param>
    ''' <param name="BitCount">Nombre de bobines: ici 2</param>
    ''' <param name="tempOutputValues">valeurs des bobines :0b11, 0b10, 0b..., 0b00. Ici un tableau de une case (Seulement deux bobines)</param>
    ''' <remarks></remarks>
    Private Sub WriteMultipleCoilsMaster(StartAddress As Integer, BitCount As Integer, tempOutputValues As Byte())
        Dim tempList As List(Of Byte)
        Dim tempData As Integer

        Array.Reverse(tempOutputValues)
        tempList = tempOutputValues.ToList()
        tempList.Insert(0, tempList.Count)
        tempOutputValues = tempList.ToArray()

        tempData = BitCount
        StartAddress = StartAddress << 16
        tempData = tempData + StartAddress

        FillDatagram(15, tempData, tempOutputValues)

        SendMessageToServer(datagram)
    End Sub

    ''' <summary>
    ''' Permet au client de modifier une bobine (UP ou DOWN)
    ''' </summary>
    ''' <param name="OutputAddress">Adresse de la bobine</param>
    ''' <param name="OutputValue">Valeurs de la bobine : 0 ou 1</param>
    ''' <remarks></remarks>
    Private Sub WriteSingleCoilMaster(OutputAddress As Integer, OutputValue As Integer)
        Dim tempData As Integer
        tempData = OutputValue
        OutputAddress = OutputAddress << 16

        tempData = tempData + OutputAddress

        FillDatagram(5, tempData)

        SendMessageToServer(datagram)
    End Sub

    ''' <summary>
    ''' Fonction pour obtenir l'état des 6 capteurs à partir de l'adresse 0
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InquireSensorsMaster()
        FillDatagram(2, 6)
        SendMessageToServer(datagram)
    End Sub

    ''' <summary>
    ''' Fonction pour remplir le datagramme MODBUS/TCP
    ''' </summary>
    ''' <param name="FuncCode">Octet identifiant la fonction MODBUS utilisé</param>
    ''' <param name="Data">Quatre octets identifiants les données à traiter</param>
    ''' <param name="DataFc15">Données supplémentaires pour FC15 (optionnel)</param>
    ''' <remarks></remarks>
    Private Sub FillDatagram(FuncCode As Byte, Data As Integer, Optional DataFc15 As Byte() = Nothing)
        ReDim datagram(11)

        datagram(0) = BitConverter.GetBytes(transactionID)(1)
        datagram(1) = BitConverter.GetBytes(transactionID)(0)

        datagram(2) = 0
        datagram(3) = 0

        datagram(6) = 1
        datagram(7) = FuncCode

        Select Case FuncCode
            Case &H1
                datagram(4) = 0
                datagram(5) = 6
                datagram(8) = BitConverter.GetBytes(Data)(3)
                datagram(9) = BitConverter.GetBytes(Data)(2)
                datagram(10) = BitConverter.GetBytes(Data)(1)
                datagram(11) = BitConverter.GetBytes(Data)(0)
            Case &H2
                datagram(4) = 0
                datagram(5) = 6
                datagram(8) = BitConverter.GetBytes(Data)(3)
                datagram(9) = BitConverter.GetBytes(Data)(2)
                datagram(10) = BitConverter.GetBytes(Data)(1)
                datagram(11) = BitConverter.GetBytes(Data)(0)
            Case &H5
                datagram(4) = 0
                datagram(5) = 6
                datagram(8) = BitConverter.GetBytes(Data)(3)
                datagram(9) = BitConverter.GetBytes(Data)(2)
                datagram(10) = BitConverter.GetBytes(Data)(1)
                datagram(11) = 0
            Case &HF
                Dim temp As New List(Of Byte)
                'La fonction FC15 de MODBUS permet au maximum de modifier l'état de 256 bits, ce qui amène à un maximum de 32 octets (256/8) pour définir l'état des bits.
                'Donc datagram(4) est toujours à 0
                datagram(4) = 0
                datagram(5) = DataFc15.Length + 6
                datagram(8) = BitConverter.GetBytes(Data)(3)
                datagram(9) = BitConverter.GetBytes(Data)(2)
                datagram(10) = BitConverter.GetBytes(Data)(1)
                datagram(11) = BitConverter.GetBytes(Data)(0)

                temp.AddRange(datagram)
                temp.AddRange(DataFc15)

                datagram = temp.ToArray()
        End Select

    End Sub
#End Region

End Class
