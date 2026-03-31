using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FPSCookingPrototype.Gameplay.Items;
using FPSCookingPrototype.Gameplay.Player;
using FPSCookingPrototype.Gameplay.Stations;

namespace FPSCookingPrototype.Gameplay.Interfaces
{
	public interface IInteractable
	{
	
		bool IsActive
		{
			get;
		}
		InteractionType Interaction
		{
			get;
		}

		void OnHoverEnter();
		void OnHoverExit();
	}
}
