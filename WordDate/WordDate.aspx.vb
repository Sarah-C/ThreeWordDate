Public Class WordDate
    Inherits System.Web.UI.Page

    Public output As String = "error"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim data As String = Request("data")
        If data IsNot Nothing Then
            If data.Contains(",") Then
                processNumbers(data)
            Else
                processWords(data)
            End If
        End If
        Response.Write(output)
    End Sub

    Public Sub processNumbers(ByVal text As String)
        If Not Regex.IsMatch(text, "^[0-9,]+$") Then Return
        Dim e As New Engine()
        Dim data As List(Of String) = text.Split({","c}).ToList()
        Dim dt As Date = New Date(data(0), data(1), data(2), data(3), data(4), data(5))
        Dim t As TimeSpan = dt - New Date(1970, 1, 1)
        Dim secondsSinceEpoch As Long = t.TotalSeconds
        output = e.getKeyphrase(secondsSinceEpoch)
    End Sub

    Public Sub processWords(ByVal text As String)
        Dim e As New Engine()
        Dim epoch = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        Dim unixTime As Long = e.getValue(text)
        If unixTime = -1 Then Return
        Dim d As Date = epoch.AddSeconds(unixTime)
        Dim s As New StringBuilder()
        s.Append(d.Year & ",")
        s.Append(d.Month.ToString("D2") & ",")
        s.Append(d.Day.ToString("D2") & ",")
        s.Append(d.Hour.ToString("D2") & ",")
        s.Append(d.Minute.ToString("D2") & ",")
        s.Append(d.Second.ToString("D2"))
        output = s.ToString()
    End Sub

End Class