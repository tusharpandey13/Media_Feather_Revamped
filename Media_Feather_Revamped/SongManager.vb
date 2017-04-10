Class SongManager : Implements IDisposable
    Public Media As New media.Media
    Public Songs As Dictionary(Of Integer, Song) = New Dictionary(Of Integer, Song)
    Public CurrentIndex%
    Public Randomized As Byte = 0

    Public Property Volume%
        Get
            Return Media.Volume
        End Get
        Set(value%)
            Media.Volume = clamp(value, 0, 100)
        End Set
    End Property
    Public ReadOnly Property State As media.Media.State
        Get
            Return Media.Status
        End Get
    End Property


    Public Sub OneSecondElapsed()
        Songs.Values(CurrentIndex).Elapsed += 1
    End Sub
    Public Sub LoadSongs(SongList As List(Of String))
        For i = 0 To SongList.Count - 1
            Songs.Add(i, New Song() With {.Path = SongList(i), .Name = IO.Path.GetFileNameWithoutExtension(.Path)})
        Next
    End Sub
    Public Function GetSongs(Optional Filter$ = "") As Dictionary(Of Integer, Song)
        Dim td As New Dictionary(Of Integer, Song)
        For Each i% In Songs.Keys
            If Songs.Values(i).Name.Contains(Filter) Then td.Add(i, Songs.Values(i))
        Next
        Return td
    End Function
    Public Function StartPlayback(index%) As Boolean
        Media.Close()
        Media.Open(Songs.Values(index).Path)
        Media.Play()
        CurrentIndex = index
        Return 1
    End Function
    Public Sub TogglePlayPause()
        If Media.Status = 1 Then ' if playing
            Media.Pause()
        ElseIf Media.Status = 2 Then ' if paused
            Media.Resume()
        ElseIf Media.Status = 0 Then ' if closed
            StartPlayback(Songs.Values(CurrentIndex).Name)
        ElseIf Media.Status = 3 Then 'if stopped
            Media.Play()
        End If
    End Sub
    Public Sub PreviousTrack()
        Media.Stop()
        If Not Randomized Then
            StartPlayback(CurrentIndex - 1)
        Else

        End If
    End Sub
    Public Sub NextTrack()
        Media.Stop()
        If Not Randomized Then
            StartPlayback(CurrentIndex + 1)
        Else

        End If
    End Sub
    Public Sub StopPlayback()
        Media.Stop()
    End Sub




    Public Sub Dispose() Implements IDisposable.Dispose
        Media.Stop()
        Media.Close()
        Songs.Clear()
        Songs = Nothing
        Randomized = 0
    End Sub
End Class


Class Song
    Public Name$
    Public Path$
    Public Duration%
    Public Property Elapsed%

    Public Class EqualityComparer
        Implements IEqualityComparer(Of Song)

        Public Overloads Function Equals(x As Song, y As Song) As Boolean Implements IEqualityComparer(Of Song).Equals
            Return x.Path.ToLower = y.Path.ToLower
        End Function

        Public Overloads Function GetHashCode(obj As Song) As Integer Implements IEqualityComparer(Of Song).GetHashCode
            Return IO.Path.GetFileName(obj.Path).GetHashCode
        End Function
    End Class

End Class