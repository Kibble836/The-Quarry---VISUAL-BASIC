<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Debugger
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
        Me.lstMap = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'lstMap
        '
        Me.lstMap.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstMap.FormattingEnabled = True
        Me.lstMap.Location = New System.Drawing.Point(12, 12)
        Me.lstMap.Name = "lstMap"
        Me.lstMap.Size = New System.Drawing.Size(743, 615)
        Me.lstMap.TabIndex = 0
        '
        'Debugger
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(771, 639)
        Me.Controls.Add(Me.lstMap)
        Me.Name = "Debugger"
        Me.Text = "Debugger"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lstMap As System.Windows.Forms.ListBox
End Class
