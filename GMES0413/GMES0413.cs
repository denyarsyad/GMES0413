using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JPlatform.Client.JERPBaseForm6;
using JPlatform.Client.Library6.interFace;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using System.Diagnostics;
using JPlatform.Client.Controls6;
using DevExpress.XtraGrid;
using JPlatform.Client.CSIGMESBaseform6;
using System.Net;
using System.Reflection;
using System.IO;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using System.Collections;
using CSI.MES.P.DAO;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Net.Sockets;
using System.Management;
using DevExpress.Export;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing.Drawing2D;
using System.Net.Mail;
using System.Net;
using System.Globalization;
using DevExpress.XtraReports.UI;
using Oracle.ManagedDataAccess.Client;

namespace CSI.MES.P
{
    public partial class GMES0413 : CSIGMESBaseform6
    {
        public GMES0413()
        {
            InitializeComponent();
        }

        DataTable dtItem = new DataTable();
        DataTable dtDest = new DataTable();
        DataTable dtNumber = new DataTable();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            NewButton = true;
            DeleteButton = true;
            PreviewButton = true;
            PrintButton = false;
            AddButton = false;
            DeleteRowButton = false;
            SaveButton = false;

            //MAIN REGISTER
            dtEFrom.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
            dtETo.EditValue = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            fnGetCbo("GET_STATUS");
            txtInterval.Text = "5";
            chkAutoRf.CheckState = CheckState.Checked;

            //WAITING LIST
            dtFrom.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
            dtTo.EditValue = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            txtMnt.Text = "5";
            chkARef.CheckState = CheckState.Checked;

            InitControls(grdMain);
            InitControls(grdWaitingList);

            #region [DESIGN MAIN REGISTER]
            lblDate.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblTo.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblStatus.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblMinutes.Font = new Font("Calibri", 12, FontStyle.Bold);

            dtEFrom.Font = new Font("Calibri", 12, FontStyle.Bold);
            dtETo.Font = new Font("Calibri", 12, FontStyle.Bold);
            cboStatus.Font = new Font("Calibri", 12, FontStyle.Bold);
            txtInterval.Font = new Font("Calibri", 12, FontStyle.Bold);
            chkAutoRf.Font = new Font("Calibri", 12, FontStyle.Bold);

            gvwMain.OptionsView.ShowGroupPanel = false;
            //gvwMain.ColumnPanelRowHeight = 40;
            gvwMain.OptionsView.ShowColumnHeaders = false;
            gvwMain.BandPanelRowHeight = 40;

