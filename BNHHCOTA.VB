Imports System.Data
Imports NTSInformatica.CLN__STD

Public Class FRMHHCOTA
    Inherits FRM__CHIL
#Region "Property"
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
#End Region

#Region "Variabili"
    Public strNomeTabella As String = "HHCORRTAGLIE"

    Public oCleCOTA As CLEHHCOTA

    Public dsCOTA As DataSet
    Public dcCOTA As BindingSource = New BindingSource()

    Public oCallParams As CLE__CLDP

    Private components As System.ComponentModel.IContainer
    Public WithEvents NtsBarManager1 As NTSInformatica.NTSBarManager
    Public WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Public WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Public WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Public WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Public WithEvents tlbMain As NTSInformatica.NTSBar
    Public WithEvents tlbNuovo As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbCancella As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbRipristina As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbZoom As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbStrumenti As NTSInformatica.NTSBarSubItem
    Public WithEvents tlbImpostaStampante As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbGuida As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbEsci As NTSInformatica.NTSBarButtonItem
    Public WithEvents pnCOTA As NTSInformatica.NTSPanel
    Public WithEvents grHhac As NTSInformatica.NTSGrid
    Public WithEvents cn_dtini As NTSGridColumn
    Public WithEvents ct_tiponum As NTSGridColumn
    Public WithEvents ct_eu As NTSGridColumn
    Friend WithEvents ct_uk As NTSGridColumn
    Friend WithEvents ct_usman As NTSGridColumn
    Friend WithEvents ct_uswoman As NTSGridColumn
    'Public WithEvents cn_jmp As NTSGridColumn
    'Friend WithEvents cn_forn As NTSGridColumn
    'Friend WithEvents xx_desforn As NTSGridColumn
    Public WithEvents grvHhac As NTSInformatica.NTSGridView

