Public Class MenuPage

    'Menu Elements
    Dim Selection As Integer = 1    'Indicates Selection State
    Dim FPI As Integer = 1          'Frame per Interval
    Dim GamePort As New GamePage    'Object of GamePage

    'Entry Point of MenuPage
    Private Sub MenuPage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MenuEngine()
    End Sub

    'Setups a MenuPage
    Sub MenuEngine()
        ByteCore.FormRadius(Me, 55)
        Me.DoubleBuffered = True
        Label1.ForeColor = Color.White
        Label2.ForeColor = Color.FromArgb(122, 122, 139)
        Timer1.Interval = 3000
        Timer1.Start()
    End Sub

    'Loops the FPI 
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        'Animation for Y to I Transition
        If FPI = 1 Then
            Timer1.Interval = 100
            PictureBox2.Visible = False
            PictureBox1.Visible = False
            FPI += 1
        ElseIf FPI = 2 Then
            Timer1.Interval = 100
            PictureBox2.Visible = True
            PictureBox1.Visible = False
            FPI += 1
        ElseIf FPI = 3 Then
            Timer1.Interval = 100
            PictureBox2.Visible = False
            PictureBox1.Visible = False
            FPI += 1
        ElseIf FPI = 4 Then
            Timer1.Interval = 150
            PictureBox2.Visible = False
            PictureBox1.Visible = True
            FPI += 1
        ElseIf FPI = 5 Then
            Timer1.Interval = 100
            PictureBox2.Visible = False
            PictureBox1.Visible = False
            FPI += 1
        ElseIf FPI = 6 Then
            Timer1.Interval = 200
            PictureBox2.Visible = True
            PictureBox1.Visible = False
            FPI += 1
        ElseIf FPI = 7 Then
            Timer1.Interval = 100
            PictureBox2.Visible = False
            PictureBox1.Visible = False
            FPI += 1
        ElseIf FPI = 8 Then
            Timer1.Interval = 3000
            PictureBox2.Visible = True
            PictureBox1.Visible = False
            FPI += 1

            'Animation for I to Y Transition
        ElseIf FPI = 9 Then
            Timer1.Interval = 100
            PictureBox2.Visible = False
            PictureBox1.Visible = False
            FPI += 1
        ElseIf FPI = 10 Then
            Timer1.Interval = 100
            PictureBox2.Visible = False
            PictureBox1.Visible = True
            FPI += 1
        ElseIf FPI = 11 Then
            Timer1.Interval = 100
            PictureBox2.Visible = False
            PictureBox1.Visible = False
            FPI += 1
        ElseIf FPI = 12 Then
            Timer1.Interval = 150
            PictureBox2.Visible = True
            PictureBox1.Visible = False
            FPI += 1
        ElseIf FPI = 13 Then
            Timer1.Interval = 100
            PictureBox2.Visible = False
            PictureBox1.Visible = False
            FPI += 1
        ElseIf FPI = 14 Then
            Timer1.Interval = 200
            PictureBox2.Visible = False
            PictureBox1.Visible = True
            FPI += 1
        ElseIf FPI = 15 Then
            Timer1.Interval = 100
            PictureBox2.Visible = False
            PictureBox1.Visible = False
            FPI += 1
        ElseIf FPI = 16 Then
            Timer1.Interval = 3000
            PictureBox2.Visible = False
            PictureBox1.Visible = True
            FPI = 1
        End If

    End Sub

    'Player's Controls to Menu Selection
    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Up
                Selection = 1
                PictureBox3.Location = New Point(217, 234)
                Label1.ForeColor = Color.White
                Label2.ForeColor = Color.FromArgb(122, 122, 139)
            Case Keys.Down
                Selection = 2
                PictureBox3.Location = New Point(236, 266)
                Label1.ForeColor = Color.FromArgb(165, 195, 194)
                Label2.ForeColor = Color.White
            Case Keys.Enter
                If Selection = 1 Then
                    Me.Hide()
                    GamePage.Show()
                    'ElseIf Selection = 2 Then
                    'Me.Hide()
                    'Dim OptionsWindow As New Form4
                    'OptionsWindow.Show()
                End If
            Case Keys.Escape
                End
        End Select
    End Sub


End Class
