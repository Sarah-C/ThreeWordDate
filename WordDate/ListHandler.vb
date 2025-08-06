Public MustInherit Class ListHandler

    Private lists As New Dictionary(Of Integer, List(Of Integer))
    Private straightList As List(Of String) = Nothing
    Private wordCount As ULong = 0

    Friend MustOverride ReadOnly Property longString() As String

    Public Function count() As ULong
        If wordCount = 0 Then wordCount = (longString.Length - longString.Replace(vbCrLf, "").Length) / 2
        Return wordCount
    End Function

    Public Function listAndItem(ByVal listNumber As Integer, ByVal itemNumber As Integer) As String
        If Not lists.ContainsKey(listNumber) Then makeRndListIndex(listNumber)
        Return item(lists(listNumber)(itemNumber))
    End Function

    Public Function index(ByVal listNumber As Integer, ByVal text As String) As Integer
        If straightList Is Nothing Then makeStraightList()
        If Not lists.ContainsKey(listNumber) Then makeRndListIndex(listNumber)
        Dim pos As Integer = straightList.IndexOf(text)
        If pos >= 0 Then
            Return lists(listNumber).IndexOf(pos)
        Else
            Return -1
        End If
    End Function

    Public Sub makeRndListIndex(ByVal listNumber As Integer)
        lists(listNumber) = randomListIndexes(count(), listNumber)
    End Sub

    Public Sub makeStraightList()
        straightList = longString.Split(New String() {vbCrLf}, StringSplitOptions.None).ToList().Skip(1).ToList()
    End Sub

    Public Function item(ByVal index As Integer) As String
        If straightList Is Nothing Then makeStraightList()
        Return straightList(index)
    End Function

    Public Function randomListIndexes(ByVal listLength As Integer, ByVal seed As Integer) As List(Of Integer)
        Dim rng As New Random(seed)
        Dim intList As New List(Of Integer)
        Dim index As Integer = listLength - 1
        While index >= 0
            intList.Add(index)
            index -= 1
        End While
        Do While listLength > 1
            listLength -= 1
            Dim k As Integer = rng.Next(listLength + 1)
            Dim value As Integer = intList(k)
            intList(k) = intList(listLength)
            intList(listLength) = value
        Loop
        Return intList
    End Function

End Class
