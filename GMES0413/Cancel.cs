using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSI.MES.P
{
    public partial class Cancel : Form
    {
        GMES0413 _frm;
        bool opn = true;
        int tick = 0;

        public Cancel(DataTable dtData, string userId, GMES0413 frm)
        {
            InitializeComponent();

            _frm = frm;

            try
            {
                if (dtData.Rows.Count > 0)
                {
                    txtRegId.Text = dtData.Rows[0][0].ToString();
                    txtUserNm.Text = dtData.Rows[0][1].ToString();
                    mmoReason.Text = dtData.Rows[0][2].ToString();
                    string status = dtData.Rows[0][3].ToString();

                    txtRegId.ReadOnly = true;
                    txtUserNm.ReadOnly = true;
                    if (status == "C")
                    {
                        mmoReason.ReadOnly = true;
                        btnCancel.Enabled = false;
                    }

                    fnDesign();

                    if (userId != dtData.Rows[0][4].ToString())
                    {
                        btnCancel.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cancel " + ex.Message);
            }
        }

        private void fnDesign()
        {
            try
            {
                lblRegId.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblDash5.Font = new Font("Calibri", 11, FontStyle.Bold);
                lblDestination.Font = new Font("Calibri", 11, FontStyle.Bold);

                txtRegId.Font = new Font("Calibri", 11, FontStyle.Bold);
                txtUserNm.Font = new Font("Calibri", 11, FontStyle.Bold);
                mmoReason.Font = new Font("Calibri", 11, FontStyle.Bold);

                btnCancel.Image = Properties.Resources.cancelled;
            }
            catch (Exception ex)
            {
                MessageBox.Show("fnDesign: " + ex.Message);
            }
        }

        private void pctSave_MouseDown(object sender, MouseEventArgs e)
        {
            btnCancel.Image = Properties.Resources.cancelledClick;
        }

        private void pctSave_MouseUp(object sender, MouseEventArgs e)
        {
            btnCancel.Image = Properties.Resources.cancelled;
        }

        private void pctSave_Click(object sender, EventArgs e)
        {
            try
            {
                string regId = txtRegId.Text;
                string memo = mmoReason.Text;

                if (regId != "" && memo != "")
                {
                    _frm.fnCanceled("SET_CANCEL", regId, memo);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("pctSave_Click " + ex.Message);
            }
        }

    }
}
