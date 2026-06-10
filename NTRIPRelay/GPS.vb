Public Class GPS
    Public Function GenerateGPGGAcode() As String
        Dim posnum As Double = 0
        Dim minutes As Double = 0

        Dim UTCTime As Date = Date.UtcNow

        '$GPGGA,052158,4158.7333,N,09147.4277,W,2,08,3.1,260.4,M,-32.6,M,,*79

        Dim mycode As String = "GPGGA,"
        If Hour(UTCTime) < "10" Then
            mycode = mycode & "0"
        End If
        mycode = mycode & Hour(UTCTime)
        If Minute(UTCTime) < "10" Then
            mycode = mycode & "0"
        End If
        mycode = mycode & Minute(UTCTime)
        If Second(UTCTime) < "10" Then
            mycode = mycode & "0"
        End If
        mycode = mycode & Second(UTCTime)
        mycode = mycode & ","


        posnum = Math.Abs(Form1.NTRIPManualLat)
        minutes = posnum Mod 1
        posnum = posnum - minutes
        minutes = minutes * 60
        posnum = (posnum * 100) + minutes
        If posnum < 1000 Then
            mycode = mycode & "0"
            If posnum < 100 Then
                mycode = mycode & "0"
            End If
        End If
        mycode = mycode & posnum.ToString

        If Form1.NTRIPManualLat > 0 Then
            mycode = mycode & ",N,"
        Else
            mycode = mycode & ",S,"
        End If

        posnum = Math.Abs(Form1.NTRIPManualLon)
        minutes = posnum Mod 1
        posnum = posnum - minutes
        minutes = minutes * 60
        posnum = (posnum * 100) + minutes
        If posnum < 10000 Then
            mycode = mycode & "0"
            If posnum < 1000 Then
                mycode = mycode & "0"
                If posnum < 100 Then
                    mycode = mycode & "0"
                End If
            End If
        End If
        mycode = mycode & posnum.ToString

        If Form1.NTRIPManualLon > 0 Then
            mycode = mycode & ",E,"
        Else
            mycode = mycode & ",W,"
        End If

        mycode = mycode & "4,10,1,200,M,1,M,"

        mycode = mycode & (Second(Now) Mod 6) + 3 & ",0"


        mycode = "$" & mycode & "*" & CalculateChecksum(mycode)   'Add checksum data
        Return mycode
    End Function


    Public Function CalculateChecksum(ByVal sentence As String) As String
        ' Calculates the checksum for a sentence
        ' Loop through all chars to get a checksum
        Dim Character As Char
        Dim Checksum As Integer
        For Each Character In sentence
            Select Case Character
                Case "$"c
                    ' Ignore the dollar sign
                Case "*"c
                    ' Stop processing before the asterisk
                    Exit For
                Case Else
                    ' Is this the first value for the checksum?
                    If Checksum = 0 Then
                        ' Yes. Set the checksum to the value
                        Checksum = Convert.ToByte(Character)
                    Else
                        ' No. XOR the checksum with this character's value
                        Checksum = Checksum Xor Convert.ToByte(Character)
                    End If
            End Select
        Next
        ' Return the checksum formatted as a two-character hexadecimal
        Return Checksum.ToString("X2")
    End Function

End Class
