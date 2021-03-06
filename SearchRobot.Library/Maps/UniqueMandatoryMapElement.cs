﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SearchRobot.Library.Maps
{
	public abstract class UniqueMandatoryMapElement : MapElement
	{
		protected UniqueMandatoryMapElement(Map map) : base(map)
		{ }

		protected UniqueMandatoryMapElement()
		{ }

		protected bool IsUnique()
		{
			return Map.Elements.Count(e => e.GetType() == GetType()) <= 1;
		}

		public override bool IsValid()
		{
            return base.IsValid();
			//return IsUnique() && base.IsValid();
		}
	}
}
