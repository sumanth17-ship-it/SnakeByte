Public Class LoadingPage
    Dim Bridge As String
    Dim Bar As Integer
    Dim MenuPage As New MenuPage
    Dim GamePage As New GamePage
    Dim Assets(20) As Object
    Dim AssetName(20) As String
    Dim Existence As Boolean
    Dim Length As Integer
    Dim Index As Integer = 0
    Dim Label1 As Label
    Dim Label2 As Label
    Dim PictureBox1 As PictureBox
    Dim PictureBox2 As PictureBox
    Dim PictureBox3 As PictureBox
    Dim Form As Form

    Private Sub LoadingPage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ByteCore.FormRadius(Me, 55)
        Me.DoubleBuffered = True
        LoadEngine()
    End Sub

    Sub LoadEngine()
        Bridge = ByteCore.Bridge
        AssetLoad()
        Timer1.Interval = 300
        Timer1.Start()
    End Sub

    Sub AssetLoad()
        If Bridge = "MenuPage" Then
            Form = MenuPage

            Assets(0) = Label1
            AssetName(0) = "Package UITxt01"

            Assets(1) = Label2
            AssetName(1) = "Package UITxt02"

            Assets(2) = PictureBox1
            AssetName(2) = "Package UIImg01"

            Assets(3) = PictureBox2
            AssetName(3) = "Package UIImg02"

            Assets(4) = PictureBox3
            AssetName(4) = "Package UIImg03"

            Length = 5
        End If
    End Sub

    Function AssetExistence(ByVal Page As Form) As Boolean
        Return Existence = Page.Controls.Contains(DirectCast(Assets(Index), Control))
    End Function

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Index = Length Then
            Timer1.Stop()
            PageTransition()
        End If
        If Bar <= 100 AndAlso Index < Length AndAlso AssetExistence(MenuPage) Then
            Bar += 10
            Button1.Size = New Size(Button1.Size.Width + 20, Button1.Size.Height)
            Label11.Text = "Building " & AssetName(Index)
            Index += 1
        Else
            Timer1.Stop()
        End If
    End Sub

    Sub PageTransition()
        If Bridge = "MenuPage" Then
            Me.Close()
            MenuPage.Show()
        End If
    End Sub

End Class