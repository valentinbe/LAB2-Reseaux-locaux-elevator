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
            Me.ServerName = New System.Windows.Forms.TextBox
            Me.Valid = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'ServerName
            '
            Me.ServerName.Location = New System.Drawing.Point(12, 12)
            Me.ServerName.Name = "ServerName"
            Me.ServerName.Size = New System.Drawing.Size(199, 20)
            Me.ServerName.TabIndex = 0
            Me.ServerName.Text = "IP Address / Serveur Name"
            '
            'Valid
            '
            Me.Valid.Location = New System.Drawing.Point(83, 38)
            Me.Valid.Name = "Valid"
            Me.Valid.Size = New System.Drawing.Size(60, 23)
            Me.Valid.TabIndex = 1
            Me.Valid.Text = "OK"
            Me.Valid.UseVisualStyleBackColor = True
            '
            'ServerNameForm
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(223, 69)
            Me.Controls.Add(Me.Valid)
            Me.Controls.Add(Me.ServerName)
            Me.Name = "ServerNameForm"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Enter Server Name"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Private Sub Valid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Valid.Click
            Elevator.ServerName = Me.ServerName.Text()
            Me.Close()
        End Sub

        Private Sub ServerName_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ServerName.KeyUp
            If e.KeyCode = Keys.Enter Then
                Valid_Click(sender, e)
            End If
        End Sub
    End Class
End Namespace
