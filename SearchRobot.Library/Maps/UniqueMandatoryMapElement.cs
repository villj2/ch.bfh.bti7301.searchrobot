using System;
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
			return Map.Elements.All(e => e.GetType() != GetType());
		}

		public override bool IsValid()
		{
			return IsUnique() && base.IsValid();
		}
	}
}
