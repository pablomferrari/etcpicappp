using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using UIKit;
using QuickLook;
using EtcPicApp.Data;
using ETLAppInternal.iOS;
using Foundation;

[assembly: Dependency(typeof(FileSaveIOs))]
namespace ETLAppInternal.iOS
{
    public class FileSaveIOs : IFileSave
    {
        //Method to save document as a file in iOS and view the saved document.
        public async Task SaveAndView(string filename, string contentType, MemoryStream stream)
        {
            //Get the root path of iOS device.
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, filename);

            //Create a file and write the stream into it.
            FileStream fileStream = File.Open(filePath, FileMode.Create);
            stream.Position = 0;
            stream.CopyTo(fileStream);

            fileStream.Flush();
            fileStream.Close();

            //Launch the saved file for viewing in default viewer.
            UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (currentController.PresentedViewController != null)
                currentController = currentController.PresentedViewController;
            UIView currentView = currentController.View;

            QLPreviewController preview = new QLPreviewController();
            QLPreviewItem item = new QLPreviewItemBundle(filename, filePath);
            preview.DataSource = new PreviewControllerDS(item);

            currentController.PresentViewController(preview, true, null);
        }

	}

	public class PreviewControllerDS : QLPreviewControllerDataSource
	{
		private QLPreviewItem _item;

		public PreviewControllerDS(QLPreviewItem item)
		{
			_item = item;
		}

		public override nint PreviewItemCount(QLPreviewController controller)
		{
			return (nint)1;
		}

		public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
		{
			return _item;
		}
	}

	public class QLPreviewItemFileSystem : QLPreviewItem
	{
		string _fileName, _filePath;

		public QLPreviewItemFileSystem(string fileName, string filePath)
		{
			_fileName = fileName;
			_filePath = filePath;
		}

		public override string ItemTitle
		{
			get
			{
				return _fileName;
			}
		}
		public override NSUrl ItemUrl
		{
			get
			{
				return NSUrl.FromFilename(_filePath);
			}
		}
	}

	public class QLPreviewItemBundle : QLPreviewItem
	{
		string _fileName, _filePath;
		public QLPreviewItemBundle(string fileName, string filePath)
		{
			_fileName = fileName;
			_filePath = filePath;
		}

		public override string ItemTitle
		{
			get
			{
				return _fileName;
			}
		}
		public override NSUrl ItemUrl
		{
			get
			{
				var documents = NSBundle.MainBundle.BundlePath;
				var lib = Path.Combine(documents, _filePath);
				var url = NSUrl.FromFilename(lib);
				return url;
			}
		}
	}
}