' Epuration et amélioration du code grâce au travail de 
' CHASSINAT Adrien
' ING4 SE en 2013-2014

Imports System.Net.Sockets
Imports System.Net

Namespace AsyncSocket
    Public MustInherit Class AsynchronousSocket
        Protected _readbuf As Byte()
        Protected _sendbuf As Byte()
        Protected _started As Boolean
        Protected _receiveCallback As MyEventHandler

        Delegate Sub MyEventHandler(ByVal sender As Object, ByVal e As AsyncEventArgs)

        Public Sub New()
            _readbuf = New Byte(100) {}
        End Sub

        Public Sub AttachReceiveCallBack(ByRef receiveCallback As MyEventHandler)
            Me._receiveCallback = receiveCallback
        End Sub

        Public MustOverride Sub ReceiveMessage()
        Public MustOverride Sub SendMessage(ByVal message As Byte())

        Protected MustOverride Sub ReceiveCallback(ByVal asyncResult As IAsyncResult)
        Protected MustOverride Sub SendCallback(ByVal asyncResult As IAsyncResult)

        Public MustOverride Sub Close()

        Public ReadOnly Property Started() As Boolean
            Get
                Return _started
            End Get
        End Property

        Class AsyncEventArgs
            Inherits EventArgs

            Private _receivedBytes As Byte()

            Public Sub New(ByVal bytes As Byte())
                Me._receivedBytes = bytes
            End Sub

            Public ReadOnly Property ReceivedBytes() As Byte()
                Get
                    Return _receivedBytes
                End Get
            End Property
        End Class
    End Class


    Namespace ServerSocket
        Public Class AsynchronousServer
            Inherits AsynchronousSocket
            Private _socketServer As Socket
            Private _socketClient As Socket
            Private _localsocketClientIsShutingDown As Boolean = False

            Public Sub RunServer()
                Me._socketServer = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                Me._socketServer.Bind(New IPEndPoint(IPAddress.Any, 15))
                Me._socketServer.Listen(1)
                Me._socketServer.BeginAccept(AddressOf ConnectionAcceptCallback, Me._socketServer)
                Me._started = True
                'Me.DisplayMessageInfo("Server launched")
            End Sub

            Private Sub ConnectionAcceptCallback(ByVal asyncResult As IAsyncResult)
                Try
                    Dim socket As Socket = CType(asyncResult.AsyncState, Socket)
                    If Not (socket.Handle.ToInt32 = -1) Then
                        Me._socketClient = socket.EndAccept(asyncResult)
                        Me._localsocketClientIsShutingDown = False
                        'Me.DisplayMessageInfo("A client is connected")
                        Me._socketClient.BeginReceive(Me._readbuf, 0, Me._readbuf.Length, SocketFlags.None, AddressOf ReceiveCallback, Me._socketClient)
                    End If
                Catch ex As SocketException
                    'Me.DisplayMessageError(ex.Message)
                Catch ex As ObjectDisposedException
                    'Me.DisplayMessageError(ex.Message)
                End Try
            End Sub

            Public Overrides Sub ReceiveMessage()
                If Not (Me._socketClient Is Nothing) AndAlso Me._socketClient.Connected Then
                    Me._socketClient.BeginReceive(Me._readbuf, 0, Me._readbuf.Length, SocketFlags.None, AddressOf ReceiveCallback, Me._socketClient)
                Else
                    'Me.DisplayMessageInfo("No client connected")
                End If
            End Sub


            Protected Overrides Sub ReceiveCallback(ByVal asyncResult As IAsyncResult)
                Try
                    Dim socket As Socket = CType(asyncResult.AsyncState, Socket)
                    Dim read As Integer = socket.EndReceive(asyncResult)
                    If read > 0 Then
                        'Me.formElevator.NewMessageFromClient(Me.readbuf)

                        If Me._receiveCallback IsNot Nothing Then
                            Dim data(read - 1) As Byte
                            Array.Copy(Me._readbuf, data, read)
                            Me._receiveCallback(Me, New AsyncEventArgs(data))
                        End If

                        Array.Clear(Me._readbuf, 0, Me._readbuf.Length)
                        Me._socketClient.BeginReceive(Me._readbuf, 0, Me._readbuf.Length, SocketFlags.None, AddressOf ReceiveCallback, Me._socketClient)
                    End If
                    If read = 0 AndAlso Not Me._localsocketClientIsShutingDown Then
                        socket.Close()
                        'Me.DisplayMessageInfo("Client Socket is closing")
                        Me._socketServer.BeginAccept(AddressOf ConnectionAcceptCallback, Me._socketServer)
                    End If
                Catch ex As SocketException
                    'Me.DisplayMessageError(ex.Message)
                Catch ex As ObjectDisposedException
                    'Me.DisplayMessageError(ex.Message)
                End Try
            End Sub

            Public Overrides Sub SendMessage(ByVal message As Byte())
                If Not (Me._socketClient Is Nothing) AndAlso Me._socketClient.Connected AndAlso message IsNot Nothing Then
                    Me._sendbuf = message
                    Me._socketClient.BeginSend(Me._sendbuf, 0, Me._sendbuf.Length, SocketFlags.None, AddressOf SendCallback, Me._socketClient)
                ElseIf message IsNot Nothing Then
                    'Me.DisplayMessageInfo("No client connected")
                End If
            End Sub

            Protected Overrides Sub SendCallback(ByVal asyncResult As IAsyncResult)
                Try
                    Dim socket As Socket = CType(asyncResult.AsyncState, Socket)
                    Dim send As Integer = socket.EndSend(asyncResult)
                    Array.Clear(Me._sendbuf, 0, Me._sendbuf.Length)
                Catch ex As SocketException
                    'Me.DisplayMessageError(ex.Message)
                Catch ex As ObjectDisposedException
                    'Me.DisplayMessageError(ex.Message)
                End Try
            End Sub

            Public Sub CloseClient()
                If Not (Me._socketClient Is Nothing) AndAlso Me._socketClient.Connected Then
                    Me._localsocketClientIsShutingDown = True
                    Me._socketClient.Shutdown(SocketShutdown.Both)
                    Me._socketClient.Close()
                    Me._socketServer.BeginAccept(AddressOf ConnectionAcceptCallback, Me._socketServer)
                End If
                'Me.DisplayMessageInfo("Client stopped")
            End Sub

            Public Overrides Sub Close()
                If Not (Me._socketClient Is Nothing) AndAlso Me._socketClient.Connected Then
                    CloseClient()
                End If
                If Not (Me._socketServer Is Nothing) AndAlso Not (Me._socketServer.Handle.ToInt32 = -1) Then
                    Me._socketServer.Close()
                End If
                Me._started = False
                'Me.DisplayMessageInfo("Server stopped")
            End Sub

        End Class
    End Namespace

    Namespace ClientSocket
        Public Class AsynchronousClient
            Inherits AsynchronousSocket
            Private _socketClient As Socket
            Private _localsocketClientIsShutingDown As Boolean

            Public Sub ConnectToServer(ByVal ServerName As String)
                If Me._socketClient Is Nothing OrElse Not Me._socketClient.Connected Then
                    Me._socketClient = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                    Dim ipadress As IPAddress()
                    Dim he As IPHostEntry = Dns.GetHostEntry(ServerName)
                    ipadress = he.AddressList
                    Me._socketClient.BeginConnect(New IPEndPoint(ipadress.Last, 15), AddressOf ConnectCallback, Me._socketClient)
                Else
                    'Me.DisplayMessageInfo("Already connected to the Server")
                End If
            End Sub

            Private Sub ConnectCallback(ByVal asyncResult As IAsyncResult)
                Try
                    Dim socket As Socket = CType(asyncResult.AsyncState, Socket)
                    Me._socketClient = socket
                    socket.EndConnect(asyncResult)
                    'Me.DisplayMessageInfo("Connected to the Server")
                    Me._localsocketClientIsShutingDown = False
                    Me._socketClient.BeginReceive(Me._readbuf, 0, Me._readbuf.Length, SocketFlags.None, AddressOf ReceiveCallback, Me._socketClient)
                Catch ex As SocketException
                    'Me.DisplayMessageError(ex.Message)
                Catch ex As ObjectDisposedException
                    'Me.DisplayMessageError(ex.Message)
                End Try
            End Sub

            Public Overrides Sub SendMessage(ByVal message As Byte())
                If Not (Me._socketClient Is Nothing) AndAlso Me._socketClient.Connected AndAlso message IsNot Nothing Then
                    Me._sendbuf = message
                    Me._socketClient.BeginSend(Me._sendbuf, 0, Me._sendbuf.Length, SocketFlags.None, AddressOf SendCallback, Me._socketClient)
                ElseIf message IsNot Nothing Then
                    'Me.DisplayMessageInfo("No connection to the Server")
                End If
            End Sub

            Protected Overrides Sub SendCallback(ByVal asyncResult As IAsyncResult)
                Try
                    Dim socket As Socket = CType(asyncResult.AsyncState, Socket)
                    Dim send As Integer = socket.EndSend(asyncResult)
                Catch ex As SocketException
                    'Me.DisplayMessageError(ex.Message)
                Catch ex As ObjectDisposedException
                    'Me.DisplayMessageError(ex.Message)
                End Try
            End Sub

            Public Overrides Sub ReceiveMessage()
                If Not (Me._socketClient Is Nothing) AndAlso Me._socketClient.Connected Then
                    Me._socketClient.BeginReceive(Me._readbuf, 0, Me._readbuf.Length, SocketFlags.None, AddressOf ReceiveCallback, Me._socketClient)
                Else
                    'Me.DisplayMessageInfo("No connection to the Server")
                End If
            End Sub

            Protected Overrides Sub ReceiveCallback(ByVal asyncResult As IAsyncResult)
                Try
                    Dim socket As Socket = CType(asyncResult.AsyncState, Socket)
                    Dim read As Integer = socket.EndReceive(asyncResult)
                    If read > 0 Then
                        If Me._receiveCallback IsNot Nothing Then
                            Dim data(read - 1) As Byte
                            Array.Copy(Me._readbuf, data, read)

                            Me._receiveCallback(Me, New AsyncEventArgs(data))
                        End If
                        Array.Clear(Me._readbuf, 0, Me._readbuf.Length)
                        Me._socketClient.BeginReceive(Me._readbuf, 0, Me._readbuf.Length, SocketFlags.None, AddressOf ReceiveCallback, Me._socketClient)
                    End If
                    If read = 0 AndAlso Not Me._localsocketClientIsShutingDown Then
                        Me._socketClient.Close()
                        'Me.DisplayMessageInfo("Server Socket is closing")
                    End If
                Catch ex As SocketException
                    ' Me.DisplayMessageError(ex.Message)
                Catch ex As ObjectDisposedException
                    'Me.DisplayMessageError(ex.Message)
                End Try
            End Sub

            Public Overrides Sub Close()
                If Not (Me._socketClient Is Nothing) AndAlso Me._socketClient.Connected Then
                    Me._localsocketClientIsShutingDown = True
                    Me._socketClient.Shutdown(SocketShutdown.Both)
                    Me._socketClient.Close()
                    Me._started = False
                    'Me.DisplayMessageInfo("Connection closed")
                End If
            End Sub
        End Class
    End Namespace

End Namespace
