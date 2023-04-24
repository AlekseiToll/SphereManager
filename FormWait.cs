using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SphereManager
{
	public partial class FormWait : Form
	{
		public FormWait()
		{
			InitializeComponent();
		}

		public void CloseWithResultOk()
		{
			this.DialogResult = DialogResult.OK;
		}
	}
}
