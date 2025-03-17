using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using System.Globalization;
using System.Threading;

namespace CSI.MES.P
{
    public partial class Register : Form
    {
        GMES0413 _frm;
        bool opn = true;
        int tick = 0;

        public Register(DataTable dtData, DataTable dtItem, DataTable dtDest, DataTable dtNumber, GMES0413 frm)
        {
            InitializeComponent();

            _frm = frm;

            try
            {
                fnDesign();
                txtRentDt.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtRentTime.Text = DateTime.Now.ToString("HH:mm:ss");
                txtUserId.Text = dtData.Rows[0][1].ToString();
                txtUserNm.Text = dtData.Rows[1][1].ToString();
                txtEmpid.Text = dtData.Rows[2][1].ToString();
                txtEmpNm.Text = dtData.Rows[3][1].ToString();
                
                dtEStart.EditValue = DateTime.Now.AddDays(1).Date.AddHours(08).AddMinutes(30).ToString("yyyy-MM-dd HH:mm");
                dtEStart.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
                dtEStart.Properties.ShowPopupShadow = true;
                dtEStart.Properties.ShowClear = false;
                dtEStart.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
                dtEStart.Properties.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;
                // Ubah font pada kalender popup
                dtEStart.Properties.AppearanceCalendar.Header.Font = new Font("Calibri", 12, FontStyle.Bold);
                dtEStart.Properties.AppearanceCalendar.DayCell.Font = new Font("Calibri", 10);
                dtEStart.Properties.AppearanceCalendar.WeekNumber.Font = new Font("Calibri", 10);

                dtEEnd.EditValue = DateTime.Now.AddDays(1).Date.AddHours(15).AddMinutes(30).ToString("yyyy-MM-dd HH:mm");
                dtEEnd.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
                dtEEnd.Properties.ShowPopupShadow = true;
                dtEEnd.Properties.ShowClear = false;
                dtEEnd.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
                dtEEnd.Properties.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;

                dtEEnd.Properties.AppearanceCalendar.Header.Font = new Font("Calibri", 12, FontStyle.Bold);
                dtEEnd.Properties.AppearanceCalendar.DayCell.Font = new Font("Calibri", 10);
                dtEEnd.Properties.AppearanceCalendar.WeekNumber.Font = new Font("Calibri", 10);


                txtNumber.Text = "1";
                txtNumber.Visible = false;
                cboNumber.Properties.DataSource = dtNumber;
                cboNumber.Properties.DisplayMember = "NAME";
                cboNumber.Properties.ValueMember = "CODE";
                cboNumber.EditValue = dtNumber.Rows[0]["CODE"];
                lblUnit.Text = "Min 3 Persons";
                cboDept.Properties.DataSource = dtItem;
                cboDept.Properties.DisplayMember = "NAME";
                cboDept.Properties.ValueMember = "CODE";
                string dept = dtData.Rows[4][1].ToString().Substring(0, 6);
                cboDept.EditValue = dept;
                cboDestination.Properties.DataSource = dtDest;
                cboDestination.Properties.ValueMember = "CODE";
                cboDestination.Properties.DisplayMember = "NAME";
                chkSentMail.CheckState = CheckState.Checked;

                //READ ONLY
                txtRentDt.ReadOnly = true;
                txtRentTime.ReadOnly = true;
                txtUserId.ReadOnly = true;
                txtUserNm.ReadOnly = true;
                txtEmpid.ReadOnly = false;
                txtEmpNm.ReadOnly = true;
                cboDestination.Enabled = false;
                string messYn = dtData.Rows[5][1].ToString();
                if (messYn == "N")
                {
                    chkUnofficial.Enabled = false;
                }
                else
                {
                    chkUnofficial.Enabled = true;
                }

                string today = DateTime.Now.ToString("dddd").ToUpper().Trim();
                if (today.Contains("SATURDAY"))
                {
                    lblNote.Text = "Note: Max Request Before 9 AM";
                }
                else if (today.Contains("SUNDAY"))
                {
                    lblNote.Text = "Note: Cannot Register for Today (Sunday)";
                }
                else if (today.Contains("MONDAY"))
                {
                    lblNote.Text = "Note: Max Request Before 2 PM for the Next Day";
                }
                else
                {
                    lblNote.Text = "Note: Max Request Before 2 PM";
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
                lblRentDt.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblUserId.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblEmpid.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblDept.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblUsageDate.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblMemo.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblPurposes.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblPassenger.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblDash1.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblDash2.Font = new Font("Calibri", 11, FontStyle.Bold);
                //lblDash3.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblDash4.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblNum.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblUnit.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblDestination.Font = new Font("Calibri", 11, FontStyle.Bold);

                txtRentDt.Font = new Font("Calibri", 11, FontStyle.Bold);
                txtRentTime.Font = new Font("Calibri", 11, FontStyle.Bold);
                txtUserId.Font = new Font("Calibri", 11, FontStyle.Bold);
                txtUserNm.Font = new Font("Calibri", 11, FontStyle.Bold);
                txtEmpid.Font = new Font("Calibri", 11, FontStyle.Bold);
                txtEmpNm.Font = new Font("Calibri", 11, FontStyle.Bold);
                //txtDeptCd.Font = new Font("Calibri", 11, FontStyle.Bold);
                //txtDeptNm.Font = new Font("Calibri", 11, FontStyle.Bold);
                dtEStart.Font = new Font("Calibri", 11, FontStyle.Bold);
                dtEEnd.Font = new Font("Calibri", 11, FontStyle.Bold);
                mmoPassenger.Font = new Font("Calibri", 11, FontStyle.Bold);
                chkOfficial.Font = new Font("Calibri", 11, FontStyle.Bold);
                chkUnofficial.Font = new Font("Calibri", 11, FontStyle.Bold);
                mmoMemo.Font = new Font("Calibri", 11, FontStyle.Bold);
                txtNumber.Font = new Font("Calibri", 11, FontStyle.Bold);
                txtNumber.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                cboNumber.Font = new Font("Calibri", 11, FontStyle.Bold);
                chkSentMail.Font = new Font("Calibri", 11, FontStyle.Bold);
                cboDept.Font = new Font("Calibri", 11, FontStyle.Bold);
                cboDestination.Font = new Font("Calibri", 11, FontStyle.Bold);

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
                    //cboDestination.Properties.DataSource = null;
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
            pctSave.Image = Properties.Resources.save;
            pctSave.BackColor = Color.LightGray;
        }

        private void pctSave_MouseUp(object sender, MouseEventArgs e)
        {
            pctSave.Image = Properties.Resources.save;
            pctSave.BackColor = Color.Transparent;
        }

        private void pctSave_Click(object sender, EventArgs e)
        {
            try
            {
                string RENTAL_DATE = txtRentDt.Text.Replace("-", "");
                string RENTAL_TIME = txtRentTime.Text.Replace(":", "");
                string USER_ID = txtUserId.Text;
                string USER_NAME = txtUserNm.Text;
                string EMP_ID = txtEmpid.Text;
                string DEPT_CODE = cboDept.EditValue.ToString(); //txtDeptCd.Text;
                string DEPT_NAME = cboDept.Text; //txtDeptNm.Text;
                string PURPOSES = (chkOfficial.CheckState == CheckState.Checked && chkUnofficial.CheckState == CheckState.Unchecked ? "Y" : chkOfficial.CheckState == CheckState.Unchecked && chkUnofficial.CheckState == CheckState.Checked ? "N" : "");
                string MEMO = mmoMemo.Text;
                string PASSANGER = mmoPassenger.Text;
                string START_DATE = dtEStart.DateTime.ToString("yyyyMMdd HHmm");
                string END_DATE = dtEEnd.DateTime.ToString("yyyyMMdd HHmm");
                string MEMBERS = cboNumber.EditValue.ToString(); //txtNumber.Text;
                string IS_SEND_MAIL = chkSentMail.CheckState == CheckState.Checked ? "Y" : "N";
                string DESTINATION = PURPOSES == "" ? "" : cboDestination.EditValue.ToString();

                bool isSaved = _frm.fnSave(RENTAL_DATE, RENTAL_TIME, USER_ID, USER_NAME, EMP_ID, DEPT_CODE, DEPT_NAME, START_DATE, END_DATE, DESTINATION, PURPOSES, PASSANGER, MEMBERS, IS_SEND_MAIL, txtEmpNm.Text, MEMO);

                if (isSaved)
                {
                    this.Close();
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
                    string messYn = _frm.getMessYn(txtEmpid.Text);
                    if (messYn == "N")
                    {
                        chkUnofficial.Enabled = false;
                    }
                    else
                    {
                        chkUnofficial.Enabled = true;
                    }
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
