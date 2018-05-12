using System;
using System.Collections.Generic;
using System.Text;
using Documents.iOS.Models;

namespace Documents.iOS.Managers
{
    public class LicenseManager
    {
        public IEnumerable<LicenseDetails> GetLicenseDetails()
        {
            var data = new List<LicenseDetails>();

            data.Add(new LicenseDetails()
            {
                License = System.IO.File.ReadAllText(System.IO.Path.Combine(Foundation.NSBundle.MainBundle.BundlePath, "Licenses", "SharpCompress.txt")),
                Title = "SharpCompress",
                Uri = new Uri("https://github.com/adamhathcock/sharpcompress")
            });

			data.Add(new LicenseDetails()
			{
				License = System.IO.File.ReadAllText(System.IO.Path.Combine(Foundation.NSBundle.MainBundle.BundlePath, "Licenses", "XamarinEssentials.txt")),
				Title = "Xamarin Essentials",
				Uri = new Uri("https://github.com/xamarin/Essentials")
			});

            return data;
        }
    }
}