            gvwMain.Bands[5].Children[0].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
            gvwMain.Bands[5].Children[0].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gvwMain.Bands[5].Children[0].Width = 50;
            gvwMain.Bands[5].Children[1].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
            gvwMain.Bands[5].Children[1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gvwMain.Bands[5].Children[1].Width = 50;


            gvwMain.Bands[6].Children[0].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
            gvwMain.Bands[6].Children[0].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gvwMain.Bands[6].Children[0].Width = 50;
            gvwMain.Bands[6].Children[1].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
            gvwMain.Bands[6].Children[1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gvwMain.Bands[6].Children[1].Width = 50;
            gvwMain.Bands[6].Children[2].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
            gvwMain.Bands[6].Children[2].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gvwMain.Bands[6].Children[2].Width = 50;

            for (int i = 0; i < gvwMain.Columns.Count; i++)
            {
                if (i < 15)
                {
                    gvwMain.Bands[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gvwMain.Bands[i].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
                    gvwMain.Bands[i].AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                }
            }

            pctRed.BackColor = ColorTranslator.FromHtml("#F47174");
            pctRed.Image = new Bitmap(1, 1);
            pctYellow.BackColor = ColorTranslator.FromHtml("#FFFDD0");
            pctYellow.Image = new Bitmap(1, 1);
            pctGrey.BackColor = ColorTranslator.FromHtml("#D1CFC8");
            pctGrey.Image = new Bitmap(1, 1);
            pctGreen.BackColor = ColorTranslator.FromHtml("#EAFFDE");
            pctGreen.Image = new Bitmap(1, 1);

            lblRed.Font = new Font("Calibri", 10, FontStyle.Regular);
            lblRed.Text = "Cancelled";
            lblYellow.Font = new Font("Calibri", 10, FontStyle.Regular);
            lblYellow.Text = "Registered/Belum Input GA";
            lblGrey.Font = new Font("Calibri", 10, FontStyle.Regular);
            lblGrey.Text = "Waktu Keberangkatan Sudah Kadaluwarsa";
            lblGreen.Font = new Font("Calibri", 10, FontStyle.Regular);
            lblGreen.Text = "Finished/GA Sudah Input"; 

            #endregion

            #region [DESIGN WAITING LIST]

            lblDt.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblDash.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblMnt.Font = new Font("Calibri", 12, FontStyle.Bold);

            dtFrom.Font = new Font("Calibri", 12, FontStyle.Bold);
            dtTo.Font = new Font("Calibri", 12, FontStyle.Bold);
            txtMnt.Font = new Font("Calibri", 12, FontStyle.Bold);
            chkARef.Font = new Font("Calibri", 12, FontStyle.Bold);

            lblToday.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblBooked.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblStandby.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblcolon.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblcolon1.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblcolon2.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblRToday.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblRBooked.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblRStandby.Font = new Font("Calibri", 12, FontStyle.Bold);

            lblTomorrow.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblBookedT.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblStandbyT.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblcolon3.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblcolon4.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblcolon5.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblRTomorrow.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblRBookedT.Font = new Font("Calibri", 12, FontStyle.Bold);
            lblRStandbyT.Font = new Font("Calibri", 12, FontStyle.Bold);


            gvwWaitingList.OptionsView.ShowGroupPanel = false;
            //gvwWaitingList.ColumnPanelRowHeight = 40;
            gvwWaitingList.OptionsView.ShowColumnHeaders = false;
            gvwWaitingList.BandPanelRowHeight = 40;

            gvwWaitingList.Bands[5].Children[0].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
            gvwWaitingList.Bands[5].Children[0].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gvwWaitingList.Bands[5].Children[0].Width = 50;
            gvwWaitingList.Bands[5].Children[1].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
            gvwWaitingList.Bands[5].Children[1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gvwWaitingList.Bands[5].Children[1].Width = 50;


            gvwWaitingList.Bands[6].Children[0].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
            gvwWaitingList.Bands[6].Children[0].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gvwWaitingList.Bands[6].Children[0].Width = 50;
            gvwWaitingList.Bands[6].Children[1].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
            gvwWaitingList.Bands[6].Children[1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gvwWaitingList.Bands[6].Children[1].Width = 50;
            gvwWaitingList.Bands[6].Children[2].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
            gvwWaitingList.Bands[6].Children[2].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gvwWaitingList.Bands[6].Children[2].Width = 50;

            for (int i = 0; i < gvwWaitingList.Columns.Count; i++)
            {
                if (i < 15)
                {
                    gvwWaitingList.Bands[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gvwWaitingList.Bands[i].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
                    gvwWaitingList.Bands[i].AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                }
            }

            #endregion

            fnGetCboPop("GET_CBO_DEPT");
            fnGetCboPopDest("GET_CBO_DESTINATION");
            fnGetCboPopNumber("GET_NUMBER");
            //fnGetAdmin();

            fnGetCarStockInfo("CAR_STOCK_INFO", DateTime.Now.ToString("yyyyMMdd"));

            QueryClick();
        }

        public override void QueryClick()
        {
            base.QueryClick();

            InitControls(grdMain);
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                fnSearchW("GET_WAITING_LIST", dtFrom.DateTime.ToString("yyyyMMdd"), dtTo.DateTime.ToString("yyyyMMdd"), "");
            }
            else
            {
                fnSearch("GET_DATA", dtEFrom.DateTime.ToString("yyyyMMdd"), dtETo.DateTime.ToString("yyyyMMdd"), cboStatus.EditValue.ToString());
            }

            fnGetCarStockInfo("CAR_STOCK_INFO", DateTime.Now.ToString("yyyyMMdd"));
        }

        public override void NewClick()
        {
            base.NewClick();

            try
            {
                DataTable dtData = new DataTable();
                dtData.Columns.Add("CODE");
                dtData.Columns.Add("NAME");
                dtData.Rows.Add("USER_ID", SessionInfo.UserID);
                dtData.Rows.Add("USER_NAME", SessionInfo.UserName);
                string nik = getNik(SessionInfo.UserID);
                dtData.Rows.Add("NIK", nik);

                if (nik.Length > 0)
                {
                    dtData.Rows.Add("NIK_NAME", getName(nik));
                    dtData.Rows.Add("DEPT", getDept(nik));
                    dtData.Rows.Add("MESS_YN", getMessYn(nik));
                }
                else
                {
                    dtData.Rows.Add("NIK_NAME", "");
                    dtData.Rows.Add("DEPT", "010020");
                    dtData.Rows.Add("MESS_YN", "");
                }

                Register rgs = new Register(dtData, dtItem, dtDest, dtNumber, this);
                rgs.ShowDialog();

                #region [COMMENT]
                //string nik = getNik(SessionInfo.UserID);
                //if (nik.Length > 0)
                //{
                //    dtData.Rows.Add("NIK", nik);
                //}
                //else
                //{
                //    dtData.Rows.Add("NIK", "");
                //}
                #endregion
            }
            catch (Exception ex)
            {
                MessageBoxW("NewClick " + ex.Message);
            }
        }

        public override void SaveClick()
        {
            base.SaveClick();

            try
            {
                if (this.SetYesNoMessageBox("Are you sure?", "Save Data", IconType.Warning) == DialogResult.Yes)
                {
                    gvwMain.PostEditor();
                    gvwMain.UpdateCurrentRow();

                    int cntSucced = 0;
                    int cntError = 0;

                    DataTable dt = grdMain.DataSource as DataTable;
                    if (dt != null)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string ck = row.RowState.ToString();
                            if (row.RowState == DataRowState.Modified)
                            {
                                string rentalNo = row["RENT_ID"].ToString();
                                string driverId = row["DRIVER_ID"].ToString();
                                string detailCar = row["DETAIL_CAR"].ToString();

                                if (rentalNo != "" && driverId != "" && detailCar != "")
                                {
                                    fnUpdate(rentalNo, driverId, detailCar);
                                    cntSucced++;
                                }
                                else
                                {
                                    cntError++;
                                }
                            }
                        }

                        dt.AcceptChanges();
                    }

                    #region [OLD]
                    //GridView vw = grdMain.MainView as GridView;
                    //for (int i = 0; i < vw.DataRowCount; i++)
                    //{
                    //    string rentId = vw.GetRowCellValue(i, "RENT_ID").ToString();
                    //    string driverId = vw.GetRowCellValue(i, "DRIVER_ID").ToString();
                    //    string car = vw.GetRowCellValue(i, "DETAIL_CAR").ToString();

                    //    //string originalDriverId = GetOriginalValue(rentId, "DRIVER_ID");
                    //    //string originalCar = GetOriginalValue(rentId, "DETAIL_CAR");

                    //    if (!string.IsNullOrEmpty(driverId))
                    //    {
                    //        fnUpdate(rentId, driverId, car);
                    //    }
                    //}
                    #endregion

                    QueryClick();
                    if (cntSucced > 0 && cntError == 0)
                    {
                        MessageBoxW("Update Succeed: " + cntSucced);
                    }
                    else
                    {
                        MessageBoxW("Update Succeed: " + cntSucced + " & Unsucceed: " + cntError);
                    }
                    cntSucced = 0;
                    cntError = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("SaveClick " + ex.Message);
            }
        }

        public override void PreviewClick()
        {
            base.PreviewClick();

            try
            {
                if (xtraTabControl1.SelectedTabPageIndex == 0)
                {
                    int row = gvwWaitingList.FocusedRowHandle;
                    string col = gvwWaitingList.Columns["W_RENT_ID"].FieldName;
                    string cekId = gvwWaitingList.GetRowCellValue(row, col).ToString();

                    fnPreview("GET_PREVIEW", cekId);
                }
                else
                {
                    int row = gvwMain.FocusedRowHandle;
                    string col = gvwMain.Columns["RENT_ID"].FieldName;
                    string cekId = gvwMain.GetRowCellValue(row, col).ToString();

                    fnPreview("GET_PREVIEW", cekId);
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("PreviewClick " + ex.Message);
            }
        }

        public override void DeleteClick()
        {
            base.DeleteClick();

            try
            {
                if (xtraTabControl1.SelectedTabPageIndex == 0)
                {
                    int rowFocused = gvwWaitingList.FocusedRowHandle;
                    string col = gvwWaitingList.Columns[0].FieldName;
                    string cekId = gvwWaitingList.GetRowCellValue(rowFocused, col).ToString();
                    string userId = SessionInfo.UserID;

                    fnSearchCancel("GET_DATA_CANCEL", cekId, userId);
                }
                else
                {
                    int rowFocused = gvwMain.FocusedRowHandle;
                    string col = gvwMain.Columns[0].FieldName;
                    string cekId = gvwMain.GetRowCellValue(rowFocused, col).ToString();
                    string userId = SessionInfo.UserID;

                    fnSearchCancel("GET_DATA_CANCEL", cekId, userId);
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("DeleteClick " + ex.Message);
            }
        }

        private void fnGetCbo(string paramType)
        {
            try
            {
                SP_GMES0413 cProc = new SP_GMES0413();
                DataTable dtData = null;
                dtData = cProc.SetParamData(dtData, paramType, "", "", "");
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];
                    if (dtData.Rows.Count > 0)
                    {
                        cboStatus.Properties.DataSource = dtData;
                        cboStatus.Properties.DisplayMember = "NAME";
                        cboStatus.Properties.ValueMember = "CODE";

                        DataRow[] defRow = dtData.Select("CODE = 'A'");
                        cboStatus.EditValue = defRow.Length > 0 ? cboStatus.EditValue = defRow[0]["CODE"] : cboStatus.EditValue = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnGetCbo " + ex.Message);
            }
        }

        private void fnGetCboPop(string paramType)
        {
            try
            {
                SP_GMES0413 cProc = new SP_GMES0413();
                DataTable dtData = null;
                dtData = cProc.SetParamData(dtData, paramType, "", "", "");
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtItem = rs.ResultDataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnGetCbo " + ex.Message);
            }
        }

        private void fnSearch(string paramType, string paramFrom, string paramTo, string paramStatus)
        {
            try
            {
                //REFRESH GRID
                while (gvwMain.RowCount > 0)
                {
                    gvwMain.DeleteRow(0);
                }

                SP_GMES0413 cProc = new SP_GMES0413();
                DataTable dtData = null;
                DataTable dtAdmin = null;
                
                dtData = cProc.SetParamData(dtData, paramType, paramFrom, paramTo, paramStatus);
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];
                    if (dtData.Rows.Count > 0)
                    {
                        grdMain.DataSource = dtData;
                        PreviewButton = true;
                        //SetData(grdMain, dtData);
                        dtData.AcceptChanges();

                        DataTable dtDriver = null;
                        DataTable dtCar = null;
                        dtDriver = cProc.SetParamData(dtDriver, "GET_DRIVER", "", "");
                        dtCar = cProc.SetParamData(dtCar, "GET_CAR", "", "");
                        ResultSet rsDrvr = CommonCallQuery(dtDriver, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);
                        ResultSet rsCar = CommonCallQuery(dtCar, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);
                        if (rsDrvr != null && rsDrvr.ResultDataSet.Tables.Count > 0)
                        {
                            dtDriver = rsDrvr.ResultDataSet.Tables[0];
                            if (dtDriver.Rows.Count > 0)
                            {
                                repDriver.DataSource = dtDriver;
                                repDriver.DisplayMember = "NAME";
                                repDriver.ValueMember = "CODE";
                            }
                        }
                        
                        if (rsCar != null && rsCar.ResultDataSet.Tables.Count > 0)
                        {
                            dtCar = rsCar.ResultDataSet.Tables[0];
                            if (dtCar.Rows.Count > 0)
                            {
                                repCar.DataSource = dtCar;
                                repCar.DisplayMember = "NAME";
                                repCar.ValueMember = "CODE";
                            }
                        }


                        fnDesign();

                        ////VALIDASI ADMIN
                        //dtAdmin = cProc.SetParamData(dtAdmin, "GET_ADMIN");
                        //ResultSet rSet = CommonCallQuery(dtAdmin, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);
                        //dtAdmin = rSet.ResultDataSet.Tables[0];
                        //if (dtAdmin.Rows.Count > 0)
                        //{
                        //    foreach (DataRow rw in dtAdmin.Rows)
                        //    {
                        //        string adm = rw[0].ToString();
                        //        if (SessionInfo.UserID.ToUpper().Contains(adm.ToUpper()))
                        //        {
                        //            gvwMain.Columns["DRIVER_ID"].OptionsColumn.AllowEdit = true;
                        //            gvwMain.Columns["DETAIL_CAR"].OptionsColumn.AllowEdit = true;
                        //            //PreviewButton = true;
                        //            return;
                        //        }
                        //    }
                        //}
                    }
                    else
                    {
                        PreviewButton = false;
                        //fnDesign();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBoxW("fnSearch " + ex.Message);
            }
        }

        private void fnSearchW(string paramType, string paramFrom, string paramTo, string paramStatus)
        {
            try
            {
                //REFRESH GRID
                while (gvwWaitingList.RowCount > 0)
                {
                    gvwWaitingList.DeleteRow(0);
                }

                SP_GMES0413 cProc = new SP_GMES0413();
                DataTable dtData = null;

                dtData = cProc.SetParamData(dtData, paramType, paramFrom, paramTo, paramStatus);
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];
                    if (dtData.Rows.Count > 0)
                    {
                        grdWaitingList.DataSource = dtData;
                        PreviewButton = true;
                        dtData.AcceptChanges();

                        fnDesignW();
                    }
                    else
                    {
                        PreviewButton = false;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBoxW("fnSearch " + ex.Message);
            }
        }

        public bool fnSave(string RENTAL_DATE, string RENTAL_TIME, string USER_ID, string USER_NAME, string EMP_ID, string DEPT_CODE, string DEPT_NAME,
                           string START_DATE, string END_DATE, string DESTINATION, string PURPOSES, string PASSENGER, string MEMBERS, string IS_SEND_MAIL, 
                           string EMP_NAME, string MEMO)
        {
            bool isSucced = false;

            try
            {
                if (this.SetYesNoMessageBox("Are you sure?", "Save Data", IconType.Warning) == DialogResult.Yes)
                {
                    pbProgressShow();

                    string status = MEMBERS.pxToInt() < 3 ? "W" : "R";
                    int cekCarStock = fnCheckCarStock("CHECK_CAR_STOCK", START_DATE);
                    if (cekCarStock == 0 && status == "R")
                    {
                        status = "C";
                    }

                    SP_GMES0413 cProc = new SP_GMES0413("S");
                    DataTable dtData = null;

                    dtData = cProc.SetParamDataInsert(dtData,
                                                      "SAVE",       //ACTION
                                                      RENTAL_DATE,
                                                      RENTAL_TIME,
                                                      "GA",
                                                      "3110",
                                                      USER_ID,
                                                      USER_NAME,
                                                      EMP_ID,
                                                      DEPT_CODE,
                                                      DEPT_NAME,
                                                      "",
                                                      PURPOSES, //ACTIVITIY_CD
                                                      "",
                                                      DESTINATION, //PLACE_DESC
                                                      "",
                                                      MEMBERS, // MEMBER OF EMPLOYEE
                                                      PASSENGER, //USE_DESC
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      START_DATE,
                                                      END_DATE,
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      status, //STATUS --> WAITING LIST OR REGISTER
                                                      MEMO, //DATA MEMO
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      SessionInfo.UserID,   //CREATOR
                                                      DateTime.Now.ToString("yyyyMMdd HHmmss"), //CREATED_DT
                                                      Dns.GetHostName() + "|" + getIpAddress() + "|" + GetMacAddress(), //CREATE_PC
                                                      "",
                                                      "",
                                                      ""
                                                      );

                    if (dtData != null)
                    {
                        if (PURPOSES != "" && EMP_NAME != "")
                        {
                            DateTime departureDt = (START_DATE != "" ? DateTime.ParseExact(START_DATE, "yyyyMMdd HHmm", CultureInfo.InvariantCulture) : DateTime.ParseExact("19990101", "yyyyMMdd", CultureInfo.InvariantCulture));
                            string departureDay = departureDt.DayOfWeek.ToString().Trim().ToUpper();
                            DateTime regDt = (RENTAL_DATE != "" ? DateTime.ParseExact(RENTAL_DATE + " " + RENTAL_TIME, "yyyyMMdd HHmmss", CultureInfo.InvariantCulture) : DateTime.ParseExact("19990101 083000", "yyyyMMdd HHmmss", CultureInfo.InvariantCulture));
                            string regDay = regDt.DayOfWeek.ToString().Trim().ToUpper();

                            //VARIABEL VALIDASI BARU
                            int regDayNum = ConvertDayToNumber(regDt); //HARI PADA SAAT REGISTRASI
                            int depDayNum = ConvertDayToNumber(departureDt); //HARI KEBERANGKATAN
                            TimeSpan regTime = TimeSpan.ParseExact(regDt.ToString("HHmm"), "hhmm", CultureInfo.InvariantCulture); //WAKTU REGISTRASI
                            //string depHour = departureDt.ToString("HHmm"); //JAM KEBERANGKATAN

                            if (departureDt > regDt)
                            {
                                #region [CURRENT VALIDATION]
                                //VALIDASI KEBERANGKATAN SABTU
                                if (depDayNum == 7) //JIKA PEMESANAN UNTUK HARI SABTU (7)
                                {
                                    if (regDayNum < 6) //JIKA REGISTRASI DILAKUKAN SEBELUM HARI JUMAT (6), KAMIS ATAU SEBELUMNYA (< 6)
                                    {
                                        MessageBoxW("Pemesanan untuk hari Sabtu hanya bisa dilakukan mulai Jumat pukul 00:01 AM hingga Sabtu pukul 08:59 AM.", IconType.Error);
                                        pbProgressHide();
                                        return isSucced = false;
                                    }
                                    else if (regDayNum == 6) //JIKA REGISTRASI DILAKUKAN PADA HARI JUMAT (6)
                                    {
                                        if (regTime < TimeSpan.ParseExact("0001", "hhmm", CultureInfo.InvariantCulture)) //JIKA REGISTRASI SEBELUM PUKUL 00:01 AM
                                        {
                                            MessageBoxW("Pemesanan untuk hari Sabtu hanya bisa dilakukan mulai Jumat pukul 00:01 AM hingga Sabtu pukul 08:59 AM.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }
                                    }
                                    else if (regDayNum == 7) //JIKA REGISTRASI DI LAKUKAN PADA HARI SABTU (7) (BUKAN DI MINGGU DEPAN)
                                    {
                                        if ((departureDt - regDt).TotalDays > 6)
                                        {
                                            MessageBoxW("Pemesanan untuk hari Sabtu hanya bisa dilakukan mulai Jumat sebelum hari keberangkatan pukul 00:01 AM hingga hari keberangkatan pukul 08:59 AM.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }

                                        if (regTime >= TimeSpan.ParseExact("0900", "hhmm", CultureInfo.InvariantCulture)) //JIKA REGISTRASI SETELAH PUKUL 09:00 AM
                                        {
                                            MessageBoxW("Pemesanan untuk hari Sabtu harus sebelum jam 09:00.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }
                                    }
                                }

                                //COBA VALIDASI KEBERANGKATAN MINGGU
                                if (depDayNum == 1) //JIKA PEMESANAN UNTUK HARI MINGGU (1)
                                {
                                    if (regDayNum < 6) //JIKA REGISTRASI DILAKUKAN SEBELUM HARI JUMAT (6), KAMIS ATAU SEBELUMNYA (< 6)
                                    {
                                        MessageBoxW("Pemesanan untuk hari Minggu hanya bisa dilakukan mulai Jumat pukul 00:01 AM hingga Sabtu pukul 08:59 AM.", IconType.Error);
                                        pbProgressHide();
                                        return isSucced = false;
                                    }
                                    else if (regDayNum == 6) //JIKA REGISTRASI DILAKUKAN PADA HARI JUMAT (6)
                                    {
                                        if (regTime < TimeSpan.ParseExact("0001", "hhmm", CultureInfo.InvariantCulture)) //JIKA REGISTRASI SEBELUM PUKUL 00:01 AM
                                        {
                                            MessageBoxW("Pemesanan untuk hari Sabtu hanya bisa dilakukan mulai Jumat pukul 00:01 AM hingga Sabtu pukul 08:59 AM.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }
                                    }
                                    else if (regDayNum == 7) //JIKA REGISTRASI DI LAKUKAN PADA HARI SABTU (7)
                                    {
                                        if ((departureDt - regDt).TotalDays > 6)
                                        {
                                            MessageBoxW("Pemesanan untuk hari Minggu hanya bisa dilakukan mulai Jumat sebelum hari keberangkatan pukul 00:01 AM hingga Sabtu sebelum hari keberangkatan pukul 08:59 AM.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }

                                        if (regTime >= TimeSpan.ParseExact("0900", "hhmm", CultureInfo.InvariantCulture)) //JIKA REGISTRASI SETELAH PUKUL 09:00 AM
                                        {
                                            MessageBoxW("Pemesanan untuk hari Minggu harus sebelum jam 09:00 pada hari Sabtu.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }
                                    }
                                }

                                //COBA VALIDASI KEBERANGKATAN SENIN
                                if (depDayNum == 2) //JIKA PEMESANAN UNTUK HARI SENIN (2)
                                {
                                    if (regDayNum < 6) //JIKA REGISTRASI DILAKUKAN SEBELUM HARI JUMAT (6), KAMIS ATAU SEBELUMNYA (< 6)
                                    {
                                        MessageBoxW("Pemesanan untuk hari Senin hanya bisa dilakukan mulai Jumat pukul 00:01 AM hingga Sabtu pukul 08:59 AM.", IconType.Error);
                                        pbProgressHide();
                                        return isSucced = false;
                                    }
                                    else if (regDayNum == 6) //JIKA REGISTRASI DILAKUKAN PADA HARI JUMAT (6)
                                    {
                                        if (regTime < TimeSpan.ParseExact("0001", "hhmm", CultureInfo.InvariantCulture)) //JIKA REGISTRASI SEBELUM PUKUL 00:01 AM
                                        {
                                            MessageBoxW("Pemesanan untuk hari Sabtu hanya bisa dilakukan mulai Jumat pukul 00:01 AM hingga Sabtu pukul 08:59 AM.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        };
                                    }
                                    else if (regDayNum == 7) //JIKA REGISTRASI DI LAKUKAN PADA HARI SABTU (7)
                                    {
                                        if ((departureDt - regDt).TotalDays > 6)
                                        {
                                            MessageBoxW("Pemesanan untuk hari Senin hanya bisa dilakukan mulai Jumat sebelum hari keberangkatan pukul 00:01 AM hingga Sabtu sebelum hari keberangkatan pukul 08:59 AM.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }

                                        if (regTime >= TimeSpan.ParseExact("0900", "hhmm", CultureInfo.InvariantCulture)) //JIKA REGISTRASI SETELAH PUKUL 09:00 AM 
                                        {
                                            MessageBoxW("Pemesanan untuk hari Senin harus sebelum jam 09:00 pada hari Sabtu.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }
                                    }
                                }

                                //COBA VALIDASI KEBERANGAKATAN SELASA, RABU, KAMIS, JUMAT.
                                if (depDayNum >= 3 && depDayNum <= 6) //JIKA PEMESANAN UNTUK HARI SELASA (3), RABU (4), KAMIS (5), JUMAT (6)
                                {
                                    if (regDayNum == (depDayNum - 1)) //JIKA REGISTRASI DILAKUKAN SEHARI SEBELUMNYA
                                    {
                                        if (regTime < TimeSpan.ParseExact("0001", "hhmm", CultureInfo.InvariantCulture)) //JIKA REGISTRASI SEBELUM PUKUL 00:01 AM
                                        {
                                            MessageBoxW("Registrasi hanya bisa dilakukan mulai pukul 00:01 sehari sebelum keberangkatan.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }
                                    }
                                    else if (regDayNum == depDayNum) // JIKA REGISTRASI DILAKUKAN DI HARI KEBERANGKATAN
                                    {
                                        if (regTime > TimeSpan.ParseExact("1400", "hhmm", CultureInfo.InvariantCulture)) //JIKA REGISTRASI MELEWATI PUKU 2:00 PM 
                                        {
                                            MessageBoxW("Registrasi tidak bisa dilakukan setelah pukul 2:00 PM di hari keberangkatan.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }
                                    }
                                    //int cek = (depDayNum - 1);
                                    //if (regDayNum < (depDayNum - 1)) 
                                    else if (regDayNum != (depDayNum - 1))//JIKA REGISTRASI DILAKUKAN LEBIH DARI SEHARI SEBELUMNYA
                                    {
                                        MessageBoxW("Registrasi hanya bisa dilakukan sehari sebelum keberangkatan mulai pukul 00:01 hingga hari keberangkatan pukul 2:30 PM.", IconType.Error);
                                        pbProgressHide();
                                        return isSucced = false;
                                    }
                                }
                                #endregion


                                if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                                {
                                    if (status == "W")
                                    {
                                        MessageBoxW("A minimum of 3 passengers is required. You will be placed on the Waiting List", IconType.Information);
                                        xtraTabControl1.SelectedTabPageIndex = 0;
                                    }
                                    else if (status == "C")
                                    {
                                        MessageBoxW("The standby car stock for rental is fully booked!", IconType.Information);
                                        xtraTabControl1.SelectedTabPageIndex = 1;
                                    }
                                    else
                                    {
                                        MessageBoxW("Register Succeed");
                                        xtraTabControl1.SelectedTabPageIndex = 1;
                                    }

                                    isSucced = true;
                                    //REFRESH PAGE
                                    if (xtraTabControl1.SelectedTabPageIndex == 0)
                                    {
                                        dtFrom.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
                                        dtTo.EditValue = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                                        fnSearchW("GET_WAITING_LIST", dtFrom.DateTime.ToString("yyyyMMdd"), dtTo.DateTime.ToString("yyyyMMdd"), "");
                                    }
                                    else
                                    {
                                        dtFrom.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
                                        dtTo.EditValue = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                                        fnSearch("GET_DATA", dtEFrom.DateTime.ToString("yyyyMMdd"), dtETo.DateTime.ToString("yyyyMMdd"), cboStatus.EditValue.ToString());
                                    }
                                    fnGetCarStockInfo("CAR_STOCK_INFO", DateTime.Now.ToString("yyyyMMdd"));

                                    // Kirim email jika diaktifkan dan passengers > 2
                                    if (IS_SEND_MAIL == "Y" && Convert.ToInt16(MEMBERS) > 2)
                                    {
                                        fnSentMail(RENTAL_DATE, RENTAL_TIME, EMP_NAME, USER_NAME, EMP_ID, DEPT_CODE, DEPT_NAME, START_DATE, END_DATE, DESTINATION, PURPOSES, PASSENGER);
                                    }
                                }
                                else
                                {
                                    MessageBoxW("Failed Saved");
                                }

                                #region [VALIDATION FROM PROC]
                                //ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);
                                //if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                                //{
                                //    dtData = rs.ResultDataSet.Tables[0];

                                //    if (dtData.Rows.Count > 0)
                                //    {
                                //        MessageBoxW("OK nih");
                                //        //if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                                //        //{
                                //        //    if (status == "W")
                                //        //    {
                                //        //        MessageBoxW("A minimum of 3 passengers is required. You will be placed on the Waiting List");
                                //        //        xtraTabControl1.SelectedTabPageIndex = 0;
                                //        //    }
                                //        //    else if (status == "C")
                                //        //    {
                                //        //        MessageBoxW("The standby car stock for rental is fully booked!");
                                //        //        xtraTabControl1.SelectedTabPageIndex = 1;
                                //        //    }
                                //        //    else
                                //        //    {
                                //        //        MessageBoxW("Register Succeed");
                                //        //        xtraTabControl1.SelectedTabPageIndex = 1;
                                //        //    }

                                //        //    isSucced = true;
                                //        //    QueryClick();

                                //        //    // Kirim email jika diaktifkan
                                //        //    if (IS_SEND_MAIL == "Y")
                                //        //    {
                                //        //        fnSentMail(RENTAL_DATE, RENTAL_TIME, EMP_NAME, USER_NAME, EMP_ID, DEPT_CODE, DEPT_NAME, START_DATE, END_DATE, DESTINATION, PURPOSES, PASSENGER);
                                //        //    }
                                //        //}
                                //        //else
                                //        //{
                                //        //    MessageBoxW("Failed Saved");
                                //        //}
                                //        #region [OLD]
                                //        //if (dtData.Rows[0][0].ToString().Contains("SUCCEED"))
                                //        //{
                                //        //    if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                                //        //    {
                                //        //        if (status == "W")
                                //        //        {
                                //        //            MessageBoxW("A minimum of 3 passengers is required. You will be placed on the Waiting List");
                                //        //            xtraTabControl1.SelectedTabPageIndex = 0;
                                //        //        }
                                //        //        else if (status == "C")
                                //        //        {
                                //        //            MessageBoxW("The standby car stock for rental is fully booked!");
                                //        //            xtraTabControl1.SelectedTabPageIndex = 1;
                                //        //        }
                                //        //        else
                                //        //        {
                                //        //            MessageBoxW("Register Succeed");
                                //        //            xtraTabControl1.SelectedTabPageIndex = 1;
                                //        //        }

                                //        //        isSucced = true;
                                //        //        QueryClick();

                                //        //        // Kirim email jika diaktifkan
                                //        //        if (IS_SEND_MAIL == "Y")
                                //        //        {
                                //        //            fnSentMail(RENTAL_DATE, RENTAL_TIME, EMP_NAME, USER_NAME, EMP_ID, DEPT_CODE, DEPT_NAME, START_DATE, END_DATE, DESTINATION, PURPOSES, PASSENGER);
                                //        //        }
                                //        //    }
                                //        //    else
                                //        //    {
                                //        //        MessageBoxW("Failed Saved");
                                //        //    }
                                //        //}
                                //        //else
                                //        //{
                                //        //    MessageBoxW(dtData.Rows[0][0].ToString());
                                //        //}
                                //        #endregion
                                //    }
                                //    else
                                //    {
                                //        MessageBoxW("Please Call IT");
                                //    }
                                //}
                                //else
                                //{
                                //    MessageBoxW("T1");
                                //    //if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                                //    //{
                                //    //    if (status == "W")
                                //    //    {
                                //    //        MessageBoxW("A minimum of 3 passengers is required. You will be placed on the Waiting List");
                                //    //        xtraTabControl1.SelectedTabPageIndex = 0;
                                //    //    }
                                //    //    else if (status == "C")
                                //    //    {
                                //    //        MessageBoxW("The standby car stock for rental is fully booked!");
                                //    //        xtraTabControl1.SelectedTabPageIndex = 1;
                                //    //    }
                                //    //    else
                                //    //    {
                                //    //        MessageBoxW("Register Succeed");
                                //    //        xtraTabControl1.SelectedTabPageIndex = 1;
                                //    //    }

                                //    //    isSucced = true;
                                //    //    QueryClick();

                                //    //    // Kirim email jika diaktifkan
                                //    //    if (IS_SEND_MAIL == "Y")
                                //    //    {
                                //    //        fnSentMail(RENTAL_DATE, RENTAL_TIME, EMP_NAME, USER_NAME, EMP_ID, DEPT_CODE, DEPT_NAME, START_DATE, END_DATE, DESTINATION, PURPOSES, PASSENGER);
                                //    //    }
                                //    //}
                                //    //else
                                //    //{
                                //    //    MessageBoxW("Failed Saved");
                                //    //}
                                //}
                                #endregion
                            }
                            else
                            {
                                MessageBoxW("Departure has expired");
                            }

                            #region [VALIDATION]

                            //if (departureDay.Contains("SATURDAY")) //SABTU
                            //{
                            //    DateTime fridayBeforeDeparture = departureDt;

                            //    // Tentukan hari Jumat sebelum keberangkatan
                            //    while (fridayBeforeDeparture.DayOfWeek != DayOfWeek.Friday)
                            //    {
                            //        fridayBeforeDeparture = fridayBeforeDeparture.AddDays(-1);
                            //    }

                            //    // Batas akhir registrasi: Jumat sebelum keberangkatan, pukul 12:59 PM
                            //    DateTime latestAllowedReg = fridayBeforeDeparture.Date.AddHours(8).AddMinutes(59);

                            //    // Validasi registrasi
                            //    if (regDt <= latestAllowedReg)
                            //    {
                            //        //MessageBoxW("Registration is valid.");
                            //        if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                            //        {
                            //            MessageBoxW("Save Succeed");
                            //            isSucced = true;
                            //            QueryClick();

                            //            // Kirim email jika diaktifkan
                            //            if (IS_SEND_MAIL == "Y")
                            //            {
                            //                fnSentMail(RENTAL_DATE, RENTAL_TIME, EMP_NAME, USER_NAME, EMP_ID, DEPT_CODE, DEPT_NAME, START_DATE, END_DATE, DESTINATION, PURPOSES, PASSENGER);
                            //            }
                            //        }
                            //        else
                            //        {
                            //            MessageBoxW("Failed Saved");
                            //        }
                            //    }
                            //    else if (regDt < departureDt && departureDt < DateTime.Now.Date.AddHours(8).AddMinutes(59)) //TEST NEW RULE
                            //    {
                            //        //MessageBoxW("Valid");
                            //        if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                            //        {
                            //            MessageBoxW("Save Succeed");
                            //            isSucced = true;
                            //            QueryClick();

                            //            // Kirim email jika diaktifkan
                            //            if (IS_SEND_MAIL == "Y")
                            //            {
                            //                fnSentMail(RENTAL_DATE, RENTAL_TIME, EMP_NAME, USER_NAME, EMP_ID, DEPT_CODE, DEPT_NAME, START_DATE, END_DATE, DESTINATION, PURPOSES, PASSENGER);
                            //            }
                            //        }
                            //        else
                            //        {
                            //            MessageBoxW("Failed Saved");
                            //        }
                            //    }
                            //    else
                            //    {
                            //        MessageBoxW("Registration is not valid. It must be done before Friday at 9 AM.");
                            //    }
                            //}
                            //else if (departureDay.Contains("SUNDAY")) //MINGGU
                            //{
                            //    DateTime fridayBeforeDeparture = departureDt;

                            //    // Tentukan hari Jumat sebelum keberangkatan
                            //    while (fridayBeforeDeparture.DayOfWeek != DayOfWeek.Friday)
                            //    {
                            //        fridayBeforeDeparture = fridayBeforeDeparture.AddDays(-2);
                            //    }

                            //    // Batas akhir registrasi: Jumat
                            //    DateTime latestAllowedReg = fridayBeforeDeparture.Date.AddHours(23).AddMinutes(59);

                            //    // Validasi registrasi
                            //    if (regDt <= latestAllowedReg)
                            //    {
                            //        //MessageBoxW("Registration is valid.");
                            //        if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                            //        {
                            //            MessageBoxW("Save Succeed");
                            //            isSucced = true;
                            //            QueryClick();

                            //            // Kirim email jika diaktifkan
                            //            if (IS_SEND_MAIL == "Y")
                            //            {
                            //                fnSentMail(RENTAL_DATE, RENTAL_TIME, EMP_NAME, USER_NAME, EMP_ID, DEPT_CODE, DEPT_NAME, START_DATE, END_DATE, DESTINATION, PURPOSES, PASSENGER);
                            //            }
                            //        }
                            //        else
                            //        {
                            //            MessageBoxW("Failed Saved");
                            //        }
                            //    }
                            //    else
                            //    {
                            //        MessageBoxW("Registration is not valid. The last registration is on Friday");
                            //    }
                            //}
                            //else if (departureDay.Contains("MONDAY")) //SENIN
                            //{
                            //    if (departureDt.Hour < 9)
                            //    {
                            //        DateTime fridayBeforeDeparture = departureDt;

                            //        // Tentukan hari Jumat sebelum keberangkatan
                            //        while (fridayBeforeDeparture.DayOfWeek != DayOfWeek.Friday)
                            //        {
                            //            fridayBeforeDeparture = fridayBeforeDeparture.AddDays(-3);
                            //        }

                            //        // Batas akhir registrasi: Jumat
                            //        DateTime latestAllowedReg = fridayBeforeDeparture.Date.AddHours(23).AddMinutes(59);

                            //        // Validasi registrasi
                            //        if (regDt <= latestAllowedReg)
                            //        {
                            //            //MessageBoxW("Registration is valid.");
                            //            if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                            //            {
                            //                MessageBoxW("Save Succeed");
                            //                isSucced = true;
                            //                QueryClick();

                            //                // Kirim email jika diaktifkan
                            //                if (IS_SEND_MAIL == "Y")
                            //                {
                            //                    fnSentMail(RENTAL_DATE, RENTAL_TIME, EMP_NAME, USER_NAME, EMP_ID, DEPT_CODE, DEPT_NAME, START_DATE, END_DATE, DESTINATION, PURPOSES, PASSENGER);
                            //                }
                            //            }
                            //            else
                            //            {
                            //                MessageBoxW("Failed Saved");
                            //            }
                            //        }
                            //        else
                            //        {
                            //            MessageBoxW("Registration is not valid. The last registration is on Friday");
                            //        }
                            //    }
                            //    else
                            //    {
                            //        //if (regDt <= departureDt.Date.AddHours(9))
                            //        if (regDt <= DateTime.Now.Date.AddHours(12).AddMinutes(59)) //DI GANTI JIKA KEBERANGKATAN H (SENIN), REGISTRASI MAKS JAM 1
                            //        {
                            //            //MessageBoxW("Registration is valid.");
                            //            if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                            //            {
                            //                MessageBoxW("Save Succeed");
                            //                isSucced = true;
                            //                QueryClick();

                            //                // Kirim email jika diaktifkan
                            //                if (IS_SEND_MAIL == "Y")
                            //                {
                            //                    fnSentMail(RENTAL_DATE, RENTAL_TIME, EMP_NAME, USER_NAME, EMP_ID, DEPT_CODE, DEPT_NAME, START_DATE, END_DATE, DESTINATION, PURPOSES, PASSENGER);
                            //                }
                            //            }
                            //            else
                            //            {
                            //                MessageBoxW("Failed Saved");
                            //            }
                            //        }
                            //        else
                            //        {
                            //            MessageBoxW("Registration is not valid. It must be done before 1 PM");
                            //        }
                            //    }
                            //}
                            //else //SELASA - JUMAT
                            //{
                            //    DateTime dayBeforeDeparture = departureDt.AddDays(-1);
                            //    DateTime latestRegTime = dayBeforeDeparture.Date.AddHours(13);
                            //    DateTime maxDt = DateTime.Now.Date.AddHours(12).AddMinutes(59);

                            //    if (departureDt.Hour < 9) //REGIS SEBELUM JAM 9
                            //    {
                            //        // Cek jika registrasi dilakukan sebelum jam 1 siang pada H-1
                            //        //if (regDt < latestRegTime)
                            //        if (regDt < maxDt) //DI GANTI JADI MAX HARI H, SEBELUM JAM 1 PM
                            //        {
                            //            //MessageBoxW("Registration is valid.");
                            //            if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                            //            {
                            //                MessageBoxW("Save Succeed");
                            //                isSucced = true;
                            //                QueryClick();

                            //                // Kirim email jika diaktifkan
                            //                if (IS_SEND_MAIL == "Y")
                            //                {
                            //                    fnSentMail(RENTAL_DATE, RENTAL_TIME, EMP_NAME, USER_NAME, EMP_ID, DEPT_CODE, DEPT_NAME, START_DATE, END_DATE, DESTINATION, PURPOSES, PASSENGER);
                            //                }
                            //            }
                            //            else
                            //            {
                            //                MessageBoxW("Failed Saved");
                            //            }
                            //        }
                            //        else
                            //        {
                            //            MessageBoxW("Registration is not valid. It must be done before 1 PM on the previous day");
                            //        }
                            //    }
                            //    else //REGIS SETELAH JAM 9
                            //    {
                            //        //if (regDt <= departureDt.Date.AddHours(9))
                            //        if (regDt <= DateTime.Now.Date.AddHours(12).AddMinutes(59)) //DI GANTI JIKA KEBERANGKATAN H (SELASA - JUMAT), REGISTRASI MAKS JAM 1
                            //        {
                            //            //MessageBoxW("Registration is valid.");
                            //            if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                            //            {
                            //                MessageBoxW("Save Succeed");
                            //                isSucced = true;
                            //                QueryClick();

                            //                // Kirim email jika diaktifkan
                            //                if (IS_SEND_MAIL == "Y")
                            //                {
                            //                    fnSentMail(RENTAL_DATE, RENTAL_TIME, EMP_NAME, USER_NAME, EMP_ID, DEPT_CODE, DEPT_NAME, START_DATE, END_DATE, DESTINATION, PURPOSES, PASSENGER);
                            //                }
                            //            }
                            //            else
                            //            {
                            //                MessageBoxW("Failed Saved");
                            //            }
                            //        }
                            //        else
                            //        {
                            //            MessageBoxW("Registration is not valid. It must be done before 1 PM");
                            //        }
                            //    }
                            //}

                            #endregion

                        }
                        else
                        {
                            MessageBoxW("Please provide the purposes and the correct employee id");
                        }
                    }
                    else
                    {
                        MessageBoxW("Failed Saved");
                    }

                    pbProgressHide();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnSave " + ex.Message);
                pbProgressHide();
            }
            //finally
            //{
            //    pbProgressHide();
            //}

            return isSucced;
        }

        public bool fnEdit(string RENT_ID, string EMPID, string DEPT_CD, string DEPT_NM, string START_TIME, string END_TIME, string DESTINATION, string PURPOSE, string MEMBERS, string MEMBER_NAME, 
                           string EMP_NAME, string RENT_DATE, string RENT_TIME, string MEMO, string IS_SEND_MAIL, string USER_NM)
        {
            bool isSucced = false;

            try
            {
                if (this.SetYesNoMessageBox("Are you sure?", "Save Data", IconType.Warning) == DialogResult.Yes)
                {
                    pbProgressShow();

                    SP_GMES0413 cProc = new SP_GMES0413("S");
                    DataTable dtData = null;
                    string status = MEMBERS.pxToInt() < 3 ? "W" : "R"; //STATUS WAITING LIST JIKA KURANG DARI 3 ORANG
                    int cekCarStock = fnCheckCarStock("CHECK_CAR_STOCK", START_TIME);
                    if (cekCarStock == 0 && status == "R")
                    {
                        status = "C";
                    }

                    dtData = cProc.SetParamDataInsert(dtData,
                                                      "EDIT", //ACTION
                                                      RENT_ID, //RENTAL_DATE
                                                      RENT_DATE, //RENTAL_TIME --> PINJAM DULU 100
                                                      "GA", //DIV
                                                      "3110", //PLANT
                                                      RENT_TIME, //USER ID --> PINJAM DULU 100
                                                      MEMO, // USER NAME --> PINJAM DULU BUAT MEMO
                                                      EMPID,
                                                      DEPT_CD,
                                                      DEPT_NM,
                                                      "",
                                                      PURPOSE,     //ACTIVITIY_CD
                                                      "",
                                                      DESTINATION,  //PLACE_DESC
                                                      "",
                                                      MEMBERS, // MEMBER OF EMPLOYEE
                                                      MEMBER_NAME,    //USE_DESC
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      START_TIME,
                                                      END_TIME,
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      status, //RENTAL_STATUS
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      SessionInfo.UserID,   //UPDATER
                                                      DateTime.Now.ToString("yyyyMMdd HHmmss"), //UPDATE_DT
                                                      Dns.GetHostName() + "|" + getIpAddress() + "|" + GetMacAddress() //UPDATE_PC
                                                      );

                    if (dtData != null)
                    {
                        if (PURPOSE != "" && EMP_NAME != "")
                        {
                            DateTime departureDt = (START_TIME != "" ? DateTime.ParseExact(START_TIME, "yyyyMMdd HHmm", CultureInfo.InvariantCulture) : DateTime.ParseExact("19990101", "yyyyMMdd", CultureInfo.InvariantCulture));
                            string departureDay = departureDt.DayOfWeek.ToString().Trim().ToUpper();
                            DateTime regDt = DateTime.Now;
                            string regDay = regDt.DayOfWeek.ToString().Trim().ToUpper();

                            //VARIABEL VALIDASI BARU
                            int regDayNum = ConvertDayToNumber(regDt); //HARI PADA SAAT REGISTRASI
                            int depDayNum = ConvertDayToNumber(departureDt); //HARI KEBERANGKATAN
                            TimeSpan regTime = TimeSpan.ParseExact(regDt.ToString("HHmm"), "hhmm", CultureInfo.InvariantCulture); //WAKTU REGISTRASI

                            #region [CURRENT VALIDATION]
                            if (departureDt > regDt)
                            {
                                //VALIDASI KEBERANGKATAN SABTU
                                if (depDayNum == 7) //JIKA PEMESANAN UNTUK HARI SABTU (7)
                                {
                                    if (regDayNum < 6) //JIKA REGISTRASI DILAKUKAN SEBELUM HARI JUMAT (6), KAMIS ATAU SEBELUMNYA (< 6)
                                    {
                                        MessageBoxW("Pemesanan untuk hari Sabtu hanya bisa dilakukan mulai Jumat pukul 00:01 AM hingga Sabtu pukul 08:59 AM.", IconType.Error);
                                        pbProgressHide();
                                        return isSucced = false;
                                    }
                                    else if (regDayNum == 6) //JIKA REGISTRASI DILAKUKAN PADA HARI JUMAT (6)
                                    {
                                        if (regTime < TimeSpan.ParseExact("0001", "hhmm", CultureInfo.InvariantCulture)) //JIKA REGISTRASI SEBELUM PUKUL 00:01 AM
                                        {
                                            MessageBoxW("Pemesanan untuk hari Sabtu hanya bisa dilakukan mulai Jumat pukul 00:01 AM hingga Sabtu pukul 08:59 AM.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }
                                    }
                                    else if (regDayNum == 7) //JIKA REGISTRASI DI LAKUKAN PADA HARI SABTU (7) (BUKAN DI MINGGU DEPAN)
                                    {
                                        if ((departureDt - regDt).TotalDays > 6)
                                        {
                                            MessageBoxW("Pemesanan untuk hari Sabtu hanya bisa dilakukan mulai Jumat sebelum hari keberangkatan pukul 00:01 AM hingga hari keberangkatan pukul 08:59 AM.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }

                                        if (regTime >= TimeSpan.ParseExact("0900", "hhmm", CultureInfo.InvariantCulture)) //JIKA REGISTRASI SETELAH PUKUL 09:00 AM
                                        {
                                            MessageBoxW("Pemesanan untuk hari Sabtu harus sebelum jam 09:00.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }
                                    }
                                }

                                //COBA VALIDASI KEBERANGKATAN MINGGU
                                if (depDayNum == 1) //JIKA PEMESANAN UNTUK HARI MINGGU (1)
                                {
                                    if (regDayNum < 6) //JIKA REGISTRASI DILAKUKAN SEBELUM HARI JUMAT (6), KAMIS ATAU SEBELUMNYA (< 6)
                                    {
                                        MessageBoxW("Pemesanan untuk hari Minggu hanya bisa dilakukan mulai Jumat pukul 00:01 AM hingga Sabtu pukul 08:59 AM.", IconType.Error);
                                        pbProgressHide();
                                        return isSucced = false;
                                    }
                                    else if (regDayNum == 6) //JIKA REGISTRASI DILAKUKAN PADA HARI JUMAT (6)
                                    {
                                        if (regTime < TimeSpan.ParseExact("0001", "hhmm", CultureInfo.InvariantCulture)) //JIKA REGISTRASI SEBELUM PUKUL 00:01 AM
                                        {
                                            MessageBoxW("Pemesanan untuk hari Sabtu hanya bisa dilakukan mulai Jumat pukul 00:01 AM hingga Sabtu pukul 08:59 AM.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }
                                    }
                                    else if (regDayNum == 7) //JIKA REGISTRASI DI LAKUKAN PADA HARI SABTU (7)
                                    {
                                        if ((departureDt - regDt).TotalDays > 6)
                                        {
                                            MessageBoxW("Pemesanan untuk hari Minggu hanya bisa dilakukan mulai Jumat sebelum hari keberangkatan pukul 00:01 AM hingga Sabtu sebelum hari keberangkatan pukul 08:59 AM.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }

                                        if (regTime >= TimeSpan.ParseExact("0900", "hhmm", CultureInfo.InvariantCulture)) //JIKA REGISTRASI SETELAH PUKUL 09:00 AM
                                        {
                                            MessageBoxW("Pemesanan untuk hari Minggu harus sebelum jam 09:00 pada hari Sabtu.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }
                                    }
                                }

                                //COBA VALIDASI KEBERANGKATAN SENIN
                                if (depDayNum == 2) //JIKA PEMESANAN UNTUK HARI SENIN (2)
                                {
                                    if (regDayNum < 6) //JIKA REGISTRASI DILAKUKAN SEBELUM HARI JUMAT (6), KAMIS ATAU SEBELUMNYA (< 6)
                                    {
                                        MessageBoxW("Pemesanan untuk hari Senin hanya bisa dilakukan mulai Jumat pukul 00:01 AM hingga Sabtu pukul 08:59 AM.", IconType.Error);
                                        pbProgressHide();
                                        return isSucced = false;
                                    }
                                    else if (regDayNum == 6) //JIKA REGISTRASI DILAKUKAN PADA HARI JUMAT (6)
                                    {
                                        if (regTime < TimeSpan.ParseExact("0001", "hhmm", CultureInfo.InvariantCulture)) //JIKA REGISTRASI SEBELUM PUKUL 00:01 AM
                                        {
                                            MessageBoxW("Pemesanan untuk hari Sabtu hanya bisa dilakukan mulai Jumat pukul 00:01 AM hingga Sabtu pukul 08:59 AM.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        };
                                    }
                                    else if (regDayNum == 7) //JIKA REGISTRASI DI LAKUKAN PADA HARI SABTU (7)
                                    {
                                        if ((departureDt - regDt).TotalDays > 6)
                                        {
                                            MessageBoxW("Pemesanan untuk hari Senin hanya bisa dilakukan mulai Jumat sebelum hari keberangkatan pukul 00:01 AM hingga Sabtu sebelum hari keberangkatan pukul 08:59 AM.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }

                                        if (regTime >= TimeSpan.ParseExact("0900", "hhmm", CultureInfo.InvariantCulture)) //JIKA REGISTRASI SETELAH PUKUL 09:00 AM
                                        {
                                            MessageBoxW("Pemesanan untuk hari Senin harus sebelum jam 09:00 pada hari Sabtu.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }
                                    }
                                }

                                //COBA VALIDASI KEBERANGAKATAN SELASA, RABU, KAMIS, JUMAT.
                                if (depDayNum >= 3 && depDayNum <= 6) //JIKA PEMESANAN UNTUK HARI SELASA (3), RABU (4), KAMIS (5), JUMAT (6)
                                {
                                    if (regDayNum == (depDayNum - 1)) //JIKA REGISTRASI DILAKUKAN SEHARI SEBELUMNYA
                                    {
                                        if (regTime < TimeSpan.ParseExact("0001", "hhmm", CultureInfo.InvariantCulture)) //JIKA REGISTRASI SEBELUM PUKUL 00:01 AM
                                        {
                                            MessageBoxW("Registrasi hanya bisa dilakukan mulai pukul 00:01 sehari sebelum keberangkatan.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }
                                    }
                                    else if (regDayNum == depDayNum) // JIKA REGISTRASI DILAKUKAN DI HARI KEBERANGKATAN
                                    {
                                        if (regTime > TimeSpan.ParseExact("1400", "hhmm", CultureInfo.InvariantCulture)) //JIKA REGISTRASI MELEWATI PUKU 2:00 PM 
                                        {
                                            MessageBoxW("Registrasi tidak bisa dilakukan setelah pukul 2:00 PM di hari keberangkatan.", IconType.Error);
                                            pbProgressHide();
                                            return isSucced = false;
                                        }
                                    }
                                    //int cek = (depDayNum - 1);
                                    //if (regDayNum < (depDayNum - 1)) 
                                    else if (regDayNum != (depDayNum - 1))//JIKA REGISTRASI DILAKUKAN LEBIH DARI SEHARI SEBELUMNYA
                                    {
                                        MessageBoxW("Registrasi hanya bisa dilakukan sehari sebelum keberangkatan mulai pukul 00:01 hingga hari keberangkatan pukul 2:30 PM.", IconType.Error);
                                        pbProgressHide();
                                        return isSucced = false;
                                    }
                                }


                                if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                                {
                                    //MessageBoxW("Save Succeed");
                                    if (status == "W")
                                    {
                                        MessageBoxW("A minimum of 3 passengers is required. You will be placed on the Waiting List");
                                        xtraTabControl1.SelectedTabPageIndex = 0;
                                    }
                                    else if (status == "C")
                                    {
                                        MessageBoxW("The standby car stock for rental is fully booked!");
                                        xtraTabControl1.SelectedTabPageIndex = 1;
                                    }
                                    else
                                    {
                                        MessageBoxW("Register Succeed");
                                        xtraTabControl1.SelectedTabPageIndex = 1;
                                    }

                                    isSucced = true;
                                    //REFRESH PAGE
                                    if (xtraTabControl1.SelectedTabPageIndex == 0)
                                    {
                                        dtFrom.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
                                        dtTo.EditValue = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                                        fnSearchW("GET_WAITING_LIST", dtFrom.DateTime.ToString("yyyyMMdd"), dtTo.DateTime.ToString("yyyyMMdd"), "");
                                    }
                                    else
                                    {
                                        dtFrom.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
                                        dtTo.EditValue = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                                        fnSearch("GET_DATA", dtEFrom.DateTime.ToString("yyyyMMdd"), dtETo.DateTime.ToString("yyyyMMdd"), cboStatus.EditValue.ToString());
                                    }
                                    fnGetCarStockInfo("CAR_STOCK_INFO", DateTime.Now.ToString("yyyyMMdd"));

                                    // Kirim email jika diaktifkan dan passengers > 2
                                    if (IS_SEND_MAIL == "Y" && Convert.ToInt16(MEMBERS) > 2)
                                    {
                                        fnSentMail(RENT_DATE, RENT_TIME, EMP_NAME, USER_NM, EMPID, DEPT_CD, DEPT_NM, START_TIME, END_TIME, DESTINATION, PURPOSE, MEMBER_NAME);
                                    }
                                }
                                else
                                {
                                    MessageBoxW("Failed Saved");
                                }
                            }
                            else
                            {
                                MessageBoxW("Departure has expired");
                            }

                            #endregion

                            #region [FAILED VALIDATION]

                            //if (departureDay.Contains("SATURDAY")) //SABTU
                            //{
                            //    DateTime fridayBeforeDeparture = departureDt;

                            //    // Tentukan hari Jumat sebelum keberangkatan
                            //    while (fridayBeforeDeparture.DayOfWeek != DayOfWeek.Friday)
                            //    {
                            //        fridayBeforeDeparture = fridayBeforeDeparture.AddDays(-1);
                            //    }

                            //    // Batas akhir registrasi: Jumat sebelum keberangkatan, pukul 12:59 PM
                            //    DateTime latestAllowedReg = fridayBeforeDeparture.Date.AddHours(12).AddMinutes(59);

                            //    // Validasi registrasi
                            //    if (regDt <= latestAllowedReg)
                            //    {
                            //        //MessageBoxW("Registration is valid.");
                            //        if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                            //        {
                            //            MessageBoxW("Save Succeed");
                            //            isSucced = true;
                            //            QueryClick();
                            //        }
                            //        else
                            //        {
                            //            MessageBoxW("Failed Saved");
                            //        }
                            //    }
                            //    else
                            //    {
                            //        MessageBoxW("Registration is not valid. It must be done before Friday at 1 PM.");
                            //    }
                            //}
                            //else if (departureDay.Contains("SUNDAY")) //MINGGU
                            //{
                            //    DateTime fridayBeforeDeparture = departureDt;

                            //    // Tentukan hari Jumat sebelum keberangkatan
                            //    while (fridayBeforeDeparture.DayOfWeek != DayOfWeek.Friday)
                            //    {
                            //        fridayBeforeDeparture = fridayBeforeDeparture.AddDays(-2);
                            //    }

                            //    // Batas akhir registrasi: Jumat
                            //    DateTime latestAllowedReg = fridayBeforeDeparture.Date.AddHours(23).AddMinutes(59);

                            //    // Validasi registrasi
                            //    if (regDt <= latestAllowedReg)
                            //    {
                            //        //MessageBoxW("Registration is valid.");
                            //        if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                            //        {
                            //            MessageBoxW("Save Succeed");
                            //            isSucced = true;
                            //            QueryClick();
                            //        }
                            //        else
                            //        {
                            //            MessageBoxW("Failed Saved");
                            //        }
                            //    }
                            //    else
                            //    {
                            //        MessageBoxW("Registration is not valid. The last registration is on Friday");
                            //    }
                            //}
                            //else if (departureDay.Contains("MONDAY")) //SENIN
                            //{
                            //    if (departureDt.Hour < 9)
                            //    {
                            //        DateTime fridayBeforeDeparture = departureDt;

                            //        // Tentukan hari Jumat sebelum keberangkatan
                            //        while (fridayBeforeDeparture.DayOfWeek != DayOfWeek.Friday)
                            //        {
                            //            fridayBeforeDeparture = fridayBeforeDeparture.AddDays(-3);
                            //        }

                            //        // Batas akhir registrasi: Jumat
                            //        DateTime latestAllowedReg = fridayBeforeDeparture.Date.AddHours(23).AddMinutes(59);

                            //        // Validasi registrasi
                            //        if (regDt <= latestAllowedReg)
                            //        {
                            //            //MessageBoxW("Registration is valid.");
                            //            if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                            //            {
                            //                MessageBoxW("Save Succeed");
                            //                isSucced = true;
                            //                QueryClick();
                            //            }
                            //            else
                            //            {
                            //                MessageBoxW("Failed Saved");
                            //            }
                            //        }
                            //        else
                            //        {
                            //            MessageBoxW("Registration is not valid. The last registration is on Friday");
                            //        }
                            //    }
                            //    else
                            //    {
                            //        if (regDt <= departureDt.Date.AddHours(9))
                            //        {
                            //            //MessageBoxW("Registration is valid.");
                            //            if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                            //            {
                            //                MessageBoxW("Save Succeed");
                            //                isSucced = true;
                            //                QueryClick();
                            //            }
                            //            else
                            //            {
                            //                MessageBoxW("Failed Saved");
                            //            }
                            //        }
                            //        else
                            //        {
                            //            MessageBoxW("Registration is not valid. It must be done before 9 AM");
                            //        }
                            //    }
                            //}
                            //else //SELASA - JUMAT
                            //{
                            //    DateTime dayBeforeDeparture = departureDt.AddDays(-1);
                            //    DateTime latestRegTime = dayBeforeDeparture.Date.AddHours(13);

                            //    if (departureDt.Hour < 9)
                            //    {
                            //        // Cek jika registrasi dilakukan sebelum jam 1 siang pada H-1
                            //        if (regDt < latestRegTime)
                            //        {
                            //            //MessageBoxW("Registration is valid.");
                            //            if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                            //            {
                            //                MessageBoxW("Save Succeed");
                            //                isSucced = true;
                            //                QueryClick();
                            //            }
                            //            else
                            //            {
                            //                MessageBoxW("Failed Saved");
                            //            }
                            //        }
                            //        else
                            //        {
                            //            MessageBoxW("Registration is not valid. It must be done before 1 PM on the previous day");
                            //        }
                            //    }
                            //    else
                            //    {
                            //        if (regDt <= departureDt.Date.AddHours(9))
                            //        {
                            //            //MessageBoxW("Registration is valid.");
                            //            if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                            //            {
                            //                MessageBoxW("Save Succeed");
                            //                isSucced = true;
                            //                QueryClick();
                            //            }
                            //            else
                            //            {
                            //                MessageBoxW("Failed Saved");
                            //            }
                            //        }
                            //        else
                            //        {
                            //            MessageBoxW("Registration is not valid. It must be done before 9 AM");
                            //        }
                            //    }
                            //}

                            #endregion

                        }
                        else
                        {
                            MessageBoxW("Please provide the purposes and the correct employee id");
                        }
                    }

                    pbProgressHide();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnEdit " + ex.Message);
            }
            //finally
            //{
            //    pbProgressHide();
            //}

            return isSucced;
        }

        private void fnUpdate(string RENT_ID, string DRIVER_ID, string DETAIL_CAR)
        {
            try
            {
                if (this.SetYesNoMessageBox("Are you sure?", "Save Data", IconType.Warning) == DialogResult.Yes)
                {
                    SP_GMES0413 cProc = new SP_GMES0413("S");
                    DataTable dtData = null;

                    dtData = cProc.SetParamDataInsert(dtData,
                                                      "UPDATE", //ACTION
                                                      RENT_ID, // RENTAL_DATE ==> PINJEM VARIABELNYA UNTUK PARAMETER RENT_NO
                                                      DRIVER_ID, //RENT_TIME ==> PINJEM VARIABELNYA UNTUK PARAMETER DRIVER
                                                      DETAIL_CAR, //RENT_DIV ==> PINJEM VARIABELNYA UNTUK PARAMETER CAR
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "", //ACTIVITIY_CD
                                                      "",
                                                      "", //PLACE_DESC
                                                      "",
                                                      "",
                                                      "", //USE_DESC
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "F", //CONFIRM/FINISH
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      SessionInfo.UserID,   //UPDATER
                                                      DateTime.Now.ToString("yyyyMMdd HHmmss"), //UPDATE_DT
                                                      Dns.GetHostName() + "|" + getIpAddress() + "|" + GetMacAddress() //UPDATE_PC
                                                      );

                    if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                    {
                        //MessageBoxW("Save Succeed");
                        //QueryClick();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnUpdate " + ex.Message);
            }
        }

        private void fnDesign()
        {
            try
            {
                //gvwMain.ColumnPanelRowHeight = 40;
                gvwMain.OptionsView.ShowColumnHeaders = false;
                gvwMain.BandPanelRowHeight = 40;
                gvwMain.RowHeight = 30;
                gvwMain.OptionsView.ShowFooter = true;
                gvwMain.Appearance.FooterPanel.Font = new Font("Calibri", 12, FontStyle.Bold);
                gvwMain.Columns["HOUR_DURATION"].Summary.Clear();
                gvwMain.Columns["HOUR_DURATION"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "HOUR_DURATION", "Total: {0:N0}");
                //gvwMain.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

                for (int i = 0; i < gvwMain.Columns.Count; i++)
                {
                    if (i < 15)
                    {
                        gvwMain.Bands[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gvwMain.Bands[i].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
                        gvwMain.Bands[i].AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    }

                    gvwMain.Columns[i].OptionsColumn.AllowEdit = false;
                    gvwMain.Columns[i].AppearanceCell.Font = new Font("Calibri", 12, FontStyle.Regular);
                    gvwMain.Columns[i].Width = gvwMain.Columns[i].GetBestWidth();

                    if (i == 0 || i == 1 || i == 2 || i == 10 || i == 11 || i == 13 || i == 15)
                    {
                        gvwMain.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    }
                    else if (i == 12 || i == 17)
                    {
                        gvwMain.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    }
                }

                gvwMain.Bands[5].Children[0].Width = 80;
                gvwMain.Bands[5].Children[1].Width = 80;
                gvwMain.Bands[6].Children[0].Width = 80;
                gvwMain.Bands[6].Children[1].Width = 80;
                gvwMain.Bands[6].Children[2].Width = 80;
                gvwMain.Columns["SEQ"].Width = 80;
            }
            catch (Exception ex)
            {
                MessageBoxW("fnDesign " + ex.Message);
            }
        }

        private void fnDesignW()
        {
            try
            {
                //gvwWaitingList.ColumnPanelRowHeight = 40;
                gvwWaitingList.OptionsView.ShowColumnHeaders = false;
                gvwWaitingList.BandPanelRowHeight = 40;
                gvwWaitingList.RowHeight = 30;
                gvwWaitingList.OptionsView.ShowFooter = true;
                gvwWaitingList.Appearance.FooterPanel.Font = new Font("Calibri", 12, FontStyle.Bold);
                gvwWaitingList.Columns["W_HOUR_DURATION"].Summary.Clear();
                gvwWaitingList.Columns["W_HOUR_DURATION"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "W_HOUR_DURATION", "Total: {0:N0}");
                //gvwWaitingList.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

                for (int i = 0; i < gvwWaitingList.Columns.Count; i++)
                {
                    if (i < 15)
                    {
                        gvwWaitingList.Bands[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gvwWaitingList.Bands[i].AppearanceHeader.Font = new Font("Calibri", 12, FontStyle.Bold);
                        gvwWaitingList.Bands[i].AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    }

                    gvwWaitingList.Columns[i].OptionsColumn.AllowEdit = false;
                    gvwWaitingList.Columns[i].AppearanceCell.Font = new Font("Calibri", 12, FontStyle.Regular);
                    gvwWaitingList.Columns[i].Width = gvwMain.Columns[i].GetBestWidth();

                    if (i == 0 || i == 1 || i == 2 || i == 10 || i == 11 || i == 13 || i == 15)
                    {
                        gvwWaitingList.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    }
                    else if (i == 12 || i == 17)
                    {
                        gvwWaitingList.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    }
                }

                gvwWaitingList.Bands[5].Children[0].Width = 80;
                gvwWaitingList.Bands[5].Children[1].Width = 80;
                gvwWaitingList.Bands[6].Children[0].Width = 80;
                gvwWaitingList.Bands[6].Children[1].Width = 80;
                gvwWaitingList.Bands[6].Children[2].Width = 80;
                gvwWaitingList.Columns["W_SEQ"].Width = 80;
            }
            catch (Exception ex)
            {
                MessageBoxW("fnDesignW " + ex.Message);
            }
        }

        private void fnPreview(string paramType, string paramId)
        {
            try
            {
                SP_GMES0413 cProc = new SP_GMES0413();
                DataTable dtData = null;

                dtData = cProc.SetParamData(dtData, paramType, paramId);
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];
                    if (dtData.Rows.Count > 0)
                    {
                        rptSIOK rpt = new rptSIOK();
                        rpt.BindData(dtData);

                        ReportPrintTool prntTool = new ReportPrintTool(rpt);
                        prntTool.ShowPreview();

                        Form prvForm = prntTool.PreviewForm;
                        if (prvForm != null)
                        {
                            prvForm.WindowState = FormWindowState.Maximized;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBoxW("fnPreview " + ex.Message);
            }
        }

        private void fnGetAdmin()
        {
            //try
            //{
            //    //GET ADMIN
            //    SP_GMES0413 cProc = new SP_GMES0413();
            //    DataTable dtAdmin = null;
            //    dtAdmin = cProc.SetParamData(dtAdmin, "GET_ADMIN");
            //    ResultSet rSet = CommonCallQuery(dtAdmin, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);
            //    dtAdmin = rSet.ResultDataSet.Tables[0];
            //    if (dtAdmin.Rows.Count > 0)
            //    {
            //        foreach (DataRow rw in dtAdmin.Rows)
            //        {
            //            string adm = rw[0].ToString();
            //            if (SessionInfo.UserID.ToUpper().Contains(adm.ToUpper()))
            //            {
            //                gvwMain.Columns["DRIVER_ID"].OptionsColumn.AllowEdit = true;
            //                gvwMain.Columns["DETAIL_CAR"].OptionsColumn.AllowEdit = true;
            //                //PreviewButton = true;
            //                return;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBoxW("fnGetAdmin " + ex.Message);
            //}
        }

        public string getNik(string userId)
        {
            try
            {
                SP_GMES0413 cProc = new SP_GMES0413();
                DataTable dtData = null;

                dtData = cProc.SetParamData(dtData, "GET_NIK", userId);
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    DataTable dt = rs.ResultDataSet.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        string nik = dt.Rows[0][0].ToString();
                        return nik;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string getName(string nik)
        {
            try
            {
                SP_GMES0413 cProc = new SP_GMES0413();
                DataTable dtData = null;

                dtData = cProc.SetParamData(dtData, "GET_NAME", nik);
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    DataTable dt = rs.ResultDataSet.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        string name = dt.Rows[0][0].ToString();
                        return name;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string getDept(string nik)
        {
            try
            {
                SP_GMES0413 cProc = new SP_GMES0413();
                DataTable dtData = null;

                dtData = cProc.SetParamData(dtData, "GET_DEPT", nik);
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    DataTable dt = rs.ResultDataSet.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        string dept = dt.Rows[0][0].ToString();
                        return dept;
                    }
                    else
                    {
                        return "010020";
                    }
                }
                else
                {
                    return "010020";
                }
            }
            catch (Exception)
            {
                return "010020";
            }
        }

        private string getIpAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBoxW("getIpAddress " + ex.Message);
                return null;
            }
        }

        private string GetMacAddress()
        {
            try
            {
                var macAddr =
                    (
                        from nic in NetworkInterface.GetAllNetworkInterfaces()
                        where nic.OperationalStatus == OperationalStatus.Up
                        select nic.GetPhysicalAddress().ToString()
                    ).FirstOrDefault();
                return macAddr;
            }
            catch (Exception ex)
            {
                MessageBoxW("GetMacAddress " + ex.Message);
                return null;
            }
        }

        private void fnSentMail(string RENTAL_DATE, string RENTAL_TIME, string EMP_NAME, string USER_NAME, string EMP_ID, string DEPT_CODE, string DEPT_NAME,
                                string START_DATE, string END_DATE, string DESTINATION, string PURPOSES, string PASSENGER)
        {
            try
            {
                string keperluan = (PURPOSES == "Y" ? "Official (Dinas)" : PURPOSES == "N" ? "Unofficial (Non-Dinas)" : "-");
                DateTime dtRent = DateTime.ParseExact(RENTAL_DATE, "yyyyMMdd", null);
                string rentDt = dtRent.ToString("yyyy-MM-dd");
                DateTime dtStart = DateTime.ParseExact(START_DATE, "yyyyMMdd HHmm", null);
                string startDt = dtStart.ToString("yyyy-MM-dd HH:mm");
                DateTime dtEnd = DateTime.ParseExact(END_DATE, "yyyyMMdd HHmm", null);
                string endDt = dtEnd.ToString("yyyy-MM-dd HH:mm");

                SP_GMES0413 cProc = new SP_GMES0413();
                DataTable dtData = null;

                dtData = cProc.SetParamData(dtData, "GET_MAIL", "");
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];
                    if (dtData.Rows.Count > 0)
                    {
 
                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress("gmes.automail@changshininc.com", "GMES.AUTOMAIL", System.Text.Encoding.UTF8);

                        #region [OLD]
                        //mail.To.Add("pga.variendra@changshininc.com");
                        //mail.CC.Add("pga.dian@changshininc.com");
                        //mail.CC.Add("youngwoo.kim@dskorea.com");
                        //mail.Bcc.Add("it.deny@changshininc.com");
                        #endregion

                        for (int i = 0; i < dtData.Rows.Count; i++)
                        {
                            if (dtData.Rows[i]["TYPE"].ToString() == "TO")
                            {
                                mail.To.Add(dtData.Rows[i]["EMAIL"].ToString());
                            }
                            else if (dtData.Rows[i]["TYPE"].ToString() == "CC")
                            {
                                mail.CC.Add(dtData.Rows[i]["EMAIL"].ToString());
                            }
                            else if (dtData.Rows[i]["TYPE"].ToString() == "BCC")
                            {
                                mail.Bcc.Add(dtData.Rows[i]["EMAIL"].ToString());
                            }
                        }

                        mail.Subject = "Official Car Request";
                        mail.Body = "<!DOCTYPE html> " +
                                    "<html lang=\"en\"> " +
                                    "<head> " +
                                    "</head> " +
                                    "<body> " +
                                    "<p>Dear GA Team,</p> " +
                                    "<p>This email was automatically forwarded by the system.</p> " +
                                    "<p><strong>Car Booking Details:</strong></p> " +
                                    "<table style=\"border-collapse: collapse;\"> " +
                                    "    <tr> " +
                                    "        <td style=\"padding-right: 10px; vertical-align: top;\">Name of Booker</td> " +
                                    "        <td>: " + EMP_NAME + "/" + DEPT_NAME + "</td> " +
                                    "    </tr> " +
                                    "    <tr> " +
                                    "        <td style=\"padding-right: 10px; vertical-align: top;\">Booking Date</td> " +
                                    "        <td>: " + rentDt + "</td> " +
                                    "    </tr> " +
                                    "    <tr> " +
                                    "        <td style=\"padding-right: 10px; vertical-align: top;\">Usage Date</td> " +
                                    "        <td>: " + startDt + " - " + endDt + "</td> " +
                                    "    </tr> " +
                                    "    <tr> " +
                                    "        <td style=\"padding-right: 10px; vertical-align: top;\">Destination</td> " +
                                    "        <td>: " + DESTINATION + "</td> " +
                                    "    </tr> " +
                                    "    <tr> " +
                                    "        <td style=\"padding-right: 10px; vertical-align: top;\">Purpose</td> " +
                                    "        <td>: " + keperluan + " </td> " +
                                    "    </tr> " +
                                    "    <tr> " +
                                    "        <td style=\"padding-right: 10px; vertical-align: top;\">User or Load</td> " +
                                    "        <td>: " + PASSENGER + "</td> " +
                                    "    </tr> " +
                                    "</table> " +
                                    "<p>Please process this promptly according to the applicable procedures. If you have any further questions, please " +
                                    "    contact the applicant directly.</p> " +
                                    "<p>Thank you,</p> " +
                                    "<p>GMES System</p> " +
                                    "</body> " +
                                    "</html>";

                        mail.IsBodyHtml = true;
                        mail.SubjectEncoding = System.Text.Encoding.UTF8;
                        mail.BodyEncoding = System.Text.Encoding.UTF8;

                        SmtpClient smtpServer = new SmtpClient("jjmail2.dskorea.com", 587);
                        smtpServer.UseDefaultCredentials = false;
                        smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpServer.Credentials = new System.Net.NetworkCredential("gmes.automail@dskorea.com", "csg1122!@");
                        smtpServer.EnableSsl = true;
                        System.Net.ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
                        smtpServer.Send(mail);



                    }
                }

            }
            catch (Exception ex)
            {
                MessageBoxW("fnSentMail " + ex.Message);
            }
        }

        private void gvwMain_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            //SaveButton = true;
        }

        private void gvwMain_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                GridView vw = sender as GridView;
                string driverIdValue = vw.GetRowCellValue(e.RowHandle, "DRIVER_ID").ToString();
                string carValue = vw.GetRowCellValue(e.RowHandle, "DETAIL_CAR").ToString();
                string sts = vw.GetRowCellValue(e.RowHandle, "STATUS").ToString();
                string startDate = vw.GetRowCellValue(e.RowHandle, "START_DATE").ToString();
                DateTime departure = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                if (e.Column.FieldName.Contains("DRIVER_ID"))
                {
                    if (driverIdValue.Length > 0 && sts == "F")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#EAFFDE");
                    }
                    else if (driverIdValue.Length == 0 && sts == "R")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#FFFDD0");
                    }
                }

                if (e.Column.FieldName.Contains("CONTACT"))
                {
                    if (sts == "F")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#EAFFDE");
                    }
                    else if (sts == "R")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#FFFDD0");
                    }
                }

                if (e.Column.FieldName.Contains("DETAIL_CAR"))
                {
                    if (carValue.Length > 0 && sts == "F")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#EAFFDE");
                    }
                    else if (carValue.Length == 0 && sts == "R")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#FFFDD0");
                    }
                }

                if (e.Column.FieldName.Contains("SERIAL_NO"))
                {
                    if (sts == "F")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#EAFFDE");
                    }
                    else if (sts == "R")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#FFFDD0");
                    }
                }

                if (e.Column.FieldName.Contains("COLOR"))
                {
                    if (sts == "F")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#EAFFDE");
                    }
                    else if (sts == "R")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#FFFDD0");
                    }
                }

                if (e.Column.FieldName.Contains("STATUS"))
                {
                    if (sts == "R")
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#FFFDD0");
                    }
                    else
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#EAFFDE");
                    }
                }

                if (departure < DateTime.Now && sts == "R")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#D1CFC8");
                }

                if (sts == "C")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#F47174");
                    e.Appearance.ForeColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("gvwMain_RowCellStyle " + ex.Message);
            }
        }

        private void cboStatus_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                QueryClick();
            }
            catch (Exception ex)
            {
                MessageBoxW("cboStatus_EditValueChanged " + ex.Message);
            }
        }

        private void dtETo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtETo.EditValue != null && dtEFrom.EditValue != null && cboStatus.EditValue != null)
                {
                    if (dtEFrom.DateTime > dtETo.DateTime)
                    {
                        dtEFrom.DateTime = dtETo.DateTime;
                    }

                    QueryClick();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("dtETo_EditValueChanged " + ex.Message);
            }
        }

        private void dtEFrom_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtETo.EditValue != null && dtEFrom.EditValue != null && cboStatus.EditValue != null)
                {
                    if (dtEFrom.DateTime > dtETo.DateTime)
                    {
                        dtETo.DateTime = dtEFrom.DateTime;
                    }

                    QueryClick();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("dtEFrom_EditValueChanged " + ex.Message);
            }
        }

        private void gvwMain_RowClick(object sender, RowClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 2)
                {
                    int rowFocused = gvwMain.FocusedRowHandle;
                    string col = gvwMain.Columns[0].FieldName;
                    string cekId = gvwMain.GetRowCellValue(rowFocused, col).ToString();
                    string userId = SessionInfo.UserID;

                    fnSearchById("GET_DATA_ID", cekId, userId);

                    QueryClick();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("gvwMain_RowClick " + ex.Message);
            }
        }

        private void fnSearchById(string paramType, string paramId, string paramUserId)
        {
            try
            {
                SP_GMES0413 cProc = new SP_GMES0413();
                DataTable dtData = null;
                
                dtData = cProc.SetParamData(dtData, paramType, paramId);
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];

                    if (dtData.Rows.Count > 0)
                    {
                        editRegister popEdit = new editRegister(dtData, dtItem, dtDest, paramUserId, dtNumber, this);
                        popEdit.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnSearchById " + ex.Message);
            }
        }

        private void chkAutoRf_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkAutoRf.CheckState == CheckState.Checked)
                {
                    tmrRefresh.Interval = Convert.ToInt16(txtInterval.Text) * 60 * 1000;
                    tmrRefresh.Enabled = true;
                    tmrRefresh.Start();
                }
                else
                {
                    tmrRefresh.Enabled = false;
                    tmrRefresh.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("chkAutoRf_CheckedChanged " + ex.Message);
            }
        }

        private void txtInterval_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string interval = txtInterval.Text != "0" ? txtInterval.Text : "0";

                if (interval != "0")
                {
                    chkAutoRf.CheckState = CheckState.Checked;
                }
                else
                {
                    chkAutoRf.CheckState = CheckState.Unchecked;
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("txtInterval_EditValueChanged " + ex.Message);
            }
        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            QueryClick();
        }

        private void fnSearchCancel(string paramType, string paramId, string paramUserId)
        {
            try
            {
                SP_GMES0413 cProc = new SP_GMES0413();
                DataTable dtData = null;

                dtData = cProc.SetParamData(dtData, paramType, paramId);
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];

                    if (dtData.Rows.Count > 0)
                    {
                        Cancel cncl = new Cancel(dtData, paramUserId, this);
                        cncl.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnSearchCancel " + ex.Message);
            }
        }

        public void fnCanceled(string paramType, string paramId, string paramMemo)
        {
            try
            {
                if (this.SetYesNoMessageBox("Are you sure?", "Cancel Data", IconType.Warning) == DialogResult.Yes)
                {
                    SP_GMES0413 cProc = new SP_GMES0413("S");
                    DataTable dtData = null;

                    dtData = cProc.SetParamDataInsert(dtData,
                                                      paramType, //ACTION
                                                      paramId, // RENTAL_DATE ==> PINJEM VARIABELNYA UNTUK PARAMETER RENT_NO
                                                      paramMemo, //RENT_TIME ==> PINJEM VARIABELNYA UNTUK PARAMETER DRIVER
                                                      "", //RENT_DIV ==> PINJEM VARIABELNYA UNTUK PARAMETER CAR
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "", //ACTIVITIY_CD
                                                      "",
                                                      "", //PLACE_DESC
                                                      "",
                                                      "",
                                                      "", //USE_DESC
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "C", //STATUS
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      SessionInfo.UserID,   //UPDATER
                                                      DateTime.Now.ToString("yyyyMMdd HHmmss"), //UPDATE_DT
                                                      Dns.GetHostName() + "|" + getIpAddress() + "|" + GetMacAddress() //UPDATE_PC
                                                      );

                    if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                    {
                        MessageBoxW("Canceled Succeed");
                        QueryClick();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnCanceled " + ex.Message);
            }
        }

        //private void simpleButton1_Click(object sender, EventArgs e)
        //{
        //    //fnSentMail("2025-02-12", "08:05:09", "kwangsik.sun", "kwangsik.sun", "202400040", "040000", "INFORMATION TECHNOLOGY", "2025-02-12 17:30", "2025-02-12 23:30", "CKP", "Official (Dinas)", "Lee, SC / Kim, hu");
        //}

        private void fnGetCboPopDest(string paramType)
        {
            try
            {
                SP_GMES0413 cProc = new SP_GMES0413();
                DataTable dtData = null;
                dtData = cProc.SetParamData(dtData, paramType, "", "", "");
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtDest = rs.ResultDataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnGetCboPopDest " + ex.Message);
            }
        }

        private void fnGetCboPopNumber(string paramType)
        {
            try
            {
                SP_GMES0413 cProc = new SP_GMES0413();
                DataTable dtData = null;
                dtData = cProc.SetParamData(dtData, paramType, "", "", "");
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtNumber = rs.ResultDataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnGetCboPopNumber " + ex.Message);
            }
        }

        private void fnGetCarStockInfo(string paramType, string paramDate)
        {
            try
            {
                SP_GMES0413 cProc = new SP_GMES0413();
                DataTable dtData = null;

                dtData = cProc.SetParamData(dtData, paramType, paramDate);
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    DataTable dt = rs.ResultDataSet.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        lblRToday.Text = dt.Rows[0][1].ToString();
                        lblRBooked.Text = dt.Rows[0][2].ToString();
                        lblRStandby.Text = dt.Rows[0][3].ToString();

                        lblRTomorrow.Text = dt.Rows[1][1].ToString();
                        lblRBookedT.Text = dt.Rows[1][2].ToString();
                        lblRStandbyT.Text = dt.Rows[1][3].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnGetCarStockInfo " + ex.Message);
            }
        }

        private void gvwWaitingList_RowClick(object sender, RowClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 2)
                {
                    int rowFocused = gvwWaitingList.FocusedRowHandle;
                    string col = gvwWaitingList.Columns[0].FieldName;
                    string cekId = gvwWaitingList.GetRowCellValue(rowFocused, col).ToString();
                    string userId = SessionInfo.UserID;

                    fnSearchById("GET_DATA_ID", cekId, userId);

                    QueryClick();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("gvwWaitingList_RowClick " + ex.Message);
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPageIndex == 0)
                {
                    dtFrom.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
                    dtTo.EditValue = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    fnSearchW("GET_WAITING_LIST", dtFrom.DateTime.ToString("yyyyMMdd"), dtTo.DateTime.ToString("yyyyMMdd"), "");
                }
                else
                {
                    dtFrom.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
                    dtTo.EditValue = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    fnSearch("GET_DATA", dtEFrom.DateTime.ToString("yyyyMMdd"), dtETo.DateTime.ToString("yyyyMMdd"), cboStatus.EditValue.ToString());
                }

                fnGetCarStockInfo("CAR_STOCK_INFO", DateTime.Now.ToString("yyyyMMdd"));
            }
            catch (Exception ex)
            {
                MessageBoxW("xtraTabControl1_SelectedPageChanged " + ex.Message);
            }
        }

        private int fnCheckCarStock(string paramType, string paramDeparture)
        {
            int cnt = 0;
            try
            {
                SP_GMES0413 cProc = new SP_GMES0413();
                DataTable dtData = null;

                dtData = cProc.SetParamData(dtData, paramType, paramDeparture);
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    DataTable dt = rs.ResultDataSet.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        cnt = dt.Rows[0][0].pxToInt();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnCheckCarStock " + ex.Message);
            }
            return cnt;
        }

        static int ConvertDayToNumber(DateTime date)
        {
            // DayOfWeek mengembalikan 0 (Sunday) hingga 6 (Saturday)
            return ((int)date.DayOfWeek + 1); // Ubah agar Minggu = 1, Senin = 2, dst.
        }

        public bool fnDeleteCancel(string paramType, string paramID)
        {
            bool isSucced = false;

            try
            {
                if (this.SetYesNoMessageBox("Are you sure?", "Delete This Data", IconType.Warning) == DialogResult.Yes)
                {
                    pbProgressShow();

                    SP_GMES0413 cProc = new SP_GMES0413("S");
                    DataTable dtData = null;

                    dtData = cProc.SetParamDataInsert(dtData,
                                                      "DELETE_CANCEL", //ACTION
                                                      paramID, //RENTAL_DATE
                                                      "", //RENTAL_TIME
                                                      "GA", //DIV
                                                      "3110", //PLANT
                                                      "", //USER ID
                                                      "", // USER NAME
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "", //ACTIVITIY_CD
                                                      "",
                                                      "", //PLACE_DESC
                                                      "",
                                                      "", // MEMBER OF EMPLOYEE
                                                      "", //USE_DESC
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "", //RENTAL_STATUS
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      SessionInfo.UserID,   //UPDATER
                                                      DateTime.Now.ToString("yyyyMMdd HHmmss"), //UPDATE_DT
                                                      Dns.GetHostName() + "|" + getIpAddress() + "|" + GetMacAddress() //UPDATE_PC
                                                      );

                    if (dtData != null)
                    {
                        if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                        {
                            isSucced = true;
                            QueryClick();
                        }
                        else
                        {
                            MessageBoxW("Failed Saved");
                        }
                    }

                    pbProgressHide();
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fnDeleteCancel " + ex.Message);
            }

            return isSucced;
        }

        public string fnGetSession()
        {
            string id = "";

            try
            {
                id = SessionInfo.UserID;
            }
            catch (Exception ex)
            {
                MessageBoxW("fnGetSession " + ex.Message);
            }

            return id;
        }

        public string getMessYn(string userId)
        {
            try
            {
                SP_GMES0413 cProc = new SP_GMES0413();
                DataTable dtData = null;

                dtData = cProc.SetParamData(dtData, "GET_MESS_YN", userId);
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    DataTable dt = rs.ResultDataSet.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        string messYn = dt.Rows[0][0].ToString();
                        return messYn;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
