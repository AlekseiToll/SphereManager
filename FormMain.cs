using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

using AMQPlib;
using DeviceIO;
using SphServiceLib;
using XMLparserLib;

namespace SphereManager
{
	public partial class FormMain : Form
	{
		private SphSettings settings_;

		private int selectedMarkerId_ = -1;

		private ManagerCore managerCore_;
		private BackgroundWorker bwMoveMarkersToStartPosition_;
		private BackgroundWorker bwExecuteTest_;

		FormWait frmWait_ = new FormWait();

		// поток, принимающий пакеты из очереди AMQP
		private AMQPlistenThread amqpListen_;
		private Thread amqpListenThread_;

		public FormMain()
		{
			InitializeComponent();
		}

		private void btnMoveMarker_Click(object sender, EventArgs e)
		{
			if (selectedMarkerId_ == -1)
			{
				MessageBox.Show("Необходимо выбрать маркер в списке!");
				return;
			}

			try
			{
				int x, y;
				managerCore_.GetMarkerCoordinates(selectedMarkerId_, out x, out y);
				if (x == -1 && y == -1)
				{
					MessageBox.Show("Маркер невозможно переместить, т.к. неизвестна его текущая позиция. Не получены данные от системы позиционирования.");
					return;
				}

				if (!managerCore_.CalcNewCoordinates(selectedMarkerId_, 
										(float)numDistance.Value,
										(int) numAngle.Value, out x, out y))
				{
					MessageBox.Show("Не удалось рассчитать координаты!");
					return;
				}

				if (!managerCore_.Move(selectedMarkerId_, x, y))
				{
					MessageBox.Show("Ошибка передачи данных маркеру!");
					return;
				}
				MessageBox.Show("Маркер успешно перемещен");
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in btnMoveMarker_Click():");
				throw;
			}
		}

		private void btnDeleteMarker_Click(object sender, EventArgs e)
		{
			try
			{
				if (selectedMarkerId_ == -1)
				{
					MessageBox.Show("Необходимо выбрать маркер в списке!");
					return;
				}

				if (!managerCore_.DeleteMarker(selectedMarkerId_))
				{
					SphService.WriteToLogFailed("Unable to delete marker: " +
						selectedMarkerId_);
					return;
				}

				// delete from ListView
				for (int iItem = 0; iItem < lvMarkerList.Items.Count; ++iItem)
				{
					if (lvMarkerList.Items[iItem].Text == selectedMarkerId_.ToString())
					{
						lvMarkerList.Items.RemoveAt(iItem);
						--iItem;
						SphService.WriteToLogGeneral("Marker was deleted " +
							selectedMarkerId_);
					}
				}
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in btnDeleteMarker_Click():");
				MessageBox.Show("Не удается удалить маркер!");
				//throw;
			}
		}

		private void connector_AddMarker(int markerId, IPAddress ip)
		{
			try
			{
				if (this.InvokeRequired == false) // thread checking
				{
					ListViewItem itemNewMarker = new ListViewItem(markerId.ToString());
					//itemNewMarker.SubItems.Add("name");
					//itemNewMarker.SubItems.Add("coord");
					itemNewMarker.SubItems.Add(ip.ToString());
					lvMarkerList.Items.Add(itemNewMarker);
				}
				else
				{
					ManagerCore.AddMarkerHandler handler =
						new ManagerCore.AddMarkerHandler(connector_AddMarker);
					this.Invoke(handler, new object[] { markerId, ip });
				}
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in connector_AddMarker():");
			}
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			try
			{
				SphService.Init();

				if (File.Exists(SphService.logGeneralName))
					File.Delete(SphService.logGeneralName);
				if (File.Exists(SphService.logFailedName))
					File.Delete(SphService.logFailedName);

				settings_ = new SphSettings();
				settings_.LoadSettings();

				// start the thread receiving packets from RabbitMQ
				amqpListen_ = new AMQPlistenThread(ref settings_);
				amqpListen_.OnAMQPpacketReceived +=
					new AMQPlistenThread.AMQPpacketReceivedHandler(frmMain_OnAMQPpacketReceived);
				amqpListenThread_ = new Thread(new ThreadStart(amqpListen_.Run));
				//string locale = settings_.CurrentLanguage.Equals("ru") ? "ru-RU" : "en-US";
				//amqpListenThread_.CurrentCulture =
				//amqpListenThread_.CurrentUICulture = new System.Globalization.CultureInfo(locale, false);
				amqpListenThread_.Start();

				// start the thread receiving packets from markers
				managerCore_ = new ManagerCore(ref settings_);
				managerCore_.OnAddMarker += connector_AddMarker;
				managerCore_.OnCloseWaitForm += Core_CloseWaitForm;
				managerCore_.StartListenMarkers();
				// and from positioning system
				managerCore_.StartListenXY();

				// by default we use "test XML" mode
				scMain.Panel1.Enabled = false;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in FormMain_Load():");
				throw;
			}
		}

