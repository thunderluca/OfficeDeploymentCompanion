namespace OfficeDeploymentCompanion.Models
{
    public static class Products
    {
        public static Product[] AvailableDictionary = new[]
        {
            new Product { Name = "Access", Id = "Access" },
            new Product { Name = "Excel", Id = "Excel" },
            new Product { Name = "OneDrive for Business", Id = "Groove" },
            new Product { Name = "Skype for Business", Id = "Lync" },
            new Product { Name = "OneDrive", Id = "OneDrive" },
            new Product { Name = "OneNote", Id = "OneNote" },
            new Product { Name = "Outlook", Id = "Outlook" },
            new Product { Name = "PowerPoint", Id = "PowerPoint" },
            new Product { Name = "Publisher", Id = "Publisher" },
            new Product { Name = "Word", Id = "Work" }
        };

        public class Product
        {
            public string Name { get; set; }

            public string Id { get; set; }
        }
    }
}
