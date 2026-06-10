<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.LineTop = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabStream = New System.Windows.Forms.TabPage
        Me.lblServerStatus = New System.Windows.Forms.Label
        Me.lblServerName = New System.Windows.Forms.Label
        Me.pbServer = New System.Windows.Forms.ProgressBar
        Me.pbNTRIP = New System.Windows.Forms.ProgressBar
        Me.Label8 = New System.Windows.Forms.Label
        Me.lblServerUptime = New System.Windows.Forms.Label
        Me.btnServerEdit = New System.Windows.Forms.Button
        Me.btnServerConnect = New System.Windows.Forms.Button
        Me.lblClientUptime = New System.Windows.Forms.Label
        Me.lblNTRIPStatus = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.lblClientName = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnNTRIPEdit = New System.Windows.Forms.Button
        Me.btnNTRIPConnect = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.boxMountpoint = New System.Windows.Forms.ComboBox
        Me.TabLog = New System.Windows.Forms.TabPage
        Me.rtbEvents = New System.Windows.Forms.RichTextBox
        Me.TabControl1.SuspendLayout()
        Me.TabStream.SuspendLayout()
        Me.TabLog.SuspendLayout()
        Me.SuspendLayout()
        '
        'LineTop
        '
        Me.LineTop.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineTop.BackColor = System.Drawing.Color.Black
        Me.LineTop.Location = New System.Drawing.Point(0, 136)
        Me.LineTop.Margin = New System.Windows.Forms.Padding(0)
        Me.LineTop.Name = "LineTop"
        Me.LineTop.Size = New System.Drawing.Size(590, 2)
        Me.LineTop.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 20)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Source Caster:"
        '
        'Timer1
        '
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabStream)
        Me.TabControl1.Controls.Add(Me.TabLog)
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(598, 266)
        Me.TabControl1.TabIndex = 11
        '
        'TabStream
        '
        Me.TabStream.BackColor = System.Drawing.Color.White
        Me.TabStream.Controls.Add(Me.lblServerStatus)
        Me.TabStream.Controls.Add(Me.lblServerName)
        Me.TabStream.Controls.Add(Me.pbServer)
        Me.TabStream.Controls.Add(Me.pbNTRIP)
        Me.TabStream.Controls.Add(Me.Label8)
        Me.TabStream.Controls.Add(Me.lblServerUptime)
        Me.TabStream.Controls.Add(Me.btnServerEdit)
        Me.TabStream.Controls.Add(Me.btnServerConnect)
        Me.TabStream.Controls.Add(Me.lblClientUptime)
        Me.TabStream.Controls.Add(Me.lblNTRIPStatus)
        Me.TabStream.Controls.Add(Me.Label5)
        Me.TabStream.Controls.Add(Me.lblClientName)
        Me.TabStream.Controls.Add(Me.Label3)
        Me.TabStream.Controls.Add(Me.btnNTRIPEdit)
        Me.TabStream.Controls.Add(Me.btnNTRIPConnect)
        Me.TabStream.Controls.Add(Me.Label2)
        Me.TabStream.Controls.Add(Me.boxMountpoint)
        Me.TabStream.Controls.Add(Me.Label1)
        Me.TabStream.Controls.Add(Me.LineTop)
        Me.TabStream.Location = New System.Drawing.Point(4, 22)
        Me.TabStream.Name = "TabStream"
        Me.TabStream.Padding = New System.Windows.Forms.Padding(3)
        Me.TabStream.Size = New System.Drawing.Size(590, 240)
        Me.TabStream.TabIndex = 0
        Me.TabStream.Text = "Client and Server"
        '
        'lblServerStatus
        '
        Me.lblServerStatus.AutoSize = True
        Me.lblServerStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServerStatus.Location = New System.Drawing.Point(75, 184)
        Me.lblServerStatus.Name = "lblServerStatus"
        Me.lblServerStatus.Size = New System.Drawing.Size(107, 20)
        Me.lblServerStatus.TabIndex = 82
        Me.lblServerStatus.Text = "Disconnected"
        '
        'lblServerName
        '
        Me.lblServerName.AutoSize = True
        Me.lblServerName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServerName.Location = New System.Drawing.Point(159, 151)
        Me.lblServerName.Name = "lblServerName"
        Me.lblServerName.Size = New System.Drawing.Size(208, 20)
        Me.lblServerName.TabIndex = 81
        Me.lblServerName.Text = "127.0.0.1:10000 /STREAM1"
        '
        'pbServer
        '
        Me.pbServer.Location = New System.Drawing.Point(278, 214)
        Me.pbServer.Name = "pbServer"
        Me.pbServer.Size = New System.Drawing.Size(309, 23)
        Me.pbServer.TabIndex = 80
        Me.pbServer.Visible = False
        '
        'pbNTRIP
        '
        Me.pbNTRIP.Location = New System.Drawing.Point(278, 102)
        Me.pbNTRIP.Name = "pbNTRIP"
        Me.pbNTRIP.Size = New System.Drawing.Size(309, 23)
        Me.pbNTRIP.TabIndex = 79
        Me.pbNTRIP.Visible = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(9, 184)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(60, 20)
        Me.Label8.TabIndex = 77
        Me.Label8.Text = "Status:"
        '
        'lblServerUptime
        '
        Me.lblServerUptime.AutoSize = True
        Me.lblServerUptime.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServerUptime.Location = New System.Drawing.Point(9, 217)
        Me.lblServerUptime.Name = "lblServerUptime"
        Me.lblServerUptime.Size = New System.Drawing.Size(64, 20)
        Me.lblServerUptime.TabIndex = 76
        Me.lblServerUptime.Text = "Uptime:"
        '
        'btnServerEdit
        '
        Me.btnServerEdit.BackColor = System.Drawing.Color.LightGray
        Me.btnServerEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnServerEdit.Location = New System.Drawing.Point(490, 148)
        Me.btnServerEdit.Name = "btnServerEdit"
        Me.btnServerEdit.Size = New System.Drawing.Size(97, 27)
        Me.btnServerEdit.TabIndex = 73
        Me.btnServerEdit.Text = "Edit"
        Me.btnServerEdit.UseVisualStyleBackColor = False
        '
        'btnServerConnect
        '
        Me.btnServerConnect.BackColor = System.Drawing.Color.LightGray
        Me.btnServerConnect.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnServerConnect.Location = New System.Drawing.Point(490, 181)
        Me.btnServerConnect.Name = "btnServerConnect"
        Me.btnServerConnect.Size = New System.Drawing.Size(97, 27)
        Me.btnServerConnect.TabIndex = 72
        Me.btnServerConnect.Text = "Connect"
        Me.btnServerConnect.UseVisualStyleBackColor = False
        '
        'lblClientUptime
        '
        Me.lblClientUptime.AutoSize = True
        Me.lblClientUptime.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientUptime.Location = New System.Drawing.Point(9, 105)
        Me.lblClientUptime.Name = "lblClientUptime"
        Me.lblClientUptime.Size = New System.Drawing.Size(64, 20)
        Me.lblClientUptime.TabIndex = 71
        Me.lblClientUptime.Text = "Uptime:"
        '
        'lblNTRIPStatus
        '
        Me.lblNTRIPStatus.AutoSize = True
        Me.lblNTRIPStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNTRIPStatus.Location = New System.Drawing.Point(75, 72)
        Me.lblNTRIPStatus.Name = "lblNTRIPStatus"
        Me.lblNTRIPStatus.Size = New System.Drawing.Size(107, 20)
        Me.lblNTRIPStatus.TabIndex = 70
        Me.lblNTRIPStatus.Text = "Disconnected"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(8, 72)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 20)
        Me.Label5.TabIndex = 67
        Me.Label5.Text = "Status:"
        '
        'lblClientName
        '
        Me.lblClientName.AutoSize = True
        Me.lblClientName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientName.Location = New System.Drawing.Point(129, 6)
        Me.lblClientName.Name = "lblClientName"
        Me.lblClientName.Size = New System.Drawing.Size(124, 20)
        Me.lblClientName.TabIndex = 66
        Me.lblClientName.Text = "127.0.0.1:10000"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 39)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 20)
        Me.Label3.TabIndex = 65
        Me.Label3.Text = "Stream:"
        '
        'btnNTRIPEdit
        '
        Me.btnNTRIPEdit.BackColor = System.Drawing.Color.LightGray
        Me.btnNTRIPEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNTRIPEdit.Location = New System.Drawing.Point(490, 3)
        Me.btnNTRIPEdit.Name = "btnNTRIPEdit"
        Me.btnNTRIPEdit.Size = New System.Drawing.Size(97, 27)
        Me.btnNTRIPEdit.TabIndex = 64
        Me.btnNTRIPEdit.Text = "Edit"
        Me.btnNTRIPEdit.UseVisualStyleBackColor = False
        '
        'btnNTRIPConnect
        '
        Me.btnNTRIPConnect.BackColor = System.Drawing.Color.LightGray
        Me.btnNTRIPConnect.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNTRIPConnect.Location = New System.Drawing.Point(490, 36)
        Me.btnNTRIPConnect.Name = "btnNTRIPConnect"
        Me.btnNTRIPConnect.Size = New System.Drawing.Size(97, 27)
        Me.btnNTRIPConnect.TabIndex = 63
        Me.btnNTRIPConnect.Text = "Connect"
        Me.btnNTRIPConnect.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(8, 151)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(145, 20)
        Me.Label2.TabIndex = 45
        Me.Label2.Text = "Destination Caster:"
        '
        'boxMountpoint
        '
        Me.boxMountpoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.boxMountpoint.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxMountpoint.FormattingEnabled = True
        Me.boxMountpoint.Location = New System.Drawing.Point(79, 36)
        Me.boxMountpoint.Name = "boxMountpoint"
        Me.boxMountpoint.Size = New System.Drawing.Size(405, 28)
        Me.boxMountpoint.TabIndex = 44
        '
        'TabLog
        '
        Me.TabLog.BackColor = System.Drawing.Color.White
        Me.TabLog.Controls.Add(Me.rtbEvents)
        Me.TabLog.Location = New System.Drawing.Point(4, 22)
        Me.TabLog.Name = "TabLog"
        Me.TabLog.Padding = New System.Windows.Forms.Padding(3)
        Me.TabLog.Size = New System.Drawing.Size(590, 240)
        Me.TabLog.TabIndex = 1
        Me.TabLog.Text = "Event Log"
        '
        'rtbEvents
        '
        Me.rtbEvents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbEvents.BackColor = System.Drawing.Color.White
        Me.rtbEvents.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbEvents.ForeColor = System.Drawing.Color.Black
        Me.rtbEvents.Location = New System.Drawing.Point(0, 0)
        Me.rtbEvents.Name = "rtbEvents"
        Me.rtbEvents.ReadOnly = True
        Me.rtbEvents.Size = New System.Drawing.Size(594, 287)
        Me.rtbEvents.TabIndex = 0
        Me.rtbEvents.Text = "Events will show up here."
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(598, 265)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(614, 303)
        Me.MinimumSize = New System.Drawing.Size(614, 303)
        Me.Name = "Form1"
        Me.Text = "NTRIP Relay"
        Me.TabControl1.ResumeLayout(False)
        Me.TabStream.ResumeLayout(False)
        Me.TabStream.PerformLayout()
        Me.TabLog.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LineTop As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabStream As System.Windows.Forms.TabPage
    Friend WithEvents TabLog As System.Windows.Forms.TabPage
    Friend WithEvents rtbEvents As System.Windows.Forms.RichTextBox
    Friend WithEvents boxMountpoint As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblClientName As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnNTRIPEdit As System.Windows.Forms.Button
    Friend WithEvents btnNTRIPConnect As System.Windows.Forms.Button
    Friend WithEvents lblClientUptime As System.Windows.Forms.Label
    Friend WithEvents lblNTRIPStatus As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents pbServer As System.Windows.Forms.ProgressBar
    Friend WithEvents pbNTRIP As System.Windows.Forms.ProgressBar
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblServerUptime As System.Windows.Forms.Label
    Friend WithEvents btnServerEdit As System.Windows.Forms.Button
    Friend WithEvents btnServerConnect As System.Windows.Forms.Button
    Friend WithEvents lblServerStatus As System.Windows.Forms.Label
    Friend WithEvents lblServerName As System.Windows.Forms.Label

End Class
