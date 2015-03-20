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
            _receiveCallback = receiveCallback
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
                _receivedBytes = bytes
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
                _socketServer = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                _socketServer.Bind(New IPEndPoint(IPAddress.Any, 15))
                _socketServer.Listen(1)
                _socketServer.BeginAccept(AddressOf ConnectionAcceptCallback, _socketServer)
                _started = True
                'Me.DisplayMessageInfo("Server launched")
            End Sub

            Private Sub ConnectionAcceptCallback(ByVal asyncResult As IAsyncResult)
                Try
                    Dim socket As Socket = CType(asyncResult.AsyncState, Socket)
                    If Not (socket.Handle.ToInt32 = -1) Then
                        _socketClient = socket.EndAccept(asyncResult)
                        _localsocketClientIsShutingDown = False
                        'Me.DisplayMessageInfo("A client is connected")
                        _socketClient.BeginReceive(_readbuf, 0, _readbuf.Length, SocketFlags.None, AddressOf ReceiveCallback, _socketClient)
                    End If
                Catch ex As SocketException
                    'Me.DisplayMessageError(ex.Message)
                Catch ex As ObjectDisposedException
                    'Me.DisplayMessageError(ex.Message)
                End Try
            End Sub

            Public Overrides Sub ReceiveMessage()
                If Not (_socketClient Is Nothing) AndAlso _socketClient.Connected Then
                    _socketClient.BeginReceive(_readbuf, 0, _readbuf.Length, SocketFlags.None, AddressOf ReceiveCallback, _socketClient)
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

                        If _receiveCallback IsNot Nothing Then
                            Dim data(read - 1) As Byte
                            Array.Copy(_readbuf, data, read)
                            _receiveCallback(Me, New AsyncEventArgs(data))
                        End If

                        Array.Clear(_readbuf, 0, _readbuf.Length)
                        _socketClient.BeginReceive(_readbuf, 0, _readbuf.Length, SocketFlags.None, AddressOf ReceiveCallback, _socketClient)
                    End If
                    If read = 0 AndAlso Not _localsocketClientIsShutingDown Then
                        socket.Close()
                        'Me.DisplayMessageInfo("Client Socket is closing")
                        _socketServer.BeginAccept(AddressOf ConnectionAcceptCallback, _socketServer)
                    End If
                Catch ex As SocketException
                    'Me.DisplayMessageError(ex.Message)
                Catch ex As ObjectDisposedException
                    'Me.DisplayMessageError(ex.Message)
                End Try
            End Sub

            Public Overrides Sub SendMessage(ByVal message As Byte())
                If Not (_socketClient Is Nothing) AndAlso _socketClient.Connected AndAlso message IsNot Nothing Then
                    _sendbuf = message
                    _socketClient.BeginSend(_sendbuf, 0, _sendbuf.Length, SocketFlags.None, AddressOf SendCallback, _socketClient)
                ElseIf message IsNot Nothing Then
                    'Me.DisplayMessageInfo("No client connected")
                End If
            End Sub

            Protected Overrides Sub SendCallback(ByVal asyncResult As IAsyncResult)
                Try
                    Dim socket As Socket = CType(asyncResult.AsyncState, Socket)
                    Dim send As Integer = socket.EndSend(asyncResult)
                    Array.Clear(_sendbuf, 0, _sendbuf.Length)
                Catch ex As SocketException
                    'Me.DisplayMessageError(ex.Message)
                Catch ex As ObjectDisposedException
                    'Me.DisplayMessageError(ex.Message)
                End Try
            End Sub

            Public Sub CloseClient()
                If Not (_socketClient Is Nothing) AndAlso _socketClient.Connected Then
                    _localsocketClientIsShutingDown = True
                    _socketClient.Shutdown(SocketShutdown.Both)
                    _socketClient.Close()
                    _socketServer.BeginAccept(AddressOf ConnectionAcceptCallback, _socketServer)
                End If
                'Me.DisplayMessageInfo("Client stopped")
            End Sub

            Public Overrides Sub Close()
                If Not (_socketClient Is Nothing) AndAlso _socketClient.Connected Then
                    CloseClient()
                End If
                If Not (_socketServer Is Nothing) AndAlso Not (_socketServer.Handle.ToInt32 = -1) Then
                    _socketServer.Close()
                End If
                _started = False
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
                If _socketClient Is Nothing OrElse Not _socketClient.Connected Then
                    _socketClient = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                    Dim ipadress As IPAddress()
                    Dim he As IPHostEntry = Dns.GetHostEntry(ServerName)
                    ipadress = he.AddressList
                    _socketClient.BeginConnect(New IPEndPoint(ipadress.Last, 15), AddressOf ConnectCallback, _socketClient)
                Else
                    'Me.DisplayMessageInfo("Already connected to the Server")
                End If
            End Sub

            Private Sub ConnectCallback(ByVal asyncResult As IAsyncResult)
                Try
                    Dim socket As Socket = CType(asyncResult.AsyncState, Socket)
                    _socketClient = socket
                    socket.EndConnect(asyncResult)
                    'Me.DisplayMessageInfo("Connected to the Server")
                    _localsocketClientIsShutingDown = False
                    _socketClient.BeginReceive(_readbuf, 0, _readbuf.Length, SocketFlags.None, AddressOf ReceiveCallback, _socketClient)
                Catch ex As SocketException
                    'Me.DisplayMessageError(ex.Message)
                Catch ex As ObjectDisposedException
                    'Me.DisplayMessageError(ex.Message)
                End Try
            End Sub

            Public Overrides Sub SendMessage(ByVal message As Byte())
                If Not (_socketClient Is Nothing) AndAlso _socketClient.Connected AndAlso message IsNot Nothing Then
                    _sendbuf = message
                    _socketClient.BeginSend(_sendbuf, 0, _sendbuf.Length, SocketFlags.None, AddressOf SendCallback, _socketClient)
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
                If Not (_socketClient Is Nothing) AndAlso _socketClient.Connected Then
                    _socketClient.BeginReceive(_readbuf, 0, _readbuf.Length, SocketFlags.None, AddressOf ReceiveCallback, _socketClient)
                Else
                    'Me.DisplayMessageInfo("No connection to the Server")
                End If
            End Sub

            Protected Overrides Sub ReceiveCallback(ByVal asyncResult As IAsyncResult)
                Try
                    Dim socket As Socket = CType(asyncResult.AsyncState, Socket)
                    Dim read As Integer = socket.EndReceive(asyncResult)
                    If read > 0 Then
                        If _receiveCallback IsNot Nothing Then
                            Dim data(read - 1) As Byte
                            Array.Copy(_readbuf, data, read)

                            _receiveCallback(Me, New AsyncEventArgs(data))
                        End If
                        Array.Clear(_readbuf, 0, _readbuf.Length)
                        _socketClient.BeginReceive(_readbuf, 0, _readbuf.Length, SocketFlags.None, AddressOf ReceiveCallback, _socketClient)
                    End If
                    If read = 0 AndAlso Not _localsocketClientIsShutingDown Then
                        _socketClient.Close()
                        'Me.DisplayMessageInfo("Server Socket is closing")
                    End If
                Catch ex As SocketException
                    ' Me.DisplayMessageError(ex.Message)
                Catch ex As ObjectDisposedException
                    'Me.DisplayMessageError(ex.Message)
                End Try
            End Sub

            Public Overrides Sub Close()
                If Not (_socketClient Is Nothing) AndAlso _socketClient.Connected Then
                    _localsocketClientIsShutingDown = True
                    _socketClient.Shutdown(SocketShutdown.Both)
                    _socketClient.Close()
                    _started = False
                    'Me.DisplayMessageInfo("Connection closed")
                End If
            End Sub
        End Class
    End Namespace

End Namespace
