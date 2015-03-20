Imports System
Imports System.Drawing
Imports System.Windows.Forms

Namespace AsyncSocket

    Public Class ServerNameForm
        Inherits Form
        Friend WithEvents Valid As System.Windows.Forms.Button
        Private WithEvents ServerName As TextBox

        Public Sub New()
            InitializeComponent()

        End Sub

        Private Sub InitializeComponent()
            ServerName = New System.Windows.Forms.TextBox
            Valid = New System.Windows.Forms.Button
            SuspendLayout()
            '
            'ServerName
            '
            ServerName.Location = New System.Drawing.Point(12, 12)
            ServerName.Name = "ServerName"
            ServerName.Size = New System.Drawing.Size(199, 20)
            ServerName.TabIndex = 0
            ServerName.Text = "IP Address / Serveur Name"
            '
            'Valid
            '
            Valid.Location = New System.Drawing.Point(83, 38)
            Valid.Name = "Valid"
            Valid.Size = New System.Drawing.Size(60, 23)
            Valid.TabIndex = 1
            Valid.Text = "OK"
            Valid.UseVisualStyleBackColor = True
            '
            'ServerNameForm
            '
            AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            ClientSize = New System.Drawing.Size(223, 69)
            Controls.Add(Valid)
            Controls.Add(ServerName)
            Name = "ServerNameForm"
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Text = "Enter Server Name"
            ResumeLayout(False)
            PerformLayout()

        End Sub

        Private Sub Valid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Valid.Click
            Elevator.ServerName = ServerName.Text()
            Close()
        End Sub

        Private Sub ServerName_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ServerName.KeyUp
            If e.KeyCode = Keys.Enter Then
                Valid_Click(sender, e)
            End If
        End Sub
    End Class
End Namespace
