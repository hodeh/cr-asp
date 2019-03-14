Imports System.IO

Public Class ViewCrystalReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oReportMain As CrystalDecisions.CrystalReports.Engine.ReportDocument

        oReportMain = New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Try
            ControlLoad(oReportMain)


        Catch ex As Exception
            Dim str As String = ex.ToString()
            str = str
        Finally
            ControlUnload(oReportMain)
        End Try
    End Sub
    Private Sub ControlLoad(ByRef oReport As CrystalDecisions.CrystalReports.Engine.ReportDocument)
        'Put user code to initialize the page here
        Response.Clear()
        Response.Buffer = True

        Dim newReportPath As String = Server.MapPath("./CrystalPOC.rpt")
        oReport.Load(newReportPath)
        'Dim myOraConnectionInfo As New OraConnectionInfo(Global.DED.EIT.SystemFramework.Common.GetAppConfigStringDirect("Application.ConnectionString"))
        'oReport.SetDatabaseLogon(
        '      myOraConnectionInfo.OraUserName,
        '      myOraConnectionInfo.OraUserPwd,
        '      myOraConnectionInfo.OraServer, "", True)

        'Dim subRptCount As Integer = oReport.Subreports.Count
        'For i As Integer = 0 To subRptCount - 1
        '    oReport.Subreports.Item(i).SetDatabaseLogon(
        '          myOraConnectionInfo.OraUserName,
        '          myOraConnectionInfo.OraUserPwd,
        '          myOraConnectionInfo.OraServer, "", True)
        'Next


        'paramField.ParameterFieldName = "PEFORM_NUMBER"
        'discreteVal.Value = PermitEFormNumber
        'paramField.CurrentValues.Add(discreteVal)
        'paramFields.Add(paramField)

        'For Each pf As CrystalDecisions.Shared.ParameterField In paramFields
        '    oReport.SetParameterValue(pf.ParameterFieldName, pf.CurrentValues)
        'Next


        'Create byte array

        Dim MyExportOptions As New CrystalDecisions.Shared.ExportOptions
        MyExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        Dim MyExportRequestContext As New CrystalDecisions.Shared.ExportRequestContext
        MyExportRequestContext.ExportInfo = MyExportOptions

        Using MyStream As Stream = oReport.FormatEngine.ExportToStream(MyExportRequestContext)
            Dim MyBuffer(CInt(MyStream.Length)) As Byte
            MyStream.Read(MyBuffer, 0, CType(MyStream.Length, Integer))

            Response.Clear()
            Response.ClearHeaders()
            Response.ClearContent()
            Response.ContentType = "application/pdf"
            Response.AddHeader("Content-Length", CInt(MyStream.Length).ToString()) 'add the header that specifies the file size, so that the browser can show the download progress
            If System.Web.HttpContext.Current.Response.IsClientConnected Then
                System.Web.HttpContext.Current.Response.Flush()
                System.Web.HttpContext.Current.Response.BinaryWrite(MyBuffer)
            End If


        End Using



    End Sub

    Private Sub ControlUnload(ByRef oReport As CrystalDecisions.CrystalReports.Engine.ReportDocument)
        If Not oReport Is Nothing Then
            oReport.Close()
            oReport.Dispose()
            oReport = Nothing
        End If
        GC.Collect()
    End Sub

    Private Sub ViewCrystalReport_Init(sender As Object, e As EventArgs) Handles Me.Init
        Response.Buffer = True
        Response.Clear()
        Response.ClearHeaders()
        Response.ClearContent()
    End Sub
End Class