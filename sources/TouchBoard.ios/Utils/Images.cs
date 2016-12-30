using System;
using UIKit;
using Foundation;

namespace KeyboardCompanion
{
	public static class Images
	{
		public static string ImagesFolder = Util.USING_IPHONE ? "iphone/":"ipad/";

		public static UIImage ExportConfigImg = UIImage.FromBundle("Main-Export");
		public static UIImage ImportConfigImg = UIImage.FromBundle("Main-Import");
		public static UIImage ListConfigsImg = UIImage.FromBundle("Main-Select");
		public static UIImage NewConfigImg = UIImage.FromBundle("Main-Add");
		public static UIImage SearchConfigImg = UIImage.FromFile(ImagesFolder + "home-search-config");
		public static UIImage SettingsImg = UIImage.FromBundle("Main-Settings");
		public static UIImage EditConfigImg = UIImage.FromBundle("Main-Edit");
		public static UIImage EditConfigImgSelected = UIImage.FromFile(ImagesFolder + "home-edit-config-selected");
		public static UIImage RefreshImage = UIImage.FromBundle("Refresh");
	}
}

