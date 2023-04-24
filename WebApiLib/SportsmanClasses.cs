using System.Collections.Generic;

namespace WebApiLib
{
	public class Position
	{
		public string name { get; set; }
		public string description { get; set; }
		public int id { get; set; }
		public string created { get; set; }
	}

	public class Region
	{
		public string code { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public int id { get; set; }
		public string created { get; set; }
	}

	public class Address
	{
		public string addressText { get; set; }
		public string kladrCode { get; set; }
		public string index { get; set; }
		public Region region { get; set; }
		public int id { get; set; }
		public string created { get; set; }
	}

	public class Organization
	{
		public string site { get; set; }
		public string branch { get; set; }
		public List<Address> addresses { get; set; }
		public string type { get; set; }
		public string status { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public int id { get; set; }
		public string created { get; set; }
	}

	public class Coach
	{
		public Position position { get; set; }
		public List<Organization> organizations { get; set; }
		public string fname { get; set; }
		public string lname { get; set; }
		public string patronymic { get; set; }
		public string birthdate { get; set; }
		public Address address { get; set; }
		public int id { get; set; }
		public string created { get; set; }
	}

	public class SportsmanInfo
	{
		public List<Coach> coaches { get; set; }
		public List<Organization> organizations { get; set; }
		public string fname { get; set; }
		public string lname { get; set; }
		public string patronymic { get; set; }
		public string birthdate { get; set; }
		public Address address { get; set; }
		public int id { get; set; }
		public string created { get; set; }
	}
}