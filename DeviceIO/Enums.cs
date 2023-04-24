namespace DeviceIO
{
	internal enum MARKER_ANSWER
	{
		COMMAND_OK = 0,
		COMMAND_FAILED = 1
	}

	internal enum TEST_EVENT
	{
		SPORTSMAN_REACH_LAP_FINISH = 0,
		LAP_TIME_IS_UP = 1
	}

	public enum MARKER_LIGHT_COLOR
	{
		GREEN,
		RED,
		BLUE,
		YELLOW
	}

	/// <summary>Commands from marker</summary>
	internal enum COMMANDS_MARKER
	{
		NODE_INFO = 1,
		CONFIRM = 2,
		BAD_COMMAND = 3,
		OPTIONS = 4,
	}

	/// <summary>Commands from PC to marker</summary>
	internal enum COMMANDS_PC
	{
		WRITE_EEPROM = 1,
		Kp = 2,
		GET_OPTIONS = 3,
		Ki = 4,
		CLEAR = 5,
		CALIBR_GYRO = 6,
		CALIBR_ACC = 7,
		DIRECT_PWM = 8,
		DIRECT_PWM_OFF = 9,
		PWM1_DIR = 10,
		PWM2_DIR = 11,
		//MOTOR1_DIR1 = 12,
		//MOTOR1_DIR2 = 13,
		//MOTOR2_DIR1 = 14,
		//MOTOR2_DIR2 = 15,
		KpRot = 16,
		KiRot = 17,
		KGYRO = 18,
		//SLEEP = 19,
		SET_ADDR = 20,
		//SET_SPEED = 21,
		//SET_SPEED_ROT = 22,
		//SET_ANGLE_ROT = 23,
		HORIZONT = 24,
		KeyUpDown = 29, // sloping
		KeyDownDown = 30, // sloping
		KeyUpUp = 34, // cancel sloping
		KeyDownUp = 35, // cancel sloping
		KeyLeftDown = 31, // turning
		KeyRightDown = 32, // turning
		KeyLeftUp = 36, // stop turning
		KeyRightUp = 37, // stop turning
		LedRed_On = 38,
		LedRed_Off = 39,
		LedYellow_On = 40,
		LedYellow_Off = 41,
		LedGreen_On = 42,
		LedGreen_Off = 43,
		LedBlue_On = 44,
		LedBlue_Off = 45
	}
}