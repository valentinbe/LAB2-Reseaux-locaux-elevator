' Epuration et amélioration du code grâce au travail de 
' CHASSINAT Adrien
' ING4 SE en 2013-2014

Imports RLI___TP2___Elevator.AsyncSocket
Imports RLI___TP2___Elevator.AsyncSocket.ClientSocket
Imports RLI___TP2___Elevator.AsyncSocket.ServerSocket

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Elevator
    Inherits System.Windows.Forms.Form

    Private _socket As AsynchronousSocket

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Elevator))
        Me.ConnectToServer = New System.Windows.Forms.Button()
        Me.LauchServer = New System.Windows.Forms.Button()
        Me.PanelConnexion = New System.Windows.Forms.Panel()
        Me.LabelConnexion = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PositionSensor3 = New System.Windows.Forms.Label()
        Me.PositionSensor2 = New System.Windows.Forms.Label()
        Me.PositionSensor1 = New System.Windows.Forms.Label()
        Me.PositionSensor0 = New System.Windows.Forms.Label()
        Me.ButtonCallFloor3 = New System.Windows.Forms.Button()
        Me.ButtonCallFloor2 = New System.Windows.Forms.Button()
        Me.ButtonCallFloor1 = New System.Windows.Forms.Button()
        Me.ButtonCallFloor0 = New System.Windows.Forms.Button()
        Me.PanelSensors = New System.Windows.Forms.Panel()
        Me.LabelLedSensor4 = New System.Windows.Forms.Label()
        Me.LedSensor4 = New System.Windows.Forms.Panel()
        Me.LabelLedSensor3 = New System.Windows.Forms.Label()
        Me.LabelLedSensor2 = New System.Windows.Forms.Label()
        Me.LabelLedSensor1 = New System.Windows.Forms.Label()
        Me.LabelLedSensor0 = New System.Windows.Forms.Label()
        Me.LedSensor3 = New System.Windows.Forms.Panel()
        Me.LedSensor2 = New System.Windows.Forms.Panel()
        Me.LedSensor1 = New System.Windows.Forms.Panel()
        Me.LedSensor0 = New System.Windows.Forms.Panel()
        Me.LabelSensors = New System.Windows.Forms.Label()
        Me.ElevatorPhys = New System.Windows.Forms.PictureBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.CoilDown = New System.Windows.Forms.CheckBox()
        Me.CoilUP = New System.Windows.Forms.CheckBox()
        Me.LabelCoils = New System.Windows.Forms.Label()
        Me.PositionSensor4 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.PanelConnexion.SuspendLayout
        CType(Me.PictureBox1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.PanelSensors.SuspendLayout
        CType(Me.ElevatorPhys,System.ComponentModel.ISupportInitialize).BeginInit
        Me.Panel1.SuspendLayout
        Me.SuspendLayout
        '
        'ConnectToServer
        '
        Me.ConnectToServer.ForeColor = System.Drawing.Color.Red
        Me.ConnectToServer.Location = New System.Drawing.Point(23, 37)
        Me.ConnectToServer.Name = "ConnectToServer"
        Me.ConnectToServer.Size = New System.Drawing.Size(77, 37)
        Me.ConnectToServer.TabIndex = 0
        Me.ConnectToServer.Text = "Connect to the Server"
        Me.ConnectToServer.UseVisualStyleBackColor = true
        '
        'LauchServer
        '
        Me.LauchServer.ForeColor = System.Drawing.Color.Red
        Me.LauchServer.Location = New System.Drawing.Point(23, 80)
        Me.LauchServer.Name = "LauchServer"
        Me.LauchServer.Size = New System.Drawing.Size(77, 38)
        Me.LauchServer.TabIndex = 1
        Me.LauchServer.Text = "Launch the Server"
        Me.LauchServer.UseVisualStyleBackColor = true
        '
        'PanelConnexion
        '
        Me.PanelConnexion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelConnexion.Controls.Add(Me.LabelConnexion)
        Me.PanelConnexion.Controls.Add(Me.LauchServer)
        Me.PanelConnexion.Controls.Add(Me.ConnectToServer)
        Me.PanelConnexion.Location = New System.Drawing.Point(641, 12)
        Me.PanelConnexion.Name = "PanelConnexion"
        Me.PanelConnexion.Size = New System.Drawing.Size(132, 134)
        Me.PanelConnexion.TabIndex = 2
        '
        'LabelConnexion
        '
        Me.LabelConnexion.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LabelConnexion.AutoSize = true
        Me.LabelConnexion.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.LabelConnexion.Location = New System.Drawing.Point(1, 0)
        Me.LabelConnexion.Name = "LabelConnexion"
        Me.LabelConnexion.Size = New System.Drawing.Size(117, 24)
        Me.LabelConnexion.TabIndex = 2
        Me.LabelConnexion.Text = "Connection"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"),System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(245, 222)
        Me.PictureBox1.TabIndex = 6
        Me.PictureBox1.TabStop = false
        '
        'PositionSensor3
        '
        Me.PositionSensor3.AutoSize = true
        Me.PositionSensor3.Location = New System.Drawing.Point(265, 175)
        Me.PositionSensor3.Name = "PositionSensor3"
        Me.PositionSensor3.Size = New System.Drawing.Size(78, 13)
        Me.PositionSensor3.TabIndex = 11
        Me.PositionSensor3.Text = "Sensor/Input 3"
        '
        'PositionSensor2
        '
        Me.PositionSensor2.AutoSize = true
        Me.PositionSensor2.Location = New System.Drawing.Point(265, 325)
        Me.PositionSensor2.Name = "PositionSensor2"
        Me.PositionSensor2.Size = New System.Drawing.Size(78, 13)
        Me.PositionSensor2.TabIndex = 12
        Me.PositionSensor2.Text = "Sensor/Input 2"
        '
        'PositionSensor1
        '
        Me.PositionSensor1.AutoSize = true
        Me.PositionSensor1.Location = New System.Drawing.Point(265, 475)
        Me.PositionSensor1.Name = "PositionSensor1"
        Me.PositionSensor1.Size = New System.Drawing.Size(78, 13)
        Me.PositionSensor1.TabIndex = 13
        Me.PositionSensor1.Text = "Sensor/Input 1"
        '
        'PositionSensor0
        '
        Me.PositionSensor0.AutoSize = true
        Me.PositionSensor0.Location = New System.Drawing.Point(265, 625)
        Me.PositionSensor0.Name = "PositionSensor0"
        Me.PositionSensor0.Size = New System.Drawing.Size(78, 13)
        Me.PositionSensor0.TabIndex = 14
        Me.PositionSensor0.Text = "Sensor/Input 0"
        '
        'ButtonCallFloor3
        '
        Me.ButtonCallFloor3.Location = New System.Drawing.Point(510, 85)
        Me.ButtonCallFloor3.Name = "ButtonCallFloor3"
        Me.ButtonCallFloor3.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCallFloor3.TabIndex = 15
        Me.ButtonCallFloor3.Text = "Call Floor 3"
        Me.ButtonCallFloor3.UseVisualStyleBackColor = true
        '
        'ButtonCallFloor2
        '
        Me.ButtonCallFloor2.Location = New System.Drawing.Point(510, 235)
        Me.ButtonCallFloor2.Name = "ButtonCallFloor2"
        Me.ButtonCallFloor2.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCallFloor2.TabIndex = 16
        Me.ButtonCallFloor2.Text = "Call Floor 2"
        Me.ButtonCallFloor2.UseVisualStyleBackColor = true
        '
        'ButtonCallFloor1
        '
        Me.ButtonCallFloor1.Location = New System.Drawing.Point(510, 385)
        Me.ButtonCallFloor1.Name = "ButtonCallFloor1"
        Me.ButtonCallFloor1.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCallFloor1.TabIndex = 17
        Me.ButtonCallFloor1.Text = "Call Floor 1"
        Me.ButtonCallFloor1.UseVisualStyleBackColor = true
        '
        'ButtonCallFloor0
        '
        Me.ButtonCallFloor0.Location = New System.Drawing.Point(510, 535)
        Me.ButtonCallFloor0.Name = "ButtonCallFloor0"
        Me.ButtonCallFloor0.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCallFloor0.TabIndex = 18
        Me.ButtonCallFloor0.Text = "Call Floor 0"
        Me.ButtonCallFloor0.UseVisualStyleBackColor = true
        '
        'PanelSensors
        '
        Me.PanelSensors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelSensors.Controls.Add(Me.LabelLedSensor4)
        Me.PanelSensors.Controls.Add(Me.LedSensor4)
        Me.PanelSensors.Controls.Add(Me.LabelLedSensor3)
        Me.PanelSensors.Controls.Add(Me.LabelLedSensor2)
        Me.PanelSensors.Controls.Add(Me.LabelLedSensor1)
        Me.PanelSensors.Controls.Add(Me.LabelLedSensor0)
        Me.PanelSensors.Controls.Add(Me.LedSensor3)
        Me.PanelSensors.Controls.Add(Me.LedSensor2)
        Me.PanelSensors.Controls.Add(Me.LedSensor1)
        Me.PanelSensors.Controls.Add(Me.LedSensor0)
        Me.PanelSensors.Controls.Add(Me.LabelSensors)
        Me.PanelSensors.Location = New System.Drawing.Point(26, 277)
        Me.PanelSensors.Name = "PanelSensors"
        Me.PanelSensors.Size = New System.Drawing.Size(155, 77)
        Me.PanelSensors.TabIndex = 19
        '
        'LabelLedSensor4
        '
        Me.LabelLedSensor4.AutoSize = true
        Me.LabelLedSensor4.Location = New System.Drawing.Point(121, 33)
        Me.LabelLedSensor4.Name = "LabelLedSensor4"
        Me.LabelLedSensor4.Size = New System.Drawing.Size(13, 13)
        Me.LabelLedSensor4.TabIndex = 13
        Me.LabelLedSensor4.Text = "4"
        '
        'LedSensor4
        '
        Me.LedSensor4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LedSensor4.Location = New System.Drawing.Point(118, 49)
        Me.LedSensor4.Name = "LedSensor4"
        Me.LedSensor4.Size = New System.Drawing.Size(20, 20)
        Me.LedSensor4.TabIndex = 12
        '
        'LabelLedSensor3
        '
        Me.LabelLedSensor3.AutoSize = true
        Me.LabelLedSensor3.Location = New System.Drawing.Point(96, 33)
        Me.LabelLedSensor3.Name = "LabelLedSensor3"
        Me.LabelLedSensor3.Size = New System.Drawing.Size(13, 13)
        Me.LabelLedSensor3.TabIndex = 11
        Me.LabelLedSensor3.Text = "3"
        '
        'LabelLedSensor2
        '
        Me.LabelLedSensor2.AutoSize = true
        Me.LabelLedSensor2.Location = New System.Drawing.Point(70, 33)
        Me.LabelLedSensor2.Name = "LabelLedSensor2"
        Me.LabelLedSensor2.Size = New System.Drawing.Size(13, 13)
        Me.LabelLedSensor2.TabIndex = 10
        Me.LabelLedSensor2.Text = "2"
        '
        'LabelLedSensor1
        '
        Me.LabelLedSensor1.AutoSize = true
        Me.LabelLedSensor1.Location = New System.Drawing.Point(44, 33)
        Me.LabelLedSensor1.Name = "LabelLedSensor1"
        Me.LabelLedSensor1.Size = New System.Drawing.Size(13, 13)
        Me.LabelLedSensor1.TabIndex = 9
        Me.LabelLedSensor1.Text = "1"
        '
        'LabelLedSensor0
        '
        Me.LabelLedSensor0.AutoSize = true
        Me.LabelLedSensor0.Location = New System.Drawing.Point(18, 33)
        Me.LabelLedSensor0.Name = "LabelLedSensor0"
        Me.LabelLedSensor0.Size = New System.Drawing.Size(13, 13)
        Me.LabelLedSensor0.TabIndex = 8
        Me.LabelLedSensor0.Text = "0"
        '
        'LedSensor3
        '
        Me.LedSensor3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LedSensor3.Location = New System.Drawing.Point(93, 49)
        Me.LedSensor3.Name = "LedSensor3"
        Me.LedSensor3.Size = New System.Drawing.Size(20, 20)
        Me.LedSensor3.TabIndex = 7
        '
        'LedSensor2
        '
        Me.LedSensor2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LedSensor2.Location = New System.Drawing.Point(67, 49)
        Me.LedSensor2.Name = "LedSensor2"
        Me.LedSensor2.Size = New System.Drawing.Size(20, 20)
        Me.LedSensor2.TabIndex = 6
        '
        'LedSensor1
        '
        Me.LedSensor1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LedSensor1.Location = New System.Drawing.Point(41, 49)
        Me.LedSensor1.Name = "LedSensor1"
        Me.LedSensor1.Size = New System.Drawing.Size(20, 20)
        Me.LedSensor1.TabIndex = 5
        '
        'LedSensor0
        '
        Me.LedSensor0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LedSensor0.Location = New System.Drawing.Point(15, 49)
        Me.LedSensor0.Name = "LedSensor0"
        Me.LedSensor0.Size = New System.Drawing.Size(20, 20)
        Me.LedSensor0.TabIndex = 4
        '
        'LabelSensors
        '
        Me.LabelSensors.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LabelSensors.AutoSize = true
        Me.LabelSensors.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.LabelSensors.Location = New System.Drawing.Point(3, 0)
        Me.LabelSensors.Name = "LabelSensors"
        Me.LabelSensors.Size = New System.Drawing.Size(131, 20)
        Me.LabelSensors.TabIndex = 3
        Me.LabelSensors.Text = "Sensors/Inputs"
        '
        'ElevatorPhys
        '
        Me.ElevatorPhys.BackColor = System.Drawing.SystemColors.Control
        Me.ElevatorPhys.BackgroundImage = CType(resources.GetObject("ElevatorPhys.BackgroundImage"),System.Drawing.Image)
        Me.ElevatorPhys.InitialImage = CType(resources.GetObject("ElevatorPhys.InitialImage"),System.Drawing.Image)
        Me.ElevatorPhys.Location = New System.Drawing.Point(355, 50)
        Me.ElevatorPhys.Name = "ElevatorPhys"
        Me.ElevatorPhys.Size = New System.Drawing.Size(140, 140)
        Me.ElevatorPhys.TabIndex = 21
        Me.ElevatorPhys.TabStop = false
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.CoilDown)
        Me.Panel1.Controls.Add(Me.CoilUP)
        Me.Panel1.Controls.Add(Me.LabelCoils)
        Me.Panel1.Location = New System.Drawing.Point(26, 385)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(155, 77)
        Me.Panel1.TabIndex = 20
        '
        'CoilDown
        '
        Me.CoilDown.AutoSize = true
        Me.CoilDown.Location = New System.Drawing.Point(49, 48)
        Me.CoilDown.Name = "CoilDown"
        Me.CoilDown.Size = New System.Drawing.Size(54, 17)
        Me.CoilDown.TabIndex = 5
        Me.CoilDown.Text = "Down"
        Me.CoilDown.UseVisualStyleBackColor = true
        '
        'CoilUP
        '
        Me.CoilUP.AutoSize = true
        Me.CoilUP.Location = New System.Drawing.Point(49, 24)
        Me.CoilUP.Name = "CoilUP"
        Me.CoilUP.Size = New System.Drawing.Size(41, 17)
        Me.CoilUP.TabIndex = 4
        Me.CoilUP.Text = "UP"
        Me.CoilUP.UseVisualStyleBackColor = true
        '
        'LabelCoils
        '
        Me.LabelCoils.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LabelCoils.AutoSize = true
        Me.LabelCoils.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.LabelCoils.Location = New System.Drawing.Point(8, 0)
        Me.LabelCoils.Name = "LabelCoils"
        Me.LabelCoils.Size = New System.Drawing.Size(117, 20)
        Me.LabelCoils.TabIndex = 3
        Me.LabelCoils.Text = "Coils/Outputs"
        '
        'PositionSensor4
        '
        Me.PositionSensor4.AutoSize = true
        Me.PositionSensor4.Location = New System.Drawing.Point(265, 25)
        Me.PositionSensor4.Name = "PositionSensor4"
        Me.PositionSensor4.Size = New System.Drawing.Size(78, 13)
        Me.PositionSensor4.TabIndex = 22
        Me.PositionSensor4.Text = "Sensor/Input 4"
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.ForeColor = System.Drawing.SystemColors.ButtonShadow
        Me.Panel2.Location = New System.Drawing.Point(344, 14)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(162, 629)
        Me.Panel2.TabIndex = 23
        '
        'Timer1
        '
        Me.Timer1.Interval = 2000
        '
        'Elevator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(787, 655)
        Me.Controls.Add(Me.PositionSensor4)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ElevatorPhys)
        Me.Controls.Add(Me.PanelSensors)
        Me.Controls.Add(Me.ButtonCallFloor0)
        Me.Controls.Add(Me.ButtonCallFloor1)
        Me.Controls.Add(Me.ButtonCallFloor2)
        Me.Controls.Add(Me.ButtonCallFloor3)
        Me.Controls.Add(Me.PositionSensor0)
        Me.Controls.Add(Me.PositionSensor1)
        Me.Controls.Add(Me.PositionSensor2)
        Me.Controls.Add(Me.PositionSensor3)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.PanelConnexion)
        Me.Controls.Add(Me.Panel2)
        Me.Name = "Elevator"
        Me.Text = "Elevator - RLI - TP3"
        Me.PanelConnexion.ResumeLayout(false)
        Me.PanelConnexion.PerformLayout
        CType(Me.PictureBox1,System.ComponentModel.ISupportInitialize).EndInit
        Me.PanelSensors.ResumeLayout(false)
        Me.PanelSensors.PerformLayout
        CType(Me.ElevatorPhys,System.ComponentModel.ISupportInitialize).EndInit
        Me.Panel1.ResumeLayout(false)
        Me.Panel1.PerformLayout
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents ConnectToServer As System.Windows.Forms.Button
    Friend WithEvents LauchServer As System.Windows.Forms.Button
    Friend WithEvents PanelConnexion As System.Windows.Forms.Panel
    Friend WithEvents LabelConnexion As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PositionSensor3 As System.Windows.Forms.Label
    Friend WithEvents PositionSensor2 As System.Windows.Forms.Label
    Friend WithEvents PositionSensor1 As System.Windows.Forms.Label
    Friend WithEvents PositionSensor0 As System.Windows.Forms.Label
    Friend WithEvents ButtonCallFloor3 As System.Windows.Forms.Button
    Friend WithEvents ButtonCallFloor2 As System.Windows.Forms.Button
    Friend WithEvents ButtonCallFloor1 As System.Windows.Forms.Button
    Friend WithEvents ButtonCallFloor0 As System.Windows.Forms.Button
    Friend WithEvents PanelSensors As System.Windows.Forms.Panel
    Friend WithEvents LabelSensors As System.Windows.Forms.Label
    Friend WithEvents ElevatorPhys As System.Windows.Forms.PictureBox
    Friend WithEvents LedSensor0 As System.Windows.Forms.Panel
    Friend WithEvents LedSensor1 As System.Windows.Forms.Panel
    Friend WithEvents LabelLedSensor3 As System.Windows.Forms.Label
    Friend WithEvents LabelLedSensor2 As System.Windows.Forms.Label
    Friend WithEvents LabelLedSensor1 As System.Windows.Forms.Label
    Friend WithEvents LabelLedSensor0 As System.Windows.Forms.Label
    Friend WithEvents LedSensor3 As System.Windows.Forms.Panel
    Friend WithEvents LedSensor2 As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents LabelCoils As System.Windows.Forms.Label
    Friend WithEvents LabelLedSensor4 As System.Windows.Forms.Label
    Friend WithEvents LedSensor4 As System.Windows.Forms.Panel
    Friend WithEvents PositionSensor4 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Public WithEvents CoilDown As System.Windows.Forms.CheckBox
    Friend WithEvents CoilUP As System.Windows.Forms.CheckBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
