﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymJournal.App.Models
{
	public class UserInfo
	{
		public Guid Id { get; set; }
		public Guid Token { get; set; }
		public string Name { get; set; }
		public string Role { get; set; }
	}
}