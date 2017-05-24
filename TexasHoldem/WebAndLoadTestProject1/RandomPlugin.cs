using Microsoft.VisualStudio.TestTools.WebTesting;
using System;

namespace WebAndLoadTestProject1
{
[System.ComponentModel.DisplayName("Generate random roomName")]
	[System.ComponentModel.Description("Generates a new random roomName")]
	public class UniqueIdentifierPlugin : WebTestPlugin
	{
		[System.ComponentModel.DisplayName("Target Context Parameter Name")]
		[System.ComponentModel.Description("Name of the context parameter that will receive the generated value.")]
		public string ContextParamTarget { get; set; }

		[System.ComponentModel.DisplayName("roomName Length")]
		[System.ComponentModel.Description("Length of the generated roomName.")]
		public int RoomNameLength { get; set; }

		public override void PreWebTest(object sender, PreWebTestEventArgs e)
		{
			// Generate new roomName with specified output format
			var newRoomName = "room";
			var r = new Random();
			r.Next(1000, 9999);
			newRoomName += r;

			// Set the context paramaeter with generated roomName
			e.WebTest.Context[ContextParamTarget] = newRoomName;

			base.PreWebTest(sender, e);
		}
	}
}
