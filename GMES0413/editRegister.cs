using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace CSI.MES.P
{
    public partial class editRegister : Form
    {
        GMES0413 _frm;
        bool opn = true;
        int tick = 0;

        public editRegister(DataTable dtData, DataTable dtItem, DataTable dtDest, string userId, DataTable dtNumber, GMES0413 frm)
        {
            InitializeComponent();

            _frm = frm;

            try
            {
                fnDesign();

                txtRentID.Text = dtData.Rows[0][0].ToString();
                txtRentDt.Text = dtData.Rows[0][11].ToString();
                txtRentTime.Text = dtData.Rows[0][12].ToString();
                txtUserId.Text = dtData.Rows[0][1].ToString();
                txtUserNm.Text = dtData.Rows[0][2].ToString();
                txtEmpid.Text = dtData.Rows[0][3].ToString();
                txtEmpNm.Text = _frm.getName(dtData.Rows[0][3].ToString());

                cboDept.Properties.DataSource = dtItem;
                cboDept.Properties.DisplayMember = "NAME";
                cboDept.Properties.ValueMember = "CODE";
                string dept = dtData.Rows[0][4].ToString();
                cboDept.EditValue = dept;
                cboDestination.Properties.DataSource = dtDest;
                cboDestination.Properties.ValueMember = "CODE";
                cboDestination.Properties.DisplayMember = "NAME";
                cboDestination.EditValue = dtData.Rows[0][8].ToString();

                mmoMemo.Text = dtData.Rows[0][14].ToString();
                chkOfficial.CheckState = dtData.Rows[0][9].ToString() == "Y" ? CheckState.Checked : CheckState.Unchecked;
                chkUnofficial.CheckState = dtData.Rows[0][9].ToString() == "N" ? CheckState.Checked : CheckState.Unchecked;
                mmoPassenger.Text = dtData.Rows[0][10].ToString();

                dtEStart.EditValue = dtData.Rows[0][5].ToString();
                dtEEnd.EditValue = dtData.Rows[0][6].ToString();

                dtEStart.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
                dtEStart.Properties.ShowPopupShadow = true;
                dtEStart.Properties.ShowClear = false;
                dtEStart.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
                dtEStart.Properties.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;
                // Ubah font pada kalender popup
                dtEStart.Properties.AppearanceCalendar.Header.Font = new Font("Calibri", 12, FontStyle.Bold);
                dtEStart.Properties.AppearanceCalendar.DayCell.Font = new Font("Calibri", 10);
                dtEStart.Properties.AppearanceCalendar.WeekNumber.Font = new Font("Calibri", 10);

                dtEEnd.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
                dtEEnd.Properties.ShowPopupShadow = true;
                dtEEnd.Properties.ShowClear = false;
                dtEEnd.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
                dtEEnd.Properties.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;

                dtEEnd.Properties.AppearanceCalendar.Header.Font = new Font("Calibri", 12, FontStyle.Bold);
                dtEEnd.Properties.AppearanceCalendar.DayCell.Font = new Font("Calibri", 10);
                dtEEnd.Properties.AppearanceCalendar.WeekNumber.Font = new Font("Calibri", 10);
                chkSentMail.CheckState = CheckState.Checked;

                //txtNumber.Text = dtData.Rows[0][7].ToString();
                txtNumber.Visible = false;
                cboNumber.Properties.DataSource = dtNumber;
                cboNumber.Properties.DisplayMember = "NAME";
                cboNumber.Properties.ValueMember = "CODE";
                cboNumber.EditValue = dtData.Rows[0][7].ToString();
                lblUnit.Text = "Min 3 Persons";


                ////READ ONLY
                DateTime startDate = dtEStart.EditValue != null ? Convert.ToDateTime(dtEStart.EditValue) : DateTime.MinValue;

                if (dtData.Rows[0][13].ToString() == "R" && startDate > DateTime.Now || dtData.Rows[0][13].ToString() == "W" && startDate > DateTime.Now)
                {
                    txtRentID.ReadOnly = true;
                    txtRentDt.ReadOnly = true;
                    txtRentTime.ReadOnly = true;
                    txtUserId.ReadOnly = true;
                    txtUserNm.ReadOnly = true;
                    txtEmpid.ReadOnly = false;
                    txtEmpNm.ReadOnly = true;
                    //chkSentMail.Enabled = false;
                }
                else
                {
                    txtRentID.ReadOnly = true;
                    txtRentDt.ReadOnly = true;
                    txtRentTime.ReadOnly = true;
                    txtUserId.ReadOnly = true;
                    txtUserNm.ReadOnly = true;
                    txtEmpid.ReadOnly = true;
                    txtEmpNm.ReadOnly = true;
                    cboDept.ReadOnly = true;
                    dtEStart.ReadOnly = true;
                    dtEEnd.ReadOnly = true;
                    mmoMemo.ReadOnly = true;
                    txtNumber.ReadOnly = true;
                    cboNumber.ReadOnly = true;
                    mmoPassenger.ReadOnly = true;
                    cboDestination.ReadOnly = true;
                    
                    chkOfficial.Enabled = false;
                    chkUnofficial.Enabled = false;
                    chkSentMail.Enabled = false;
                    pctSave.Enabled = false;
                }

                if (userId != dtData.Rows[0][1].ToString())
                {
                    //pctSave.Enabled = false;
                    txtEmpid.ReadOnly = true;
                    cboDept.ReadOnly = true;
                    dtEStart.ReadOnly = true;
                    dtEEnd.ReadOnly = true;
                    chkOfficial.Enabled = false;
                    chkUnofficial.Enabled = false;
                    cboDestination.ReadOnly = true;
                    mmoMemo.ReadOnly = true;
                    //txtNumber.ReadOnly = true;
                    //mmoPassenger.ReadOnly = true;
                    //chkSentMail.Enabled = false;

                }

                lblStatus.Text = dtData.Rows[0][13].ToString();
                lblStatus.Visible = false;
                if (lblStatus.Text == "C" && txtUserId.Text.Trim().ToLower() == _frm.fnGetSession().Trim().ToLower())
                {
                    pctSave.Image = Properties.Resources.delete_file;
                    pctSave.Enabled = true;
                }

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Reserve " + ex.Message);
            }
        }

        private void fnDesign()
        {
            try
            {
                lblRentId.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblRentDt.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblUserId.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblEmpid.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblDept.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblUsageDate.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblDestination.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblPurposes.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblPassenger.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblDash1.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblDash2.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblDash4.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblNum.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblUnit.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblDestination.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblMemo.Font = new Font("Calibri", 11, FontStyle.Bold);

                txtRentID.Font = new Font("Calibri", 11, FontStyle.Bold);
                txtRentDt.Font = new Font("Calibri", 11, FontStyle.Bold);
                txtRentTime.Font = new Font("Calibri", 11, FontStyle.Bold);
                txtUserId.Font = new Font("Calibri", 11, FontStyle.Bold);
                txtUserNm.Font = new Font("Calibri", 11, FontStyle.Bold);
                txtEmpid.Font = new Font("Calibri", 11, FontStyle.Bold);
                txtEmpNm.Font = new Font("Calibri", 11, FontStyle.Bold);

                dtEStart.Font = new Font("Calibri", 11, FontStyle.Bold);
                dtEEnd.Font = new Font("Calibri", 11, FontStyle.Bold);
                mmoPassenger.Font = new Font("Calibri", 11, FontStyle.Bold);
                chkOfficial.Font = new Font("Calibri", 11, FontStyle.Bold);
                chkUnofficial.Font = new Font("Calibri", 11, FontStyle.Bold);
                mmoMemo.Font = new Font("Calibri", 11, FontStyle.Bold);
                txtNumber.Font = new Font("Calibri", 11, FontStyle.Bold);
                txtNumber.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                chkSentMail.Font = new Font("Calibri", 11, FontStyle.Bold);
                cboDept.Font = new Font("Calibri", 11, FontStyle.Bold);
                cboDestination.Font = new Font("Calibri", 11, FontStyle.Bold);
                cboNumber.Font = new Font("Calibri", 11, FontStyle.Bold);

                //pctSave.Image = Properties.Resources.simpan;

                pctSave.Image = Properties.Resources.save;
            }
            catch (Exception ex)
            {
                MessageBox.Show("fnDesign: " + ex.Message);
            }
        }

        private void chkOfficial_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkOfficial.CheckState == CheckState.Checked)
                {
                    chkUnofficial.CheckState = CheckState.Unchecked;
                    cboDestination.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("chkOfficial_CheckedChanged " + ex.Message);
            }
        }

        private void chkUnofficial_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkUnofficial.CheckState == CheckState.Checked)
                {
                    chkOfficial.CheckState = CheckState.Unchecked;
                    DataTable dt = (DataTable)cboDestination.Properties.DataSource;
                    if (dt.Rows.Count > 0)
                    {
                        cboDestination.EditValue = dt.Rows[0]["CODE"];
                    }
                    cboDestination.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("chkUnofficial_CheckedChanged " + ex.Message);
            }
        }

        private void pctSave_MouseDown(object sender, MouseEventArgs e)
        {
            if (lblStatus.Text == "C")
            {
                pctSave.Image = Properties.Resources.delete_file;
                pctSave.BackColor = Color.LightGray;
            }
            else
            {
                pctSave.Image = Properties.Resources.save;
                pctSave.BackColor = Color.LightGray;
            }
        }

        private void pctSave_MouseUp(object sender, MouseEventArgs e)
        {
            if (lblStatus.Text == "C")
            {
                pctSave.Image = Properties.Resources.delete_file;
                pctSave.BackColor = Color.LightGray;
            }
            else
            {
                pctSave.Image = Properties.Resources.save;
                pctSave.BackColor = Color.Transparent;
            }
        }

        private void pctSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblStatus.Text != "C")
                {
                    string EMPID, DEPT_CD, DEPT_NM, START_DT, END_DT, DESTINATION, PURPOSE, MEMBERS, PASSANGERS, RENT_ID, EMP_NAME, MEMO, RENT_DATE, RENT_TIME, IS_SENT_MAIL, USER_NM;
                    EMPID = txtEmpid.Text;
                    DEPT_CD = cboDept.EditValue.ToString();
                    DEPT_NM = cboDept.Text;
                    START_DT = dtEStart.DateTime.ToString("yyyyMMdd HHmm");
                    END_DT = dtEEnd.DateTime.ToString("yyyyMMdd HHmm");
                    MEMO = mmoMemo.Text;
                    PURPOSE = (chkOfficial.CheckState == CheckState.Checked && chkUnofficial.CheckState == CheckState.Unchecked ? "Y" : chkOfficial.CheckState == CheckState.Unchecked && chkUnofficial.CheckState == CheckState.Checked ? "N" : "");
                    MEMBERS = cboNumber.EditValue.ToString(); //txtNumber.Text;
                    PASSANGERS = mmoPassenger.Text;
                    RENT_ID = txtRentID.Text;
                    EMP_NAME = txtEmpNm.Text;
                    DESTINATION = cboDestination.EditValue.ToString();
                    RENT_DATE = txtRentDt.Text.Replace("-", "");
                    RENT_TIME = txtRentTime.Text.Replace(":", "");
                    IS_SENT_MAIL = chkSentMail.CheckState == CheckState.Checked ? "Y" : "N";
                    USER_NM = txtUserNm.Text;


                    bool isSaved = _frm.fnEdit(RENT_ID, EMPID, DEPT_CD, DEPT_NM, START_DT, END_DT, DESTINATION, PURPOSE, MEMBERS, PASSANGERS, EMP_NAME, RENT_DATE, RENT_TIME, MEMO, IS_SENT_MAIL, USER_NM);

                    if (isSaved)
                    {
                        this.Close();
                    }
                }
                else
                {
                    string RENT_ID = txtRentID.Text;

                    bool isSucceed = _frm.fnDeleteCancel("DELETE_CANCEL", RENT_ID);

                    if (isSucceed)
                    {
                        this.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("pctSave_Click " + ex.Message);
            }
        }

        private void dtEStart_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtEStart.DateTime > dtEEnd.DateTime)
                {
                    dtEEnd.DateTime = dtEStart.DateTime.Date.AddHours(15).AddMinutes(30);
                }
                else if (dtEStart.DateTime < DateTime.Now)
                {
                    dtEStart.DateTime = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("dtEStart_EditValueChanged " + ex.Message);
            }
        }

        private void dtEEnd_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtEStart.DateTime > dtEEnd.DateTime)
                {
                    dtEStart.DateTime = dtEEnd.DateTime.Date.AddHours(08).AddMinutes(30);
                }
                else if (dtEEnd.DateTime < DateTime.Now)
                {
                    dtEEnd.DateTime = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("dtEEnd_EditValueChanged " + ex.Message);
            }
        }

        private void txtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("txtNumber_KeyPress " + ex.Message);
            }
        }

        private void txtEmpid_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtEmpid.Text.Length > 7)
                {
                    txtEmpNm.Text = _frm.getName(txtEmpid.Text);
                    string dept = _frm.getDept(txtEmpid.Text).Length > 0 ? _frm.getDept(txtEmpid.Text).Substring(0, 6) : "010020";
                    cboDept.EditValue = dept;
                }
                else
                {
                    txtEmpNm.Text = "";
                    cboDept.EditValue = "010020";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("txtEmpid_EditValueChanged " + ex.Message);
            }
        }


    }
}
