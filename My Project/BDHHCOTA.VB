Imports System.Data
Imports NTSInformatica.CLN__STD
Imports System.Text


Public Class CLDHHCOTA
    Inherits CLD__BASE

    Public Overridable Function GetBNHHCOTA(ByVal strDitta As String, ByRef dsOut As DataSet) As Boolean

        Dim sSql As New StringBuilder

        Try
            sSql.AppendLine("SELECT * ")
            sSql.AppendLine("FROM HHCORRTAGLIE")

            dsOut = OpenRecordset(sSql.ToString(), CLE__APP.DBTIPO.DBAZI, "HHCORRTAGLIE") 'nometabella

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            'Non eseguo la gestione errori standard ma giro l'errore 
            'direttamente al componente entity che mi ha chiamato
            CLN__STD.GestErr(ex, Me, "")
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetBNHHCOTAZoom(ByVal strDitta As String, ByRef dsOut As DataSet) As Boolean
        Dim sSql As New StringBuilder
        Try
            sSql.AppendLine("SELECT ct_tiponum, ct_eu, ct_uk, ct_usman, ct_uswoman ")
            sSql.AppendLine("FROM HHCORRTAGLIE ")



            dsOut = OpenRecordset(sSql.ToString(), CLE__APP.DBTIPO.DBAZI, "HHCORRTAGLIE") 'nometabella

            Return True

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try

    End Function
End Class