#End Region

    Public Overloads Function Init(ByRef Menu As CLE__MENU, ByRef Param As CLE__CLDP, Optional ByVal Ditta As String = "", Optional ByRef SharedControls As CLE__EVNT = Nothing) As Boolean
        oMenu = Menu
        oApp = oMenu.App
        oCallParams = Param

        If Ditta <> "" Then
            DittaCorrente = Ditta
        Else
            DittaCorrente = oApp.Ditta
        End If

        InitializeComponent()
        Me.MinimumSize = Me.Size

        '------------------------------------------------
        'Creo e attivo l'entity, in più inizializzo la funzione che dovrà rilevare gli eventi dall'ENTITY stesso
        Dim strErr As String = ""
        Dim objTmp As Object = Nothing
        If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BNHHCOTA", "BEHHCOTA", objTmp, strErr, False, "", "") = False Then  ''MUST DA CAMBIARE
            oApp.MsgBoxErr(oApp.Tr(Me, 128182574024687500, "ERRORE in fase di creazione Entity:" & vbCrLf & "|" & strErr & "|"))
            Return False
        End If
        oCleCOTA = CType(objTmp, CLEHHCOTA)
        '------------------------------------------------

        AddHandler oCleCOTA.RemoteEvent, AddressOf GestisciEventiEntity
        If oCleCOTA.Init(oApp, NTSScript, oMenu.oCleComm, strNomeTabella, False, "", "") = False Then Return False

        Return True
    End Function

    Public Overridable Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim GridLevelNode1 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode()
        Me.NtsBarManager1 = New NTSInformatica.NTSBarManager()
        Me.tlbMain = New NTSInformatica.NTSBar()
        Me.tlbNuovo = New NTSInformatica.NTSBarButtonItem()
        Me.tlbRipristina = New NTSInformatica.NTSBarButtonItem()
        Me.tlbCancella = New NTSInformatica.NTSBarButtonItem()
        Me.tlbZoom = New NTSInformatica.NTSBarButtonItem()
        Me.tlbStrumenti = New NTSInformatica.NTSBarSubItem()
        Me.tlbImpostaStampante = New NTSInformatica.NTSBarButtonItem()
        Me.tlbGuida = New NTSInformatica.NTSBarButtonItem()
        Me.tlbEsci = New NTSInformatica.NTSBarButtonItem()
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.pnCOTA = New NTSInformatica.NTSPanel()
        Me.grHhac = New NTSInformatica.NTSGrid()
        Me.grvHhac = New NTSInformatica.NTSGridView()
        Me.ct_tiponum = New NTSInformatica.NTSGridColumn()
        Me.ct_eu = New NTSInformatica.NTSGridColumn()
        Me.ct_uk = New NTSInformatica.NTSGridColumn()
        Me.ct_usman = New NTSInformatica.NTSGridColumn()
        Me.ct_uswoman = New NTSInformatica.NTSGridColumn()
        Me.cn_dtini = New NTSInformatica.NTSGridColumn()
        CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pnCOTA, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnCOTA.SuspendLayout()
        CType(Me.grHhac, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grvHhac, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NtsBarManager1
        '
        Me.NtsBarManager1.AllowCustomization = False
        Me.NtsBarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.tlbMain})
        Me.NtsBarManager1.DockControls.Add(Me.barDockControlTop)
        Me.NtsBarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.NtsBarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.NtsBarManager1.DockControls.Add(Me.barDockControlRight)
        Me.NtsBarManager1.Form = Me
        Me.NtsBarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.tlbNuovo, Me.tlbCancella, Me.tlbRipristina, Me.tlbZoom, Me.tlbStrumenti, Me.tlbImpostaStampante, Me.tlbGuida, Me.tlbEsci})
        Me.NtsBarManager1.MaxItemId = 16
        '
        'tlbMain
        '
        Me.tlbMain.BarName = "tlbMain"
        Me.tlbMain.DockCol = 0
        Me.tlbMain.DockRow = 0
        Me.tlbMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.tlbMain.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbNuovo), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbRipristina), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbCancella), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbZoom), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbStrumenti, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbGuida, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbEsci)})
        Me.tlbMain.OptionsBar.AllowQuickCustomization = False
        Me.tlbMain.OptionsBar.DisableClose = True
        Me.tlbMain.OptionsBar.DrawDragBorder = False
        Me.tlbMain.OptionsBar.UseWholeRow = True
        Me.tlbMain.Text = "tlbMain"
        '
        'tlbNuovo
        '
        Me.tlbNuovo.Caption = "Nuovo"
        Me.tlbNuovo.Id = 0
        Me.tlbNuovo.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2)
        Me.tlbNuovo.Name = "tlbNuovo"
        Me.tlbNuovo.Visible = True
        '
        'tlbRipristina
        '
        Me.tlbRipristina.Caption = "Ripristina"
        Me.tlbRipristina.Id = 3
        Me.tlbRipristina.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F8)
        Me.tlbRipristina.Name = "tlbRipristina"
        Me.tlbRipristina.Visible = True
        '
        'tlbCancella
        '
        Me.tlbCancella.Caption = "Cancella"
        Me.tlbCancella.Id = 2
        Me.tlbCancella.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F4)
        Me.tlbCancella.Name = "tlbCancella"
        Me.tlbCancella.Visible = True
        '
        'tlbZoom
        '
        Me.tlbZoom.Caption = "Zoom"
        Me.tlbZoom.Id = 4
        Me.tlbZoom.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5)
        Me.tlbZoom.Name = "tlbZoom"
        Me.tlbZoom.Visible = True
        '
        'tlbStrumenti
        '
        Me.tlbStrumenti.Caption = "Strumenti"
        Me.tlbStrumenti.Id = 9
        Me.tlbStrumenti.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbImpostaStampante)})
        Me.tlbStrumenti.Name = "tlbStrumenti"
        Me.tlbStrumenti.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionInMenu
        Me.tlbStrumenti.Visible = True
        '
        'tlbImpostaStampante
        '
        Me.tlbImpostaStampante.Caption = "Imposta stampante"
        Me.tlbImpostaStampante.Id = 10
        Me.tlbImpostaStampante.Name = "tlbImpostaStampante"
        Me.tlbImpostaStampante.Visible = True
        '
        'tlbGuida
        '
        Me.tlbGuida.Caption = "Guida"
        Me.tlbGuida.Id = 13
        Me.tlbGuida.Name = "tlbGuida"
        Me.tlbGuida.Visible = True
        '
        'tlbEsci
        '
        Me.tlbEsci.Caption = "Esci"
        Me.tlbEsci.Id = 14
        Me.tlbEsci.Name = "tlbEsci"
        Me.tlbEsci.Visible = True
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Margin = New System.Windows.Forms.Padding(3, 32, 3, 32)
        Me.barDockControlTop.Size = New System.Drawing.Size(682, 29)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 503)
        Me.barDockControlBottom.Margin = New System.Windows.Forms.Padding(3, 32, 3, 32)
        Me.barDockControlBottom.Size = New System.Drawing.Size(682, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 29)
        Me.barDockControlLeft.Margin = New System.Windows.Forms.Padding(3, 32, 3, 32)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 474)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(682, 29)
        Me.barDockControlRight.Margin = New System.Windows.Forms.Padding(3, 32, 3, 32)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 474)
        '
        'pnCOTA
        '
        Me.pnCOTA.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.pnCOTA.Appearance.Options.UseBackColor = True
        Me.pnCOTA.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.pnCOTA.Controls.Add(Me.grHhac)
        Me.pnCOTA.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnCOTA.Location = New System.Drawing.Point(0, 29)
        Me.pnCOTA.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.pnCOTA.LookAndFeel.UseDefaultLookAndFeel = False
        Me.pnCOTA.Margin = New System.Windows.Forms.Padding(3, 32, 3, 32)
        Me.pnCOTA.Name = "pnCOTA"
        Me.pnCOTA.Size = New System.Drawing.Size(682, 474)
        '
        'grHhac
        '
        Me.grHhac.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grHhac.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(3, 32, 3, 32)
        GridLevelNode1.RelationName = "Level1"
        Me.grHhac.LevelTree.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode1})
        Me.grHhac.Location = New System.Drawing.Point(0, 0)
        Me.grHhac.MainView = Me.grvHhac
        Me.grHhac.Margin = New System.Windows.Forms.Padding(3, 32, 3, 32)
        Me.grHhac.Name = "grHhac"
        Me.grHhac.NTSNoRidimensionamentoUltimaColonna = False
        Me.grHhac.Size = New System.Drawing.Size(682, 474)
        Me.grHhac.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.grvHhac})
        '
        'grvHhac
        '
        Me.grvHhac.ActiveFilterEnabled = False
        Me.grvHhac.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.ct_tiponum, Me.ct_eu, Me.ct_uk, Me.ct_usman, Me.ct_uswoman})
        Me.grvHhac.CustomizationFormBounds = New System.Drawing.Rectangle(680, 326, 208, 170)
        Me.grvHhac.Enabled = True
        Me.grvHhac.GridControl = Me.grHhac
        Me.grvHhac.Name = "grvHhac"
        Me.grvHhac.OptionsCustomization.AllowRowSizing = True
        Me.grvHhac.OptionsFilter.AllowFilterEditor = False
        Me.grvHhac.OptionsNavigation.EnterMoveNextColumn = True
        Me.grvHhac.OptionsNavigation.UseTabKey = False
        Me.grvHhac.OptionsSelection.EnableAppearanceFocusedRow = False
        Me.grvHhac.OptionsView.ColumnAutoWidth = False
        Me.grvHhac.OptionsView.EnableAppearanceEvenRow = True
        Me.grvHhac.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden
        Me.grvHhac.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
        Me.grvHhac.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
        Me.grvHhac.OptionsView.ShowGroupPanel = False
        '
        'ct_tiponum
        '
        Me.ct_tiponum.AppearanceCell.Options.UseBackColor = True
        Me.ct_tiponum.AppearanceCell.Options.UseTextOptions = True
        Me.ct_tiponum.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.ct_tiponum.Caption = "Tipo Numero"
        Me.ct_tiponum.Enabled = True
        Me.ct_tiponum.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.ct_tiponum.Name = "ct_tiponum"
        Me.ct_tiponum.NTSSetparam___Tipo = NTSInformatica.NTSGridColumn.NTSSetparamTipoColonna.Numero
        Me.ct_tiponum.Visible = True
        Me.ct_tiponum.VisibleIndex = 0
        '
        'ct_eu
        '
        Me.ct_eu.AppearanceCell.Options.UseBackColor = True
        Me.ct_eu.AppearanceCell.Options.UseTextOptions = True
        Me.ct_eu.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.ct_eu.Caption = "EU"
        Me.ct_eu.Enabled = True
        Me.ct_eu.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.ct_eu.Name = "ct_eu"
        Me.ct_eu.Visible = True
        Me.ct_eu.VisibleIndex = 1
        '
        'ct_uk
        '
        Me.ct_uk.AppearanceCell.Options.UseBackColor = True
        Me.ct_uk.AppearanceCell.Options.UseTextOptions = True
        Me.ct_uk.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.ct_uk.Caption = "UK"
        Me.ct_uk.Enabled = True
        Me.ct_uk.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.ct_uk.Name = "ct_uk"
        Me.ct_uk.Visible = True
        Me.ct_uk.VisibleIndex = 2
        '
        'ct_usman
        '
        Me.ct_usman.AppearanceCell.Options.UseBackColor = True
        Me.ct_usman.AppearanceCell.Options.UseTextOptions = True
        Me.ct_usman.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.ct_usman.Caption = "US MAN"
        Me.ct_usman.Enabled = True
        Me.ct_usman.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.ct_usman.Name = "ct_usman"
        Me.ct_usman.Visible = True
        Me.ct_usman.VisibleIndex = 3
        '
        'ct_uswoman
        '
        Me.ct_uswoman.AppearanceCell.Options.UseBackColor = True
        Me.ct_uswoman.AppearanceCell.Options.UseTextOptions = True
        Me.ct_uswoman.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.ct_uswoman.Caption = "US WOMAN"
        Me.ct_uswoman.Enabled = True
        Me.ct_uswoman.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.ct_uswoman.Name = "ct_uswoman"
        Me.ct_uswoman.Visible = True
        Me.ct_uswoman.VisibleIndex = 4
        '
        'cn_dtini
        '
        Me.cn_dtini.AppearanceCell.Options.UseBackColor = True
        Me.cn_dtini.AppearanceCell.Options.UseTextOptions = True
        Me.cn_dtini.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.cn_dtini.Caption = "Data inizio valitià"
        Me.cn_dtini.Enabled = True
        Me.cn_dtini.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.cn_dtini.Name = "cn_dtini"
        Me.cn_dtini.Visible = True
        Me.cn_dtini.VisibleIndex = 4
        '
        'FRMHHCOTA
        '
        Me.ClientSize = New System.Drawing.Size(682, 503)
        Me.Controls.Add(Me.pnCOTA)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.HelpButton = True
        Me.Margin = New System.Windows.Forms.Padding(3, 26, 3, 26)
        Me.Name = "FRMHHCOTA"
        Me.Text = "CORRISPONDENZA TAGLIE"
        CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pnCOTA, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnCOTA.ResumeLayout(False)
        CType(Me.grHhac, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grvHhac, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#Region "Form"

    Public Overridable Sub FRMHHCOTA_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Try
            dcCOTA.Dispose()
            If Not dsCOTA Is Nothing Then dsCOTA.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Public Overridable Sub FRMHHANC_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            If Not Salva() Then e.Cancel = True
        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub

    Public Overridable Sub FRMHHCOTA_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try


            'Predispongo i controlli
            GctlSetRoules()
            InitControls()

            '-------------------------------------------------
            'Leggo dal database i dati e collego l'NTSBinding
            grHhac.DataSource = Nothing
            If oCleCOTA.Apri(DittaCorrente, dsCOTA) Then
                dcCOTA.DataSource = dsCOTA.Tables(strNomeTabella)
                dsCOTA.AcceptChanges()
                grHhac.DataSource = dcCOTA
            End If


        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub
#End Region

#Region "Griglia"

    Public Overridable Sub grvHhac_NTSFocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles grvHhac.NTSFocusedRowChanged
        Try

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub

    Public Overridable Sub grvHhac_NTSBeforeRowUpdate(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowAllowEventArgs) Handles grvHhac.NTSBeforeRowUpdate
        Try
            If Not Salva() Then
                'Rimango sulla riga su cui sono
                e.Allow = False
            End If

        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub


#End Region

    Public Overridable Sub InitControls()
        InitControlsBeginEndInit(Me, False)

        Try
            'Carico le immagini della Toolbar
            Try
                tlbNuovo.GlyphPath = (oApp.ChildImageDir & "\new.png")
                tlbCancella.GlyphPath = (oApp.ChildImageDir & "\delete.png")
                tlbRipristina.GlyphPath = (oApp.ChildImageDir & "\restore.png")
                tlbZoom.GlyphPath = (oApp.ChildImageDir & "\zoom.png")
                tlbStrumenti.GlyphPath = (oApp.ChildImageDir & "\options.png")
                tlbGuida.GlyphPath = (oApp.ChildImageDir & "\help.png")
                tlbEsci.GlyphPath = (oApp.ChildImageDir & "\exit.png")
            Catch ex As Exception
            End Try
            tlbMain.NTSSetToolTip()


            grvHhac.NTSSetParam(oMenu, "CORRISPONDENZA TAGLIE")

            ct_tiponum.NTSSetParamNUM(oMenu, "Tipo Numero", "0", 15)
            ct_eu.NTSSetParamSTR(oMenu, "EU", 10)
            ct_uk.NTSSetParamSTR(oMenu, "UK", 10)
            ct_uswoman.NTSSetParamSTR(oMenu, "US WOMAN", 10)
            ct_usman.NTSSetParamSTR(oMenu, "US MAN", 10)


            'Chiamo lo script per inizializzare i controlli caricati con Source Extender
            NTSScriptExec("InitControls", Me, Nothing)

        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
        InitControlsBeginEndInit(Me, True)
    End Sub

    Public Overridable Function Salva() As Boolean
        Try
            '----------------------------------------------------------------------------------------------------------
            Me.ValidaLastControl()
            '----------------------------------------------------------------------------------------------------------
            Select Case Me.NTSChiediSalva(grvHhac, oCleCOTA.RecordIsChanged)
                Case DialogResult.Yes : If Not oCleCOTA.Salva(False) Then Return False
                Case DialogResult.No : oCleCOTA.Ripristina(dcCOTA.Position, dcCOTA.Filter)
                Case DialogResult.Cancel : Return False
            End Select
            '----------------------------------------------------------------------------------------------------------
            Return True
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Function

#Region "Toolbar"

    Public Overridable Sub tlbNuovo_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbNuovo.ItemClick
        Try
            grvHhac.NTSNuovo()
        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub

    Public Overridable Sub tlbCancella_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbCancella.ItemClick
        Try
            '---------------------------------------------------------------------------------------------------------
            If grvHhac.NTSGetCurrentDataRow Is Nothing Then Return
            '---------------------------------------------------------------------------------------------------------
            If Me.NTSChiediCancella(grvHhac) = DialogResult.Yes Then oCleCOTA.Salva(True)
            '---------------------------------------------------------------------------------------------------------
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub

    Public Overridable Sub tlbRipristina_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbRipristina.ItemClick
        Try
            If grvHhac.NTSRipristinaRigaCorrenteBefore(dcCOTA, CType(IIf(sender Is Nothing, False, True), Boolean)) Then
                oCleCOTA.Ripristina(dcCOTA.Position, dcCOTA.Filter)
                grvHhac.NTSRipristinaRigaCorrenteAfter()
            End If
        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub

    Public Overridable Sub tlbZoom_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbZoom.ItemClick

        Try
            NTSCallStandardZoom()

        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub

    Public Overridable Sub tlbGuida_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbGuida.ItemClick
        SendKeys.Send("{F1}")
    End Sub

    Public Overridable Sub tlbEsci_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbEsci.ItemClick
        Try
            If Not Salva() Then Return
            Me.Close()
        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub

    Public Overridable Sub tlbImpostaStampante_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbImpostaStampante.ItemClick
        Try
            oMenu.ReportImposta(Me)
        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub

    Private Sub grHhac_Click(sender As Object, e As EventArgs) Handles grHhac.Click

    End Sub


#End Region

End Class
