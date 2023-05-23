namespace ZaculeuValley.IxchelAdmin.Models
{
    public class ControllerListViewModel
    {
        public string Name { get; set; }
        public string ApiUrl { get; set; }

        

        public string DisplayedName
        {
            get
            {
                switch (Name)
                {
                    case "InstitutionCountries":
                        return "Countries";
                    case "InstitutionAreas":
                        return "Areas";
                    case "InstitutionDistricts":
                        return "Districts";
                    default:
                        return Name.Replace("Controller", "");
                }
            }
        }


        //    private static readonly Dictionary<string, string> DisplayedNameMap = new Dictionary<string, string>
        //{
        //    { "InstitutionAreas", "Areas" },
        //    { "InstitutionCountries", "Countries" },
        //    { "InstitutionDistricts", "Districts" }
        //};

        //    public string DisplayedName
        //    {
        //        get
        //        {
        //            if (DisplayedNameMap.ContainsKey(Name))
        //                return DisplayedNameMap[Name];

        //            return Name.Replace("Controller", "");
        //        }
        //    }

        //public string DisplayedName
        //{
        //    get
        //    {
        //        if (Name == "InstitutionAreas")
        //            return "Areas";
        //        else if (Name == "InstitutionCountries")
        //            return "Countries";
        //        else if (Name == "InstitutionDistricts")
        //            return "Districts";
        //        else
        //            return Name.Replace("Controller", "");
        //    }
        //}
    }
}