		void frmMain_OnAMQPpacketReceived(AMQPpacket packet)
		{
			try
			{
				if (this.InvokeRequired == false) // thread checking
				{
					managerCore_.ProcessNewAMQPpacket(ref packet);
				}
				else
				{
					AMQPlistenThread.AMQPpacketReceivedHandler packetReceived =
							frmMain_OnAMQPpacketReceived;
					this.Invoke(packetReceived, new object[] { packet });
				}
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in frmMain_OnAMQPpacketReceived():");
			}
		}

		private void lvMarkerList_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				gbMoveMarker.Enabled = lvMarkerList.SelectedItems.Count > 0;
				btnDeleteMarker.Enabled = lvMarkerList.SelectedItems.Count > 0;

				if (lvMarkerList.SelectedItems.Count > 0)
				{
					selectedMarkerId_ = Int32.Parse(lvMarkerList.SelectedItems[0].Text);
				}
				else selectedMarkerId_ = -1;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in lvMarkerList_SelectedIndexChanged():");
				selectedMarkerId_ = -1;
			}
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			managerCore_.StopListening();
			amqpListen_.StopThread();
		}

		private void miModeService_Click(object sender, EventArgs e)
		{
			miModeTestXML.Checked = !miModeService.Checked;
			scMain.Panel1.Enabled = !miModeTestXML.Checked;
			scMain.Panel2.Enabled = !miModeService.Checked;
		}

		private void miModeTestXML_Click(object sender, EventArgs e)
		{
			miModeService.Checked = !miModeTestXML.Checked;
			scMain.Panel1.Enabled = !miModeTestXML.Checked;
			scMain.Panel2.Enabled = !miModeService.Checked;
		}

		private void btnOpenTestFile_Click(object sender, EventArgs e)
		{
			try
			{
				btnStartTest.Enabled = false;
				btnStopTest.Enabled = false;

				// open file
				if (!string.IsNullOrEmpty(settings_.XmlPath))
				{
					openFileDlgXml.InitialDirectory = settings_.XmlPath;
				}
				if(openFileDlgXml.ShowDialog() != DialogResult.OK)
					return;
				string path = openFileDlgXml.FileName;
				int indexSlash = path.LastIndexOf('/');
				if (indexSlash == -1) indexSlash = path.LastIndexOf('\\');
				if (indexSlash != -1)
				{
					path = path.Substring(0, indexSlash);
					settings_.XmlPath = path;
					settings_.SaveSettings();
				}
				else
				{
					SphService.WriteToLogFailed("Error while XML path storing: " + openFileDlgXml.FileName);
				}

				// parse the test
				MarkerTest test;
				if (!XMLparser.Parse(openFileDlgXml.FileName, out test))
				{
					MessageBox.Show("Ошибка при анализе теста!");
					return;
				}

				if (test.GetAllMarkersCount() > lvMarkerList.Items.Count)
				{
					MessageBox.Show(string.Format("Обнаруженных маркеров не хватит для выполнения теста!\n (Обнаружено {0} маркеров, необходимо {1})", lvMarkerList.Items.Count, test.GetAllMarkersCount()));
					return;
				}

				managerCore_.SetTest(ref test);
				btnStartTest.Enabled = true;
				btnStopTest.Enabled = true;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in btnOpenTestFile_Click():");
				throw;
			}
		}

		private void btnStartTest_Click(object sender, EventArgs e)
		{
			try
			{
				// send commands to markers to move them to start positions
				bwMoveMarkersToStartPosition_ = new BackgroundWorker();
				bwMoveMarkersToStartPosition_.WorkerSupportsCancellation = true;
				bwMoveMarkersToStartPosition_.DoWork += bwMoveMarkersToStartPosition_DoWork;
				bwMoveMarkersToStartPosition_.RunWorkerCompleted += 
					bwMoveMarkersToStartPosition_RunWorkerCompleted;
				bwMoveMarkersToStartPosition_.RunWorkerAsync();
				
				if (frmWait_.ShowDialog() == DialogResult.Cancel)
				{
					managerCore_.BreakTest();
					MessageBox.Show("Выполнение теста остановлено");
				}
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in btnStartTest_Click():");
				throw;
			}
		}

		private void bwMoveMarkersToStartPosition_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				managerCore_.MoveMarkersToStartPositions(ref e);
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Exception in bwMoveMarkersToStartPosition_DoWork():");
				//throw;  unhandled exception
			}
		}

		private void bwMoveMarkersToStartPosition_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				if (e.Cancelled) return;

				bool res = false;
				if (e.Result != null) res = (bool)e.Result;
				if (!res)
				{
					MessageBox.Show("Произошла ошибка при расстановке маркеров. Попробуйте запустить тест заново");
					return;
				}
	
				MessageBox.Show(
					"Маркеры успешно расставлены. Убедитесь, что спортсмен занял нужную позицию и нажмите 'ОК'");
				StartExecuteTest();
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, 
					"Exception in bwMoveMarkersToStartPosition_RunWorkerCompleted():");
				//throw;  unhandled exception
			}
		}

		private void StartExecuteTest()
		{
			try
			{
				bwExecuteTest_ = new BackgroundWorker();
				bwExecuteTest_.WorkerSupportsCancellation = true;
				bwExecuteTest_.DoWork += bwExecuteTest_DoWork;
				bwExecuteTest_.RunWorkerCompleted += bwExecuteTest_RunWorkerCompleted;
				bwExecuteTest_.RunWorkerAsync();
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Exception in StartExecuteTest():");
				throw;
			}
		}

		private void bwExecuteTest_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				managerCore_.ExecuteTest(ref e);
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Exception in bwExecuteTest_DoWork():");
				//throw;  unhandled exception
			}
		}

		private void bwExecuteTest_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				if (e.Cancelled) return;

				if (managerCore_.GetTestType() == MarkerTestType.CYCLE)
				{
					MessageBox.Show(string.Format(
						"Тест завершен, спорстмен выполнил {0} циклов", (int) e.Result));
				}
				else if (managerCore_.GetTestType() == MarkerTestType.POINT)
				{
					ResultPointToPointTest res = (ResultPointToPointTest)e.Result;
					MessageBox.Show(string.Format(
						"Тест завершен, спорстмен прошел дистанцию за {0}", 
						res.DistanceTime));
				}
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Exception in bwExecuteTest_RunWorkerCompleted():");
				//throw;  unhandled exception
			}
		}

		private void CloseWaitForm()
		{
			frmWait_.CloseWithResultOk();
		}

		private void Core_CloseWaitForm()
		{
			if (InvokeRequired == false) // thread checking
			{
				CloseWaitForm();	
			}
			else
			{
				ManagerCore.CloseWaitFormHandler handler = Core_CloseWaitForm;
				Invoke(handler, null);
			}
		}

		private void btnStopTest_Click(object sender, EventArgs e)
		{
			managerCore_.BreakTest();
		}

		private void miSettings_Click(object sender, EventArgs e)
		{
			FormSettings frm = new FormSettings(ref settings_);
			if(frm.ShowDialog() == DialogResult.OK)
				settings_.LoadSettings();
		}
	}
}
