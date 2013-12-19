Imports System.Xml
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Net.Sockets
Imports FritzBoxNET.Internal

Namespace UPnP
    Public Class FritzMini
        ' Where the service are controlled
        Public controlURL As String = "/upnp/control/fritzmini"

        ' Where events can be monitored
        Public eventURL As String = "/upnp/control/fritzmini"

        ' Where the scpd file can be found
        Public SCPDURL As String = "/mini-scpd.xml"

        ' The destination hostname (IP)
        Public host As String = ""

        ' The destination port
        Public port As String = "49000"

        ' Username, is used for authentication (should not changed)
        Public HTTPusername As String = "fbox"

        ' Password, is used for authentication (THAT should be changed...)
        Public HTTPpassword As String = ""

        ' Needed urn schema
        Public urnSchema As String = "urn:schemas-upnp-org:service:fritzmini:1"
        Public urnReturnPrefix As String = "New"

        ' Needed prefix for SOAP submit
        Public prefixSOAP As String = "New"

        ' Which minimum major version is required
        Const specVersionMajorMin = 1

        ' Which minimum minor version is required
        Const specVersionMinorMin = 0

        ' Which maximum major version is required
        Const specVersionMajorMax = 1

        ' Which maximum minor version is required
        Const specVersionMinorMax = 0

        ' We need some internal functions...
        Private InternalStringFunctions As New Internal.StringHelper
        Private InternalNetworkFunctions As New Internal.NetworkHelper
        Public Function GetVersion()
            Dim submitTable As New Hashtable

            ' Function returns: ServerVersion, ProtocolVersion
            Dim returnTable As New Hashtable
            returnTable.Add("DaemonVersion", "")
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "GetVersion", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function GetListInfo()
            Dim submitTable As New Hashtable

            ' Function returns: Number
            Dim returnTable As New Hashtable
            returnTable.Add("NumberUser", "")
            returnTable.Add("NumberService", "")
            returnTable.Add("NumberFritzMini", "")
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "GetListInfo", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function GetUserByIndex(ByVal Index As Integer)
            Dim submitTable As New Hashtable
            submitTable.Add("Index", Index)

            ' Function returns: Number
            Dim returnTable As New Hashtable
            returnTable.Add("UserID", "")
            returnTable.Add("UserFriendlyName", "")
            returnTable.Add("UserFritzMinis", "")
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "GetUserByIndex", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function GetUserByID(ByVal UserID As String)
            Dim submitTable As New Hashtable
            submitTable.Add("UserID", UserID)

            ' Function returns: Number
            Dim returnTable As New Hashtable
            returnTable.Add("UserFriendlyName", "")
            returnTable.Add("UserFritzMinis", "")
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "GetUserByID", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function AddUser(ByVal UserFriendlyName As String)
            Dim submitTable As New Hashtable
            submitTable.Add("UserFriendlyName", UserFriendlyName)

            ' Function returns: Number
            Dim returnTable As New Hashtable
            returnTable.Add("UserID", "")
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "AddUser", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function SetUserFriendlyName(ByVal UserID As Integer, ByVal UserFriendlyName As String)
            Dim submitTable As New Hashtable
            submitTable.Add("UserID", UserID)
            submitTable.Add("UserFriendlyName", UserFriendlyName)

            ' Function returns: Number
            Dim returnTable As New Hashtable
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "SetUserFriendlyName", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function DeleteUser(ByVal UserID As Integer)
            Dim submitTable As New Hashtable
            submitTable.Add("UserID", UserID)

            ' Function returns: Number
            Dim returnTable As New Hashtable
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "DeleteUser", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function GetFritzMiniByIndex(ByVal Index As Integer)
            Dim submitTable As New Hashtable
            submitTable.Add("Index", Index)

            ' Function returns: Number
            Dim returnTable As New Hashtable
            returnTable.Add("FritzMiniMAC", "")
            returnTable.Add("FritzMiniName", "")
            returnTable.Add("FritzMiniUserID", "")
            returnTable.Add("FritzMiniStatus", "")
            returnTable.Add("FritzMiniEngineState", "")
            returnTable.Add("FritzMiniVersionHW", "")
            returnTable.Add("FritzMiniVersionSW", "")
            returnTable.Add("FritzMiniVersionOEM", "")
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "GetFritzMiniByIndex", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function GetFritzMiniByMAC(ByVal FritzMiniMAC As String)
            Dim submitTable As New Hashtable
            submitTable.Add("FritzMiniMAC", FritzMiniMAC)

            ' Function returns: Number
            Dim returnTable As New Hashtable
            returnTable.Add("FritzMiniMAC", "")
            returnTable.Add("FritzMiniName", "")
            returnTable.Add("FritzMiniUserID", "")
            returnTable.Add("FritzMiniStatus", "")
            returnTable.Add("FritzMiniEngineState", "")
            returnTable.Add("FritzMiniVersionHW", "")
            returnTable.Add("FritzMiniVersionSW", "")
            returnTable.Add("FritzMiniVersionOEM", "")
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "GetFritzMiniByMAC", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function SetFritzMiniName(ByVal FritzMiniMAC As String, ByVal FritzMiniName As String)
            Dim submitTable As New Hashtable
            submitTable.Add("FritzMiniMAC", FritzMiniMAC)
            submitTable.Add("FritzMiniName", FritzMiniName)

            ' Function returns: Number
            Dim returnTable As New Hashtable
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "SetFritzMiniName", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function SetFritzMiniUser(ByVal FritzMiniMAC As String, ByVal FritzMiniUserID As Integer)
            Dim submitTable As New Hashtable
            submitTable.Add("FritzMiniMAC", FritzMiniMAC)
            submitTable.Add("FritzMiniUserID", FritzMiniUserID)

            ' Function returns: Number
            Dim returnTable As New Hashtable
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "SetFritzMiniUser", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function DeleteFritzMini(ByVal FritzMiniMAC As String)
            Dim submitTable As New Hashtable
            submitTable.Add("FritzMiniMAC", FritzMiniMAC)

            ' Function returns: Number
            Dim returnTable As New Hashtable
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "DeleteFritzMini", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function GetServiceByIndex(ByVal Index As Integer)
            Dim submitTable As New Hashtable
            submitTable.Add("Index", Index)

            ' Function returns: Number
            Dim returnTable As New Hashtable
            returnTable.Add("ServiceName", "")
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "GetServiceByIndex", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function GetServiceData(ByVal UserID As Integer, ByVal ServiceName As String, ByVal ServiceCommand As String, ByVal ServiceParameter As String)
            Dim submitTable As New Hashtable
            submitTable.Add("UserID", UserID)
            submitTable.Add("ServiceName", ServiceName)
            submitTable.Add("ServiceCommand", ServiceCommand)
            submitTable.Add("ServiceParameter", ServiceParameter)

            ' Function returns: Number
            Dim returnTable As New Hashtable
            returnTable.Add("ServiceData", "")
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "GetServiceData", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function SetServiceData(ByVal UserID As Integer, ByVal ServiceName As String, ByVal ServiceCommand As String, ByVal ServiceParameter As String, ByVal ServiceData As String)
            Dim submitTable As New Hashtable
            submitTable.Add("UserID", UserID)
            submitTable.Add("ServiceName", ServiceName)
            submitTable.Add("ServiceCommand", ServiceCommand)
            submitTable.Add("ServiceParameter", ServiceParameter)
            submitTable.Add("ServiceData", ServiceParameter)

            ' Function returns: Number
            Dim returnTable As New Hashtable
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "SetServiceData", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
    End Class
End Namespace
Namespace Network
    Public Class FritzMini

        ' Username, is used for authentication (should not changed)
        Public HTTPusername As String = "fbox"

        ' Password, is used for authentication (THAT should be changed...)
        Public HTTPpassword As String = ""
        Public Function GetServiceURL(ByVal URL As String)
            ' Define network credential for authentication
            Dim urlCredential As New System.Net.NetworkCredential(HTTPusername, HTTPpassword)

            ' Define request
            Dim urlRequest As System.Net.WebRequest = System.Net.WebRequest.Create(URL)
            urlRequest.PreAuthenticate = True
            urlRequest.Credentials = urlCredential

            ' Response return
            Dim urlResponse As System.Net.WebResponse = urlRequest.GetResponse()

            ' Read thr stuff ...
            Dim urlReader As StreamReader = New StreamReader(urlResponse.GetResponseStream())

            ' Make a string ...
            Dim urlContent As String = urlReader.ReadToEnd()

            ' Return XML stuff
            Return urlContent
        End Function

    End Class
End Namespace