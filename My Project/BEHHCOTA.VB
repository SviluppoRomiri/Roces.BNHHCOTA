Imports System.Data
Imports NTSInformatica.CLN__STD


Public Class CLEHHCOTA
    Inherits CLE__BASE

    Public oCldCOTA As CLDHHCOTA  'oggetto DAL

    Private Moduli_P As Integer = CLN__STD.bsModAll
    Private ModuliExt_P As Integer = CLN__STD.bsModExtAll
    Private ModuliSup_P As Integer = 0
    Private ModuliSupExt_P As Integer = 0
    Private ModuliPtn_P As Integer = 0
    Private ModuliPtnExt_P As Integer = 0

    Public ReadOnly Property Moduli() As Integer
        Get
            Return Moduli_P
        End Get
    End Property
    Public ReadOnly Property ModuliExt() As Integer
        Get
            Return ModuliExt_P
        End Get
    End Property
    Public ReadOnly Property ModuliSup() As Integer
        Get
            Return ModuliSup_P
        End Get
    End Property
    Public ReadOnly Property ModuliSupExt() As Integer
        Get
            Return ModuliSupExt_P
        End Get
    End Property
    Public ReadOnly Property ModuliPtn() As Integer
        Get
            Return ModuliPtn_P
        End Get
    End Property
    Public ReadOnly Property ModuliPtnExt() As Integer
        Get
            Return ModuliPtnExt_P
        End Get
    End Property

    Public Overrides Function Init(ByRef App As CLE__APP,
                              ByRef oScriptEngine As INT__SCRIPT, ByRef oCleLbmenu As Object, ByVal strTabella As String,
                              ByVal bFiller1 As Boolean, ByVal strFiller1 As String,
                              ByVal strFiller2 As String) As Boolean
        If MyBase.strNomeDal = "BD__BASE" Then MyBase.strNomeDal = "BDHHCOTA"
        MyBase.Init(App, oScriptEngine, oCleLbmenu, strTabella, False, "", "")
        oCldCOTA = CType(MyBase.ocldBase, CLDHHCOTA)
        oCldCOTA.Init(oApp)

        Return True
    End Function

    Public Overrides Function Apri(ByVal strDitta As String, ByRef ds As System.Data.DataSet) As Boolean
        Dim dReturn As Boolean = False
        Try
            '--------------------------------------
            'Tengo un puntatore al datatable che verrà usato nell'UI, quindi chiedo i dati al DAL
            strDittaCorrente = strDitta
            dReturn = oCldCOTA.GetBNHHCOTA(strDittaCorrente, ds)
            If dReturn = False Then Return False

            '--------------------------------------------------------------
            'Imposto i valori di default della tabella: con la riga che segue prendo prima i valori dal database
            oCldCOTA.SetTableDefaultValueFromDB(strNomeTabella, ds)

            SetDefaultValue(ds)

            dsShared = ds

            '--------------------------------------
            'Creo gli eventi per la gestione del Datatable dentro l'Entity
            AddHandler dsShared.Tables(strNomeTabella).ColumnChanging, AddressOf BeforeColUpdate
            AddHandler dsShared.Tables(strNomeTabella).ColumnChanged, AddressOf AfterColUpdate

            bHasChanges = False

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------

            CLN__STD.GestErr(ex, Me, "")

            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function ApriZoom(ByVal strDitta As String, ByRef ds As System.Data.DataSet) As Boolean
        Dim dReturn As Boolean = False

        Try

            Return oCldCOTA.GetBNHHCOTAZoom(strDittaCorrente, ds)

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Function

    Public Overrides Sub SetDefaultValue(ByRef ds As DataSet)
        Try
            Dim ultimogg As Date = New Date(2099, 12, 31)

            'Imposto i valori di default diversi da quelli impostati nel database
            ds.Tables(strNomeTabella).Columns("codditt").DefaultValue = strDittaCorrente

        Catch ex As Exception
            '--------------------------------------------------------------

            CLN__STD.GestErr(ex, Me, "")

            '--------------------------------------------------------------
        End Try
    End Sub

    Public Overrides Sub BeforeColUpdate(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
        Dim strErr As String = ""
        Try
            'memorizzo il valore corrente di cella per testarlo nella AfterColUpdate
            'solo se il dato è uguale a quello precedentemente contenuto nella cella
            If ValoriUguali(e.ProposedValue.ToString, e.Row(e.Column.ColumnName).ToString) Then
                strPrevCelValue += e.Column.ColumnName.ToUpper + ";"
                Return
            End If
            '-------------------------------------------------------------
            'controllo che in una cella short non venga inserito un numero troppo grande
            If Not CheckCellaShort(e, strErr) Then Throw New NTSException(strErr)
            '-------------------------------------------------------------
            'cerco e, se la trovo, eseguo la funzione specifica per la colonna modificata
            Dim strFunction As String = "BeforeColUpdate_" & e.Column.ColumnName.ToLower
            Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
            If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

        Catch ex As Exception
            '--------------------------------------------------------------

            CLN__STD.GestErr(ex, Me, "")

            '--------------------------------------------------------------
        End Try
    End Sub

    'Public Overridable Sub BeforeColUpdate_cn_forn(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    '    Dim strCodForn As String = NTSCStr(e.ProposedValue)
    '    Dim dttRecord As New DataTable
    '    Dim strDescForn As String = ""

    '    Try
    '        If Not oCldCOTA.ValCodiceDb(strCodForn, strDittaCorrente, "artico", "N", strDescForn, dttRecord) Then
    '            ThrowRemoteEvent(New NTSEventArgs("", "Codice articolo non esistente, verrà ripristinato quello precedente"))
    '            e.ProposedValue = e.Row(e.Column.ColumnName)
    '            Return
    '        End If


    '        If dttRecord IsNot Nothing Then
    '            If dttRecord.Rows.Count = 1 Then
    '                If NTSCStr(dttRecord.Rows(0)!an_descr1) IsNot String.Empty Then
    '                    strDescForn = NTSCStr(dttRecord.Rows(0)!an_descr1)
    '                    e.Row!xx_desforn = strDescForn
    '                End If
    '            End If
    '        End If

    '        Return

    '    Catch ex As Exception
    '        CLN__STD.GestErr(ex, Me, "")
    '    End Try

    'End Sub

    Public Overrides Sub AfterColUpdate(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
        Try
            'non valido la colonna se il dato non è cambiato
            If strPrevCelValue.IndexOf(e.Column.ColumnName.ToUpper + ";") > -1 Then
                strPrevCelValue = strPrevCelValue.Remove(strPrevCelValue.IndexOf(e.Column.ColumnName.ToUpper + ";"), e.Column.ColumnName.ToUpper.Length + 1)
                Return
            End If

            bHasChanges = True

            'comunico che una cella è cambiata, per fare in modo che se il dato è contenuto in una griglia 
            'vengano fatte le routine di validazione del caso
            ThrowRemoteEvent(New NTSEventArgs("GRIAGG", e.Column.Table.TableName & "§" & e.Column.ColumnName))

            e.Row.EndEdit()
            e.Row.EndEdit()

            '-------------------------------------------------------------
            'cerco e, se la trovo, eseguo la funzione specifica per la colonna modificata
            Dim strFunction As String = "AfterColUpdate_" & e.Column.ColumnName.ToLower
            Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
            If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})
        Catch ex As Exception
            '--------------------------------------------------------------

            CLN__STD.GestErr(ex, Me, "")

            '--------------------------------------------------------------
        End Try
    End Sub


    Public Overrides Function TestPreSalva() As Boolean
        'Eseguo i controlli necessari prima di fare il salvataggio della riga
        Dim dtrTmp As DataRow() = dsShared.Tables(strNomeTabella).Select(Nothing, Nothing, DataViewRowState.Added Or DataViewRowState.ModifiedCurrent)
        Dim i As Integer = 0

        Try

            For i = 0 To dtrTmp.Length - 1
                If NTSCStr(dtrTmp(i)!ct_tiponum).Trim = "0" Then
                    ThrowRemoteEvent(New NTSEventArgs("", "Numero obbligatorio, 0 non valido."))
                    Return False
                End If

                If NTSCStr(dtrTmp(i)!ct_eu).Trim = String.Empty Then
                    ThrowRemoteEvent(New NTSEventArgs("", "Taglia obbligatoria."))
                    Return False
                End If

                If NTSCStr(dtrTmp(i)!ct_uk).Trim = String.Empty Then
                    ThrowRemoteEvent(New NTSEventArgs("", "Taglia obbligatoria."))
                    Return False
                End If

                If NTSCStr(dtrTmp(i)!ct_usman).Trim = String.Empty Then
                    ThrowRemoteEvent(New NTSEventArgs("", "Taglia obbligatoria."))
                    Return False
                End If

                If NTSCStr(dtrTmp(i)!ct_uswoman).Trim = String.Empty Then
                    ThrowRemoteEvent(New NTSEventArgs("", "Taglia obbligatoria."))
                    Return False
                End If

                'If NTSCInt(dtrTmp(i)!cn_forn) = 0 Then
                '    ThrowRemoteEvent(New NTSEventArgs("", "Codice cliente obbligatorio."))
                '    Return False
                'End If
            Next


            Return True

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Function

    Public Overrides Function Salva(ByVal bDelete As Boolean) As Boolean

        Dim bResult As Boolean = False
        Try
            '----------------------------------------
            'controlli pre-salvataggio (solo se non è una delete)
            If Not bDelete Then
                If Not TestPreSalva() Then Return False
            End If

            '----------------------------------------
            'chiamo il dal per salvare
            bResult = oCldCOTA.ScriviTabellaSemplice(strDittaCorrente, strNomeTabella, dsShared.Tables(strNomeTabella), "", "", "")

            If bResult Then
                bHasChanges = False
            End If

            Return bResult

        Catch ex As Exception
            '--------------------------------------------------------------

            CLN__STD.GestErr(ex, Me, "")

            '--------------------------------------------------------------
        End Try
    End Function



End Class
