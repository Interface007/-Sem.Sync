Imports System.Xml
Imports System.Text
Imports System.Net.Sockets
Imports FritzBoxNET.Internal

Namespace UPnP
    Public Class Phonebook
        ' Where the service are controlled
        Public controlURL As String = "/upnp/control/phonebook"

        ' Where events can be monitored
        Public eventURL As String = "/upnp/control/phonebook"

        ' Where the scpd file can be found
        Public SCPDURL As String = "/phonebook-scpd.xml"

        ' The destination hostname (IP)
        Public host As String = ""

        ' The destination port
        Public port As String = "49000"

        ' Username, is used for authentication (should not changed)
        Public HTTPusername As String = "fbox"

        ' Password, is used for authentication (THAT should be changed...)
        Public HTTPpassword As String = ""

        ' Needed urn schema
        Public urnSchema As String = "urn:schemas-any-com:service:phonebook:1"
        Public urnReturnPrefix As String = ""

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
        Public Function OpenPort()
            Dim submitTable As New Hashtable

            ' Function returns: Port
            Dim returnTable As New Hashtable
            returnTable.Add("Port", "")
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "OpenPort", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
    End Class
End Namespace
Namespace Network
    Public Class Phonebook

        ' The destination hostname (IP)
        Public host As String = ""

        ' The destination port
        Public port As String = "0"

        ' For internal use
        Private connectionEstablished As Boolean = False
        Private clientSocketTCP As New System.Net.Sockets.TcpClient
        Private Function ConnectNetwork()
            ' Establish connection to given host and port
            clientSocketTCP.Connect(host, port)

            Dim remoteStream As NetworkStream = clientSocketTCP.GetStream()

            If remoteStream.CanWrite And remoteStream.CanRead Then
                connectionEstablished = True
                Return True
            Else
                MsgBox("Fehler!")
                Return False
            End If

        End Function
        Public Function GetEntryCount(Optional ByVal Phonebook As Integer = 0)
            ' If initial call - establish connection
            If connectionEstablished = False Then Me.ConnectNetwork()

            ' Define network remote stream
            Dim remoteStream As NetworkStream = clientSocketTCP.GetStream()

            ' Define byte command for getting entry count
            Dim phonebookIndex As String = "&H" & Phonebook.ToString("x").ToUpper
            'Dim entryCountRequest As Byte() = {&H6, &H0, &H0, &H0, &H0, &H0}
            Dim entryCountRequest As Byte() = {&H6, &H0, &H0, &H0, phonebookIndex, &H0}

            ' Define network response byte return
            Dim responseBytesRead(clientSocketTCP.ReceiveBufferSize) As Byte

            'ExceptionHandler.WriteContextEntry("fritz.net send tcp stream - get entry count", entryCountRequest)

            ' Write command and flush it
            remoteStream.Write(entryCountRequest, 0, entryCountRequest.Length)
            remoteStream.Flush()

            ' Read response
            remoteStream.Read(responseBytesRead, 0, CInt(clientSocketTCP.ReceiveBufferSize))
            'ExceptionHandler.WriteContextEntry("fritz.net receive tcp stream - get entry count", responseBytesRead)

            Dim responseString As String = Encoding.ASCII.GetString(responseBytesRead)

            Dim entryCount As Integer = CInt("&H" & Asc(responseString.Substring(2, 1)).ToString("x").ToUpper)
            Return entryCount
        End Function
        Public Function GetEntry(ByVal EntryNumber As Integer, Optional ByVal Phonebook As Integer = 0)
            ' If initial call - establish connection
            If connectionEstablished = False Then Me.ConnectNetwork()

            ' Define network remote stream
            Dim remoteStream As NetworkStream = clientSocketTCP.GetStream()

            ' Get total entry count
            Dim entryCount As Integer
            If Phonebook Then
                entryCount = Me.GetEntryCount(Phonebook)
            Else
                entryCount = Me.GetEntryCount()
            End If

            ' If entry index is out of range, leave function
            If entryCount - 1 < EntryNumber Then
                Return False
            End If

            ' Define byte command for getting entry count
            Dim entryHexIndex As String = "&H" & EntryNumber.ToString("x").ToUpper
            Dim phonebookIndex As String = "&H" & Phonebook.ToString("x").ToUpper
            Dim entryRequest As Byte() = {&H8, &H0, &H1, &H0, phonebookIndex, &H0, entryHexIndex, &H0}
            'Dim entryRequest As Byte() = {&H8, &H0, &H1, &H0, &H0, &H0, entryHexIndex, &H0}

            ' Define network response byte return
            Dim responseBytesRead(clientSocketTCP.ReceiveBufferSize) As Byte

            ' Write command and flush it
            'ExceptionHandler.WriteContextEntry("fritz.net send tcp stream - get entry", entryRequest)
            remoteStream.Write(entryRequest, 0, entryRequest.Length)
            remoteStream.Flush()

            ' Read response - first response is the record length
            Dim bytesReceivedLength As Int32 = remoteStream.Read(responseBytesRead, 0, CInt(clientSocketTCP.ReceiveBufferSize))
            'ExceptionHandler.WriteContextEntry("fritz.net receive tcp stream - get entry", responseBytesRead)

            Dim responseStringLength As String = Encoding.ASCII.GetString(responseBytesRead)

            ' Read response - the data
            remoteStream.Read(responseBytesRead, 0, CInt(clientSocketTCP.ReceiveBufferSize))
            Dim responseStringXML As String = Encoding.Default.GetString(responseBytesRead)

            ' Define XML Document object
            Dim returnXML As New XmlDocument

            ' Parse XML data to XML object
            returnXML.LoadXml(responseStringXML)

            ' Set up namespace manager for XPath  
            Dim ns As XmlNamespaceManager = New XmlNamespaceManager(returnXML.NameTable)
            ns.AddNamespace(String.Empty, "")

            ' Get forecast with XPath  
            Dim nodes As XmlNodeList = returnXML.SelectNodes("/", ns)

            Return nodes
        End Function
    End Class
End Namespace