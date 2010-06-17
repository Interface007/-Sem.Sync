Imports System.Xml
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Net.Sockets
Imports FritzBoxNET.Internal
Namespace Network
    Public Class Information

        ' The destination hostname (IP)
        Public host As String = ""

        ' The destination port
        Public port As String = "80"

        ' Username, is used for authentication (should not changed)
        Public HTTPusername As String = "fbox"

        ' Password, is used for authentication (THAT should be changed...)
        Public HTTPpassword As String = ""
        Public Function GetFirmwareVersion()
            ' Get whole configuration
            Dim FBConfigSettings As Hashtable = Me.GetConfigDefinition()

            ' Pick up needed part ...
            Dim FBConfigSetting As String = FBConfigSettings("AccessoryUrl") + "&"

            ' Extract the part, we need - Main Version
            Dim FBConfigPart1 As String = FBConfigSetting.Substring(FBConfigSetting.IndexOf("&version=") + 1)
            FBConfigPart1 = FBConfigPart1.Substring(Len("version") + 1, FBConfigPart1.IndexOf("&") - (Len("version") + 1))

            ' Extract the part, we need - Sub Version
            Dim FBConfigPart2 As String = FBConfigSetting.Substring(FBConfigSetting.IndexOf("&subversion=") + 1)

            If (FBConfigPart2.IndexOf("&") - (Len("subversion") + 1)) = 0 Then
                FBConfigPart2 = ""
            Else
                FBConfigPart2 = FBConfigPart2.Substring(Len("subversion") + 1, FBConfigPart2.IndexOf("&") - (Len("subversion") + 1))
            End If

            Dim FBFirmwareVersion As String = FBConfigPart1
            If Len(FBConfigPart2) > 0 Then
                FBFirmwareVersion += "-" + FBConfigPart2
            End If

            Return FBFirmwareVersion
        End Function
        Public Function GetBoxModel()
            ' Get whole configuration
            Dim FBConfigSettings As Hashtable = Me.GetConfigDefinition()

            ' Pick up needed part ...
            Dim FBProductName As String = FBConfigSettings("ProduktName")

            Return (FBProductName)
        End Function
        Public Function GetConfigDefinition()

            ' Define network credential for authentication
            Dim urlCredential As New System.Net.NetworkCredential(HTTPusername, HTTPpassword)

            ' Define request
            Dim urlRequest As System.Net.WebRequest = System.Net.WebRequest.Create("http://" + Me.host + ":" + Me.port + "/html/config.def")
            urlRequest.PreAuthenticate = True
            urlRequest.Credentials = urlCredential

            ' Response return
            Dim urlResponse As System.Net.WebResponse = urlRequest.GetResponse()

            ' Read thr stuff ...
            Dim urlReader As StreamReader = New StreamReader(urlResponse.GetResponseStream())

            ' Make a string ...
            Dim urlContent As String = urlReader.ReadToEnd()

            ' Define array which can be parsed
            Dim configRawArray = Split(urlContent, vbLf)

            ' Define configuration hash - contains configuration of Fritz!Box
            Dim configTable As New Hashtable
            Dim configKey As String = ""
            Dim configValue As String = ""

            For Each configRaw As String In configRawArray
                If Len(configRaw) > 0 Then
                    configKey = configRaw.Substring(configRaw.IndexOf(":") + 1, configRaw.IndexOf("'") - configRaw.IndexOf(":") - 2)
                    configValue = configRaw.Substring(configRaw.IndexOf("'") + 1, configRaw.LastIndexOf("'") - configRaw.IndexOf("'") - 1)
                    configTable.Add(configKey, configValue)
                End If
            Next

            Return (configTable)
        End Function
    End Class

End Namespace