using TMPro;

using UnityEngine;

namespace FPSCookingPrototype.UI.Tasks
{


	public class StationTaskItemUI:MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _text;

		public void SetText( string value )
		{
			if( _text != null )
				_text.text = value;
		}

		public void SetColor( Color color )
		{
			if( _text != null )
				_text.color = color;
		}
	}


}
