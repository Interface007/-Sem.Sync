Imports FritzBoxNET.Internal

Namespace UPnP
    Public Class AURA
        ' Where the service are controlled
        Public controlURL As String = "/upnp/control/aura"

        ' Where events can be monitored
        Public eventURL As String = "/upnp/control/aura"

        ' Where the scpd file can be found
        Public SCPDURL As String = "/aura-scpd.xml"

        ' The destination hostname (IP)
        Public host As String = ""

        ' The destination port
        Public port As String = "49000"

        ' Username, is used for authentication (should not changed)
        Public HTTPusername As String = "fbox"

        ' Password, is used for authentication (THAT should be changed...)
        Public HTTPpassword As String = ""

        ' Needed urn schema
        Public urnSchema As String = "urn:schemas-any-com:service:aura:1"
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
            ' +++ Return the used server and protocol version +++

            Dim submitTable As New Hashtable

            ' Function returns: ServerVersion, ProtocolVersion
            Dim returnTable As New Hashtable
            returnTable.Add("ServerVersion", "")
            returnTable.Add("ProtocolVersion", "")
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "GetVersion", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function GetListInfo()
            ' +++ Returns the number of connected devices +++

            Dim submitTable As New Hashtable

            ' Function returns: Number
            Dim returnTable As New Hashtable
            returnTable.Add("Number", "")
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "GetListInfo", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function GetDeviceByIndex(ByVal Index As Integer)
            ' +++ Get device information by index number +++

            Dim submitTable As New Hashtable
            submitTable.Add("Index", Index)

            ' Function returns: DeviceHandle, Name, HardwareId, SerialNumber, TopologyId, Class, Manufacturer, Status, ClientIP
            Dim returnTable As New Hashtable
            returnTable.Add("DeviceHandle", "")
            returnTable.Add("Name", "")
            returnTable.Add("HardwareId", "")
            returnTable.Add("SerialNumber", "")
            returnTable.Add("TopologyId", "")
            returnTable.Add("Class", "")
            returnTable.Add("Manufacturer", "")
            returnTable.Add("Status", "")
            returnTable.Add("ClientIP", "")
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "GetDeviceByIndex", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function GetDeviceByHandle(ByVal DeviceHandle As Integer)
            ' +++ Get device information by handle number +++

            Dim submitTable As New Hashtable
            submitTable.Add("DeviceHandle", DeviceHandle)

            ' Function returns: Name, HardwareId, SerialNumber, TopologyId, Class, Manufacturer, Status, ClientIP
            Dim returnTable As New Hashtable
            returnTable.Add("Name", "")
            returnTable.Add("HardwareId", "")
            returnTable.Add("SerialNumber", "")
            returnTable.Add("TopologyId", "")
            returnTable.Add("Class", "")
            returnTable.Add("Manufacturer", "")
            returnTable.Add("Status", "")
            returnTable.Add("ClientIP", "")
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "GetDeviceByHandle", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function ConnectDevice(ByVal DeviceHandle As Integer)
            ' +++ Connect device +++

            Dim submitTable As New Hashtable
            submitTable.Add("DeviceHandle", DeviceHandle)

            ' Function returns: -
            Dim returnTable As New Hashtable
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "ConnectDevice", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function DisconnectDevice(ByVal DeviceHandle As Integer)
            ' +++ Disconnect device +++

            Dim submitTable As New Hashtable
            submitTable.Add("DeviceHandle", DeviceHandle)

            ' Function returns: -
            Dim returnTable As New Hashtable
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "DisconnectDevice", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
    End Class
End Namespace
