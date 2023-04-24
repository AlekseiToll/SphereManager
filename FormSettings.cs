using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SphServiceLib;

namespace SphereManager
{
	public partial class FormSettings : Form
	{
		private SphSettings settings_;
		private bool bNeedToSave_ = false;

		public FormSettings(ref SphSettings settings)
		{
			InitializeComponent();

			settings_ = settings;
		}

		private void FormSettings_Load(object sender, EventArgs e)
		{
			tbPortUdpFromMarkers.Text = settings_.PortUdpFromMarkers.ToString();
			tbPortUdpFromXY.Text = settings_.PortUdpFromPosSystem.ToString();

			tbAmpqHostName.Text = settings_.AmpqHost;
			tbAmpqUser.Text = settings_.AmpqUser;
			tbAmpqPass.Text = settings_.AmpqPswd;
			tbAmpqQueue.Text = settings_.AmpqQueueName;

			tbTimeoutMoving.Text = settings_.TimeoutForMoving.ToString();

			tbInaccMarker.Text = settings_.InnaccuracyMarkerPos.ToString();
			tbInaccSportsman.Text = settings_.InnaccuracySportsmanPos.ToString();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			bNeedToSave_ = true;
		}

		private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (bNeedToSave_)
				{
					settings_.AmpqHost = tbAmpqHostName.Text;
					settings_.AmpqPswd = tbAmpqPass.Text;
					settings_.AmpqUser = tbAmpqUser.Text;
					settings_.AmpqQueueName = tbAmpqQueue.Text;

					settings_.PortUdpFromMarkers = Int32.Parse(tbPortUdpFromMarkers.Text);
					settings_.PortUdpFromPosSystem = Int32.Parse(tbPortUdpFromXY.Text);

					settings_.TimeoutForMoving = Int32.Parse(tbTimeoutMoving.Text);

					settings_.InnaccuracyMarkerPos = Int32.Parse(tbInaccMarker.Text);
					settings_.InnaccuracySportsmanPos = Int32.Parse(tbInaccSportsman.Text);

					settings_.SaveSettings();
				}
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in FormSettings_FormClosing():");
				MessageBox.Show(this, "Введены некорректные данные!");
				e.Cancel = true;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			bNeedToSave_ = false;
		}

		private void OnlyDigit_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == '\b');
		}
	}
}
