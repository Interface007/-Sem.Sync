Imports System.Xml
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Net.Sockets
Imports FritzBoxNET.Internal

Namespace UPnP
    Public Class Foncontrol
        ' Where the service are controlled
        Public controlURL As String = "/upnp/control/foncontrol"

        ' Where events can be monitored
        Public eventURL As String = "/upnp/control/foncontrol"

        ' Where the scpd file can be found
        Public SCPDURL As String = "/foncontrol-scpd.xml"

        ' The destination hostname (IP)
        Public host As String = ""

        ' The destination port
        Public port As String = "49000"

        ' Username, is used for authentication (should not changed)
        Public HTTPusername As String = "fbox"

        ' Password, is used for authentication (THAT should be changed...)
        Public HTTPpassword As String = ""

        ' Needed urn schema
        Public urnSchema As String = "urn:schemas-any-com:service:foncontrol:1"
        Public urnReturnPrefix As String = ""

        ' Needed prefix for SOAP submit
        Public prefixSOAP As String = ""

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
        Public Function GetUserList()
            ' +++ Get list of available "users" +++

            Dim submitTable As New Hashtable

            ' Function returns: ServerVersion, ProtocolVersion
            Dim returnTable As New Hashtable
            returnTable.Add("UserList", "")
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "GetUserList", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
        Public Function GetCallLists(ByVal User As Integer)
            ' +++ Get call list for user +++

            Dim submitTable As New Hashtable
            submitTable.Add("User", User)

            ' Function returns: ServerVersion, ProtocolVersion
            Dim returnTable As New Hashtable
            returnTable.Add("CallListsURL", "")
            returnTable = InternalNetworkFunctions.GetXMLContent(Me, "GetCallLists", Me.urnSchema, submitTable, returnTable)

            Return returnTable
        End Function
    End Class
End Namespace
Namespace Network
    Public Class Foncontrol

        ' Username, is used for authentication (should not changed)
        Public HTTPusername As String = "fbox"

        ' Password, is used for authentication (THAT should be changed...)
        Public HTTPpassword As String = ""
        Public Function GetCallList(ByVal URL As String)
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

            ' Define XML Document object for parsing result of GetUserList
            Dim returnXML As New XmlDocument

            ' Build valid XML and parse XML data to XML object 
            returnXML.LoadXml(urlContent)

            ' Set up namespace manager for XPath  
            Dim ns As XmlNamespaceManager = New XmlNamespaceManager(returnXML.NameTable)
            ns.AddNamespace(String.Empty, "")

            ' Get forecast with XPath  
            Dim nodes As XmlNodeList = returnXML.SelectNodes("/call_lists", ns)

            ' Return XML stuff
            Return nodes
        End Function
    End Class
End Namespace
