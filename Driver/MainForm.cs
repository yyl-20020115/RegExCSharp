using RegularExpression;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Driver
{
	public partial class MainForm : Form
	{

		RegEx m_regEx = new RegEx();
		bool m_bFirstTime = true;

		public MainForm()
		{
			InitializeComponent();
		}

		private void btnCompile_Click(object sender, EventArgs e)
		{
			if (txtRegEx.Text.Length == 0)
			{
				MessageBox.Show("You must enter a regular expression before compiling.");
				txtRegEx.Select();
				return;
			}
			StringBuilder sb = new StringBuilder();
			try
			{
				ErrorCode errCode = m_regEx.Compile(this.txtRegEx.Text, sb);
				if (errCode != ErrorCode.ERR_SUCCESS)
				{
					string sErrSubstring = txtRegEx.Text.Substring(m_regEx.LastErrorPosition, m_regEx.LastErrorLength);
					string sFormat = "Error occurred during compilation.\nCode: {0}\nAt: {1}\nSubstring: {2}";
					sFormat = String.Format(sFormat, errCode.ToString(), m_regEx.LastErrorPosition, sErrSubstring);
					txtRegEx.Select(m_regEx.LastErrorPosition, m_regEx.LastErrorLength);
					MessageBox.Show(sFormat);
					txtRegEx.Select();
					return;
				}
				this.matchDS.Clear();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error occurred during compilation.\n\n" + ex.Message);
				txtRegEx.Select();
				return;
			}
			txtStat.Text = sb.ToString();
			m_bFirstTime = true;
		}

		private void txtStat_KeyDown(object sender, KeyEventArgs e)
		{
			m_bFirstTime = true;
		}

		private void txtStat_MouseDown(object sender, MouseEventArgs e)
		{
			m_bFirstTime = true;
		}

		private void btnFindAll_Click(object sender, EventArgs e)
		{
			if (!m_regEx.IsReady)
			{
				MessageBox.Show("You must first compile a regular expression.");
				txtRegEx.Focus();
				return;
			}

			matchDS.Clear();
			matchDS.AcceptChanges();

			int nFoundStart = -1;
			int nFoundEnd = -1;
			int nStartAt = 0;
			int nMatchLength = -1;

			do
			{
				bool bFound = m_regEx.FindMatch(txtSearchString.Text, nStartAt, txtSearchString.Text.Length - 1, ref nFoundStart, ref nFoundEnd);
				if (bFound == true)
				{
					string sSubstring = "{Empty String}";
					nMatchLength = nFoundEnd - nFoundStart + 1;
					if (nMatchLength > 0)
					{
						sSubstring = txtSearchString.Text.Substring(nFoundStart, nMatchLength);
					}
					matchDS.MatchInfo.AddMatchInfoRow(sSubstring, nFoundStart, nFoundEnd, nMatchLength);
					matchDS.AcceptChanges();
					nStartAt = nFoundEnd + 1;
				}
				else
				{
					break;
				}
			} while (nStartAt < txtSearchString.Text.Length);
		}

		private void btnFindNext_Click(object sender, EventArgs e)
		{
			if (!m_regEx.IsReady)
			{
				MessageBox.Show("You must first compile a regular expression.");
				txtRegEx.Focus();
				return;
			}
			int nFoundStart = -1;
			int nFoundEnd = -1;
			int nStartAt = -1;

			if (m_bFirstTime)
			{
				nStartAt = txtSearchString.SelectionStart;
				m_bFirstTime = false;
			}
			else
			{
				nStartAt = txtSearchString.SelectionStart + 1;
			}

			bool bFound = m_regEx.FindMatch(txtSearchString.Text, nStartAt, txtSearchString.Text.Length - 1, ref nFoundStart, ref nFoundEnd);
			if (bFound)
			{
				int nMatchLength = nFoundEnd - nFoundStart + 1;
				if (nMatchLength == 0)
				{
					MessageBox.Show("Matched an empty string at position " + nFoundStart.ToString() + ".");
				}
				else
				{
					txtSearchString.Select(nFoundStart, nMatchLength);
					txtSearchString.ScrollToCaret();
				}
			}
			else
			{
				MessageBox.Show("No match found.");
			}
		}

		private void grdResult_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				DataRowView drv = (DataRowView)grdResult.Rows[e.RowIndex].DataBoundItem;
				MatchInfoDS.MatchInfoRow r = (MatchInfoDS.MatchInfoRow)drv.Row;

				txtSearchString.Select(r.StartIndex, r.Length);
				txtSearchString.ScrollToCaret();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private void txtSearchString_KeyDown(object sender, KeyEventArgs e)
		{
			m_bFirstTime = true;
		}

		private void txtSearchString_MouseDown(object sender, MouseEventArgs e)
		{
			m_bFirstTime = true;
		}


		private void txtSearchString_Enter(object sender, EventArgs e)
		{
			this.AcceptButton = btnFindAll;

		}

		private void txtRegEx_Enter(object sender, EventArgs e)
		{
			this.AcceptButton = btnCompile;

		}

		private void chkGreedy_CheckedChanged(object sender, EventArgs e)
		{
			m_regEx.UseGreedy = chkGreedy.Checked;

		}

		private void btnFindFirst_Click(object sender, EventArgs e)
		{
			if (!m_regEx.IsReady)
			{
				MessageBox.Show("You must first compile a regular expression.");
				txtRegEx.Focus();
				return;
			}
			int nFoundStart = -1;
			int nFoundEnd = -1;

			bool bFound = m_regEx.FindMatch(txtSearchString.Text, 0, txtSearchString.Text.Length - 1, ref nFoundStart, ref nFoundEnd);
			if (bFound)
			{
				int nMatchLength = nFoundEnd - nFoundStart + 1;
				if (nMatchLength == 0)
				{
					MessageBox.Show("Matched an empty string at position " + nFoundStart.ToString() + ".");
				}
				else
				{
					txtSearchString.Select(nFoundStart, nMatchLength);
					txtSearchString.ScrollToCaret();
				}
			}
			else
			{
				MessageBox.Show("No match found.");
			}
		}

		private void TestMatch()
		{
			RegEx re = new RegEx();
			string sPattern = "a_*p";
			ErrorCode errCode = re.Compile(sPattern);

			if (errCode != ErrorCode.ERR_SUCCESS)
			{
				string sErrSubstring = sPattern.Substring(re.LastErrorPosition, re.LastErrorLength);
				string sFormat = "Error occurred during compilation.\nCode: {0}\nAt: {1}\nSubstring: {2}";
				sFormat = String.Format(sFormat, errCode.ToString(), m_regEx.LastErrorPosition, sErrSubstring);
				MessageBox.Show(sFormat);
				return;
			}

			int nFoundStart = -1;
			int nFoundEnd = -1;
			string sToSearch = "appleandpotato";

			bool bFound = m_regEx.FindMatch(sToSearch, 0, sToSearch.Length - 1, ref nFoundStart, ref nFoundEnd);
			if (bFound)
			{
				int nMatchLength = nFoundEnd - nFoundStart + 1;
				if (nMatchLength == 0)
				{
					MessageBox.Show("Matched an empty string at position " + nFoundStart.ToString() + ".");
				}
				else
				{
					string sMatchString = sToSearch.Substring(nFoundStart, nMatchLength);

					MessageBox.Show("Match found at: " + nFoundStart.ToString() + "\n" + sMatchString);
				}
			}
			else
			{
				MessageBox.Show("No match found.");
			}
		}
	}
}