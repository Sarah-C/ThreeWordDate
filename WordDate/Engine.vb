Imports System.Text.RegularExpressions

Public Class Engine

    Public adjectives As AdjectivesClass = AdjectivesClass.Instance()
    Public nouns As NounsClass = NounsClass.Instance()

    Public variationCount As Long = adjectives.count() * adjectives.count() * nouns.count()
    Public div1 As Long = adjectives.count() * nouns.count()
    Public div2 As Long = nouns.count()

    Public Function getKeyphrase(ByVal value As Long) As String
        If value < 0 Or value >= variationCount Then
            Throw New Exception("Value too high! Needs to be less than " & variationCount)
        End If
        value = shuffleBits(value)
        Dim a As Long = value \ div1
        Dim b As Long = (value - (a * div1)) \ div2
        Dim c As Long = value - ((a * div1) + (b * div2))
        Dim word1 As String = firstCharToUpper(adjectives.listAndItem(0, a))
        Dim word2 As String = firstCharToUpper(adjectives.listAndItem(1, b))
        Dim word3 As String = firstCharToUpper(nouns.listAndItem(0, c))
        Return word1 & word2 & word3
    End Function

    Public Function firstCharToUpper(ByVal input As String) As String
        If String.IsNullOrEmpty(input) Then Return input
        Return input.First().ToString().ToUpper() & input.Substring(1)
    End Function

    Public Function getValue(ByVal text As String) As Long
        If Not Regex.IsMatch(text, "^[a-zA-Z]+$") Then Return -1
        Dim words As List(Of String) = Regex.Split(text, "(?<!^)(?=[A-Z])").Select(Function(x) x.ToLower()).ToList()
        If words.Count <> 3 Then Return -1
        If adjectives.index(0, words(0)) = -1 Then Return -1
        If adjectives.index(1, words(1)) = -1 Then Return -1
        If nouns.index(0, words(2)) = -1 Then Return -1
        'Dim r As New Regex("(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])")
        'Dim words As List(Of String) = r.Replace(text, " ").ToLower().Split(" "c).ToList()
        Dim d1 As Long = adjectives.index(0, words(0)) * div1
        Dim d2 As Long = adjectives.index(1, words(1)) * div2
        Dim d3 As Long = nouns.index(0, words(2))
        Return unshuffleBits(d1 + d2 + d3)
    End Function

    Public Function shuffleBits(ByVal v As Long) As Long
        Dim locations() As Integer = {31, 28, 25, 22, 19, 16, 13, 10, 7, 4, 1, 0, 30, 27, 24, 21, 18, 15, 12, 9, 6, 3, 29, 26, 23, 20, 17, 14, 11, 8, 5, 2}
        Return transposeBits(v, locations)
    End Function

    Public Function unshuffleBits(ByVal v As Long) As Long
        Dim locations() As Integer = {31, 19, 9, 30, 18, 8, 29, 17, 7, 28, 16, 6, 27, 15, 5, 26, 14, 4, 25, 13, 3, 24, 12, 2, 23, 11, 1, 22, 10, 0, 21, 20}
        Return transposeBits(v, locations)
    End Function

    Public Function transposeBits(ByVal v As Long, ByVal locations() As Integer) As Long
        Dim bitsIn() As Char = Convert.ToString(v, 2).PadLeft(32, "0").ToCharArray()
        Dim bitsOutString As String = ""
        For Each a As Long In locations
            bitsOutString = bitsOutString.Insert(0, bitsIn(a))
        Next
        Return Convert.ToUInt32(bitsOutString, 2)
    End Function

End Class
