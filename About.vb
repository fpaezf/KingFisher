Public Class About
    Private Sub About_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.TopMost = False
        Me.WindowState = FormWindowState.Minimized
        Me.ShowInTaskbar = False
        Me.Hide()
        Bird.Show()
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Bird.Animation.Enabled = False
        Me.Show()
        Me.Visible = True
        Me.ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
        Me.TopMost = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Bird.ShowDebug = False
        NotifyIcon1.Visible = False
        Bird.StopAllAnimations()
        Bird.PictureBox1.Image = My.Resources.Dead
        Bird.Is_Dead = True
        Bird.Animation.Enabled = True
        Bird.Select()
        Me.Hide()
    End Sub

    Private Sub About_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Bird.Animation.Enabled = False
        Dim result As DialogResult = MessageBox.Show("Kill the bird and close application?", "Please confirm", MessageBoxButtons.YesNo)
        If (result = DialogResult.Yes) Then
            Bird.Animation.Enabled = True
            e.Cancel = True
            Button1.PerformClick()
        Else
            e.Cancel = True
            Bird.Animation.Enabled = True
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.WindowState = FormWindowState.Minimized
        Me.ShowInTaskbar = False
        Me.TopMost = False
        Me.Hide()
        Bird.Animation.Enabled = True
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            Bird.PlaySounds = True
        Else
            Bird.PlaySounds = False
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            Bird.ShowDebug = True
            Bird.Select()
        Else
            Bird.ShowDebug = False
        End If
    End Sub
End Class