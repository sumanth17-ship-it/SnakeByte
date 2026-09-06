Module ByteCore
    Public Bridge As String

    Public Sub FormRadius(ByVal Form As Form, ByVal radius As Integer)
        Dim path As New Drawing2D.GraphicsPath()

        path.AddArc(0, 0, radius, radius, 180, 90)
        path.AddArc(Form.Width - radius, 0, radius, radius, 270, 90)
        path.AddArc(Form.Width - radius, Form.Height - radius, radius, radius, 0, 90)
        path.AddArc(0, Form.Height - radius, radius, radius, 90, 90)
        path.CloseAllFigures()

        Form.Region = New Region(path)
    End Sub

    Sub Loading(ByVal Pathway As String)
        Bridge = Pathway
    End Sub
End Module
