Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Net.Sockets
Imports System.Net
Imports System.Xml
Imports System.Io
Namespace Internal
    Friend Class OutputHelper
        Public Sub DebugOutput(ByVal DebugText As String)
            Console.WriteLine("[" + Timer.ToString + "] " + DebugText)
        End Sub
    End Class
    Friend Class StringHelper
        Public Function MD5Hex(ByVal stringValue As String)
            Dim md5 As New System.Security.Cryptography.MD5CryptoServiceProvider
            Dim Data As Byte()
            Dim Result As Byte()
            Dim Res As String = ""
            Dim tmp As String = ""

            Data = Encoding.ASCII.GetBytes(stringValue)
            Result = md5.ComputeHash(Data)

            For i As Integer = 0 To Result.Length - 1
                tmp = Hex(Result(i))
                If Len(tmp) = 1 Then
                    tmp = "0" & tmp
                End If
                Res += tmp
            Next

            Return Res
        End Function
    End Class
    Friend Class NetworkHelper
        ' Print debug informations?
        Public DebugOutput As Boolean = False

        Public Function SOAPAction(ByVal callClass As Object, ByVal action As String, ByVal urn As String, ByVal submitValues As Hashtable, ByVal returnValues As Hashtable)
            Dim InternalStringFunctions As New Internal.StringHelper
            Dim OutputHelper As New Internal.OutputHelper

            ' Define SOAP Request
            OutputHelper.DebugOutput("Define XML SOAP request")
            Dim sRequest As String = ""
            sRequest += "<?xml version=""1.0""?>" + vbCrLf
            sRequest += "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"" s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">" + vbCrLf
            sRequest += "<s:Body>" + vbCrLf
            sRequest += "<u:" + action + " xmlns:u=""" + urn + """>" + vbCrLf
            If submitValues.Count > 0 Then
                For Each submitItem As DictionaryEntry In submitValues
                    sRequest += "<" + callClass.prefixSOAP + submitItem.Key + ">"
                    sRequest += submitItem.Value.ToString
                    sRequest += "</" + callClass.prefixSOAP + submitItem.Key + ">"
                    sRequest += vbCrLf
                Next
            End If
            sRequest += "</u:" + action + ">" + vbCrLf
            sRequest += "</s:Body>" + vbCrLf
            sRequest += "</s:Envelope>" + vbCrLf
            OutputHelper.DebugOutput("XML SOAP output - " + sRequest)

            ' Define network credential for authentication
            Dim urlCredential As New System.Net.NetworkCredential(callClass.HTTPusername, callClass.HTTPpassword)

            ' Define URL for request
            Dim urlFull As String = "http://" + callClass.host + ":" + callClass.port + callClass.controlURL.ToString

            Try
                ' Define request
                Dim urlRequest As HttpWebRequest = System.Net.WebRequest.Create(urlFull)
                urlRequest.PreAuthenticate = True
                urlRequest.Credentials = urlCredential
                urlRequest.Method = "POST"
                urlRequest.Headers.Add("SOAPACTION", """" + urn + "#" + action + """")
                urlRequest.ContentType = "text/xml; charset=""utf-8"""
                urlRequest.UserAgent = "AVM UPnP/1.0 Client 1.0"
                urlRequest.ContentLength = Len(sRequest).ToString
                urlRequest.Accept = "text/xml"
                urlRequest.AllowWriteStreamBuffering = True

                Dim urlRequestStream As Stream = urlRequest.GetRequestStream
                Dim urlPostBytes As Byte() = Encoding.ASCII.GetBytes(sRequest)
                urlRequestStream.Write(urlPostBytes, 0, urlPostBytes.Length)
                urlRequestStream.Close()

                ' Response return
                Dim urlResponse As System.Net.WebResponse = urlRequest.GetResponse()

                ' Read thr stuff ...
                Dim urlReader As StreamReader = New StreamReader(urlResponse.GetResponseStream())

                ' Make a string ...
                Dim urlContent As String = urlReader.ReadToEnd()

                OutputHelper.DebugOutput("Return XML - " + urlContent)

                Return (urlContent)
            Catch e As Exception
                OutputHelper.DebugOutput("Exception (" + urlFull + ") - " + e.ToString())
                Return ("")
            End Try
            Return False
        End Function
        Public Function GetXMLKey(ByVal callClass As Object, ByVal XMLCode As String, ByVal urnSchema As String, ByVal command As String, ByVal Key As String, Optional ByVal errorCode As Boolean = False)

            ' If XML Code is empty, leave function ...
            If Len(XMLCode) = 0 Then
                Return Nothing
            End If

            ' Define new XmlDocument Object
            Dim xmlSource As New System.Xml.XmlDocument()

            ' Remove unwanted signs (NULL for example...)
            XMLCode = XMLCode.Substring(0, XMLCode.LastIndexOf("s:Envelope") + Len("s:Envelope") + 1)

            ' Load given xml code string as xml structure
            xmlSource.LoadXml(XMLCode)

            ' Define NamespaceManager for direct access of keys/values
            Dim nsmgr As XmlNamespaceManager = New XmlNamespaceManager(xmlSource.NameTable)

            ' Add given urn schema
            nsmgr.AddNamespace("u", urnSchema)

            ' Get given command and subkey
            Dim xn As XmlNodeList

            If errorCode = False Then
                xn = xmlSource.DocumentElement.SelectNodes("//u:" + command + "Response/" + callClass.urnReturnPrefix + Key, nsmgr)
            Else
                xn = xmlSource.DocumentElement.SelectNodes("//u:" + command + "/u:" + Key, nsmgr)
            End If

            ' Return result or nothing
            If xn.Count > 0 Then
                Return xn.Item(0).InnerXml.ToString
            Else
                Return Nothing
            End If
        End Function
        Public Function GetXMLContent(ByVal callClass As Object, ByVal command As String, ByVal urnSchema As String, ByVal submitTable As Hashtable, ByVal returnTable As Hashtable)

            Dim XMLCode As String = ""
            Dim XMLResult As String = ""
            Dim XMLErrorCode As String = ""
            Dim newReturnTable As New Hashtable

            If returnTable.Count > 0 Then
                XMLCode = Me.SOAPAction(callClass, command, urnSchema, submitTable, returnTable)
                XMLErrorCode = GetXMLKey(callClass, XMLCode, "urn:schemas-upnp-org:control-1-0", "UPnPError", "errorCode", True)
                returnTable.Add("errorCode", XMLErrorCode)
                For Each returnItem As DictionaryEntry In returnTable
                    XMLResult = GetXMLKey(callClass, XMLCode, urnSchema, command, returnItem.Key)
                    '-- MsgBox(command)
                    returnItem.Value = XMLResult
                    newReturnTable.Add(returnItem.Key, XMLResult)
                Next
                returnTable = newReturnTable
            Else
                XMLCode = Me.SOAPAction(callClass, command, urnSchema, submitTable, returnTable)
                XMLErrorCode = GetXMLKey(callClass, XMLCode, "urn:schemas-upnp-org:control-1-0", "UPnPError", "errorCode", True)
                returnTable.Add("errorCode", XMLErrorCode)
            End If
            Return returnTable
        End Function

    End Class
End Namespace
